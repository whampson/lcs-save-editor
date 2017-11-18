#region License
/* Copyright (c) 2017 Wes Hampson
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using WHampson.Cascara.Types;

namespace WHampson.Cascara
{
    internal sealed class TemplateProcessor
    {
        private const string IdentifierPattern = @"^[a-zA-Z_][\da-zA-Z_]*$";
        private const string ValueofOpPattern = @"\${(.+?)}";
        private const string OffsetofOpPattern = @"\$\[([\[\]\S]+)\]";
        private const string SizeofOpPattern = @"\$\((.+?)\)";
        private const string TypeOpPattern = @"type[ ]+(.+)";

        private delegate int DirectiveProcessAction(XElement elem);
        private delegate T AttributeProcessAction<T>(XAttribute attr);

        //private XDocument templateDoc;

        private IntPtr dataPtr;
        private int dataLen;
        private int dataOffset;

        private Dictionary<string, TypeDefinition> typeMap;
        private Dictionary<string, DirectiveProcessAction> directiveActionMap;
        private Dictionary<string, Delegate> attributeActionMap;
        private Dictionary<string, double> localsMap;

        private Stack<SymbolTable> symTablStack;

        // TODO: Change into Stack<TemplateFile> after implemented
        private Stack<string> pathStack;

        private bool isEvaluatingUnion;
        private bool isEvalutingTypedef;
        private bool isConductingDryRun;    // Analyzing a struct and computing size, but not applying to binary data
        private int dryRunRecursionDepth;

        private TextWriter echoWriter;

        public TemplateProcessor(/*XDocument doc*/)
        {
            //templateDoc = doc ?? throw new ArgumentNullException("doc");

            dataPtr = IntPtr.Zero;
            dataLen = 0;
            dataOffset = 0;

            typeMap = new Dictionary<string, TypeDefinition>();
            directiveActionMap = new Dictionary<string, DirectiveProcessAction>();
            attributeActionMap = new Dictionary<string, Delegate>();
            localsMap = new Dictionary<string, double>();

            symTablStack = new Stack<SymbolTable>();

            pathStack = new Stack<string>();

            isEvalutingTypedef = false;
            isConductingDryRun = false;
            dryRunRecursionDepth = 0;

            echoWriter = Console.Out;

            BuildTypeMap();
            BuildDirectiveActionMap();
            BuildAttributeActionMap();
        }

        public TextWriter EchoWriter
        {
            get { return echoWriter; }
            set { echoWriter = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        public SymbolTable Process(string templatePath, IntPtr dataPtr, int dataLen)
        {
            this.dataPtr = dataPtr;
            this.dataLen = dataLen;

            symTablStack.Push(new SymbolTable());
            ProcessTemplate(templatePath);
            return symTablStack.Pop();
        }

        // TODO: create TemplateFile class and pass that here
        private int ProcessTemplate(string templatePath)
        {
            isEvalutingTypedef = false;
            isConductingDryRun = false;
            dryRunRecursionDepth = 0;
            dataOffset = 0;

            // Prevent 'include' cycles
            if (pathStack.Contains(templatePath))
            {
                throw new TemplateException("Template inclusion cycle detected.");
            }

            pathStack.Push(Path.GetFullPath(templatePath));
            XDocument doc = OpenXmlFile(templatePath);

            // Validate root element
            if (doc.Root.Name.LocalName != Keywords.TemplateRoot)
            {
                string fmt = "Template must have a root element named '{0}'.";
                throw TemplateException.Create(doc.Root, fmt, Keywords.TemplateRoot);
            }
            if (!HasChildren(doc.Root))
            {
                throw new TemplateException("Empty binary file template.");
            }

            int bytesProcessed = ProcessStructMembers(doc.Root);
            pathStack.Pop();

            return bytesProcessed;
        }

        private int ProcessElement(XElement elem)
        {
            string elemName = elem.Name.LocalName;
            int localOffset = 0;

            // Process 'struct'
            if (elemName == Keywords.Struct)
            {
                localOffset += ProcessStruct(elem);
                return localOffset;
            }

            // Process 'union'
            if (elemName == Keywords.Union)
            {
                localOffset += ProcessUnion(elem);
                return localOffset;
            }

            // Ensure element name corresponds to either a primitive type,
            // user-defined type, or directive
            bool isType = typeMap.TryGetValue(elemName, out TypeDefinition typeDef);
            bool isDirective = directiveActionMap.ContainsKey(elemName);
            if (!isDirective && !isType)
            {
                string fmt = "Unknown type or directive '{0}'.";
                throw TemplateException.Create(elem, fmt, elemName);
            }

            // Process directive
            if (isDirective)
            {
                DirectiveProcessAction action = directiveActionMap[elemName];
                return action(elem);
            }

            // Process primitive or user-defined type
            // Ensure element has no child elements (only allowed on 'struct's)
            if (HasChildren(elem))
            {
                string fmt = "Type '{0}' cannot contain child elements.";
                throw TemplateException.Create(elem, fmt, elemName);
            }

            if (typeDef.IsStruct)
            {
                // Process user-defined struct type
                // "Copy and paste" members from type definition into copy of current element
                XElement copy = new XElement(elem);
                copy.Add(typeMap[elemName].Members);
                localOffset += ProcessStruct(copy);
            }
            else
            {
                // Process primitive
                localOffset += ProcessPrimitiveType(elem);
            }

            return localOffset;
        }

        private int ProcessStruct(XElement elem)
        {
            EnsureAttributes(elem, Keywords.Comment, Keywords.Count, Keywords.Name);

            // Get attribute values
            int count = GetAttributeValue<int>(elem, Keywords.Count, false, 1);
            string name = GetAttributeValue<string>(elem, Keywords.Name, false, null);

            // Process the struct 'count' times
            int localOffset = 0;
            int elemSize = 0;
            string varName;
            for (int i = 0; i < count; i++)
            {
                // Tack on array index to var name so it's unique
                varName = name + "[" + i + "]";

                SymbolTableEntry entry = null;
                SymbolTable curSymTabl = symTablStack.Peek();
                if (!isConductingDryRun && name != null)
                {
                    if (localsMap.ContainsKey(name))
                    {
                        string fmt = "Variable '{0}' already defined as a local.";
                        throw TemplateException.Create(elem, fmt, name);
                    }

                    // Create new symbol table and make current table its parent
                    SymbolTable newSymTabl = new SymbolTable(varName, curSymTabl);

                    // Create symbol table entry for this struct in the current table
                    // Type remains 'null' and size is 0 because we haven't processed the struct yet
                    TypeInfo tInfo = new TypeInfo(null, dataOffset, 0, false);
                    entry = new SymbolTableEntry(tInfo, newSymTabl);

                    if (!curSymTabl.AddEntry(varName, entry))
                    {
                        string fmt = "Variable '{0}' already defined.";
                        throw TemplateException.Create(elem, fmt, name);
                    }

                    // Push new symbol table onto the stack to make it "active"
                    symTablStack.Push(newSymTabl);
                }
                else
                {
                    // Push a new, nameless symbol table onto the stack
                    // so children can still reference other members
                    // within the scope of the struct
                    symTablStack.Push(new SymbolTable(null, curSymTabl));
                }

                // Process the struct
                elemSize = ProcessStructMembers(elem);
                localOffset += elemSize;

                if (!isConductingDryRun && name != null)
                {
                    // We've finished processing the struct, so now we can set the size
                    entry.TypeInfo.Size = elemSize;
                    entry.TypeInfo.IsFullyDefined = true;

                    // Make the previous table "active"
                    symTablStack.Pop();
                }
            }

            return localOffset;
        }

        private int ProcessStructMembers(XElement elem)
        {
            // Ensure struct element has child elements
            if (!HasChildren(elem))
            {
                string fmt = "Empty struct.";
                throw TemplateException.Create(elem, fmt);
            }

            // Process child elements
            int localOffset = 0;
            foreach (XElement memberElem in elem.Elements())
            {
                localOffset += ProcessElement(memberElem);
            }

            return localOffset;
        }

        private int ProcessUnion(XElement elem)
        {

            EnsureAttributes(elem, Keywords.Comment, Keywords.Count, Keywords.Name);

            int count = GetAttributeValue<int>(elem, Keywords.Count, false, 1);
            string name = GetAttributeValue<string>(elem, Keywords.Name, false, null);

            int localOffset = 0;
            string varName;
            for (int i = 0; i < count; i++)
            {
                /* ============= TODO: Put into own function ======== */
                // Tack on array index to var name so it's unique
                varName = name + "[" + i + "]";

                SymbolTableEntry entry = null;
                SymbolTable curSymTabl = symTablStack.Peek();
                if (!isConductingDryRun && name != null)
                {
                    if (localsMap.ContainsKey(name))
                    {
                        string fmt = "Variable '{0}' already defined as a local.";
                        throw TemplateException.Create(elem, fmt, name);
                    }

                    // Create new symbol table and make current table its parent
                    SymbolTable newSymTabl = new SymbolTable(varName, curSymTabl);

                    // Create symbol table entry for this struct in the current table
                    // Type remains 'null' and size is 0 because we haven't processed the struct yet
                    TypeInfo tInfo = new TypeInfo(null, dataOffset, 0, false);
                    entry = new SymbolTableEntry(tInfo, newSymTabl);

                    if (!curSymTabl.AddEntry(varName, entry))
                    {
                        string fmt = "Variable '{0}' already defined.";
                        throw TemplateException.Create(elem, fmt, name);
                    }

                    // Push new symbol table onto the stack to make it "active"
                    symTablStack.Push(newSymTabl);
                }
                else
                {
                    // Push a new, nameless symbol table onto the stack
                    // so children can still reference other members
                    // within the scope of the struct
                    symTablStack.Push(new SymbolTable(null, curSymTabl));
                }

                /* ============================================================ */

                int startOffset = dataOffset;
                int bytesRead = 0;
                int size = 0;
                foreach (XElement memberElem in elem.Elements())
                {
                    bytesRead = ProcessElement(memberElem);
                    size = Math.Max(bytesRead, size);
                    dataOffset = startOffset;
                }
                localOffset += size;
                dataOffset += localOffset;

                if (!isConductingDryRun && name != null)
                {
                    // We've finished processing the struct, so now we can set the size
                    entry.TypeInfo.Size = size;
                    entry.TypeInfo.IsFullyDefined = true;

                    // Make the previous table "active"
                    symTablStack.Pop();
                }
            }

            return localOffset;
        }

        private int ProcessPrimitiveType(XElement elem)
        {
            EnsureAttributes(elem, Keywords.Comment, Keywords.Count, Keywords.Name);

            // Get attribute values
            int count = GetAttributeValue<int>(elem, Keywords.Count, false, 1);
            string name = GetAttributeValue<string>(elem, Keywords.Name, false, null);

            string elemName = elem.Name.LocalName;
            TypeDefinition tDef = typeMap[elemName];

            // Process primitive 'count' times
            int localOffset = 0;
            string varName;
            for (int i = 0; i < count; i++)
            {
                // Make sure we have enough bytes left in the buffer
                EnsureCapacity(elem, tDef.Size);

                // Tack on array index to var name so it's unique
                varName = name + "[" + i + "]";

                if (!isConductingDryRun)
                {
                    if (name != null)
                    {
                        if (localsMap.ContainsKey(name))
                        {
                            string fmt = "Variable '{0}' already defined as a local.";
                            throw TemplateException.Create(elem, fmt, name);
                        }

                        // Create symbol table entry for this type
                        // It's not a struct so the child symbol table is 'null'
                        TypeInfo tInfo = new TypeInfo(tDef.Kind, dataOffset, tDef.Size, true);
                        SymbolTableEntry e = new SymbolTableEntry(tInfo, null);
                        if (!symTablStack.Peek().AddEntry(varName, e))
                        {
                            string fmt = "Variable '{0}' already defined.";
                            throw TemplateException.Create(elem, fmt, name);
                        }
                    }

                    // Increment data pointer
                    dataOffset += tDef.Size;
                }
                localOffset += tDef.Size;
            }

            return localOffset;
        }

        private int ProcessAlignDirective(XElement elem)
        {
            EnsureAttributes(elem, Keywords.Comment, Keywords.Count, Keywords.Kind);

            // Get attribute values
            TypeDefinition defaultType = typeMap[Keywords.Int8];
            TypeDefinition kind = GetAttributeValue<TypeDefinition>(elem, Keywords.Kind, false, defaultType);
            int count = GetAttributeValue<int>(elem, Keywords.Count, false, 1);

            // Skip ahead correct number of bytes as defined by 'kind' and 'count'
            int off = kind.Size * count;
            EnsureCapacity(elem, off);

            if (!isConductingDryRun)
            {
                dataOffset += off;
            }
            
            return off;
        }

        private int ProcessEchoDirective(XElement elem)
        {
            if (isEvalutingTypedef)
            {
                return 0;
            }
            if (HasChildren(elem))
            {
                string fmt = "Directive '{0}' cannot contain child elements.";
                throw TemplateException.Create(elem, fmt, Keywords.Echo);
            }

            EnsureAttributes(elem, Keywords.Comment, Keywords.Message, Keywords.Newline, Keywords.Raw);

            string message = GetAttributeValue<string>(elem, Keywords.Message, true, null);
            bool hasNewline = GetAttributeValue<bool>(elem, Keywords.Newline, false, true);
            bool isRaw = GetAttributeValue<bool>(elem, Keywords.Raw, false, false);

            XAttribute messageAttr = elem.Attribute(Keywords.Message);

            if (!isRaw)
            {
                try
                {
                    message = ResolveVariables(message);
                    message = ResolveEscapeSequences(message);
                }
                catch (Exception e)
                {
                    if (e is ArithmeticException || e is OverflowException
                        || e is FormatException || e is TemplateException)
                    {
                        throw TemplateException.Create(e, messageAttr, e.Message);
                    }

                    throw;
                }
            }

            if (hasNewline)
            {
                echoWriter.WriteLine(message);
            }
            else
            {
                echoWriter.Write(message);
            }

            return 0;
        }

        private int ProcessIncludeDirective(XElement elem)
        {
            EnsureAttributes(elem, Keywords.Comment, Keywords.Path);

            string path = GetAttributeValue<string>(elem, Keywords.Path, true, null);

            try
            {
                return ProcessTemplate(path);
            }
            catch (TemplateException ex)
            {
                throw TemplateException.Create(ex, elem, ex.Message);
            }
        }

        private int ProcessLocalDirective(XElement elem)
        {
            if (isEvalutingTypedef)
            {
                return 0;
            }
            if (HasChildren(elem))
            {
                string fmt = "Directive '{0}' cannot contain child elements.";
                throw TemplateException.Create(elem, fmt, Keywords.Echo);
            }

            EnsureAttributes(elem, Keywords.Comment, Keywords.Name, Keywords.Value);

            string name = GetAttributeValue<string>(elem, Keywords.Name, true, null);
            double value = GetAttributeValue<double>(elem, Keywords.Value, true, 0);

            if (symTablStack.Peek().GetEntry(name) != null)
            {
                string fmt = "Variable '{0}' already exists as a non-local variable.";
                throw TemplateException.Create(elem, fmt, name);
            }

            localsMap[name] = value;

            return 0;
        }

        private int ProcessTypedefDirective(XElement elem)
        {
            if (isEvalutingTypedef)
            {
                string fmt = "Nested type definitions are not allowed.";
                throw TemplateException.Create(elem, fmt);
            }
            isEvalutingTypedef = true;

            EnsureAttributes(elem, Keywords.Comment, Keywords.Kind, Keywords.Name);

            string typename = GetAttributeValue<string>(elem, Keywords.Name, true, null);
            if (typeMap.ContainsKey(typename))
            {
                string fmt = "Type '{0}' has already been defined.";
                throw TemplateException.Create(elem, fmt, typename);
            }
            else if (Keywords.ReservedWords.Contains(typename))
            {
                string fmt = "Cannot use reserved word '{0}' as a type name.";
                throw TemplateException.Create(elem, fmt, typename);
            }

            TypeDefinition kind =
                GetAttributeValue<TypeDefinition>(elem, Keywords.Kind, true, null);   // Type analysis happens here

            typeMap[typename] = kind;
            isEvalutingTypedef = false;

            return 0;
        }

        /// <summary>
        /// Makes sure the only attributes present on a given <see cref="XElement"/>
        /// are ones with names matching a list of valid attributes. Also makes sure
        /// that each attribute has a non-whitespace value.
        /// </summary>
        /// <param name="elem">
        /// The <see cref="XElement"/> to check for valid attributes.
        /// </param>
        /// <param name="validAttributes">
        /// An array of valid attribute names.
        /// </param>
        /// <exception cref="TemplateException">
        /// If an attribute whose name does not appear in the list of valid attributes
        /// is present or if the attribute value is empty.
        /// </exception>
        private void EnsureAttributes(XElement elem, params string[] validAttributes)
        {
            foreach (XAttribute attr in elem.Attributes())
            {
                string name = attr.Name.LocalName;
                if (!validAttributes.Contains(name))
                {
                    string fmt = "Unknown attribute '{0}'.";
                    throw TemplateException.Create(attr, fmt, name);
                }
                else if (string.IsNullOrWhiteSpace(attr.Value))
                {
                    string fmt = "Attribute '{0}' cannot have an empty value.";
                    throw TemplateException.Create(attr, fmt, name);
                }
            }
        }

        /// <summary>
        /// Gets the value of the attribute with the specified name
        /// from the specified <see cref="XElement"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value of the attribute.
        /// </typeparam>
        /// <param name="elem">
        /// The element from which to get the attribute.
        /// </param>
        /// <param name="name">
        /// The name of the attribute to get the value of.
        /// </param>
        /// <param name="isRequired">
        /// A value indicating whether this attribute must be present.
        /// </param>
        /// <param name="defaultValue">
        /// The default value to return if the attribute is not present.
        /// </param>
        /// <returns>
        /// The attribute value if present.
        /// </returns>
        /// <exception cref="TemplateException">
        /// Thrown if the attribute is marked as required but not present.
        /// </exception>
        private T GetAttributeValue<T>(XElement elem, string name, bool isRequired, T defaultValue)
        {
            if (!attributeActionMap.ContainsKey(name))
            {
                // Should never happen;
                // name SHOULD HAVE been validated before this method is called
                string msg = string.Format("Unknown attribute '{0}'", name);
                throw new TemplateException(msg);
            }

            XAttribute attr = elem.Attribute(name);
            if (attr == null)
            {
                if (isRequired)
                {
                    string fmt = "Missing required attribute '{0}'.";
                    throw TemplateException.Create(elem, fmt, name);
                }

                return defaultValue;
            }

            // Process the attribute value
            AttributeProcessAction<T> process = (AttributeProcessAction<T>) attributeActionMap[name];
            return process(attr);
        }

        private int ProcessCountAttribute(XAttribute attr)
        {
            try
            {
                string valStr = ResolveVariables(attr.Value);
                double val = NumberUtils.EvaluateExpression(valStr);

                if (val < 0 || !NumberUtils.IsInteger(val))
                {
                    string msg = "Value '{0}' is not a non-negative integer.";
                    throw TemplateException.Create(attr, msg, val);
                }

                return Convert.ToInt32(val);
            }
            catch (Exception e)
            {
                if (e is ArithmeticException ||  e is OverflowException
                    || e is FormatException || e is TemplateException)
                {
                    throw TemplateException.Create(e, attr, e.Message);
                }

                throw;
            }
        }

        private TypeDefinition ProcessKindAttribute(XAttribute attr)
        {
            string typeName = attr.Value;
            XElement srcElem = attr.Parent;

            // Process 'struct'
            if (typeName == Keywords.Struct)
            {
                // Enable 'dry run'
                // We only want to gather the type information,
                // but not apply it to underlying data
                if (dryRunRecursionDepth == 0)
                {
                    isConductingDryRun = true;
                }

                dryRunRecursionDepth++;
                int size = ProcessStructMembers(srcElem);
                dryRunRecursionDepth--;

                // Disable 'dry run'
                if (dryRunRecursionDepth == 0)
                {
                    isConductingDryRun = false;
                }

                return TypeDefinition.CreateStruct(srcElem.Elements(), size);
            }

            // Process primitive or user-defined type
            if (typeMap.ContainsKey(typeName))
            {
                if (HasChildren(srcElem))
                {
                    string fmt = "Type '{0}' cannot contain child elements.";
                    throw TemplateException.Create(attr, fmt, typeName);
                }

                return typeMap[typeName];
            }
            else
            {
                string fmt = "Unknown type '{0}'.";
                throw TemplateException.Create(attr, fmt, typeName);
            }
        }

        private string ProcessMessageAttribute(XAttribute attr)
        {
            return attr.Value;
        }

        private string ProcessNameAttribute(XAttribute attr)
        {
            if (!Regex.IsMatch(attr.Value, IdentifierPattern))
            {
                string fmt = "'{0}' is not a valid identifier. Identifiers may consist only of "
                    + "alphanumeric characters and underscores, and may not begin with a number.";
                throw TemplateException.Create(attr, fmt, attr.Value);
            }

            return attr.Value;
        }

        private bool ProcessNewlineAttribute(XAttribute attr)
        {
            return ProcessBooleanAttribute(attr);
        }

        private string ProcessPathAttribute(XAttribute attr)
        {
            string parent = Path.GetDirectoryName(pathStack.Peek()); 
            return Path.Combine(parent, attr.Value);
        }

        private bool ProcessRawAttribute(XAttribute attr)
        {
            return ProcessBooleanAttribute(attr);
        }

        private double ProcessValueAttribute(XAttribute attr)
        {
            try
            {
                string valStr = ResolveVariables(attr.Value);
                return NumberUtils.EvaluateExpression(valStr);
            }
            catch (Exception e)
            {
                if (e is ArithmeticException || e is OverflowException
                    || e is FormatException || e is TemplateException)
                {
                    throw TemplateException.Create(e, attr, e.Message);
                }

                throw;
            }
        }

        private bool ProcessBooleanAttribute(XAttribute attr)
        {
            if (!bool.TryParse(attr.Value, out bool val))
            {
                string fmt = "'{0}' is not a valid boolean value.";
                throw TemplateException.Create(attr, fmt, attr.Value);
            }

            return val;
        }

        /// <summary>
        /// Replaces all variable references in the given string with their values.
        /// </summary>
        /// <param name="s">
        /// The string on which to resolve variables.
        /// </param>
        /// <returns>
        /// The input string with all variables replaced with their corresponding values.
        /// </returns>
        /// <exception cref="TemplateException">
        /// If an undefined variable is present in the string.
        /// </exception>
        private string ResolveVariables(string s)
        {
            // Resolve values
            s = Regex.Replace(s, ValueofOpPattern, ResolveValueof);
            s = Regex.Replace(s, OffsetofOpPattern, ResolveOffsetof);
            s = Regex.Replace(s, SizeofOpPattern, ResolveSizeof);

            return s;
        }

        /// <summary>
        /// Resolves all instances of the 'valueof' operator in
        /// the given regex <see cref="Match"/>.
        /// </summary>
        private string ResolveValueof(Match m)
        {
            if (isEvalutingTypedef)
            {
                throw new TemplateException("Variables cannot be used when defining a type.");
            }

            string varName = m.Groups[1].Value;

            // Handle special variables
            switch (varName)
            {
                case Keywords.Filesize:
                    return dataLen + "";

                case Keywords.Offset:
                    return dataOffset + "";
            }

            // Handle locals
            if (localsMap.TryGetValue(varName, out double localVal))
            {
                return localVal + "";
            }

            // Handle variables tied to the binary data
            SymbolTableEntry e = GetSymbolInfo(varName);
            if (!e.TypeInfo.IsFullyDefined)
            {
                string msg = string.Format("Variable '{0}' is not yet fully defined.", varName);
                throw new TemplateException(msg);
            }
            else if (e.TypeInfo.IsStruct)
            {
                throw new TemplateException("Cannot take the value of a struct.");
            }

            return GetValue(e.TypeInfo.Type, e.TypeInfo.Offset).ToString();
        }

        /// <summary>
        /// Resolves all instances of the 'offsetof' operator in
        /// the given regex <see cref="Match"/>.
        /// </summary>
        private string ResolveOffsetof(Match m)
        {
            if (isEvalutingTypedef)
            {
                throw new TemplateException("Variables cannot be used when defining a type.");
            }

            string varName = m.Groups[1].Value;

            // Handle locals
            if (localsMap.ContainsKey(varName))
            {
                string msg = "Offset not defined for local variables.";
                throw new TemplateException(msg);
            }

            // Handle variables tied to the binary data
            SymbolTableEntry e = GetSymbolInfo(varName);
            return e.TypeInfo.Offset + "";
        }

        /// <summary>
        /// Resolves all instances of the 'sizeof' operator in
        /// the given regex <see cref="Match"/>.
        /// </summary>
        private string ResolveSizeof(Match m)
        {
            if (isEvalutingTypedef)
            {
                throw new TemplateException("Variables cannot be used when defining a type.");
            }

            string varName = m.Groups[1].Value;

            // Handle locals
            if (localsMap.ContainsKey(varName))
            {
                string msg = "Size not defined for local variables.";
                throw new TemplateException(msg);
            }

            // Handle special 'sizeof type' operator
            string typeName = null;
            Match m2 = Regex.Match(varName, TypeOpPattern);
            if (m2.Success)
            {
                typeName = m2.Groups[1].Value;
            }
            if (!string.IsNullOrWhiteSpace(typeName))
            {
                // Get size of type
                if (!typeMap.TryGetValue(typeName, out TypeDefinition tDef))
                {
                    string msg = string.Format("Invalid type '{0}'", typeName);
                    throw new TemplateException(msg);
                }

                return tDef.Size + "";
            }

            // Get size of variable value
            SymbolTableEntry e = GetSymbolInfo(varName);
            if (!e.TypeInfo.IsFullyDefined)
            {
                string msg = string.Format("Variable '{0}' is not yet fully defined.", varName);
                throw new TemplateException(msg);
            }

            return e.TypeInfo.Size + "";
        }

        /// <summary>
        /// Gets information about the specified symbol from
        /// the active symbol table. An exception is thrown if
        /// the symbol name is undefined.
        /// </summary>
        /// <exception cref="TemplateException">
        /// Thrown if the symbol name is undefined.
        /// </exception>
        private SymbolTableEntry GetSymbolInfo(string name)
        {
            SymbolTable tabl = symTablStack.Peek();
            if (!tabl.ContainsEntry(name))
            {
                string msg = string.Format("Variable '{0}' not defined.", name);
                throw new TemplateException(msg);
            }

            return tabl.GetEntry(name);
        }

        /// <summary>
        /// Gets the value of type <paramref name="t"/> at
        /// the specified <paramref name="offset"/>.
        /// </summary>
        private object GetValue(Type t, int offset)
        {
            // Create pointer to offset and dereference
            Type ptrType = typeof(Pointer<>).MakeGenericType(new Type[] { t });
            object ptrObj = Activator.CreateInstance(ptrType, dataPtr + offset);
            object val = ptrObj.GetType().GetProperty("Value").GetValue(ptrObj);

            return val;
        }

        /// <summary>
        /// Checks to make sure there are at least <paramref name="localOffset"/>
        /// bytes remaining in the file buffer from the current offset.
        /// </summary>
        /// <exception cref="TemplateException">
        /// Thrown if there are not enough bytes left in the buffer.
        /// </exception>
        private void EnsureCapacity(XElement elem, int localOffset)
        {
            int absOffset = dataOffset + localOffset;
            if (absOffset < 0)
            {
                string fmt = "Attempt to read data before the beginning of the file "
                    + "({0} bytes from beginning).";
                throw TemplateException.Create(elem, fmt, absOffset, dataLen);
            }
            else if (absOffset > dataLen)
            {
                string fmt = "Reached end of file. Offset: {0}, length: {1}.";
                throw TemplateException.Create(elem, fmt, absOffset, dataLen);
            }
        }

        /// <summary>
        /// Checks whether the specified <see cref="XElement"/> has child elements.
        /// </summary>
        private bool HasChildren(XElement elem)
        {
            return elem.Elements().Count() != 0;
        }

        /// <summary>
        /// Populates the map of built-in type names to <see cref="TypeInfo"/> definitions.
        /// </summary>
        private void BuildTypeMap()
        {
            typeMap[Keywords.Bool] = TypeDefinition.CreatePrimitive(typeof(bool), 1);
            typeMap[Keywords.Bool8] = TypeDefinition.CreatePrimitive(typeof(bool), 1);
            typeMap[Keywords.Bool16] = TypeDefinition.CreatePrimitive(typeof(bool), 2);
            typeMap[Keywords.Bool32] = TypeDefinition.CreatePrimitive(typeof(bool), 4);
            typeMap[Keywords.Bool64] = TypeDefinition.CreatePrimitive(typeof(bool), 8);
            typeMap[Keywords.Byte] = TypeDefinition.CreatePrimitive(typeof(byte), 1);
            typeMap[Keywords.Char] = TypeDefinition.CreatePrimitive(typeof(char), 1);
            typeMap[Keywords.Char8] = TypeDefinition.CreatePrimitive(typeof(char), 1);
            typeMap[Keywords.Char16] = TypeDefinition.CreatePrimitive(typeof(char), 2);
            typeMap[Keywords.Double] = TypeDefinition.CreatePrimitive(typeof(double), 4);
            typeMap[Keywords.Float] = TypeDefinition.CreatePrimitive(typeof(float), 4);
            typeMap[Keywords.Int] = TypeDefinition.CreatePrimitive(typeof(int), 4);
            typeMap[Keywords.Int8] = TypeDefinition.CreatePrimitive(typeof(sbyte), 1);
            typeMap[Keywords.Int16] = TypeDefinition.CreatePrimitive(typeof(short), 2);
            typeMap[Keywords.Int32] = TypeDefinition.CreatePrimitive(typeof(int), 4);
            typeMap[Keywords.Int64] = TypeDefinition.CreatePrimitive(typeof(long), 8);
            typeMap[Keywords.Long] = TypeDefinition.CreatePrimitive(typeof(long), 8);
            typeMap[Keywords.Short] = TypeDefinition.CreatePrimitive(typeof(short), 2);
            typeMap[Keywords.Single] = TypeDefinition.CreatePrimitive(typeof(float), 4);
            typeMap[Keywords.UInt] = TypeDefinition.CreatePrimitive(typeof(uint), 4);
            typeMap[Keywords.UInt8] = TypeDefinition.CreatePrimitive(typeof(byte), 1);
            typeMap[Keywords.UInt16] = TypeDefinition.CreatePrimitive(typeof(ushort), 2);
            typeMap[Keywords.UInt32] = TypeDefinition.CreatePrimitive(typeof(uint), 4);
            typeMap[Keywords.UInt64] = TypeDefinition.CreatePrimitive(typeof(ulong), 8);
            typeMap[Keywords.ULong] = TypeDefinition.CreatePrimitive(typeof(ulong), 8);
            typeMap[Keywords.UShort] = TypeDefinition.CreatePrimitive(typeof(ushort), 2);
        }

        /// <summary>
        /// Populates the map of directive names to processing functions.
        /// </summary>
        private void BuildDirectiveActionMap()
        {
            directiveActionMap[Keywords.Align] = ProcessAlignDirective;
            directiveActionMap[Keywords.Echo] = ProcessEchoDirective;
            directiveActionMap[Keywords.Include] = ProcessIncludeDirective;
            directiveActionMap[Keywords.Local] = ProcessLocalDirective;
            directiveActionMap[Keywords.Typedef] = ProcessTypedefDirective;
        }

        /// <summary>
        /// Populates the map of attribute names to processing functions.
        /// </summary>
        private void BuildAttributeActionMap()
        {
            attributeActionMap[Keywords.Count] = (AttributeProcessAction<int>) ProcessCountAttribute;
            attributeActionMap[Keywords.Kind] = (AttributeProcessAction<TypeDefinition>) ProcessKindAttribute;
            attributeActionMap[Keywords.Message] = (AttributeProcessAction<string>) ProcessMessageAttribute;
            attributeActionMap[Keywords.Name] = (AttributeProcessAction<string>) ProcessNameAttribute;
            attributeActionMap[Keywords.Newline] = (AttributeProcessAction<bool>) ProcessNewlineAttribute;
            attributeActionMap[Keywords.Path] = (AttributeProcessAction<string>) ProcessPathAttribute;
            attributeActionMap[Keywords.Raw] = (AttributeProcessAction<bool>) ProcessRawAttribute;
            attributeActionMap[Keywords.Value] = (AttributeProcessAction<double>) ProcessValueAttribute;
        }

        /// <summary>
        /// Replaces all C-like escape sequences with the non-printable
        /// character they represent.
        /// </summary>
        /// <remarks>
        /// Not all C escape sequences are supported.
        /// </remarks>
        /// <param name="s">
        /// The string on which to resolve escape sequences.
        /// </param>
        /// <returns>
        /// The input string with all valid C-like escape sequences
        /// resolved.
        /// </returns>
        /// <exception cref="FormatException">
        /// If an invalid escape sequence exists in the string.
        /// </exception>
        private static string ResolveEscapeSequences(string s)
        {
            return Regex.Replace(s, @"\\([\S\s])", m =>
            {
                char c = m.Groups[1].Value[0];
                string esc = "";
                switch (c)
                {
                    // Backspace
                    case 'b':
                        esc += '\b';
                        break;

                    // Linefeed
                    case 'n':
                        esc += '\n';
                        break;

                    // Carriage return
                    case 'r':
                        esc += '\r';
                        break;

                    // Horizontal tab
                    case 't':
                        esc += '\t';
                        break;

                    // Literal backslash
                    case '\\':
                        esc += '\\';
                        break;

                    default:
                        string msg = string.Format(@"Invalid escape sequence '\{0}'", c);
                        throw new FormatException(msg);
                }

                return esc;
            });
        }

        /// <summary>
        /// Loads data from an XML file located at the specified path.
        /// </summary>
        /// <param name="path">
        /// The path to the XML file to load.
        /// </param>
        /// <returns>
        /// The loaded XML data as an <see cref="XDocument"/>.
        /// </returns>
        /// <exception cref="TemplateException">
        /// Thrown if there is an error while loading the XML document.
        /// </exception>
        private static XDocument OpenXmlFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path cannot be empty or null");
            }

            try
            {
                return XDocument.Load(path, LoadOptions.SetLineInfo);
            }
            catch (XmlException e)
            {
                throw new TemplateException(e.Message, e);
            }
        }
    }
}
