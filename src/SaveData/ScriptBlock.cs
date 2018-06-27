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

using WHampson.Cascara;

namespace WHampson.LcsSaveEditor.SaveData
{
    internal class ScriptVariable
    {
        private readonly Primitive<int> valueAsInt;
        private readonly Primitive<float> valueAsFloat;
        private readonly Primitive<Bool32> valueAsBoolean;

        public ScriptVariable()
        {
            valueAsInt = new Primitive<int>(null, 0);
            valueAsFloat = new Primitive<float>(null, 0);
            valueAsBoolean = new Primitive<Bool32>(null, 0);
        }

        public int ValueAsInt
        {
            get { return valueAsInt.Value; }
            set { valueAsInt.Value = value; }
        }

        public float ValueAsFloat
        {
            get { return valueAsFloat.Value; }
            set { valueAsFloat.Value = value; }
        }

        public Bool32 ValueAsBoolean
        {
            get { return valueAsBoolean.Value; }
            set { valueAsBoolean.Value = value; }
        }
    }

    internal class ScriptBlock : DataBlock
    {
        private readonly BlockHeader nestedHeader;
        private readonly Primitive<uint> globalVariablesSize;
        private readonly ScriptVariable[] globalVariables;
        private readonly Primitive<uint> onAMissionFlag;
        private readonly Primitive<uint> timeLastMissionPassed;
        //private readonly BuildingSwap[] buildingSwapArray;
        //private readonly InvisibilitySetting[] invisibilitySettingArray;
        private readonly Primitive<bool> usingAMultiScriptFile;
        private readonly Primitive<uint> mainScriptSize;
        private readonly Primitive<uint> largestMissionScriptSize;
        private readonly Primitive<ushort> numberOfMissionScripts;
        private readonly Primitive<ushort> numberOfExclusiveMissionScripts;
        //private readonly MissionScript[] runningScripts;

        public BlockHeader NestedHeader
        {
            get { return nestedHeader; }
        }

        public uint GlobalVariablesSize
        {
            get { return globalVariablesSize.Value; }
            set { globalVariablesSize.Value = value; }
        }

        public ScriptVariable[] GlobalVariables
        {
            get { return globalVariables; }
        }

        public uint OnAMissionFlag
        {
            get { return onAMissionFlag.Value; }
            set { onAMissionFlag.Value = value; }
        }

        public uint TimeLastMissionPassed
        {
            get { return timeLastMissionPassed.Value; }
            set { timeLastMissionPassed.Value = value; }
        }

        //public BuildingSwap[] BuildingSwaps
        //{
        //    get { return buildingSwapArray; }
        //}

        //public InvisibilitySetting[] InvisibilitySettings
        //{
        //    get { return invisibilitySettingArray; }
        //}

        public bool UsingAMultiScriptFile
        {
            get { return usingAMultiScriptFile.Value; }
            set { usingAMultiScriptFile.Value = value; }
        }

        public uint MainScriptSize
        {
            get { return mainScriptSize.Value; }
            set { mainScriptSize.Value = value; }
        }

        public uint LargestMissionScriptSize
        {
            get { return largestMissionScriptSize.Value; }
            set { largestMissionScriptSize.Value = value; }
        }

        public ushort TotalMissionScriptsCount
        {
            get { return numberOfMissionScripts.Value; }
            set { numberOfMissionScripts.Value = value; }
        }

        public ushort RunningScriptsCount
        {
            get { return numberOfExclusiveMissionScripts.Value; }
            set { numberOfExclusiveMissionScripts.Value = value; }
        }

        //public MissionScript[] RunningScripts
        //{
        //    get { return runningScripts; }
        //}
    }
}
