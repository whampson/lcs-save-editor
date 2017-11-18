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
using WHampson.Cascara.Types;

namespace WHampson.Cascara
{
    /// <summary>
    /// Retains information regarding the data type represented
    /// by a contiguous set of bytes.
    /// </summary>
    public sealed class TypeInfo
    {

        /// <summary>
        /// Creates a new <see cref="TypeInfo"/> object using the specified
        /// type, offset, and size.
        /// </summary>
        /// <param name="type">
        /// The .NET <see cref="System.Type"/> that the data represents.
        /// </param>
        /// <param name="offset">
        /// The location in the overall dataset of the first byte
        /// of data represented by this type instance.
        /// </param>
        /// <param name="size">
        /// The number of bytes that an instance of this type occupies.
        /// </param>
        /// <param name="isFullyDefined">
        /// Indicates whether the type has been fully defined.
        /// This is needed when processing structs so we can the struct entry
        /// to the symbol table before its size is fully known.
        /// </param>
        internal TypeInfo(Type type, int offset, int size, bool isFullyDefined)
        {
            if (offset < 0)
            {
                throw new ArgumentException("Offset must be a non-negative integer.", nameof(offset));
            }

            if (size < 0)
            {
                throw new ArgumentException("Size must be a non-negative integer.", nameof(size));
            }

            Type = type;
            Offset = offset;
            Size = size;
            IsFullyDefined = isFullyDefined;
        }

        /// <summary>
        /// Gets the .NET <see cref="System.Type"/> that the data represents.
        /// If the data represents a struct, then this value is <code>null</code>.
        /// </summary>
        public Type Type
        {
            get;
        }

        /// <summary>
        /// Gets location in the binary file data of the first byte
        /// of data for this type instance.
        /// </summary>
        public int Offset
        {
            get;
        }

        /// <summary>
        /// Gets the number of bytes that an instance of this type occupies.
        /// </summary>
        public int Size
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the type has been fully defined.
        /// </summary>
        internal bool IsFullyDefined
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the type is a struct.
        /// </summary>
        public bool IsStruct
        {
            get { return Type == null; }
        }

        public override string ToString()
        {
            return string.Format("[Type: {0}, Offset: {1}, Size: {2}, IsStruct: {3}]",
                Type, Offset, Size, IsStruct);
        }
    }
}
