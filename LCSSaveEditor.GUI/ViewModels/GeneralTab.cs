namespace LCSSaveEditor.GUI.ViewModels
{
    public class GeneralTab : TabPageBase
    {
        public GeneralTab(MainWindow mainViewModel)
            : base("General", TabPageVisibility.WhenFileIsOpen, mainViewModel)
        {

        }
    }
}
