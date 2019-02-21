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

namespace LcsSaveEditor.Core
{
    /// <summary>
    /// A log entry.
    /// </summary>
    public class LogItem
    {
        /// <summary>
        /// Creates a new <see cref="LogItem"/>.
        /// </summary>
        /// <param name="level">The item's importance level.</param>
        /// <param name="timestamp">The item's timestamp.</param>
        /// <param name="message">The item's message.</param>
        public LogItem(LogLevel level, DateTime timestamp, string message)
        {
            Level = level;
            Timestamp = timestamp;
            Message = message;
        }

        /// <summary>
        /// Gets this item's importance level.
        /// </summary>
        public LogLevel Level { get; }

        /// <summary>
        /// Gets this item's timestamp.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Gets this item's message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Converts this <see cref="LogItem"/> to a <see cref="string"/>.
        /// </summary>
        /// <returns>This item as a string.</returns>
        public override string ToString()
        {
            return string.Format("{0}  {1}  {2}",
                Timestamp.ToString("HH:mm:ss.fff"),
                Level.ToString().PadRight(5),
                Message);
        }
    }
}
