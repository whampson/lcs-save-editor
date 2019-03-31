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
using System.Reflection;
using System.Text;

namespace LcsSaveEditor.Core.Helpers
{
    public static class VersionHelper
    {
        public static string GetAppVersionString()
        {
            Version vers = Assembly.GetExecutingAssembly().GetName().Version;
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0}.{1}", vers.Major, vers.Minor);
#if !DEBUG
            if (vers.Build != 0) {
                sb.Append("." + vers.Build);
            }
#else
            sb.AppendFormat(".{0}.{1}", vers.Build, vers.Revision);
#endif

            return sb.ToString();
        }

        public static string GetAppVersionStringFull()
        {
            Version vers = Assembly.GetExecutingAssembly().GetName().Version;
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0}.{1}.{2}.{3}", vers.Major, vers.Minor, vers.Build, vers.Revision);
            return sb.ToString();
        }
    }
}
