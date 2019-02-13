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
using System.Collections.Generic;
using System.IO;

namespace LcsSaveEditor.Infrastructure
{
    /// <summary>
    /// A simple event logger.
    /// </summary>
    public static class Logger
    {
        private static List<LogItem> m_logItems;

        static Logger()
        {
            m_logItems = new List<LogItem>();
        }

        /// <summary>
        /// Add an entry to the log with the 'Debug' importance level.
        /// </summary>
        /// <param name="item">The item to add log.</param>
        public static void Debug(object item)
        {
            Log(LogLevel.Debug, item);
        }

        /// <summary>
        /// Add an entry to the log with the 'Info' importance level.
        /// </summary>
        /// <param name="item">The item to add log.</param>
        public static void Info(object item)
        {
            Log(LogLevel.Info, item);
        }

        /// <summary>
        /// Add an entry to the log with the 'Warn' importance level.
        /// </summary>
        /// <param name="item">The item to add log.</param>
        public static void Warn(object item)
        {
            Log(LogLevel.Warn, item);
        }

        /// <summary>
        /// Add an entry to the log with the 'Error' importance level.
        /// </summary>
        /// <param name="item">The item to add log.</param>
        public static void Error(object item)
        {
            Log(LogLevel.Error, item);
        }

        /// <summary>
        /// Add an entry to the log with the 'Fatal' importance level.
        /// </summary>
        /// <param name="item">The item to add log.</param>
        public static void Fatal(object item)
        {
            Log(LogLevel.Fatal, item);
        }

        /// <summary>
        /// Add an entry to the log.
        /// </summary>
        /// <param name="level">The entry's level of importance.</param>
        /// <param name="item">The item to add log.</param>
        public static void Log(LogLevel level, object item)
        {
            if (item == null) {
                return;
            }

            DateTime time = DateTime.Now;
            string[] lines = item.ToString().Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (string line in lines) {
                m_logItems.Add(new LogItem(level, time, line));
            }
        }

        /// <summary>
        /// Gets all items in the log as a read-only collection
        /// </summary>
        /// <returns>All log entires as a read-only collection.</returns>
        public static IReadOnlyCollection<LogItem> GetLogItems()
        {
            return m_logItems.AsReadOnly();
        }

        /// <summary>
        /// Writes all log entries to the specified file. If the file
        /// exists, its contents will be overwritten.
        /// </summary>
        /// <param name="path">The file to write.</param>
        /// <exception cref="System.ArgumentException"/>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.UnauthorizedAccessException"/>
        /// <exception cref="System.IO.DirectoryNotFoundException"/>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.IO.PathTooLongException"/>
        /// <exception cref="System.Security.SecurityException"/><
        public static void WriteLogFile(string path)
        {
            using (StreamWriter writer = new StreamWriter(path)) {
                foreach (LogItem item in m_logItems) {
                    writer.WriteLine(item);
                }
            }
        }
    }
}
