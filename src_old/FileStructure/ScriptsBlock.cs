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

namespace WHampson.LcsSaveEditor.FileStructure
{
    public class ScriptsBlock : DataBlock
    {
        private readonly BlockHeader nestedHeader;
        private readonly Primitive<uint> globalVariablesSize;
        private readonly ScriptVariable[] globalVariables;
        private readonly Primitive<uint> onAMissionFlag;
        private readonly Primitive<uint> timeLastMissionPassed;
        private readonly BuildingSwap[] buildingSwapArray;
        private readonly InvisibilitySetting[] invisibilitySettingArray;
        private readonly Primitive<bool> usingAMultiScriptFile;
        private readonly Primitive<uint> mainScriptSize;
        private readonly Primitive<uint> largestMissionScriptSize;
        private readonly Primitive<ushort> numberOfMissionScripts;
        private readonly Primitive<ushort> numberOfExclusiveMissionScripts;
        private readonly MissionScript[] runningScripts;

        public ScriptsBlock()
        {
            nestedHeader = new BlockHeader();
            globalVariablesSize = new Primitive<uint>(null, 0);
            globalVariables = new ScriptVariable[0];
            onAMissionFlag = new Primitive<uint>(null, 0);
            timeLastMissionPassed = new Primitive<uint>(null, 0);
            buildingSwapArray = new BuildingSwap[0];
            invisibilitySettingArray = new InvisibilitySetting[0];
            usingAMultiScriptFile = new Primitive<bool>(null, 0);
            mainScriptSize = new Primitive<uint>(null, 0);
            largestMissionScriptSize = new Primitive<uint>(null, 0);
            numberOfMissionScripts = new Primitive<ushort>(null, 0);
            numberOfExclusiveMissionScripts = new Primitive<ushort>(null, 0);
            runningScripts = new MissionScript[0];
        }

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

        public BuildingSwap[] BuildingSwaps
        {
           get { return buildingSwapArray; }
        }

        public InvisibilitySetting[] InvisibilitySettings
        {
           get { return invisibilitySettingArray; }
        }

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

        public MissionScript[] RunningScripts
        {
           get { return runningScripts; }
        }
    }
}
