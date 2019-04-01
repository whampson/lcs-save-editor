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
using LcsSaveEditor.Models;
using LcsSaveEditor.Resources;
using System;
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

        public ICommand ReloadFileCommand
        {
            get { return new RelayCommand(ReloadFileWithConfirmation, () => IsFileOpen); }
        }

        public ICommand CloseFileCommand
        {
            get { return new RelayCommand(CloseFileWithConfirmation, () => IsFileOpen); }
        }

        public ICommand ExitAppCommand
        {
            get { return new RelayCommand(ExitApp); }
        }

        public ICommand ShowLogWindowCommand
        {
            get { return new RelayCommand(ShowLogWindow); }
        }

        public ICommand ShowAboutWindowCommand
        {
            get { return new RelayCommand(ShowAboutWindow); }
        }

        public void PopulateTabs()
        {
            m_tabs.Add(new StartViewModel(this));
            m_tabs.Add(new GeneralViewModel(this));
            m_tabs.Add(new WeaponsViewModel(this));
            m_tabs.Add(new ScriptsViewModel(this));

            OnTabRefresh(
                TabRefreshTrigger.WindowLoaded,
                GetTabIndex(FrontendResources.Main_Page_Start));
        }

        private void UpdateWindowTitle()
        {
            if (IsFileOpen) {
                WindowTitle = string.Format("{0} {1} - [{2}]",
                    FrontendResources.Main_Window_Title,
                    VersionHelper.GetAppVersionString(),
                    MostRecentFilePath);
            }
            else {
                WindowTitle = string.Format("{0} {1}",
                    FrontendResources.Main_Window_Title,
                    VersionHelper.GetAppVersionString());
            }
        }

        private int GetTabIndex(string tabName)
        {
            PageViewModel item = m_tabs.Where(x => x.Title == tabName).FirstOrDefault();
            if (item == null) {
                return -1;
            }

            return m_tabs.IndexOf(item);
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

        /// <summary>
        /// Displays a modal 'error' dialog.
        /// </summary>
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
                title: title ?? FrontendResources.Common_Label_Error,
                icon: MessageBoxImage.Error));
        }

        /// <summary>
        /// Displays a modal 'open file' dialog.
        /// </summary>
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
                title: title ?? FrontendResources.Common_Label_Open,
                resultAction: resultAction));
        }

        /// <summary>
        /// Displays a modal 'save file' dialog.
        /// </summary>
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
                title: title ?? FrontendResources.Common_Label_SaveAs,
                resultAction: resultAction));
        }

        public void ShowGxtSelectionDialog(Action<bool?, GxtSelectionEventArgs> resultAction)
        {
            OnGxtSelectionDialogRequested(new GxtSelectionEventArgs(resultAction));
        }

        /// <summary>
        /// Opens the log window.
        /// </summary>
        private void ShowLogWindow()
        {
            OnLogWindowRequested();
        }

        /// <summary>
        /// Opens the about window.
        /// </summary>
        private void ShowAboutWindow()
        {
            // TODO: invoke custom dialog using AboutDialogRequested event

            OnMessageBoxRequested(new MessageBoxEventArgs(
                "(placeholder)",
                title: FrontendResources.About_Window_Title,
                icon: MessageBoxImage.Information));
        }

        /// <summary>
        /// Displays a modal dialog asking the user if they want to
        /// save changes before closing the file.
        /// </summary>
        private void ShowSaveBeforeCloseDialog()
        {
            OnMessageBoxRequested(new MessageBoxEventArgs(
                FrontendResources.Main_DialogText_SaveBeforeClose,
                title: FrontendResources.Main_DialogTitle_SaveBeforeClose,
                buttons: MessageBoxButton.YesNoCancel,
                icon: MessageBoxImage.Question,
                defaultResult: MessageBoxResult.Yes,
                resultAction: CloseFilePrompt_ResultAction));
        }

        /// <summary>
        /// Displays a modal dialog asking the user if they want to
        /// discard all unsaved changes and reload the current file.
        /// </summary>
        private void ShowConfirmReloadDialog()
        {
            OnMessageBoxRequested(new MessageBoxEventArgs(
                FrontendResources.Main_DialogText_ConfirmReload,
                title: FrontendResources.Main_DialogTitle_ConfirmReload,
                buttons: MessageBoxButton.YesNo,
                icon: MessageBoxImage.Question,
                defaultResult: MessageBoxResult.No,
                resultAction: ReloadConfirmation_ResultAction));
        }

        /// <summary>
        /// Opens the specified <see cref="SaveData"/> file for editing,
        /// then updates the UI state accordingly.
        /// </summary>
        private void OpenFile(string path)
        {
            if (IsFileOpen) {
                CloseFileWithConfirmation();
            }

            if (IsFileOpen) {
                // Quit if the user opted to cancel closing the file.
                return;
            }

            if (!LoadSaveData(path, out SaveData data)) {
                StatusText = FrontendResources.Main_StatusText_LoadFail;
                return;
            }
            StatusText = FrontendResources.Main_StatusText_LoadSuccess;

            CurrentSaveData = data;
            CurrentSaveData.PropertyChanged += CurrentSaveData_PropertyChanged;

            MostRecentFilePath = path;
            AddRecentFile(path);

            OnDataLoaded(CurrentSaveData);
            OnTabRefresh(
                TabRefreshTrigger.FileLoaded,
                GetTabIndex(FrontendResources.Main_Page_General));

            IsFileModified = false;
            UpdateWindowTitle();
        }

        /// <summary>
        /// Saves changes made to the current <see cref="SaveData"/> file,
        /// then updates the UI accordingly.
        /// </summary>
        private void SaveFile(string path)
        {
            if (!WriteSaveData(CurrentSaveData, path)) {
                StatusText = FrontendResources.Main_StatusText_SaveFail;
                return;
            }
            StatusText = FrontendResources.Main_StatusText_SaveSuccess;

            MostRecentFilePath = path;
            AddRecentFile(path);

            IsFileModified = false;
            UpdateWindowTitle();
        }

        /// <summary>
        /// Prompts the user to confirm reloading the current <see cref="SaveData"/> file,
        /// then reloads the file if the user chose continue.
        /// </summary>
        private void ReloadFileWithConfirmation()
        {
            if (IsFileModified) {
                ShowConfirmReloadDialog();
            }
            else {
                ReloadFile();
            }
        }

        /// <summary>
        /// Reloads the current <see cref="SaveData"/> file, discarding
        /// all unsaved changes, then updates the UI accordingly.
        /// </summary>
        private void ReloadFile()
        {
            // Close the current file, don't refresh tabs
            OnDataClosing(CurrentSaveData);
            CurrentSaveData.PropertyChanged -= CurrentSaveData_PropertyChanged;
            CurrentSaveData = null;
            Logger.Info(CommonResources.Info_FileClosed);

            // Re-open the file
            if (!LoadSaveData(MostRecentFilePath, out SaveData data)) {
                StatusText = FrontendResources.Main_StatusText_ReloadFail;
                UpdateWindowTitle();
                return;
            }
            StatusText = FrontendResources.Main_StatusText_ReloadSuccess;
            CurrentSaveData = data;
            CurrentSaveData.PropertyChanged += CurrentSaveData_PropertyChanged;
            OnDataLoaded(CurrentSaveData);

            IsFileModified = false;
        }

        /// <summary>
        /// Prompts the user to confirm closing the current <see cref="SaveData"/> file,
        /// then closes the file if the user chose continue.
        /// </summary>
        private void CloseFileWithConfirmation()
        {
            if (IsFileModified) {
                ShowSaveBeforeCloseDialog();
            }
            else {
                CloseFile();
            }
        }

        /// <summary>
        /// Closes the current <see cref="SaveData"/> file,
        /// then updates the UI accordingly.
        /// </summary>
        private void CloseFile()
        {
            OnTabRefresh(
                TabRefreshTrigger.FileClosed,
                GetTabIndex(FrontendResources.Main_Page_Start));
            OnDataClosing(CurrentSaveData);

            CurrentSaveData.PropertyChanged -= CurrentSaveData_PropertyChanged;
            CurrentSaveData = null;
            Logger.Info(CommonResources.Info_FileClosed);

            IsFileModified = false;
            StatusText = FrontendResources.Main_StatusText_FileClosed;
            UpdateWindowTitle();
        }

        /// <summary>
        /// Closes the main window, then exits the application.
        /// </summary>
        private void ExitApp()
        {
            Application.Current.MainWindow.Close();
        }

        /// <summary>
        /// Loads a <see cref="SaveData"/> file from the specified path.
        /// If an I/O or security error occurrs, an error dialog will be shown.
        /// The UI state is not changed.
        /// </summary>
        /// <returns>
        /// True if the file was successfully loaded,
        /// False otherwise.
        /// </returns>
        private bool LoadSaveData(string path, out SaveData data)
        {
            void ErrorHandler(string text, string title, Exception ex, bool showException = true)
            {
                Logger.Error(CommonResources.Error_LoadFail);
                Logger.Error("({0})", ex.Message);
                ShowErrorDialog(text, title: title, exception: showException ? ex : null);
            }

            try {
                data = SaveData.Load(path);
                return true;
            }
            catch (InvalidDataException ex) {
                ErrorHandler(
                    FrontendResources.Main_DialogText_InvalidGtaData,
                    FrontendResources.Main_DialogTitle_InvalidGtaData,
                    ex, showException: false);
            }
            catch (IOException ex) {
                ErrorHandler(
                    FrontendResources.Main_DialogText_FileLoadFail,
                    FrontendResources.Main_DialogTitle_FileLoadFail,
                    ex);
            }
            catch (SecurityException ex) {
                ErrorHandler(
                    FrontendResources.Main_DialogText_FileLoadFail,
                    FrontendResources.Main_DialogTitle_FileLoadFail,
                    ex);
            }

            data = null;
            return false;
        }

        /// <summary>
        /// Writes a <see cref="SaveData"/> file to the specified file path.
        /// If an I/O or security error occurrs, an error dialog will be shown.
        /// The UI state is not changed.
        /// </summary>
        /// <returns>
        /// True if the file was successfully written,
        /// False otherwise.
        /// </returns>
        private bool WriteSaveData(SaveData data, string path)
        {
            void ErrorHandler(string text, string title, Exception ex)
            {
                Logger.Error(CommonResources.Error_SaveFail);
                Logger.Error("({0})", ex.Message);
                ShowErrorDialog(text, title: title, exception: ex);
            }

            try {
                data.Store(path);
                return true;
            }
            catch (IOException ex) {
                ErrorHandler(
                    FrontendResources.Main_DialogText_FileSaveFail,
                    FrontendResources.Main_DialogTitle_FileSaveFail,
                    ex);
            }
            catch (SecurityException ex) {
                ErrorHandler(
                    FrontendResources.Main_DialogText_FileSaveFail,
                    FrontendResources.Main_DialogTitle_FileSaveFail,
                    ex);
            }

            return false;
        }
    }
}
