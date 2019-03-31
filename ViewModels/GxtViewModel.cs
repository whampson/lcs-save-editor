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
using LcsSaveEditor.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace LcsSaveEditor.ViewModels
{
    public partial class GxtViewModel : ObservableObject
    {
        private Dictionary<string, string> m_gxtTable;
        private KeyValuePair<string, string> m_selectedItem;
        private int m_selectedIndex;

        public GxtViewModel()
        {
            m_gxtTable = GxtHelper.CurrentMainTable;
            m_selectedIndex = -1;
        }

        public Dictionary<string, string> GxtTable
        {
            get { return m_gxtTable; }
            set { m_gxtTable = value; OnPropertyChanged(); }
        }

        public KeyValuePair<string, string> SelectedItem
        {
            get { return m_selectedItem; }
            set { m_selectedItem = value; OnPropertyChanged(); }
        }

        public int SelectedIndex
        {
            get { return m_selectedIndex; }
            set { m_selectedIndex = value; OnPropertyChanged(); }
        }

        public ICommand SelectCommand
        {
            get {
                return new RelayCommand(
                    () => OnDialogCloseRequested(new DialogCloseEventArgs(true)),
                    () => SelectedIndex != -1);
            }
        }

        public ICommand CloseWindowCommand
        {
            get { return new RelayCommand(() => OnDialogCloseRequested(new DialogCloseEventArgs(false))); }
        }

        public event EventHandler<DialogCloseEventArgs> DialogCloseRequested;

        private void OnDialogCloseRequested(DialogCloseEventArgs e)
        {
            DialogCloseRequested?.Invoke(this, e);
        }
    }
}
