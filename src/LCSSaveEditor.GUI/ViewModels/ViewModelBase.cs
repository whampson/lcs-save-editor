using GTASaveData.LCS;
using LCSSaveEditor.Core;
using WpfEssentials;

namespace LCSSaveEditor.GUI.ViewModels
{
    public abstract class ViewModelBase : ObservableObject
    {
        public Editor TheEditor => Editor.TheEditor;
        public LCSSave TheSave => TheEditor.ActiveFile;
        public Settings TheSettings => Settings.TheSettings;
        public Gxt TheText => Gxt.TheText;
        public Carcols TheCarcols => Carcols.TheCarcols;

        public SimpleVariables SimpleVars => TheSave.SimpleVars;
        public ScriptData Scripts => TheSave.Scripts;
        public GarageData Garages => TheSave.Garages;
        public PlayerInfo PlayerInfo => TheSave.PlayerInfo;
        public Stats Stats => TheSave.Stats;
    }
}
