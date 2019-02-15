#region License
/* Copyright(c) 2016-2019 Wes Hampson
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
#endregion

using LcsSaveEditor.Infrastructure;
using LcsSaveEditor.Resources;
using LcsSaveEditor.ViewModels;
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
        private void LoadSettings()
        {
            string path = Settings.DefaultSettingsPath;
            if (!File.Exists(path)) {
                return;
            }

            Logger.Info("Loading settings...");

            try {
                Settings.Current = Settings.Load(path);
            }
            catch (InvalidOperationException ex) {
                Logger.Error("Failed to load settings! {0}", ex.InnerException.Message);
            }
        }

        private void SaveSettings()
        {
            string path = Settings.DefaultSettingsPath;

            Logger.Info("Saving settings...");

            try {
                Settings.Current.Store(path);
            }
            catch (InvalidOperationException ex) {
                Logger.Error("Failed to save settings! {0}", ex.InnerException.Message);
            }
        }

        private void LoadUI()
        {
            Logger.Info("Loading UI...");

            MainWindow = new MainWindow();
            MainWindow.Show();
        }

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

            Logger.Info("========== {0} ==========", Strings.AppName);
            Logger.Info("Version: {0}", GetAppVersionString());
            Logger.Info("Host OS: {0}", Environment.OSVersion);
            Logger.Info("==========={0}===========", new string('=', Strings.AppName.Length));

            LoadSettings();
            LoadUI();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Logger.Info("Exiting...");
            SaveSettings();

            if (Logger.SaveOnExit) {
                Logger.WriteLogFile(Logger.SaveOnExitFilename);
            }
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
