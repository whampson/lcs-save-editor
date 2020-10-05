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
    public partial class StatsWindow : ChildWindowBase
    {
        public new ViewModels.StatsWindow ViewModel
        {
            get { return (ViewModels.StatsWindow) DataContext; }
            set { DataContext = value; }
        }

        public StatsWindow()
        {
            InitializeComponent();
        }

        protected override void WindowLoaded()
        {
            base.WindowLoaded();

            ViewModel.RefreshStats();
        }

        protected override void WindowVisibleChanged(bool isVisible)
        {
            base.WindowVisibleChanged(isVisible);

            if (isVisible)
            {
                ViewModel.RefreshStats();
            }
        }
    }
}
