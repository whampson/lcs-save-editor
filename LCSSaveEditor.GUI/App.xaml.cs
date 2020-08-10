using GTASaveData.LCS;
using LCSSaveEditor.Core;
using LCSSaveEditor.GUI.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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
        public static string Author => "Wes Hampson";
        public static string AuthorContact => "thehambone93@gmail.com";
        public static string Copyright => $"Copyright (C) 2016-2020 {Author}";
        public static string SettingsPath => "settings.json";
        public static Uri GxtResourceUri => new Uri(@"pack://application:,,,/Resources/ENGLISH.GXT");

        public static string InformationalVersion => Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            .InformationalVersion;

        public static string FileVersion => Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyFileVersionAttribute>()
            .Version;

        public static string SaveDataLibraryVersion => Assembly
            .GetAssembly(typeof(LCSSave))
            .GetCustomAttribute<AssemblyFileVersionAttribute>()
            .Version;


        private static readonly StringWriter LogWriter;
        public static string LogText => LogWriter.ToString();

        public MainWindow TheWindow { get; private set; }

        static App()
        {
            LogWriter = new StringWriter();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += Application_UnhandledException;

            Log.InfoStream = LogWriter;
            Log.ErrorStream = LogWriter;

            Log.Info($"{Name} {InformationalVersion}");
            Log.Info($"{Copyright}");
        #if DEBUG
            Log.Info($"DEBUG build.");
            Log.Info($"App version = {FileVersion}");
            Log.Info($"Lib version = {SaveDataLibraryVersion}");
        #endif

            TheWindow = new MainWindow
            {
                Height = 600,
                Width = 800
            };

            TheWindow.Show();
        }

        private void Application_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            Log.Error(ex);

            if (!Debugger.IsAttached)
            {
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

                return;
            }

            // Let the debugger handle the exception
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
