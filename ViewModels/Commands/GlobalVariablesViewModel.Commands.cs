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

using LcsSaveEditor.DataTypes;
using LcsSaveEditor.Helpers;
using LcsSaveEditor.Infrastructure;
using LcsSaveEditor.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
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
                        title: FrontendResources.GlobalVariables_DialogTitle_LoadSymbols,
                        filter: FrontendResources.FileFilter_Ini,
                        initialDirectory: Settings.Current.CustomVariablesFileDialogDirectory));
            }
        }

        public ICommand SaveSymbolsCommand
        {
            get {
                return new RelayCommand<Action<bool?, FileDialogEventArgs>>(
                    (x) => MainViewModel.ShowSaveFileDialog(
                        FileDialog_ResultAction,
                        title: FrontendResources.GlobalVariables_DialogTitle_SaveSymbols,
                        fileName: DefaultSymbolsFileName,
                        filter: FrontendResources.FileFilter_Ini,
                        initialDirectory: Settings.Current.CustomVariablesFileDialogDirectory));
            }
        }

        public void AutoLoadCustomVariables()
        {
            GamePlatform fileType = MainViewModel.CurrentSaveData.FileType;
            string path = Settings.Current.GetCustomVariablesFile(fileType);
            
            if (path != null) {
                LoadCustomVariables(path);
            }
        }

        private void LoadCustomVariables(string path)
        {
            GamePlatform fileType = MainViewModel.CurrentSaveData.FileType;
            Action<Exception> errorHandler = (ex) =>
            {
                Settings.Current.SetCustomVariablesFile(fileType, null);
                Logger.Error(CommonResources.Error_SymbolsLoadFail);
                Logger.Error("({0})", ex.Message);
                MainViewModel.ShowErrorDialog(
                    FrontendResources.GlobalVariables_DialogText_SymbolsLoadFail,
                    title: FrontendResources.GlobalVariables_DialogTitle_SymbolsLoadFail,
                    exception: ex);
                MainViewModel.StatusText = CommonResources.Error_SymbolsLoadFail;
            };

            try {
                Dictionary<string, string> dict = IniHelper.ReadAllKeys(path);
                for (int i = 0; i < m_namedGlobalVariables.Count; i++) {
                    if (dict.TryGetValue(i.ToString(), out string sym)) {
                        m_namedGlobalVariables[i].Name = sym;
                    }
                }

                Settings.Current.SetCustomVariablesFile(fileType, path);

                Logger.Info(CommonResources.Info_SymbolsLoadSuccess);
                MainViewModel.StatusText = CommonResources.Info_SymbolsLoadSuccess;
            }
            catch (IOException ex) {
                errorHandler(ex);
            }
            catch (SecurityException ex) {
                errorHandler(ex);
            }
            catch (UnauthorizedAccessException ex) {
                errorHandler(ex);
            }
        }

        private void SaveCustomVariables(string path)
        {
            GamePlatform fileType = MainViewModel.CurrentSaveData.FileType;
            string platformName = GamePlatformHelper.GetPlatformName(fileType);
            Action<Exception> errorHandler = (ex) =>
            {
                Settings.Current.SetCustomVariablesFile(fileType, null);
                Logger.Error(CommonResources.Error_SymbolsSaveFail);
                Logger.Error("({0})", ex.Message);
                MainViewModel.ShowErrorDialog(
                    FrontendResources.GlobalVariables_DialogText_SymbolsSaveFail,
                    title: FrontendResources.GlobalVariables_DialogTitle_SymbolsSaveFail,
                    exception: ex);
                MainViewModel.StatusText = CommonResources.Error_SymbolsSaveFail;
            };

            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 0; i < m_namedGlobalVariables.Count; i++) {
                string sym = m_namedGlobalVariables[i].Name;
                if (!string.IsNullOrWhiteSpace(sym)) {
                    dict[i.ToString()] = sym;
                }
            }

            try {
                IniHelper.WriteComment(path, string.Format(FrontendResources.GlobalVariables_Ini_Compatibility, platformName));
                IniHelper.AppendComment(path, FrontendResources.GlobalVariables_Ini_GeneratedBy + "\n");
                IniHelper.AppendAllKeys(path, dict);

                Settings.Current.SetCustomVariablesFile(fileType, path);

                Logger.Info(CommonResources.Info_SymbolsSaveSuccess);
                MainViewModel.StatusText = CommonResources.Info_SymbolsSaveSuccess;
            }
            catch (IOException ex) {
                errorHandler(ex);
            }
            catch (SecurityException ex) {
                errorHandler(ex);
            }
            catch (UnauthorizedAccessException ex) {
                errorHandler(ex);
            }
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
