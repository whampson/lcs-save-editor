using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using GTASaveData.LCS;
using LCSSaveEditor.Core;
using LCSSaveEditor.GUI.Types;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class GeneralTab : TabPageBase
    {
        private SimpleVariables m_simpleVars;
        private Stats m_stats;
        private string m_saveTitle;
        private float m_progress;

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

        public string SaveTitleOnDisplay
        {
            get { return m_saveTitle; }
            set { m_saveTitle = value; OnPropertyChanged(); }
        }

        public float ProgressOnDisplay
        {
            get { return m_progress; }
            set { m_progress = value; OnPropertyChanged(); }
        }

        public ZoneLevel Location
        {
            get { return (ZoneLevel) SimpleVars.CurrentLevel; }
            set { SimpleVars.CurrentLevel = (int) value; }
        }

        public DateTime GameClock
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

        public PadMode PadMode
        {
            get
            {
                if (TheSave.FileFormat.IsPSP)
                {
                    if (SimpleVars.PadMode == 0 || SimpleVars.PadMode == 1) return PadMode.Setup2;
                    if (SimpleVars.PadMode == 2 || SimpleVars.PadMode == 3) return PadMode.Setup1;
                }
                return (PadMode) SimpleVars.PadMode;
            }
            set
            {
                short mode = (short) value;
                if (TheSave.FileFormat.IsPSP)
                {
                    if (value == PadMode.Setup1) mode = 2;
                    if (value == PadMode.Setup2) mode = 0;
                }
                SimpleVars.PadMode = mode;
                OnPropertyChanged();
            }
        }

        public OnFootCameraMode OnFootCameraMode
        {
            get { return (OnFootCameraMode) TheEditor.GetGlobal(GlobalVariable.CameraModeOnFoot); }
            set
            {
                SimpleVars.CameraModeOnFoot = (float) value;
                TheEditor.SetGlobal(GlobalVariable.CameraModeOnFoot, (int) value);
                // OnPropertyChanged called in GlobalVariables_CollectionChanged
            }
        }

        public InCarCameraMode InCarCameraMode
        {
            get { return (InCarCameraMode) SimpleVars.CameraModeInCar; }
            set { SimpleVars.CameraModeInCar = (float) value; OnPropertyChanged(); }
        }

        public Language Language
        {
            get { return (Language) SimpleVars.Language; }
            set { SimpleVars.Language = (int) value; OnPropertyChanged(); }
        }

        public GeneralTab(MainWindow window)
            : base("General", TabPageVisibility.WhenFileIsOpen, window)
        { }

        public override void Load()
        {
            base.Load();

            SimpleVars = TheWindow.TheSave.SimpleVars;
            Stats = TheWindow.TheSave.Stats;

            SimpleVars.PropertyChanged += Data_PropertyChanged;
            Stats.PropertyChanged += Data_PropertyChanged;
            TheSave.Scripts.GlobalVariables.CollectionChanged += GlobalVariables_CollectionChanged;
        }

        public override void Unload()
        {
            base.Unload();

            SimpleVars.PropertyChanged -= Data_PropertyChanged;
            Stats.PropertyChanged -= Data_PropertyChanged;
            TheSave.Scripts.GlobalVariables.CollectionChanged -= GlobalVariables_CollectionChanged;
        }

        public override void Update()
        {
            base.Update();

            OnTitleChanged();
            OnProgressChanged();
            OnPropertyChanged(nameof(Location));
            OnPropertyChanged(nameof(GameClock));
            OnPropertyChanged(nameof(TotalTimePlayed));
            OnPropertyChanged(nameof(PadMode));
            OnPropertyChanged(nameof(OnFootCameraMode));
            OnPropertyChanged(nameof(InCarCameraMode));
            OnPropertyChanged(nameof(Language));
        }

        private void OnTitleChanged()
        {
            if (!TheWindow.TheText.TryGetValue("MAIN", Stats.LastMissionPassedName, out string title))
            {
                title = $"(invalid GXT key: {Stats.LastMissionPassedName})";
            }

            SaveTitleOnDisplay = title;
        }

        private void OnProgressChanged()
        {
            ProgressOnDisplay = Stats.ProgressMade / Stats.TotalProgressInGame;
        }

        private void Data_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SimpleVars.LastMissionPassedName):
                    OnTitleChanged();
                    break;
                case nameof(Stats.ProgressMade):
                case nameof(Stats.TotalProgressInGame):
                    OnProgressChanged();
                    break;
            }
        }

        private void GlobalVariables_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null)
            {
                return;
            }

            for (int i = 0; i < e.NewItems.Count; i++)
            {
                int index = e.NewStartingIndex + i;
                GlobalVariable g = TheEditor.GetGlobalId(index);
                switch (g)
                {
                    case GlobalVariable.CameraModeOnFoot:
                        OnPropertyChanged(nameof(OnFootCameraMode));
                        break;
                }
            }
        }

        public ICommand SelectSaveTitleCommand => new RelayCommand
        (
            () => TheWindow.ShowGxtDialog((r,e) =>
            {
                if (r == true)
                {
                    SimpleVars.LastMissionPassedName = e.SelectedKey;
                    Stats.LastMissionPassedName = e.SelectedKey;
                }
            })
        );

        public ICommand ShowTargetOnMapCommand => new RelayCommand
        (
            () => TheWindow.ShowInfo("TODO: plot target on map")
        );

        public static WeatherType[] WeatherList = new WeatherType[]
        {
            WeatherType.Sunny, WeatherType.ExtraSunny, WeatherType.Sunny, WeatherType.Cloudy,
            WeatherType.Cloudy, WeatherType.Sunny, WeatherType.Sunny, WeatherType.Sunny,
            WeatherType.ExtraSunny, WeatherType.ExtraSunny, WeatherType.Sunny, WeatherType.Rainy,
            WeatherType.Sunny, WeatherType.Sunny, WeatherType.Cloudy, WeatherType.Cloudy,
            WeatherType.Foggy, WeatherType.Cloudy, WeatherType.Sunny, WeatherType.Sunny,
            WeatherType.Sunny, WeatherType.Cloudy, WeatherType.Rainy, WeatherType.Foggy,
            WeatherType.Cloudy, WeatherType.Cloudy, WeatherType.Sunny, WeatherType.ExtraSunny,
            WeatherType.Sunny, WeatherType.Sunny, WeatherType.Cloudy, WeatherType.Foggy,
            WeatherType.Rainy, WeatherType.Foggy, WeatherType.Cloudy, WeatherType.Sunny,
            WeatherType.Sunny, WeatherType.Cloudy, WeatherType.Hurricane, WeatherType.Cloudy,
            WeatherType.Sunny, WeatherType.ExtraSunny, WeatherType.ExtraSunny, WeatherType.Sunny,
            WeatherType.Sunny, WeatherType.Sunny, WeatherType.Sunny, WeatherType.ExtraSunny,
            WeatherType.Sunny, WeatherType.Sunny, WeatherType.Cloudy, WeatherType.Foggy,
            WeatherType.Cloudy, WeatherType.Sunny, WeatherType.Cloudy, WeatherType.Cloudy,
            WeatherType.Sunny, WeatherType.ExtraSunny, WeatherType.Cloudy, WeatherType.Hurricane,
            WeatherType.Cloudy, WeatherType.Sunny, WeatherType.Sunny, WeatherType.ExtraSunny,
        };
    }
}
