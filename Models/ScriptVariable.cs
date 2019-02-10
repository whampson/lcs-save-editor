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

using LcsSaveEditor.Infrastructure;
using System;
using System.IO;
using System.Text;

namespace LcsSaveEditor.Models
{
    /// <summary>
    /// Represents a game script variable.
    /// </summary>
    public class ScriptVariable : SerializableObject
    {
        private uint m_value;

        public uint ValueUInt32
        {
            get { return m_value; }
            set {
                m_value = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ValueInt32));
                OnPropertyChanged(nameof(ValueSingle));
                OnPropertyChanged(nameof(ValueBoolean));
            }
        }

        public int ValueInt32
        {
            get { return (int) m_value; }
            set {
                unchecked {
                    m_value = (uint) value;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(ValueUInt32));
                OnPropertyChanged(nameof(ValueSingle));
                OnPropertyChanged(nameof(ValueBoolean));
            }
        }

        public float ValueSingle
        {
            get {
                return BitConverter.ToSingle(BitConverter.GetBytes(m_value), 0);
            }
            set {
                m_value = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
                OnPropertyChanged();
                OnPropertyChanged(nameof(ValueUInt32));
                OnPropertyChanged(nameof(ValueInt32));
                OnPropertyChanged(nameof(ValueBoolean));
            }
        }

        public bool ValueBoolean
        {
            get { return m_value != 0; }
            set {
                m_value = value ? 1U : 0U;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ValueUInt32));
                OnPropertyChanged(nameof(ValueInt32));
                OnPropertyChanged(nameof(ValueSingle));
            }
        }

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_value = r.ReadUInt32();
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write(m_value);
            }

            return stream.Position - start;
        }

        public override string ToString()
        {
            return ValueUInt32.ToString();
        }

        public static implicit operator uint(ScriptVariable v)
        {
            return v.ValueUInt32;
        }

        public static implicit operator ScriptVariable(uint v)
        {
            return new ScriptVariable { ValueUInt32 = v };
        }

        public static explicit operator float(ScriptVariable v)
        {
            return v.ValueSingle;
        }

        public static explicit operator ScriptVariable(float v)
        {
            return new ScriptVariable { ValueSingle = v };
        }

        public static explicit operator bool(ScriptVariable v)
        {
            return v.ValueBoolean;
        }

        public static explicit operator ScriptVariable(bool v)
        {
            return new ScriptVariable { ValueBoolean = v };
        }
    }
}
