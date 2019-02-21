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

using LcsSaveEditor.Core;
using LcsSaveEditor.Models;

namespace LcsSaveEditor.ViewModels
{
    /// <summary>
    /// A <see cref="ScriptVariable"/> wrapper that associates a name with a script variable.
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

        public uint Value
        {
            get { return m_value; }
            set { m_value = value; OnPropertyChanged(); }
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(Name)) {
                return Name + " = " + Value;
            }
            return Value.ToString();
        }
    }
}
