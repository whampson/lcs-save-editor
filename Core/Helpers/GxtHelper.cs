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

using LcsSaveEditor.Core.Extensions;
using System.Collections.Generic;
using System.IO;

namespace LcsSaveEditor.Core.Helpers
{
    public static class GxtHelper
    {
        public static Dictionary<string, string> CurrentMainTable;

        static GxtHelper() {
            CurrentMainTable = new Dictionary<string, string>();
        }

        private class TKeyEntry
        {
            public int Offset { get; set; }
            public string Key { get; set; }
        }

        public static Dictionary<string, string> ReadMainTable(Stream stream)
        {
            Dictionary<string, string> main = new Dictionary<string, string>();

            using (BinaryReader r = new BinaryReader(stream)) {
                string key;
                string val;
                int off;
                int size;
                int n;
                long mark;

                // Read file header
                key = r.ReadString(4);      // 'TABL'
                if (key != "TABL") {
                    throw new InvalidDataException();
                }
                r.ReadInt32();              // size of table section

                // Find table offset
                key = r.ReadString(8);      // 'MAIN'
                if (key != "MAIN") {
                    throw new InvalidDataException();
                }
                stream.Position = r.ReadInt32();

                // Read key list header
                key = r.ReadString(4);      // 'TKEY'
                if (key != "TKEY") {
                    throw new InvalidDataException();
                }
                size = r.ReadInt32();       // size of key section

                // Read keys
                n = size / 12;
                TKeyEntry[] keys = new TKeyEntry[n];
                for (int i = 0; i < n; i++) {
                    off = r.ReadInt32();
                    key = r.ReadString(8);
                    keys[i] = new TKeyEntry() { Offset = off, Key = key };
                }

                // Read data header
                key = r.ReadString(4);      // 'TDAT'
                if (key != "TDAT") {
                    throw new InvalidDataException();
                }
                r.ReadInt32();              // size of data section

                // Read text data
                mark = stream.Position;
                foreach (TKeyEntry k in keys) {
                    stream.Position = mark + k.Offset;
                    val = r.ReadWideString();
                    main[k.Key] = val;
                }
            }

            return main;
        }
    }
}
