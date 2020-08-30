using GTASaveData.Types;
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
using Xceed.Wpf.Toolkit.Core;

namespace LCSSaveEditor.GUI.Controls
{
    /// <summary>
    /// Interaction logic for LocationPicker.xaml
    /// </summary>
    public partial class LocationPicker : UserControl
    {
        public static readonly DependencyProperty GapThicknessProperty = DependencyProperty.Register(
            nameof(GapThickness), typeof(Thickness), typeof(LocationPicker));

        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            nameof(X), typeof(float), typeof(LocationPicker));

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            nameof(Y), typeof(float), typeof(LocationPicker));

        public static readonly DependencyProperty ZProperty = DependencyProperty.Register(
            nameof(Z), typeof(float), typeof(LocationPicker));

        public static readonly RoutedEvent XChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(XChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LocationPicker));

        public static readonly RoutedEvent YChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(YChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LocationPicker));

        public static readonly RoutedEvent ZChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(ZChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LocationPicker));

        public Thickness GapThickness
        {
            get { return (Thickness) GetValue(GapThicknessProperty); }
            set { SetValue(GapThicknessProperty, value); }
        }

        public float X
        {
            get { return (float) GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public float Y
        {
            get { return (float) GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public float Z
        {
            get { return (float) GetValue(ZProperty); }
            set { SetValue(ZProperty, value); }
        }

        public event RoutedEventHandler XChanged
        {
            add { AddHandler(XChangedEvent, value); }
            remove { RemoveHandler(XChangedEvent, value); }
        }

        public event RoutedEventHandler YChanged
        {
            add { AddHandler(YChangedEvent, value); }
            remove { RemoveHandler(YChangedEvent, value); }
        }

        public event RoutedEventHandler ZChanged
        {
            add { AddHandler(ZChangedEvent, value); }
            remove { RemoveHandler(ZChangedEvent, value); }
        }

        public LocationPicker()
        {
            InitializeComponent();
        }

        private void X_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            float oldValue = (float) (e.OldValue ?? new float());
            float newValue = (float) (e.NewValue ?? new float());

            if (oldValue != newValue)
            {
                RaiseEvent(new PropertyChangedEventArgs<float>(XChangedEvent, oldValue, newValue));
            }
        }

        private void Y_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            float oldValue = (float) (e.OldValue ?? new float());
            float newValue = (float) (e.NewValue ?? new float());

            if (oldValue != newValue)
            {
                RaiseEvent(new PropertyChangedEventArgs<float>(YChangedEvent, oldValue, newValue));
            }
        }

        private void Z_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            float oldValue = (float) (e.OldValue ?? new float());
            float newValue = (float) (e.NewValue ?? new float());

            if (oldValue != newValue)
            {
                RaiseEvent(new PropertyChangedEventArgs<float>(ZChangedEvent, oldValue, newValue));
            }
        }
    }
}
