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

using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace LcsSaveEditor.Core.Behaviors
{
    /// <summary>
    /// Causes an <see cref="ItemsControl"/> to scroll
    /// automatically to the bottom of the items list
    /// each time a new item is added, but only if the
    /// scroll bar is already at the bottom of the list.
    /// </summary>
    /// <remarks>
    /// Adapted from https://stackoverflow.com/a/12255329.
    /// </remarks>
    public class ScrollOnNewItem : Behavior<ItemsControl>
    {
        private ScrollViewer m_scrollViewer;
        private bool m_isScrollDownEnabled;

        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.Unloaded += AssociatedObject_Unloaded;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(AssociatedObject.ItemsSource is INotifyCollectionChanged itemsSource)) {
                return;
            }

            itemsSource.CollectionChanged += ItemsSource_CollectionChanged;
            m_scrollViewer = AssociatedObject.GetSelfAndAncestors()
                .Where(x => x.GetType().Equals(typeof(ScrollViewer)))
                .FirstOrDefault() as ScrollViewer;
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            if (!(AssociatedObject.ItemsSource is INotifyCollectionChanged itemsSource)) {
                return;
            }

            itemsSource.CollectionChanged -= ItemsSource_CollectionChanged;
        }

        private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (m_scrollViewer == null) {
                return;
            }

            m_isScrollDownEnabled = m_scrollViewer.ScrollableHeight > 0
                && m_scrollViewer.VerticalOffset + m_scrollViewer.ViewportHeight < m_scrollViewer.ExtentHeight;

            if (e.Action == NotifyCollectionChangedAction.Add && !m_isScrollDownEnabled) {
                int count = AssociatedObject.Items.Count;
                if (count == 0) {
                    return;
                }

                object item = AssociatedObject.Items[count - 1];
                FrameworkElement frameworkElement = AssociatedObject.ItemContainerGenerator
                    .ContainerFromItem(item) as FrameworkElement;
                if (frameworkElement == null) {
                    return;
                }

                frameworkElement.BringIntoView();
            }
        }
    }
}
