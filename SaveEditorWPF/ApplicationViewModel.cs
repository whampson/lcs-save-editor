using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SaveEditorWPF
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool) value;
        }
    }

    public class ApplicationViewModel : ObservableObject
    {
        private bool _isEditingFile;

        public ApplicationViewModel()
        {
            _isEditingFile = false;
        }

        public bool IsEditingFile
        {
            get { return _isEditingFile; }
            set { _isEditingFile = value; OnPropertyChanged(); }
        }

        private void RefreshTabs()
        {
           
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
            MessageBox.Show("Close");
            IsEditingFile = false;
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
