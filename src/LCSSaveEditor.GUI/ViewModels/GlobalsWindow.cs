using LCSSaveEditor.Core;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using WpfEssentials;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class GlobalsWindow : ChildWindowBase
    {
        private ObservableCollection<GlobalVariableInfo> m_globals;
        private GlobalVariableInfo m_selectedItem;
        private bool m_showSavedOnly;
        private bool m_handlersRegistered;

        public ObservableCollection<GlobalVariableInfo> Globals
        {
            get { return m_globals; }
            set { m_globals = value; OnPropertyChanged(); }
        }

        public GlobalVariableInfo SelectedItem
        {
            get { return m_selectedItem; }
            set { m_selectedItem = value; OnPropertyChanged(); }
        }

        public bool ShowSavedOnly
        {
            get { return m_showSavedOnly; }
            set { m_showSavedOnly = value; OnPropertyChanged(); }
        }

        public GlobalsWindow()
            : base()
        {
            Globals = new ObservableCollection<GlobalVariableInfo>();
            ShowSavedOnly = true;
        }

        public override void Initialize()
        {
            base.Initialize();

            TheEditor.FileOpened += TheEditor_FileOpened;
            TheEditor.FileClosing += TheEditor_FileClosing;

            RegisterChangeHandlers();
            UpdateList();
        }

        public override void Shutdown()
        {
            base.Shutdown();

            TheEditor.FileOpened -= TheEditor_FileOpened;
            TheEditor.FileClosing -= TheEditor_FileClosing;

            UnregisterChangeHandlers();
        }

        public void UpdateIntValue()
        {
            if (SelectedItem != null)
            {
                int oldVal = TheEditor.GetGlobal(SelectedItem.Index);
                int newVal = SelectedItem.IntValue;
                if (oldVal != newVal)
                {
                    TheEditor.SetGlobal(SelectedItem.Index, newVal);
                }
            }
        }

        public void UpdateFloatValue()
        {
            if (SelectedItem != null)
            {
                float oldVal = TheEditor.GetGlobalAsFloat(SelectedItem.Index);
                float newVal = SelectedItem.FloatValue;
                if (oldVal != newVal)
                {
                    TheEditor.SetGlobal(SelectedItem.Index, newVal);
                }
            }
        }

        public void UpdateList()
        {
            Globals.Clear();

            if (TheEditor.IsFileOpen)
            {
                if (ShowSavedOnly)
                {
                    PopulateSavedVariables();
                }
                else
                {
                    PopulateAllVariables();
                }
            }
        }

        private void PopulateSavedVariables()
        {
            foreach (GlobalVariable var in Enum.GetValues(typeof(GlobalVariable)))
            {
                int index = TheEditor.GetIndexOfGlobal(var);
                if (index == -1) continue;

                Globals.Add(new GlobalVariableInfo()
                {
                    Index = index,
                    IntValue = TheEditor.GetGlobal(var),
                    FloatValue = TheEditor.GetGlobalAsFloat(var),
                    Name = var.ToString()
                });
            }
        }

        private void PopulateAllVariables()
        {
            for (int i = 0, enumIndex = 0; i < TheEditor.GetNumGlobals(); i++, enumIndex++)
            {
                while (TheEditor.GetIndexOfGlobal((GlobalVariable) enumIndex) < i)
                {
                    enumIndex++;
                }

                string name = Enum.GetName(typeof(GlobalVariable), enumIndex);
                Globals.Add(new GlobalVariableInfo()
                {
                    Index = i,
                    IntValue = TheEditor.GetGlobal(i),
                    FloatValue = TheEditor.GetGlobalAsFloat(i),
                    Name = name
                });
            }
        }

        public void RegisterChangeHandlers()
        {
            if (!m_handlersRegistered && TheSave != null)
            {
                Scripts.GlobalVariables.CollectionChanged += GlobalVariables_CollectionChanged;
                m_handlersRegistered = true;
            }
        }

        public void UnregisterChangeHandlers()
        {
            if (m_handlersRegistered && TheSave != null)
            {
                Scripts.GlobalVariables.CollectionChanged -= GlobalVariables_CollectionChanged;
                m_handlersRegistered = false;
            }
        }

        private void GlobalVariables_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            GlobalVariableInfo item = (ShowSavedOnly)
                        ? Globals.Where(x => x.Index == e.NewStartingIndex).FirstOrDefault()
                        : Globals[e.NewStartingIndex];

            item.IntValue = TheEditor.GetGlobal(item.Index);
            item.FloatValue = TheEditor.GetGlobalAsFloat(item.Index);
        }

        private void TheEditor_FileOpened(object sender, EventArgs e)
        {
            RegisterChangeHandlers();
            UpdateList();
        }

        private void TheEditor_FileClosing(object sender, EventArgs e)
        {
            UnregisterChangeHandlers();
            Globals.Clear();
        }

        public class GlobalVariableInfo : ObservableObject
        {
            private int m_index;
            private string m_name;
            private int m_intValue;
            private float m_floatValue;

            public int Index
            {
                get { return m_index; }
                set { m_index = value; OnPropertyChanged(); }
            }

            public string Name
            {
                get { return m_name; }
                set { m_name = value; OnPropertyChanged(); }
            }

            public int IntValue
            {
                get { return m_intValue; }
                set { m_intValue = value; OnPropertyChanged(); }
            }

            public float FloatValue
            {
                get { return m_floatValue; }
                set { m_floatValue = value; OnPropertyChanged(); }
            }
        }
    }
}
