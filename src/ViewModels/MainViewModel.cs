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
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WHampson.LcsSaveEditor.Helpers;
using WHampson.LcsSaveEditor.Models;

namespace WHampson.LcsSaveEditor.ViewModels
{
    public class MainViewModel : ObservableObject
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
            _statusText = "No file loaded.";
            Tabs = new ObservableCollection<PageViewModel>();
            RefreshTabs();
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

        public ObservableCollection<PageViewModel> Tabs
        {
            get;
        }

        private void RefreshTabs()
        {
            Tabs.Clear();

            if (IsEditingFile) {
                Tabs.Add(new WeaponsViewModel(GameState));
                Tabs.Add(new TestViewModel(GameState));
            }
            else {
                Tabs.Add(new StartupPageViewModel());
            }

            if (Tabs.Count() == 1) {
                SelectedTabIndex = 0;
            }
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

            OpenFileDialog diag = new OpenFileDialog();
            bool? fileSelected = diag.ShowDialog();

            if (fileSelected == null || fileSelected == false) {
                return;
            }

            OpenFileFromPath(diag.FileName);
        }

        private void OpenFileFromPath(string path)
        {
            SaveDataFile data;
            try {
                data = SaveDataFile.Load(path);
            }
            catch (InvalidDataException e) {
                ShowErrorDialog(e.Message);
                StatusText = "File load failed.";
                return;
            }
            catch (NotSupportedException e) {
                ShowErrorDialog(e.Message);
                StatusText = "File load failed.";
                return;
            }

            GameState = data;
            FileType = data.FileType;
            FilePath = path;
            IsEditingFile = true;
            StatusText = "File loaded successfully!";
            RefreshTabs();
        }

        public static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBoxEx.Show("Do you want to save your changes?", "Save", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            }

            GameState = null;
            FilePath = null;
            IsEditingFile = false;
            StatusText = "No file loaded.";
            RefreshTabs();
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
            OpenFileFromPath(path);
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
            GameState.Store(FilePath);
            IsFileModified = false;
            StatusText = "File saved successfully!";
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
            SaveFileDialog diag = new SaveFileDialog();
            diag.Filter = "All Files (*.*)|*.*";
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
            if (IsEditingFile) {
                CloseFile.Execute(null);
            }
            Application.Current.Shutdown();
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
