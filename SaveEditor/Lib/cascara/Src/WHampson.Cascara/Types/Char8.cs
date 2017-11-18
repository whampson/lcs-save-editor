#region License
/* Copyright (c) 2017 Wes Hampson
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
using System.Runtime.InteropServices;

namespace WHampson.Cascara.Types
{
    /// <summary>
    /// An 8-bit character value.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 1, Pack = 0)]
    public struct Char8 : IConvertible, IComparable, IComparable<Char8>, IEquatable<Char8>
    {
        private byte m_value;

        private Char8(char value)
        {
            m_value = (byte) value;
        }

        private char CharValue
        {
            get { return (char) m_value; }
        }

        public int CompareTo(Char8 other)
        {
            return CharValue.CompareTo(other.CharValue);
        }

        public bool Equals(Char8 other)
        {
            return CharValue == other.CharValue;
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (!(obj is Char8))
            {
                string fmt = "Object is not an instance of {0}.";
                string msg = string.Format(fmt, GetType().Name);

                throw new ArgumentException(msg, "obj");
            }

            return CompareTo((Char8) obj);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Char8))
            {
                return false;
            }

            return Equals((Char8) obj);
        }

        public override int GetHashCode()
        {
            return CharValue.GetHashCode();
        }

        public override string ToString()
        {
            return CharValue.ToString();
        }

        public static implicit operator Char8(char value)
        {
            return new Char8(value);
        }

        public static explicit operator char(Char8 value)
        {
            return value.CharValue;
        }

        #region IConvertibleImpl
        public TypeCode GetTypeCode()
        {
            return TypeCode.Char;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(CharValue);
        }

        public char ToChar(IFormatProvider provider)
        {
            return CharValue;
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(CharValue);
        }

        public byte ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(CharValue);
        }

        public short ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(CharValue);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(CharValue);
        }

        public int ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(CharValue);
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(CharValue);
        }

        public long ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(CharValue);
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(CharValue);
        }

        public float ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(CharValue);
        }

        public double ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(CharValue);
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(CharValue);
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(CharValue);
        }

        public string ToString(IFormatProvider provider)
        {
            return Convert.ToString(CharValue);
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType == GetType())
            {
                return this;
            }

            string fmt = "Cannot convert {0} to {1}.";
            string msg = string.Format(fmt, GetType(), conversionType);

            throw new InvalidCastException(msg);
        }
        #endregion
    }
}
