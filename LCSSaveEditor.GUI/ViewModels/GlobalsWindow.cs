using GTASaveData.LCS;
using LCSSaveEditor.Core;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using WpfEssentials;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class GlobalsWindow : WindowBase
    {
        private ObservableCollection<GlobalVariableInfo> m_globals;
        private bool m_showAll;
        private bool m_handlersRegistered;

        public ObservableCollection<GlobalVariableInfo> Globals
        {
            get { return m_globals; }
            set { m_globals = value; OnPropertyChanged(); }
        }

        public bool ShowAll
        {
            get { return m_showAll; }
            set { m_showAll = value; OnPropertyChanged(); }
        }

        public Editor TheEditor => Editor.TheEditor;
        public LCSSave TheSave => Editor.TheEditor.ActiveFile;

        public GlobalsWindow()
            : base()
        {
            Globals = new ObservableCollection<GlobalVariableInfo>();
        }

        public override void Initialize()
        {
            base.Initialize();

            TheEditor.FileOpened += TheEditor_FileOpened;
            TheEditor.FileClosing += TheEditor_FileClosing;
        }

        public override void Shutdown()
        {
            base.Shutdown();

            TheEditor.FileOpened -= TheEditor_FileOpened;
            TheEditor.FileClosing -= TheEditor_FileClosing;
        }

        public void UpdateList()
        {
            Globals.Clear();

            if (ShowAll)
            {
                for (int i = 0; i < TheEditor.GetNumGlobals(); i++)
                {
                    string name = Enum.GetName(typeof(GlobalVariable), i);

                    Globals.Add(new GlobalVariableInfo()
                    {
                        Index = i,
                        IntValue = TheEditor.GetGlobal(i),
                        FloatValue = TheEditor.GetGlobalAsFloat(i),
                        Name = name
                    });
                }
            }
            else
            {
                foreach (GlobalVariable var in Enum.GetValues(typeof(GlobalVariable)))
                {
                    Globals.Add(new GlobalVariableInfo()
                    {
                        Index = TheEditor.GetIndexOfGlobal(var),
                        IntValue = TheEditor.GetGlobal(var),
                        FloatValue = TheEditor.GetGlobalAsFloat(var),
                        Name = var.ToString()
                    });
                }
            }
        }
        
        public void RegisterChangeHandlers()
        {
            if (!m_handlersRegistered)
            {
                TheSave.Scripts.GlobalVariables.CollectionChanged += GlobalVariables_CollectionChanged;
                m_handlersRegistered = true;
            }
        }

        public void UnregisterChangeHandlers()
        {
            if (m_handlersRegistered)
            {
                TheSave.Scripts.GlobalVariables.CollectionChanged -= GlobalVariables_CollectionChanged;
                m_handlersRegistered = false;
            }
        }

        private void GlobalVariables_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Replace:
                {
                    GlobalVariableInfo item = (ShowAll)
                        ? Globals[e.NewStartingIndex]
                        : Globals.Where(x => x.Index == e.NewStartingIndex).FirstOrDefault();

                    item.IntValue = TheEditor.GetGlobal(item.Index);
                    item.FloatValue = TheEditor.GetGlobalAsFloat(item.Index);
                    break;
                }
                default:
                    break;
            }
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
