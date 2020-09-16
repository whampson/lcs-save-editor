using GTASaveData.Types;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace LCSSaveEditor.GUI.Controls
{
    /// <summary>
    /// Interaction logic for LocationPicker2D.xaml
    /// </summary>
    public partial class LocationPicker2D : LocationPickerBase
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value), typeof(Vector2D?), typeof(LocationPicker2D),
            new FrameworkPropertyMetadata(default(Vector2D?),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnValueChanged, OnCoerceValue, false, UpdateSourceTrigger.PropertyChanged));

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(ValueChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Vector2D?>), typeof(LocationPicker2D));

        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            nameof(X), typeof(float), typeof(LocationPicker2D));

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            nameof(Y), typeof(float), typeof(LocationPicker2D));

        public event RoutedPropertyChangedEventHandler<Vector2D?> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        public Vector2D? Value
        {
            get { return (Vector2D?) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }

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

        private bool m_suppressValueChanged;
        private bool m_suppressComponentsChanged;

        public LocationPicker2D()
        {
            InitializeComponent();
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LocationPicker2D obj)
            {
                obj.OnValueChanged((Vector2D?) e.OldValue, (Vector2D?) e.NewValue);
            }
        }

        private void OnValueChanged(Vector2D? oldValue, Vector2D? newValue)
        {
            if (!m_suppressValueChanged && newValue.HasValue)
            {
                m_suppressComponentsChanged = true;
                try
                {
                    X = newValue.Value.X;
                    Y = newValue.Value.Y;
                }
                finally
                {
                    m_suppressComponentsChanged = false;
                }
                
            }
        }

        private static object OnCoerceValue(DependencyObject d, object value)
        {
            return value;
        }

        private void XComponent_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!m_suppressComponentsChanged
                && e.OldValue is float oldValue
                && e.NewValue is float newValue
                && oldValue != newValue)
            {
                m_suppressValueChanged = true;
                try
                {
                    Value = new Vector2D(newValue, Y);
                }
                finally
                {
                    m_suppressValueChanged = false;
                }
            }
        }

        private void YComponent_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!m_suppressComponentsChanged
                && e.OldValue is float oldValue
                && e.NewValue is float newValue
                && oldValue != newValue)
            {
                m_suppressValueChanged = true;
                try
                {
                    Value = new Vector2D(X, newValue);
                }
                finally
                {
                    m_suppressValueChanged = false;
                }
            }
        }
    }
}
