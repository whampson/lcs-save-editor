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
using System.Reflection;
using System.Runtime.InteropServices;
using WHampson.Cascara.Types;

using Pointer = WHampson.Cascara.Types.Pointer;

namespace WHampson.Cascara
{
    /// <summary>
    /// Represents a non-text file.
    /// </summary>
    public class BinaryFile : IDisposable
    {
        private IntPtr dataPtr;
        private int dataLen;
        private SymbolTable symTabl;
        private bool hasBeenDisposed;

        // Use Open() to create a new instance
        private BinaryFile(string path, IntPtr addr, int len)
        {
            SourcePath = path;
            dataPtr = addr;
            dataLen = len;
            symTabl = new SymbolTable();

            hasBeenDisposed = false;
        }

        /// <summary>
        /// Gets the path to the file currently loaded.
        /// </summary>
        public string SourcePath
        {
            get;
        }

        /// <summary>
        /// Gets a pointer to the address of the start of the
        /// binary file's data.
        /// </summary>
        protected IntPtr BaseAddress
        {
            get;
        }

        /// <summary>
        /// Gets the number of bytes in the binary file.
        /// </summary>
        public int Length
        {
            get
            {
                if (hasBeenDisposed)
                {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                return dataLen;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this file is no longer available for use.
        /// </summary>
        public bool IsClosed
        {
            get { return hasBeenDisposed; }
        }

        /// <summary>
        /// Gets a value indicating whether the given variable
        /// is defined for this <see cref="BinaryFile"/> instance.
        /// </summary>
        /// <param name="name">
        /// The variable name to check for.
        /// </param>
        /// <returns>
        /// <code>True</code> if <paramref name="name"/> is a valid
        /// variable identifier, <code>False</code> otherwise.
        /// </returns>
        public bool Contains(string name)
        {
            return symTabl.GetEntry(name) != null;
        }

        /// <summary>
        /// Gets a value indicating whether the variable name
        /// is defined and has a value that is part of an array.
        /// </summary>
        /// <param name="name">
        /// The name of the variable to check for.
        /// </param>
        /// <returns>
        /// <code>True</code> if <paramref name="name"/> is defined
        /// and is part of an array, <code>False</code> otherwise.
        /// </returns>
        public bool IsArray(string name)
        {
            return symTabl.IsArray(name);
        }

        /// <summary>
        /// Gets a value indicating whether the variable name
        /// is defined and has a value that is a primitive data type.
        /// </summary>
        /// <param name="name">
        /// The name of the variable to check for.
        /// </param>
        /// <returns>
        /// <code>True</code> if <paramref name="name"/> is defined
        /// and has a value that is a primitive type, <code>False</code> otherwise.
        /// </returns>
        public bool IsPrimitive(string name)
        {
            return symTabl.IsPrimitive(name);
        }

        /// <summary>
        /// Gets a value indicating whether the variable name
        /// is defined and has a value that is a struct.
        /// </summary>
        /// <param name="name">
        /// The name of the variable to check for.
        /// </param>
        /// <returns>
        /// <code>True</code> if <paramref name="name"/> is defined
        /// and has a value that is a struct, <code>False</code> otherwise.
        /// </returns>
        public bool IsStruct(string name)
        {
            return symTabl.IsStruct(name);
        }

        /// <summary>
        /// Gets the number of elements in an array.
        /// </summary>
        /// <param name="name">
        /// The name of the array.
        /// </param>
        /// <returns>
        /// The number of elements in the array,
        /// 0 if the variable is not an array.
        /// </returns>
        public int GetElemCount(string name)
        {
            return symTabl.GetElemCount(name);
        }

        /// <summary>
        /// Gets the .NET <see cref="Type"/> corresponding with the
        /// value of a variable. This method can only be used on variables
        /// whose values are a primitive type (see <see cref="IsPrimitive(string)"/>).
        /// </summary>
        /// <param name="name">
        /// The name of the variable to get the type of.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/> of the variable.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if an attempt is made to retrieve the type of a struct.
        /// </exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public Type GetType(string name)
        {
            SymbolTableEntry entry = GetSymbolTableEntry(name);
            if (entry.TypeInfo.IsStruct)
            {
                string fmt = "Cannot the type of struct '{0}' as structs are made up of many types.";
                throw new ArgumentException(string.Format(fmt, name));
            }

            return entry.TypeInfo.Type;
        }

        /// <summary>
        /// Gets the location of the start of a variable's data
        /// as the number of bytes from the start of the binary file.
        /// </summary>
        /// <param name="name">
        /// The name of the variable to get toe offset of.
        /// </param>
        /// <returns>
        /// The offset of the variable specified by <paramref name="name"/>.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if the variable name does not exist.
        /// </exception>
        public int GetOffset(string name)
        {
            SymbolTableEntry entry = GetSymbolTableEntry(name);
            return entry.TypeInfo.Offset;
        }

        /// <summary>
        /// Gets the size in bytes of a variable's value.
        /// </summary>
        /// <param name="name">
        /// The name of the variable to get the size of.
        /// </param>
        /// <returns>
        /// The size in bytes of the variable's value.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if the variable name does not exist.
        /// </exception>
        public int GetSize(string name)
        {
            SymbolTableEntry entry = GetSymbolTableEntry(name);
            return entry.TypeInfo.Size;
        }

        /// <summary>
        /// Gets a typeless <see cref="Pointer"/> that points to
        /// a position in the binary file.
        /// </summary>
        /// <param name="offset">
        /// The position in the binary file data to point to,
        /// measured as the number of bytes from the start of
        /// the file.
        /// </param>
        /// <returns>
        /// A typeless pointer to the desired offset.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the offset exceeds the bounds of the binary file data.
        /// </exception>
        public Pointer GetPointer(int offset)
        {
            if (!RangeCheck(offset))
            {
                throw new ArgumentOutOfRangeException("offset");
            }

            return new Pointer(dataPtr + offset);
        }

        /// <summary>
        /// Gets a typeless <see cref="Pointer"/> that points to
        /// the position of a variable's value in the binary file.
        /// </summary>
        /// <param name="name">
        /// The name of the variable to get a pointer to.
        /// </param>
        /// <returns>
        /// A typeless pointer to the variable's value.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if the variable name does not exist.
        /// </exception>
        public Pointer GetPointer(string name)
        {
            SymbolTableEntry entry = GetSymbolTableEntry(name);
            return new Pointer(dataPtr + entry.TypeInfo.Offset);
        }

        /// <summary>
        /// Gets a <see cref="Pointer{T}"/> to a value
        /// at the given position in the binary file.
        /// </summary>
        /// <typeparam name="T">
        /// The type of data being pointed to.
        /// </typeparam>
        /// <param name="offset">
        /// The position in the binary file data to point to,
        /// measured as the number of bytes from the start of
        /// the file.
        /// </param>
        /// <returns>
        /// A pointer to a type <typeparamref name="T"/> at the desired offset.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the offset exceeds the bounds of the binary file data.
        /// </exception>
        public Pointer<T> GetPointer<T>(int offset)
            where T : struct
        {
            if (!RangeCheck(offset))
            {
                throw new ArgumentOutOfRangeException("offset");
            }

            return new Pointer<T>(dataPtr + offset);
        }

        /// <summary>
        /// Gets a <see cref="Pointer{T}"/> that points to
        /// the position of a variable's value in the binary file.
        /// </summary>
        /// <typeparam name="T">
        /// The type of data being pointed to.
        /// </typeparam>
        /// <param name="name">
        /// The name of the variable to point to.
        /// </param>
        /// <returns>
        /// A pointer to the variable's value interpreted
        /// as a type <typeparamref name="T"/>.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if the variable name does not exist.
        /// </exception>
        public Pointer<T> GetPointer<T>(string name)
            where T : struct
        {
            SymbolTableEntry entry = GetSymbolTableEntry(name);
            if (entry.TypeInfo.IsStruct)
            {
                string fmt = "Cannot get a pointer to struct '{0}' using type '{1}'.";
                throw new NotSupportedException(string.Format(fmt, name, typeof(T).Name));
            }

            return new Pointer<T>(dataPtr + entry.TypeInfo.Offset);
        }

        /// <summary>
        /// Gets a pointer to an array of the specified type.
        /// </summary>
        /// <typeparam name="T">
        /// The element type.
        /// </typeparam>
        /// <param name="offset">
        /// The position in the binary file of the first byte in the array.
        /// </param>
        /// <param name="count">
        /// The number of elements in the array.
        /// </param>
        /// <returns>
        /// An <see cref="ArrayPointer{T}"/> object that points to
        /// the array at the desired offset.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the offset exceeds the bounds of the binary file data.
        /// </exception>
        public ArrayPointer<T> GetArrayPointer<T>(int offset, int count)
            where T : struct
        {
            if (!RangeCheck(offset))
            {
                throw new ArgumentOutOfRangeException("offset");
            }

            return new ArrayPointer<T>(dataPtr + offset, count);
        }

        /// <summary>
        /// Gets a pointer to an array with the specified name.
        /// </summary>
        /// <typeparam name="T">
        /// The element type.
        /// </typeparam>
        /// <param name="name">
        /// The name of the array.
        /// </param>
        /// <returns>
        /// An <see cref="ArrayPointer{T}"/> object that points to
        /// the array with matching name.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if the variable name does not exist.
        /// </exception>
        public ArrayPointer<T> GetArrayPointer<T>(string name)
            where T : struct
        {
            SymbolTableEntry entry = GetSymbolTableEntry(name);
            int count;
            if (!IsArray(name))
            {
                count = 1;
            }
            else
            {
                count = GetElemCount(name);
            }

            return new ArrayPointer<T>(dataPtr + entry.TypeInfo.Offset, count);
        }

        /// <summary>
        /// Gets the value of the bytes at the specified offset interpreted
        /// as type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the data to be retrieved.
        /// </typeparam>
        /// <param name="offset">
        /// The position in the binary file of the first byte to read.
        /// </param>
        /// <returns>
        /// The value of the bytes at the specified offset interpreted as
        /// the specified type.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the offset exceeds the bounds of the binary file data.
        /// </exception>
        public T GetValue<T>(int offset)
            where T : struct
        {
            Pointer<T> pValue = GetPointer<T>(offset);

            return pValue.Value;
        }

        /// <summary>
        /// Gets the value of the variable with the specified name
        /// as the specified type.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to get.
        /// </typeparam>
        /// <param name="name">
        /// The name of the variable to get the value of.
        /// </param>
        /// <returns>
        /// The value of the variable as type <typeparamref name="T"/>.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if the variable name does not exist.
        /// </exception>
        public T GetValue<T>(string name)
            where T : struct
        {
            Pointer<T> pValue = GetPointer<T>(name);

            return pValue.Value;
        }

        /// <summary>
        /// Gets the value of the bytes at the specified offset interpreted
        /// as the specified type.
        /// </summary>
        /// <param name="valType">
        /// The type of the value to get.
        /// </param>
        /// <param name="offset">
        /// The position in the binary file of the first byte to read.
        /// </param>
        /// <returns>
        /// The value of the bytes at the specified offset interpreted as
        /// the specified type.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the type is not a value type.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the offset exceeds the bounds of the binary file data.
        /// </exception>
        public object GetValue(Type valType, int offset)
        {
            Pointer ptr = dataPtr + offset;
            return ptr.GetValue(valType);
        }

        /// <summary>
        /// Gets the value of the variable with the specified name
        /// as the specified type.
        /// </summary>
        /// <param name="valType">
        /// The type of the value to get.
        /// </param>
        /// <param name="name">
        /// The name of the variable to get the value of.
        /// </param>
        /// <returns>
        /// The value of the variable as the specified type.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the type is not a value type.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if the variable name does not exist.
        /// </exception>
        public object GetValue(Type valType, string name)
        {
            Pointer ptr = GetPointer(name);
            return ptr.GetValue(valType);
        }

        /// <summary>
        /// Sets the value at the specified offset.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to be set.
        /// </typeparam>
        /// <param name="offset">
        /// The location of the first byte to be set.
        /// </param>
        /// <param name="value">
        /// The new value to place at the specified offset.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the offset exceeds the bounds of the binary file data.
        /// </exception>
        public void SetValue<T>(int offset, T value)
            where T : struct
        {
            Pointer<T> pValue = GetPointer<T>(offset);
            pValue.Value = value;
        }

        /// <summary>
        /// Sets the value of the variable with the specified name.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to be set.
        /// </typeparam>
        /// <param name="name">
        /// The name of the variable whose value to set.
        /// </param>
        /// <param name="value">
        /// The new value to give the variable.
        /// </param>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if the variable name does not exist.
        /// </exception>
        public void SetValue<T>(string name, T value)
            where T : struct
        {
            Pointer<T> pValue = GetPointer<T>(name);
            pValue.Value = value;
        }

        /// <summary>
        /// Gets a collection of the most immediate descendants to the
        /// top level of the binary file. That is, a collection
        /// containing the first level of children.
        /// </summary>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing the
        /// most immediate descendants to the top-level of the binary file.
        /// </returns>
        public Dictionary<string, TypeInfo> GetChildren()
        {
            return symTabl.GetChildren()
                .ToDictionary(k => k.Key, v => v.Value.TypeInfo);
        }

        /// <summary>
        /// Gets a collection of the most immediate descendants of the
        /// specified variable.
        /// </summary>
        /// <param name="name">
        /// The name of the variable to get the children of.
        /// </param>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing all
        /// children of the specified variable, if any.
        /// </returns>
        public Dictionary<string, TypeInfo> GetChildren(string name)
        {
            return symTabl.GetChildren(name)
                .ToDictionary(k => k.Key, v => v.Value.TypeInfo);
        }

        /// <summary>
        /// Gets a collection of all variables that exist in the
        /// binary file data. The collection contains the variables
        /// in the order in which they appear in the file data.
        /// </summary>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing all
        /// descendants of the binary file, if any.
        /// </returns>
        public Dictionary<string, TypeInfo> GetDescendants()
        {
            return symTabl.GetDescendants()
                .ToDictionary(k => k.Key, v => v.Value.TypeInfo);
        }

        /// <summary>
        /// Gets a collection of all descendants of the specified variable
        /// in the order in which they appear in the binary file data.
        /// </summary>
        /// <param name="name">
        /// The name of the variable to get the descendants of.
        /// </param>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing all
        /// descendants of the specified variable, if any.
        /// </returns>
        public Dictionary<string, TypeInfo> GetDescendants(string name)
        {
            return symTabl.GetDescendants(name)
                .ToDictionary(k => k.Key, v => v.Value.TypeInfo);
        }

        /// <summary>
        /// Runs a Cascara Binary Template on the file contents
        /// and maps variable names to offsets in the file data
        /// based on the layout of the template.
        /// </summary>
        /// <param name="templateFilePath">
        /// The path to the template to run.
        /// </param>
        public void ApplyTemplate(string templateFilePath)
        {
            ApplyTemplate(templateFilePath, Console.Out);
        }

        /// <summary>
        /// Runs a Cascara Binary Template on the file contents
        /// and maps variable names to offsets in the file data
        /// based on the layout of the template.
        /// </summary>
        /// <param name="templateFilePath">
        /// The path to the template to run.
        /// </param>
        /// <param name="echoWriter">
        /// A <see cref="TextWriter"/> to use as the output stream
        /// for the <code>echo</code> directive.
        /// </param>
        public void ApplyTemplate(string templateFilePath, TextWriter echoWriter)
        {
            TemplateProcessor proc = new TemplateProcessor();
            proc.EchoWriter = echoWriter;
            symTabl = proc.Process(templateFilePath, dataPtr, dataLen);
        }

        /// <summary>
        /// Creates an instance of <typeparamref name="T"/> and sets
        /// the values of the object's public, settable properties
        /// with the values of variables whose names match the properties'
        /// names. If a property's type is a reference type, it will be
        /// set recursively using the members of a struct variable with the
        /// same name. This method requires that <see cref="ApplyTemplate(string)"/>
        /// has been run.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object to be instantiated with the data
        /// from the binary file.
        /// </typeparam>
        /// <returns>
        /// An object of type <typeparamref name="T"/> whose properties are
        /// set using data from the binary file.
        /// </returns>
        public T ExtractData<T>() where T : new()
        {
            // TODO: Rename to Deserialize()?
            // Perhaps Serialize() will become a function later in life...
            return (T) ExtractData(symTabl, typeof(T));
        }

        private object ExtractData(SymbolTable tabl, Type t)
        {
            object o = Activator.CreateInstance(t);
            PropertyInfo[] props = t.GetProperties();

            foreach (PropertyInfo p in props)
            {
                SymbolTableEntry sInfo = tabl.GetEntry(p.Name);
                if (sInfo == null)
                {
                    continue;
                }
                
                // TODO: Ensure AnArray[] gets set correctly (both refTypes and valTypes)

                //bool isCascaraPrimitive = typeof(ICascaraType).IsAssignableFrom(p.PropertyType);
                bool isCascaraPointer = typeof(ICascaraPointer).IsAssignableFrom(p.PropertyType);
                bool isStruct = sInfo.TypeInfo.IsStruct;
                bool isArrayPointer = Pointer.IsArrayPointer(p.PropertyType);
                bool isArray = p.PropertyType.IsArray;

                if (p.PropertyType.IsPrimitive)
                {
                    SetPrimitiveValue(p, o, sInfo);
                }
                else if (isCascaraPointer)
                {
                    if (isArrayPointer)
                    {
                        int elemCount = tabl.GetElemCount(p.Name);
                        SetArrayPointerValue(p, o, sInfo, elemCount);
                    }
                    else
                    {
                        SetPointerValue(p, o, sInfo);
                    }
                }
                else if (isStruct)
                {
                    if (p.PropertyType.IsArray)
                    {
                        SetStructArrayElements(p, o, tabl);
                    }
                    else
                    {
                        object memb = ExtractData(sInfo.Child, p.PropertyType);
                        p.SetValue(o, memb);
                    }
                }
                else
                {
                    // temp
                    throw new InvalidOperationException("Operation not allowed.");
                }
            }

            return o;
        }

        private void SetStructArrayElements(PropertyInfo p, object o, SymbolTable tabl)
        {
            Type propType = p.PropertyType;
            Type elemType = propType.GetElementType();

            int elemCount = tabl.GetElemCount(p.Name);
            Array a = Array.CreateInstance(elemType, elemCount);
            for (int i = 0; i < elemCount; i++)
            {
                string elemName = string.Format("{0}[{1}]", p.Name, i);
                SymbolTableEntry sInfo = tabl.GetEntry(elemName);
                // TODO: watch out for sInfo == null
                object memb = ExtractData(sInfo.Child, elemType);
                a.SetValue(memb, i);
            }

            p.SetValue(o, a);
        }

        private void SetPrimitiveValue(PropertyInfo p, object o, SymbolTableEntry sInfo)
        {
            object val = GetValue(p.PropertyType, sInfo.TypeInfo.Offset);
            p.SetValue(o, Convert.ChangeType(val, p.PropertyType));
        }

        private void SetPointerValue(PropertyInfo p, object o, SymbolTableEntry sInfo)
        {
            Pointer ptr = GetPointer(sInfo.TypeInfo.Offset);

            p.SetValue(o, Convert.ChangeType(ptr, p.PropertyType));
        }

        private void SetArrayPointerValue(PropertyInfo p, object o, SymbolTableEntry sInfo, int elemCount)
        {
            object ptrVal = Activator.CreateInstance(p.PropertyType, dataPtr + sInfo.TypeInfo.Offset, elemCount);
            p.SetValue(o, ptrVal);
        }

        /// <summary>
        /// Store the current state of the file data at the specified location.
        /// </summary>
        /// <param name="filePath">
        /// The path to write the file data.
        /// </param>
        public void Write(string filePath)
        {
            if (hasBeenDisposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            byte[] data = new byte[dataLen];
            Marshal.Copy(dataPtr, data, 0, dataLen);

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                fs.Write(data, 0, dataLen);
            }
        }

        /// <summary>
        /// Closes out the file by releasing all unmanaged resources.
        /// </summary>
        public void Close()
        {
            if (hasBeenDisposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            Dispose();
        }

        /// <summary>
        /// Checks whether a given offset falls within the
        /// range of the file buffer.
        /// </summary>
        private bool RangeCheck(int offset)
        {
            return offset > -1 && offset < dataLen;
        }

        /// <summary>
        /// Helper function that gets an entry from the symbol table and
        /// throws a <see cref="KeyNotFoundException"/> if the name doesn't exist.
        /// </summary>
        private SymbolTableEntry GetSymbolTableEntry(string name)
        {
            SymbolTableEntry entry = symTabl.GetEntry(name);
            if (entry == null)
            {
                string fmt = "The name '{0}' does not exist in the binary file data.";
                throw new KeyNotFoundException(string.Format(fmt, name));
            }

            return entry;
        }

        public override string ToString()
        {
            return string.Format("[Path: {0}, Length: {1}]", SourcePath, Length);
        }

        #region Disposal
        protected virtual void Dispose(bool disposing)
        {
            if (!hasBeenDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // Free unmanaged resources (unmanaged objects)
                if (dataPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(dataPtr);
                    dataPtr = IntPtr.Zero;
                }

                hasBeenDisposed = true;
            }
        }

        ~BinaryFile()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// Loads the contents of the file at the specified
        /// path into memory as a <see cref="BinaryFile"/>.
        /// </summary>
        /// <param name="filePath">
        /// The path to the file to load.
        /// </param>
        /// <returns>
        /// A <see cref="BinaryFile"/> object containing the
        /// contents of the file loaded.
        /// </returns>
        public static BinaryFile Open(string filePath)
        {
            const int OneGibiByte = 1 << 30;

            FileInfo fInfo = new FileInfo(filePath);
            if (fInfo.Length > OneGibiByte)
            {
                throw new IOException("File size must be less than 1 GiB.");
            }

            int len = (int) fInfo.Length;
            byte[] data = new byte[len];

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                fs.Read(data, 0, len);
            }

            IntPtr dataPtr = Marshal.AllocHGlobal(len);
            Marshal.Copy(data, 0, dataPtr, len);

            return new BinaryFile(Path.GetFullPath(filePath), dataPtr, len);
        }
    }
}
