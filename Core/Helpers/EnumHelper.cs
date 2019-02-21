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
using System.Linq;
using System.Reflection;

namespace LcsSaveEditor.Core.Helpers
{
    /// <summary>
    /// Helper class for getting attribute information from Enum values.
    /// </summary>
    public static class EnumAttributeHelper
    {
        /// <summary>
        /// Gets an <see cref="Attribute"/> attached to an enum value.
        /// </summary>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <param name="e">The enum value.</param>
        /// <returns>
        /// The <see cref="Attribute"/> instance attached to the enum value.
        /// Null of none exist.
        /// </returns>
        public static T GetAttribute<T>(Enum e) where T : Attribute
        {
            if (e == null) {
                return null;
            }

            MemberInfo[] m = e.GetType().GetMember(e.ToString());
            if (m.Count() == 0) {
                return null;
            }

            return (T) Attribute.GetCustomAttribute(m[0], typeof(T));
        }

        /// <summary>
        /// Checks whether an enum value has a specific <see cref="Attribute"/>
        /// attached to it.
        /// </summary>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <param name="e">The enum value.</param>
        /// <returns>
        /// True if the specified enum value has the attribute attached to it,
        /// False otherwise.
        /// </returns>
        public static bool HasAttribute<T>(Enum e) where T : Attribute
        {
            if (e == null) {
                return false;
            }

            MemberInfo[] m = e.GetType().GetMember(e.ToString());
            if (m.Count() == 0) {
                return false;
            }

            return Attribute.IsDefined(m[0], typeof(T));
        }
    }
}
