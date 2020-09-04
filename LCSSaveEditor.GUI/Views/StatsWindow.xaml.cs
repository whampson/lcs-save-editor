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
using System.Windows.Shapes;

namespace LCSSaveEditor.GUI.Views
{
    /// <summary>
    /// Interaction logic for StatsWindow.xaml
    /// </summary>
    public partial class StatsWindow : Window
    {
        //private bool m_suppressChangeHandlers;

        public ViewModels.StatsWindow ViewModel
        {
            get { return (ViewModels.StatsWindow) DataContext; }
            set { DataContext = value; }
        }

        public bool HideOnClose { get; set; }

        public StatsWindow()
        {
            InitializeComponent();
            HideOnClose = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
            ViewModel.WindowHideRequest += ViewModel_WindowHideRequest;
            ViewModel.RegisterChangeHandlers();
            ViewModel.RefreshStats();
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
                    ViewModel.RefreshStats();
                }
            }
        }
    }
}
