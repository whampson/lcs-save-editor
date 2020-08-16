using System;
using System.Globalization;
using System.Windows.Data;

namespace LCSSaveEditor.GUI.Converters
{
    public class UInt32Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ChangeType(value, typeof(long));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] intBits = BitConverter.GetBytes((long) value);
            return BitConverter.ToUInt32(intBits, 0);
        }
    }
}
