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

using LcsSaveEditor.Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace LcsSaveEditor.Models
{
    /// <summary>
    /// Variables related to the game script.
    /// This information is stored in Block 1 of the save file.
    /// </summary>
    public abstract class Scripts : SerializableObject
    {
        protected uint m_globalVarsSize;
        protected ObservableCollection<ScriptVariable> m_globalVars;
        protected uint m_unknown;
        protected uint m_onMissionFlag;
        protected uint m_lastMissionPassedTime;
        protected Collective[] m_collectiveArray;
        protected uint m_nextFreeCollective;
        protected StaticReplacement[] m_buildingSwapArray;
        protected InvisibleObject[] m_invisibilitySettingArray;
        protected bool m_usingAMultiScriptFile;
        protected bool _m_playerHasMetDebbieHarry;
        protected uint m_mainScriptSize;
        protected uint m_largestMissionScriptSize;
        protected ushort m_numberOfMissionScripts;
        protected ushort m_numberOfExclusiveMissionScripts;
        protected ObservableCollection<RunningScript> m_runningScripts;

        public Scripts()
        {
            m_globalVars = new ObservableCollection<ScriptVariable>();
            m_collectiveArray = new Collective[32];
            m_buildingSwapArray = new StaticReplacement[80];
            m_invisibilitySettingArray = new InvisibleObject[52];
            m_runningScripts = new ObservableCollection<RunningScript>();
        }

        public uint GlobalVariablesSize
        {
            get { return m_globalVarsSize; }
            set { m_globalVarsSize = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ScriptVariable> GlobalVariables
        {
            get { return m_globalVars; }
            set { m_globalVars = value; OnPropertyChanged(); }
        }

        public uint OnMissionFlag
        {
            get { return m_onMissionFlag; }
            set { m_onMissionFlag = value; OnPropertyChanged(); }
        }

        public uint LastMissionPassedTime
        {
            get { return m_lastMissionPassedTime; }
            set { m_lastMissionPassedTime = value; OnPropertyChanged(); }
        }

        public Collective[] CollectiveArray
        {
            get { return m_collectiveArray; }
            set { m_collectiveArray = value; OnPropertyChanged(); }
        }

        public uint NextFreeCollective
        {
            get { return m_nextFreeCollective; }
            set { m_nextFreeCollective = value; OnPropertyChanged(); }
        }

        public StaticReplacement[] BuildingSwapArray
        {
            get { return m_buildingSwapArray; }
            set { m_buildingSwapArray = value; OnPropertyChanged(); }
        }

        public InvisibleObject[] InvisibilitySettingArray
        {
            get { return m_invisibilitySettingArray; }
            set { m_invisibilitySettingArray = value; OnPropertyChanged(); }
        }

        public bool UsingAMultiScriptFile
        {
            get { return m_usingAMultiScriptFile; }
            set { m_usingAMultiScriptFile = value; OnPropertyChanged(); }
        }

        public uint MainScriptSize
        {
            get { return m_mainScriptSize; }
            set { m_mainScriptSize = value; OnPropertyChanged(); }
        }

        public uint LargestMissionScriptSize
        {
            get { return m_largestMissionScriptSize; }
            set { m_largestMissionScriptSize = value; OnPropertyChanged(); }
        }

        public ushort NumberOfMissionScripts
        {
            get { return m_numberOfMissionScripts; }
            set { m_numberOfMissionScripts = value; OnPropertyChanged(); }
        }

        public ushort NumberOfExclusiveMissionScripts
        {
            get { return m_numberOfExclusiveMissionScripts; }
            set { m_numberOfExclusiveMissionScripts = value; OnPropertyChanged(); }
        }

        public ObservableCollection<RunningScript> RunningScripts
        {
            get { return m_runningScripts; }
            set { m_runningScripts = value; OnPropertyChanged(); }
        }

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_globalVarsSize = r.ReadUInt32();
                for (int i = 0; i < m_globalVarsSize / 4; i++) {
                    m_globalVars.Add(Deserialize<ScriptVariable>(stream));
                }
                m_unknown = r.ReadUInt32();
                m_onMissionFlag = r.ReadUInt32();
                m_lastMissionPassedTime = r.ReadUInt32();
                for (int i = 0; i < m_collectiveArray.Length; i++) {
                    m_collectiveArray[i] = Deserialize<Collective>(stream);
                }
                m_nextFreeCollective = r.ReadUInt32();
                for (int i = 0; i < m_buildingSwapArray.Length; i++) {
                    m_buildingSwapArray[i] = Deserialize<StaticReplacement>(stream);
                }
                for (int i = 0; i < m_invisibilitySettingArray.Length; i++) {
                    m_invisibilitySettingArray[i] = Deserialize<InvisibleObject>(stream);
                }
                m_usingAMultiScriptFile = r.ReadBoolean();
                _m_playerHasMetDebbieHarry = r.ReadBoolean();
                r.ReadBytes(2);     // align bytes
                m_mainScriptSize = r.ReadUInt32();
                m_largestMissionScriptSize = r.ReadUInt32();
                m_numberOfMissionScripts = r.ReadUInt16();
                r.ReadBytes(2);     // align bytes
                m_numberOfExclusiveMissionScripts = r.ReadUInt16();
                r.ReadBytes(2);     // align bytes
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write(m_globalVarsSize);
                for (int i = 0; i < m_globalVarsSize / 4; i++) {
                    Serialize(m_globalVars[i], stream);
                }
                w.Write(m_unknown);
                w.Write(m_onMissionFlag);
                w.Write(m_lastMissionPassedTime);
                for (int i = 0; i < m_collectiveArray.Length; i++) {
                    Serialize(m_collectiveArray[i], stream);
                }
                w.Write(m_nextFreeCollective);
                for (int i = 0; i < m_buildingSwapArray.Length; i++) {
                    Serialize(m_buildingSwapArray[i], stream);
                }
                for (int i = 0; i < m_invisibilitySettingArray.Length; i++) {
                    Serialize(m_invisibilitySettingArray[i], stream);
                }
                w.Write(m_usingAMultiScriptFile);
                w.Write(_m_playerHasMetDebbieHarry);
                w.Write(new byte[2]);       // align bytes
                w.Write(m_mainScriptSize);
                w.Write(m_largestMissionScriptSize);
                w.Write(m_numberOfMissionScripts);
                w.Write(new byte[2]);       // align bytes
                w.Write(m_numberOfExclusiveMissionScripts);
                w.Write(new byte[2]);       // align bytes
                for (int i = 0; i < m_numberOfExclusiveMissionScripts; i++) {
                    Serialize(m_runningScripts[i], stream);
                }
            }

            return stream.Position - start;
        }
    }
}
