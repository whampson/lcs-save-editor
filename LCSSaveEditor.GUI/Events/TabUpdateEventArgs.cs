using System;

namespace LCSSaveEditor.GUI.Events
{
    /// <summary>
    /// Parameters for handling a tab update event.
    /// </summary>
    public class TabUpdateEventArgs : EventArgs
    {
        /// <summary>
        /// The event trigger type.
        /// </summary>
        public TabUpdateTrigger Trigger { get; }

        public TabUpdateEventArgs(TabUpdateTrigger trigger)
        {
            Trigger = trigger;
        }
    }

    /// <summary>
    /// Tab update trigger event types.
    /// </summary>
    public enum TabUpdateTrigger
    {
        /// <summary>
        /// Update due to the window loading.
        /// </summary>
        WindowLoaded,

        /// <summary>
        /// Update due to the window closing.
        /// </summary>
        WindowClosing,

        /// <summary>
        /// Update due to a file being opened.
        /// </summary>
        FileOpened,

        /// <summary>
        /// Update due to a file closing.
        /// </summary>
        FileClosing
    }
}
