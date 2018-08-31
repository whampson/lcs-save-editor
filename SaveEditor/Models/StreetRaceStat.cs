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

namespace WHampson.LcsSaveEditor.Models
{
    public class StreetRaceStat
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
}
