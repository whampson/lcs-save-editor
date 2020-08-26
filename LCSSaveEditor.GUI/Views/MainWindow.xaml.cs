using LCSSaveEditor.Core;
using LCSSaveEditor.GUI.Events;
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
        private GlobalsWindow m_globalsWindow;
        private MapWindow m_mapWindow;
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

        private T CreateWindow<T>() where T : Window, new()
        {
            return new T() { Owner = this };
        }

        private void LazyLoadWindow<T>(T window, out T outWindow) where T : Window, new()
        {
            if (window != null && window.IsVisible)
            {
                window.Focus();
                outWindow = window;
                return;
            }
            
            if (window == null)
            {
                window = new T() { Owner = this };
            }
            
            window.Show();
            outWindow = window;
        }

        private void DestroyAllWindows()
        {
            if (m_globalsWindow != null)
            {
                m_globalsWindow.HideOnClose = false;
                m_globalsWindow.Close();
                m_globalsWindow = null;
            }

            if (m_mapWindow != null)
            {
                m_mapWindow.HideOnClose = false;
                m_mapWindow.Close();
                m_mapWindow = null;
            }

            if (m_logWindow != null)
            {
                m_logWindow.HideOnClose = false;
                m_logWindow.Close();
                m_logWindow = null;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_initialized) return;

            ViewModel.Initialize();
            ViewModel.FileDialogRequest += ViewModel_FileDialogRequest;
            ViewModel.FolderDialogRequest += ViewModel_FolderDialogRequest;
            ViewModel.GxtDialogRequest += ViewModel_GxtDialogRequest;
            ViewModel.MessageBoxRequest += ViewModel_MessageBoxRequest;
            ViewModel.SettingsWindowRequest += ViewModel_SettingsWindowRequest;
            ViewModel.GlobalsWindowRequest += ViewModel_GlobalsWindowRequest;
            ViewModel.MapWindowRequest += ViewModel_MapWindowRequest;
            ViewModel.LogWindowRequest += ViewModel_LogWindowRequest;
            ViewModel.AboutWindowRequest += ViewModel_AboutWindowRequest;
            ViewModel.DestroyAllWindowsRequest += ViewModel_DestroyAllWindowsRequest;

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
            ViewModel.FileDialogRequest -= ViewModel_FileDialogRequest;
            ViewModel.FolderDialogRequest -= ViewModel_FolderDialogRequest;
            ViewModel.GxtDialogRequest -= ViewModel_GxtDialogRequest;
            ViewModel.MessageBoxRequest -= ViewModel_MessageBoxRequest;
            ViewModel.SettingsWindowRequest -= ViewModel_SettingsWindowRequest;
            ViewModel.GlobalsWindowRequest -= ViewModel_GlobalsWindowRequest;
            ViewModel.MapWindowRequest -= ViewModel_MapWindowRequest;
            ViewModel.LogWindowRequest -= ViewModel_LogWindowRequest;
            ViewModel.AboutWindowRequest -= ViewModel_AboutWindowRequest;
            ViewModel.DestroyAllWindowsRequest -= ViewModel_DestroyAllWindowsRequest;

            DestroyAllWindows();

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

        private void Window_Activated(object sender, EventArgs e)
        {
            ViewModel.CheckForExternalChanges();
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

        private void ViewModel_GxtDialogRequest(object sender, GxtDialogEventArgs e)
        {
            GxtDialog d = new GxtDialog() { Owner = this };
            d.ViewModel.TableName = e.TableName;
            d.ViewModel.AllowTableSelection = e.AllowTableSelection;

            bool? r = d.ShowDialog();
            e.SelectedKey = d.ViewModel.SelectedItem.Key;
            e.SelectedValue = d.ViewModel.SelectedItem.Value;
            e.Callback?.Invoke(r, e);
        }

        private void ViewModel_MessageBoxRequest(object sender, MessageBoxEventArgs e)
        {
            e.Show(this);
        }

        private void ViewModel_SettingsWindowRequest(object sender, EventArgs e)
        {
            ViewModel.ShowInfo($"TODO: settings");
        }

        private void ViewModel_GlobalsWindowRequest(object sender, EventArgs e)
        {
            LazyLoadWindow(m_globalsWindow, out m_globalsWindow);
        }

        private void ViewModel_MapWindowRequest(object sender, EventArgs e)
        {
            LazyLoadWindow(m_mapWindow, out m_mapWindow);
        }

        private void ViewModel_LogWindowRequest(object sender, EventArgs e)
        {
            LazyLoadWindow(m_logWindow, out m_logWindow);
        }

        private void ViewModel_AboutWindowRequest(object sender, EventArgs e)
        {
                ViewModel.ShowInfo(
                    $"{App.Name}\n" +
                    "(C) 2016-2020 Wes Hampson\n" +
                    "\n" +
                   $"Version: {App.Version}\n",
                    title: "About");
        }

        private void ViewModel_DestroyAllWindowsRequest(object sender, EventArgs e)
        {
            DestroyAllWindows();
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
