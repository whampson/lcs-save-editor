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
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        public ViewModels.LogWindow ViewModel
        {
            get { return (ViewModels.LogWindow) DataContext; }
            set { DataContext = value; }
        }

        public bool HideOnClose { get; set; }

        public LogWindow()
        {
            InitializeComponent();
            HideOnClose = true;
        }

        private void UpdateTextBox()
        {
            m_textBox.Text = App.LogText;
            m_textBox.ScrollToEnd();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.WindowHideRequest += ViewModel_WindowHideRequest;
            Log.LogEvent += Log_LogEvent;
            UpdateTextBox();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (HideOnClose)
            {
                e.Cancel = true;
                Hide();
                return;
            }

            Log.LogEvent -= Log_LogEvent;
            ViewModel.WindowHideRequest -= ViewModel_WindowHideRequest;
        }

        private void ViewModel_WindowHideRequest(object sender, EventArgs e)
        {
            Hide();
        }

        private void Log_LogEvent(object sender, EventArgs e)
        {
            UpdateTextBox();
        }
    }
}
