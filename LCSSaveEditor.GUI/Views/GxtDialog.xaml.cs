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
using LCSSaveEditor.Core;
using LCSSaveEditor.GUI.Events;

namespace LCSSaveEditor.GUI.Views
{
    /// <summary>
    /// Interaction logic for GxtDialog.xaml
    /// </summary>
    public partial class GxtDialog : Window
    {
        public ViewModels.GxtDialog ViewModel
        {
            get { return (ViewModels.GxtDialog) DataContext; }
            set { DataContext = value; }
        }

        public GxtDialog()
        {
            InitializeComponent();
        }

        private void Dialog_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.DialogCloseRequest += ViewModel_DialogCloseRequest;
        }

        private void Dialog_Closing(object sender, CancelEventArgs e)
        {
            ViewModel.DialogCloseRequest -= ViewModel_DialogCloseRequest;
        }

        private void ViewModel_DialogCloseRequest(object sender, DialogCloseEventArgs e)
        {
            DialogResult = e.DialogResult;
            Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.Table.Clear();
            if (e.AddedItems == null)
            {
                return;
            }

            if (Gxt.TheText.TryGetTable(ViewModel.TableName, out var table))
            {
                ViewModel.Table = table;
            }
        }
    }
}
