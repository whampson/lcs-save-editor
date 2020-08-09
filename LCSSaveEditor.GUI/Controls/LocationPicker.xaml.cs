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
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(Value), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LocationPicker));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value), typeof(Vector3D), typeof(LocationPicker),
            new PropertyMetadata(default(Vector3D)));

        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        public Vector3D Value
        {
            get { return (Vector3D) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public LocationPicker()
        {
            InitializeComponent();
        }

        private void OnValueChanged(Vector3D oldValue, Vector3D newValue)
        {
            RaiseEvent(new PropertyChangedEventArgs<Vector3D>(ValueChangedEvent, oldValue, newValue));
        }

        private void X_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            float oldValue = (float) (e.OldValue ?? new float());
            float newValue = (float) (e.NewValue ?? new float());

            if (oldValue != newValue)
            {
                OnValueChanged(
                    new Vector3D() { X = oldValue, Y = Value.Y, Z = Value.Z },
                    new Vector3D() { X = newValue, Y = Value.Y, Z = Value.Z });
            }
        }

        private void Y_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            float oldValue = (float) (e.OldValue ?? new float());
            float newValue = (float) (e.NewValue ?? new float());

            if (oldValue != newValue)
            {
                OnValueChanged(
                    new Vector3D() { X = Value.X, Y = oldValue, Z = Value.Z },
                    new Vector3D() { X = Value.X, Y = newValue, Z = Value.Z });
            }
        }

        private void Z_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            float oldValue = (float) (e.OldValue ?? new float());
            float newValue = (float) (e.NewValue ?? new float());

            if (oldValue != newValue)
            {
                OnValueChanged(
                    new Vector3D() { X = Value.X, Y = Value.Y, Z = oldValue },
                    new Vector3D() { X = Value.X, Y = Value.Y, Z = newValue });
            }
        }
    }
}
