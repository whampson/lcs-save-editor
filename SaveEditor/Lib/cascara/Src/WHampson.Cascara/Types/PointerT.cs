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
using System.Runtime.InteropServices;

namespace WHampson.Cascara.Types
{
    /// <summary>
    /// A pointer to some data in memory.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the data pointed to.
    /// </typeparam>
    public class Pointer<T> : Pointer
        where T : struct
    {
        /// <summary>
        /// Creates a new <see cref="Pointer{T}"/>
        /// that points to the specified address.
        /// </summary>
        /// <param name="addr">
        /// The address to point to.
        /// </param>
        public Pointer(IntPtr addr)
            : base(addr)
        {
        }

        /// <summary>
        /// Dereferences the pointer and gets or sets the value pointed to.
        /// </summary>
        public T Value
        {
            get { return this[0]; }
            set { this[0] = value; }
        }

        /// <summary>
        /// Increments the address pointed to by <paramref name="i"/> units,
        /// then dereferences the value at that address so it can be get or set.
        /// </summary>
        /// <param name="i">
        /// The offset in units of the type <see cref="T"/>. This means that if
        /// <see cref="T"/> is 4 bytes, an offset of "1" will add 4 bytes to the address.
        /// </param>
        public T this[int i]
        {
            get
            {
                int size = Marshal.SizeOf(typeof(T));
                Pointer<T> ptr = Address + (i * size);

                return ptr.GetValue<T>();
            }

            set
            {
                int size = Marshal.SizeOf(typeof(T));
                Pointer<T> ptr = Address + (i * size);

                ptr.SetValue(value);
            }
        }

        public override string ToString()
        {
            return string.Format("[Address: {0}, Type: {1}]", Address.ToInt64(), typeof(T));
        }

        /// <summary>
        /// Converts an <see cref="IntPtr"/> into a <see cref="Pointer{T}"/>.
        /// </summary>
        /// <param name="ptr">
        /// The <see cref="IntPtr"/> to convert.
        /// </param>
        public static implicit operator Pointer<T>(IntPtr ptr)
        {
            return new Pointer<T>(ptr);
        }

        /// <summary>
        /// Converts a <see cref="Pointer{T}"/> into an <see cref="IntPtr"/>.
        /// </summary>
        /// <param name="ptr">
        /// The <see cref="Pointer{T}"/> to convert.
        /// </param>
        public static implicit operator IntPtr(Pointer<T> ptr)
        {
            return ptr.Address;
        }

        /// <summary>
        /// Adds an offset to the address pointed to by a <see cref="Pointer{T}"/>.
        /// The offset is in units of <see cref="T"/>.
        /// </summary>
        /// <remarks>
        /// The offset in units of the type <see cref="T"/>. This means that if
        /// <see cref="T"/> is 4 bytes, an offset of "1" will add 4 bytes to the address.
        /// </remarks>
        /// <param name="ptr">
        /// The base address.
        /// </param>
        /// <param name="off">
        /// The offset to add to the base.
        /// </param>
        /// <returns>
        /// A <see cref="Pointer{T}"/> object that points to the new address.
        /// </returns>
        public static Pointer<T> operator +(Pointer<T> ptr, int off)
        {
            int siz = Marshal.SizeOf(typeof(T));

            return new Pointer<T>(ptr.Address + (siz * off));
        }

        /// <summary>
        /// Subtracts an offset from the address pointed to by a <see cref="Pointer{T}"/>.
        /// The offset is in units of <see cref="T"/>.
        /// </summary>
        /// <remarks>
        /// The offset in units of the type <see cref="T"/>. This means that if
        /// <see cref="T"/> is 4 bytes, an offset of "1" will add 4 bytes to the address.
        /// </remarks>
        /// <param name="ptr">
        /// The base address.
        /// </param>
        /// <param name="off">
        /// The offset to subtract from the base.
        /// </param>
        /// <returns>
        /// A <see cref="Pointer{T}"/> object that points to the new address.
        /// </returns>
        public static Pointer<T> operator -(Pointer<T> ptr, int off)
        {
            int siz = Marshal.SizeOf(typeof(T));

            return new Pointer<T>(ptr.Address - (siz * off));
        }
    }
}
