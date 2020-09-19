using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using GTASaveData.LCS;
using LCSSaveEditor.Core;
using LCSSaveEditor.GUI.Types;
using WpfEssentials;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class StatsWindow : ChildWindowBase
    {
        private ObservableCollection<Stat> m_statsList;
        private Stat m_criminalRating;
        private bool m_handlersRegistered;

        public ObservableCollection<Stat> Statistics
        {
            get { return m_statsList; }
            set { m_statsList = value; OnPropertyChanged(); }
        }

        public Stat CriminalRating
        {
            get { return m_criminalRating; }
            set { m_criminalRating = value; OnPropertyChanged(); }
        }
        public StatsWindow()
            : base()
        {
            Statistics = new ObservableCollection<Stat>();
            CriminalRating = new Stat();
        }

        public override void Initialize()
        {
            base.Initialize();

            TheEditor.FileOpened += TheEditor_FileOpened;
            TheEditor.FileClosing += TheEditor_FileClosing;

            RegisterChangeHandlers();
            RefreshStats();
        }

        private void Data_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender == SimpleVars)
            {
                switch (e.PropertyName)
                {
                    case nameof(SimpleVariables.TimeInMilliseconds):
                    case nameof(SimpleVariables.Language):
                        RefreshStats();
                        break;
                    case nameof(SimpleVariables.HasPlayerCheated):
                        UpdateCriminalRating(GetCriminalRatingNumber());
                        break;
                }
            }
            else if (sender == PlayerInfo)
            {
                switch (e.PropertyName)
                {
                    case nameof(PlayerInfo.Money):
                    case nameof(PlayerInfo.MoneyOnScreen):
                        UpdateCriminalRating(GetCriminalRatingNumber());
                        break;
                }
            }
            else if (sender == Stats)
            {
                RefreshStats();
            }
        }

        private void Data_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshStats();
        }

        public override void Shutdown()
        {
            base.Shutdown();

            TheEditor.FileOpened -= TheEditor_FileOpened;
            TheEditor.FileClosing -= TheEditor_FileClosing;

            UnregisterChangeHandlers();
        }

        public void RefreshStats()
        {
            Statistics.Clear();

            UpdateCriminalRating(GetCriminalRatingNumber());

            AddStat("PER_COM", Stats.ProgressMade / Stats.TotalProgressInGame, percentage: true);
            AddStat("NMISON", Stats.MissionsGiven);
            AddStat("ST_TIME", TimeSpan.FromMilliseconds(0));     // not saved
            AddStat("ST_OVTI", TimeSpan.FromMilliseconds(SimpleVars.TimeInMilliseconds));
            AddStat("DAYSPS", Stats.DaysPassed);
            AddStat("NUMSHV", Stats.SafeHouseVisits);
            AddStat("FEST_RP", $"{Stats.NumberKillFrenziesPassed} out of {Collectibles.NumRampages}");
            AddStat("PERPIC", $"{GetNumPackagesFound()} out of {Collectibles.NumPackages}");
            AddStat("PE_WAST", Stats.PeopleKilledByPlayer);
            AddStat("PE_WSOT", Stats.PeopleKilledByOthers);
            AddStat("CAR_EXP", Stats.CarsExploded);
            AddStat("BOA_EXP", Stats.BoatsExploded);
            AddStat("HEL_DST", Stats.HelisDestroyed);
            AddStat("TYREPOP", Stats.TyresPopped);
            AddStat("ST_STAR", Stats.WantedStarsAttained);
            AddStat("ST_STGN", Stats.WantedStarsEvaded);
            AddStat("TM_BUST", Stats.TimesArrested);
            AddStat("TM_DED", Stats.TimesDied);
            AddStat("ST_HEAD", Stats.HeadsPopped);
            AddStat("DAYPLC", 0);       // not saved
            int gangMembersWasted = GetGangMembersWasted();
            if (gangMembersWasted > 0) AddStat("ST_GANG", GetLeastFavoriteGangName());
            AddStat("GNG_WST", gangMembersWasted);
            AddStat("DED_CRI", GetCriminalsWasted());
            AddStat("HTMKLD", Stats.HitmenKilled);
            AddStat("KGS_EXP", Stats.KgsOfExplosivesUsed);
            AddStat("BUL_FIR", Stats.RoundsFiredByPlayer);
            AddStat("BUL_HIT", Stats.BulletsThatHit);
            AddStat("ACCURA", (float) Stats.BulletsThatHit / Stats.RoundsFiredByPlayer, percentage: true);
            AddStat("CAR_CRU", Stats.CarsCrushed);
            if (SimpleVars.Language == 0)
            {
                AddStat("FEST_DF", MetersToMiles(Stats.DistanceTravelledOnFoot));
                AddStat("FEST_DC", MetersToMiles(Stats.DistanceTravelledByCar));
                AddStat("DISTBIK", MetersToMiles(Stats.DistanceTravelledByBike));
                AddStat("DISTBOA", MetersToMiles(Stats.DistanceTravelledByBoat));
                AddStat("TOT_DIS", MetersToMiles(GetTotalDistanceTravelled()));
            }
            else
            {
                AddStat("FESTDFM", Stats.DistanceTravelledOnFoot);
                AddStat("FESTDCM", Stats.DistanceTravelledByCar);
                AddStat("DISTBIM", Stats.DistanceTravelledByBike);
                AddStat("DISTBOM", Stats.DistanceTravelledByBoat);
                AddStat("TOTDISM", GetTotalDistanceTravelled());
            }
            AddStat("MXCARDM", Stats.MaximumJumpDistance);
            AddStat("MXCARJM", Stats.MaximumJumpHeight);
            AddStat("MXFLIP", Stats.MaximumJumpFlips);
            AddStat("MXJUMP", $"{Stats.MaximumJumpSpins}\u00B0");
            AddStat("BSTSTU");
            AddStat(null, GetBestInsaneStuntName());
            AddStat("NOUNIF", $"{GetNumStuntJumpsFound()} out of {Collectibles.NumStuntJumps}");
            AddStat("ST_WHEE", Stats.LongestWheelie);
            AddStat("ST_WHED", Stats.LongestWheelieDist);
            AddStat("ST_STOP", Stats.LongestStoppie);
            AddStat("ST_STOD", Stats.LongestStoppieDist);
            AddStat("ST_FAPD", Stats.LongestFacePlantDist);
            AddStat("ST_2WHE", Stats.Longest2Wheel);
            AddStat("ST_2WHD", Stats.Longest2WheelDist);
            if (Stats.LoanSharks > 0) AddStat("ST_LOAN", Stats.LoanSharks);     // beta vc leftover
            if (Stats.CriminalsCaught > 0) AddStat("FEST_CC", Stats.CriminalsCaught);
            if (Stats.HighestLevelVigilanteMission > 0) AddStat("FEST_HV", Stats.HighestLevelVigilanteMission);
            if (Stats.PassengersDroppedOffWithTaxi > 0) AddStat("PASDRO", Stats.PassengersDroppedOffWithTaxi);
            if (Stats.MoneyMadeWithTaxi > 0) AddStat("MONTAX", Stats.MoneyMadeWithTaxi, money: true);
            if (Stats.LivesSavedWithAmbulance > 0) AddStat("FEST_LS", Stats.LivesSavedWithAmbulance);
            if (Stats.HighestLevelAmbulanceMission > 0) AddStat("FEST_HA", Stats.HighestLevelAmbulanceMission);
            if (Stats.FiresExtinguished > 0) AddStat("FEST_FE", Stats.FiresExtinguished);
            if (Stats.HighestLevelFireMission > 0) AddStat("FIRELVL", Stats.HighestLevelFireMission);
            if (Stats.CarsSold > 0) AddStat("CARSLD", Stats.CarsSold);
            if (Stats.MoneyMadeWithCarSales > 0) AddStat("MONCAR", Stats.MoneyMadeWithCarSales, money: true);
            if (Stats.BikesSold > 0) AddStat("BIKESLD", Stats.BikesSold);
            if (Stats.MoneyMadeWithBikeSales > 0) AddStat("MONBIKE", Stats.MoneyMadeWithBikeSales, money: true);
            if (Stats.NumberOfExportedCars > 0) AddStat("CARSEXP", $"{Stats.NumberOfExportedCars} out of {Stats.TotalNumberOfCarExport}");
            if (Stats.HighestLevelSlashTv > 0) AddStat("SLTVLVL", Stats.HighestLevelSlashTv);
            if (Stats.MoneyMadeWithSlashTv > 0) AddStat("SLTVMON", Stats.MoneyMadeWithSlashTv, money: true);
            if (Stats.TotalKillsOnSlashTv > 0) AddStat("SLTVKIL", Stats.TotalKillsOnSlashTv);
            if (Stats.CashMadeCollectingTrash > 0) AddStat("MONTRS", Stats.CashMadeCollectingTrash, money: true);
            if (Stats.GuardianAngelMissionsPassed > 0) AddStat("GDAMSP", Stats.GuardianAngelMissionsPassed);
            if (Stats.HighestGuardianAngelJusticeDished > 0) AddStat("HGAJD", Stats.HighestGuardianAngelJusticeDished);
            if (Stats.GuardianAngelHighestLevelInd > 0 ||
                Stats.GuardianAngelHighestLevelCom > 0 ||
                Stats.GuardianAngelHighestLevelSub > 0) AddStat("GDA_HL");
            if (Stats.GuardianAngelHighestLevelInd > 0) AddStat("IND_ZON", $"\t{Stats.GuardianAngelHighestLevelInd}", indentLevel: 1);
            if (Stats.GuardianAngelHighestLevelCom > 0) AddStat("COM_ZON", $"\t{Stats.GuardianAngelHighestLevelCom}", indentLevel: 1);
            if (Stats.GuardianAngelHighestLevelSub > 0) AddStat("SUB_ZON", $"\t{Stats.GuardianAngelHighestLevelSub}", indentLevel: 1);
            if (Stats.MostTimeLeftTrainRace > 0) AddStat("FSTTRT", TimeSpan.FromSeconds(Stats.MostTimeLeftTrainRace));
            if (Stats.BestHeliRacePosition != int.MaxValue) AddStat("BHLPOS", Stats.BestHeliRacePosition);
            AddStat("TMSOUT", Stats.NumberOutfitChanges);
            if (Stats.BestBanditPositions[0] != 0x7FFFFFFF ||
                Stats.BestBanditPositions[1] != 0x7FFFFFFF ||
                Stats.BestBanditPositions[2] != 0x7FFFFFFF) AddStat("BNDRACE");
            if (Stats.BestBanditPositions[0] != 0x7FFFFFFF) AddStat("BNDRAC0", indentLevel: 1);
            if (Stats.BestBanditPositions[0] != 0x7FFFFFFF) AddStat("DBIKEBP", Stats.BestBanditPositions[0], indentLevel: 2);
            if (Stats.BestBanditPositions[0] != 0x7FFFFFFF) AddStat("DBIKEFL", TimeSpan.FromSeconds(Stats.BestBanditLapTimes[0]), indentLevel: 2);
            if (Stats.BestBanditPositions[1] != 0x7FFFFFFF) AddStat("BNDRAC1", indentLevel: 1);
            if (Stats.BestBanditPositions[1] != 0x7FFFFFFF) AddStat("DBIKEBP", Stats.BestBanditPositions[1], indentLevel: 2);
            if (Stats.BestBanditPositions[1] != 0x7FFFFFFF) AddStat("DBIKEFL", TimeSpan.FromSeconds(Stats.BestBanditLapTimes[1]), indentLevel: 2);
            if (Stats.BestBanditPositions[2] != 0x7FFFFFFF) AddStat("BNDRAC2", indentLevel: 1);
            if (Stats.BestBanditPositions[2] != 0x7FFFFFFF) AddStat("DBIKEBP", Stats.BestBanditPositions[2], indentLevel: 2);
            if (Stats.BestBanditPositions[2] != 0x7FFFFFFF) AddStat("DBIKEFL", TimeSpan.FromSeconds(Stats.BestBanditLapTimes[2]), indentLevel: 2);
            if (Stats.BestStreetRacePositions[0] != 0x7FFFFFFF) AddStat("BSRPS0");
            if (Stats.BestStreetRacePositions[0] != 0x7FFFFFFF) AddStat("DBIKEBP", Stats.BestStreetRacePositions[0], indentLevel: 1);
            if (Stats.BestStreetRacePositions[0] != 0x7FFFFFFF) AddStat("DBIKEFL", TimeSpan.FromSeconds(Stats.FastestStreetRaceLapTimes[0]), indentLevel: 1);
            if (Stats.BestStreetRacePositions[0] != 0x7FFFFFFF) AddStat("DBIKEFT", TimeSpan.FromSeconds(Stats.FastestStreetRaceTimes[0]), indentLevel: 1);
            if (Stats.BestStreetRacePositions[1] != 0x7FFFFFFF) AddStat("BSRPS1");
            if (Stats.BestStreetRacePositions[1] != 0x7FFFFFFF) AddStat("DBIKEBP", Stats.BestStreetRacePositions[1], indentLevel: 1);
            if (Stats.BestStreetRacePositions[1] != 0x7FFFFFFF) AddStat("DBIKEFL", TimeSpan.FromSeconds(Stats.FastestStreetRaceLapTimes[1]), indentLevel: 1);
            if (Stats.BestStreetRacePositions[1] != 0x7FFFFFFF) AddStat("DBIKEFT", TimeSpan.FromSeconds(Stats.FastestStreetRaceTimes[1]), indentLevel: 1);
            if (Stats.BestStreetRacePositions[2] != 0x7FFFFFFF) AddStat("BSRPS2");
            if (Stats.BestStreetRacePositions[2] != 0x7FFFFFFF) AddStat("DBIKEBP", Stats.BestStreetRacePositions[2], indentLevel: 1);
            if (Stats.BestStreetRacePositions[2] != 0x7FFFFFFF) AddStat("DBIKEFL", TimeSpan.FromSeconds(Stats.FastestStreetRaceLapTimes[2]), indentLevel: 1);
            if (Stats.BestStreetRacePositions[2] != 0x7FFFFFFF) AddStat("DBIKEFT", TimeSpan.FromSeconds(Stats.FastestStreetRaceTimes[2]), indentLevel: 1);
            if (Stats.BestStreetRacePositions[3] != 0x7FFFFFFF) AddStat("BSRPS3");
            if (Stats.BestStreetRacePositions[3] != 0x7FFFFFFF) AddStat("DBIKEBP", Stats.BestStreetRacePositions[3], indentLevel: 1);
            if (Stats.BestStreetRacePositions[3] != 0x7FFFFFFF) AddStat("DBIKEFL", TimeSpan.FromSeconds(Stats.FastestStreetRaceLapTimes[3]), indentLevel: 1);
            if (Stats.BestStreetRacePositions[3] != 0x7FFFFFFF) AddStat("DBIKEFT", TimeSpan.FromSeconds(Stats.FastestStreetRaceTimes[3]), indentLevel: 1);
            if (Stats.BestStreetRacePositions[4] != 0x7FFFFFFF) AddStat("BSRPS4");
            if (Stats.BestStreetRacePositions[4] != 0x7FFFFFFF) AddStat("DBIKEBP", Stats.BestStreetRacePositions[4], indentLevel: 1);
            if (Stats.BestStreetRacePositions[4] != 0x7FFFFFFF) AddStat("DBIKEFL", TimeSpan.FromSeconds(Stats.FastestStreetRaceLapTimes[4]), indentLevel: 1);
            if (Stats.BestStreetRacePositions[4] != 0x7FFFFFFF) AddStat("DBIKEFT", TimeSpan.FromSeconds(Stats.FastestStreetRaceTimes[4]), indentLevel: 1);
            if (Stats.BestStreetRacePositions[5] != 0x7FFFFFFF) AddStat("BSRPS5");
            if (Stats.BestStreetRacePositions[5] != 0x7FFFFFFF) AddStat("DBIKEBP", Stats.BestStreetRacePositions[5], indentLevel: 1);
            if (Stats.BestStreetRacePositions[5] != 0x7FFFFFFF) AddStat("DBIKEFL", TimeSpan.FromSeconds(Stats.FastestStreetRaceLapTimes[5]), indentLevel: 1);
            if (Stats.BestStreetRacePositions[5] != 0x7FFFFFFF) AddStat("DBIKEFT", TimeSpan.FromSeconds(Stats.FastestStreetRaceTimes[5]), indentLevel: 1);
            if (Stats.FastestDirtBikeLapTimes[0] > 0 ||
                Stats.FastestDirtBikeLapTimes[1] > 0 ||
                Stats.FastestDirtBikeLapTimes[2] > 0 ||
                Stats.FastestDirtBikeLapTimes[3] > 0 ||
                Stats.FastestDirtBikeLapTimes[4] > 0 ||
                Stats.FastestDirtBikeLapTimes[5] > 0 ||
                Stats.FastestDirtBikeLapTimes[6] > 0 ||
                Stats.FastestDirtBikeLapTimes[7] > 0 ||
                Stats.FastestDirtBikeLapTimes[8] > 0 ||
                Stats.FastestDirtBikeLapTimes[9] > 0) AddStat("DBIKEHE");
            if (Stats.FastestDirtBikeLapTimes[0] > 0) AddStat("DBIKE0", indentLevel: 1);
            if (Stats.FastestDirtBikeLapTimes[0] > 0) AddStat("DBIKEFL", TimeSpan.FromSeconds(Stats.FastestDirtBikeLapTimes[0]), indentLevel: 2);
            if (Stats.FastestDirtBikeLapTimes[0] > 0) AddStat("DBIKEFT", TimeSpan.FromSeconds(Stats.FastestDirtBikeTimes[0]), indentLevel: 2);
            if (Stats.FastestDirtBikeLapTimes[1] > 0) AddStat("DBIKE1", indentLevel: 1);
            if (Stats.FastestDirtBikeLapTimes[1] > 0) AddStat("DBIKEFL", TimeSpan.FromSeconds(Stats.FastestDirtBikeLapTimes[1]), indentLevel: 2);
            if (Stats.FastestDirtBikeLapTimes[1] > 0) AddStat("DBIKEFT", TimeSpan.FromSeconds(Stats.FastestDirtBikeTimes[1]), indentLevel: 2);
            if (Stats.FastestDirtBikeLapTimes[2] > 0) AddStat("DBIKE2", indentLevel: 1);
            if (Stats.FastestDirtBikeLapTimes[2] > 0) AddStat("DBIKEFL", TimeSpan.FromSeconds(Stats.FastestDirtBikeLapTimes[2]), indentLevel: 2);
            if (Stats.FastestDirtBikeLapTimes[2] > 0) AddStat("DBIKEFT", TimeSpan.FromSeconds(Stats.FastestDirtBikeTimes[2]), indentLevel: 2);
            if (Stats.FastestDirtBikeLapTimes[3] > 0) AddStat("DBIKE3", indentLevel: 1);
            if (Stats.FastestDirtBikeLapTimes[3] > 0) AddStat("DBIKEFL", TimeSpan.FromSeconds(Stats.FastestDirtBikeLapTimes[3]), indentLevel: 2);
            if (Stats.FastestDirtBikeLapTimes[3] > 0) AddStat("DBIKEFT", TimeSpan.FromSeconds(Stats.FastestDirtBikeTimes[3]), indentLevel: 2);
            if (Stats.FastestDirtBikeLapTimes[4] > 0) AddStat("DBIKE4", indentLevel: 1);
            if (Stats.FastestDirtBikeLapTimes[4] > 0) AddStat("DBIKEFL", TimeSpan.FromSeconds(Stats.FastestDirtBikeLapTimes[4]), indentLevel: 2);
            if (Stats.FastestDirtBikeLapTimes[4] > 0) AddStat("DBIKEFT", TimeSpan.FromSeconds(Stats.FastestDirtBikeTimes[4]), indentLevel: 2);
            if (Stats.FastestDirtBikeLapTimes[5] > 0) AddStat("DBIKE5", indentLevel: 1);
            if (Stats.FastestDirtBikeLapTimes[5] > 0) AddStat("DBIKEFL", TimeSpan.FromSeconds(Stats.FastestDirtBikeLapTimes[5]), indentLevel: 2);
            if (Stats.FastestDirtBikeLapTimes[5] > 0) AddStat("DBIKEFT", TimeSpan.FromSeconds(Stats.FastestDirtBikeTimes[5]), indentLevel: 2);
            if (Stats.FastestDirtBikeLapTimes[6] > 0) AddStat("DBIKE6", indentLevel: 1);
            if (Stats.FastestDirtBikeLapTimes[6] > 0) AddStat("DBIKEFL", TimeSpan.FromSeconds(Stats.FastestDirtBikeLapTimes[6]), indentLevel: 2);
            if (Stats.FastestDirtBikeLapTimes[6] > 0) AddStat("DBIKEFT", TimeSpan.FromSeconds(Stats.FastestDirtBikeTimes[6]), indentLevel: 2);
            if (Stats.FastestDirtBikeLapTimes[7] > 0) AddStat("DBIKE7", indentLevel: 1);
            if (Stats.FastestDirtBikeLapTimes[7] > 0) AddStat("DBIKEFL", TimeSpan.FromSeconds(Stats.FastestDirtBikeLapTimes[7]), indentLevel: 2);
            if (Stats.FastestDirtBikeLapTimes[7] > 0) AddStat("DBIKEFT", TimeSpan.FromSeconds(Stats.FastestDirtBikeTimes[7]), indentLevel: 2);
            if (Stats.FastestDirtBikeLapTimes[8] > 0) AddStat("DBIKE8", indentLevel: 1);
            if (Stats.FastestDirtBikeLapTimes[8] > 0) AddStat("DBIKEFL", TimeSpan.FromSeconds(Stats.FastestDirtBikeLapTimes[8]), indentLevel: 2);
            if (Stats.FastestDirtBikeLapTimes[8] > 0) AddStat("DBIKEFT", TimeSpan.FromSeconds(Stats.FastestDirtBikeTimes[8]), indentLevel: 2);
            if (Stats.FastestDirtBikeLapTimes[9] > 0) AddStat("DBIKE9", indentLevel: 1);
            if (Stats.FastestDirtBikeLapTimes[9] > 0) AddStat("DBIKEFL", TimeSpan.FromSeconds(Stats.FastestDirtBikeLapTimes[9]), indentLevel: 2);
            if (Stats.FastestDirtBikeLapTimes[9] > 0) AddStat("DBIKEFT", TimeSpan.FromSeconds(Stats.FastestDirtBikeTimes[9]), indentLevel: 2);
            if (Stats.DirtBikeMostAir > 0) AddStat("DBIKEAI", Stats.DirtBikeMostAir);
            if (Stats.BestTimeGoGoFaggio > 0) AddStat("FEST_GO", TimeSpan.FromSeconds(Stats.BestTimeGoGoFaggio));       // bugged stat
            if (Stats.MovieStunts > 0) AddStat("ST_MOVI", Stats.MovieStunts);     // beta vc leftover
            if (Stats.PhotosTaken > 0) AddStat("ST_PHOT", Stats.PhotosTaken);
            if (Stats.PizzasDelivered > 0) AddStat("ST_PIZZ", Stats.PizzasDelivered);
            if (Stats.NoodlesDelivered > 0) AddStat("ST_NOOD", Stats.NoodlesDelivered);
            if (Stats.MoneyMadeFromTourist > 0) AddStat("MO_TOUR", Stats.MoneyMadeFromTourist, money: true);
            if (Stats.TouristsTakenToSpots > 0) AddStat("TA_TOUR", Stats.TouristsTakenToSpots);
            if (Stats.GarbagePickups > 0) AddStat("ST_GARB", Stats.GarbagePickups);     // beta vc leftover
            if (Stats.IceCreamSold > 0) AddStat("ST_ICEC", Stats.IceCreamSold);     // vc leftover
            // FastestTimes, HighestScores, BestPositions, all VC leftovers
            if (Stats.TopShootingRangeScore > 0) AddStat("TOP_SHO", Stats.TopShootingRangeScore);   // vc leftover
            if (Stats.ShootingRank > 0) AddStat("TOP_SHO", Stats.ShootingRank);   // vc leftover
            if (Stats.TopScrapyardChallengeScore > 0) AddStat("SC_SCRA", Stats.TopScrapyardChallengeScore);
            if (Stats.Top9mmMayhemScore > 0) AddStat("SC_9MM", Stats.Top9mmMayhemScore);
            if (Stats.TopScooterShooterScore > 0) AddStat("SC_SCOO", Stats.TopScooterShooterScore);
            if (Stats.TopWichitaWipeoutScore > 0) AddStat("SC_WICH", Stats.TopWichitaWipeoutScore);
            if (Stats.MaxSecondsOnCarnageLeft > 0) AddStat("FEST_CN", Stats.MaxSecondsOnCarnageLeft);
            if (Stats.MaxKillsOnRcTriad > 0) AddStat("FEST_TR", Stats.MaxKillsOnRcTriad);
            if (Stats.BloodringKills > 0) AddStat("ST_BRK", Stats.BloodringKills);  // vc leftover
            if (Stats.BloodringTime > 0) AddStat("ST_BRK", Stats.BloodringTime);  // vc leftover
            AddStat("ST_DRWN", Stats.TimesDrowned);
            if (Stats.SeagullsKilled > 0) AddStat("SEAGULL", Stats.SeagullsKilled);
            AddStat("FST_MFR", GetMostFavoriteRadioStationName());
            AddStat("FST_LFR", GetLeastFavoriteRadioStationName());
            AddStat("SPRAYIN", Stats.Sprayings);
            AddStat("ST_WEAP", Stats.WeaponBudget, money: true);
            AddStat("ST_AUTO", Stats.AutoPaintingBudget, money: true);
            AddStat("ST_DAMA", Stats.PropertyDestroyed, money: true);
            //if (Stats.NumPropertyOwned > 0) AddStat("PROPOWN", Stats.NumPropertyOwned);  // vc leftover
            // property owned, VC leftover
            AddStat("CHASE", GetHighestMediaAttentionName());
            if (Stats.UnlockedCostumes != PlayerOutfitFlags.None) AddStat("OUTFITS");
            if (Stats.UnlockedCostumes.HasFlag(PlayerOutfitFlags.Casual)) AddStat("OUTF_00", indentLevel: 1);
            if (Stats.UnlockedCostumes.HasFlag(PlayerOutfitFlags.Leone)) AddStat("OUTF_01", indentLevel: 1);
            if (Stats.UnlockedCostumes.HasFlag(PlayerOutfitFlags.Overalls)) AddStat("OUTF_02", indentLevel: 1);
            if (Stats.UnlockedCostumes.HasFlag(PlayerOutfitFlags.AvengingAngels)) AddStat("OUTF_03", indentLevel: 1);
            if (Stats.UnlockedCostumes.HasFlag(PlayerOutfitFlags.Chauffer)) AddStat("OUTF_04", indentLevel: 1);
            if (Stats.UnlockedCostumes.HasFlag(PlayerOutfitFlags.Lawyer)) AddStat("OUTF_05", indentLevel: 1);
            if (Stats.UnlockedCostumes.HasFlag(PlayerOutfitFlags.Tuxedo)) AddStat("OUTF_06", indentLevel: 1);
            if (Stats.UnlockedCostumes.HasFlag(PlayerOutfitFlags.TheKing)) AddStat("OUTF_07", indentLevel: 1);
            if (Stats.UnlockedCostumes.HasFlag(PlayerOutfitFlags.Cox)) AddStat("OUTF_08", indentLevel: 1);
            if (Stats.UnlockedCostumes.HasFlag(PlayerOutfitFlags.Underwear)) AddStat("OUTF_09", indentLevel: 1);
            if (Stats.UnlockedCostumes.HasFlag(PlayerOutfitFlags.Hero)) AddStat("OUTF_10", indentLevel: 1);
            if (Stats.UnlockedCostumes.HasFlag(PlayerOutfitFlags.Dragon)) AddStat("OUTF_11", indentLevel: 1);
            if (Stats.UnlockedCostumes.HasFlag(PlayerOutfitFlags.Antonio)) AddStat("OUTF_12", indentLevel: 1);
            if (Stats.UnlockedCostumes.HasFlag(PlayerOutfitFlags.Sweats)) AddStat("OUTF_13", indentLevel: 1);
            if (Stats.UnlockedCostumes.HasFlag(PlayerOutfitFlags.Goodfella)) AddStat("OUTF_14", indentLevel: 1);
            if (Stats.UnlockedCostumes.HasFlag(PlayerOutfitFlags.Wiseguy)) AddStat("OUTF_15", indentLevel: 1);
        }

        private void UpdateCriminalRating(int rating)
        {
            CriminalRating = MakeStat("CRIMRA", $"{GetCriminalRatingString(rating)} ({rating})");
        }

        public int GetCriminalRatingNumber()
        {
            int rating = 10 * Stats.HighestLevelFireMission + Stats.FiresExtinguished
                       + 10 * Stats.HighestLevelAmbulanceMission + Stats.LivesSavedWithAmbulance
                       + Stats.CriminalsCaught
                       + 30 * Stats.HelisDestroyed
                       + Stats.TotalLegitimateKills
                       - 3 * Stats.TimesArrested
                       - 3 * Stats.TimesDied
                       + TheSave.PlayerInfo.Money / 5000;

            if (SimpleVars.HasPlayerCheated || Stats.CheatedCount > 0)
            {
                rating -= Stats.CheatedCount;
                if (rating <= -10000)
                {
                    rating = -10000;
                }
            }
            else if (rating <= 0)
            {
                rating = 0;
            }

            if (Stats.RoundsFiredByPlayer > 100)
            {
                rating += (int) ((float) Stats.BulletsThatHit / Stats.RoundsFiredByPlayer * 500.0f);
            }
            if (Stats.TotalProgressInGame > 0)
            {
                rating += (int) ((float) Stats.ProgressMade / Stats.TotalProgressInGame * 1000.0f);
            }

            return rating;
        }

        public string GetCriminalRatingString(int rating)
        {
            if (rating < 0)
            {
                if (rating > -500) return Gxt.TheText["MAIN"]["RATNG53"];
                if (rating > -2000) return Gxt.TheText["MAIN"]["RATNG54"];
                if (rating > -4000) return Gxt.TheText["MAIN"]["RATNG55"];
                if (rating > -6000) return Gxt.TheText["MAIN"]["RATNG56"];
                return Gxt.TheText["MAIN"]["RATNG57"];
            }
            if (rating < 20) return Gxt.TheText["MAIN"]["RATNG1"];
            if (rating < 50) return Gxt.TheText["MAIN"]["RATNG2"];
            if (rating < 75) return Gxt.TheText["MAIN"]["RATNG3"];
            if (rating < 100) return Gxt.TheText["MAIN"]["RATNG4"];
            if (rating < 120) return Gxt.TheText["MAIN"]["RATNG5"];
            if (rating < 150) return Gxt.TheText["MAIN"]["RATNG6"];
            if (rating < 200) return Gxt.TheText["MAIN"]["RATNG7"];
            if (rating < 240) return Gxt.TheText["MAIN"]["RATNG8"];
            if (rating < 270) return Gxt.TheText["MAIN"]["RATNG9"];
            if (rating < 300) return Gxt.TheText["MAIN"]["RATNG10"];
            if (rating < 335) return Gxt.TheText["MAIN"]["RATNG11"];
            if (rating < 370) return Gxt.TheText["MAIN"]["RATNG12"];
            if (rating < 400) return Gxt.TheText["MAIN"]["RATNG13"];
            if (rating < 450) return Gxt.TheText["MAIN"]["RATNG14"];
            if (rating < 500) return Gxt.TheText["MAIN"]["RATNG15"];
            if (rating < 550) return Gxt.TheText["MAIN"]["RATNG16"];
            if (rating < 600) return Gxt.TheText["MAIN"]["RATNG17"];
            if (rating < 610) return Gxt.TheText["MAIN"]["RATNG18"];
            if (rating < 650) return Gxt.TheText["MAIN"]["RATNG19"];
            if (rating < 700) return Gxt.TheText["MAIN"]["RATNG20"];
            if (rating < 850) return Gxt.TheText["MAIN"]["RATNG21"];
            if (rating < 1000) return Gxt.TheText["MAIN"]["RATNG22"];
            if (rating < 1005) return Gxt.TheText["MAIN"]["RATNG23"];
            if (rating < 1150) return Gxt.TheText["MAIN"]["RATNG24"];
            if (rating < 1300) return Gxt.TheText["MAIN"][Stats.TimesArrested > 0 ? "RATNG25" : "RATNG24"];
            if (rating < 1500) return Gxt.TheText["MAIN"]["RATNG26"];
            if (rating < 1700) return Gxt.TheText["MAIN"]["RATNG27"];
            if (rating < 2000) return Gxt.TheText["MAIN"]["RATNG28"];
            if (rating < 2100) return Gxt.TheText["MAIN"]["RATNG29"];
            if (rating < 2300) return Gxt.TheText["MAIN"]["RATNG30"];
            if (rating < 2500) return Gxt.TheText["MAIN"]["RATNG31"];
            if (rating < 2750) return Gxt.TheText["MAIN"]["RATNG32"];
            if (rating < 3000) return Gxt.TheText["MAIN"]["RATNG33"];
            if (rating < 3500) return Gxt.TheText["MAIN"]["RATNG34"];
            if (rating < 4000) return Gxt.TheText["MAIN"]["RATNG35"];
            if (rating < 5000) return Gxt.TheText["MAIN"]["RATNG36"];
            if (rating < 7500) return Gxt.TheText["MAIN"]["RATNG37"];
            if (rating < 10000) return Gxt.TheText["MAIN"]["RATNG38"];
            if (rating < 20000) return Gxt.TheText["MAIN"]["RATNG39"];
            if (rating < 30000) return Gxt.TheText["MAIN"]["RATNG40"];
            if (rating < 40000) return Gxt.TheText["MAIN"]["RATNG41"];
            if (rating < 50000) return Gxt.TheText["MAIN"]["RATNG42"];
            if (rating < 65000) return Gxt.TheText["MAIN"]["RATNG43"];
            if (rating < 80000) return Gxt.TheText["MAIN"]["RATNG44"];
            if (rating < 100000) return Gxt.TheText["MAIN"]["RATNG45"];
            if (rating < 150000) return Gxt.TheText["MAIN"]["RATNG46"];
            if (rating < 200000) return Gxt.TheText["MAIN"]["RATNG47"];
            if (rating < 300000) return Gxt.TheText["MAIN"]["RATNG48"];
            if (rating < 375000) return Gxt.TheText["MAIN"]["RATNG49"];
            if (rating < 500000) return Gxt.TheText["MAIN"]["RATNG50"];
            if (rating < 1000000) return Gxt.TheText["MAIN"]["RATNG51"];
            return Gxt.TheText["MAIN"][TheSave.PlayerInfo.MoneyOnScreen > 10000000 ? "RATNG52" : "RATNG51"];
        }

        public int GetNumPackagesFound()
        {
            int packagesFound = 0;
            for (int i = 0; i < Collectibles.NumPackages; i++)
            {
                if (TheEditor.GetGlobal(GlobalVariable.Package1Collected + i) != 0) packagesFound++;
            }

            return packagesFound;
        }

        public int GetNumStuntJumpsFound()
        {
            int stuntJumpsFound = 0;
            for (int i = 0; i < Collectibles.NumStuntJumps; i++)
            {
                if (TheEditor.GetGlobal(GlobalVariable.Package1Collected + i) != 0) stuntJumpsFound++;
            }

            return stuntJumpsFound;
        }

        public int GetCriminalsWasted()
        {
            return Stats.PedsKilledOfThisType[(int) PedType.CRIMINAL];
        }

        public int GetGangMembersWasted()
        {
            return Stats.PedsKilledOfThisType[(int) PedType.GANG1]
                 + Stats.PedsKilledOfThisType[(int) PedType.GANG2]
                 + Stats.PedsKilledOfThisType[(int) PedType.GANG3]
                 + Stats.PedsKilledOfThisType[(int) PedType.GANG4]
                 + Stats.PedsKilledOfThisType[(int) PedType.GANG5]
                 + Stats.PedsKilledOfThisType[(int) PedType.GANG6]
                 + Stats.PedsKilledOfThisType[(int) PedType.GANG7]
                 + Stats.PedsKilledOfThisType[(int) PedType.GANG8]
                 + Stats.PedsKilledOfThisType[(int) PedType.GANG9];
        }

        public string GetLeastFavoriteGangName()
        {
            int maxKills = int.MinValue;
            PedType leastFavGang = PedType.PLAYER1;

            for (PedType g = PedType.GANG1; g <= PedType.GANG9; g = (PedType) ((int) g + 1))
            {
                int kills = Stats.PedsKilledOfThisType[(int) g];
                if (kills > maxKills)
                {
                    maxKills = kills;
                    leastFavGang = g;
                }
            }

            return leastFavGang switch
            {
                PedType.GANG1 => Gxt.TheText["MAIN"]["ST_GNG1"],
                PedType.GANG2 => Gxt.TheText["MAIN"]["ST_GNG2"],
                PedType.GANG3 => Gxt.TheText["MAIN"]["ST_GNG3"],
                PedType.GANG4 => Gxt.TheText["MAIN"]["ST_GNG4"],
                PedType.GANG5 => Gxt.TheText["MAIN"]["ST_GNG5"],
                PedType.GANG6 => Gxt.TheText["MAIN"]["ST_GNG6"],
                PedType.GANG7 => Gxt.TheText["MAIN"]["ST_GNG7"],
                PedType.GANG8 => Gxt.TheText["MAIN"]["ST_GNG8"],
                PedType.GANG9 => Gxt.TheText["MAIN"]["ST_GNG9"],
                _ => null,
            };
        }

        public float GetTotalDistanceTravelled()
        {
            return Stats.DistanceTravelledOnFoot
                 + Stats.DistanceTravelledByCar
                 + Stats.DistanceTravelledByBike
                 + Stats.DistanceTravelledByBoat;
        }

        public float MetersToMiles(float m)
        {
            const float MilesPerMeter = 0.00059880241f; // game uses the wrong conversion factor lol

            return m * MilesPerMeter;
        }

        public string GetBestInsaneStuntName()
        {
            return Stats.BestStuntJump switch
            {
                1 => Gxt.TheText["MAIN"]["INSTUN"],
                2 => Gxt.TheText["MAIN"]["PRINST"],
                3 => Gxt.TheText["MAIN"]["DBINST"],
                4 => Gxt.TheText["MAIN"]["DBPINS"],
                5 => Gxt.TheText["MAIN"]["TRINST"],
                6 => Gxt.TheText["MAIN"]["PRTRST"],
                7 => Gxt.TheText["MAIN"]["QUINST"],
                8 => Gxt.TheText["MAIN"]["PQUINS"],
                _ => Gxt.TheText["MAIN"]["NOSTUC"],
            };
        }

        public string GetMostFavoriteRadioStationName()
        {
            int radio = -1;
            float maxListenTime = float.MinValue;

            for (int i = 0; i < Stats.FavoriteRadioStationList.Count; i++)
            {
                float listenTime = Stats.FavoriteRadioStationList[i];
                if (listenTime > maxListenTime)
                {
                    maxListenTime = listenTime;
                    radio = i;
                }
            }

            return GetRadioStationName(radio);
        }

        public string GetLeastFavoriteRadioStationName()
        {
            int radio = -1;
            float minListenTime = float.MaxValue;

            for (int i = 0; i < Stats.FavoriteRadioStationList.Count; i++)
            {
                float listenTime = Stats.FavoriteRadioStationList[i];
                if (listenTime < minListenTime)
                {
                    minListenTime = listenTime;
                    radio = i;
                }
            }

            return GetRadioStationName(radio);
        }

        public string GetRadioStationName(int radio)
        {
            return radio switch
            {
                0 => Gxt.TheText["MAIN"]["FEA_FM0"],
                1 => Gxt.TheText["MAIN"]["FEA_FM1"],
                2 => Gxt.TheText["MAIN"]["FEA_FM2"],
                3 => Gxt.TheText["MAIN"]["FEA_FM3"],
                4 => Gxt.TheText["MAIN"]["FEA_FM4"],
                5 => Gxt.TheText["MAIN"]["FEA_FM5"],
                6 => Gxt.TheText["MAIN"]["FEA_FM6"],
                7 => Gxt.TheText["MAIN"]["FEA_FM7"],
                8 => Gxt.TheText["MAIN"]["FEA_FM8"],
                9 => Gxt.TheText["MAIN"]["FEA_FM9"],
                10 => Gxt.TheText["MAIN"]["FEA_MP3"],
                _ => null,
            };
        }

        public string GetHighestMediaAttentionName()
        {
            float chase = Stats.HighestChaseValue;
            if (chase < 20) return Gxt.TheText["MAIN"]["CHASE1"];
            if (chase >= 20 && chase < 50) return Gxt.TheText["MAIN"]["CHASE2"];
            if (chase >= 50 && chase < 75) return Gxt.TheText["MAIN"]["CHASE3"];
            if (chase >= 75 && chase < 100) return Gxt.TheText["MAIN"]["CHASE4"];
            if (chase >= 100 && chase < 150) return Gxt.TheText["MAIN"]["CHASE5"];
            if (chase >= 150 && chase < 200) return Gxt.TheText["MAIN"]["CHASE6"];
            if (chase >= 200 && chase < 250) return Gxt.TheText["MAIN"]["CHASE7"];
            if (chase >= 250 && chase < 300) return Gxt.TheText["MAIN"]["CHASE8"];
            if (chase >= 300 && chase < 350) return Gxt.TheText["MAIN"]["CHASE9"];
            if (chase >= 350 && chase < 400) return Gxt.TheText["MAIN"]["CHASE10"];
            if (chase >= 400 && chase < 500) return Gxt.TheText["MAIN"]["CHASE11"];
            if (chase >= 500 && chase < 600) return Gxt.TheText["MAIN"]["CHASE12"];
            if (chase >= 600 && chase < 700) return Gxt.TheText["MAIN"]["CHASE13"];
            if (chase >= 700 && chase < 800) return Gxt.TheText["MAIN"]["CHASE14"];
            if (chase >= 800 && chase < 900) return Gxt.TheText["MAIN"]["CHASE15"];
            if (chase >= 900 && chase < 1000) return Gxt.TheText["MAIN"]["CHASE16"];
            if (chase >= 1000 && chase < 1200) return Gxt.TheText["MAIN"]["CHASE17"];
            if (chase >= 1200 && chase < 1400) return Gxt.TheText["MAIN"]["CHASE18"];
            if (chase >= 1400 && chase < 1600) return Gxt.TheText["MAIN"]["CHASE19"];
            if (chase >= 1600 && chase < 1800) return Gxt.TheText["MAIN"]["CHASE20"];
            return Gxt.TheText["MAIN"]["CHASE21"];
        }

        private void AddStat(string name, TimeSpan value,
            int indentLevel = 0)
        {
            string time = "";
            if ((int) value.TotalHours > 0) time += $"{(int) value.TotalHours}:";
            time += $"{value.Minutes:D2}";
            time += $":{value.Seconds:D2}";

            AddStat(name, time, indentLevel);
        }

        private void AddStat(string name, float value,
            int indentLevel = 0, bool percentage = false, bool money = false)
        {
            if (float.IsNaN(value))
            {
                value = 0;
            }

            if (percentage)
            {
                AddStat(name, $"{value:P2}", indentLevel);
            }
            else if (money)
            {
                AddStat(name, $"${value:F2}", indentLevel);
            }
            else
            {
                AddStat(name, $"{value:F2}", indentLevel);
            }
        }

        private void AddStat(string name, int value,
            int indentLevel = 0)
        {
            AddStat(name, $"{value}", indentLevel);
        }

        private void AddStat(string name, string value = "",
            int indentLevel = 0, bool gxt = true)
        {
            Statistics.Add(MakeStat(name, value, indentLevel, gxt));
        }

        private Stat MakeStat(string name, string value,
            int indentLevel = 0, bool gxt = true)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = "";
            }
            else if (gxt)
            {
                Gxt.TheText.TryGetValue("MAIN", name, out name);
                if (name.StartsWith('~'))
                {
                    // Filter-out GXT color token (e.g '~w~')
                    int i = 1;
                    while (name[i++] != '~') { }

                    Debug.Assert(i < name.Length);
                    name = name.Substring(i);
                }
            }

            return new Stat() { Name = name, Value = value, IndentLevel = indentLevel };
        }

        public void RegisterChangeHandlers()
        {
            if (!m_handlersRegistered && TheSave != null)
            {
                SimpleVars.PropertyChanged += Data_PropertyChanged;
                PlayerInfo.PropertyChanged += Data_PropertyChanged;
                Stats.PropertyChanged += Data_PropertyChanged;
                Stats.PedsKilledOfThisType.CollectionChanged += Data_CollectionChanged;
                Stats.BestBanditLapTimes.CollectionChanged += Data_CollectionChanged;
                Stats.BestBanditPositions.CollectionChanged += Data_CollectionChanged;
                Stats.BestStreetRacePositions.CollectionChanged += Data_CollectionChanged;
                Stats.FastestStreetRaceLapTimes.CollectionChanged += Data_CollectionChanged;
                Stats.FastestStreetRaceTimes.CollectionChanged += Data_CollectionChanged;
                Stats.FastestDirtBikeLapTimes.CollectionChanged += Data_CollectionChanged;
                Stats.FastestDirtBikeTimes.CollectionChanged += Data_CollectionChanged;
                Stats.FavoriteRadioStationList.CollectionChanged += Data_CollectionChanged;
                m_handlersRegistered = true;
            }
        }

        public void UnregisterChangeHandlers()
        {
            if (m_handlersRegistered && TheSave != null)
            {
                SimpleVars.PropertyChanged -= Data_PropertyChanged;
                PlayerInfo.PropertyChanged -= Data_PropertyChanged;
                Stats.PropertyChanged -= Data_PropertyChanged;
                Stats.PedsKilledOfThisType.CollectionChanged -= Data_CollectionChanged;
                Stats.BestBanditLapTimes.CollectionChanged -= Data_CollectionChanged;
                Stats.BestBanditPositions.CollectionChanged -= Data_CollectionChanged;
                Stats.BestStreetRacePositions.CollectionChanged -= Data_CollectionChanged;
                Stats.FastestStreetRaceLapTimes.CollectionChanged -= Data_CollectionChanged;
                Stats.FastestStreetRaceTimes.CollectionChanged -= Data_CollectionChanged;
                Stats.FastestDirtBikeLapTimes.CollectionChanged -= Data_CollectionChanged;
                Stats.FastestDirtBikeTimes.CollectionChanged -= Data_CollectionChanged;
                Stats.FavoriteRadioStationList.CollectionChanged -= Data_CollectionChanged;
                m_handlersRegistered = false;
            }
        }

        private void TheEditor_FileOpened(object sender, EventArgs e)
        {
            RegisterChangeHandlers();
            RefreshStats();
        }

        private void TheEditor_FileClosing(object sender, EventArgs e)
        {
            UnregisterChangeHandlers();
            UpdateCriminalRating(0);
            Statistics.Clear();
        }
    }

    public class Stat : ObservableObject
    {
        private string m_name;
        private string m_value;
        private int m_indentLevel;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; OnPropertyChanged(); }
        }
        public string Value
        {
            get { return m_value; }
            set { m_value = value; OnPropertyChanged(); }
        }

        public int IndentLevel
        {
            get { return m_indentLevel; }
            set { m_indentLevel = value; OnPropertyChanged(); }
        }
    }
}
