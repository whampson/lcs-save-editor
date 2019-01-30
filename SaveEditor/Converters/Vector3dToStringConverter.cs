#region License
/* Copyright(c) 2016-2019 Wes Hampson
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
#endregion

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using WHampson.LcsSaveEditor.Models;

namespace WHampson.Gta3CarGenEditor.Converters
{
    /// <summary>
    /// Converts a <see cref="Vector3d"/> into a <see cref="string"/> and back.
    /// </summary>
    [ValueConversion(typeof(Vector3d), typeof(string))]
    public class Vector3dToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is Vector3d)) {
                return DependencyProperty.UnsetValue;
            }

            Vector3d v = value as Vector3d;

            return string.Format("{0},{1},{2}", v.X, v.Y, v.Z);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is string)) {
                return DependencyProperty.UnsetValue;
            }

            Vector3d v = new Vector3d();
            string src = value as string;
            string[] compStr = src.Split(',');
            if (compStr.Length < 3) {
                return v;
            }

            bool validX = float.TryParse(compStr[0], out float x);
            bool validY = float.TryParse(compStr[1], out float y);
            bool validZ = float.TryParse(compStr[2], out float z);

            if (validX && validY && validZ) {
                v.X = x;
                v.Y = y;
                v.Z = z;
            }

            return v;
        }
    }
}
