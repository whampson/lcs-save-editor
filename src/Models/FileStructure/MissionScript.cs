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
    public class MissionScript
    {
        private readonly Primitive<uint> nextScriptPointer;
        private readonly Primitive<uint> previousScriptPointer;
        private readonly Primitive<uint> threadId;
        private readonly Primitive<Char8> name;
        private readonly Primitive<uint> instructionPointer;
        private readonly Primitive<uint> returnStack;
        private readonly Primitive<ushort> returnStackTop;
        private readonly ScriptVariable[] variables;
        private readonly Primitive<uint> timer1;
        private readonly Primitive<uint> timer2;
        private readonly Primitive<uint> wakeupTime;

        private StringWrapper nameWrapper;
        private ArrayWrapper<uint> returnStackWrapper;

        public MissionScript()
        {
            nextScriptPointer = new Primitive<uint>(null, 0);
            previousScriptPointer = new Primitive<uint>(null, 0);
            threadId = new Primitive<uint>(null, 0);
            name = new Primitive<Char8>(null, 0);
            instructionPointer = new Primitive<uint>(null, 0);
            returnStack = new Primitive<uint>(null, 0);
            returnStackTop = new Primitive<ushort>(null, 0);
            variables = new ScriptVariable[0];
            timer1 = new Primitive<uint>(null, 0);
            timer2 = new Primitive<uint>(null, 0);
            wakeupTime = new Primitive<uint>(null, 0);
        }

        public uint NextScriptPointer
        {
            get { return nextScriptPointer.Value; }
            set { nextScriptPointer.Value = value; }
        }

        public uint PreviousScriptPointer
        {
            get { return previousScriptPointer.Value; }
            set { previousScriptPointer.Value = value; }
        }

        public uint ThreadId
        {
            get { return threadId.Value; }
            set { threadId.Value = value; }
        }

        public string Name
        {
            get {
                if (nameWrapper == null) {
                    nameWrapper = new StringWrapper(name);
                }

                return nameWrapper.Value;
            }

            set {
                if (nameWrapper == null) {
                    nameWrapper = new StringWrapper(name);
                }

                nameWrapper.Value = value;
            }
        }

        public uint InstructionPointer
        {
            get { return instructionPointer.Value; }
            set { instructionPointer.Value = value; }
        }

        public ArrayWrapper<uint> ReturnStack
        {
            get {
                if (returnStackWrapper == null) {
                    returnStackWrapper = new ArrayWrapper<uint>(returnStack);
                }

                return returnStackWrapper;
            }
        }

        public ushort ReturnStackTop
        {
            get { return returnStackTop.Value; }
            set { returnStackTop.Value = value; }
        }

        public ScriptVariable[] Variables
        {
            get { return variables; }
        }

        public uint Timer1
        {
            get { return timer1.Value; }
            set { timer1.Value = value; }
        }

        public uint Timer2
        {
            get { return timer2.Value; }
            set { timer2.Value = value; }
        }

        public uint WakeupTime
        {
            get { return wakeupTime.Value; }
            set { wakeupTime.Value = value; }
        }
    }
}
