using LCSSaveEditor.GUI.Events;
using System;
using System.Windows.Input;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.ViewModels
{
    public abstract class DialogBase : WindowBase
    {
        public event EventHandler<DialogCloseEventArgs> DialogCloseRequest;

        public DialogBase()
            : base()
        { }

        public void CloseDialog(bool? result = null)
        {
            DialogCloseRequest?.Invoke(this, new DialogCloseEventArgs(result));
        }

        public ICommand CloseCommand => new RelayCommand<bool?>
        (
            (result) => CloseDialog(result)
        );

        public ICommand CancelCommand => new RelayCommand
        (
            () => CloseDialog(false)
        );
    }
}
