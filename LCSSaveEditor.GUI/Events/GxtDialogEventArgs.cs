using System;

namespace LCSSaveEditor.GUI.Events
{
    public class GxtDialogEventArgs : EventArgs
    {
        public string TableName { get; set; }
        public bool AllowTableSelection { get; set; }
        public string SelectedKey { get; set; }
        public string SelectedValue { get; set; }
        public bool Modal { get; set; }
        public Action<bool?, GxtDialogEventArgs> Callback { get; set; }
    }
}
