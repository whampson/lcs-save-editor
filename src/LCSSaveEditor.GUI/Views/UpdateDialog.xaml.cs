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
using LCSSaveEditor.GUI.Utils;

namespace LCSSaveEditor.GUI.Views
{
    /// <summary>
    /// Interaction logic for UpdateDialog.xaml
    /// </summary>
    public partial class UpdateDialog : DialogBase
    {
        public new ViewModels.UpdateDialog ViewModel
        {
            get { return (ViewModels.UpdateDialog) DataContext; }
            set { DataContext = value; }
        }

        public UpdateDialog()
        {
            InitializeComponent();
        }

        public void SetUpdateInfo(GitHubRelease updateInfo)
        {
            ViewModel.UpdateInfo = updateInfo;
        }

        public string GetDownloadedPackagePath()
        {
            return ViewModel.DownloadPath;
        }

        protected override void WindowClosing(CancelEventArgs e)
        {
            base.WindowClosing(e);
            if (ViewModel.IsDownloading)
            {
                ViewModel./*Hey kid, I'm a computer,*/StopAllTheDownloading();
            }
        }
    }
}
