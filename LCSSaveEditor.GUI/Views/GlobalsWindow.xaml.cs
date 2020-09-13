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
    public partial class GlobalsWindow : ChildWindowBase
    {
        private bool m_suppressChangeHandlers;

        public new ViewModels.GlobalsWindow ViewModel
        {
            get { return (ViewModels.GlobalsWindow) DataContext; }
            set { DataContext = value; }
        }

        public GlobalsWindow()
        {
            InitializeComponent();
        }

        protected override void WindowLoaded()
        {
            base.WindowLoaded();
            ViewModel.UpdateList();
        }

        protected override void WindowVisibleChanged(bool isVisible)
        {
            base.WindowVisibleChanged(isVisible);

            if (isVisible)
            {
                ViewModel.UpdateList();
            }
        }

        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            ViewModel.UpdateList();
        }

        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!m_suppressChangeHandlers)
            {
                m_suppressChangeHandlers = true;
                ViewModel.UpdateIntValue();
                m_suppressChangeHandlers = false;
            }
        }

        private void SingleUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!m_suppressChangeHandlers)
            {
                m_suppressChangeHandlers = true;
                ViewModel.UpdateFloatValue();
                m_suppressChangeHandlers = false;
            }
        }
    }
}
