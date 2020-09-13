using System;

namespace LCSSaveEditor.GUI.Events
{
    public class FileIOEventArgs : EventArgs
    {
        public string Path { get; set; }

        public bool SuppressPrompting { get; set; }
    }
}
