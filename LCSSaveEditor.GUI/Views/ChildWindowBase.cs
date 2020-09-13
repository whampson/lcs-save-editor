using System;
using System.ComponentModel;
using System.Windows;

namespace LCSSaveEditor.GUI.Views
{
    public class ChildWindowBase : WindowBase
    {
        public new ViewModels.ChildWindowBase ViewModel
        {
            get { return (ViewModels.ChildWindowBase) DataContext; }
            set { DataContext = value; }
        }

        public bool HideOnClose { get; set; }

        public ChildWindowBase()
            : base()
        {
            HideOnClose = true;
        }

        protected override void WindowLoaded()
        {
            base.WindowLoaded();
            ViewModel.WindowHideRequest += ViewModel_WindowHideRequest;
        }

        protected override void WindowClosing(CancelEventArgs e)
        {
            if (HideOnClose)
            {
                e.Cancel = true;
                Hide();
                return;
            }

            base.WindowClosing(e);
            ViewModel.WindowHideRequest -= ViewModel_WindowHideRequest;
        }

        protected virtual void WindowVisibleChanged(bool isVisible)
        { }

        protected void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsInitialized && e.NewValue is bool isVisible)
            {
                WindowVisibleChanged(isVisible);
            }
        }

        private void ViewModel_WindowHideRequest(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
