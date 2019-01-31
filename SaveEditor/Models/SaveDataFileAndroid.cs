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
using System.IO;
using System.Text;

namespace WHampson.LcsSaveEditor.Models
{
    public class SaveDataFileAndroid : SaveDataFile
    {
        public SaveDataFileAndroid()
            : base(GamePlatform.Android)
        { }

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                ReadDataBlock(stream, m_simpleVars);
                ReadDataBlock(stream, m_scripts);
                ReadDataBlock(stream, m_garages);
                ReadDataBlock(stream, m_playerInfo);
                ReadDataBlock(stream, m_stats);
                r.ReadBytes(3);                 // Read trailing 3 bytes (ignored)
            }

            DeserializeDataBlocks();

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
