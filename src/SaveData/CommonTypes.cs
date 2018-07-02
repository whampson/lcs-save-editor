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
        private readonly Primitive<Char8> tag;
        private readonly Primitive<uint> blockSize;

        private StringWrapper tagWrapper;

        public BlockHeader()
        {
            tag = new Primitive<Char8>(null, 0);
            blockSize = new Primitive<uint>(null, 0);
        }

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

        public uint BlockSize
        {
            get { return blockSize.Value; }
            set { blockSize.Value = value; }
        }
    }

    internal class Vector2d
    {
        private readonly Primitive<float> x;
        private readonly Primitive<float> y;

        public Vector2d()
        {
            x = new Primitive<float>(null, 0);
            y = new Primitive<float>(null, 0);
        }

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
        private readonly Primitive<float> x;
        private readonly Primitive<float> y;
        private readonly Primitive<float> z;

        public Vector3d()
        {
            x = new Primitive<float>(null, 0);
            y = new Primitive<float>(null, 0);
            z = new Primitive<float>(null, 0);
        }

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

    internal class BanditRaceStat
    {
        private readonly Primitive<uint> thrashinRc;
        private readonly Primitive<uint> raginRc;
        private readonly Primitive<uint> chasinRc;

        public BanditRaceStat()
        {
            thrashinRc = new Primitive<uint>(null, 0);
            raginRc = new Primitive<uint>(null, 0);
            chasinRc = new Primitive<uint>(null, 0);
        }

        public uint ThrashinRc
        {
            get { return thrashinRc.Value; }
            set { thrashinRc.Value = value; }
        }

        public uint RaginRc
        {
            get { return raginRc.Value; }
            set { raginRc.Value = value; }
        }

        public uint ChasinRc
        {
            get { return chasinRc.Value; }
            set { chasinRc.Value = value; }
        }
    }

    internal class StreetRaceStat
    {
        private readonly Primitive<uint> lowRiderRumble;
        private readonly Primitive<uint> deimosDash;
        private readonly Primitive<uint> wiCheetahRun;
        private readonly Primitive<uint> redLightRacing;
        private readonly Primitive<uint> torringtonTT;
        private readonly Primitive<uint> gangstaGP;

        public StreetRaceStat()
        {
            lowRiderRumble = new Primitive<uint>(null, 0);
            deimosDash = new Primitive<uint>(null, 0);
            wiCheetahRun = new Primitive<uint>(null, 0);
            redLightRacing = new Primitive<uint>(null, 0);
            torringtonTT = new Primitive<uint>(null, 0);
            gangstaGP = new Primitive<uint>(null, 0);
        }

        public uint LowRiderRumble
        {
            get { return lowRiderRumble.Value; }
            set { lowRiderRumble.Value = value; }
        }

        public uint DeimosDash
        {
            get { return deimosDash.Value; }
            set { deimosDash.Value = value; }
        }

        public uint WiCheetahRun
        {
            get { return wiCheetahRun.Value; }
            set { wiCheetahRun.Value = value; }
        }

        public uint RedLightRacing
        {
            get { return redLightRacing.Value; }
            set { redLightRacing.Value = value; }
        }

        public uint TorringtonTT
        {
            get { return torringtonTT.Value; }
            set { torringtonTT.Value = value; }
        }

        public uint GangstaGP
        {
            get { return gangstaGP.Value; }
            set { gangstaGP.Value = value; }
        }
    }

    internal class DirtBikeStat
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

        public DirtBikeStat()
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
