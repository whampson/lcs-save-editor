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
using LcsSaveEditor.Extensions;
using LcsSaveEditor.Infrastructure;
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

        public GamePlatform FileType
        {
            get { return m_fileType; }
        }

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
        /// Creates a new <see cref="SaveData"/> object by parsing bytes
        /// from the specified file.
        /// </summary>
        /// <param name="path">The path to the file to load.</param>
        /// <returns>The newly-created <see cref="SaveData"/>.</returns>
        /// <exception cref="InvalidDataException">
        /// Thrown if the file is not a valid save data file.
        /// </exception>
        /// <exception cref="IOException">
        /// Thrown if an I/O error occurs.
        /// </exception>
        public static SaveData Load(string path)
        {
            GamePlatform fileType;
            byte[] rawData;

            // TODO: decrypt PSP saves (?)

            Logger.Info(CommonResources.Info_LoadingGtaData);

            // TODO: use stream

            rawData = File.ReadAllBytes(path);
            fileType = DetectFileType(rawData);

            SaveData data;
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

        public void Store(string path)
        {
            Logger.Info(CommonResources.Info_SavingGtaData);

            byte[] data = Serialize(this);
            File.WriteAllBytes(path, data);

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
        
        private static GamePlatform DetectFileType(byte[] data)
        {
            // TODO: rewrite using stream
            const int SimpSizePS2 = 0x0F8;
            const int SimpSizePSP = 0x0BC;
            const int RunningScriptSizeAndroid = 0x21C;
            const int RunningScriptSizeIOS = 0x228;

            // Determine if PS2 or PSP by size of SIMP block.
            int sizeOfSimp = ReadInt(data, 0x04);
            if (sizeOfSimp == SimpSizePS2) {
                return GamePlatform.PS2;
            }
            else if (sizeOfSimp == SimpSizePSP) {
                return GamePlatform.PSP;
            }

            // Distinguish iOS and Android by size of RunningScript.
            int sizeOfSrpt = ReadInt(data, sizeOfSimp + 0x0C);
            int srptDataOffset = sizeOfSimp + 0x10;
            int scriptVarSpaceSize = ReadInt(data, srptDataOffset + 0x08);
            int scriptVarOffset = srptDataOffset + 0x0C;
            int numRunningScripts = ReadInt(data, scriptVarOffset + scriptVarSpaceSize + 0x7C0);
            int runningScriptsOffset = scriptVarOffset + scriptVarSpaceSize + 0x7C4;
            int sizeOfRunningScript = (sizeOfSrpt + srptDataOffset - runningScriptsOffset) / numRunningScripts;
            if (sizeOfRunningScript == RunningScriptSizeAndroid) {
                return GamePlatform.Android;
            }
            else if (sizeOfRunningScript == RunningScriptSizeIOS) {
                return GamePlatform.IOS;
            }

            // Not valid!
            throw new InvalidDataException(CommonResources.Error_BadSaveData);
        }

        private static int ReadInt(byte[] data, int index)
        {
            return BitConverter.ToInt32(data, index);
        }
    }
}
