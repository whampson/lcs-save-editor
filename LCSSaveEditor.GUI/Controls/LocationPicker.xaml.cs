using GTASaveData.Types;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace LCSSaveEditor.GUI.Controls
{
    /// <summary>
    /// Interaction logic for LocationPicker.xaml
    /// </summary>
    public partial class LocationPicker : UserControl
    {
        public static readonly DependencyProperty GapThicknessProperty = DependencyProperty.Register(
            nameof(GapThickness), typeof(Thickness), typeof(LocationPicker));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value), typeof(Vector3D?), typeof(LocationPicker),
            new FrameworkPropertyMetadata(default(Vector3D?),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnValueChanged, OnCoerceValue, false, UpdateSourceTrigger.PropertyChanged));

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(ValueChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Vector3D?>), typeof(LocationPicker));

        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            nameof(X), typeof(float), typeof(LocationPicker));

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            nameof(Y), typeof(float), typeof(LocationPicker));

        public static readonly DependencyProperty ZProperty = DependencyProperty.Register(
            nameof(Z), typeof(float), typeof(LocationPicker));

        public event RoutedPropertyChangedEventHandler<Vector3D?> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        public Thickness GapThickness
        {
            get { return (Thickness) GetValue(GapThicknessProperty); }
            set { SetValue(GapThicknessProperty, value); }
        }

        public Vector3D? Value
        {
            get { return (Vector3D?) GetValue(ValueProperty); }
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

        public float Z
        {
            get { return (float) GetValue(ZProperty); }
            set { SetValue(ZProperty, value); }
        }

        private bool m_suppressValueChanged;
        private bool m_suppressComponentsChanged;

        public LocationPicker()
        {
            InitializeComponent();
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LocationPicker obj)
            {
                obj.OnValueChanged((Vector3D?) e.OldValue, (Vector3D?) e.NewValue);
            }
        }

        private void OnValueChanged(Vector3D? oldValue, Vector3D? newValue)
        {
            if (!m_suppressValueChanged && newValue.HasValue)
            {
                m_suppressComponentsChanged = true;
                try
                {
                    X = newValue.Value.X;
                    Y = newValue.Value.Y;
                    Z = newValue.Value.Z;
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
                    Value = new Vector3D(newValue, Y, Z);
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
                    Value = new Vector3D(X, newValue, Z);
                }
                finally
                {
                    m_suppressValueChanged = false;
                }
            }
        }

        private void ZComponent_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!m_suppressComponentsChanged
                && e.OldValue is float oldValue
                && e.NewValue is float newValue
                && oldValue != newValue)
            {
                m_suppressValueChanged = true;
                try
                {
                    Value = new Vector3D(X, Y, newValue);
                }
                finally
                {
                    m_suppressValueChanged = false;
                }
            }
        }
    }
}
