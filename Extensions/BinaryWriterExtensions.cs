#region License
/* Copyright(c) 2016-2019 Wes Hampson
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
using System.IO;

namespace LcsSaveEditor.Extensions
{
    /// <summary>
    /// Extensions to the <see cref="BinaryWriter"/> class.
    /// </summary>
    public static class BinaryWriterExtensions
    {
        /// <summary>
        /// Writes a four-byte <see cref = "bool" /> value to the current
        /// stream, with 0 representing false and 1 representing true.
        /// </summary>
        /// <param name="w">The <see cref="BinaryReader"/> writing the stream.</param>
        public static void WriteBoolean32(this BinaryWriter w, bool value)
        {
            w.Write(value ? 1U : 0U);
        }

        /// <summary>
        /// Writes a zero-terminated string to the current stream.
        /// </summary>
        /// <param name="w">The <see cref="BinaryWriter"/> writing the stream.</param>
        /// <param name="s">The string to write.</param>
        public static void WriteString(this BinaryWriter w, string s)
        {
            WriteString(w, s, s.Length + 1);
        }

        /// <summary>
        /// Writes a string of the specified length to the current stream. If the
        /// specified length is greater than the string length, the string will be
        /// zero-padded from the right to match the specified length.
        /// </summary>
        /// <param name="w">The <see cref="BinaryWriter"/> writing the stream.</param>
        /// <param name="s">The string to write.</param>
        /// <param name="length">The number of characters to write.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the specified string is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the length parameter is negative.
        /// </exception>
        public static void WriteString(this BinaryWriter w, string s, int length)
        {
            if (s == null) {
                throw new ArgumentNullException(nameof(s));
            }

            if (length < 0) {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            s = s.PadRight(length, '\0');
            for (int i = 0; i < length; i++) {
                w.Write((byte) s[i]);
            }
        }

        /// <summary>
        /// Writes a zero-terminated wide string to the current stream.
        /// In a wide string, each character occupies two bytes.
        /// </summary>
        /// <param name="w">The <see cref="BinaryWriter"/> writing the stream.</param>
        /// <param name="s">The string to write.</param>
        public static void WriteWideString(this BinaryWriter w, string s)
        {
            WriteWideString(w, s, s.Length + 1);
        }

        /// <summary>
        /// Writes a wide string of the specified length to the current stream. In a
        /// wide string, each character occupies two bytes. If the specified length
        /// is greater than the string length, the string will be zero-padded from the
        /// right to match the specified length.
        /// </summary>
        /// <param name="w">The <see cref="BinaryWriter"/> writing the stream.</param>
        /// <param name="s">The string to write.</param>
        /// <param name="length">The number of characters to write.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the specified string is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the length parameter is negative.
        /// </exception>
        public static void WriteWideString(this BinaryWriter w, string s, int length)
        {
            if (s == null) {
                throw new ArgumentNullException(nameof(s));
            }

            if (length < 0) {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            s = s.PadRight(length, '\0');
            for (int i = 0; i < length; i++) {
                w.Write((ushort) s[i]);
            }
        }
    }
}
