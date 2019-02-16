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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security;
using System.Windows;
using System.Windows.Input;

namespace LcsSaveEditor.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public ICommand OpenFileCommand
        {
            get {
                return new RelayCommand<Action<bool?, FileDialogEventArgs>>(
                    (x) => ShowOpenFileDialog(
                        FileDialog_ResultAction,
                        filter: FrontendResources.FileFilter_Gta,
                        initialDirectory: Settings.Current.SaveDataFileDialogDirectory));
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
                    (x) => ShowSaveFileDialog(
                        FileDialog_ResultAction,
                        filter: FrontendResources.FileFilter_Gta,
                        initialDirectory: Settings.Current.SaveDataFileDialogDirectory),
                    (x) => IsFileOpen);
            }
        }

        public ICommand ReloadCommand
        {
            get {
                return new RelayCommand<Action<string>>(
                    (x) => ReloadFile(),
                    (x) => IsFileOpen);
            }
        }

        public ICommand ExitAppCommand
        {
            get { return new RelayCommand(ExitApp); }
        }

        public ICommand ShowLogWindowCommand
        {
            get { return new RelayCommand(ShowLogWindow); }
        }

        public ICommand ShowAboutDialogCommand
        {
            get { return new RelayCommand(ShowAboutDialog); }
        }

        public void ShowErrorDialog(string message,
            string title = null,
            Exception exception = null)
        {
            string msg;
            if (exception != null) {
                msg = string.Format("{0}\n\n{1}: {2}", message, exception.GetType().Name, exception.Message);
            }
            else {
                msg = message;
            }

            OnMessageBoxRequested(new MessageBoxEventArgs(
                msg,
                title: title ?? FrontendResources.Common_Error,
                icon: MessageBoxImage.Error));
        }

        public void ShowOpenFileDialog(Action<bool?, FileDialogEventArgs> resultAction,
            string title = null,
            string fileName = null,
            string filter = null,
            string initialDirectory = null)
        {
            OnFileDialogRequested(new FileDialogEventArgs(
                FileDialogType.OpenDialog,
                fileName: fileName,
                filter: filter,
                initialDirectory: initialDirectory,
                title: title ?? FrontendResources.Common_Open,
                resultAction: resultAction));
        }

        public void ShowSaveFileDialog(Action<bool?, FileDialogEventArgs> resultAction,
            string title = null,
            string fileName = null,
            string filter = null,
            string initialDirectory = null)
        {
            OnFileDialogRequested(new FileDialogEventArgs(
                FileDialogType.SaveDialog,
                fileName: fileName,
                filter: filter,
                initialDirectory: initialDirectory,
                title: title ?? FrontendResources.Common_SaveAs,
                resultAction: resultAction));
        }

        private void ShowLogWindow()
        {
            OnLogWindowRequested();
        }

        private void ShowAboutDialog()
        {
            // TODO: invoke custom dialog using AboutDialogRequested event

            OnMessageBoxRequested(new MessageBoxEventArgs(
                "(placeholder)",
                title: FrontendResources.About_Window_Title,
                icon: MessageBoxImage.Information));
        }

        private void ShowCloseFilePrompt()
        {
            OnMessageBoxRequested(new MessageBoxEventArgs(
                FrontendResources.Main_DialogText_SaveBeforeClose,
                title: FrontendResources.Main_DialogTitle_SaveBeforeClose,
                buttons: MessageBoxButton.YesNoCancel,
                icon: MessageBoxImage.Question,
                defaultResult: MessageBoxResult.Yes,
                resultAction: CloseFilePrompt_ResultAction));
        }

        private void ShowReloadConfirmation()
        {
            OnMessageBoxRequested(new MessageBoxEventArgs(
                FrontendResources.Main_DialogText_ConfirmReload,
                title: FrontendResources.Main_DialogTitle_ConfirmReload,
                buttons: MessageBoxButton.YesNo,
                icon: MessageBoxImage.Question,
                defaultResult: MessageBoxResult.No,
                resultAction: ReloadConfirmation_ResultAction));
        }

        private SaveData LoadSaveData(string path)
        {
            Action<string, string, Exception> errorHandler = (text, title, ex) =>
            {
                Logger.Error(CommonResources.Error_LoadFail);
                Logger.Error("({0})", ex.Message);
                ShowErrorDialog(text, title: title, exception: ex);
            };

            SaveData saveData = null;
            try {
                saveData = SaveData.Load(path);
            }
            catch (InvalidDataException ex) {
                errorHandler(
                    FrontendResources.Main_DialogText_InvalidGtaData,
                    FrontendResources.Main_DialogTitle_InvalidGtaData,
                    ex);
            }
            catch (IOException ex) {
                errorHandler(
                    FrontendResources.Main_DialogText_FileLoadFail,
                    FrontendResources.Main_DialogTitle_FileLoadFail,
                    ex);
            }
            catch(SecurityException ex) {
                errorHandler(
                    FrontendResources.Main_DialogText_FileLoadFail,
                    FrontendResources.Main_DialogTitle_FileLoadFail,
                    ex);
            }

            return saveData;
        }

        private bool WriteSaveData(SaveData saveData, string path)
        {
            Action<string, string, Exception> errorHandler = (text, title, ex) =>
            {
                Logger.Error(CommonResources.Error_SaveFail);
                Logger.Error("({0})", ex.Message);
                ShowErrorDialog(text, title: title, exception: ex);
            };

            bool result = false;

            try {
                saveData.Store(path);
                result = true;
            }
            catch (IOException ex) {
                errorHandler(
                    FrontendResources.Main_DialogText_FileSaveFail,
                    FrontendResources.Main_DialogTitle_FileSaveFail,
                    ex);
            }
            catch (SecurityException ex) {
                errorHandler(
                    FrontendResources.Main_DialogText_FileSaveFail,
                    FrontendResources.Main_DialogTitle_FileSaveFail,
                    ex);
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
                StatusText = CommonResources.Error_LoadFail;
                return;
            }
            StatusText = CommonResources.Info_LoadSuccess;

            CurrentSaveData = saveData;
            CurrentSaveData.PropertyChanged += CurrentSaveData_PropertyChanged;

            AddRecentFile(path);
            ReloadTabs();

            WindowTitle = string.Format("{0} - [{1}]", FrontendResources.Main_Window_Title, path);
        }

        private void SaveFile(string path)
        {
            MostRecentFilePath = path;

            bool result = WriteSaveData(CurrentSaveData, path);
            if (!result) {
                StatusText = CommonResources.Error_SaveFail;
                return;
            }
            StatusText = CommonResources.Info_SaveSuccess;

            IsFileModified = false;
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
            Logger.Info(CommonResources.Info_FileClosed);

            ReloadTabs();

            IsFileModified = false;
            StatusText = FrontendResources.Main_StatusText_NoFileLoaded;
            WindowTitle = FrontendResources.Main_Window_Title;
        }

        private void ReloadFile()
        {
            if (IsFileModified) {
                ShowReloadConfirmation();
            }
            else {
                DoReloadFile();
            }
        }

        private void DoReloadFile()
        {
            DoCloseFile();
            OpenFile(MostRecentFilePath);
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
                Tabs.Add(new WeaponsViewModel(this, CurrentSaveData));
                Tabs.Add(new GlobalVariablesViewModel(this, CurrentSaveData));
                SelectedTabIndex = 0;
            }
        }

        private void AddRecentFile(string path)
        {
            // Add a new item to the head of the list
            m_recentFiles.Add(new RecentlyAccessedFile
            {
                Path = path,
                Command = new RelayCommand<string>(dummy => OpenFile(path))
            });
        }

        public void LoadRecentFiles()
        {
            // Not using AddRecentFile() so we can preserve the order!
            m_recentFiles.Items = new ObservableCollection<RecentlyAccessedFile>(
                Settings.Current.RecentFiles.Select(x => new RecentlyAccessedFile
                {
                    Path = x,
                    Command = new RelayCommand<string>(dummy => OpenFile(x))
                }).Take(m_recentFiles.Capacity).ToList());
        }

        public void SaveRecentFiles()
        {
            Settings.Current.RecentFiles = m_recentFiles.Items.Select(x => x.Path).ToList();
        }
    }
}
