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
using LcsSaveEditor.Resources;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        public const int CollectiveArrayCount = 32;
        public const int BuildingSwapArrayCount = 80;
        public const int InvisibilitySettingArrayCount = 52;

        protected bool m_isSerializing;

        protected uint m_globalVarsSize;
        protected FullyObservableCollection<ScriptVariable> m_globalVars;
        protected uint m_unknown;
        protected uint m_onMissionFlag;
        protected uint m_lastMissionPassedTime;
        protected FullyObservableCollection<Collective> m_collectiveArray;
        protected uint m_nextFreeCollective;
        protected FullyObservableCollection<StaticReplacement> m_buildingSwapArray;
        protected FullyObservableCollection<InvisibleObject> m_invisibilitySettingArray;
        protected bool m_usingAMultiScriptFile;
        protected bool _m_playerHasMetDebbieHarry;
        protected uint m_mainScriptSize;
        protected uint m_largestMissionScriptSize;
        protected ushort m_numberOfMissionScripts;
        protected ushort m_numberOfExclusiveMissionScripts;
        protected FullyObservableCollection<RunningScript> m_runningScripts;

        public Scripts()
        {
            m_globalVars = new FullyObservableCollection<ScriptVariable>();
            m_collectiveArray = new FullyObservableCollection<Collective>();
            m_buildingSwapArray = new FullyObservableCollection<StaticReplacement>();
            m_invisibilitySettingArray = new FullyObservableCollection<InvisibleObject>();
            m_runningScripts = new FullyObservableCollection<RunningScript>();

            m_globalVars.CollectionChanged += GlobalVars_CollectionChanged;
            m_globalVars.ItemPropertyChanged += GlobalVars_ItemPropertyChanged;
            m_collectiveArray.CollectionChanged += CollectiveArray_CollectionChanged;
            m_collectiveArray.ItemPropertyChanged += CollectiveArray_ItemPropertyChanged;
            m_buildingSwapArray.CollectionChanged += BuildingSwapArray_CollectionChanged;
            m_buildingSwapArray.ItemPropertyChanged += BuildingSwapArray_ItemPropertyChanged;
            m_invisibilitySettingArray.CollectionChanged += InvisibilitySettingArray_CollectionChanged;
            m_invisibilitySettingArray.ItemPropertyChanged += InvisibilitySettingArray_ItemPropertyChanged;
            m_runningScripts.CollectionChanged += RunningScripts_CollectionChanged;
            m_runningScripts.ItemPropertyChanged += RunningScripts_ItemPropertyChanged;
        }

        public uint GlobalVariablesSize
        {
            get { return m_globalVarsSize; }
            set { m_globalVarsSize = value; OnPropertyChanged(); }
        }

        public FullyObservableCollection<ScriptVariable> GlobalVariables
        {
            get { return m_globalVars; }
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

        public FullyObservableCollection<Collective> CollectiveArray
        {
            get { return m_collectiveArray; }
        }

        public uint NextFreeCollective
        {
            get { return m_nextFreeCollective; }
            set { m_nextFreeCollective = value; OnPropertyChanged(); }
        }

        public FullyObservableCollection<StaticReplacement> BuildingSwapArray
        {
            get { return m_buildingSwapArray; }
        }

        public FullyObservableCollection<InvisibleObject> InvisibilitySettingArray
        {
            get { return m_invisibilitySettingArray; }
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

        public FullyObservableCollection<RunningScript> RunningScripts
        {
            get { return m_runningScripts; }
        }

        private void GlobalVars_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (m_isSerializing) {
                return;
            }

            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Reset:
                    m_globalVarsSize = (uint) (m_globalVars.Count * 4);
                    break;
            }
            OnPropertyChanged(nameof(GlobalVariables));
        }

        private void GlobalVars_ItemPropertyChanged(object sender, ItemPropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(GlobalVariables));
        }

        private void CollectiveArray_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (m_isSerializing) {
                return;
            }

            switch (e.Action) {
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Replace:
                    OnPropertyChanged(nameof(CollectiveArray));
                    break;
                default:
                    string msg = string.Format(Strings.ExceptionStaticArray, nameof(CollectiveArray));
                    throw new NotSupportedException(msg);
            }
        }

        private void CollectiveArray_ItemPropertyChanged(object sender, ItemPropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CollectiveArray));
        }

        private void BuildingSwapArray_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (m_isSerializing) {
                return;
            }

            switch (e.Action) {
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Replace:
                    OnPropertyChanged(nameof(BuildingSwapArray));
                    break;
                default:
                    string msg = string.Format(Strings.ExceptionStaticArray, nameof(BuildingSwapArray));
                    throw new NotSupportedException(msg);
            }
        }

        private void BuildingSwapArray_ItemPropertyChanged(object sender, ItemPropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(BuildingSwapArray));
        }

        private void InvisibilitySettingArray_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (m_isSerializing) {
                return;
            }

            switch (e.Action) {
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Replace:
                    OnPropertyChanged(nameof(InvisibilitySettingArray));
                    break;
                default:
                    string msg = string.Format(Strings.ExceptionStaticArray, nameof(InvisibilitySettingArray));
                    throw new NotSupportedException(msg);
            }
        }

        private void InvisibilitySettingArray_ItemPropertyChanged(object sender, ItemPropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(InvisibilitySettingArray));
        }

        private void RunningScripts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (m_isSerializing) {
                return;
            }

            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Reset:
                    m_numberOfExclusiveMissionScripts = (ushort) m_runningScripts.Count;
                    break;
            }
            OnPropertyChanged(nameof(RunningScripts));
        }

        private void RunningScripts_ItemPropertyChanged(object sender, ItemPropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(RunningScripts));
        }
    }

    public class Scripts<TRunningScript> : Scripts
        where TRunningScript : RunningScript, new()
    {
        protected override long DeserializeObject(Stream stream)
        {
            m_isSerializing = true;

            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_globalVarsSize = r.ReadUInt32();
                for (int i = 0; i < m_globalVarsSize / 4; i++) {
                    m_globalVars.Add(r.ReadUInt32());
                }
                m_unknown = r.ReadUInt32();
                m_onMissionFlag = r.ReadUInt32();
                m_lastMissionPassedTime = r.ReadUInt32();
                for (int i = 0; i < CollectiveArrayCount; i++) {
                    m_collectiveArray.Add(Deserialize<Collective>(stream));
                }
                m_nextFreeCollective = r.ReadUInt32();
                for (int i = 0; i < BuildingSwapArrayCount; i++) {
                    m_buildingSwapArray.Add(Deserialize<StaticReplacement>(stream));
                }
                for (int i = 0; i < InvisibilitySettingArrayCount; i++) {
                    m_invisibilitySettingArray.Add(Deserialize<InvisibleObject>(stream));
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
                for (int i = 0; i < m_numberOfExclusiveMissionScripts; i++) {
                    m_runningScripts.Add(Deserialize<TRunningScript>(stream));
                }
            }

            m_isSerializing = false;
            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            m_isSerializing = true;

            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write(m_globalVarsSize);
                for (int i = 0; i < m_globalVarsSize / 4; i++) {
                    w.Write(m_globalVars[i]);
                }
                w.Write(m_unknown);
                w.Write(m_onMissionFlag);
                w.Write(m_lastMissionPassedTime);
                for (int i = 0; i < CollectiveArrayCount; i++) {
                    Serialize(m_collectiveArray[i], stream);
                }
                w.Write(m_nextFreeCollective);
                for (int i = 0; i < BuildingSwapArrayCount; i++) {
                    Serialize(m_buildingSwapArray[i], stream);
                }
                for (int i = 0; i < InvisibilitySettingArrayCount; i++) {
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

            m_isSerializing = false;
            return stream.Position - start;
        }
    }
}
