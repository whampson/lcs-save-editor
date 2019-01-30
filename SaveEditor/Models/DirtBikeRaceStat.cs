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
using WHampson.LcsSaveEditor.Helpers;

namespace WHampson.LcsSaveEditor.Models
{
    public class DirtBikeRaceStat : SerializableObject
    {
        private uint m_course1;
        private uint m_course2;
        private uint m_course3;
        private uint m_course4;
        private uint m_course5;
        private uint m_course6;
        private uint m_course7;
        private uint m_course8;
        private uint m_course9;
        private uint m_course10;

        public uint Course1
        {
            get { return m_course1; }
            set { m_course1 = value; OnPropertyChanged(); }
        }

        public uint Course2
        {
            get { return m_course2; }
            set { m_course2 = value; OnPropertyChanged(); }
        }

        public uint Course3
        {
            get { return m_course3; }
            set { m_course3 = value; OnPropertyChanged(); }
        }

        public uint Course4
        {
            get { return m_course4; }
            set { m_course4 = value; OnPropertyChanged(); }
        }

        public uint Course5
        {
            get { return m_course5; }
            set { m_course5 = value; OnPropertyChanged(); }
        }

        public uint Course6
        {
            get { return m_course6; }
            set { m_course6 = value; OnPropertyChanged(); }
        }

        public uint Course7
        {
            get { return m_course7; }
            set { m_course7 = value; OnPropertyChanged(); }
        }

        public uint Course8
        {
            get { return m_course8; }
            set { m_course8 = value; OnPropertyChanged(); }
        }

        public uint Course9
        {
            get { return m_course9; }
            set { m_course9 = value; OnPropertyChanged(); }
        }

        public uint Course10
        {
            get { return m_course10; }
            set { m_course10 = value; OnPropertyChanged(); }
        }

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_course1 = r.ReadUInt32();
                m_course2 = r.ReadUInt32();
                m_course3 = r.ReadUInt32();
                m_course4 = r.ReadUInt32();
                m_course5 = r.ReadUInt32();
                m_course6 = r.ReadUInt32();
                m_course7 = r.ReadUInt32();
                m_course8 = r.ReadUInt32();
                m_course9 = r.ReadUInt32();
                m_course10 = r.ReadUInt32();
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write(m_course1);
                w.Write(m_course2);
                w.Write(m_course3);
                w.Write(m_course4);
                w.Write(m_course5);
                w.Write(m_course6);
                w.Write(m_course7);
                w.Write(m_course8);
                w.Write(m_course9);
                w.Write(m_course10);
            }

            return stream.Position - start;
        }
    }
}
