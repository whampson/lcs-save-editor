using System;
using System.Collections.Generic;
using System.Text;
using LCSSaveEditor.GUI.Utils;

namespace LCSSaveEditor.GUI.Events
{
    public class UpdateInfoEventArgs : EventArgs
    {
        public GitHubRelease UpdateInfo { get; set; }
        public Action<string> InstallCallback { get; set; }
    }
}
