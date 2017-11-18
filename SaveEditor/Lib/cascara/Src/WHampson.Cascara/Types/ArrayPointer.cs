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

namespace WHampson.Cascara.Types
{
    /// <summary>
    /// A pointer to an array of values in memory.
    /// </summary>
    /// <typeparam name="T">
    /// The type of each element in the array.
    /// </typeparam>
    public class ArrayPointer<T> : Pointer<T>, IEnumerable
        where T : struct
    {
        /// <summary>
        /// Creates a new <see cref="ArrayPointer{T}"/>
        /// that points to the array located at the specified address.
        /// </summary>
        /// <param name="addr">
        /// The address to point to.
        /// </param>
        /// <param name="count">
        /// The number of elements in the array.
        /// </param>
        public ArrayPointer(IntPtr addr, int count)
            : base(addr)
        {
            Count = count;
        }

        /// <summary>
        /// Gets the number of elements in the array.
        /// </summary>
        public int Count
        {
            get;
        }

        public IEnumerator GetEnumerator()
        {
            return new ArrayPointerEnumerator<T>(this);
        }

        public override string ToString()
        {
            return string.Format("[Address: {0}, Type: {1}, Count: {2}]",
                Address.ToInt64(), typeof(T), Count);
        }

        #region Enumerator
        private class ArrayPointerEnumerator<U> : IEnumerator
            where U : struct
        {
            private ArrayPointer<U> arrayPtr;
            private int position;

            public ArrayPointerEnumerator(ArrayPointer<U> a)
            {
                arrayPtr = a;
                Reset();
            }

            public object Current
            {
                get { return arrayPtr[position]; }
            }

            public bool MoveNext()
            {
                position++;

                return position < arrayPtr.Count;
            }

            public void Reset()
            {
                position = -1;
            }
        }
        #endregion
    }
}
