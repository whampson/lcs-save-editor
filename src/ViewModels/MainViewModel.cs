#region License
/* Copyright(c) 2016-2018 Wes Hampson
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

using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using WHampson.LcsSaveEditor.Helpers;
using WHampson.LcsSaveEditor.Models;
using WHampson.LcsSaveEditor.Properties;

namespace WHampson.LcsSaveEditor.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _statusText;
        private SaveDataFile _gameState;
        private GamePlatform _fileType;
        private string _filePath;
        private bool _isEditingFile;
        private bool _isFileModified;
        private int _selectedTabIndex;

        public MainViewModel()
        {
            _statusText = Resources.NoFileLoadedMessage;
            Tabs = new ObservableCollection<PageViewModelBase>();
            ReloadTabs();
        }

        public string StatusText
        {
            get { return _statusText; }
            set { _statusText = value; OnPropertyChanged(); }
        }

        public SaveDataFile GameState
        {
            get { return _gameState; }
            set { _gameState = value; OnPropertyChanged(); }
        }

        public GamePlatform FileType
        {
            get { return _fileType; }
            set { _fileType = value; OnPropertyChanged(); }
        }

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; OnPropertyChanged(); }
        }

        public bool IsEditingFile
        {
            get { return _isEditingFile; }
            set { _isEditingFile = value; OnPropertyChanged(); }
        }

        public bool IsFileModified
        {
            get { return _isFileModified; }
            set { _isFileModified = value; OnPropertyChanged(); }
        }

        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set { _selectedTabIndex = value; OnPropertyChanged(); }
        }

        public ObservableCollection<PageViewModelBase> Tabs
        {
            get;
        }

        private void ReloadTabs()
        {
            Tabs.Clear();

            if (IsEditingFile) {
                Tabs.Add(new WeaponsViewModel(GameState));
            }
            else {
                Tabs.Add(new StartupPageViewModel());
            }

            SelectedTabIndex = 0;
        }

        private void OpenFileFromPath(string path)
        {
            SaveDataFile data;
            try {
                data = SaveDataFile.Load(path);
            }
            catch (InvalidDataException e) {
                OnFileLoadFailed(e);
                return;
            }
            catch (NotSupportedException e) {
                OnFileLoadFailed(e);
                return;
            }
            catch (IOException e) {
                OnFileLoadFailed(e);
                return;
            }
            catch (UnauthorizedAccessException e) {
                OnFileLoadFailed(e);
                return;
            }

            GameState = data;
            FileType = data.FileType;
            FilePath = path;
            IsEditingFile = true;
            StatusText = Resources.FileLoadSuccessMessage;
            ReloadTabs();
        }

        private void SaveFileToPath(string path)
        {
            try {
                GameState.Store(path);
            }
            catch (IOException e) {
                OnFileSaveFailed(e);
                return;
            }
            catch (UnauthorizedAccessException e) {
                OnFileSaveFailed(e);
                return;
            }
            
            IsFileModified = false;
            StatusText = Resources.FileSaveSuccessMessage;
        }        

        private void OnFileLoadFailed(Exception e)
        {
            ShowErrorDialog(e.Message);
            StatusText = Resources.FileLoadFailureMessage;
            Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);       // TODO: logger
        }

        private void OnFileSaveFailed(Exception e)
        {
            ShowErrorDialog(e.Message);
            StatusText = Resources.FileSaveFailureMessage;
            Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);       // TODO: logger
        }

        private void ShowErrorDialog(string text)
        {
            OnMessageBoxRequested(new MessageBoxEventArgs(
                null,
                text,
                Resources.ErrorDialogCaption,
                MessageBoxButton.OK,
                MessageBoxImage.Error));
        }

        private void ShowSavePrompt(Action<MessageBoxResult> resultAction)
        {
            OnMessageBoxRequested(new MessageBoxEventArgs(
                resultAction,
                Resources.FileSavePromptDialogMessage,
                Resources.FileSavePromptDialogCaption,
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question,
                MessageBoxResult.Yes));
        }

        #region Commands
        public ICommand OpenFile
        {
            get {
                return new RelayCommand(OpenFile_Execute);
            }
        }

        private void OpenFile_Execute()
        {
            if (IsEditingFile) {
                CloseFile.Execute(null);
            }

            // Don't continue if user cancelled
            if (IsEditingFile) {
                return;
            }

            // TODO: this should be invoked by the view
            OpenFileDialog diag = new OpenFileDialog();
            bool? fileSelected = diag.ShowDialog();

            if (fileSelected == null || fileSelected == false) {
                return;
            }

            OpenFileFromPath(diag.FileName);
        }

        public ICommand CloseFile
        {
            get {
                return new RelayCommand(CloseFile_Execute, CloseFile_CanExecute);
            }
        }

        private bool CloseFile_CanExecute()
        {
            return IsEditingFile;
        }

        private void CloseFile_Execute()
        {
            if (IsFileModified) {
                ShowSavePrompt(DecideSaveAndCloseFile);
            }
            else {
                DoFileClose();
            }
        }

        private void DecideSaveAndCloseFile(MessageBoxResult dialogResult)
        {
            switch (dialogResult) {
                case MessageBoxResult.Yes:
                    SaveFile.Execute(null);
                    DoFileClose();
                    break;
                case MessageBoxResult.No:
                    DoFileClose();
                    break;
            }
        }

        private void DoFileClose()
        {
            GameState = null;
            FilePath = null;
            IsEditingFile = false;
            ReloadTabs();
            StatusText = Resources.NoFileLoadedMessage;
            IsFileModified = false;
        }

        public ICommand ReloadFile
        {
            get {
                return new RelayCommand(ReloadFile_Execute, ReloadFile_CanExecute);
            }
        }

        private bool ReloadFile_CanExecute()
        {
            return IsEditingFile;
        }

        private void ReloadFile_Execute()
        {
            string path = FilePath;
            CloseFile.Execute(null);

            if (!IsEditingFile) {
                // Only reload if user didn't cancel
                OpenFileFromPath(path);
            }
        }

        public ICommand SaveFile
        {
            get {
                return new RelayCommand(SaveFile_Execute, SaveFile_CanExecute);
            }
        }

        private bool SaveFile_CanExecute()
        {
            return IsEditingFile;
        }

        private void SaveFile_Execute()
        {
            OnMessageBoxRequested(new MessageBoxEventArgs(null, "Saving File!"));
            SaveFileToPath(FilePath);
        }

        public ICommand SaveFileAs
        {
            get {
                return new RelayCommand(SaveFileAs_Execute, SaveFileAs_CanExecute);
            }
        }

        private bool SaveFileAs_CanExecute()
        {
            return IsEditingFile;
        }

        private void SaveFileAs_Execute()
        {
            // TODO: this should be invoked by the view
            SaveFileDialog diag = new SaveFileDialog
            {
                Filter = "All Files (*.*)|*.*"
            };
            bool? fileSelected = diag.ShowDialog();

            if (fileSelected == null || fileSelected == false) {
                return;
            }

            FilePath = diag.FileName;
            SaveFile.Execute(null);
        }

        public ICommand ExitApplication
        {
            get {
                return new RelayCommand(ExitApplication_Execute);
            }
        }

        private void ExitApplication_Execute()
        {
            Application.Current.MainWindow.Close();
        }

        public ICommand AboutApplication
        {
            get {
                return new RelayCommand(AboutApplication_Execute);
            }
        }

        private void AboutApplication_Execute()
        {
            MessageBox.Show("AboutApplication");
        }
        #endregion
    }
}
