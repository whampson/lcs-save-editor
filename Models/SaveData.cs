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

using LcsSaveEditor.Core;
using LcsSaveEditor.Core.Extensions;
using LcsSaveEditor.Resources;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace LcsSaveEditor.Models
{
    /// <summary>
    /// Represents a saved Grand Theft Auto: Liberty City Stories game.
    /// </summary>
    public abstract class SaveData : SerializableObject
    {
        // Block header tags
        private const string SimpleVarsTag = "SIMP";
        private const string ScriptsTag = "SRPT";
        private const string ScriptsTag2 = "SCR";
        private const string GaragesTag = "GRGE";
        private const string PlayerInfoTag = "PLYR";
        private const string StatsTag = "STAT";
        
        // Raw block data
        protected DataBlock m_block0;
        protected DataBlock m_block1;
        protected DataBlock m_block2;
        protected DataBlock m_block3;
        protected DataBlock m_block4;

        // Deserialized data
        private SimpleVars m_simpleVars;
        private Scripts m_scripts;
        private Garages m_garages;
        private PlayerInfo m_playerInfo;
        private Stats m_stats;

        private GamePlatform m_fileType;

        protected SaveData(GamePlatform fileType)
        {
            m_fileType = fileType;

            m_block0 = new DataBlock(SimpleVarsTag);
            m_block1 = new DataBlock(ScriptsTag, ScriptsTag2);
            m_block2 = new DataBlock(GaragesTag);
            m_block3 = new DataBlock(PlayerInfoTag);
            m_block4 = new DataBlock(StatsTag);
        }

        /// <summary>
        /// Gets the <see cref="GamePlatform"/> indicating which
        /// version of the game this save is compatible with.
        /// </summary>
        public GamePlatform FileType
        {
            get { return m_fileType; }
        }

        /// <summary>
        /// Gets or sets a data structure containing miscellaneous game variables.
        /// </summary>
        public SimpleVars SimpleVars
        {
            get { return m_simpleVars; }
            set {
                if (m_simpleVars != null) {
                    m_simpleVars.PropertyChanged -= SimpleVars_PropertyChanged;
                }
                m_simpleVars = value;
                m_simpleVars.PropertyChanged += SimpleVars_PropertyChanged;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a data structure containing the game's mission script states.
        /// </summary>
        public Scripts Scripts
        {
            get { return m_scripts; }
            set {
                if (m_scripts != null) {
                    m_scripts.PropertyChanged -= Scripts_PropertyChanged;
                }
                m_scripts = value;
                m_scripts.PropertyChanged += Scripts_PropertyChanged;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a data structure containing information regarding the
        /// game's garages.
        /// </summary>
        public Garages Garages
        {
            get { return m_garages; }
            set {
                if (m_garages != null) {
                    m_garages.PropertyChanged -= Garages_PropertyChanged;
                }
                m_garages = value;
                m_garages.PropertyChanged += Garages_PropertyChanged;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a data structure containing player attributes.
        /// </summary>
        public PlayerInfo PlayerInfo
        {
            get { return m_playerInfo; }
            set {
                if (m_playerInfo != null) {
                    m_playerInfo.PropertyChanged -= PlayerInfo_PropertyChanged;
                }
                m_playerInfo = value;
                m_playerInfo.PropertyChanged += PlayerInfo_PropertyChanged;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a data structure containing game statistics.
        /// </summary>
        public Stats Stats
        {
            get { return m_stats; }
            set {
                if (m_stats != null) {
                    m_stats.PropertyChanged -= Stats_PropertyChanged;
                }
                m_stats = value;
                m_stats.PropertyChanged += Stats_PropertyChanged;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Checks whether a file is a valid GTA:LCS save data file.
        /// </summary>
        /// <param name="fs">An open file stream.</param>
        /// <returns>True if the file is valid, false otherwise.</returns>
        private static bool IsFileValid(FileStream fs)
        {
            fs.Position = 0;

            try {
                using (BinaryReader r = new BinaryReader(fs, Encoding.Default, true)) {
                    string tag;
                    int size;

                    // SIMP
                    tag = r.ReadString(4);
                    if (tag != SimpleVarsTag) {
                        return false;
                    }
                    size = r.ReadInt32();
                    r.ReadBytes(size);

                    // SRPT
                    tag = r.ReadString(4);
                    if (tag != ScriptsTag) {
                        return false;
                    }
                    size = r.ReadInt32();
                    r.ReadBytes(size);

                    // GRGE
                    tag = r.ReadString(4);
                    if (tag != GaragesTag) {
                        return false;
                    }
                    size = r.ReadInt32();
                    r.ReadBytes(size);

                    // PLYR
                    tag = r.ReadString(4);
                    if (tag != PlayerInfoTag) {
                        return false;
                    }
                    size = r.ReadInt32();
                    r.ReadBytes(size);

                    // STAT
                    tag = r.ReadString(4);
                    if (tag != StatsTag) {
                        return false;
                    }
                    size = r.ReadInt32();
                    r.ReadBytes(size);
                }
            }
            catch (EndOfStreamException) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets a <see cref="GamePlatform"/> value indicating which
        /// game system a GTA:LCS save data file is compatible with.
        /// </summary>
        /// <param name="fs">An open file stream.</param>
        /// <returns>
        /// A <see cref="GamePlatform"/> value indicating which game
        /// system the file is compatible with, null if the type
        /// cannot be determined.
        /// </returns>
        private static GamePlatform? GetFileType(FileStream fs)
        {
            const int SimpSizePS2 = 0xF8;
            const int SimpSizePSP = 0xBC;
            const int RunningScriptSizeAndroid = 0x21C;
            const int RunningScriptSizeIOS = 0x228;

            fs.Position = 0;
            using (BinaryReader r = new BinaryReader(fs, Encoding.Default, true)) {
                int size;
                int count;
                int srptSize;
                long srptOffset;

                /* == Distinguish PS2 and PSP by size of SIMP block == */

                r.ReadString(4);        // "SIMP"
                size = r.ReadInt32();
                if (size == SimpSizePS2) {
                    return GamePlatform.PS2;
                }
                else if (size == SimpSizePSP) {
                    return GamePlatform.PSP;
                }

                /* == Distinguish Android and iOS by size of RunningScript structure == */

                // Read past simple vars
                r.ReadBytes(size);

                // Read past global variables
                r.ReadString(4);        // "SRPT"
                srptSize = r.ReadInt32();
                srptOffset = fs.Position;
                r.ReadString(4);        // "SCR\0"
                r.ReadInt32();
                size = r.ReadInt32();
                r.ReadBytes(size);

                // Read past other script info
                r.ReadBytes(0x7C0);

                // Get size of RunningScript
                count = r.ReadInt16();
                size = (int) ((srptOffset + srptSize - fs.Position) / count);
                if (size == RunningScriptSizeAndroid) {
                    return GamePlatform.Android;
                }
                else if (size == RunningScriptSizeIOS) {
                    return GamePlatform.IOS;
                }
            }

            Logger.Warn(CommonResources.Info_FileTypeUnknown);
            return null;
        }

        /// <summary>
        /// Loads a GTA:LCS save data file from disk and creates a new
        /// <see cref="SaveData"/> using the data in the file.
        /// </summary>
        /// <param name="path">The file to load.</param>
        /// <returns>The newly-created <see cref="SaveData"/>.</returns>
        /// <exception cref="System.IO.InvalidDataException">
        /// Thrown if the file is not a valid GTA:LCS save data file.
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException"/>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Security.SecurityException"/>
        public static SaveData Load(string path)
        {
            // TODO: decrypt PSP saves?

            SaveData data;
            GamePlatform? fileType;

            Logger.Info(CommonResources.Info_LoadingGtaData);

            using (FileStream fs = new FileStream(path, FileMode.Open)) {
                if (!IsFileValid(fs)) {
                    throw new InvalidDataException(CommonResources.Error_InvalidGtaSaveData);
                }

                fileType = GetFileType(fs);
                if (fileType == null) {
                    throw new InvalidDataException(CommonResources.Info_FileTypeUnknown);
                }
            }

            byte[] rawData = File.ReadAllBytes(path);

            switch (fileType) {
                case GamePlatform.Android:
                    data = Deserialize<SaveDataAndroid>(rawData);
                    break;
                case GamePlatform.IOS:
                    data = Deserialize<SaveDataIOS>(rawData);
                    break;
                case GamePlatform.PS2:
                    data = Deserialize<SaveDataPS2>(rawData);
                    break;
                case GamePlatform.PSP:
                    data = Deserialize<SaveDataPSP>(rawData);
                    break;
                default:
                    string msg = string.Format(CommonResources.Error_Oops, nameof(Load));
                    throw new InvalidOperationException(msg);
            }

            Logger.Info(CommonResources.Info_LoadSuccess);
            Logger.Info(CommonResources.Info_FileFormat, fileType);
            Logger.Info(CommonResources.Info_FileSize, rawData.Length);

            return data;
        }

        /// <summary>
        /// Writes this <see cref="SaveData"/> to a file on disk.
        /// </summary>
        /// <param name="path">The file to write.</param>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Security.SecurityException"/>
        public void Store(string path)
        {
            Logger.Info(CommonResources.Info_SavingGtaData);
            File.WriteAllBytes(path, Serialize(this));
            Logger.Info(CommonResources.Info_SaveSuccess);
        }

        protected void DeserializeDataBlocks()
        {
            DeserializeSimpleVars();
            DeserializeScripts();
            DeserializeGarages();
            DeserializePlayerInfo();
            DeserializeStats();

            m_simpleVars.PropertyChanged += SimpleVars_PropertyChanged;
            m_scripts.PropertyChanged += Scripts_PropertyChanged;
            m_garages.PropertyChanged += Garages_PropertyChanged;
            m_playerInfo.PropertyChanged += PlayerInfo_PropertyChanged;
            m_stats.PropertyChanged += Stats_PropertyChanged;
        }

        private void DeserializeSimpleVars()
        {
            switch (FileType) {
                case GamePlatform.Android:
                case GamePlatform.IOS:
                    m_simpleVars = Deserialize<SimpleVarsAndroidIOS>(m_block0.Data);
                    break;
                case GamePlatform.PS2:
                    m_simpleVars = Deserialize<SimpleVarsPS2>(m_block0.Data);
                    break;
                case GamePlatform.PSP:
                    m_simpleVars = Deserialize<SimpleVarsPSP>(m_block0.Data);
                    break;
            }
        }

        private void DeserializeScripts()
        {
            switch (FileType) {
                case GamePlatform.Android:
                case GamePlatform.PS2:
                case GamePlatform.PSP:
                    m_scripts = Deserialize<Scripts<RunningScriptAndroidPS2PSP>>(m_block1.Data);
                    break;
                case GamePlatform.IOS:
                    m_scripts = Deserialize<Scripts<RunningScriptIOS>>(m_block1.Data);
                    break;
            }
        }

        private void DeserializeGarages()
        {
            switch (FileType) {
                case GamePlatform.Android:
                case GamePlatform.IOS:
                    m_garages = Deserialize<Garages<GarageAndroidIOS>>(m_block2.Data);
                    break;
                case GamePlatform.PS2:
                    m_garages = Deserialize<Garages<GaragePS2>>(m_block2.Data);
                    break;
                case GamePlatform.PSP:
                    m_garages = Deserialize<Garages<GaragePSP>>(m_block2.Data);
                    break;
            }
        }

        private void DeserializePlayerInfo()
        {
            switch (FileType) {
                case GamePlatform.Android:
                case GamePlatform.IOS:
                    m_playerInfo = Deserialize<PlayerInfoAndroidIOS>(m_block3.Data);
                    break;
                case GamePlatform.PS2:
                    m_playerInfo = Deserialize<PlayerInfoPS2>(m_block3.Data);
                    break;
                case GamePlatform.PSP:
                    m_playerInfo = Deserialize<PlayerInfoPSP>(m_block3.Data);
                    break;
            }
        }

        private void DeserializeStats()
        {
            switch (FileType) {
                case GamePlatform.Android:
                case GamePlatform.IOS:
                    m_stats = Deserialize<Stats<FavoriteRadioStationListAndroidIOS>>(m_block4.Data);
                    break;
                case GamePlatform.PS2:
                case GamePlatform.PSP:
                    m_stats = Deserialize<Stats<FavoriteRadioStationListPS2PSP>>(m_block4.Data);
                    break;
            }
        }

        protected void SerializeDataBlocks()
        {
            m_block0.Data = Serialize(m_simpleVars);
            m_block1.Data = Serialize(m_scripts);
            m_block2.Data = Serialize(m_garages);
            m_block3.Data = Serialize(m_playerInfo);
            m_block4.Data = Serialize(m_stats);
        }

        protected int ComputeChecksum(Stream stream)
        {
            using (MemoryStream m = new MemoryStream()) {
                stream.Position = 0;
                stream.CopyTo(m);
                return m.ToArray().Sum(x => x);
            }
        }

        protected int ReadDataBlock(Stream stream, DataBlock block)
        {
            Logger.Info(CommonResources.Info_ReadingBlock, block.Tag);

            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                string tag = r.ReadString(4);
                if (tag != block.Tag) {
                    string msg = string.Format(CommonResources.Error_BadBlockTag,
                        tag, block.Tag);
                    throw new InvalidDataException(msg);
                }

                int blockSize = r.ReadInt32();
                if (blockSize > stream.Length) {
                    string msg = string.Format(CommonResources.Error_BadBlockSize,
                        blockSize, stream.Length);
                    throw new InvalidDataException(msg);
                }

                if (block.NestedTag != null) {
                    tag = r.ReadString(4);
                    if (tag != block.NestedTag) {
                        string msg = string.Format(CommonResources.Error_BadBlockTag,
                            tag, block.Tag);
                        throw new InvalidDataException(msg);
                    }

                    blockSize = r.ReadInt32();
                    if (blockSize > stream.Length) {
                        string msg = string.Format(CommonResources.Error_BadBlockSize,
                            blockSize, stream.Length);
                        throw new InvalidDataException(msg);
                    }
                }

                block.Data = r.ReadBytes(blockSize);

                if (block.Data.Length != blockSize) {
                    Logger.Warn(CommonResources.Error_IncorrectBlockSize, blockSize, block.Data.Length);
                }
            }

            return (int) (stream.Position - start);
        }

        protected int WriteBlockData(Stream stream, DataBlock block)
        {
            Logger.Info(CommonResources.Info_WritingBlock, block.Tag);

            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.WriteString(block.Tag, 4);
                if (block.NestedTag != null) {
                    w.Write(block.Data.Length + 8);
                    w.WriteString(block.NestedTag, 4);
                }
                w.Write(block.Data.Length);
                w.Write(block.Data);
            }

            return (int) (stream.Position - start);
        }

        private void SimpleVars_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SimpleVars));
        }

        private void Scripts_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Scripts));
        }

        private void Garages_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Garages));
        }

        private void PlayerInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(PlayerInfo));
        }

        private void Stats_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Stats));
        }
    }
}
