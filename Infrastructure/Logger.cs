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
using System.Text;

namespace LcsSaveEditor.Infrastructure
{
    /// <summary>
    /// A simple event logger.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Default <see cref="LogLevel"/> when the standard output stream
        /// is redirected to the logger.
        /// </summary>
        public static readonly LogLevel DefaultStandardOutLevel = LogLevel.Debug;

        /// <summary>
        /// Default <see cref="LogLevel"/> when the standard error stream
        /// is redirected to the logger.
        /// </summary>
        public static readonly LogLevel DefaultStandardErrorLevel = LogLevel.Error;

        private static List<LogItem> m_logItems;
        private static LogWriter m_stdout;
        private static LogWriter m_stderr;

        static Logger()
        {
            m_logItems = new List<LogItem>();
            m_stdout = new LogWriter();
            m_stderr = new LogWriter();

            m_stdout.NewLineAdded += OutputStream_NewLineAdded;
            m_stderr.NewLineAdded += OutputStream_NewLineAdded;

            StandardOutLogLevel = DefaultStandardOutLevel;
            StandardErrorLogLevel = DefaultStandardErrorLevel;
        }

        /// <summary>
        /// Gets or sets the current <see cref="LogLevel"/> for the standard output stream.
        /// </summary>
        public static LogLevel StandardOutLogLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current <see cref="LogLevel"/> for the standard error stream.
        /// </summary>
        public static LogLevel StandardErrorLogLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Causes the standard output stream to be redirected to the logger.
        /// </summary>
        public static void ConsumeStandardOut()
        {
            Console.SetOut(m_stdout);
        }

        /// <summary>
        /// Causes the standard error stream to be redirected to the logger.
        /// </summary>
        public static void ConsumeStandardError()
        {
            Console.SetError(m_stderr);
        }

        /// <summary>
        /// Adds an entry to the log with the 'Debug' importance level.
        /// </summary>
        /// <param name="item">The item to add log.</param>
        public static void Debug(object item)
        {
            Log(LogLevel.Debug, item);
        }

        /// <summary>
        /// Adds an entry to the log with the 'Info' importance level.
        /// </summary>
        /// <param name="item">The item to add log.</param>
        public static void Info(object item)
        {
            Log(LogLevel.Info, item);
        }

        /// <summary>
        /// Adds an entry to the log with the 'Warn' importance level.
        /// </summary>
        /// <param name="item">The item to add log.</param>
        public static void Warn(object item)
        {
            Log(LogLevel.Warn, item);
        }

        /// <summary>
        /// Adds an entry to the log with the 'Error' importance level.
        /// </summary>
        /// <param name="item">The item to add log.</param>
        public static void Error(object item)
        {
            Log(LogLevel.Error, item);
        }

        /// <summary>
        /// Adds an entry to the log with the 'Fatal' importance level.
        /// </summary>
        /// <param name="item">The item to add log.</param>
        public static void Fatal(object item)
        {
            Log(LogLevel.Fatal, item);
        }

        /// <summary>
        /// Adds an entry to the log.
        /// </summary>
        /// <param name="level">The entry's level of importance.</param>
        /// <param name="item">The item to add log.</param>
        public static void Log(LogLevel level, object item)
        {
            if (item == null) {
                return;
            }

            string[] lines = item.ToString()
                .Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            DateTime time = DateTime.Now;
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

        /// <summary>
        /// Handles the <see cref="LogWriter.NewLineAdded"/> event
        /// for the standard output and standard error <see cref="LogWriter"/>s.
        /// </summary>
        /// <param name="sender">The <see cref="LogWriter"/> that raised the event.</param>
        /// <param name="e">Empty <see cref="EventArgs"/>.</param>
        private static void OutputStream_NewLineAdded(object sender, EventArgs e)
        {
            Queue<string> lineBuffer;
            LogLevel level;

            if (sender == m_stdout) {
                lineBuffer = m_stdout.LineBuffer;
                level = StandardOutLogLevel;
            }
            else if (sender == m_stderr) {
                lineBuffer = m_stderr.LineBuffer;
                level = StandardErrorLogLevel;
            }
            else {
                return;
            }

            while (lineBuffer.Count != 0) {
                Log(level, lineBuffer.Dequeue());
            }
        }

        /// <summary>
        /// A line-buffered TextWriter for use with the
        /// <see cref="Logger"/> to consume output streams.
        /// </summary>
        private class LogWriter : TextWriter
        {
            /// <summary>
            /// Fires when a newline has been added to the buffer.
            /// </summary>
            public event EventHandler NewLineAdded;

            private string m_buffer;

            /// <summary>
            /// Creates a new <see cref="LogWriter"/>.
            /// </summary>
            public LogWriter()
                : base()
            {
                m_buffer = string.Empty;
                LineBuffer = new Queue<string>();
            }

            /// <summary>
            /// Gets the current line buffer queue.
            /// </summary>
            public Queue<string> LineBuffer
            {
                get;
            }

            /// <summary>
            /// Gets the text writer encoding.
            /// </summary>
            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }

            /// <summary>
            /// Adds a character to the line buffer.
            /// Carriage Return characters are ignored.
            /// </summary>
            /// <param name="value">The character to add.</param>
            public override void Write(char value)
            {
                // Throw away carriage returns to make newline detection easier
                if (value == '\r') {
                    return;
                }

                if (value == '\n') {
                    OnNewLineAdded();
                }
                else {
                    m_buffer += value;
                }
            }

            /// <summary>
            /// Raises the <see cref="NewLineAdded"/> event.
            /// </summary>
            private void OnNewLineAdded()
            {
                LineBuffer.Enqueue(m_buffer);
                m_buffer = string.Empty;

                NewLineAdded?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
