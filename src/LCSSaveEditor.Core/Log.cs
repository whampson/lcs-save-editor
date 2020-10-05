using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace LCSSaveEditor.Core
{
    public static class Log
    {
        public static event EventHandler LogEvent;

        public static string InfoPrefix  => $"{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}  Info: ";
        public static string ErrorPrefix => $"{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} Error: ";
        public static TextWriter InfoStream { get; set; }
        public static TextWriter ErrorStream { get; set; }
        public static bool AutomaticNewline { get; set; }

        static Log()
        {
            InfoStream = Console.Out;
            ErrorStream = Console.Error;
            AutomaticNewline = true;
        }

        public static void Info(object value)
        {
            string txt = InfoPrefix + value.ToString();

            if (AutomaticNewline)
                InfoStream.WriteLine(txt);
            else
                InfoStream.Write(txt);

            LogEvent?.Invoke(null, EventArgs.Empty);
        }

        public static void InfoF(string format, params object[] args)
        {
            Info(string.Format(format, args));
        }

        public static void Error(object value)
        {
            string txt = ErrorPrefix + value.ToString();

            if (AutomaticNewline)
                ErrorStream.WriteLine(txt);
            else
                ErrorStream.Write(txt);

            LogEvent?.Invoke(null, EventArgs.Empty);
        }

        public static void ErrorF(string format, params object[] args)
        {
            Error(string.Format(format, args));
        }

        public static void Exception(Exception e, [CallerMemberName] string caller = null)
        {
            Error($"{caller}(): {e.GetType().Name}: {e.Message}");
        }
    }
}
