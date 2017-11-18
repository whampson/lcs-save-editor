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
    /// A typeless pointer to some data in memory.
    /// </summary>
    /// <remarks>
    /// This can be thought of as a <code>void/code> pointer.
    /// </remarks>
    public class Pointer : ICascaraPointer
    {
        /// <summary>
        /// Creates a new typeless <see cref="Pointer"/>
        /// that points to the specified address.
        /// </summary>
        /// <param name="addr">
        /// The address to point to.
        /// </param>
        public Pointer(IntPtr addr)
        {
            Address = addr;
        }

        public IntPtr Address
        {
            get;
        }

        public bool IsNull()
        {
            return Address == IntPtr.Zero;
        }

        public object GetValue(Type t)
        {
            if (!t.IsValueType)
            {
                throw new InvalidOperationException("Type must be a value type.");
            }

            return Marshal.PtrToStructure(Address, t);
        }

        public T GetValue<T>()
            where T : struct
        {
            return Marshal.PtrToStructure<T>(Address);
        }

        public void SetValue<T>(T value)
            where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] data = new byte[size];

            Marshal.StructureToPtr(value, Address, true);
        }

        /// <summary>
        /// Converts this typeless <see cref="Pointer"/> into a <see cref="Pointer{T}"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the data pointed to.
        /// </typeparam>
        /// <returns>
        /// A <see cref="Pointer{T}"/> object that points to the same
        /// location as this pointer.
        /// </returns>
        public Pointer<T> ToPointer<T>()
            where T : struct
        {
            return new Pointer<T>(Address);
        }

        public override string ToString()
        {
            return string.Format("[Address: {0}]", Address.ToInt64());
        }

        #region IConvertableImpl
        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public string ToString(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            // Handle conversion to self
            if (conversionType == GetType())
            {
                return this;
            }

            // Handle conversion to Pointer<> and any subclass of Pointer<>
            if (TypeUtils.IsAssignableToGenericType(conversionType, typeof(Pointer<>)))
            {
                return Activator.CreateInstance(conversionType, Address);
            }

            throw new InvalidCastException();
        }
        #endregion

        /// <summary>
        /// Converts an <see cref="IntPtr"/> into a <see cref="Pointer"/>.
        /// </summary>
        /// <param name="ptr">
        /// The <see cref="IntPtr"/> to convert.
        /// </param>
        public static implicit operator Pointer(IntPtr ptr)
        {
            return new Pointer(ptr);
        }

        /// <summary>
        /// Converts a <see cref="Pointer"/> into an <see cref="IntPtr"/>.
        /// </summary>
        /// <param name="ptr">
        /// The <see cref="Pointer"/> to convert.
        /// </param>
        public static implicit operator IntPtr(Pointer ptr)
        {
            return ptr.Address;
        }

        /// <summary>
        /// Adds an offset to the address pointed to by a <see cref="Pointer"/>.
        /// The offset is in units of bytes.
        /// </summary>
        /// <param name="ptr">
        /// The base address.
        /// </param>
        /// <param name="off">
        /// The offset to add to the base.
        /// </param>
        /// <returns>
        /// A <see cref="Pointer"/> object that points to the new address.
        /// </returns>
        public static Pointer operator +(Pointer ptr, int off)
        {
            return new Pointer(ptr.Address + off);
        }

        /// <summary>
        /// Subtracts an offset from the address pointed to by a <see cref="Pointer"/>.
        /// The offset is in units of bytes.
        /// </summary>
        /// <param name="ptr">
        /// The base address.
        /// </param>
        /// <param name="off">
        /// The offset to subtract from the base.
        /// </param>
        /// <returns>
        /// A <see cref="Pointer"/> object that points to the new address.
        /// </returns>
        public static Pointer operator -(Pointer ptr, int off)
        {
            return new Pointer(ptr.Address - off);
        }

        public static bool IsArrayPointer(Pointer ptr)
        {
            return IsArrayPointer(ptr.GetType());
        }

        public static bool IsArrayPointer<T>(Pointer ptr)
            where T : struct
        {
            return IsArrayPointer<T>(ptr.GetType());
        }

        public static bool IsArrayPointer(Type t)
        {
            if (t.IsGenericType)
            {
                Type propGenType = t.GetGenericTypeDefinition();
                if (propGenType == typeof(ArrayPointer<>))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsArrayPointer<T>(Type t)
            where T : struct
        {
            if (!IsArrayPointer(t))
            {
                return false;
            }

            Type genType = t.GetGenericArguments()[0];  // We only need the first one

            return genType == typeof(T);
        }
    }
}
