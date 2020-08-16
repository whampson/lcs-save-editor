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
    /// Interaction logic for LocationPicker2D.xaml
    /// </summary>
    public partial class LocationPicker2D : UserControl
    {
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(Value), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LocationPicker2D));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value), typeof(Vector2D), typeof(LocationPicker2D));

        public static readonly DependencyProperty GapThicknessProperty = DependencyProperty.Register(
            nameof(GapThickness), typeof(Thickness), typeof(LocationPicker2D));

        public Thickness GapThickness
        {
            get { return (Thickness) GetValue(GapThicknessProperty); }
            set { SetValue(GapThicknessProperty, value); }
        }

        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        public Vector2D Value
        {
            get { return (Vector2D) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public LocationPicker2D()
        {
            InitializeComponent();
        }

        private void OnValueChanged(Vector2D oldValue, Vector2D newValue)
        {
            RaiseEvent(new PropertyChangedEventArgs<Vector2D>(ValueChangedEvent, oldValue, newValue));
        }

        private void X_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            float oldValue = (float) (e.OldValue ?? new float());
            float newValue = (float) (e.NewValue ?? new float());

            if (oldValue != newValue)
            {
                OnValueChanged(
                    new Vector2D() { X = oldValue, Y = Value.Y },
                    new Vector2D() { X = newValue, Y = Value.Y });
            }
        }

        private void Y_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            float oldValue = (float) (e.OldValue ?? new float());
            float newValue = (float) (e.NewValue ?? new float());

            if (oldValue != newValue)
            {
                OnValueChanged(
                    new Vector2D() { X = Value.X, Y = oldValue },
                    new Vector2D() { X = Value.X, Y = newValue });
            }
        }
    }
}
