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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WHampson.Cascara
{
    /// <summary>
    /// Associates identifiers with a type, offset and child
    /// <see cref="SymbolTable"/> for nested identifiers.
    /// </summary>
    internal sealed class SymbolTable : IEnumerable<KeyValuePair<string, SymbolTableEntry>>
    {
        private Dictionary<string, SymbolTableEntry> entries;

        /// <summary>
        /// Creates a new nameless, parentless <see cref="SymbolTable"/>.
        /// Use this for creating the root table.
        /// </summary>
        public SymbolTable()
            : this(null, null)
        {
        }

        /// <summary>
        /// Creates a new named <see cref="SymbolTable"/> that is the
        /// child of an existing table.
        /// </summary>
        /// <param name="name">
        /// The parent symbol associated with the symbol table.
        /// </param>
        /// <param name="parent">
        /// The <see cref="SymbolTable"/> from which this table stems.
        /// </param>
        public SymbolTable(string name, SymbolTable parent)
        {
            Name = name;
            Parent = parent;
            entries = new Dictionary<string, SymbolTableEntry>();
        }

        /// <summary>
        /// Gets locally scoped name of this table.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// Gets the parent table.
        /// Returns <code>null</code> if this is the root table.
        /// </summary>
        public SymbolTable Parent
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this table has a parent table.
        /// </summary>
        public bool HasParent
        {
            get { return Parent != null; }
        }

        /// <summary>
        /// Adds the provided <see cref="SymbolTableEntry"/> to the table and
        /// associates it with the provided name.
        /// </summary>
        /// <param name="symbol">
        /// The name to be given to the table entry.
        /// </param>
        /// <param name="e">
        /// The <see cref="SymbolTableEntry"/> to add to the table.
        /// </param>
        /// <returns>
        /// <code>False</code> if the given name already exists in the table.
        /// </returns>
        public bool AddEntry(string name, SymbolTableEntry e)
        {
            string symbol = CreateSymbol(name);
            if (entries.ContainsKey(symbol))
            {
                return false;
            }

            entries.Add(symbol, e);

            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the provided entry name
        /// exists in the current table or parent tables.
        /// </summary>
        /// <param name="name">
        /// The name to check for existence
        /// </param>
        /// <returns>
        /// <code>True</code> if the entry exists.
        /// <<code>False</code> if not found.
        /// </returns>
        public bool ContainsEntry(string name)
        {
            return GetEntry(name) != null;
        }

        /// <summary>
        /// Gets an entry from the table.
        /// </summary>
        /// <remarks>
        /// The parent tables will be searched for the entry if necessary.
        /// </remarks>
        /// <param name="name">
        /// The name of the entry to get.
        /// </param>
        /// <returns>
        /// The entry if it exists, <code>null</code> otherwise.
        /// </returns>
        public SymbolTableEntry GetEntry(string name)
        {
            string[] splitname = name.Split('.');

            // Traverse up the tree until we find the first part of the name.
            SymbolTableEntry top = SearchUp(this, CreateSymbol(splitname[0]));
            if (top == null)
            {
                return null;
            }

            // Traverse down from the top until we find the entry whose name
            // matches the last part of the provided name
            SymbolTableEntry result = top;
            SymbolTable tabl = result.Child;
            string sym;

            for (int i = 1; i < splitname.Length; i++)
            {
                sym = CreateSymbol(splitname[i]);
                if (tabl == null || !tabl.entries.TryGetValue(sym, out result))
                {
                    return null;
                }
                tabl = result.Child;
            }

            return result;
        }

        /// <summary>
        /// Gets a collection of all symbol names in the symbol table
        /// and child tables.
        /// </summary>
        /// <returns>
        /// A collection of all symbol names.
        /// </returns>
        public IEnumerable<string> GetAllKeys()
        {
            return GetAllKeys(this);
        }

        /// <summary>
        /// Gets the most immediate descendants of this symbol table.
        /// </summary>
        /// <returns>
        /// A dictionary containing the children of the current symbol table (if any).
        /// </returns>
        public Dictionary<string, SymbolTableEntry> GetChildren()
        {
            return GetChildren(this);
        }

        /// <summary>
        /// Gets the most immediate descendants of the symbol with the
        /// specified name.
        /// </summary>
        /// <param name="name">
        /// The name of the symbol to get the children of.
        /// </param>
        /// <returns>
        /// A dictionary containing the children of the specified symbol (if any).
        /// </returns>
        public Dictionary<string, SymbolTableEntry> GetChildren(string name)
        {
            SymbolTableEntry e = GetEntry(name);
            if (e == null)
            {
                string msg = string.Format("Variable '{0}' not found.", name);
                throw new KeyNotFoundException(msg);
            }

            if (!e.HasChild)
            {
                return new Dictionary<string, SymbolTableEntry>();
            }

            return GetChildren(e.Child);
        }

        /// <summary>
        /// Gets all symbols that stem from this symbol table,
        /// all the way to the leaves.
        /// </summary>
        /// <returns>
        /// A dictionary containing all descendants of the current symbol table (if any).
        /// </returns>
        public Dictionary<string, SymbolTableEntry> GetDescendants()
        {
            return GetDescendants(this);
        }

        /// <summary>
        /// Gets all symbols that stem from this specified symbol,
        /// all the way to the leaves.
        /// </summary>
        /// <param name="name">
        /// The name of the symbol to get the descendants of.
        /// </param>
        /// <returns>
        /// A dictionary containing all descendants of the specified symbol (if any).
        /// </returns>
        public Dictionary<string, SymbolTableEntry> GetDescendants(string name)
        {
            SymbolTableEntry e = GetEntry(name);
            if (e == null)
            {
                string msg = string.Format("Variable '{0}' not found.", name);
                throw new KeyNotFoundException(msg);
            }

            if (!e.HasChild)
            {
                return new Dictionary<string, SymbolTableEntry>();
            }

            return GetDescendants(e.Child);
        }

        /// <summary>
        /// Gets the globally-scoped name of this table.
        /// </summary>
        /// <returns>
        /// The globally-scoped name of this table.
        /// </returns>
        public string GetFullyQualifiedName()
        {
            if (Parent == null)
            {
                return null;
            }

            string localName = Name;
            if (!Parent.IsArray(localName))
            {
                localName = StripArrayNotation(localName);
            }

            if (Parent.Name == null)
            {
                return localName;
            }

            return Parent.GetFullyQualifiedName() + "." + localName;
        }

        /// <summary>
        /// Gets the fully-qualified name of the entry with the
        /// matching <paramref name="name"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the entry.
        /// </param>
        /// <returns>
        /// The fully-qualified name of the entry.
        /// </returns>
        public string GetFullyQualifiedName(string name)
        {
            if (!ContainsEntry(name))
            {
                return null;
            }

            if (!IsArray(name))
            {
                name = StripArrayNotation(name);
            }

            string baseName = GetFullyQualifiedName();
            if (string.IsNullOrEmpty(baseName))
            {
                return name;
            }

            return baseName + "." + name;
        }

        /// <summary>
        /// Gets a value indicating whether the given name
        /// refers to a primitive data type.
        /// </summary>
        /// <remarks>
        /// Primitives and structs are mutually exclusive.
        /// </remarks>
        /// <param name="name">
        /// The name to check for.
        /// </param>
        /// <returns>
        /// <code>True</code> if the name refers to a primitive type,
        /// <code>False</code> otherwise.
        /// </returns>
        public bool IsPrimitive(string name)
        {
            return ContainsEntry(name) && !IsStruct(name);
        }

        /// <summary>
        /// Gets a value indicating whether the given name
        /// refers to a struct.
        /// </summary>
        /// <remarks>
        /// Primitives and structs are mutually exclusive.
        /// </remarks>
        /// <param name="name">
        /// The name to check for.
        /// </param>
        /// <returns>
        /// <code>True</code> if the name refers to a struct,
        /// <code>False</code> otherwise.
        /// </returns>
        public bool IsStruct(string name)
        {
            if (!ContainsEntry(name))
            {
                return false;
            }

            return GetEntry(name).TypeInfo.IsStruct;
        }

        /// <summary>
        /// Gets a value indicating whether the given name
        /// refers to an array.
        /// </summary>
        /// <param name="name">
        /// The name to check for.
        /// </param>
        /// <returns>
        /// <code>True</code> if the name refers to an array,
        /// <code>False</code> otherwise.
        /// </returns>
        public bool IsArray(string name)
        {
            name = StripArrayNotation(name);
            string elem1 = name + "[1]";

            return ContainsEntry(elem1);
        }

        /// <summary>
        /// Gets the number of elements in the array
        /// referred to by the given name.
        /// </summary>
        /// <param name="name">
        /// The name of the array to get the element count for.
        /// </param>
        /// <returns>
        /// The number of elements in the array.
        /// If the given name does not exist or does not refer
        /// to an array, 0 is returned.
        /// </returns>
        public int GetElemCount(string name)
        {
            if (!IsArray(name))
            {
                return 0;
            }

            name = StripArrayNotation(name);
            int count = 0;
            do
            {
                string elemName = name + string.Format("[{0}]", count);
                if (!ContainsEntry(elemName))
                {
                    break;
                }
                count++;
            } while (true);

            return count;
        }

        private string StripArrayNotation(string s)
        {
            return Regex.Replace(s, @"\[\d+\]$", "");
        }

        public IEnumerator<KeyValuePair<string, SymbolTableEntry>> GetEnumerator()
        {
            return entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return string.Format("[Name: {0}, NumEntries: {1}, HasParent: {2}]",
                GetFullyQualifiedName(), entries.Count, HasParent);
        }

        /// <summary>
        /// Creates a unique symbol for the given identifier by appending
        /// an array index to the end (if not already present).
        /// </summary>
        /// <param name="name">
        /// The identifier to turn into a symbol.
        /// </param>
        /// <returns>
        /// The newly-created symbol.
        /// </returns>
        private static string CreateSymbol(string name)
        {
            const string ArrayNotationPattern = @"^.*\[\d+\]$";

            if (!Regex.IsMatch(name, ArrayNotationPattern))
            {
                name = name + "[0]";
            }

            return name;
        }

        /// <summary>
        /// Searches the parent tables for the given symbol.
        /// </summary>
        /// <param name="tabl">
        /// The table to begin the search from.
        /// </param>
        /// <param name="symbolName">
        /// The symbol to search for.
        /// </param>
        /// <returns>
        /// The <see cref="SymbolTableEntry"/> corresponding to the symbol if found,
        /// <code>null</code> if not found.
        /// </returns>
        private static SymbolTableEntry SearchUp(SymbolTable tabl, string symbolName)
        {
            if (tabl == null)
            {
                return null;
            }

            if (tabl.entries.TryGetValue(symbolName, out SymbolTableEntry entry))
            {
                return entry;
            }

            return SearchUp(tabl.Parent, symbolName);
        }

        /// <summary>
        /// Gets a collection of all symbol names from the given
        /// <see cref="SymbolTable"/> and child tables.
        /// </summary>
        /// <param name="tabl">
        /// The table to get the collection of symbols from.
        /// </param>
        /// <returns>
        /// The collection of all symbols found in the given table.
        /// </returns>
        private static IEnumerable<string> GetAllKeys(SymbolTable tabl)
        {
            List<string> keys = new List<string>();
            foreach (KeyValuePair<string, SymbolTableEntry> kvp in tabl)
            {
                string name = tabl.GetFullyQualifiedName(kvp.Key);
                keys.Add(name);

                SymbolTableEntry entry = kvp.Value;
                if (entry.HasChild)
                {
                    keys.AddRange(GetAllKeys(entry.Child));
                }
            }

            return keys;
        }

        private static Dictionary<string, SymbolTableEntry> GetChildren(SymbolTable tabl)
        {
            Dictionary<string, SymbolTableEntry> children = new Dictionary<string, SymbolTableEntry>();
            foreach (KeyValuePair<string, SymbolTableEntry> kvp in tabl)
            {
                string name = tabl.GetFullyQualifiedName(kvp.Key);
                SymbolTableEntry entry = kvp.Value;
                children[name] = entry;
            }

            return children;
        }

        private static Dictionary<string, SymbolTableEntry> GetDescendants(SymbolTable tabl)
        {
            Dictionary<string, SymbolTableEntry> descendants = new Dictionary<string, SymbolTableEntry>();
            foreach (KeyValuePair<string, SymbolTableEntry> kvp in tabl)
            {
                string name = tabl.GetFullyQualifiedName(kvp.Key);
                SymbolTableEntry entry = kvp.Value;
                descendants[name] = entry;

                if (entry.HasChild)
                {
                    Dictionary<string, SymbolTableEntry> childEntries = GetDescendants(entry.Child);
                    descendants = descendants.Concat(childEntries).ToDictionary(x => x.Key, y => y.Value);
                }
            }

            return descendants;
        }
    }
}
