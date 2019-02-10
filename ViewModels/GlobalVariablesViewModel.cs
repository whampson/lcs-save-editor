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
        private FullyObservableCollection<GlobalVariable> m_namedVariables;
        private bool m_isShowingColumnInt32;
        private bool m_isShowingColumnUInt32;
        private bool m_isShowingColumnFloat;
        private bool m_isShowingColumnBoolean;

        public GlobalVariablesViewModel()
            : base(Strings.PageHeaderGlobalVariables)
        {
            m_namedVariables = new FullyObservableCollection<GlobalVariable>();
            m_isShowingColumnInt32 = true;
            m_isShowingColumnUInt32 = false;
            m_isShowingColumnFloat = true;
            m_isShowingColumnBoolean = true;
        }

        public GlobalVariablesViewModel(SaveData saveData)
            : this()
        {
            foreach (ScriptVariable v in saveData.Scripts.GlobalVariables) {
                m_namedVariables.Add(new GlobalVariable(v));
            }
            saveData.Scripts.GlobalVariables.CollectionChanged += GlobalVariables_CollectionChanged;
        }

        public FullyObservableCollection<GlobalVariable> GlobalVariables
        {
            get { return m_namedVariables; }
            set { m_namedVariables = value; OnPropertyChanged(); }
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
