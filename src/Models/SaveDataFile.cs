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

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel;
using System.IO;
using WHampson.Cascara;
using WHampson.LcsSaveEditor.Helpers;
using WHampson.LcsSaveEditor.Properties;

namespace WHampson.LcsSaveEditor.Models
{
    public class SaveDataFile : IDisposable
    {
        public static SaveDataFile Load(string path)
        {
            bool valid = IsFileValid(path, out BinaryData data);
            if (!valid) {
                throw new InvalidDataException(Resources.InvalidDataMessage);
            }

            GamePlatform type = DetectFileType(data);
            if (!IsFileTypeSupported(type)) {
                string msg = string.Format(Resources.UnsupportedDataFormatMessage, GetGamePlatformName(type));
                throw new NotSupportedException(msg);
            }

            string scriptPath;
            switch (type) {
                case GamePlatform.Android:
                    scriptPath = "../../resources/scripts/androidsave.xml";
                    break;
                case GamePlatform.PS2:
                    scriptPath = "../../resources/scripts/ps2save.xml";
                    break;
                default:
                    scriptPath = string.Empty;
                    break;
            }

            LayoutScript script = LayoutScript.Load(scriptPath);
            data.RunLayoutScript(script);

            DeserializationFlags flags = 0;
            flags |= DeserializationFlags.Fields;
            flags |= DeserializationFlags.NonPublic;
            SaveDataFile sg = data.Deserialize<SaveDataFile>(flags);
            sg.FileType = type;
            sg.RawData = data;

            File.WriteAllText("out.json", sg.ToString());

            return sg;
        }

        private SimpleVarsBlock simpleVars;
        private ScriptsBlock scripts;
        private GaragesBlock garages;
        private PlayerBlock player;
        private StatsBlock stats;

        private Primitive<uint> checksum;

        public SaveDataFile()
        {
            simpleVars = new SimpleVarsBlock();
            scripts = new ScriptsBlock();
            garages = new GaragesBlock();
            player = new PlayerBlock();
            stats = new StatsBlock();

            checksum = new Primitive<uint>(null, 0);
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public GamePlatform FileType
        {
            get;
            private set;
        }

        public SimpleVarsBlock SimpleVars
        {
            get { return simpleVars; }
        }

        public ScriptsBlock Scripts
        {
            get { return scripts; }
        }

        public GaragesBlock Garages
        {
            get { return garages; }
        }

        public PlayerBlock Player
        {
            get { return player; }
        }

        public StatsBlock Stats
        {
            get { return stats; }
        }

        private uint Checksum
        {
            get { return checksum.Value; }
            set { checksum.Value = value; }
        }

        private BinaryData RawData
        {
            get;
            set;
        }

        public void Store(string path)
        {
            UpdateChecksum();
            RawData.Store(path);
        }

        private void UpdateChecksum()
        {
            if (FileType != GamePlatform.PS2) {
                return;
            }

            uint cksum = 0;
            byte[] data = RawData.ToArray();

            for (int i = 0; i < data.Length - sizeof(uint); i++) {
                cksum += data[i];
            }
            Checksum = cksum;
        }

        public override string ToString()
        {
            // TODO: this is a hotfix to prevent an exception from being thrown when
            // serailizing properties that don't exist for the curretn file type.
            // This is NOT a good way to handle this issue and should be corrected.
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Error = (sender, e) => e.ErrorContext.Handled = true
            };

            return JsonConvert.SerializeObject(this, Formatting.Indented, settings);
        }

        /// <summary>
        /// Determines whether a set of data is a valid GTA:LCS savedata file.
        /// </summary>
        private static bool IsFileValid(string path, out BinaryData data)
        {
            const int OneKibiByte = 1024;
            const int OneMebiByte = 1024 * 1024;

            using (FileStream fs = new FileStream(path, FileMode.Open)) {
                // Quick validation by checking file size
                if (fs.Length < OneKibiByte || fs.Length > OneMebiByte) {
                    data = default(BinaryData);
                    return false;
                }

                byte[] buf = new byte[fs.Length];
                fs.Read(buf, 0, buf.Length);

                data = new BinaryData(buf);
            }

            const string SimpTag = "SIMP";
            const string SrptTag = "SRPT";
            const string ScrTag = "SCR";
            const int SimpTagOffset = 0x000;
            const int SrptTagOffsetPs2 = 0x100;
            const int SrptTagOffsetMobile = 0x144;
            const int ScrTagOffsetPs2 = 0x108;
            const int ScrTagOffsetMobile = 0x14C;

            // Check for "SIMP" block signature
            bool simpFound = data.GetString(SimpTagOffset, 4).Equals(SimpTag);
            if (!simpFound) {
                data = default(BinaryData);
                return false;
            }

            //  Check for "SRPT" and "SCR" block signatures
            bool srptPS2Found = data.GetString(SrptTagOffsetPs2, 4).Equals(SrptTag);
            bool srptMobileFound = data.GetString(SrptTagOffsetMobile, 4).Equals(SrptTag);
            bool scrPS2Found = data.GetString(ScrTagOffsetPs2, 4).Equals(ScrTag);
            bool scrMobileFound = data.GetString(ScrTagOffsetMobile, 4).Equals(ScrTag);

            if (!(srptPS2Found && scrPS2Found) && !(srptMobileFound && scrMobileFound)) {
                data = default(BinaryData);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Attempts to determine the file type (game platform) of a GTA:LCS savedata file.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static GamePlatform DetectFileType(BinaryData data)
        {
            const int SimpSizePs2 = 0x0F8;
            const int MissionScriptSizeAndroid = 0x21C;
            const int MissionScriptSizeIos = 0x228;

            // Determine if PS2 by size of SIMP block.
            int sizeOfSimp = data.Get<int>(0x04);
            if (sizeOfSimp == SimpSizePs2) {
                return GamePlatform.PS2;
            }

            // Distinguish iOS and Android by size of MissionScript.
            int sizeOfSrpt = data.Get<int>(sizeOfSimp + 0x0C);
            int srptDataOffset = sizeOfSimp + 0x10;
            int scriptVarSpaceSize = data.Get<int>(srptDataOffset + 0x08);
            int scriptVarOffset = srptDataOffset + 0x0C;
            int numRunningScripts = data.Get<int>(scriptVarOffset + scriptVarSpaceSize + 0x7C0);
            int runningScriptsOffset = scriptVarOffset + scriptVarSpaceSize + 0x7C4;

            int sizeOfRunningScript = (sizeOfSrpt + srptDataOffset - runningScriptsOffset) / numRunningScripts;

            if (sizeOfRunningScript == MissionScriptSizeAndroid) {
                return GamePlatform.Android;
            }
            else if (sizeOfRunningScript == MissionScriptSizeIos) {
                return GamePlatform.IOS;
            }

            // PSP -- ???? (TODO)
            throw new InvalidDataException(Resources.UnknownDataFormatMessage);
        }

        private static bool IsFileTypeSupported(GamePlatform v)
        {
            return !EnumHelper.HasAttribute<NotSupportedAttribute>(v);
        }

        private static string GetGamePlatformName(GamePlatform v)
        {
            return EnumHelper.GetAttribute<DescriptionAttribute>(v).Description;
        }

        public void Dispose()
        {
            ((IDisposable) RawData).Dispose();
        }
    }
}
