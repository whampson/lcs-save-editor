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

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using LcsSaveEditor.Infrastructure;

namespace LcsSaveEditor.Models
{
    /// <summary>
    /// Represents a script thread present in the game's thread pool.
    /// </summary>
    public abstract class RunningScript : SerializableObject
    {
        public const int ReturnStackCount = 16;
        public const int LocalVariablesCount = 104;

        protected uint m_nextScript;
        protected uint m_prevScript;
        protected uint m_threadId;
        protected string m_name;
        protected uint m_instructionPointer;
        protected ObservableCollection<uint> m_returnStack;
        protected ushort m_returnStackTop;
        protected ObservableCollection<uint> m_localVariables;
        protected uint m_timer1;
        protected uint m_timer2;
        protected bool m_ifResult;
        protected bool m_usesMissionCleanup;
        protected bool m_isActive;
        protected uint m_wakeTime;
        protected ushort m_logicalOperation;        // TODO: enum... gotta figure out the values first
        protected bool m_notFlag;
        protected bool m_isWastedBustedCheckEnabled;
        protected bool m_isWastedBusted;
        protected bool m_isMission;

        public RunningScript()
        {
            m_returnStack = new ObservableCollection<uint>();
            m_localVariables = new ObservableCollection<uint>();

            m_returnStack.CollectionChanged += ReturnStack_CollectionChanged;
            m_localVariables.CollectionChanged += LocalVariables_CollectionChanged;
        }

        public uint NextScriptPointer
        {
            get { return m_nextScript; }
            set { m_nextScript = value; OnPropertyChanged(); }
        }

        public uint PreviousScriptPointer
        {
            get { return m_prevScript; }
            set { m_prevScript = value; OnPropertyChanged(); }
        }

        public uint ThreadId
        {
            get { return m_threadId; }
            set { m_threadId = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get { return m_name; }
            set { m_name = value; OnPropertyChanged(); }
        }

        public uint InstructionPointer
        {
            get { return m_instructionPointer; }
            set { m_instructionPointer = value; OnPropertyChanged(); }
        }

        public ObservableCollection<uint> ReturnStack
        {
            get { return m_returnStack; }
        }

        public ushort ReturnStackTop
        {
            get { return m_returnStackTop; }
            set { m_returnStackTop = value; OnPropertyChanged(); }
        }

        public ObservableCollection<uint> LocalVariables
        {
            get { return m_localVariables; }
        }

        public uint Timer1
        {
            get { return m_timer1; }
            set { m_timer1 = value; OnPropertyChanged(); }
        }

        public uint Timer2
        {
            get { return m_timer2; }
            set { m_timer2 = value; OnPropertyChanged(); }
        }

        public bool IfResult
        {
            get { return m_ifResult; }
            set { m_ifResult = value; OnPropertyChanged(); }
        }

        public bool UsesMissionCleanup
        {
            get { return m_usesMissionCleanup; }
            set { m_usesMissionCleanup = value; OnPropertyChanged(); }
        }

        public bool IsActive
        {
            get { return m_isActive; }
            set { m_isActive = value; OnPropertyChanged(); }
        }

        public uint WakeTime
        {
            get { return m_wakeTime; }
            set { m_wakeTime = value; OnPropertyChanged(); }
        }

        public ushort LogicalOperation
        {
            get { return m_logicalOperation; }
            set { m_logicalOperation = value; OnPropertyChanged(); }
        }

        public bool NotFlag
        {
            get { return m_notFlag; }
            set { m_notFlag = value; OnPropertyChanged(); }
        }

        public bool IsWastedBustedCheckEnabled
        {
            get { return m_isWastedBustedCheckEnabled; }
            set { m_isWastedBustedCheckEnabled = value; OnPropertyChanged(); }
        }

        public bool IsWastedBusted
        {
            get { return m_isWastedBusted; }
            set { m_isWastedBusted = value; OnPropertyChanged(); }
        }

        public bool IsMission
        {
            get { return m_isMission; }
            set { m_isMission = value; OnPropertyChanged(); }
        }

        public override string ToString()
        {
            return string.Format("{0} = {1}, {2} = {3}",
                nameof(Name), Name,
                nameof(ThreadId), ThreadId);
        }

        private void LocalVariables_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(LocalVariables));
        }

        private void ReturnStack_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ReturnStack));
        }
    }
}
