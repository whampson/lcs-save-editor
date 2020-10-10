using System.ComponentModel;
using System.Windows.Interop;
using LCSSaveEditor.GUI.Events;

namespace LCSSaveEditor.GUI.Views
{
    public class DialogBase : WindowBase
    {
        public new ViewModels.DialogBase ViewModel
        {
            get { return (ViewModels.DialogBase) DataContext; }
            set { DataContext = value; }
        }

        protected override void WindowLoaded()
        {
            base.WindowLoaded();
            ViewModel.DialogCloseRequest += ViewModel_DialogCloseRequest;
        }

        protected override void WindowClosing(CancelEventArgs e)
        {
            base.WindowClosing(e);
            ViewModel.DialogCloseRequest -= ViewModel_DialogCloseRequest;
        }

        private void ViewModel_DialogCloseRequest(object sender, DialogCloseEventArgs e)
        {
            if (ComponentDispatcher.IsThreadModal)
            {
                DialogResult = e.DialogResult;
            }
            Close();
        }
    }
}
