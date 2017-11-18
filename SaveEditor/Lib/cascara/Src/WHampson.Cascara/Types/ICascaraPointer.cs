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

namespace WHampson.Cascara.Types
{
    /// <summary>
    /// Provides a framework for defining a pointer to some data in memory.
    /// </summary>
    public interface ICascaraPointer : IConvertible
    {
        /// <summary>
        /// Gets the absolute memory address pointed to.
        /// </summary>
        IntPtr Address { get; }

        /// <summary>
        /// Gets a value indicating whether this pointer refers to a valid object.
        /// </summary>
        /// <remarks>
        /// It is possible for a pointer to not be null, but still not refer to a
        /// valid object. Take care to ensure that the address pointed to is
        /// usable by the current process.
        /// </remarks>
        /// <returns>
        /// <code>True</code> if the pointer is a null pointer,
        /// <code>False</code> otherwise.
        /// </returns>
        bool IsNull();

        /// <summary>
        /// Gets the value that this pointer points to.
        /// </summary>
        /// <param name="t">
        /// The type of the value to get.
        /// </param>
        /// <returns>
        /// The value that this pointer points to in terms of
        /// the type <paramref name="t"/>.
        /// </returns>
        object GetValue(Type t);

        /// <summary>
        /// Gets the value that this pointer points to.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to get.
        /// </typeparam>
        /// <returns>
        /// The value that this pointer points to in terms of
        /// the generic type <typeparamref name="T"/>.
        /// </returns>
        T GetValue<T>() where T : struct;

        /// <summary>
        /// Sets the value that this pointer points to.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to set.
        /// </typeparam>
        /// <param name="value">
        /// The value to store at the address pointed to by this pointer.
        /// </param>
        void SetValue<T>(T value) where T : struct;
    }
}
