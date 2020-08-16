using GTASaveData.LCS;
using LCSSaveEditor.Core;
using LCSSaveEditor.GUI.Events;
using LCSSaveEditor.GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LCSSaveEditor.GUI.Views
{
    /// <summary>
    /// Interaction logic for GlobalsWindow.xaml
    /// </summary>
    public partial class GlobalsWindow : Window
    {
        public ViewModels.GlobalsWindow ViewModel
        {
            get { return (ViewModels.GlobalsWindow) DataContext; }
            set { DataContext = value; }
        }

        public bool HideOnClose { get; set; }

        public GlobalsWindow()
        {
            InitializeComponent();
            HideOnClose = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
            ViewModel.WindowHideRequest += ViewModel_WindowHideRequest;
            ViewModel.RegisterChangeHandlers();
            ViewModel.UpdateList();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (HideOnClose)
            {
                e.Cancel = true;
                Hide();
                return;
            }

            ViewModel.Shutdown();
            ViewModel.UnregisterChangeHandlers();
            ViewModel.WindowHideRequest -= ViewModel_WindowHideRequest;
        }

        private void ViewModel_WindowHideRequest(object sender, EventArgs e)
        {
            Hide();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool isVisible)
            {
                if (isVisible)
                {
                    ViewModel.UpdateList();
                }
            }
        }

        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            ViewModel.UpdateList();
        }
    }
}
