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

namespace WHampson.LcsSaveEditor.Models
{
    /// <summary>
    /// Represents a data container for a chunk of data in a <see cref="SaveData"/> file.
    /// </summary>
    public class DataBlock
    {
        /// <summary>
        /// Creates a new <see cref="DataBlock"/> object with no
        /// <see cref="Tag"/>, zero bytes of <see cref="Data"/>, and
        /// <see cref="StoreBlockSize"/> set to true.
        /// </summary>
        public DataBlock()
        {
            Data = new byte[0];
            Tag = null;
        }

        /// <summary>
        /// Gets or sets the data stored in this <see cref="DataBlock"/>.
        /// </summary>
        public byte[] Data
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the block identifier.
        /// </summary>
        public string Tag
        {
            get;
            set;
        }
    }
}
