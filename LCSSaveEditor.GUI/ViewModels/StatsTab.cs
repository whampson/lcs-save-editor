namespace LCSSaveEditor.GUI.ViewModels
{
    public class StatsTab : TabPageBase
    {
        public StatsTab(MainWindow window)
            : base("Stats", TabPageVisibility.WhenFileIsOpen, window)
        { }

        public float MixTapeListenTime
        {
            get { return (Stats.FavoriteRadioStationList.Count > 10) ? Stats.FavoriteRadioStationList[10] : 0; }
            set
            {
                if (Stats.FavoriteRadioStationList.Count > 10)
                {
                    Stats.FavoriteRadioStationList[10] = value;
                }
                OnPropertyChanged();
            }
        }
    }
}
