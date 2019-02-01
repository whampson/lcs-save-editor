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
using System.IO;
using System.Text;

namespace LcsSaveEditor.Models
{
    /// <summary>
    /// Represents a saved Grand Theft Auto: Liberty City Stories game
    /// formatted for the PlayStation 2 version of the game.
    /// </summary>
    public class SaveDataPS2 : SaveData
    {
        /// <summary>
        /// PS2 saves have a constant file size.
        /// </summary>
        private const int FileSize = 0x19000;

        public SaveDataPS2()
            : base(GamePlatform.PS2)
        { }

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                // Read data
                ReadDataBlock(stream, m_simpleVars);
                ReadDataBlock(stream, m_scripts);
                ReadDataBlock(stream, m_garages);
                ReadDataBlock(stream, m_playerInfo);
                ReadDataBlock(stream, m_stats);

                // Read padding (ignored)
                r.ReadBytes(FileSize - (int) stream.Position - 4);

                // Read checksum (ignored)
                r.ReadInt32();
            }

            DeserializeDataBlocks();

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            SerializeDataBlocks();

            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                // Data
                WriteBlockData(stream, m_simpleVars);
                WriteBlockData(stream, m_scripts);
                WriteBlockData(stream, m_garages);
                WriteBlockData(stream, m_playerInfo);
                WriteBlockData(stream, m_stats);

                // Padding
                int numPadding = FileSize - (int) stream.Position - 4;
                for (int i = 0; i < numPadding; i++) {
                    w.Write((byte) 0);
                }

                // Checksum
                w.Write(GetPS2Checksum(stream));
            }

            return stream.Position - start;
        }
    }
}
