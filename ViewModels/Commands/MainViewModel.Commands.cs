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
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace LcsSaveEditor.ViewModels
{
    public partial class MainViewModel
    {
        public ICommand OpenFileCommand
        {
            get {
                return new RelayCommand<Action<bool?, FileDialogEventArgs>>(
                    (x) => ShowOpenFileDialog(FileDialog_ResultAction));
            }
        }

        public ICommand CloseFileCommand
        {
            get { return new RelayCommand(CloseFile, () => IsFileOpen); }
        }

        public ICommand SaveFileCommand
        {
            get {
                return new RelayCommand<Action<string>>(
                    (x) => SaveFile(MostRecentFilePath),
                    (x) => IsFileOpen);
            }
        }

        public ICommand SaveFileAsCommand
        {
            get {
                return new RelayCommand<Action<bool?, FileDialogEventArgs>>(
                    (x) => ShowOpenFileDialog(FileDialog_ResultAction),
                    (x) => IsFileOpen);
            }
        }

        public ICommand ExitAppCommand
        {
            get { return new RelayCommand(ExitApp); }
        }

        public ICommand ShowAboutDialogCommand
        {
            get { return new RelayCommand(ShowAboutDialog); }
        }

        private SaveData LoadSaveData(string path)
        {
            SaveData saveData = null;
            try {
                saveData = SaveData.Load(path);
            }
            catch (IOException ex) {
                ShowErrorDialog(ex.Message);
            }
            catch (InvalidDataException ex) {
                ShowErrorDialog(ex.Message);
            }

            return saveData;
        }

        private bool WriteSaveData(SaveData saveData, string path)
        {
            bool result = false;

            try {
                saveData.Store(path);
                result = true;
            }
            catch (IOException ex) {
                ShowErrorDialog(ex.Message);
            }
            catch (InvalidDataException ex) {
                ShowErrorDialog(ex.Message);
            }

            return result;
        }

        private void OpenFile(string path)
        {
            MostRecentFilePath = path;

            // Close current file
            if (IsFileOpen) {
                CloseFile();
            }

            // Cancel if user didn't want to close current file
            if (IsFileOpen) {
                return;
            }

            // Load new file
            SaveData saveData = LoadSaveData(path);
            if (saveData == null) {
                StatusText = Strings.StatusTextFileOpenFail;
                return;
            }

            CurrentSaveData = saveData;
            CurrentSaveData.PropertyChanged += CurrentSaveData_PropertyChanged;

            ReloadTabs();

            WindowTitle = string.Format("{0} - [{1}]", Strings.AppName, path);
            StatusText = Strings.StatusTextFileOpenSuccess;
        }

        private void SaveFile(string path)
        {
            MostRecentFilePath = path;

            bool result = WriteSaveData(CurrentSaveData, path);
            if (!result) {
                StatusText = Strings.StatusTextFileSaveFailed;
                return;
            }

            IsFileModified = false;
            StatusText = Strings.StatusTextFileSaveSuccess;
        }

        private void CloseFile()
        {
            if (IsFileModified) {
                ShowCloseFilePrompt();
            }
            else {
                DoCloseFile();
            }
        }

        private void DoCloseFile()
        {
            CurrentSaveData.PropertyChanged -= CurrentSaveData_PropertyChanged;
            CurrentSaveData = null;

            ReloadTabs();

            IsFileModified = false;
            WindowTitle = Strings.AppName;
            StatusText = Strings.StatusTextFileNotLoaded;
        }

        private void ExitApp()
        {
            Application.Current.MainWindow.Close();
        }

        private void ReloadTabs()
        {
            Tabs.Clear();
            SelectedTabIndex = -1;

            if (IsFileOpen) {
                Tabs.Add(new WeaponsViewModel(CurrentSaveData));
                Tabs.Add(new GlobalVariablesViewModel(CurrentSaveData));
                SelectedTabIndex = 0;
            }
        }

        private void ShowErrorDialog(string message)
        {
            OnMessageBoxRequested(new MessageBoxEventArgs(
                message,
                Strings.DialogTitleError,
                icon: MessageBoxImage.Error));
        }

        private void ShowAboutDialog()
        {
            // TODO: invoke custom dialog using AboutDialogRequested event

            OnMessageBoxRequested(new MessageBoxEventArgs(
                "(placeholder)",
                Strings.DialogTitleAbout,
                icon: MessageBoxImage.Information));
        }

        private void ShowCloseFilePrompt()
        {
            OnMessageBoxRequested(new MessageBoxEventArgs(
                Strings.DialogTextSaveChangesPrompt,
                Strings.DialogTitleSaveChangesPrompt,
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question,
                MessageBoxResult.Yes,
                resultAction: FileClosePrompt_ResultAction));
        }

        private void ShowOpenFileDialog(Action<bool?, FileDialogEventArgs> resultAction)
        {
            OnFileDialogRequested(new FileDialogEventArgs(
                FileDialogType.OpenDialog,
                filter: Strings.FileFilterSaveData,
                title: Strings.DialogTitleOpenFile,
                resultAction: resultAction));
        }

        private void ShowSaveFileDialog(Action<bool?, FileDialogEventArgs> resultAction)
        {
            OnFileDialogRequested(new FileDialogEventArgs(
                FileDialogType.SaveDialog,
                fileName: Path.GetFileName(MostRecentFilePath),
                filter: Strings.FileFilterSaveData,
                title: Strings.DialogTitleSaveFileAs,
                resultAction: resultAction));
        }
    }
}
