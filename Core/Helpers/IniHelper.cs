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

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LcsSaveEditor.Core.Helpers
{
    /// <summary>
    /// Helper class for reading and writing .ini files.
    /// </summary>
    public static class IniHelper
    {
        private const int MaxValueLength = 256;
        private const char SeparatorChar = '=';
        private const char CommentChar = ';';

        /// <summary>
        /// Reads all keys from the specified .ini file and stores them in a dictionary.
        /// Sections are ignored.
        /// </summary>
        /// <param name="iniPath">The .ini file to read.</param>
        /// <returns>A dictionary of strings containing all the key-value pairs.</returns>
        /// <exception cref="System.UnauthorizedAccessException"/>
        /// <exception cref="System.IO.FileNotFoundException"/>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Security.SecurityException"/>
        public static Dictionary<string, string> ReadAllKeys(string iniPath)
        {
            return File.ReadLines(iniPath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Where(line => !line.Trim().StartsWith(CommentChar.ToString()))
                .Select(line => line.Split(new char[] { SeparatorChar }, 2))
                .ToDictionary(pair => pair[0], pair => pair[1]);
        }

        /// <summary>
        /// Writes all key-value pars from the specified dictionary to the
        /// specified .ini file. If the .ini file exists, its contents will
        /// be overwritten.
        /// </summary>
        /// <param name="iniPath">The .ini file to write.</param>
        /// <param name="keys">The key-value pairs to write.</param>
        /// <exception cref="System.UnauthorizedAccessException"/>
        /// <exception cref="System.IO.FileNotFoundException"/>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Security.SecurityException"/>
        public static void WriteAllKeys(string iniPath, Dictionary<string, string> keys)
        { 
            File.WriteAllLines(iniPath, keys
                .Select(pair => string.Format("{0}{1}{2}", pair.Key, SeparatorChar, pair.Value)));
        }

        /// <summary>
        /// Appends all key-value pars from the specified dictionary to the
        /// specified .ini file.
        /// </summary>
        /// <param name="iniPath">The .ini file to append.</param>
        /// <param name="keys">The key-value pairs to write.</param>
        /// <exception cref="System.UnauthorizedAccessException"/>
        /// <exception cref="System.IO.FileNotFoundException"/>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Security.SecurityException"/>
        public static void AppendAllKeys(string iniPath, Dictionary<string, string> keys)
        {
            File.AppendAllLines(iniPath, keys
                .Select(pair => string.Format("{0}{1}{2}", pair.Key, SeparatorChar, pair.Value)));
        }

        /// <summary>
        /// Writes a comment to the specified .ini file. If the
        /// .ini file exists, its contents will be overwritten.
        /// </summary>
        /// <param name="iniPath">The .ini file to append.</param>
        /// <param name="comment">The comment to write.</param>
        /// <exception cref="System.UnauthorizedAccessException"/>
        /// <exception cref="System.IO.FileNotFoundException"/>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Security.SecurityException"/>
        public static void WriteComment(string iniPath, string comment)
        {
            File.WriteAllText(iniPath, string.Format("{0} {1}\n", CommentChar, comment));
        }

        /// <summary>
        /// Appends a comment to the specified .ini file.
        /// </summary>
        /// <param name="iniPath">The .ini file to append.</param>
        /// <param name="comment">The comment to write.</param>
        /// <exception cref="System.UnauthorizedAccessException"/>
        /// <exception cref="System.IO.FileNotFoundException"/>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Security.SecurityException"/>
        public static void AppendComment(string iniPath, string comment)
        {
            File.AppendAllText(iniPath, string.Format("{0} {1}\n", CommentChar, comment));
        }
    }
}
