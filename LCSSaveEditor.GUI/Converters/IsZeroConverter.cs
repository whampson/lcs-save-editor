using System;
using System.Globalization;
using System.Windows.Data;

namespace LCSSaveEditor.GUI.Converters
{
    public class IsZeroConverter : IValueConverter
    {
        public bool Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int ivalue)
            {
                if (Invert)
                {
                    return ivalue != 0;
                }
                return ivalue == 0;
            }
            
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"Cannot convert '{value}' to type {targetType}.");
        }
    }
}
