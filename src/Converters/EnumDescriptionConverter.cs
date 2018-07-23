using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WHampson.LcsSaveEditor.Helpers;

namespace WHampson.LcsSaveEditor.Converters
{
    [ValueConversion(typeof(Enum), typeof(string))]
    public class EnumDescriptionConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) {
                return DependencyProperty.UnsetValue;
            }

            DescriptionAttribute descAttr = EnumHelper.GetAttribute<DescriptionAttribute>(value as Enum);
            if (descAttr == null) {
                return value.ToString();
            }
            else {
                return descAttr.Description;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack is not supported for this converter.");
        }
    }
}
