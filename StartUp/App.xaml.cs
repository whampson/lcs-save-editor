using LcsSaveEditor.Helpers;
using LcsSaveEditor.Resources;
using LcsSaveEditor.Views;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace LcsSaveEditor.StartUp
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow = new MainWindow();
            MainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
#if !DEBUG
            e.Handled = true;

            // TODO: crashdump

            MessageBoxEx.Show(
                MainWindow,
                string.Format("{0}\n\n{1}: {2}\n\n{3}",
                    Strings.DialogTextUnhandledException1,
                    e.Exception.GetType().Name,
                    e.Exception.Message,
                    string.Format(Strings.DialogTextUnhandledException2, Strings.AppAuthorContact)),
                Strings.DialogTitleUnhandledException,
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            // Kill the process to bypass shutdown hooks
            Process.GetCurrentProcess().Kill();
#endif
        }
    }
}
