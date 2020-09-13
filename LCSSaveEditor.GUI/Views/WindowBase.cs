using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using LCSSaveEditor.GUI.Events;
using Ookii.Dialogs.Wpf;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.Views
{
    public class WindowBase : Window
    {
        public bool IsWindowInitialized { get; private set; }
        public bool IsWindowInitializing { get; private set; }

        public ViewModels.WindowBase ViewModel
        {
            get { return (ViewModels.WindowBase) DataContext; }
            set { DataContext = value; }
        }

        public WindowBase()
        {
            IsWindowInitializing = true;
        }

        protected virtual void WindowActivated()
        {
            ViewModel.CheckForExternalChanges();
        }

        protected virtual void WindowLoaded()
        {
            ViewModel.Initialize();
            ViewModel.FileDialogRequest += ViewModel_FileDialogRequest;
            ViewModel.FolderDialogRequest += ViewModel_FolderDialogRequest;
            ViewModel.GxtDialogRequest += ViewModel_GxtDialogRequest;
            ViewModel.MessageBoxRequest += ViewModel_MessageBoxRequest;
        }

        protected virtual void WindowClosing(CancelEventArgs e)
        {
            ViewModel.Shutdown();
            ViewModel.FileDialogRequest -= ViewModel_FileDialogRequest;
            ViewModel.FolderDialogRequest -= ViewModel_FolderDialogRequest;
            ViewModel.GxtDialogRequest -= ViewModel_GxtDialogRequest;
            ViewModel.MessageBoxRequest -= ViewModel_MessageBoxRequest;
        }
        protected void Window_Activated(object sender, EventArgs e)
        {
            WindowActivated();
        }

        protected void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsWindowInitialized) return;

            WindowLoaded();

            IsWindowInitializing = false;
            IsWindowInitialized = true;
        }

        protected void Window_Closing(object sender, CancelEventArgs e)
        {
            WindowClosing(e);

            if (!e.Cancel)
            {
                IsWindowInitialized = false;
            }
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

            if (e.Modal)
            {
                bool? r = d.ShowDialog();
                e.SelectedKey = d.ViewModel.SelectedItem.Key;
                e.SelectedValue = d.ViewModel.SelectedItem.Value;
                e.Callback?.Invoke(r, e);
                return;
            }

            d.Show();
            e.Callback?.Invoke(false, e);
        }

        private void ViewModel_MessageBoxRequest(object sender, MessageBoxEventArgs e)
        {
            e.Show(this);
        }
    }
}
