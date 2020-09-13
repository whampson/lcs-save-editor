namespace LCSSaveEditor.GUI.ViewModels
{
    public class StatsTab : TabPageBase
    {
        public StatsTab(MainWindow window)
            : base("Stats", TabPageVisibility.WhenFileIsOpen, window)
        { }
    }
}
