using LCSSaveEditor.GUI.ViewModels;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool m_initialized;
        private bool m_initializing;
        private LogWindow m_logWindow;
        
        public ViewModels.MainWindow ViewModel
        {
            get { return (ViewModels.MainWindow) DataContext; }
            set { DataContext = value; }
        }

        public MainWindow()
        {
            m_initializing = true;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_initialized) return;

            ViewModel.Initialize();
            ViewModel.SettingsWindowRequest += ViewModel_SettingsWindowRequest;
            ViewModel.LogWindowRequest += ViewModel_LogWindowRequest;
            ViewModel.AboutWindowRequest += ViewModel_AboutWindowRequest;
            ViewModel.MessageBoxRequest += ViewModel_MessageBoxRequest;
            ViewModel.FileDialogRequest += ViewModel_FileDialogRequest;
            ViewModel.FolderDialogRequest += ViewModel_FolderDialogRequest;

            m_initializing = false;
            m_initialized = true;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (ViewModel.IsDirty)
            {
                e.Cancel = true;
                ViewModel.PromptSaveChanges(ExitAppDialog_Callback);
                return;
            }

            ViewModel.Shutdown();
            ViewModel.SettingsWindowRequest -= ViewModel_SettingsWindowRequest;
            ViewModel.LogWindowRequest -= ViewModel_LogWindowRequest;
            ViewModel.AboutWindowRequest -= ViewModel_AboutWindowRequest;
            ViewModel.MessageBoxRequest -= ViewModel_MessageBoxRequest;
            ViewModel.FileDialogRequest -= ViewModel_FileDialogRequest;
            ViewModel.FolderDialogRequest -= ViewModel_FolderDialogRequest;

            if (m_logWindow != null)
            {
                m_logWindow.HideOnClose = false;
                m_logWindow.Close();
            }

            m_initialized = false;
        }


        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 1)
                {
                    ViewModel.ShowError("Multiple files selected. Please select only one file.");
                    return;
                }

                ViewModel.TheSettings.SetLastAccess(files[0]);
                ViewModel.OpenFile(files[0]);
            }
            else
            {
                ViewModel.ShowError("Data format not supported. Please select a file.");
                return;
            }
        }

        private void ViewModel_SettingsWindowRequest(object sender, EventArgs e)
        {
            ViewModel.ShowInfo($"TODO: settings");
        }

        private void ViewModel_LogWindowRequest(object sender, EventArgs e)
        {
            if (m_logWindow != null && m_logWindow.IsVisible)
            {
                m_logWindow.Focus();
                return;
            }

            if (m_logWindow == null)
            {
                m_logWindow = new LogWindow() { Owner = this };
            }
            m_logWindow.Show();
        }

        private void ViewModel_AboutWindowRequest(object sender, EventArgs e)
        {
                ViewModel.ShowInfo(
                    $"{App.Name}\n" +
                    "(C) 2016-2020 Wes Hampson\n" +
                    "\n" +
                   $"Version: {App.InformationalVersion}\n",
                    title: "About");
        }

        private void ViewModel_MessageBoxRequest(object sender, MessageBoxEventArgs e)
        {
            e.Show(this);
        }

        private void ViewModel_FileDialogRequest(object sender, FileDialogEventArgs e)
        {
            e.ShowDialog(this);
        }

        private void ViewModel_FolderDialogRequest(object sender, FileDialogEventArgs e)
        {
            VistaFolderBrowserDialog d = new VistaFolderBrowserDialog();
            bool? r = d.ShowDialog(this);

            e.FileName = d.SelectedPath;
            e.Callback?.Invoke(r, e);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (m_initializing || !(e.OriginalSource is TabControl))
            {
                return;
            }

            foreach (var item in e.AddedItems)
            {
                if (item is TabPageBase page)
                {
                    page.Update();
                }
            }
        }

        private void ExitAppDialog_Callback(MessageBoxResult r)
        {
            if (r != MessageBoxResult.Cancel)
            {
                if (r == MessageBoxResult.Yes)
                {
                    ViewModel.SaveFile();
                }

                ViewModel.ClearDirty();
                ViewModel.CloseFile();
                Application.Current.Shutdown();
            }
        }
    }
}
