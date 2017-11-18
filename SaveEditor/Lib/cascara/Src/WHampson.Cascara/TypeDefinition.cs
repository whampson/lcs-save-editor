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
using System.Xml.Linq;
using WHampson.Cascara.Types;

namespace WHampson.Cascara
{
    /// <summary>
    /// Contains information about a data type used during the processing of a template.
    /// </summary>
    /// <remarks>
    /// This is different from <see cref="TypeInfo"/> in that this represents the
    /// type itself, whereas <see cref="TypeInfo"/> represents an instance of a type.
    /// </remarks>
    internal sealed class TypeDefinition
    {
        /// <summary>
        /// Creates a <see cref="TypeDefinition"/> object representing a primitive
        /// data type.
        /// </summary>
        /// <param name="t">
        /// The .NET type that this primitive represents.
        /// </param>
        /// <param name="size">
        /// The number of bytes that an instance of this type occupies.
        /// </param>
        /// <returns>
        /// The newly-created <see cref="TypeDefinition"/> object.
        /// </returns>
        public static TypeDefinition CreatePrimitive(Type t, int size)
        {
            if (t == null)
            {
                throw new ArgumentNullException("t");
            }

            if (!t.IsValueType)
            {
                throw new ArgumentException("Type must be a value type.");
            }

            if(size < 0)
            {
                throw new ArgumentException("Size must be a non-negative integer.");
            }

            return new TypeDefinition(t, new List<XElement>(), size);
        }

        /// <summary>
        /// Creates a new <see cref="TypeDefinition"/> object that represents a
        /// composite data type.
        /// </summary>
        /// <param name="members">
        /// The <see cref="XElement"/>s that describe the layout of the
        /// composite type.
        /// </param>
        /// <param name="size">
        /// The number of bytes that an instance of this type occupies.
        /// </param>
        /// <returns>
        /// The newly-created <see cref="TypeDefinition"/> object.
        /// </returns>
        public static TypeDefinition CreateStruct(IEnumerable<XElement> members, int size)
        {
            if (members == null)
            {
                throw new ArgumentNullException("members");
            }

            if (size < 0)
            {
                throw new ArgumentException("Size must be a non-negative integer.");
            }

            return new TypeDefinition(null, members, size);
        }

        private TypeDefinition(Type t, IEnumerable<XElement> members, int size)
        {
            Kind = t;
            Members = new List<XElement>(members);
            Size = size;
        }

        /// <summary>
        /// Gets the .NET <see cref="Type"/> represented.
        /// If the data represents a struct, then this value is <code>null</code>.
        /// </summary>
        public Type Kind
        {
            get;
        }

        /// <summary>
        /// Gets the collection of <see cref="XElement"/>s that define this type.
        /// </summary>
        /// <remarks>
        /// If the type does not represent a struct, this list will be empty.
        /// </remarks>
        public IEnumerable<XElement> Members
        {
            get;
        }

        /// <summary>
        /// Gets the number of bytes that an instance of this type occupies.
        /// </summary>
        public int Size
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the type is a struct.
        /// </summary>
        public bool IsStruct
        {
            get { return Kind == null; }
        }

        public override string ToString()
        {
            return string.Format("[Kind: {0}, Size: {1}, IsStruct: {2}]",
                Kind, Size, IsStruct);
        }
    }
}
