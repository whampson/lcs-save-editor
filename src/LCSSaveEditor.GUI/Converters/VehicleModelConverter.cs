using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using LCSSaveEditor.GUI.Types;
using WpfEssentials.Extensions;

namespace LCSSaveEditor.GUI.Converters
{
    public class VehicleModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is int v)
                ? Enum.Parse(typeof(Vehicle), v.ToString())
                : DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object retVal = null;
            if (value is string s)
            {
                retVal = (Enum.GetValues(typeof(Vehicle)) as IEnumerable<Vehicle>)
                    .Where(x => x.GetDescription() == s).Select(x => (int) x).FirstOrDefault();
            }
            else if (value is Vehicle v)
            {
                retVal = (int) v;
            }

            return retVal ?? throw new InvalidOperationException($"Cannot convert '{value}' to type Vehicle.");
        }
    }
}
