using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace LCSSaveEditor.GUI.Converters
{
    public class EnumFlagConverter : IValueConverter
    {
        private Enum m_target;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum e && parameter is Enum flag)
            {
                m_target = e;
                return e.HasFlag(flag);
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool v && parameter is Enum flag)
            {
                int e = EnumToInt(m_target);
                int f = EnumToInt(flag);

                int result = (v) ? (e | f) : (e & ~f);

                Enum retval = (Enum) Enum.Parse(targetType, result.ToString());
                return retval;
            }
            throw new NotSupportedException($"Cannot convert '{value}' to type {targetType}.");
        }

        private static int EnumToInt(Enum e) => (int) (object) e;
    }
}
