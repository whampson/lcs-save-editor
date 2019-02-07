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

using System.IO;
using System.Text;
using LcsSaveEditor.Infrastructure;

namespace LcsSaveEditor.Models
{
    public class BanditRaceStat : SerializableObject
    {
        private uint m_thrashinRC;
        private uint m_raginRC;
        private uint m_chasinRC;

        private uint ThrashinRC
        {
            get { return m_thrashinRC; }
            set { m_thrashinRC = value; OnPropertyChanged(); }
        }

        private uint RaginRC
        {
            get { return m_raginRC; }
            set { m_raginRC = value; OnPropertyChanged(); }
        }

        private uint ChashinRC
        {
            get { return m_chasinRC; }
            set { m_chasinRC = value; OnPropertyChanged(); }
        }

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_thrashinRC = r.ReadUInt32();
                m_raginRC = r.ReadUInt32();
                m_chasinRC = r.ReadUInt32();
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write(m_thrashinRC);
                w.Write(m_raginRC);
                w.Write(m_chasinRC);
            }

            return stream.Position - start;
        }

        public override string ToString()
        {
            return string.Format("{0} = {1}, {2} = {3}, {4} = {5}",
                nameof(ThrashinRC), ThrashinRC,
                nameof(RaginRC), RaginRC,
                nameof(ChashinRC), ChashinRC);
        }
    }
}