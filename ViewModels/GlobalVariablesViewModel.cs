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
using LcsSaveEditor.Resources;

namespace LcsSaveEditor.ViewModels
{
    public partial class GlobalVariablesViewModel : PageViewModelBase
    {
        private FullyObservableCollection<ScriptVariable> m_globalVariables;
        private FullyObservableCollection<NamedScriptVariable> m_namedGlobalVariables;
        private int m_selectedRow;
        private bool m_isShowingColumnInt32;
        private bool m_isShowingColumnUInt32;
        private bool m_isShowingColumnFloat;
        private bool m_isShowingColumnBoolean;
        private bool m_suppressGlobalVariablesChanged;
        private bool m_suppressNamedGlobalVariablesChanged;

        public GlobalVariablesViewModel()
            : base(Strings.PageHeaderGlobalVariables)
        {
            m_namedGlobalVariables = new FullyObservableCollection<NamedScriptVariable>();
            m_selectedRow = -1;
            m_isShowingColumnInt32 = true;
            m_isShowingColumnUInt32 = false;
            m_isShowingColumnFloat = true;
            m_isShowingColumnBoolean = true;
        }

        public GlobalVariablesViewModel(SaveData saveData)
            : this()
        {
            m_globalVariables = saveData.Scripts.GlobalVariables;
            foreach (ScriptVariable v in m_globalVariables) {
                m_namedGlobalVariables.Add(new NamedScriptVariable(v));
            }

            m_globalVariables.CollectionChanged += GlobalVariables_CollectionChanged;
            m_namedGlobalVariables.CollectionChanged += NamedGlobalVariables_CollectionChanged;
        }

        public FullyObservableCollection<NamedScriptVariable> NamedGlobalVariables
        {
            get { return m_namedGlobalVariables; }
            set { m_namedGlobalVariables = value; OnPropertyChanged(); }
        }

        public int SelectedRow
        {
            get { return m_selectedRow; }
            set { m_selectedRow = value; OnPropertyChanged(); }
        }

        public bool IsShowingColumnInt32
        {
            get { return m_isShowingColumnInt32; }
            set { m_isShowingColumnInt32 = value; OnPropertyChanged(); }
        }

        public bool IsShowingColumnUInt32
        {
            get { return m_isShowingColumnUInt32; }
            set { m_isShowingColumnUInt32 = value; OnPropertyChanged(); }
        }

        public bool IsShowingColumnFloat
        {
            get { return m_isShowingColumnFloat; }
            set { m_isShowingColumnFloat = value; OnPropertyChanged(); }
        }

        public bool IsShowingColumnBoolean
        {
            get { return m_isShowingColumnBoolean; }
            set { m_isShowingColumnBoolean = value; OnPropertyChanged(); }
        }
    }
}
