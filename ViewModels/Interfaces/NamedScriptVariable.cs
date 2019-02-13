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
using LcsSaveEditor.Models;
using System;

namespace LcsSaveEditor.ViewModels
{
    /// <summary>
    /// Represents a named <see cref="ScriptVariable"/>.
    /// </summary>
    public class NamedScriptVariable : ObservableObject
    {
        private string m_name;
        private ScriptVariable m_value;

        public NamedScriptVariable()
            : this(string.Empty, new ScriptVariable())
        { }

        public NamedScriptVariable(ScriptVariable value)
            : this(string.Empty, value)
        { }

        public NamedScriptVariable(string name, ScriptVariable value)
        {
            m_name = (name != null) ? name.Trim() : string.Empty;
            m_value = value ?? new ScriptVariable();
        }

        public string Name
        {
            get { return m_name; }
            set {
                m_name = (value != null) ? value.Trim() : string.Empty;
                OnPropertyChanged();
            }
        }

        public ScriptVariable Value
        {
            get { return m_value; }
            set {
                m_value = value ?? new ScriptVariable();
                OnPropertyChanged();
                OnPropertyChanged(nameof(ValueUInt32));
                OnPropertyChanged(nameof(ValueInt32));
                OnPropertyChanged(nameof(ValueSingle));
                OnPropertyChanged(nameof(ValueBoolean));
            }
        }

        public uint ValueUInt32
        {
            get { return m_value.Value; }
            set {
                m_value.Value = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ValueInt32));
                OnPropertyChanged(nameof(ValueSingle));
                OnPropertyChanged(nameof(ValueBoolean));
            }
        }

        public int ValueInt32
        {
            get { return (int) m_value.Value; }
            set {
                unchecked {
                    m_value.Value = (uint) value;
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
                return BitConverter.ToSingle(BitConverter.GetBytes(m_value.Value), 0);
            }
            set {
                m_value.Value = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
                OnPropertyChanged();
                OnPropertyChanged(nameof(ValueUInt32));
                OnPropertyChanged(nameof(ValueInt32));
                OnPropertyChanged(nameof(ValueBoolean));
            }
        }

        public bool ValueBoolean
        {
            get { return m_value.Value != 0; }
            set {
                m_value.Value = value ? 1U : 0U;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ValueUInt32));
                OnPropertyChanged(nameof(ValueInt32));
                OnPropertyChanged(nameof(ValueSingle));
            }
        }

        public override string ToString()
        {
            return string.Format("{0} = {1}, {2} = {3}, {4} = {5}, {6} = {7}, {8} = {9}",
                nameof(Name), Name,
                nameof(ValueUInt32), ValueUInt32,
                nameof(ValueInt32), ValueInt32,
                nameof(ValueSingle), ValueSingle,
                nameof(ValueBoolean), ValueBoolean);
        }
    }
}
