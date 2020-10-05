using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace LCSSaveEditor.GUI.Controls
{
    /// <summary>
    /// A custom <see cref="TabControl"/> that doesn't unload its content
    /// each time a tab is switched.
    /// </summary>
    /// <remarks>
    /// Adapted from http://stackoverflow.com/a/9802346.
    /// </remarks>
    [TemplatePart(Name = "PART_ItemsHolder", Type = typeof(Panel))]
    public class TabControlEx : TabControl
    {
        private Panel m_itemsHolder;

        public TabControlEx()
            : base()
        {
            ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_itemsHolder = GetTemplateChild("PART_ItemsHolder") as Panel;
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (m_itemsHolder == null)
            {
                return;
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                {
                    m_itemsHolder.Children.Clear();
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                {
                    if (e.OldItems != null)
                    {
                        foreach (var item in e.OldItems)
                        {
                            ContentPresenter cp = FindChildContentPresenter(item);
                            if (cp != null)
                            {
                                m_itemsHolder.Children.Remove(cp);
                            }
                        }
                    }
                    UpdateSelectedItem();
                    break;
                }
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            UpdateSelectedItem();
        }

        private void UpdateSelectedItem()
        {
            if (m_itemsHolder == null)
            {
                return;
            }

            TabItem item = GetSelectedTabItem();
            if (item != null)
            {
                CreateChildContentPresenter(item);
            }

            foreach (ContentPresenter cp in m_itemsHolder.Children)
            {
                cp.Visibility = ((cp.Tag as TabItem).IsSelected)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private ContentPresenter CreateChildContentPresenter(object item)
        {
            ContentPresenter cp = FindChildContentPresenter(item);
            if (cp != null)
            {
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
                Tag = tabItem ?? (ItemContainerGenerator.ContainerFromItem(item)),
                Margin = new Thickness(0)
            };
            m_itemsHolder.Children.Add(cp);

            return cp;
        }

        private ContentPresenter FindChildContentPresenter(object data)
        {
            if (data is TabItem tabItem)
            {
                data = tabItem.Content;
            }

            if (data == null || m_itemsHolder == null)
            {
                return null;
            }

            foreach (ContentPresenter cp in m_itemsHolder.Children)
            {
                if (cp.Content == data)
                {
                    return cp;
                }
            }

            return null;
        }

        private TabItem GetSelectedTabItem()
        {
            if (SelectedItem == null)
            {
                return null;
            }

            return SelectedItem as TabItem
                ?? ItemContainerGenerator.ContainerFromIndex(SelectedIndex) as TabItem;
        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged;
                UpdateSelectedItem();
            }
        }
    }
}
