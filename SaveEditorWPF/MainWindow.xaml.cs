using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SaveEditorWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeState();
            tabControl.Items.Add(new WeaponsPage());
        }

        private bool IsFileLoaded
        {
            get;
            set;
        }

        private bool IsFileChanged
        {
            get;
            set;
        }

        private void InitializeState()
        {
            IsFileLoaded = false;
            IsFileChanged = false;
        }

        private void LoadRecentItems()
        {
            MenuItem noRecentMenuItem = new MenuItem()
            {
                Header = "(no recent items)",
                IsEnabled = false
            };
            openRecentMenuItem.Items.Add(noRecentMenuItem);
        }

        public void OpenFile()
        {

        }

        public void CloseFile()
        {

        }

        public void ReloadFile()
        {

        }

        public void SaveFile()
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRecentItems();
        }

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Open");
        }

        private void Close_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsFileLoaded;
        }

        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Close");
        }

        private void Reload_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsFileLoaded;
        }

        private void Reload_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Reload");
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsFileLoaded;
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Save");
        }

        private void SaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsFileLoaded;
        }

        private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Save As");
        }

        private void Exit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void About_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void About_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("About");
        }
    }

    
}
