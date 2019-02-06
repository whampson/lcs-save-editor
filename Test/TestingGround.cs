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

using LcsSaveEditor.Models;
using Microsoft.Win32;
using System;
using System.IO;

namespace Test
{
    static class TestingGround
    {
        const string PlatformPS2 = "PS2";
        const string PlatformPSP = "PSP";
        const string PlatformIOS = "iOS";
        const string PlatformAndroid = "Android";

        static void Main(string[] args)
        {
            string path = GetSaveFilePath(PlatformIOS, 0);

            Console.WriteLine("Loading {0}...", Path.GetFileName(path));
            SaveData save = SaveData.Load(path);

            Console.WriteLine("Format: {0}", save.FileType);
            PrintGarages(save);

            save.Store(path + "_out");

            Console.Write("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void PrintSimpleVars(SaveData save)
        {
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.SaveNameGxt), save.SimpleVars.SaveNameGxt);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.Language), save.SimpleVars.Language);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.MillisecondsPerGameMinute), save.SimpleVars.MillisecondsPerGameMinute);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.LastClockTick), save.SimpleVars.LastClockTick);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.GameClockHours), save.SimpleVars.GameClockHours);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.GameClockMinutes), save.SimpleVars.GameClockMinutes);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.TotalTimePlayedInMilliseconds), save.SimpleVars.TotalTimePlayedInMilliseconds);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.PreviousWeather), save.SimpleVars.PreviousWeather);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.CurrentWeather), save.SimpleVars.CurrentWeather);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.ForcedWeather), save.SimpleVars.ForcedWeather);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.WeatherIndex), save.SimpleVars.WeatherIndex);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.CameraPosition), save.SimpleVars.CameraPosition);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.VehicleCamera), save.SimpleVars.VehicleCamera);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.Brightness), save.SimpleVars.Brightness);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.ShowHud), save.SimpleVars.ShowHud);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.ShowSubtitles), save.SimpleVars.ShowSubtitles);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.RadarMode), save.SimpleVars.RadarMode);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.BlurOn), save.SimpleVars.BlurOn);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.Widescreen), save.SimpleVars.Widescreen);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.RadioVolume), save.SimpleVars.RadioVolume);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.SfxVolume), save.SimpleVars.SfxVolume);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.ControllerConfiguration), save.SimpleVars.ControllerConfiguration);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.InvertLook), save.SimpleVars.InvertLook);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.Vibration), save.SimpleVars.Vibration);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.PlayerHasCheated), save.SimpleVars.PlayerHasCheated);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.ShowWaypoint), save.SimpleVars.ShowWaypoint);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.WaypointPosition), save.SimpleVars.WaypointPosition);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.Timestamp), save.SimpleVars.Timestamp);
        }

        static void PrintScripts(SaveData save)
        {
            Console.WriteLine("{0}: {1}", nameof(save.Scripts.GlobalVariablesSize), save.Scripts.GlobalVariablesSize);
            Console.WriteLine("{0}[{1}]:", nameof(save.Scripts.GlobalVariables), save.Scripts.GlobalVariables.Count);
            for (int i = 0; i < save.Scripts.GlobalVariables.Count; i++) {
                Console.WriteLine("    {0}[{1}]: {2}", nameof(save.Scripts.GlobalVariables), i, save.Scripts.GlobalVariables[i]);
            }
            Console.WriteLine("{0}: {1}", nameof(save.Scripts.OnMissionFlag), save.Scripts.OnMissionFlag);
            Console.WriteLine("{0}: {1}", nameof(save.Scripts.LastMissionPassedTime), save.Scripts.LastMissionPassedTime);
            Console.WriteLine("{0}[{1}]:", nameof(save.Scripts.CollectiveArray), save.Scripts.CollectiveArray.Length);
            for (int i = 0; i < save.Scripts.CollectiveArray.Length; i++) {
                Console.WriteLine("    {0}[{1}]: {2}", nameof(save.Scripts.CollectiveArray), i, save.Scripts.CollectiveArray[i]);
            }
            Console.WriteLine("{0}: {1}", nameof(save.Scripts.NextFreeCollective), save.Scripts.NextFreeCollective);
            Console.WriteLine("{0}[{1}]:", nameof(save.Scripts.BuildingSwapArray), save.Scripts.BuildingSwapArray.Length);
            for (int i = 0; i < save.Scripts.BuildingSwapArray.Length; i++) {
                Console.WriteLine("    {0}[{1}]: {2}", nameof(save.Scripts.BuildingSwapArray), i, save.Scripts.BuildingSwapArray[i]);
            }
            Console.WriteLine("{0}[{1}]:", nameof(save.Scripts.InvisibilitySettingArray), save.Scripts.InvisibilitySettingArray.Length);
            for (int i = 0; i < save.Scripts.InvisibilitySettingArray.Length; i++) {
                Console.WriteLine("    {0}[{1}]: {2}", nameof(save.Scripts.InvisibilitySettingArray), i, save.Scripts.InvisibilitySettingArray[i]);
            }
            Console.WriteLine("{0}: {1}", nameof(save.Scripts.UsingAMultiScriptFile), save.Scripts.UsingAMultiScriptFile);
            Console.WriteLine("{0}: {1}", nameof(save.Scripts.MainScriptSize), save.Scripts.MainScriptSize);
            Console.WriteLine("{0}: {1}", nameof(save.Scripts.LargestMissionScriptSize), save.Scripts.LargestMissionScriptSize);
            Console.WriteLine("{0}: {1}", nameof(save.Scripts.NumberOfMissionScripts), save.Scripts.NumberOfMissionScripts);
            Console.WriteLine("{0}: {1}", nameof(save.Scripts.NumberOfExclusiveMissionScripts), save.Scripts.NumberOfExclusiveMissionScripts);
            Console.WriteLine("{0}[{1}]:", nameof(save.Scripts.RunningScripts), save.Scripts.RunningScripts.Count);
            for (int i = 0; i < save.Scripts.RunningScripts.Count; i++) {
                Console.WriteLine("    {0}[{1}]: {2}", nameof(save.Scripts.RunningScripts), i, save.Scripts.RunningScripts[i]);
            }
        }

        static void PrintGarages(SaveData save)
        {
            Console.WriteLine("{0}: {1}", nameof(save.Garages.NumberOfGarages), save.Garages.NumberOfGarages);
            Console.WriteLine("{0}: {1}", nameof(save.Garages.BombsAreFree), save.Garages.BombsAreFree);
            Console.WriteLine("{0}: {1}", nameof(save.Garages.RespraysAreFree), save.Garages.RespraysAreFree);
            Console.WriteLine("{0}: {1}", nameof(save.Garages.CarTypesCollected), save.Garages.CarTypesCollected);
            Console.WriteLine("{0}: {1}", nameof(save.Garages.LastTimeHelpMessage), save.Garages.LastTimeHelpMessage);
            Console.WriteLine("{0}[{1}]:", nameof(save.Garages.StoredCars), save.Garages.StoredCars.Length);
            for (int i = 0; i < save.Garages.StoredCars.Length; i++) {
                Console.WriteLine("    {0}[{1}]: {2}", nameof(save.Garages.StoredCars), i, save.Garages.StoredCars[i]);
            }
            Console.WriteLine("{0}[{1}]:", nameof(save.Garages.Garages), save.Garages.Garages.Length);
            for (int i = 0; i < save.Garages.Garages.Length; i++) {
                Console.WriteLine("    {0}[{1}]: {2}", nameof(save.Garages.Garages), i, save.Garages.Garages[i]);
            }
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
