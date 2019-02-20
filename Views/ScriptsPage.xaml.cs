using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LcsSaveEditor.Views
{
    /// <summary>
    /// Interaction logic for ScriptsPage.xaml
    /// </summary>
    public partial class ScriptsPage : PageViewBase
    {
        public ScriptsPage()
            : base()
        {
            InitializeComponent();
        }

        private void GlobalVariablesDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = "$" + e.Row.GetIndex().ToString();
        }

        private void LocalVariablesDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex().ToString() + "@";
        }

        private void ReturnStackDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex().ToString();
        }
    }
}
