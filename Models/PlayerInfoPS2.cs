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

using LcsSaveEditor.Extensions;
using System;
using System.IO;
using System.Text;

namespace LcsSaveEditor.Models
{
    public class PlayerInfoPS2 : PlayerInfo
    {
        private uint m_unknown04;
        private byte m_unknown08;
        private byte m_unknown09;
        private byte m_unknown0A;
        private float m_unknown0B;
        private uint m_unknown17;
        private uint[] m_unknown34;

        public PlayerInfoPS2()
        {
            m_unknown34 = new uint[87];
        }

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_money = r.ReadInt32();
                m_unknown04 = r.ReadUInt32();
                m_unknown08 = r.ReadByte();
                m_unknown09 = r.ReadByte();
                m_unknown0A = r.ReadByte();
                m_unknown0B = r.ReadSingle();
                m_moneyOnScreen = r.ReadInt32();
                m_numHiddenPackagesFound = r.ReadUInt32();
                m_unknown17 = r.ReadUInt32();
                m_neverGetsTired = r.ReadBoolean32();
                m_fastReload = r.ReadBoolean32();
                m_fireProof = r.ReadBoolean32();
                m_maxHealth = r.ReadByte();
                m_maxArmor = r.ReadByte();
                m_getOutOfJailFree = r.ReadBoolean32();
                m_freeHealthCare = r.ReadBoolean32();
                m_canDoDriveBy = r.ReadBoolean();
                r.ReadBytes(2);     // align bytes
                for (int i = 0; i < m_unknown34.Length; i++) {
                    m_unknown34[i] = r.ReadUInt32();
                }
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write(m_money);
                w.Write(m_unknown04);
                w.Write(m_unknown08);
                w.Write(m_unknown09);
                w.Write(m_unknown0A);
                w.Write(m_unknown0B);
                w.Write(m_moneyOnScreen);
                w.Write(m_numHiddenPackagesFound);
                w.Write(m_unknown17);
                w.WriteBoolean32(m_neverGetsTired);
                w.WriteBoolean32(m_fastReload);
                w.WriteBoolean32(m_fireProof);
                w.Write(m_maxHealth);
                w.Write(m_maxArmor);
                w.WriteBoolean32(m_getOutOfJailFree);
                w.WriteBoolean32(m_freeHealthCare);
                w.Write(m_canDoDriveBy);
                w.Write(new byte[2]);       // align bytes
                for (int i = 0; i < m_unknown34.Length; i++) {
                    w.Write(m_unknown34[i]);
                }
            }

            return stream.Position - start;
        }
    }
}
