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
    public class RunningScriptIOS : RunningScript
    {
        private uint m_unknown04;
        private uint m_unknown0C;
        private uint m_unknown14;
        private uint m_unknown210;
        private byte m_unknown214;
        private ushort m_unknown222;
        private uint m_unknown224;

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;

            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_nextScript = r.ReadUInt32();
                m_unknown04 = r.ReadUInt32();
                m_prevScript = r.ReadUInt32();
                m_unknown0C = r.ReadUInt32();
                m_threadId = r.ReadUInt32();
                m_unknown14 = r.ReadUInt32();
                m_name = r.ReadString(8);
                m_instructionPointer = r.ReadUInt32();
                for (int i = 0; i < m_returnStack.Length; i++) {
                    m_returnStack[i] = r.ReadUInt32();
                }
                m_returnStackCount = r.ReadUInt16();
                r.ReadBytes(2);     // align bytes
                for (int i = 0; i < m_localVariables.Length; i++) {
                    m_localVariables[i] = Deserialize<ScriptVariable>(stream);
                }
                m_timer1 = r.ReadUInt32();
                m_timer2 = r.ReadUInt32();
                m_unknown210 = r.ReadUInt32();
                m_unknown214 = r.ReadByte();
                m_ifResult = r.ReadBoolean();
                m_usesMissionCleanup = r.ReadBoolean();
                m_isActive = r.ReadBoolean();
                m_wakeTime = r.ReadUInt32();
                m_logicalOperation = r.ReadUInt16();
                m_notFlag = r.ReadBoolean();
                m_isWastedBustedCheckEnabled = r.ReadBoolean();
                m_isWastedBusted = r.ReadBoolean();
                m_isMission = r.ReadBoolean();
                m_unknown222 = r.ReadUInt16();
                m_unknown224 = r.ReadUInt32();
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
