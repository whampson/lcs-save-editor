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

using LcsSaveEditor.Models;
using LcsSaveEditor.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace LcsSaveEditor.Views.Converters
{
    /// <summary>
    /// Converts a <see cref="ScriptVariable"/> to a <see cref="string"/> and back
    /// using the specified <see cref="ScriptVariableDataFormat"/>.
    /// </summary>
    [ValueConversion(typeof(ScriptVariable), typeof(string))]
    public class ScriptVariableDataFormatConverter : IMultiValueConverter
    {
        private object m_param;

        public ScriptVariableDataFormatConverter()
        {
            m_param = ScriptVariableDataFormat.Int;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            object value = values[0];
            m_param = values[1];

            if (!(value is uint v)) {
                return "0";
            }

            if (!(m_param is ScriptVariableDataFormat t)) {
                t = ScriptVariableDataFormat.Int;
            }

            switch (t) {
                case ScriptVariableDataFormat.Int:
                    return ((int) v).ToString();
                case ScriptVariableDataFormat.Float:
                    return BitConverter.ToSingle(BitConverter.GetBytes(v), 0).ToString();
                case ScriptVariableDataFormat.Hex:
                    return v.ToString("X");
            }

            return "0";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (!(value is string v)) {
                return new object[] { 0U, m_param };
            }

            if (!(m_param is ScriptVariableDataFormat t)) {
                t = ScriptVariableDataFormat.Int;
            }

            switch (t) {
                case ScriptVariableDataFormat.Int:
                    if (int.TryParse(v, out int i)) {
                        return new object[] { (uint) i, m_param };
                    }
                    break;
                case ScriptVariableDataFormat.Float:
                    if (float.TryParse(v, out float f)) {
                        return new object[] { BitConverter.ToUInt32(BitConverter.GetBytes(f), 0), m_param };
                    }
                    break;
                case ScriptVariableDataFormat.Hex:
                    if (uint.TryParse(v, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint ui)) {
                        return new object[] { ui, m_param };
                    }
                    break;
            }

            return null;
        }
    }
}
