using GTASaveData.LCS;
using LCSSaveEditor.Core;
using LCSSaveEditor.GUI.Views;
using System;
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
        public static string Copyright => $"Copyright (C) 2016-2020 {Author}";
        public static string SettingsPath => "settings.json";

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
            // Let VisualStudio handle the exception in debug mode

#if !DEBUG
            File.WriteAllText($"lcs-save-editor_{DateTime.Now:yyyyMMddHHmmss}.log", LogText);
            ShowUnhandledExceptionMessage(e.ExceptionObject as Exception);
#endif
        }

#if !DEBUG
        private void ShowUnhandledExceptionMessage(Exception e)
        {
            string text = $"An unhandled exception has occurred.";
            if (e != null)
            {
                text += $"\n\n{ e.GetType().Name}: { e.Message}";
                if (e.StackTrace != null)
                {
                    text += $"\n\nStack trace:\n{e.StackTrace}\n";
                }
            }

            TheWindow.ViewModel.ShowError(text, "Unhandled Exception");
        }
#endif

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.SelectAll();
            }
        }
    }
}
