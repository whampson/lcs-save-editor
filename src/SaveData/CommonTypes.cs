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

namespace WHampson.LcsSaveEditor.SaveData
{
    internal class BlockHeader
    {
        private Primitive<Char8> tag;
        private Primitive<uint> size;

        private StringWrapper tagWrapper;

        public string Tag
        {
            get {
                if (tagWrapper == null) {
                    tagWrapper = new StringWrapper(tag);
                }
                return tagWrapper.Value;
            }
            set {
                if (tagWrapper == null) {
                    tagWrapper = new StringWrapper(tag);
                }
                tagWrapper.Value = value;
            }
        }

        public uint Size
        {
            get { return size.Value; }
            set { size.Value = value; }
        }
    }

    internal class Vector2d
    {
        private Primitive<float> x;
        private Primitive<float> y;

        public float X
        {
            get { return x.Value; }
            set { x.Value = value; }
        }

        public float Y
        {
            get { return y.Value; }
            set { y.Value = value; }
        }
    }

    internal class Vector3d
    {
        private Primitive<float> x;
        private Primitive<float> y;
        private Primitive<float> z;

        public float X
        {
            get { return x.Value; }
            set { x.Value = value; }
        }

        public float Y
        {
            get { return y.Value; }
            set { y.Value = value; }
        }

        public float Z
        {
            get { return z.Value; }
            set { z.Value = value; }
        }
    }

    internal class Timestamp
    {
        private Primitive<uint> second;
        private Primitive<uint> minute;
        private Primitive<uint> hour;
        private Primitive<uint> day;
        private Primitive<uint> month;
        private Primitive<uint> year;

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
