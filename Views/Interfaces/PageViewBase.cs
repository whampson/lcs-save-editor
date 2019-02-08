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

using System.Windows;
using System.Windows.Controls;

namespace LcsSaveEditor.Views
{
    /// <summary>
    /// Base class for Page views.
    /// A Page view is a generic view that is shown in the main TabControl;
    /// each tab shows a separate "Page".
    /// </summary>
    public abstract class PageViewBase : UserControl
    {
        public PageViewBase()
        {
            IsPageLoaded = false;
            Loaded += View_Loaded;
        }

        /// <summary>
        /// Gets a value indicating whether the page has finished loading.
        /// </summary>
        protected bool IsPageLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// 'Loaded' event handler.
        /// </summary>
        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            IsPageLoaded = true;
        }

        /// <summary>
        /// Identifies the <see cref="DataChanged"/> routed event.
        /// </summary>
        public static readonly RoutedEvent DataChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(DataChangedEvent),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(PageViewBase));

        /// <summary>
        /// Occurs when the model data has been changed in some way.
        /// </summary>
        public event RoutedEventHandler DataChanged
        {
            add { AddHandler(DataChangedEvent, value); }
            remove { RemoveHandler(DataChangedEvent, value); }
        }

        /// <summary>
        /// Notifies all listeners that the 'DataChanged' event occurred.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnDataChanged(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoaded) {
                return;
            }

            RaiseEvent(new RoutedEventArgs(DataChangedEvent, sender));
        }
    }
}
