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

using LcsSaveEditor.Core.Extensions;
using System.IO;
using System.Text;

namespace LcsSaveEditor.Models
{
    /// <summary>
    /// RunningScript formatted for the iOS version of
    /// the game.
    /// </summary>
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
            m_isSerializing = true;

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
                for (int i = 0; i < ReturnStackCount; i++) {
                    m_returnStack.Add(r.ReadUInt32());
                }
                m_returnStackTop = r.ReadUInt16();
                r.ReadBytes(2);     // align bytes
                for (int i = 0; i < LocalVariablesCount; i++) {
                    m_localVariables.Add(r.ReadUInt32());
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

            m_isSerializing = false;
            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            m_isSerializing = true;

            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write(m_nextScript);
                w.Write(m_unknown04);
                w.Write(m_prevScript);
                w.Write(m_unknown0C);
                w.Write(m_threadId);
                w.Write(m_unknown14);
                w.WriteString(m_name, 8);
                w.Write(m_instructionPointer);
                for (int i = 0; i < ReturnStackCount; i++) {
                    w.Write(m_returnStack[i]);
                }
                w.Write(m_returnStackTop);
                w.Write(new byte[2]);       // align bytes
                for (int i = 0; i < LocalVariablesCount; i++) {
                    w.Write(m_localVariables[i]);
                }
                w.Write(m_timer1);
                w.Write(m_timer2);
                w.Write(m_unknown210);
                w.Write(m_unknown214);
                w.Write(m_ifResult);
                w.Write(m_usesMissionCleanup);
                w.Write(m_isActive);
                w.Write(m_wakeTime);
                w.Write(m_logicalOperation);
                w.Write(m_notFlag);
                w.Write(m_isWastedBustedCheckEnabled);
                w.Write(m_isWastedBusted);
                w.Write(m_isMission);
                w.Write(m_unknown222);
                w.Write(m_unknown224);
            }

            m_isSerializing = false;
            return stream.Position - start;
        }
    }
}
