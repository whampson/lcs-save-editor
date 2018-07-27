using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WHampson.LcsSaveEditor.Views
{
    public abstract class ViewBase : UserControl
    {
        public ViewBase()
        {
            IsViewLoaded = false;
            Loaded += View_Loaded;
        }

        public static readonly RoutedEvent DataChangedEvent
            = EventManager.RegisterRoutedEvent(
                "DataChangedEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WeaponsView));

        public event RoutedEventHandler DataChanged
        {
            add { AddHandler(DataChangedEvent, value); }
            remove { RemoveHandler(DataChangedEvent, value); }
        }

        protected void RaiseDataChanged(object sender, RoutedEventArgs e)
        {
            if (!IsViewLoaded) {
                return;
            }

            RaiseEvent(new RoutedEventArgs(DataChangedEvent, sender));
        }

        protected bool IsViewLoaded
        {
            get;
            private set;
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            IsViewLoaded = true;
        }
    }
}
