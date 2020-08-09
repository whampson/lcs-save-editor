using GTASaveData.Types;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LCSSaveEditor.GUI.Converters
{
    public class VectorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Vector2D v2)
            {
                return $"{v2.X:0.###}, {v2.Y:0.###}";
            }
            
            if (value is Vector3D v3)
            {
                return $"{v3.X:0.###}, {v3.Y:0.###}, {v3.Z:0.###}";
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                string[] comp = s.Split(",");
                if (comp.Length > 1 && comp.Length < 4)
                {
                    bool xValid = false, yValid = false, zValid = false;
                    float x = 0, y = 0, z = 0;

                    xValid = float.TryParse(comp[0], out x);
                    yValid = float.TryParse(comp[1], out y);
                    if (comp.Length == 3) zValid = float.TryParse(comp[2], out z);

                    if (xValid && yValid)
                    {
                        if (zValid) return new Vector3D(x, y, z);
                        return new Vector2D(x, y);
                    }
                }
            }

            return DependencyProperty.UnsetValue;
        }
    }
}
