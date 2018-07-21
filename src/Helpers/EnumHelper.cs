﻿#region License
/* Copyright(c) 2016-2018 Wes Hampson
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
using System.Linq;
using System.Reflection;

namespace WHampson.LcsSaveEditor.Helpers
{
    public static class EnumHelper
    {
        /// <summary>
        /// Gets an <see cref="Attribute"/> attached to an enum value.
        /// </summary>
        /// <typeparam name="T">The Attribute type.</typeparam>
        /// <param name="e">The enum value.</param>
        /// <returns>
        /// The <see cref="Attribute"/> instance attached to the enum value.
        /// Null of none exist.
        /// </returns>
        public static T GetAttribute<T>(Enum e) where T : Attribute
        {
            MemberInfo[] m = e.GetType().GetMember(e.ToString());
            if (m.Count() == 0) {
                return null;
            }

            return (T) Attribute.GetCustomAttribute(m[0], typeof(T));
        }
    }
}
