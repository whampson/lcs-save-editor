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
    public class RunningScript : SerializableObject
    {
        // TODO: this is the PS2/Android version
        //       implement iOS version

        private const int NameLength = 8;
        private const int ReturnStackSize = 16;
        private const int NumLocalVariables = 104;  // Unconfirmed
        
        private uint m_nextScriptPointer;
        private uint m_prevScriptPointer;
        private uint m_threadId;
        private uint m_unknown0C;
        private string m_name;
        private uint m_instructionPointer;
        private uint[] m_returnStack;
        private ushort m_returnStackCount;
        private uint[] m_localVariables;
        private uint m_timer1;
        private uint m_timer2;
        private uint m_unknown208;
        private bool m_unknown20C;
        private bool m_unknown20D;
        private bool m_unknown20E;
        private bool m_unknown20F;
        private uint m_wakeTime;
        private bool m_unknown214;
        private bool m_unknown215;
        private bool m_unknown216;
        private bool m_unknown217;
        private uint m_unknown218;

        public RunningScript()
        {
            m_name = string.Empty;
            m_returnStack = new uint[ReturnStackSize];
            m_localVariables = new uint[NumLocalVariables];
        }

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_nextScriptPointer = r.ReadUInt32();
                m_prevScriptPointer = r.ReadUInt32();
                m_threadId = r.ReadUInt32();
                m_unknown0C = r.ReadUInt32();
                m_name = Encoding.ASCII.GetString(r.ReadBytes(NameLength));
                m_instructionPointer = r.ReadUInt32();
                for (int i = 0; i < m_returnStack.Length; i++) {
                    m_returnStack[i] = r.ReadUInt32();
                }
                m_returnStackCount = r.ReadUInt16();
                for (int i = 0; i < m_localVariables.Length; i++) {
                    m_localVariables[i] = r.ReadUInt32();
                }
                m_timer1 = r.ReadUInt32();
                m_timer2 = r.ReadUInt32();
                m_unknown208 = r.ReadUInt32();
                m_unknown20C = r.ReadBoolean();
                m_unknown20D = r.ReadBoolean();
                m_unknown20E = r.ReadBoolean();
                m_unknown20F = r.ReadBoolean();
                m_wakeTime = r.ReadUInt32();
                m_unknown214 = r.ReadBoolean();
                m_unknown215 = r.ReadBoolean();
                m_unknown216 = r.ReadBoolean();
                m_unknown217 = r.ReadBoolean();
                m_unknown218 = r.ReadUInt32();
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write(m_nextScriptPointer);
                w.Write(m_prevScriptPointer);
                w.Write(m_threadId);
                w.Write(m_unknown0C);
                w.Write(Encoding.ASCII.GetBytes(m_name.ToCharArray(), 0, NameLength));  // TODO: check whether NUL is required
                w.Write(m_instructionPointer);
                for (int i = 0; i < m_returnStack.Length; i++) {
                    w.Write(m_returnStack[i]);
                }
                w.Write(m_returnStackCount);
                for (int i = 0; i < m_localVariables.Length; i++) {
                    w.Write(m_localVariables[i]);
                }
                w.Write(m_timer1);
                w.Write(m_timer2);
                w.Write(m_unknown208);
                w.Write(m_unknown20C);
                w.Write(m_unknown20D);
                w.Write(m_unknown20E);
                w.Write(m_unknown20F);
                w.Write(m_wakeTime);
                w.Write(m_unknown214);
                w.Write(m_unknown215);
                w.Write(m_unknown216);
                w.Write(m_unknown217);
                w.Write(m_unknown218);
            }

            return stream.Position - start;
        }
    }
}
