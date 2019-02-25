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
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace LcsSaveEditor.Views
{
    /// <summary>
    /// Interaction logic for GeneralPage.xaml
    /// </summary>
    public partial class GeneralPage : PageView
    {
        public GeneralPage()
            : base()
        {
            InitializeComponent();
        }

        public GeneralViewModel ViewModel
        {
            get { return (GeneralViewModel) DataContext; }
            set { DataContext = value; }
        }

        private void ViewModel_ShowSelectedWeatherItem(object sender, EventArgs e)
        {
            /*
             * Scrolls the ListBox to the selected item.
             * Can't use ScrollIntoView() because items in the ListBox are not unique.
             * Adapted from https://stackoverflow.com/a/211984.
             */

            // TODO: BUG - this event doesn't get fired the first time a file ia loaded.

            int idx = m_weatherListBox.SelectedIndex;
            if (idx == -1) {
                return;
            }

            VirtualizingStackPanel vsp = (VirtualizingStackPanel) typeof(ItemsControl)
                .InvokeMember("_itemsHost", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic, null, m_weatherListBox, null);

            double scrollHeight = vsp.ScrollOwner.ScrollableHeight;
            double offset = scrollHeight * idx / m_weatherListBox.Items.Count;
            vsp.SetVerticalOffset(offset);
        }

        private void PageView_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowSelectedWeatherItem += ViewModel_ShowSelectedWeatherItem;
        }
    }
}
