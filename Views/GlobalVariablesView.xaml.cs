#region License
/* Copyright(c) 2016-2019 Wes Hampson
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
#endregion

using LcsSaveEditor.ViewModels;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace LcsSaveEditor.Views
{
    /// <summary>
    /// Interaction logic for GlobalVariablesView.xaml
    /// </summary>
    public partial class GlobalVariablesView : PageViewBase
    {
        public GlobalVariablesView()
            : base()
        {
            InitializeComponent();
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            // Register items source CollectionChanged listener

            if (!(dg.ItemsSource is ObservableCollection<NamedScriptVariable> itemsSource)) {
                return;
            }
            itemsSource.CollectionChanged += ItemsSource_CollectionChanged;
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            // Put row number in row header

            int idx = e.Row.GetIndex();
            if (idx == dg.Items.Count - 1) {
                e.Row.Header = "*";
            }
            else {
                e.Row.Header = idx.ToString();
            }
        }

        private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Refresh the rows each time an item is added/removed.
            // This needs to happen otherwise the row numbers will have a gap or duplicate
            // until the user has scrolled far enough away to trigger another refresh.

            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                    dg.Items.Refresh();
                    break;
            }
        }
    }
}
