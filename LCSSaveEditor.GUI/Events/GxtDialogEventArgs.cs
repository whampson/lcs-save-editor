using System;
using System.Collections.Generic;
using System.Text;
using LCSSaveEditor.Core;

namespace LCSSaveEditor.GUI.Events
{
    public class GxtDialogEventArgs : EventArgs
    {
        public string TableName { get; set; }
        public bool AllowTableSelection { get; set; }
        public string SelectedKey { get; set; }
        public string SelectedValue { get; set; }
        public Action<bool?, GxtDialogEventArgs> Callback { get; set; }
    }
}
