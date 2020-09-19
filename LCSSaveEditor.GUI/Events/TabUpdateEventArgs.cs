using System;

namespace LCSSaveEditor.GUI.Events
{
    public class TabUpdateEventArgs : EventArgs
    {
        public TabUpdateTrigger Trigger { get; }

        public TabUpdateEventArgs(TabUpdateTrigger trigger)
        {
            Trigger = trigger;
        }
    }

    public enum TabUpdateTrigger
    {
        WindowLoaded,
        WindowClosing,
        FileOpened,
        FileClosing
    }
}
