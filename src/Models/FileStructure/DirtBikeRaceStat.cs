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

namespace WHampson.LcsSaveEditor.Models.FileStructure
{
    public class DirtBikeRaceStat
    {
        private readonly Primitive<uint> course1;
        private readonly Primitive<uint> course2;
        private readonly Primitive<uint> course3;
        private readonly Primitive<uint> course4;
        private readonly Primitive<uint> course5;
        private readonly Primitive<uint> course6;
        private readonly Primitive<uint> course7;
        private readonly Primitive<uint> course8;
        private readonly Primitive<uint> course9;
        private readonly Primitive<uint> course10;

        public DirtBikeRaceStat()
        {
            course1 = new Primitive<uint>(null, 0);
            course2 = new Primitive<uint>(null, 0);
            course3 = new Primitive<uint>(null, 0);
            course4 = new Primitive<uint>(null, 0);
            course5 = new Primitive<uint>(null, 0);
            course6 = new Primitive<uint>(null, 0);
            course7 = new Primitive<uint>(null, 0);
            course8 = new Primitive<uint>(null, 0);
            course9 = new Primitive<uint>(null, 0);
            course10 = new Primitive<uint>(null, 0);
        }

        public uint Course1
        {
            get { return course1.Value; }
            set { course1.Value = value; }
        }

        public uint Course2
        {
            get { return course2.Value; }
            set { course2.Value = value; }
        }

        public uint Course3
        {
            get { return course3.Value; }
            set { course3.Value = value; }
        }

        public uint Course4
        {
            get { return course4.Value; }
            set { course4.Value = value; }
        }

        public uint Course5
        {
            get { return course5.Value; }
            set { course5.Value = value; }
        }

        public uint Course6
        {
            get { return course6.Value; }
            set { course6.Value = value; }
        }

        public uint Course7
        {
            get { return course7.Value; }
            set { course7.Value = value; }
        }

        public uint Course8
        {
            get { return course8.Value; }
            set { course8.Value = value; }
        }

        public uint Course9
        {
            get { return course9.Value; }
            set { course9.Value = value; }
        }

        public uint Course10
        {
            get { return course10.Value; }
            set { course10.Value = value; }
        }
    }
}
