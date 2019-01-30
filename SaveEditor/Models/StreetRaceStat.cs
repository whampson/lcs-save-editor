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
    public class StreetRaceStat : SerializableObject
    {
        private uint m_lowRiderRumble;
        private uint m_deimosDash;
        private uint m_wiCheetahRun;
        private uint m_redLightRacing;
        private uint m_torringtonTT;
        private uint m_gangstaGP;

        public uint LowRiderRumble
        {
            get { return m_lowRiderRumble; }
            set { m_lowRiderRumble = value; OnPropertyChanged(); }
        }

        public uint DeimosDash
        {
            get { return m_deimosDash; }
            set { m_deimosDash = value; OnPropertyChanged(); }
        }

        public uint WiCheetahRun
        {
            get { return m_wiCheetahRun; }
            set { m_wiCheetahRun = value; OnPropertyChanged(); }
        }

        public uint RedLightRacing
        {
            get { return m_redLightRacing; }
            set { m_redLightRacing = value; OnPropertyChanged(); }
        }

        public uint TorringtonTT
        {
            get { return m_torringtonTT; }
            set { m_torringtonTT = value; OnPropertyChanged(); }
        }

        public uint GangstaGP
        {
            get { return m_gangstaGP; }
            set { m_gangstaGP = value; OnPropertyChanged(); }
        }

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_lowRiderRumble = r.ReadUInt32();
                m_deimosDash = r.ReadUInt32();
                m_wiCheetahRun = r.ReadUInt32();
                m_redLightRacing = r.ReadUInt32();
                m_torringtonTT = r.ReadUInt32();
                m_gangstaGP = r.ReadUInt32();
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write(m_lowRiderRumble);
                w.Write(m_deimosDash);
                w.Write(m_wiCheetahRun);
                w.Write(m_redLightRacing);
                w.Write(m_torringtonTT);
                w.Write(m_gangstaGP);
            }

            return stream.Position - start;
        }
    }
}
