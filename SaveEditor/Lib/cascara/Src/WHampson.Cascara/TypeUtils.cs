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

namespace WHampson.Cascara
{
    /// <summary>
    /// Convenience class for miscellaneous tasks related to data types.
    /// </summary>
    internal static class TypeUtils
    {
        /// <summary>
        /// Checks whether a given type can be assigned to some generic type.
        /// </summary>
        /// <remarks>
        /// Adapted from code found at https://stackoverflow.com/questions/74616#1075059
        /// </remarks>
        /// <param name="givenType">
        /// The type to check whether it can be assigned to
        /// <paramref name="genericType"/>.
        /// </param>
        /// <param name="genericType">
        /// The generic type.
        /// </param>
        /// <returns>
        /// <code>True</code> if the <paramref name="givenType"/> can be assigned to
        /// the <paramref name="genericType"/>,
        /// <code>False</code> otherwise.
        /// </returns>
        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            Type[] interfaceTypes = givenType.GetInterfaces();

            foreach (Type type in interfaceTypes)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            Type baseType = givenType.BaseType;
            if (baseType == null)
            {
                return false;
            }

            return IsAssignableToGenericType(baseType, genericType);
        }
    }
}
