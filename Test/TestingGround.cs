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
            string path = GetSaveFilePath(PlatformPS2, 3);

            Console.WriteLine("Loading {0}...\n", Path.GetFileName(path));
            SaveData save = SaveData.Load(path);

            PrintInfo(save);
            PrintSimpleVars(save);
            PrintScripts(save);
            PrintGarages(save);
            PrintPlayerInfo(save);
            PrintStats(save);

            save.Store(path + "_out");

            Console.Write("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void PrintInfo(SaveData save)
        {
            Console.WriteLine("==================== File Info ====================");
            Console.WriteLine("Format: {0}", save.FileType);
            Console.WriteLine();
        }

        static void PrintSimpleVars(SaveData save)
        {
            Console.WriteLine("==================== Simple Vars ====================");
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
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.UseVibrantColors), save.SimpleVars.UseVibrantColors);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.WidescreenEnabled), save.SimpleVars.WidescreenEnabled);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.RadioVolume), save.SimpleVars.RadioVolume);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.SfxVolume), save.SimpleVars.SfxVolume);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.ControllerConfiguration), save.SimpleVars.ControllerConfiguration);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.InvertLook), save.SimpleVars.InvertLook);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.VibrationEnabled), save.SimpleVars.VibrationEnabled);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.SwapAnalogAndDPad), save.SimpleVars.SwapAnalogAndDPad);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.HasPlayerCheated), save.SimpleVars.HasPlayerCheated);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.TaxiBounceEnabled), save.SimpleVars.TaxiBounceEnabled);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.ShowWaypoint), save.SimpleVars.ShowWaypoint);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.WaypointPosition), save.SimpleVars.WaypointPosition);
            Console.WriteLine("{0}: {1}", nameof(save.SimpleVars.Timestamp), save.SimpleVars.Timestamp);
            Console.WriteLine();
        }

        static void PrintScripts(SaveData save)
        {
            Console.WriteLine("==================== Scripts ====================");
            Console.WriteLine("{0}: {1}", nameof(save.Scripts.GlobalVariablesSize), save.Scripts.GlobalVariablesSize);
            Console.WriteLine("{0}[{1}]:", nameof(save.Scripts.GlobalVariables), save.Scripts.GlobalVariables.Count);
            for (int i = 0; i < save.Scripts.GlobalVariables.Count; i++) {
                Console.WriteLine("    {0}[{1}]: {2}", nameof(save.Scripts.GlobalVariables), i, save.Scripts.GlobalVariables[i]);
            }
            Console.WriteLine("{0}: {1}", nameof(save.Scripts.OnMissionFlag), save.Scripts.OnMissionFlag);
            Console.WriteLine("{0}: {1}", nameof(save.Scripts.LastMissionPassedTime), save.Scripts.LastMissionPassedTime);
            Console.WriteLine("{0}[{1}]:", nameof(save.Scripts.CollectiveArray), Scripts.CollectiveArrayCount);
            for (int i = 0; i < Scripts.CollectiveArrayCount; i++) {
                Console.WriteLine("    {0}[{1}]: {2}", nameof(save.Scripts.CollectiveArray), i, save.Scripts.CollectiveArray[i]);
            }
            Console.WriteLine("{0}: {1}", nameof(save.Scripts.NextFreeCollective), save.Scripts.NextFreeCollective);
            Console.WriteLine("{0}[{1}]:", nameof(save.Scripts.BuildingSwapArray), Scripts.BuildingSwapArrayCount);
            for (int i = 0; i < Scripts.BuildingSwapArrayCount; i++) {
                Console.WriteLine("    {0}[{1}]: {2}", nameof(save.Scripts.BuildingSwapArray), i, save.Scripts.BuildingSwapArray[i]);
            }
            Console.WriteLine("{0}[{1}]:", nameof(save.Scripts.InvisibilitySettingArray), Scripts.InvisibilitySettingArrayCount);
            for (int i = 0; i < Scripts.InvisibilitySettingArrayCount; i++) {
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
            Console.WriteLine();
        }

        static void PrintGarages(SaveData save)
        {
            Console.WriteLine("==================== Garages ====================");
            Console.WriteLine("{0}: {1}", nameof(save.Garages.NumberOfGarages), save.Garages.NumberOfGarages);
            Console.WriteLine("{0}: {1}", nameof(save.Garages.BombsAreFree), save.Garages.BombsAreFree);
            Console.WriteLine("{0}: {1}", nameof(save.Garages.RespraysAreFree), save.Garages.RespraysAreFree);
            Console.WriteLine("{0}: {1}", nameof(save.Garages.CarTypesCollected), save.Garages.CarTypesCollected);
            Console.WriteLine("{0}: {1}", nameof(save.Garages.LastTimeHelpMessage), save.Garages.LastTimeHelpMessage);
            Console.WriteLine("{0}[{1}]:", nameof(save.Garages.StoredCars), Garages.StoredCarsCount);
            for (int i = 0; i < Garages.StoredCarsCount; i++) {
                Console.WriteLine("    {0}[{1}]: {2}", nameof(save.Garages.StoredCars), i, save.Garages.StoredCars[i]);
            }
            Console.WriteLine("{0}[{1}]:", nameof(save.Garages.GarageObjects), Garages.GarageObjectsCount);
            for (int i = 0; i < Garages.GarageObjectsCount; i++) {
                Console.WriteLine("    {0}[{1}]: {2}", nameof(save.Garages.GarageObjects), i, save.Garages.GarageObjects[i]);
            }
            Console.WriteLine();
        }

        static void PrintPlayerInfo(SaveData save)
        {
            Console.WriteLine("==================== Player Info ====================");
            Console.WriteLine("{0}: {1}", nameof(save.PlayerInfo.Money), save.PlayerInfo.Money);
            Console.WriteLine("{0}: {1}", nameof(save.PlayerInfo.MoneyOnScreen), save.PlayerInfo.MoneyOnScreen);
            Console.WriteLine("{0}: {1}", nameof(save.PlayerInfo.NumHiddenPackagesFound), save.PlayerInfo.NumHiddenPackagesFound);
            Console.WriteLine("{0}: {1}", nameof(save.PlayerInfo.MaxHealth), save.PlayerInfo.MaxHealth);
            Console.WriteLine("{0}: {1}", nameof(save.PlayerInfo.MaxArmor), save.PlayerInfo.MaxArmor);
            Console.WriteLine("{0}: {1}", nameof(save.PlayerInfo.HasInfiniteSprint), save.PlayerInfo.HasInfiniteSprint);
            Console.WriteLine("{0}: {1}", nameof(save.PlayerInfo.HasFastReload), save.PlayerInfo.HasFastReload);
            Console.WriteLine("{0}: {1}", nameof(save.PlayerInfo.IsFireProof), save.PlayerInfo.IsFireProof);
            Console.WriteLine("{0}: {1}", nameof(save.PlayerInfo.HasGetOutOfJailFree), save.PlayerInfo.HasGetOutOfJailFree);
            Console.WriteLine("{0}: {1}", nameof(save.PlayerInfo.HasFreeHealthcare), save.PlayerInfo.HasFreeHealthcare);
            Console.WriteLine("{0}: {1}", nameof(save.PlayerInfo.CanDoDriveBy), save.PlayerInfo.CanDoDriveBy);
            Console.WriteLine();
        }

        static void PrintStats(SaveData save)
        {
            Console.WriteLine("==================== Stats ====================");
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PeopleKilledByPlayer), save.Stats.PeopleKilledByPlayer);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PeopleKilledByOthers), save.Stats.PeopleKilledByOthers);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.CarsExploded), save.Stats.CarsExploded);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.BoatsExploded), save.Stats.BoatsExploded);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.TyresPopped), save.Stats.TyresPopped);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.RoundsFiredByPlayer), save.Stats.RoundsFiredByPlayer);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType00Wasted), save.Stats.PedType00Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType01Wasted), save.Stats.PedType01Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType02Wasted), save.Stats.PedType02Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType03Wasted), save.Stats.PedType03Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType04Wasted), save.Stats.PedType04Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType05Wasted), save.Stats.PedType05Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType06Wasted), save.Stats.PedType06Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType07Wasted), save.Stats.PedType07Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType08Wasted), save.Stats.PedType08Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType09Wasted), save.Stats.PedType09Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType10Wasted), save.Stats.PedType10Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType11Wasted), save.Stats.PedType11Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType12Wasted), save.Stats.PedType12Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType13Wasted), save.Stats.PedType13Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType14Wasted), save.Stats.PedType14Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType15Wasted), save.Stats.PedType15Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType16Wasted), save.Stats.PedType16Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType17Wasted), save.Stats.PedType17Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType18Wasted), save.Stats.PedType18Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType19Wasted), save.Stats.PedType19Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType20Wasted), save.Stats.PedType20Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType21Wasted), save.Stats.PedType21Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PedType22Wasted), save.Stats.PedType22Wasted);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.HelisDestroyed), save.Stats.HelisDestroyed);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.ProgressMade), save.Stats.ProgressMade);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.TotalProgressInGame), save.Stats.TotalProgressInGame);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.KgsOfExplosivesUsed), save.Stats.KgsOfExplosivesUsed);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.BulletsThatHit), save.Stats.BulletsThatHit);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.CarsCrushed), save.Stats.CarsCrushed);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.HeadsPopped), save.Stats.HeadsPopped);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.WantedStarsAttained), save.Stats.WantedStarsAttained);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.WantedStarsEvaded), save.Stats.WantedStarsEvaded);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.TimesArrested), save.Stats.TimesArrested);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.TimesDied), save.Stats.TimesDied);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.DaysPassed), save.Stats.DaysPassed);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.SafeHouseVisits), save.Stats.SafeHouseVisits);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.Sprayings), save.Stats.Sprayings);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.MaximumJumpDistance), save.Stats.MaximumJumpDistance);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.MaximumJumpHeight), save.Stats.MaximumJumpHeight);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.MaximumJumpFlips), save.Stats.MaximumJumpFlips);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.MaximumJumpSpins), save.Stats.MaximumJumpSpins);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.BestStuntJump), save.Stats.BestStuntJump);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.NumberOfUniqueJumpsFound), save.Stats.NumberOfUniqueJumpsFound);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.TotalNumberOfUniqueJumps), save.Stats.TotalNumberOfUniqueJumps);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.MissionsGiven), save.Stats.MissionsGiven);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PassengersDroppedOffWithTaxi), save.Stats.PassengersDroppedOffWithTaxi);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.MoneyMadeWithTaxi), save.Stats.MoneyMadeWithTaxi);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.IndustrialPassed), save.Stats.IndustrialPassed);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.CommercialPassed), save.Stats.CommercialPassed);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.SuburbanPassed), save.Stats.SuburbanPassed);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PamphletMissionPassed), save.Stats.PamphletMissionPassed);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.DistanceTravelledOnFoot), save.Stats.DistanceTravelledOnFoot);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.DistanceTravelledByCar), save.Stats.DistanceTravelledByCar);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.DistanceTravelledByBike), save.Stats.DistanceTravelledByBike);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.DistanceTravelledByBoat), save.Stats.DistanceTravelledByBoat);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.LivesSavedWithAmbulance), save.Stats.LivesSavedWithAmbulance);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.CriminalsCaught), save.Stats.CriminalsCaught);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.FiresExtinguished), save.Stats.FiresExtinguished);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.HighestLevelVigilanteMission), save.Stats.HighestLevelVigilanteMission);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.HighestLevelAmbulanceMission), save.Stats.HighestLevelAmbulanceMission);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.HighestLevelFireMission), save.Stats.HighestLevelFireMission);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PhotosTaken), save.Stats.PhotosTaken);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.NumberKillFrenziesPassed), save.Stats.NumberKillFrenziesPassed);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.MaxSecondsOnCarnageLeft), save.Stats.MaxSecondsOnCarnageLeft);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.MaxKillsOnRcTriad), save.Stats.MaxKillsOnRcTriad);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.TotalNumberKillFrenzies), save.Stats.TotalNumberKillFrenzies);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.TotalNumberMissions), save.Stats.TotalNumberMissions);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.TimesDrowned), save.Stats.TimesDrowned);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.SeagullsKilled), save.Stats.SeagullsKilled);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.WeaponBudget), save.Stats.WeaponBudget);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PizzasDelivered), save.Stats.PizzasDelivered);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.NoodlesDelivered), save.Stats.NoodlesDelivered);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.MoneyMadeFromTourist), save.Stats.MoneyMadeFromTourist);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.TouristsTakenToSpots), save.Stats.TouristsTakenToSpots);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.TopScrapyardChallengeScore), save.Stats.TopScrapyardChallengeScore);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.Top9mmMayhemScore), save.Stats.Top9mmMayhemScore);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.TopScooterShooterScore), save.Stats.TopScooterShooterScore);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.TopWichitaWipeoutScore), save.Stats.TopWichitaWipeoutScore);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.LongestWheelie), save.Stats.LongestWheelie);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.LongestStoppie), save.Stats.LongestStoppie);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.Longest2Wheel), save.Stats.Longest2Wheel);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.LongestWheelieDist), save.Stats.LongestWheelieDist);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.LongestStoppieDist), save.Stats.LongestStoppieDist);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.Longest2WheelDist), save.Stats.Longest2WheelDist);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.LongestFacePlantDist), save.Stats.LongestFacePlantDist);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.AutoPaintingBudget), save.Stats.AutoPaintingBudget);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.PropertyDestroyed), save.Stats.PropertyDestroyed);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.UnlockedCostumes), save.Stats.UnlockedCostumes);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.BloodringKills), save.Stats.BloodringKills);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.BloodringTime), save.Stats.BloodringTime);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.HighestChaseValue), save.Stats.HighestChaseValue);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.KillsSinceLastCheckpoint), save.Stats.KillsSinceLastCheckpoint);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.TotalLegitimateKills), save.Stats.TotalLegitimateKills);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.LastMissionPassedName), save.Stats.LastMissionPassedName);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.CheatedCount), save.Stats.CheatedCount);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.CarsSold), save.Stats.CarsSold);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.MoneyMadeWithCarSales), save.Stats.MoneyMadeWithCarSales);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.BikesSold), save.Stats.BikesSold);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.MoneyMadeWithBikeSales), save.Stats.MoneyMadeWithBikeSales);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.NumberOfExportedCars), save.Stats.NumberOfExportedCars);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.TotalNumberOfCarExport), save.Stats.TotalNumberOfCarExport);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.HighestLevelSlashTv), save.Stats.HighestLevelSlashTv);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.MoneyMadeWithSlashTv), save.Stats.MoneyMadeWithSlashTv);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.TotalKillsOnSlashTv), save.Stats.TotalKillsOnSlashTv);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.CashMadeCollectingTrash), save.Stats.CashMadeCollectingTrash);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.HitmenKilled), save.Stats.HitmenKilled);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.HighestGuarduanAngelJustuceDished), save.Stats.HighestGuarduanAngelJustuceDished);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.GuardianAngelMissionsPassed), save.Stats.GuardianAngelMissionsPassed);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.GuardianAngelHighestLevelIndustrial), save.Stats.GuardianAngelHighestLevelIndustrial);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.GuardianAngelHighestLevelCommercial), save.Stats.GuardianAngelHighestLevelCommercial);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.GuardianAngelHighestLevelSuburban), save.Stats.GuardianAngelHighestLevelSuburban);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.MostTimeLeftTrainRace), save.Stats.MostTimeLeftTrainRace);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.BestTimeGoGoFaggio), save.Stats.BestTimeGoGoFaggio);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.DirtBikeMostAir), save.Stats.DirtBikeMostAir);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.NumberOutfitChanges), save.Stats.NumberOutfitChanges);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.BestBanditLapTimes), save.Stats.BestBanditLapTimes);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.BestBanditPositions), save.Stats.BestBanditPositions);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.BestStreetRacePositions), save.Stats.BestStreetRacePositions);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.FastestStreetRaceLapTimes), save.Stats.FastestStreetRaceLapTimes);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.FastestStreetRaceTimes), save.Stats.FastestStreetRaceTimes);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.FastestDirtBikeLapTimes), save.Stats.FastestDirtBikeLapTimes);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.FastestDirtBikeTimes), save.Stats.FastestDirtBikeTimes);
            Console.WriteLine("{0}: {1}", nameof(save.Stats.FavoriteRadioStationList), save.Stats.FavoriteRadioStationList);
            Console.WriteLine();
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
