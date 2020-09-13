using System;
using System.Windows.Input;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.ViewModels
{
    public abstract class ChildWindowBase : WindowBase
    {
        public event EventHandler WindowHideRequest;
        public event EventHandler WindowCloseRequest;

        public ICommand WindowHideCommand => new RelayCommand
        (
            () => WindowHideRequest?.Invoke(this, EventArgs.Empty)
        );

        public ICommand WindowCloseCommand => new RelayCommand
        (
            () => WindowCloseRequest?.Invoke(this, EventArgs.Empty)
        );
    }
}
