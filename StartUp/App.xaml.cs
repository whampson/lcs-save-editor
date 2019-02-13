using LcsSaveEditor.Helpers;
using LcsSaveEditor.Infrastructure;
using LcsSaveEditor.Resources;
using LcsSaveEditor.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace LcsSaveEditor.StartUp
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        private string GetAppVersionString()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            FileVersionInfo vInfo = FileVersionInfo.GetVersionInfo(asm.Location);

            return string.Format("{0} (build {1})",
                vInfo.ProductVersion, vInfo.FilePrivatePart);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Logger.ConsumeStandardOut();
            Logger.ConsumeStandardError();

            Logger.Info(Strings.AppName);
            Logger.Info("Version {0}", GetAppVersionString());

            MainWindow = new MainWindow();
            MainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
#if !DEBUG
            Logger.Fatal(e.Exception);
            string logFile = string.Format("{0}_{1}.log",
                Assembly.GetExecutingAssembly().GetName().Name,
                DateTime.Now.ToString("yyyyMMddHHmmss"));
            Logger.WriteLogFile(logFile);       // TODO: put in Documents or AppData rather than working dir

            MessageBoxEx.Show(
                MainWindow,
                string.Format("{0}\n\n{1}: {2}\n\n{3}",
                    Strings.DialogTextUnhandledException1,
                    e.Exception.GetType().Name,
                    e.Exception.Message,
                    string.Format(Strings.DialogTextUnhandledException2,
                        Strings.AppAuthorContact, logFile)),
                Strings.DialogTitleUnhandledException,
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            e.Handled = true;
            Application.Current.Shutdown();
#endif
        }
    }
}
