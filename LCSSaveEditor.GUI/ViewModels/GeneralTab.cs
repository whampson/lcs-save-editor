using System;
using System.Windows.Input;
using GTASaveData.LCS;
using LCSSaveEditor.Core;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class GeneralTab : TabPageBase
    {
        private SimpleVariables m_simpleVars;
        private string m_saveTitleOnDisplay;

        public SimpleVariables SimpleVars
        {
            get { return m_simpleVars; }
            set { m_simpleVars = value; OnPropertyChanged(); }
        }

        public string SaveTitle
        {
            get { return m_saveTitleOnDisplay; }
            set { m_saveTitleOnDisplay = value; OnPropertyChanged(); }
        }

        public DateTime ClockValue
        {
            get { return new DateTime(1, 1, 1, SimpleVars.GameClockHours, SimpleVars.GameClockMinutes, 0); }
            set
            {
                SimpleVars.GameClockHours = (byte) value.Hour;
                SimpleVars.GameClockMinutes = (byte) value.Minute;
                OnPropertyChanged();
            }
        }

        public TimeSpan TotalTimePlayed
        {
            get { return TimeSpan.FromMilliseconds(SimpleVars.TimeInMilliseconds); }
            set
            {
                SimpleVars.TimeInMilliseconds = (uint) value.TotalMilliseconds;
                OnPropertyChanged();
            }
        }

        public GeneralTab(MainWindow mainViewModel)
            : base("General", TabPageVisibility.WhenFileIsOpen, mainViewModel)
        { }

        public override void Load()
        {
            base.Load();

            SimpleVars = MainViewModel.TheSave.SimpleVars;
            Log.Info(SimpleVars.ToJsonString());

            UpdateSaveTitleOnDisplay();
            OnPropertyChanged(nameof(ClockValue));
            OnPropertyChanged(nameof(TotalTimePlayed));
        }

        private void UpdateSaveTitleOnDisplay()
        {
            if (!MainViewModel.TheText.TryGetValue("MAIN", SimpleVars.LastMissionPassedName, out string title))
            {
                title = $"(invalid GXT key: {SimpleVars.LastMissionPassedName})";
            }

            SaveTitle = title;
        }

        public ICommand SelectSaveTitleCommand => new RelayCommand
        (
            () => MainViewModel.ShowInfo("TODO: GXT selection dialog")
        );
    }
}
