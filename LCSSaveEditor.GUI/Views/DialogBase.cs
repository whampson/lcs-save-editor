using System.Windows;

namespace LCSSaveEditor.GUI.Views
{
    public class DialogBase<T> : Window
        where T : ViewModels.DialogBase
    {
        public T ViewModel
        {
            get { return (T) DataContext; }
            set { DataContext = value; }
        }

        public DialogBase()
            : base()
        { }
    }
}
