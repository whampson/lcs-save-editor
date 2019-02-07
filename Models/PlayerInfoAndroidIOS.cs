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

namespace LcsSaveEditor.Models
{
    /// <summary>
    /// The PlayerInfo structure for the Android and iOS
    /// versions of the game.
    /// </summary>
    public class PlayerInfoAndroidIOS : PlayerInfo
    {
        private uint[] m_unknown00;
        private uint m_unknown84;
        private uint m_unknown90;
        private float m_unknown94;
        private uint m_unknown98;
        private ushort m_unknown9E;

        public PlayerInfoAndroidIOS()
        {
            m_unknown00 = new uint[32];
        }

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                for (int i = 0; i < m_unknown00.Length; i++) {
                    m_unknown00[i] = r.ReadUInt32();
                }
                m_money = r.ReadInt32();
                m_unknown84 = r.ReadUInt32();
                m_moneyOnScreen = r.ReadInt32();
                m_numHiddenPackagesFound = r.ReadUInt32();
                m_unknown90 = r.ReadUInt32();
                m_unknown94 = r.ReadSingle();
                m_unknown98 = r.ReadUInt32();
                m_maxHealth = r.ReadByte();
                m_maxArmor = r.ReadByte();
                m_unknown9E = r.ReadUInt16();
                m_neverGetsTired = r.ReadBoolean();
                m_fastReload = r.ReadBoolean();
                m_fireProof = r.ReadBoolean();
                m_getOutOfJailFree = r.ReadBoolean();
                m_freeHealthCare = r.ReadBoolean();
                m_canDoDriveBy = r.ReadBoolean();
                r.ReadBytes(2);     // align bytes
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                for (int i = 0; i < m_unknown00.Length; i++) {
                    w.Write(m_unknown00[i]);
                }
                w.Write(m_money);
                w.Write(m_unknown84);
                w.Write(m_moneyOnScreen);
                w.Write(m_numHiddenPackagesFound);
                w.Write(m_unknown90);
                w.Write(m_unknown94);
                w.Write(m_unknown98);
                w.Write(m_maxHealth);
                w.Write(m_maxArmor);
                w.Write(m_unknown9E);
                w.Write(m_neverGetsTired);
                w.Write(m_fastReload);
                w.Write(m_fireProof);
                w.Write(m_getOutOfJailFree);
                w.Write(m_freeHealthCare);
                w.Write(m_canDoDriveBy);
                w.Write(new byte[2]);       // align bytes
            }

            return stream.Position - start;
        }
    }
}
