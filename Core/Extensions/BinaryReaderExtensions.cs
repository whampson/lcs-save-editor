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

namespace LcsSaveEditor.Core.Extensions
{
    /// <summary>
    /// Extensions to the <see cref="BinaryReader"/> class.
    /// </summary>
    public static class BinaryReaderExtensions
    {
        /// <summary>
        /// Reads a four-byte <see cref="bool"/> value from the current
        /// stream and advances the current position of the stream by four bytes.
        /// </summary>
        /// <param name="r">The <see cref="BinaryReader"/> reading the stream.</param>
        /// <returns>true if the bytes are nonzero; otherwise, false.</returns>
        public static bool ReadBoolean32(this BinaryReader r)
        {
            return r.ReadUInt32() != 0;
        }

        /// <summary>
        /// Reads a zero-terminated string from the current stream and advances
        /// the current position of the stream by the number of bytes read plus
        /// the zero byte.
        /// </summary>
        /// <param name="r">The <see cref="BinaryReader"/> reading the stream.</param>
        /// <returns>The string read from the stream.</returns>
        public static string ReadString(this BinaryReader r)
        {
            string s = "";
            byte c;

            while ((c = r.ReadByte()) != 0) {
                s += (char) c;
            }

            return s;
        }

        /// <summary>
        /// Reads a string of the specified length from the current stream and
        /// advances the current position of the stream by the number of bytes
        /// read. If a NUL character is found, the string returned contains the
        /// characters preceding the NUL character.
        /// </summary>
        /// <param name="r">The <see cref="BinaryReader"/> reading the stream.</param>
        /// <param name="length">The number of characters to read.</param>
        /// <returns>The string read from the stream.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the length parameter is negative.
        /// </exception>
        public static string ReadString(this BinaryReader r, int length)
        {
            if (length < 0) {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            string s = "";
            char c;
            bool zeroSeen = false;

            for (int i = 0; i < length; i++) {
                c = (char) r.ReadByte();
                if (c == '\0') {
                    zeroSeen = true;
                }
                if (!zeroSeen) {
                    s += c;
                }
            }

            return s;
        }

        /// <summary>
        /// Reads a zero-terminated wide string from the current stream and advances
        /// the current position of the stream by the number of bytes read plus
        /// the zero byte. In a wide string, each character occupies two bytes.
        /// </summary>
        /// <param name="r">The <see cref="BinaryReader"/> reading the stream.</param>
        /// <returns>The string read from the stream.</returns>
        public static string ReadWideString(this BinaryReader r)
        {
            string s = "";
            ushort c;

            while ((c = r.ReadUInt16()) != 0) {
                s += (char) c;
            }

            return s;
        }

        /// <summary>
        /// Reads wide a string of the specified length from the current stream
        /// and advances the current position of the stream by the number of bytes
        /// read. In a wide string, each character occupies two bytes. If a NUL
        /// character is found, the string returned contains the characters
        /// preceding the NUL character.
        /// </summary>
        /// <param name="r">The <see cref="BinaryReader"/> reading the stream.</param>
        /// <param name="length">The number of characters to read.</param>
        /// <returns>The string read from the stream.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the length parameter is negative.
        /// </exception>
        public static string ReadWideString(this BinaryReader r, int length)
        {
            if (length < 0) {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            string s = "";
            char c;
            bool zeroSeen = false;

            for (int i = 0; i < length; i++) {
                c = (char) r.ReadUInt16();
                if (c == '\0') {
                    zeroSeen = true;
                }
                if (!zeroSeen) {
                    s += c;
                }
            }

            return s;
        }
    }
}
