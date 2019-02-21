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

using LcsSaveEditor.Core;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace LcsSaveEditor.Models
{
    public class Timestamp : SerializableObject
    {
        private uint m_second;
        private uint m_minute;
        private uint m_hour;
        private uint m_day;
        private uint m_month;
        private uint m_year;

        public uint Second
        {
            get { return m_second; }
            set { m_second = value; OnPropertyChanged(); }
        }

        public uint Minute
        {
            get { return m_minute; }
            set { m_minute = value; OnPropertyChanged(); }
        }

        public uint Hour
        {
            get { return m_hour; }
            set { m_hour = value; OnPropertyChanged(); }
        }

        public uint Day
        {
            get { return m_day; }
            set { m_day = value; OnPropertyChanged(); }
        }

        public uint Month
        {
            get { return m_month; }
            set { m_month = value; OnPropertyChanged(); }
        }

        public uint Year
        {
            get { return m_year; }
            set { m_year = value; OnPropertyChanged(); }
        }

        public DateTime ToDateTime()
        {
            string dateStr = string.Format("{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}",
                (int) m_year,
                (int) m_month,
                (int) m_day,
                (int) m_hour,
                (int) m_minute,
                (int) m_second);

            DateTime.TryParseExact(dateStr, "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt);

            return dt;
        }

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_second = r.ReadUInt32();
                m_minute = r.ReadUInt32();
                m_hour = r.ReadUInt32();
                m_day = r.ReadUInt32();
                m_month = r.ReadUInt32();
                m_year = r.ReadUInt32();
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write(m_second);
                w.Write(m_minute);
                w.Write(m_hour);
                w.Write(m_day);
                w.Write(m_month);
                w.Write(m_year);
            }

            return stream.Position - start;
        }

        public override string ToString()
        {
            return ToDateTime().ToString("dd MMM yyyy HH:mm:ss");
        }
    }
}
