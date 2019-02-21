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

using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace LcsSaveEditor.Core.Controls
{
    /// <summary>
    /// A custom <see cref="TabControl"/> that doesn't unload its content
    /// each time a tab is switched.
    /// </summary>
    /// <remarks>
    /// Adapted from http://stackoverflow.com/a/9802346.
    /// </remarks>
    [TemplatePart(Name = "PART_ItemsHolder", Type = typeof(Panel))]
    public class NonUnloadingTabControl : TabControl
    {
        private Panel m_itemsHolder;

        public NonUnloadingTabControl()
            : base()
        {
            ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_itemsHolder = GetTemplateChild("PART_ItemsHolder") as Panel;
            UpdateSelectedItem();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (m_itemsHolder == null) {
                return;
            }

            switch (e.Action) {
                case NotifyCollectionChangedAction.Reset:
                    m_itemsHolder.Children.Clear();
                    break;

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null) {
                        foreach (var item in e.OldItems) {
                            ContentPresenter cp = FindChildContentPresenter(item);
                            if (cp != null) {
                                m_itemsHolder.Children.Remove(cp);
                            }
                        }
                    }
                    UpdateSelectedItem();
                    break;
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            UpdateSelectedItem();
        }

        private void UpdateSelectedItem()
        {
            if (m_itemsHolder == null) {
                return;
            }

            TabItem item = GetSelectedTabItem();
            if (item != null) {
                CreateChildContentPresenter(item);
            }

            foreach (ContentPresenter cp in m_itemsHolder.Children) {
                cp.Visibility = ((cp.Tag as TabItem).IsSelected)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private ContentPresenter CreateChildContentPresenter(object item)
        {
            if (item == null) {
                return null;
            }

            ContentPresenter cp = FindChildContentPresenter(item);
            if (cp != null) {
                return cp;
            }

            TabItem tabItem = item as TabItem;
            cp = new ContentPresenter
            {
                Content = (tabItem != null) ? tabItem.Content : item,
                ContentTemplate = SelectedContentTemplate,
                ContentTemplateSelector = SelectedContentTemplateSelector,
                ContentStringFormat = SelectedContentStringFormat,
                Visibility = Visibility.Collapsed,
                Tag = tabItem ?? (ItemContainerGenerator.ContainerFromItem(item))
            };
            m_itemsHolder.Children.Add(cp);

            return cp;
        }

        private ContentPresenter FindChildContentPresenter(object data)
        {
            if (data is TabItem) {
                data = (data as TabItem).Content;
            }

            if (data == null || m_itemsHolder == null) {
                return null;
            }

            foreach (ContentPresenter cp in m_itemsHolder.Children) {
                if (cp.Content == data) {
                    return cp;
                }
            }

            return null;
        }

        private TabItem GetSelectedTabItem()
        {
            if (SelectedItem == null) {
                return null;
            }

            return SelectedItem as TabItem
                ?? ItemContainerGenerator.ContainerFromIndex(SelectedIndex) as TabItem;
        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated) {
                ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged;
                UpdateSelectedItem();
            }
        }
    }
}
