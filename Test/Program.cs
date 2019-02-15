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
using LcsSaveEditor.Models;
using System;
using System.IO;

namespace Test
{
    /// <summary>
    /// Playground for testing code.
    /// </summary>
    public class Program
    {
        const string PlatformPS2 = "PS2";
        const string PlatformPSP = "PSP";
        const string PlatformIOS = "iOS";
        const string PlatformAndroid = "Android";

        public static void Main(string[] args)
        {
            LoggerTest();
        }

        static void LoggerTest()
        {
            Console.WriteLine("Console output not in log");
            Logger.Info("Logger started.");
            Logger.ConsumeStandardOut();
            Console.Write("Console output with LF\n");
            Console.Write("Console output with CRLF\r\n");
            Console.WriteLine("Console output with native line terminator");
            Console.WriteLine("Another\r\nTest\nwith\r\nlinebreaks");
            Console.Error.WriteLine("Some error occurred");
            Logger.Info("Switching StdOut to Info level...");
            Logger.StandardOutLogLevel = LogLevel.Info;
            Logger.Info("StdOut level switched!");
            Console.WriteLine("Hello, world!");
            Logger.Info("Logger ended.");
            Logger.WriteLogFile("test.log");
        }

        static void LoadPrintStore(string platform, int num)
        {
            string path = GetSaveFilePath(platform, num);

            Console.WriteLine("Loading {0}...\n", Path.GetFileName(path));
            SaveData save = SaveData.Load(path);

            SaveInfo.PrintInfo(save);
            SaveInfo.PrintSimpleVars(save);
            SaveInfo.PrintScripts(save);
            SaveInfo.PrintGarages(save);
            SaveInfo.PrintPlayerInfo(save);
            SaveInfo.PrintStats(save);

            save.Store(path + "_out");
        }

        static string GetSaveFilePath(string platform, int num)
        {
            string appPath = AppContext.BaseDirectory;
            string saveData = appPath + @"..\..\SaveData";
            string filePath = string.Format(@"{0}\{1}\lcs_{2}_save{3:00}",
                saveData, platform, platform.ToLower(), num);

            return filePath;
        }
    }
}
