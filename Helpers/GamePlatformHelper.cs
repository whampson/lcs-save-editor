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

using LcsSaveEditor.DataTypes;
using System.ComponentModel;

namespace LcsSaveEditor.Helpers
{
    /// <summary>
    /// Utility class for the <see cref="GamePlatform"/> enum.
    /// </summary>
    public static class GamePlatformHelper
    {
        /// <summary>
        /// Gets the full platform name from the
        /// <see cref="GamePlatform"/>'s Description attribute.
        /// </summary>
        /// <param name="platform">The platform to get the full name of.</param>
        /// <returns>The platform's full name.</returns>
        public static string GetPlatformName(GamePlatform platform)
        {
            return EnumHelper.GetAttribute<DescriptionAttribute>(platform).Description;
        }
    }
}
