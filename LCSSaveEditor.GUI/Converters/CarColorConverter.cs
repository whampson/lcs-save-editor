using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using LCSSaveEditor.GUI.Types;
using Xceed.Wpf.Toolkit;

namespace LCSSaveEditor.GUI.Converters
{
    public class CarColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte c)
            {
                var color = VehicleInfo.CarColors.ElementAtOrDefault(c);
                if (color == null)
                {
                    return Colors.Black;
                }
                return color.Color;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                var c = VehicleInfo.CarColors.Where(x => x.Color == color).FirstOrDefault();
                if (c == default)
                {
                    return 0;
                }

                return VehicleInfo.CarColors.IndexOf(c);
            }

            throw new NotSupportedException($"Cannot convert '{value}' to type {targetType}.");
        }
    }
}
