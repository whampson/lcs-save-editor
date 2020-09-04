using System;
using System.Collections.Generic;
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
    /// Interaction logic for StatsTab.xaml
    /// </summary>
    public partial class StatsTab : TabPageBase<ViewModels.StatsTab>
    {
        static StatsTab()
        {
            DataContextProperty.AddOwner(typeof(DataGridColumn));
            DataContextProperty.OverrideMetadata(typeof(DataGrid),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits,
                    new PropertyChangedCallback(OnDataContextChanged)));
        }
        public static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid grid)
            {
                foreach (DataGridColumn col in grid.Columns)
                {
                    col.SetValue(DataContextProperty, e.NewValue);
                }
            }
        }

        public StatsTab()
        {
            InitializeComponent();
        }
    }
}
