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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using WHampson.Gta3CarGenEditor.Extensions;
using WHampson.LcsSaveEditor.Helpers;
using WHampson.LcsSaveEditor.Resources;

namespace WHampson.LcsSaveEditor.Models
{
    public abstract class SaveData : SerializableObject
    {
        private const string SimpleVarsTag = "SIMP";
        private const string ScriptsTag = "SRPT";
        private const string GaragesTag = "GRGE";
        private const string PlayerInfoTag = "PLYR";
        private const string StatsTag = "STAT";

        protected DataBlock m_simpleVars;
        protected DataBlock m_scripts;
        protected DataBlock m_garages;
        protected DataBlock m_playerInfo;
        protected DataBlock m_stats;

        protected SaveData(GamePlatform fileType)
        {
            FileType = fileType;

            m_simpleVars = new DataBlock() { Tag = SimpleVarsTag };
            m_scripts = new DataBlock() { Tag = ScriptsTag };
            m_garages = new DataBlock() { Tag = GaragesTag };
            m_playerInfo = new DataBlock() { Tag = PlayerInfoTag };
            m_stats = new DataBlock() { Tag = StatsTag };
        }

        /// <summary>
        /// Gets the file format type based on the <see cref="GamePlatform"/>
        /// that created this save data.
        /// </summary>
        public GamePlatform FileType
        {
            get;
        }

        /// <summary>
        /// Writes this saved game data to a file.
        /// </summary>
        /// <param name="path">The file to write.</param>
        public void Store(string path)
        {
            byte[] data = Serialize(this);
            File.WriteAllBytes(path, data);
        }

        /// <summary>
        /// Computes the checksum that goes in the footer of the file.
        /// </summary>
        /// <param name="stream">The stream containing the serialized save data.</param>
        /// <returns>The serialized data checksum.</returns>
        protected int GetChecksum(Stream stream)
        {
            using (MemoryStream m = new MemoryStream()) {
                stream.Position = 0;
                stream.CopyTo(m);
                return m.ToArray().Sum(x => x);
            }
        }

        /// <summary>
        /// Sets the Data field of a <see cref="DataBlock"/> by
        /// reading data at the current position in a stream in
        /// accordance with the <see cref="DataBlock"/> parameters.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <param name="block">The block to populate.</param>
        /// <returns>The number of bytes read.</returns>
        protected int ReadDataBlock(Stream stream, DataBlock block)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                string tag;
                int blockSize;

                // Read block tag
                tag = Encoding.ASCII.GetString(r.ReadBytes(block.Tag.Length));
                if (tag != block.Tag) {
                    string msg = string.Format(Strings.ExceptionMessageInvalidBlockTag,
                        tag.StripNull(), block.Tag.StripNull());
                    throw new InvalidDataException(msg);
                }

                // Read block size
                blockSize = r.ReadInt32();
                if (blockSize > stream.Length) {
                    throw new InvalidDataException(Strings.ExceptionMessageIncorrectBlockSize);
                }

                // Read block data
                block.Data = r.ReadBytes(blockSize);
            }

            return (int) (stream.Position - start);
        }

        /// <summary>
        /// Creates a new <see cref="SaveData"/> object from binary
        /// data found inside the specified file.
        /// </summary>
        /// <param name="path">The path to the file to load.</param>
        /// <returns>The newly-created <see cref="SaveData"/>.</returns>
        /// <exception cref="PlatformNotSupportedException">
        /// Thrown if the file is a valid GTA:LCS save data file, but
        /// is from a gaming platform that is unsupported.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// Thrown if the file is not a valid GTA:LCS save data file.
        /// </exception>
        public static SaveData Load(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            GamePlatform fileType = DetectFileType(data);

            // Check if file type is supported by the editor
            if (EnumHelper.HasAttribute<NotSupportedAttribute>(fileType)) {
                string msg = string.Format(Strings.ExceptionMessageFileTypeNotSupported,
                    EnumHelper.GetAttribute<DescriptionAttribute>(fileType).Description);
                throw new PlatformNotSupportedException(msg);
            }

            switch (fileType) {
                case GamePlatform.Android:
                    return Deserialize<SaveDataAndroid>(data);
                //case GamePlatform.IOS:
                //    return Deserialize<SaveDataFileIOS>(data);
                case GamePlatform.PS2:
                    return Deserialize<SaveDataPS2>(data);
                default:
                    // Should never get here...
                    string msg = string.Format("{0} ({1})",
                        Strings.ExceptionMessageOops, nameof(SaveData));
                    throw new InvalidOperationException(msg);
            }
        }

        /// <summary>
        /// Unpacks all data blocks into their respective data fields.
        /// </summary>
        protected void DeserializeDataBlocks()
        {
            // TODO: finish
        }

        /// <summary>
        /// Serializes all data fields and stores the result in the respective
        /// data blocks.
        /// </summary>
        protected void SerializeDataBlocks()
        {
            // TODO
        }

        /// <summary>
        /// Attempts to determine the file type (game platform) of a GTA:LCS savedata file.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static GamePlatform DetectFileType(byte[] data)
        {
            const int SimpSizePs2 = 0x0F8;
            const int MissionScriptSizeAndroid = 0x21C;
            const int MissionScriptSizeIos = 0x228;

            // Determine if PS2 by size of SIMP block.
            int sizeOfSimp = ReadInt(data, 0x04);
            if (sizeOfSimp == SimpSizePs2) {
                return GamePlatform.PS2;
            }

            // Distinguish iOS and Android by size of MissionScript.
            int sizeOfSrpt = ReadInt(data, sizeOfSimp + 0x0C);
            int srptDataOffset = sizeOfSimp + 0x10;
            int scriptVarSpaceSize = ReadInt(data, srptDataOffset + 0x08);
            int scriptVarOffset = srptDataOffset + 0x0C;
            int numRunningScripts = ReadInt(data, scriptVarOffset + scriptVarSpaceSize + 0x7C0);
            int runningScriptsOffset = scriptVarOffset + scriptVarSpaceSize + 0x7C4;

            int sizeOfRunningScript = (sizeOfSrpt + srptDataOffset - runningScriptsOffset) / numRunningScripts;

            if (sizeOfRunningScript == MissionScriptSizeAndroid) {
                return GamePlatform.Android;
            }
            else if (sizeOfRunningScript == MissionScriptSizeIos) {
                return GamePlatform.IOS;
            }

            // PSP -- ???? (TODO)
            throw new InvalidDataException(Strings.ExceptionMessageInvalidSaveData);
        }

        /// <summary>
        /// Reads a 32-bit integer from an arbitrary address in an array.
        /// </summary>
        private static int ReadInt(byte[] data, int addr)
        {
            return BitConverter.ToInt32(data, addr);
        }
    }
}
