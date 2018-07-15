#region License
/* Copyright(c) 2016-2018 Wes Hampson
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

using WHampson.Cascara;

namespace WHampson.LcsSaveEditor.FileStructure
{
    public class Timestamp
    {
        private readonly Primitive<uint> second;
        private readonly Primitive<uint> minute;
        private readonly Primitive<uint> hour;
        private readonly Primitive<uint> day;
        private readonly Primitive<uint> month;
        private readonly Primitive<uint> year;

        public Timestamp()
        {
            second = new Primitive<uint>(null, 0);
            minute = new Primitive<uint>(null, 0);
            hour = new Primitive<uint>(null, 0);
            day = new Primitive<uint>(null, 0);
            month = new Primitive<uint>(null, 0);
            year = new Primitive<uint>(null, 0);
        }

        public uint Second
        {
            get { return second.Value; }
            set { second.Value = value; }
        }

        public uint Minute
        {
            get { return minute.Value; }
            set { minute.Value = value; }
        }

        public uint Hour
        {
            get { return hour.Value; }
            set { hour.Value = value; }
        }

        public uint Day
        {
            get { return day.Value; }
            set { day.Value = value; }
        }

        public uint Month
        {
            get { return month.Value; }
            set { month.Value = value; }
        }

        public uint Year
        {
            get { return year.Value; }
            set { year.Value = value; }
        }
    }
}
