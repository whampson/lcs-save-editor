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

namespace LcsSaveEditor.DataTypes
{
    /// <summary>
    /// Represents a data container in a Grand Theft Auto: Liberty City Stories
    /// save data file.
    /// </summary>
    public class DataBlock
    {
        /// <summary>
        /// Creates a new <see cref="DataBlock"/> object with the
        /// specified block tag.
        /// </summary>
        /// <param name="tag">A four-character block identifier.</param>
        public DataBlock(string tag)
        {
            Tag = tag;
            Data = new byte[0];
        }

        /// <summary>
        /// Gets the block identifier.
        /// </summary>
        public string Tag
        {
            get;
        }

        /// <summary>
        /// Gets or sets the data stored in this <see cref="DataBlock"/>.
        /// </summary>
        public byte[] Data
        {
            get;
            set;
        }

        public override string ToString()
        {
            return string.Format("{0} = {1}, {2} = {3}",
                nameof(Tag), Tag,
                "Length", Data.Length);
        }
    }
}
