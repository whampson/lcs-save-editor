using System;
using System.Collections.ObjectModel;
using System.Windows;
using GTASaveData.LCS;
using LCSSaveEditor.Core;
using LCSSaveEditor.GUI.Types;
using WpfEssentials;
using WpfEssentials.Extensions;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class StatsTab : TabPageBase
    {
        private SimpleVariables m_simpleVars;
        private Stats m_stats;

        public SimpleVariables SimpleVars
        {
            get { return m_simpleVars; }
            set { m_simpleVars = value; OnPropertyChanged(); }
        }

        public Stats Stats
        {
            get { return m_stats; }
            set { m_stats = value; OnPropertyChanged(); }
        }

        public StatsTab(MainWindow window)
            : base("Stats", TabPageVisibility.WhenFileIsOpen, window)
        {
            SimpleVars = new SimpleVariables();
            Stats = new Stats();
        }

        public override void Load()
        {
            base.Load();
            SimpleVars = TheSave.SimpleVars;
            Stats = TheSave.Stats;
        }

        public override void Update()
        {
            base.Update();
            RefreshStats();
        }

        public void RefreshStats()
        {

        }
    }
}
