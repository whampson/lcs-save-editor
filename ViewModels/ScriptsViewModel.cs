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
using LcsSaveEditor.Helpers;
using LcsSaveEditor.Infrastructure;
using LcsSaveEditor.Models;
using LcsSaveEditor.Resources;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Security;

namespace LcsSaveEditor.ViewModels
{
    public partial class ScriptsViewModel : PageViewModelBase
    {
        private int m_globalVariablesSelectedIndex;
        private int m_localVariablesSelectedIndex;
        private int m_returnStackSelectedIndex;
        private int m_activeThreadsSelectedIndex;

        private RunningScript m_activeThreadsSelectedItem;

        private FullyObservableCollection<ScriptVariable> m_globalVariables;
        private FullyObservableCollection<NamedScriptVariable> m_namedGlobalVariables;
        private FullyObservableCollection<RunningScript> m_activeThreads;

        private ScriptVariableDataType m_globalVariablesType;
        private ScriptVariableDataType m_localVariablesType;

        public ScriptsViewModel(MainViewModel mainViewModel)
            : base("Scripts", PageVisibility.WhenFileLoaded, mainViewModel)
        {
            m_namedGlobalVariables = new FullyObservableCollection<NamedScriptVariable>();
            m_activeThreads = new FullyObservableCollection<RunningScript>();

            m_globalVariablesSelectedIndex = -1;
            m_localVariablesSelectedIndex = -1;
            m_returnStackSelectedIndex = -1;
            m_activeThreadsSelectedIndex = -1;

            MainViewModel.DataLoaded += MainViewModel_DataLoaded;
            MainViewModel.DataClosing += MainViewModel_DataClosing;
        }

        public FullyObservableCollection<NamedScriptVariable> NamedGlobalVariables
        {
            get { return m_namedGlobalVariables; }
            set { m_namedGlobalVariables = value; OnPropertyChanged(); }
        }

        public int GlobalVariablesSelectedIndex
        {
            get { return m_globalVariablesSelectedIndex; }
            set { m_globalVariablesSelectedIndex = value; OnPropertyChanged(); }
        }

        public ScriptVariableDataType GlobalVariablesType
        {
            get { return m_globalVariablesType; }
            set { m_globalVariablesType = value; OnPropertyChanged(); }
        }

        public FullyObservableCollection<RunningScript> ActiveThreads
        {
            get { return m_activeThreads; }
            set { m_activeThreads = value; OnPropertyChanged(); }
        }

        public int ActiveThreadsSelectedIndex
        {
            get { return m_activeThreadsSelectedIndex; }
            set { m_activeThreadsSelectedIndex = value; OnPropertyChanged(); }
        }

        public RunningScript ActiveThreadsSelectedItem
        {
            get { return m_activeThreadsSelectedItem; }
            set { m_activeThreadsSelectedItem = value; OnPropertyChanged(); }
        }

        public int LocalVariablesSelectedIndex
        {
            get { return m_localVariablesSelectedIndex; }
            set { m_localVariablesSelectedIndex = value; OnPropertyChanged(); }
        }

        public ScriptVariableDataType LocalVariablesType
        {
            get { return m_localVariablesType; }
            set { m_localVariablesType = value; OnPropertyChanged(); }
        }

        public int ReturnStackSelectedIndex
        {
            get { return m_returnStackSelectedIndex; }
            set { m_returnStackSelectedIndex = value; OnPropertyChanged(); }
        }
    }
}
