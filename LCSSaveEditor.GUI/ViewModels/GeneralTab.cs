using System;
using System.ComponentModel;
using System.Windows.Input;
using GTASaveData.LCS;
using LCSSaveEditor.Core;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class GeneralTab : TabPageBase
    {
        private SimpleVariables m_simpleVars;
        private Stats m_stats;
        private string m_saveTitle;
        private string m_lastMissionPassedName;
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

        public string LastMissionOnDisplay
        {
            get { return m_lastMissionPassedName; }
            set { m_lastMissionPassedName = value; OnPropertyChanged(); }
        }

        public float ProgressOnDisplay
        {
            get { return m_progress; }
            set { m_progress = value; OnPropertyChanged(); }
        }

        public MapLevel Location
        {
            get { return (MapLevel) SimpleVars.CurrentLevel; }
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
            // TODO: android ios
            get { return (OnFootCameraMode) SimpleVars.CameraModeOnFoot; }
            set
            {
                SimpleVars.CameraModeOnFoot = (float) value;
                if (TheSave.FileFormat.IsPS2 || TheSave.FileFormat.IsPSP)
                {
                    TheEditor.SetGlobal(GlobalVariable.CameraModeOnFoot, (int) value);
                }
                OnPropertyChanged(); }
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
        }

        public override void Unload()
        {
            base.Unload();

            SimpleVars.PropertyChanged -= Data_PropertyChanged;
            Stats.PropertyChanged -= Data_PropertyChanged;
        }

        public override void Update()
        {
            base.Update();

            OnTitleChanged();
            OnLastMissionChanged();
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
            string title = "";
            if (TheSave.FileFormat.IsMobile)
            {
                if (!TheWindow.TheText.TryGetValue("MAIN", SimpleVars.LastMissionPassedName, out title))
                {
                    title = $"(invalid GXT key: {SimpleVars.LastMissionPassedName})";
                }
            }

            SaveTitleOnDisplay = title;
        }

        private void OnLastMissionChanged()
        {
            if (!TheWindow.TheText.TryGetValue("MAIN", Stats.LastMissionPassedName, out string title))
            {
                title = $"(invalid GXT key: {Stats.LastMissionPassedName})";
            }

            LastMissionOnDisplay = title;
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
                    OnLastMissionChanged();
                    break;
                case nameof(Stats.ProgressMade):
                case nameof(Stats.TotalProgressInGame):
                    OnTitleChanged();
                    break;
            }
        }

        public ICommand SelectSaveTitleCommand => new RelayCommand
        (
            () => TheWindow.ShowGxtDialog((r,e) =>
            {
                if (r == true)
                {
                    TheSave.Name = e.SelectedKey;
                    OnTitleChanged();
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

    public enum OnFootCameraMode
    {
        Near = 1,
        Middle,
        Far,
    }

    public enum InCarCameraMode
    {
        Bumper,
        Near,
        Middle,
        Far,
        Tripod,
        Cinematic,
    }

    public enum PadMode
    {
        [Description("Setup 1")]
        Setup1,

        [Description("Setup 2")]
        Setup2,
    }

    public enum Language
    {
        English,
        French,
        German,
        Italian,
        Spanish,
        Russian,
        Japanese,
    }

    public enum MapLevel
    {
        [Description("Liberty City")]
        None,

        [Description("Portland")]
        Industrial,

        [Description("Staunton Island")]
        Commercial,

        [Description("Shoreside Vale")]
        Suburban
    }
}
