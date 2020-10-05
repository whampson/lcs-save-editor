using GTASaveData;
using GTASaveData.LCS;
using LCSSaveEditor.Core;
using LCSSaveEditor.GUI.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LCSSaveEditor.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string Name => "GTA:LCS Save Editor";
        public static string Copyright => $"(C) 2016-2020 {Author}";
        public static string Author => "Wes Hampson";
        public static string AuthorAlias => "thehambone";
        public static string AuthorContact => "thehambone93@gmail.com";
        public static Uri AuthorContactMailTo => new Uri($"mailto:{AuthorContact}");
        public static Uri AuthorDonateUrl => new Uri(@"https://ko-fi.com/thehambone");
        public static Uri ProjectUrl => new Uri(@"https://github.com/whampson/lcs-save-editor");
        public static Uri ProjectTopicUrl => new Uri(@"https://gtaforums.com/index.php?showtopic=847469");

        private static Uri GxtResource => new Uri(@"pack://application:,,,/Resources/ENGLISH.GXT");
        private static Uri CarcolsResource => new Uri(@"pack://application:,,,/Resources/carcols.dat");
        private static string SettingsPath => "settings.json";

        public static string Version => Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            .InformationalVersion;

        public static string AssemblyName => Assembly
            .GetExecutingAssembly()
            .GetName()
            .Name;

        public static Version AssemblyFileVersion => new Version(
            Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyFileVersionAttribute>()
            .Version);

        private static readonly StringWriter LogWriter;
        public static string LogText => LogWriter.ToString();

        public static MainWindow TheWindow { get; private set; }

        static App()
        {
            LogWriter = new StringWriter();
        }

        public static void ExitApp()
        {
            Application.Current.Shutdown();
        }

        public static byte[] LoadResource(Uri resource)
        {
            using (MemoryStream m = new MemoryStream())
            {
                GetResourceStream(resource).Stream.CopyTo(m);
                return m.ToArray();
            }
        }

        private static void LoadSettings()
        {
            Settings.LoadSettings(SettingsPath);
        }

        private static void SaveSettings()
        {
            Settings.TheSettings.SaveSettings(SettingsPath);
        }

        private static void LoadCarcols()
        {
            byte[] carcols = LoadResource(CarcolsResource);
            Carcols.TheCarcols.Load(carcols);
        }

        private static void LoadGxt()
        {
            byte[] gxt = LoadResource(GxtResource);
            Gxt.TheText.Load(gxt);
        }

        private static void CleanupAfterUpdate()
        {
            UpdaterSettings s = Settings.TheSettings.Updater;
            if (s.CleanupAfterUpdate)
            {
                Log.Info("Cleaning up old version files...");
                s.CleanupAfterUpdate = false;

                int count = 0; 
                while (s.CleanupList.Count > 0)
                {
                    try
                    {
                        File.Delete(s.CleanupList[0]);
                        Log.Info($"Deleted {s.CleanupList[0]}");
                        count++;
                    }
                    catch (Exception e)
                    {
                        if (e is DirectoryNotFoundException ||
                            e is UnauthorizedAccessException)
                        {
                            Log.Exception(e);
                            continue;
                        }

                        throw;
                    }

                    s.CleanupList.RemoveAt(0);
                }

                Log.Info(string.Format("Cleaned up {0} file{1}.", count, count != 1 ? "s" : ""));
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += Application_UnhandledException;

            Log.InfoStream = LogWriter;
            Log.ErrorStream = LogWriter;

            string osVer = RuntimeInformation.OSDescription;
            string dotnetVer = RuntimeInformation.FrameworkDescription;
            string gtaLibVer = Assembly.GetAssembly(typeof(SaveData)).GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
            string lcsLibVer = Assembly.GetAssembly(typeof(LCSSave)).GetCustomAttribute<AssemblyFileVersionAttribute>().Version;

            Log.Info($"{Name} {Version}");
            Log.Info($"{Copyright}");
            Log.Info(new string('=', 40));
            Log.Info($"Operating System: {osVer}");
            Log.Info($"    .NET Runtime: {dotnetVer}");
            Log.Info($"GTASaveData.Core: {gtaLibVer}");
            Log.Info($" GTASaveData.LCS: {lcsLibVer}");
            Log.Info($" Save Editor EXE: {AssemblyFileVersion}");

#if DEBUG
            Log.Info($"DEBUG build.");
#endif
#if RELEASE_STANDALONE
            Settings.TheSettings.Updater.StandaloneRing = true;
            Log.Info("Standalone build.");
#endif

            LoadSettings();
            LoadCarcols();
            LoadGxt();

            CleanupAfterUpdate();

            TheWindow = new MainWindow();
            TheWindow.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            SaveSettings();
            Log.Info("Exiting...");

            if (Settings.TheSettings.WriteLogOnClose)
            {
                string logFile = $"{AssemblyName}_{DateTime.Now:yyyyMMddHHmmss}.log";
                File.WriteAllText(logFile, LogText);
            }
        }

        private void Application_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            Log.Error(ex);

            if (Debugger.IsAttached)
            {
                // Let the debugger handle the exception
                return;
            }

            Log.Info($"A catastrophic error has occurred. Please report this issue to {AuthorContact}.");

            string logFile = $"crash-dump_{DateTime.Now:yyyyMMddHHmmss}.log";
            File.WriteAllText(logFile, LogText);

            TheWindow.ViewModel.ShowError(
                $"An unhandled exception has occurred. The program will close and you will lose all unsaved changes.\n" +
                $"\n" +
                $"{ex.GetType().Name}: {ex.Message}\n" +
                $"\n" +
                $"A log file has been created: {logFile}. " +
                $"Please report this issue to {AuthorContact} and include this log file with your report.",
                "Unhandled Exception");

            Environment.Exit(1);
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.SelectAll();
            }
        }
    }
}
