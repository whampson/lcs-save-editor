using GTASaveData.LCS;
using LCSSaveEditor.Core;
using System;
using System.Windows.Input;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class GlobalsWindow : WindowBase
    {
        public LCSSave TheSave => Editor.TheEditor.ActiveFile;

        public GlobalsWindow()
            : base()
        { }

        public void Update()
        {
            OnPropertyChanged(nameof(TheSave));
        }
    }
}
