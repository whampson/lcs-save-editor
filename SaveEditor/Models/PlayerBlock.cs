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
    public class PlayerBlock : SerializableObject
    {
        // TODO: this is for PS2, implement iOS and Android
        private int m_money;
        private uint m_unknown04;
        private byte m_unknown08;
        private byte m_unknown09;
        private byte m_unknown0A;
        private float m_unknown0B;
        private int m_moneyOnScreen;
        private uint m_hiddenPackagesFound;
        private uint m_unknown17;
        private bool m_neverGetsTired;
        private bool m_fastReload;
        private bool m_fireProof;
        private byte m_maxHealth;
        private byte m_maxArmor;
        private bool m_getOutOfJailFree;
        private bool m_freeHealthCare;
        private bool m_unknown31;
        private byte m_unknown35;
        private byte m_unknown36;
        private byte m_unknown37;
        private uint[] m_unknown38;

        public PlayerBlock()
        {
            m_unknown38 = new uint[86];
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
                m_hiddenPackagesFound = r.ReadUInt32();
                m_unknown17 = r.ReadUInt32();
                m_neverGetsTired = r.ReadUInt32() != 0;
                m_fastReload = r.ReadUInt32() != 0;
                m_fireProof = r.ReadUInt32() != 0;
                m_maxHealth = r.ReadByte();
                m_maxArmor = r.ReadByte();
                m_getOutOfJailFree = r.ReadUInt32() != 0;
                m_freeHealthCare = r.ReadUInt32() != 0;
                m_unknown31 = r.ReadUInt32() != 0;
                m_unknown35 = r.ReadByte();
                m_unknown36 = r.ReadByte();
                m_unknown37 = r.ReadByte();
                for (int i = 0; i < m_unknown38.Length; i++) {
                    m_unknown38[i] = r.ReadUInt32();
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
                w.Write(m_hiddenPackagesFound);
                w.Write(m_neverGetsTired ? 1U : 0);
                w.Write(m_fastReload ? 1U : 0);
                w.Write(m_fireProof ? 1U : 0);
                w.Write(m_maxHealth);
                w.Write(m_maxArmor);
                w.Write(m_getOutOfJailFree ? 1U : 0);
                w.Write(m_freeHealthCare ? 1U : 0);
                w.Write(m_unknown31 ? 1U : 0);
                w.Write(m_unknown35);
                w.Write(m_unknown36);
                w.Write(m_unknown37);
                for (int i = 0; i < m_unknown38.Length; i++) {
                    w.Write(m_unknown38[i]);
                }
            }

            return stream.Position - start;
        }
    }
}
