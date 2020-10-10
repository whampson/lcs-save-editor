using LCSSaveEditor.Core;
using LCSSaveEditor.GUI.Events;
using LCSSaveEditor.GUI.ViewModels;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    public partial class MainWindow : WindowBase
    {
        private GlobalsWindow m_globalsWindow;
        private MapWindow m_mapWindow;
        private StatsWindow m_statsWindow;
        private LogWindow m_logWindow;
        
        public new ViewModels.MainWindow ViewModel
        {
            get { return (ViewModels.MainWindow) DataContext; }
            set { DataContext = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void WindowLoaded()
        {
            base.WindowLoaded();
            ViewModel.SettingsWindowRequest += ViewModel_SettingsWindowRequest;
            ViewModel.GlobalsWindowRequest += ViewModel_GlobalsWindowRequest;
            ViewModel.MapWindowRequest += ViewModel_MapWindowRequest;
            ViewModel.StatsWindowRequest += ViewModel_StatsWindowRequest;
            ViewModel.LogWindowRequest += ViewModel_LogWindowRequest;
            ViewModel.AboutWindowRequest += ViewModel_AboutWindowRequest;
            ViewModel.UpdateWindowRequest += ViewModel_UpdateWindowRequest;
#if DEBUG
            ViewModel.DestroyAllWindowsRequest += ViewModel_DestroyAllWindowsRequest;
#endif
        }

        protected override void WindowClosing(CancelEventArgs e)
        {
            if (ViewModel.IsDirty)
            {
                e.Cancel = true;
                ViewModel.PromptSaveChanges(ExitAppDialog_Callback);
                return;
            }

            base.WindowClosing(e);
            ViewModel.SettingsWindowRequest -= ViewModel_SettingsWindowRequest;
            ViewModel.GlobalsWindowRequest -= ViewModel_GlobalsWindowRequest;
            ViewModel.MapWindowRequest -= ViewModel_MapWindowRequest;
            ViewModel.LogWindowRequest -= ViewModel_LogWindowRequest;
            ViewModel.AboutWindowRequest -= ViewModel_AboutWindowRequest;
            ViewModel.UpdateWindowRequest -= ViewModel_UpdateWindowRequest;
#if DEBUG
            ViewModel.DestroyAllWindowsRequest -= ViewModel_DestroyAllWindowsRequest;
#endif
            DestroyAllWindows();
        }

        private void LazyLoadWindow<T>(T window, out T outWindow) where T : ChildWindowBase, new()
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
                window.ViewModel.OpenFileRequest += ViewModel.OpenFileRequest_Handler;
                window.ViewModel.SaveFileRequest += ViewModel.SaveFileRequest_Handler;
                window.ViewModel.CloseFileRequest += ViewModel.CloseFileRequest_Handler;
                window.ViewModel.RevertFileRequest += ViewModel.RevertFileRequest_Handler;
            }

            outWindow = window;
            window.Show();
        }

        private void DestroyWindow<T>(T window) where T : ChildWindowBase
        {
            if (window != null)
            {
                window.HideOnClose = false;
                window.Close();
                window.ViewModel.OpenFileRequest -= ViewModel.OpenFileRequest_Handler;
                window.ViewModel.SaveFileRequest -= ViewModel.SaveFileRequest_Handler;
                window.ViewModel.CloseFileRequest -= ViewModel.CloseFileRequest_Handler;
                window.ViewModel.RevertFileRequest -= ViewModel.RevertFileRequest_Handler;
            }
        }

        private void DestroyAllWindows()
        {
            DestroyWindow(m_globalsWindow);
            DestroyWindow(m_mapWindow);
            DestroyWindow(m_logWindow);
            DestroyWindow(m_statsWindow);

            m_globalsWindow = null;
            m_mapWindow = null;
            m_logWindow = null;
            m_statsWindow = null;
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

        private void ViewModel_GlobalsWindowRequest(object sender, EventArgs e)
        {
            LazyLoadWindow(m_globalsWindow, out m_globalsWindow);
        }

        private void ViewModel_MapWindowRequest(object sender, EventArgs e)
        {
            LazyLoadWindow(m_mapWindow, out m_mapWindow);
        }

        private void ViewModel_StatsWindowRequest(object sender, EventArgs e)
        {
            LazyLoadWindow(m_statsWindow, out m_statsWindow);
        }

        private void ViewModel_LogWindowRequest(object sender, EventArgs e)
        {
            LazyLoadWindow(m_logWindow, out m_logWindow);
        }

        private void ViewModel_AboutWindowRequest(object sender, EventArgs e)
        {
            AboutDialog d = new AboutDialog() { Owner = this };
            d.ShowDialog();
        }

        private void ViewModel_UpdateWindowRequest(object sender, UpdateInfoEventArgs e)
        {
            UpdateDialog d = new UpdateDialog() { Owner = this };
            d.SetUpdateInfo(e.UpdateInfo);

            bool? result = d.ShowDialog();
            if (result == true)
            {
                e.InstallCallback?.Invoke(d.GetDownloadedPackagePath());
            }
        }

#if DEBUG
        private void ViewModel_DestroyAllWindowsRequest(object sender, EventArgs e)
        {
            DestroyAllWindows();
        }
#endif

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsWindowInitializing || !(e.OriginalSource is TabControl))
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
                App.ExitApp();
            }
        }
    }
}
