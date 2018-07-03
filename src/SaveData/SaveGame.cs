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
using System.IO;
using WHampson.Cascara;

namespace WHampson.LcsSaveEditor.SaveData
{
    internal class SaveGame
    {
        public static SaveGame Load(string path)
        {
            bool valid = IsFileValid(path, out BinaryData data);
            if (!valid) {
                throw new InvalidDataException("Invalid savedata file.");
            }

            GameVersion version = DetectVersion(data);
            if (!IsGameVersionSupported(version)) {
                throw new NotSupportedException(GetGameVersionName(version) + " saves are not supported yet.");
            }

            string scriptPath = "../../resources/scripts/ps2save.xml";
            LayoutScript script = LayoutScript.Load(scriptPath);
            data.RunLayoutScript(script);

            DeserializationFlags flags = 0;
            flags |= DeserializationFlags.Fields;
            flags |= DeserializationFlags.NonPublic;
            SaveGame sg = data.Deserialize<SaveGame>(flags);

            sg.GameVersion = version;
            sg.RawData = data;

            return sg;
        }

        private SimpleVarsBlock simpleVars;
        private ScriptsBlock scripts;
        private GaragesBlock garages;
        private PlayerBlock player;
        private StatsBlock stats;

        private Primitive<uint> checksum;

        public SaveGame()
        {
            simpleVars = new SimpleVarsBlock();
            scripts = new ScriptsBlock();
            garages = new GaragesBlock();
            player = new PlayerBlock();
            stats = new StatsBlock();

            checksum = new Primitive<uint>(null, 0);
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public GameVersion GameVersion
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
            if (GameVersion != GameVersion.Ps2) {
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
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

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

            const int SimpTagOffset = 0x000;
            const int SrptTagOffsetPs2 = 0x100;
            const int SrptTagOffsetMobile = 0x144;
            const int ScrTagOffsetPs2 = 0x108;
            const int ScrTagOffsetMobile = 0x14C;

            // Check for "SIMP" block signature
            bool simpFound = data.GetString(SimpTagOffset, 4).Equals("SIMP");
            if (!simpFound) {
                data = default(BinaryData);
                return false;
            }

            //  Check for "SRPT" and "SCR" block signatures
            bool srptPS2Found = data.GetString(SrptTagOffsetPs2, 4).Equals("SRPT");
            bool srptMobileFound = data.GetString(SrptTagOffsetMobile, 4).Equals("SRPT");
            bool scrPS2Found = data.GetString(ScrTagOffsetPs2, 4).Equals("SCR");
            bool scrMobileFound = data.GetString(ScrTagOffsetMobile, 4).Equals("SCR");

            if (!(srptPS2Found && scrPS2Found) && !(srptMobileFound && scrMobileFound)) {
                data = default(BinaryData);
                return false;
            }

            return true;
        }

        private static GameVersion DetectVersion(BinaryData data)
        {
            const int SimpSizePs2 = 0x0F8;
            const int MissionScriptSizeAndroid = 0x21C;
            const int MissionScriptSizeIos = 0x228;

            // Determine if PS2 by size of SIMP block.
            int sizeOfSimp = data.Get<int>(0x04);
            if (sizeOfSimp == SimpSizePs2) {
                return GameVersion.Ps2;
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
                return GameVersion.Android;
            }
            else if (sizeOfRunningScript == MissionScriptSizeIos) {
                return GameVersion.Ios;
            }

            // PSP -- ???? (TBD)
            throw new InvalidDataException("Unable to detect savedata version.");
        }

        private static T GetAttribute<T>(object o) where T : Attribute
        {
            return (T) Attribute.GetCustomAttribute(o.GetType().GetMember(o.ToString())[0], typeof(T));
        }

        private static bool IsGameVersionSupported(GameVersion v)
        {
            return GetAttribute<GameVersionAttribute>(v).IsSupported;
        }

        private static string GetGameVersionName(GameVersion v)
        {
            return GetAttribute<GameVersionAttribute>(v).Name;
        }
    }
}
