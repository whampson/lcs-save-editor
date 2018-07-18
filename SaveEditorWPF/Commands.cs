using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SaveEditorWPF
{
    public static class Commands
    {
        public static RoutedUICommand Open = ApplicationCommands.Open;
        public static RoutedUICommand Close = ApplicationCommands.Close;
        public static RoutedUICommand Reload = new RoutedUICommand("Reload current file from disk", "Reload", typeof(MainWindow));
        public static RoutedUICommand Save = ApplicationCommands.Save;
        public static RoutedUICommand SaveAs = ApplicationCommands.SaveAs;
        public static RoutedUICommand Exit = new RoutedUICommand("Exit application", "Exit", typeof(MainWindow));
        public static RoutedUICommand About = new RoutedUICommand("About application", "About", typeof(MainWindow));
    }
}
