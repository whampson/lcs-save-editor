#region License
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
using WHampson.Cascara;

namespace WHampson.LcsSaveEditor.Models.FileStructure
{
    public class StringWrapper
    {
        private readonly Primitive<Char8> str;
        private readonly Primitive<char> str16;

        public StringWrapper(Primitive<Char8> str)
        {
            if (!str.IsCollection) {
                throw new ArgumentException("Must be a collection.", nameof(str));
            }

            this.str = str;
            this.str16 = null;
        }

        public StringWrapper(Primitive<char> str)
        {
            if (!str.IsCollection) {
                throw new ArgumentException("Must be a collection.", nameof(str));
            }

            this.str = null;
            this.str16 = str;
        }

        private bool IsString16
        {
            get { return str16 != null; }
        }

        public string Value
        {
            get {
                if (IsString16) {
                    return GetString16Value();
                }

                return GetString8Value();
            }

            set {
                if (IsString16) {
                    SetString16Value(value);
                }
                else {
                    SetString8Value(value);
                }
            }
        }

        private string GetString8Value()
        {
            string value = "";
            for (int i = 0; i < str.Length; i++) {
                char c = str[i].Value;
                if (c == '\0') {
                    break;
                }
                value += c;
            }

            return value;
        }

        private void SetString8Value(string value)
        {
            int i = 0;
            while (i < value.Length && i < str.Length) {
                str[i].Value = value[i];
                i++;
            }
            if (i < str.Length) {
                str[i].Value = '\0';
            }
        }

        private string GetString16Value()
        {
            string value = "";
            for (int i = 0; i < str16.Length; i++) {
                char c = str16[i].Value;
                if (c == '\0') {
                    break;
                }
                value += c;
            }

            return value;
        }

        private void SetString16Value(string value)
        {
            int i = 0;
            while (i < value.Length && i < str16.Length) {
                str16[i].Value = value[i];
                i++;
            }
            if (i < str16.Length) {
                str16[i].Value = '\0';
            }
        }
    }
}
