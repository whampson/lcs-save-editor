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

using LcsSaveEditor.Helpers;
using LcsSaveEditor.Infrastructure;
using LcsSaveEditor.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace LcsSaveEditor.ViewModels
{
    public partial class GlobalVariablesViewModel : PageViewModelBase
    {
        private const string DefaultSymbolsFileName = "CustomVariables.ini";

        public ICommand InsertRowAboveCommand
        {
            get { return new RelayCommand(InsertRowAbove, () => SelectedRow != -1); }
        }

        public ICommand InsertRowBelowCommand
        {
            get { return new RelayCommand(InsertRowBelow, () => SelectedRow != -1); }
        }

        public ICommand DeleteRowCommand
        {
            get { return new RelayCommand(DeleteRow, () => SelectedRow != -1); }
        }

        public ICommand LoadSymbolsCommand
        {
            get {
                return new RelayCommand<Action<bool?, FileDialogEventArgs>>(
                    (x) => MainViewModel.ShowOpenFileDialog(
                        FileDialog_ResultAction,
                        fileName: DefaultSymbolsFileName,
                        filter: Strings.FileFilterIni));
                // TODO: initialDirectory from settings
            }
        }

        public ICommand SaveSymbolsCommand
        {
            get {
                return new RelayCommand<Action<bool?, FileDialogEventArgs>>(
                    (x) => MainViewModel.ShowSaveFileDialog(
                        FileDialog_ResultAction,
                        fileName: DefaultSymbolsFileName,
                        filter: Strings.FileFilterIni));
                // TODO: initialDirectory from settings
            }
        }

        private void LoadCustomVariables(string path)
        {
            // TODO: exception handling

            Dictionary<string, string> dict = IniHelper.ReadAllKeys(path);
            for (int i = 0; i < m_namedGlobalVariables.Count; i++) {
                if (dict.TryGetValue(i.ToString(), out string sym)) {
                    m_namedGlobalVariables[i].Name = sym;
                }
            }

            MainViewModel.StatusText = string.Format(Strings.StatusTextSymbolsLoaded, path);
        }

        private void SaveCustomVariables(string path)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 0; i < m_namedGlobalVariables.Count; i++) {
                string sym = m_namedGlobalVariables[i].Name;
                if (!string.IsNullOrWhiteSpace(sym)) {
                    dict[i.ToString()] = sym;
                }
            }

            string platformName = GamePlatformHelper.GetPlatformName(MainViewModel.CurrentSaveData.FileType);

            IniHelper.WriteComment(path, string.Format(Strings.CustomVariablesCompatibilityText, platformName));
            IniHelper.AppendComment(path, Strings.CustomVariablesGeneratedByText + "\n");
            IniHelper.AppendAllKeys(path, dict);

            MainViewModel.StatusText = string.Format(Strings.StatusTextSymbolsSaved, path);
        }

        private void InsertRowAbove()
        {
            m_namedGlobalVariables.Insert(SelectedRow, new NamedScriptVariable());
        }

        private void InsertRowBelow()
        {
            m_namedGlobalVariables.Insert(SelectedRow + 1, new NamedScriptVariable());
        }

        private void DeleteRow()
        {
            m_namedGlobalVariables.RemoveAt(SelectedRow);
        }
    }
}
