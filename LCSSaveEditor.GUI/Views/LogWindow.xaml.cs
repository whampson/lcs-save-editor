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
    public partial class LogWindow : ChildWindowBase
    {
        public new ViewModels.LogWindow ViewModel
        {
            get { return (ViewModels.LogWindow) DataContext; }
            set { DataContext = value; }
        }

        public LogWindow()
        {
            InitializeComponent();
        }

        protected override void WindowLoaded()
        {
            base.WindowLoaded();
            UpdateTextBox();
            Log.LogEvent += Log_LogEvent;
        }

        protected override void WindowClosing(CancelEventArgs e)
        {
            base.WindowClosing(e);
            Log.LogEvent -= Log_LogEvent;
        }

        private void UpdateTextBox()
        {
            Dispatcher.Invoke(() =>
            {
                m_textBox.Text = App.LogText;
                m_textBox.ScrollToEnd();
            });
        }

        private void Log_LogEvent(object sender, EventArgs e)
        {
            UpdateTextBox();
        }
    }
}
