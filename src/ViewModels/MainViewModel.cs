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
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WHampson.LcsSaveEditor.Helpers;

namespace WHampson.LcsSaveEditor.ViewModels
{
    public class StartupViewModel : PageViewModel
    {
        public StartupViewModel()
            : base("Welcome")
        { }
    }

    public class WeaponsViewModel : PageViewModel
    {
        public WeaponsViewModel()
            : base("Weapons")
        { }
    }

    public class MainViewModel : ObservableObject
    {
        private bool _isEditingFile;
        private int _selectedTabIndex;

        public MainViewModel()
        {
            _isEditingFile = false;
            _selectedTabIndex = 0;
            Tabs = new ObservableCollection<PageViewModel>();
            RefreshTabs();
        }

        public bool IsEditingFile
        {
            get { return _isEditingFile; }
            set { _isEditingFile = value; OnPropertyChanged(); }
        }

        public ObservableCollection<PageViewModel> Tabs
        {
            get;
        }

        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set { _selectedTabIndex = value; OnPropertyChanged(); }
        }

        private void RefreshTabs()
        {
            Tabs.Clear();

            if (IsEditingFile) {
                Tabs.Add(new WeaponsViewModel());
            }
            else {
                Tabs.Add(new StartupViewModel());
            }

            SelectedTabIndex = 0;
        }

        public ICommand OpenFile
        {
            get {
                return new RelayCommand(OpenFile_Execute);
            }
        }

        private void OpenFile_Execute()
        {
            OpenFileDialog diag = new OpenFileDialog();
            bool? fileSelected = diag.ShowDialog();

            if (fileSelected == true) {
                IsEditingFile = true;
                //LoadFile(diag.FileName);
                RefreshTabs();
            }
        }

        public ICommand FileClose
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
            IsEditingFile = false;
            RefreshTabs();
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
            MessageBox.Show("ReloadFile");
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
            MessageBox.Show("SaveFile");
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
            MessageBox.Show("SaveFileAs");
        }

        public ICommand ExitApplication
        {
            get {
                return new RelayCommand(ExitApplication_Execute);
            }
        }

        private void ExitApplication_Execute()
        {
            // TODO: check if file modified (listen on Exit event?)
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
    }
}
