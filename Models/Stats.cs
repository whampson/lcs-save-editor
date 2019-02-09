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

using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using LcsSaveEditor.DataTypes;
using LcsSaveEditor.Extensions;
using LcsSaveEditor.Infrastructure;

namespace LcsSaveEditor.Models
{
    public abstract class Stats : SerializableObject
    {
        protected uint m_peopleKilledByPlayer;
        protected uint m_peopleKilledByOthers;
        protected uint m_carsExploded;
        protected uint m_boatsExploded;
        protected uint m_tyresPopped;
        protected uint m_roundsFiredByPlayer;
        protected uint m_pedType00Wasted;
        protected uint m_pedType01Wasted;
        protected uint m_pedType02Wasted;
        protected uint m_pedType03Wasted;
        protected uint m_pedType04Wasted;
        protected uint m_pedType05Wasted;
        protected uint m_pedType06Wasted;
        protected uint m_pedType07Wasted;
        protected uint m_pedType08Wasted;
        protected uint m_pedType09Wasted;
        protected uint m_pedType10Wasted;
        protected uint m_pedType11Wasted;
        protected uint m_pedType12Wasted;
        protected uint m_pedType13Wasted;
        protected uint m_pedType14Wasted;
        protected uint m_pedType15Wasted;
        protected uint m_pedType16Wasted;
        protected uint m_pedType17Wasted;
        protected uint m_pedType18Wasted;
        protected uint m_pedType19Wasted;
        protected uint m_pedType20Wasted;
        protected uint m_pedType21Wasted;
        protected uint m_pedType22Wasted;
        protected uint m_helisDestroyed;
        protected float m_progressMade;
        protected float m_totalProgressInGame;
        protected uint m_kgsOfExplosivesUsed;
        protected uint m_bulletsThatHit;
        protected uint m_carsCrushed;
        protected uint m_headsPopped;
        protected uint m_wantedStarsAttained;
        protected uint m_wantedStarsEvaded;
        protected uint m_timesArrested;
        protected uint m_timesDied;
        protected uint m_daysPassed;
        protected uint m_safeHouseVisits;
        protected uint m_sprayings;
        protected float m_maximumJumpDistance;
        protected float m_maximumJumpHeight;
        protected uint m_maximumJumpFlips;
        protected uint m_maximumJumpSpins;
        protected InsaneStuntJump m_bestStuntJump;
        protected uint m_numberOfUniqueJumpsFound;
        protected uint m_totalNumberOfUniqueJumps;
        protected uint m_missionsGiven;
        protected uint m_passengersDroppedOffWithTaxi;
        protected uint m_moneyMadeWithTaxi;
        protected bool m_industrialPassed;
        protected bool m_commercialPassed;
        protected bool m_suburbanPassed;
        protected bool m_pamphletMissionPassed;
        protected bool _m_noMoreHurricanes;
        protected float m_distanceTravelledOnFoot;
        protected float m_distanceTravelledByCar;
        protected float m_distanceTravelledByBike;
        protected float m_distanceTravelledByBoat;
        protected float _m_distanceTravelledByPlane;
        protected uint m_livesSavedWithAmbulance;
        protected uint m_criminalsCaught;
        protected uint m_firesExtinguished;
        protected uint m_highestLevelVigilanteMission;
        protected uint m_highestLevelAmbulanceMission;
        protected uint m_highestLevelFireMission;
        protected uint m_photosTaken;
        protected uint m_numberKillFrenziesPassed;
        protected uint m_maxSecondsOnCarnageLeft;
        protected uint m_maxKillsOnRcTriad;
        protected uint m_totalNumberKillFrenzies;
        protected uint m_totalNumberMissions;
        protected uint m_timesDrowned;
        protected uint m_seagullsKilled;
        protected float m_weaponBudget;
        protected uint _m_loanSharks;
        protected uint _m_movieStunts;
        protected float m_pizzasDelivered;
        protected float m_noodlesDelivered;
        protected float m_moneyMadeFromTourist;
        protected float m_touristsTakenToSpots;
        protected uint _m_garbagePickups;
        protected uint _m_iceCreamSold;
        protected uint _m_topShootingRangeScore;
        protected uint _m_shootingRank;
        protected float m_topScrapyardChallengeScore;
        protected float m_top9mmMayhemScore;
        protected float m_topScooterShooterScore;
        protected float m_topWichitaWipeoutScore;
        protected uint m_longestWheelie;
        protected uint m_longestStoppie;
        protected uint m_longest2Wheel;
        protected float m_longestWheelieDist;
        protected float m_longestStoppieDist;
        protected float m_longest2WheelDist;
        protected float m_longestFacePlantDist;
        protected float m_autoPaintingBudget;
        protected uint m_propertyDestroyed;
        protected uint _m_numPropertyOwned;
        protected Costumes m_unlockedCostumes;
        protected uint m_bloodringKills;
        protected uint m_bloodringTime;
        protected byte[] _m_propertyOwned;
        protected float m_highestChaseValue;
        protected uint[] _m_fastestTimes;
        protected uint[] _m_highestScores;
        protected uint[] _m_bestPositions;
        protected uint m_killsSinceLastCheckpoint;
        protected uint m_totalLegitimateKills;
        protected string m_lastMissionPassedName;
        protected uint m_cheatedCount;
        protected uint m_carsSold;
        protected uint m_moneyMadeWithCarSales;
        protected uint m_bikesSold;
        protected uint m_moneyMadeWithBikeSales;
        protected uint m_numberOfExportedCars;
        protected uint m_totalNumberOfCarExport;
        protected uint m_highestLevelSlashTv;
        protected uint m_moneyMadeWithSlashTv;
        protected uint m_totalKillsOnSlashTv;
        protected uint _m_packagesSmuggled;
        protected uint _m_smugglersWasted;
        protected uint _m_fastestSmugglingTime;
        protected uint _m_moneyMadeInCoach;
        protected uint m_cashMadeCollectingTrash;
        protected uint m_hitmenKilled;
        protected uint m_highestGuarduanAngelJustuceDished;
        protected uint m_guardianAngelMissionsPassed;
        protected uint m_guardianAngelHighestLevelIndustrial;
        protected uint m_guardianAngelHighestLevelCommercial;
        protected uint m_guardianAngelHighestLevelSuburban;
        protected uint m_mostTimeLeftTrainRace;
        protected uint m_bestTimeGoGoFaggio;
        protected uint m_dirtBikeMostAir;
        protected uint _m_highestTrainCashEarned;
        protected uint _m_fastestHeliRaceTime;
        protected uint _m_bestHeliRacePosition;
        protected uint m_numberOutfitChanges;
        protected BanditRaceStat m_bestBanditLapTimes;
        protected BanditRaceStat m_bestBanditPositions;
        protected StreetRaceStat m_bestStreetRacePositions;
        protected StreetRaceStat m_fastestStreetRaceLapTimes;
        protected StreetRaceStat m_fastestStreetRaceTimes;
        protected DirtBikeRaceStat m_fastestDirtBikeLapTimes;
        protected DirtBikeRaceStat m_fastestDirtBikeTimes;
        protected FavoriteRadioStationList m_favoriteRadioStationList;

        public Stats()
        {
            _m_propertyOwned = new byte[15];
            _m_fastestTimes = new uint[23];
            _m_highestScores = new uint[5];
            _m_bestPositions = new uint[1];
        }

        public uint PeopleKilledByPlayer
        {
            get { return m_peopleKilledByPlayer; }
            set { m_peopleKilledByPlayer = value; OnPropertyChanged(); }
        }

        public uint PeopleKilledByOthers
        {
            get { return m_peopleKilledByOthers; }
            set { m_peopleKilledByOthers = value; OnPropertyChanged(); }
        }

        public uint CarsExploded
        {
            get { return m_carsExploded; }
            set { m_carsExploded = value; OnPropertyChanged(); }
        }

        public uint BoatsExploded
        {
            get { return m_boatsExploded; }
            set { m_boatsExploded = value; OnPropertyChanged(); }
        }

        public uint TyresPopped
        {
            get { return m_tyresPopped; }
            set { m_tyresPopped = value; OnPropertyChanged(); }
        }

        public uint RoundsFiredByPlayer
        {
            get { return m_roundsFiredByPlayer; }
            set { m_roundsFiredByPlayer = value; OnPropertyChanged(); }
        }

        public uint PedType00Wasted
        {
            get { return m_pedType00Wasted; }
            set { m_pedType00Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType01Wasted
        {
            get { return m_pedType01Wasted; }
            set { m_pedType01Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType02Wasted
        {
            get { return m_pedType02Wasted; }
            set { m_pedType02Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType03Wasted
        {
            get { return m_pedType03Wasted; }
            set { m_pedType03Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType04Wasted
        {
            get { return m_pedType04Wasted; }
            set { m_pedType04Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType05Wasted
        {
            get { return m_pedType05Wasted; }
            set { m_pedType05Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType06Wasted
        {
            get { return m_pedType06Wasted; }
            set { m_pedType06Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType07Wasted
        {
            get { return m_pedType07Wasted; }
            set { m_pedType07Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType08Wasted
        {
            get { return m_pedType08Wasted; }
            set { m_pedType08Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType09Wasted
        {
            get { return m_pedType09Wasted; }
            set { m_pedType09Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType10Wasted
        {
            get { return m_pedType10Wasted; }
            set { m_pedType10Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType11Wasted
        {
            get { return m_pedType11Wasted; }
            set { m_pedType11Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType12Wasted
        {
            get { return m_pedType12Wasted; }
            set { m_pedType12Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType13Wasted
        {
            get { return m_pedType13Wasted; }
            set { m_pedType13Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType14Wasted
        {
            get { return m_pedType14Wasted; }
            set { m_pedType14Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType15Wasted
        {
            get { return m_pedType15Wasted; }
            set { m_pedType15Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType16Wasted
        {
            get { return m_pedType16Wasted; }
            set { m_pedType16Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType17Wasted
        {
            get { return m_pedType17Wasted; }
            set { m_pedType17Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType18Wasted
        {
            get { return m_pedType18Wasted; }
            set { m_pedType18Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType19Wasted
        {
            get { return m_pedType19Wasted; }
            set { m_pedType19Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType20Wasted
        {
            get { return m_pedType20Wasted; }
            set { m_pedType20Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType21Wasted
        {
            get { return m_pedType21Wasted; }
            set { m_pedType21Wasted = value; OnPropertyChanged(); }
        }

        public uint PedType22Wasted
        {
            get { return m_pedType22Wasted; }
            set { m_pedType22Wasted = value; OnPropertyChanged(); }
        }

        public uint HelisDestroyed
        {
            get { return m_helisDestroyed; }
            set { m_helisDestroyed = value; OnPropertyChanged(); }
        }

        public float ProgressMade
        {
            get { return m_progressMade; }
            set { m_progressMade = value; OnPropertyChanged(); }
        }

        public float TotalProgressInGame
        {
            get { return m_totalProgressInGame; }
            set { m_totalProgressInGame = value; OnPropertyChanged(); }
        }

        public uint KgsOfExplosivesUsed
        {
            get { return m_kgsOfExplosivesUsed; }
            set { m_kgsOfExplosivesUsed = value; OnPropertyChanged(); }
        }

        public uint BulletsThatHit
        {
            get { return m_bulletsThatHit; }
            set { m_bulletsThatHit = value; OnPropertyChanged(); }
        }

        public uint CarsCrushed
        {
            get { return m_carsCrushed; }
            set { m_carsCrushed = value; OnPropertyChanged(); }
        }

        public uint HeadsPopped
        {
            get { return m_headsPopped; }
            set { m_headsPopped = value; OnPropertyChanged(); }
        }

        public uint WantedStarsAttained
        {
            get { return m_wantedStarsAttained; }
            set { m_wantedStarsAttained = value; OnPropertyChanged(); }
        }

        public uint WantedStarsEvaded
        {
            get { return m_wantedStarsEvaded; }
            set { m_wantedStarsEvaded = value; OnPropertyChanged(); }
        }

        public uint TimesArrested
        {
            get { return m_timesArrested; }
            set { m_timesArrested = value; OnPropertyChanged(); }
        }

        public uint TimesDied
        {
            get { return m_timesDied; }
            set { m_timesDied = value; OnPropertyChanged(); }
        }

        public uint DaysPassed
        {
            get { return m_daysPassed; }
            set { m_daysPassed = value; OnPropertyChanged(); }
        }

        public uint SafeHouseVisits
        {
            get { return m_safeHouseVisits; }
            set { m_safeHouseVisits = value; OnPropertyChanged(); }
        }

        public uint Sprayings
        {
            get { return m_sprayings; }
            set { m_sprayings = value; OnPropertyChanged(); }
        }

        public float MaximumJumpDistance
        {
            get { return m_maximumJumpDistance; }
            set { m_maximumJumpDistance = value; OnPropertyChanged(); }
        }

        public float MaximumJumpHeight
        {
            get { return m_maximumJumpHeight; }
            set { m_maximumJumpHeight = value; OnPropertyChanged(); }
        }

        public uint MaximumJumpFlips
        {
            get { return m_maximumJumpFlips; }
            set { m_maximumJumpFlips = value; OnPropertyChanged(); }
        }

        public uint MaximumJumpSpins
        {
            get { return m_maximumJumpSpins; }
            set { m_maximumJumpSpins = value; OnPropertyChanged(); }
        }

        public InsaneStuntJump BestStuntJump
        {
            get { return m_bestStuntJump; }
            set { m_bestStuntJump = value; OnPropertyChanged(); }
        }

        public uint NumberOfUniqueJumpsFound
        {
            get { return m_numberOfUniqueJumpsFound; }
            set { m_numberOfUniqueJumpsFound = value; OnPropertyChanged(); }
        }

        public uint TotalNumberOfUniqueJumps
        {
            get { return m_totalNumberOfUniqueJumps; }
            set { m_totalNumberOfUniqueJumps = value; OnPropertyChanged(); }
        }

        public uint MissionsGiven
        {
            get { return m_missionsGiven; }
            set { m_missionsGiven = value; OnPropertyChanged(); }
        }

        public uint PassengersDroppedOffWithTaxi
        {
            get { return m_passengersDroppedOffWithTaxi; }
            set { m_passengersDroppedOffWithTaxi = value; OnPropertyChanged(); }
        }

        public uint MoneyMadeWithTaxi
        {
            get { return m_moneyMadeWithTaxi; }
            set { m_moneyMadeWithTaxi = value; OnPropertyChanged(); }
        }

        public bool IndustrialPassed
        {
            get { return m_industrialPassed; }
            set { m_industrialPassed = value; OnPropertyChanged(); }
        }

        public bool CommercialPassed
        {
            get { return m_commercialPassed; }
            set { m_commercialPassed = value; OnPropertyChanged(); }
        }

        public bool SuburbanPassed
        {
            get { return m_suburbanPassed; }
            set { m_suburbanPassed = value; OnPropertyChanged(); }
        }

        public bool PamphletMissionPassed
        {
            get { return m_pamphletMissionPassed; }
            set { m_pamphletMissionPassed = value; OnPropertyChanged(); }
        }

        public float DistanceTravelledOnFoot
        {
            get { return m_distanceTravelledOnFoot; }
            set { m_distanceTravelledOnFoot = value; OnPropertyChanged(); }
        }

        public float DistanceTravelledByCar
        {
            get { return m_distanceTravelledByCar; }
            set { m_distanceTravelledByCar = value; OnPropertyChanged(); }
        }

        public float DistanceTravelledByBike
        {
            get { return m_distanceTravelledByBike; }
            set { m_distanceTravelledByBike = value; OnPropertyChanged(); }
        }

        public float DistanceTravelledByBoat
        {
            get { return m_distanceTravelledByBoat; }
            set { m_distanceTravelledByBoat = value; OnPropertyChanged(); }
        }

        public uint LivesSavedWithAmbulance
        {
            get { return m_livesSavedWithAmbulance; }
            set { m_livesSavedWithAmbulance = value; OnPropertyChanged(); }
        }

        public uint CriminalsCaught
        {
            get { return m_criminalsCaught; }
            set { m_criminalsCaught = value; OnPropertyChanged(); }
        }

        public uint FiresExtinguished
        {
            get { return m_firesExtinguished; }
            set { m_firesExtinguished = value; OnPropertyChanged(); }
        }

        public uint HighestLevelVigilanteMission
        {
            get { return m_highestLevelVigilanteMission; }
            set { m_highestLevelVigilanteMission = value; OnPropertyChanged(); }
        }

        public uint HighestLevelAmbulanceMission
        {
            get { return m_highestLevelAmbulanceMission; }
            set { m_highestLevelAmbulanceMission = value; OnPropertyChanged(); }
        }

        public uint HighestLevelFireMission
        {
            get { return m_highestLevelFireMission; }
            set { m_highestLevelFireMission = value; OnPropertyChanged(); }
        }

        public uint PhotosTaken
        {
            get { return m_photosTaken; }
            set { m_photosTaken = value; OnPropertyChanged(); }
        }

        public uint NumberKillFrenziesPassed
        {
            get { return m_numberKillFrenziesPassed; }
            set { m_numberKillFrenziesPassed = value; OnPropertyChanged(); }
        }

        public uint MaxSecondsOnCarnageLeft
        {
            get { return m_maxSecondsOnCarnageLeft; }
            set { m_maxSecondsOnCarnageLeft = value; OnPropertyChanged(); }
        }

        public uint MaxKillsOnRcTriad
        {
            get { return m_maxKillsOnRcTriad; }
            set { m_maxKillsOnRcTriad = value; OnPropertyChanged(); }
        }

        public uint TotalNumberKillFrenzies
        {
            get { return m_totalNumberKillFrenzies; }
            set { m_totalNumberKillFrenzies = value; OnPropertyChanged(); }
        }

        public uint TotalNumberMissions
        {
            get { return m_totalNumberMissions; }
            set { m_totalNumberMissions = value; OnPropertyChanged(); }
        }

        public uint TimesDrowned
        {
            get { return m_timesDrowned; }
            set { m_timesDrowned = value; OnPropertyChanged(); }
        }

        public uint SeagullsKilled
        {
            get { return m_seagullsKilled; }
            set { m_seagullsKilled = value; OnPropertyChanged(); }
        }

        public float WeaponBudget
        {
            get { return m_weaponBudget; }
            set { m_weaponBudget = value; OnPropertyChanged(); }
        }

        public float PizzasDelivered
        {
            get { return m_pizzasDelivered; }
            set { m_pizzasDelivered = value; OnPropertyChanged(); }
        }

        public float NoodlesDelivered
        {
            get { return m_noodlesDelivered; }
            set { m_noodlesDelivered = value; OnPropertyChanged(); }
        }

        public float MoneyMadeFromTourist
        {
            get { return m_moneyMadeFromTourist; }
            set { m_moneyMadeFromTourist = value; OnPropertyChanged(); }
        }

        public float TouristsTakenToSpots
        {
            get { return m_touristsTakenToSpots; }
            set { m_touristsTakenToSpots = value; OnPropertyChanged(); }
        }

        public float TopScrapyardChallengeScore
        {
            get { return m_topScrapyardChallengeScore; }
            set { m_topScrapyardChallengeScore = value; OnPropertyChanged(); }
        }

        public float Top9mmMayhemScore
        {
            get { return m_top9mmMayhemScore; }
            set { m_top9mmMayhemScore = value; OnPropertyChanged(); }
        }

        public float TopScooterShooterScore
        {
            get { return m_topScooterShooterScore; }
            set { m_topScooterShooterScore = value; OnPropertyChanged(); }
        }

        public float TopWichitaWipeoutScore
        {
            get { return m_topWichitaWipeoutScore; }
            set { m_topWichitaWipeoutScore = value; OnPropertyChanged(); }
        }

        public uint LongestWheelie
        {
            get { return m_longestWheelie; }
            set { m_longestWheelie = value; OnPropertyChanged(); }
        }

        public uint LongestStoppie
        {
            get { return m_longestStoppie; }
            set { m_longestStoppie = value; OnPropertyChanged(); }
        }

        public uint Longest2Wheel
        {
            get { return m_longest2Wheel; }
            set { m_longest2Wheel = value; OnPropertyChanged(); }
        }

        public float LongestWheelieDist
        {
            get { return m_longestWheelieDist; }
            set { m_longestWheelieDist = value; OnPropertyChanged(); }
        }

        public float LongestStoppieDist
        {
            get { return m_longestStoppieDist; }
            set { m_longestStoppieDist = value; OnPropertyChanged(); }
        }

        public float Longest2WheelDist
        {
            get { return m_longest2WheelDist; }
            set { m_longest2WheelDist = value; OnPropertyChanged(); }
        }

        public float LongestFacePlantDist
        {
            get { return m_longestFacePlantDist; }
            set { m_longestFacePlantDist = value; OnPropertyChanged(); }
        }

        public float AutoPaintingBudget
        {
            get { return m_autoPaintingBudget; }
            set { m_autoPaintingBudget = value; OnPropertyChanged(); }
        }

        public uint PropertyDestroyed
        {
            get { return m_propertyDestroyed; }
            set { m_propertyDestroyed = value; OnPropertyChanged(); }
        }

        public Costumes UnlockedCostumes
        {
            get { return m_unlockedCostumes; }
            set { m_unlockedCostumes = value; OnPropertyChanged(); }
        }

        public uint BloodringKills
        {
            get { return m_bloodringKills; }
            set { m_bloodringKills = value; OnPropertyChanged(); }
        }

        public uint BloodringTime
        {
            get { return m_bloodringTime; }
            set { m_bloodringTime = value; OnPropertyChanged(); }
        }

        public float HighestChaseValue
        {
            get { return m_highestChaseValue; }
            set { m_highestChaseValue = value; OnPropertyChanged(); }
        }

        public uint KillsSinceLastCheckpoint
        {
            get { return m_killsSinceLastCheckpoint; }
            set { m_killsSinceLastCheckpoint = value; OnPropertyChanged(); }
        }

        public uint TotalLegitimateKills
        {
            get { return m_totalLegitimateKills; }
            set { m_totalLegitimateKills = value; OnPropertyChanged(); }
        }

        public string LastMissionPassedName
        {
            get { return m_lastMissionPassedName; }
            set { m_lastMissionPassedName = value; OnPropertyChanged(); }
        }

        public uint CheatedCount
        {
            get { return m_cheatedCount; }
            set { m_cheatedCount = value; OnPropertyChanged(); }
        }

        public uint CarsSold
        {
            get { return m_carsSold; }
            set { m_carsSold = value; OnPropertyChanged(); }
        }

        public uint MoneyMadeWithCarSales
        {
            get { return m_moneyMadeWithCarSales; }
            set { m_moneyMadeWithCarSales = value; OnPropertyChanged(); }
        }

        public uint BikesSold
        {
            get { return m_bikesSold; }
            set { m_bikesSold = value; OnPropertyChanged(); }
        }

        public uint MoneyMadeWithBikeSales
        {
            get { return m_moneyMadeWithBikeSales; }
            set { m_moneyMadeWithBikeSales = value; OnPropertyChanged(); }
        }

        public uint NumberOfExportedCars
        {
            get { return m_numberOfExportedCars; }
            set { m_numberOfExportedCars = value; OnPropertyChanged(); }
        }

        public uint TotalNumberOfCarExport
        {
            get { return m_totalNumberOfCarExport; }
            set { m_totalNumberOfCarExport = value; OnPropertyChanged(); }
        }

        public uint HighestLevelSlashTv
        {
            get { return m_highestLevelSlashTv; }
            set { m_highestLevelSlashTv = value; OnPropertyChanged(); }
        }

        public uint MoneyMadeWithSlashTv
        {
            get { return m_moneyMadeWithSlashTv; }
            set { m_moneyMadeWithSlashTv = value; OnPropertyChanged(); }
        }

        public uint TotalKillsOnSlashTv
        {
            get { return m_totalKillsOnSlashTv; }
            set { m_totalKillsOnSlashTv = value; OnPropertyChanged(); }
        }

        public uint CashMadeCollectingTrash
        {
            get { return m_cashMadeCollectingTrash; }
            set { m_cashMadeCollectingTrash = value; OnPropertyChanged(); }
        }

        public uint HitmenKilled
        {
            get { return m_hitmenKilled; }
            set { m_hitmenKilled = value; OnPropertyChanged(); }
        }

        public uint HighestGuarduanAngelJustuceDished
        {
            get { return m_highestGuarduanAngelJustuceDished; }
            set { m_highestGuarduanAngelJustuceDished = value; OnPropertyChanged(); }
        }

        public uint GuardianAngelMissionsPassed
        {
            get { return m_guardianAngelMissionsPassed; }
            set { m_guardianAngelMissionsPassed = value; OnPropertyChanged(); }
        }

        public uint GuardianAngelHighestLevelIndustrial
        {
            get { return m_guardianAngelHighestLevelIndustrial; }
            set { m_guardianAngelHighestLevelIndustrial = value; OnPropertyChanged(); }
        }

        public uint GuardianAngelHighestLevelCommercial
        {
            get { return m_guardianAngelHighestLevelCommercial; }
            set { m_guardianAngelHighestLevelCommercial = value; OnPropertyChanged(); }
        }

        public uint GuardianAngelHighestLevelSuburban
        {
            get { return m_guardianAngelHighestLevelSuburban; }
            set { m_guardianAngelHighestLevelSuburban = value; OnPropertyChanged(); }
        }

        public uint MostTimeLeftTrainRace
        {
            get { return m_mostTimeLeftTrainRace; }
            set { m_mostTimeLeftTrainRace = value; OnPropertyChanged(); }
        }

        public uint BestTimeGoGoFaggio
        {
            get { return m_bestTimeGoGoFaggio; }
            set { m_bestTimeGoGoFaggio = value; OnPropertyChanged(); }
        }

        public uint DirtBikeMostAir
        {
            get { return m_dirtBikeMostAir; }
            set { m_dirtBikeMostAir = value; OnPropertyChanged(); }
        }

        public uint NumberOutfitChanges
        {
            get { return m_numberOutfitChanges; }
            set { m_numberOutfitChanges = value; OnPropertyChanged(); }
        }

        public BanditRaceStat BestBanditLapTimes
        {
            get { return m_bestBanditLapTimes; }
            set {
                if (m_bestBanditLapTimes != null) {
                    m_bestBanditLapTimes.PropertyChanged -= BestBanditLapTimes_PropertyChanged;
                }
                m_bestBanditLapTimes = value;
                m_bestBanditLapTimes.PropertyChanged += BestBanditLapTimes_PropertyChanged;
                OnPropertyChanged();
            }
        }

        public BanditRaceStat BestBanditPositions
        {
            get { return m_bestBanditPositions; }
            set {
                if (m_bestBanditPositions != null) {
                    m_bestBanditPositions.PropertyChanged -= BestBanditPositions_PropertyChanged;
                }
                m_bestBanditPositions = value;
                m_bestBanditPositions.PropertyChanged += BestBanditPositions_PropertyChanged;
                OnPropertyChanged();
            }
        }

        public StreetRaceStat BestStreetRacePositions
        {
            get { return m_bestStreetRacePositions; }
            set {
                if (m_bestStreetRacePositions != null) {
                    m_bestStreetRacePositions.PropertyChanged -= BestStreetRacePositions_PropertyChanged;
                }
                m_bestStreetRacePositions = value;
                m_bestStreetRacePositions.PropertyChanged += BestStreetRacePositions_PropertyChanged;
                OnPropertyChanged();
            }
        }

        public StreetRaceStat FastestStreetRaceLapTimes
        {
            get { return m_fastestStreetRaceLapTimes; }
            set {
                if (m_fastestStreetRaceLapTimes != null) {
                    m_fastestStreetRaceLapTimes.PropertyChanged -= FastestStreetRaceLapTimes_PropertyChanged;
                }
                m_fastestStreetRaceLapTimes = value;
                m_fastestStreetRaceLapTimes.PropertyChanged += FastestStreetRaceLapTimes_PropertyChanged;
                OnPropertyChanged();
            }
        }

        public StreetRaceStat FastestStreetRaceTimes
        {
            get { return m_fastestStreetRaceTimes; }
            set {
                if (m_fastestStreetRaceTimes != null) {
                    m_fastestStreetRaceTimes.PropertyChanged -= FastestStreetRaceTimes_PropertyChanged;
                }
                m_fastestStreetRaceTimes = value;
                m_fastestStreetRaceTimes.PropertyChanged += FastestStreetRaceTimes_PropertyChanged;
                OnPropertyChanged();
            }
        }

        public DirtBikeRaceStat FastestDirtBikeLapTimes
        {
            get { return m_fastestDirtBikeLapTimes; }
            set {
                if (m_fastestDirtBikeLapTimes != null) {
                    m_fastestDirtBikeLapTimes.PropertyChanged -= FastestDirtBikeLapTimes_PropertyChanged;
                }
                m_fastestDirtBikeLapTimes = value;
                m_fastestDirtBikeLapTimes.PropertyChanged += FastestDirtBikeLapTimes_PropertyChanged;
                OnPropertyChanged();
            }
        }

        public DirtBikeRaceStat FastestDirtBikeTimes
        {
            get { return m_fastestDirtBikeTimes; }
            set {
                if (m_fastestDirtBikeTimes != null) {
                    m_fastestDirtBikeTimes.PropertyChanged -= FastestDirtBikeTimes_PropertyChanged;
                }
                m_fastestDirtBikeTimes = value;
                m_fastestDirtBikeTimes.PropertyChanged += FastestDirtBikeTimes_PropertyChanged;
                OnPropertyChanged();
            }
        }

        public FavoriteRadioStationList FavoriteRadioStationList
        {
            get { return m_favoriteRadioStationList; }
            set {
                if (m_favoriteRadioStationList != null) {
                    m_favoriteRadioStationList.PropertyChanged -= FavoriteRadioStationList_PropertyChanged;
                }
                m_favoriteRadioStationList = value;
                m_favoriteRadioStationList.PropertyChanged += FavoriteRadioStationList_PropertyChanged;
                OnPropertyChanged();
            }
        }

        private void BestBanditLapTimes_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(BestBanditLapTimes));
        }

        private void BestBanditPositions_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(BestBanditPositions));
        }

        private void BestStreetRacePositions_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(BestStreetRacePositions));
        }

        private void FastestStreetRaceLapTimes_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(FastestStreetRaceLapTimes));
        }

        private void FastestStreetRaceTimes_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(FastestStreetRaceTimes));
        }

        private void FastestDirtBikeLapTimes_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(FastestDirtBikeLapTimes));
        }

        private void FastestDirtBikeTimes_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(FastestDirtBikeTimes));
        }

        private void FavoriteRadioStationList_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(FavoriteRadioStationList));
        }
    }

    public class Stats<TFavoriteRadioStationList> : Stats
        where TFavoriteRadioStationList : FavoriteRadioStationList, new()
    {

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_peopleKilledByPlayer = r.ReadUInt32();
                m_peopleKilledByOthers = r.ReadUInt32();
                m_carsExploded = r.ReadUInt32();
                m_boatsExploded = r.ReadUInt32();
                m_tyresPopped = r.ReadUInt32();
                m_roundsFiredByPlayer = r.ReadUInt32();
                m_pedType00Wasted = r.ReadUInt32();
                m_pedType01Wasted = r.ReadUInt32();
                m_pedType02Wasted = r.ReadUInt32();
                m_pedType03Wasted = r.ReadUInt32();
                m_pedType04Wasted = r.ReadUInt32();
                m_pedType05Wasted = r.ReadUInt32();
                m_pedType06Wasted = r.ReadUInt32();
                m_pedType07Wasted = r.ReadUInt32();
                m_pedType08Wasted = r.ReadUInt32();
                m_pedType09Wasted = r.ReadUInt32();
                m_pedType10Wasted = r.ReadUInt32();
                m_pedType11Wasted = r.ReadUInt32();
                m_pedType12Wasted = r.ReadUInt32();
                m_pedType13Wasted = r.ReadUInt32();
                m_pedType14Wasted = r.ReadUInt32();
                m_pedType15Wasted = r.ReadUInt32();
                m_pedType16Wasted = r.ReadUInt32();
                m_pedType17Wasted = r.ReadUInt32();
                m_pedType18Wasted = r.ReadUInt32();
                m_pedType19Wasted = r.ReadUInt32();
                m_pedType20Wasted = r.ReadUInt32();
                m_pedType21Wasted = r.ReadUInt32();
                m_pedType22Wasted = r.ReadUInt32();
                m_helisDestroyed = r.ReadUInt32();
                m_progressMade = r.ReadSingle();
                m_totalProgressInGame = r.ReadSingle();
                m_kgsOfExplosivesUsed = r.ReadUInt32();
                m_bulletsThatHit = r.ReadUInt32();
                m_carsCrushed = r.ReadUInt32();
                m_headsPopped = r.ReadUInt32();
                m_wantedStarsAttained = r.ReadUInt32();
                m_wantedStarsEvaded = r.ReadUInt32();
                m_timesArrested = r.ReadUInt32();
                m_timesDied = r.ReadUInt32();
                m_daysPassed = r.ReadUInt32();
                m_safeHouseVisits = r.ReadUInt32();
                m_sprayings = r.ReadUInt32();
                m_maximumJumpDistance = r.ReadSingle();
                m_maximumJumpHeight = r.ReadSingle();
                m_maximumJumpFlips = r.ReadUInt32();
                m_maximumJumpSpins = r.ReadUInt32();
                m_bestStuntJump = (InsaneStuntJump) r.ReadUInt32();
                m_numberOfUniqueJumpsFound = r.ReadUInt32();
                m_totalNumberOfUniqueJumps = r.ReadUInt32();
                m_missionsGiven = r.ReadUInt32();
                m_passengersDroppedOffWithTaxi = r.ReadUInt32();
                m_moneyMadeWithTaxi = r.ReadUInt32();
                m_industrialPassed = r.ReadBoolean32();
                m_commercialPassed = r.ReadBoolean32();
                m_suburbanPassed = r.ReadBoolean32();
                m_pamphletMissionPassed = r.ReadBoolean32();
                _m_noMoreHurricanes = r.ReadBoolean32();
                m_distanceTravelledOnFoot = r.ReadSingle();
                m_distanceTravelledByCar = r.ReadSingle();
                m_distanceTravelledByBike = r.ReadSingle();
                m_distanceTravelledByBoat = r.ReadSingle();
                _m_distanceTravelledByPlane = r.ReadSingle();
                m_livesSavedWithAmbulance = r.ReadUInt32();
                m_criminalsCaught = r.ReadUInt32();
                m_firesExtinguished = r.ReadUInt32();
                m_highestLevelVigilanteMission = r.ReadUInt32();
                m_highestLevelAmbulanceMission = r.ReadUInt32();
                m_highestLevelFireMission = r.ReadUInt32();
                m_photosTaken = r.ReadUInt32();
                m_numberKillFrenziesPassed = r.ReadUInt32();
                m_maxSecondsOnCarnageLeft = r.ReadUInt32();
                m_maxKillsOnRcTriad = r.ReadUInt32();
                m_totalNumberKillFrenzies = r.ReadUInt32();
                m_totalNumberMissions = r.ReadUInt32();
                m_timesDrowned = r.ReadUInt32();
                m_seagullsKilled = r.ReadUInt32();
                m_weaponBudget = r.ReadSingle();
                _m_loanSharks = r.ReadUInt32();
                _m_movieStunts = r.ReadUInt32();
                m_pizzasDelivered = r.ReadSingle();
                m_noodlesDelivered = r.ReadSingle();
                m_moneyMadeFromTourist = r.ReadSingle();
                m_touristsTakenToSpots = r.ReadSingle();
                _m_garbagePickups = r.ReadUInt32();
                _m_iceCreamSold = r.ReadUInt32();
                _m_topShootingRangeScore = r.ReadUInt32();
                _m_shootingRank = r.ReadUInt32();
                m_topScrapyardChallengeScore = r.ReadSingle();
                m_top9mmMayhemScore = r.ReadSingle();
                m_topScooterShooterScore = r.ReadSingle();
                m_topWichitaWipeoutScore = r.ReadSingle();
                m_longestWheelie = r.ReadUInt32();
                m_longestStoppie = r.ReadUInt32();
                m_longest2Wheel = r.ReadUInt32();
                m_longestWheelieDist = r.ReadSingle();
                m_longestStoppieDist = r.ReadSingle();
                m_longest2WheelDist = r.ReadSingle();
                m_longestFacePlantDist = r.ReadSingle();
                m_autoPaintingBudget = r.ReadSingle();
                m_propertyDestroyed = r.ReadUInt32();
                _m_numPropertyOwned = r.ReadUInt32();
                m_unlockedCostumes = (Costumes) r.ReadUInt16();
                m_bloodringKills = r.ReadUInt32();
                m_bloodringTime = r.ReadUInt32();
                for (int i = 0; i < _m_propertyOwned.Length; i++) {
                    _m_propertyOwned[i] = r.ReadByte();
                }
                m_highestChaseValue = r.ReadSingle();
                for (int i = 0; i < _m_fastestTimes.Length; i++) {
                    _m_fastestTimes[i] = r.ReadUInt32();
                }
                for (int i = 0; i < _m_bestPositions.Length; i++) {
                    _m_bestPositions[i] = r.ReadUInt32();
                }
                for (int i = 0; i < _m_highestScores.Length; i++) {
                    _m_highestScores[i] = r.ReadUInt32();
                }
                m_killsSinceLastCheckpoint = r.ReadUInt32();
                m_totalLegitimateKills = r.ReadUInt32();
                m_lastMissionPassedName = r.ReadString(8);
                m_cheatedCount = r.ReadUInt32();
                m_carsSold = r.ReadUInt32();
                m_moneyMadeWithCarSales = r.ReadUInt32();
                m_bikesSold = r.ReadUInt32();
                m_moneyMadeWithBikeSales = r.ReadUInt32();
                m_numberOfExportedCars = r.ReadUInt32();
                m_totalNumberOfCarExport = r.ReadUInt32();
                m_highestLevelSlashTv = r.ReadUInt32();
                m_moneyMadeWithSlashTv = r.ReadUInt32();
                m_totalKillsOnSlashTv = r.ReadUInt32();
                _m_packagesSmuggled = r.ReadUInt32();
                _m_smugglersWasted = r.ReadUInt32();
                _m_fastestSmugglingTime = r.ReadUInt32();
                _m_moneyMadeInCoach = r.ReadUInt32();
                m_cashMadeCollectingTrash = r.ReadUInt32();
                m_hitmenKilled = r.ReadUInt32();
                m_highestGuarduanAngelJustuceDished = r.ReadUInt32();
                m_guardianAngelMissionsPassed = r.ReadUInt32();
                m_guardianAngelHighestLevelIndustrial = r.ReadUInt32();
                m_guardianAngelHighestLevelCommercial = r.ReadUInt32();
                m_guardianAngelHighestLevelSuburban = r.ReadUInt32();
                m_mostTimeLeftTrainRace = r.ReadUInt32();
                m_bestTimeGoGoFaggio = r.ReadUInt32();
                m_dirtBikeMostAir = r.ReadUInt32();
                _m_highestTrainCashEarned = r.ReadUInt32();
                _m_fastestHeliRaceTime = r.ReadUInt32();
                _m_bestHeliRacePosition = r.ReadUInt32();
                m_numberOutfitChanges = r.ReadUInt32();
                BestBanditLapTimes = Deserialize<BanditRaceStat>(stream);
                BestBanditPositions = Deserialize<BanditRaceStat>(stream);
                BestStreetRacePositions = Deserialize<StreetRaceStat>(stream);
                FastestStreetRaceLapTimes = Deserialize<StreetRaceStat>(stream);
                FastestStreetRaceTimes = Deserialize<StreetRaceStat>(stream);
                FastestDirtBikeLapTimes = Deserialize<DirtBikeRaceStat>(stream);
                FastestDirtBikeTimes = Deserialize<DirtBikeRaceStat>(stream);
                FavoriteRadioStationList = Deserialize<TFavoriteRadioStationList>(stream);
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write(m_peopleKilledByPlayer);
                w.Write(m_peopleKilledByOthers);
                w.Write(m_carsExploded);
                w.Write(m_boatsExploded);
                w.Write(m_tyresPopped);
                w.Write(m_roundsFiredByPlayer);
                w.Write(m_pedType00Wasted);
                w.Write(m_pedType01Wasted);
                w.Write(m_pedType02Wasted);
                w.Write(m_pedType03Wasted);
                w.Write(m_pedType04Wasted);
                w.Write(m_pedType05Wasted);
                w.Write(m_pedType06Wasted);
                w.Write(m_pedType07Wasted);
                w.Write(m_pedType08Wasted);
                w.Write(m_pedType09Wasted);
                w.Write(m_pedType10Wasted);
                w.Write(m_pedType11Wasted);
                w.Write(m_pedType12Wasted);
                w.Write(m_pedType13Wasted);
                w.Write(m_pedType14Wasted);
                w.Write(m_pedType15Wasted);
                w.Write(m_pedType16Wasted);
                w.Write(m_pedType17Wasted);
                w.Write(m_pedType18Wasted);
                w.Write(m_pedType19Wasted);
                w.Write(m_pedType20Wasted);
                w.Write(m_pedType21Wasted);
                w.Write(m_pedType22Wasted);
                w.Write(m_helisDestroyed);
                w.Write(m_progressMade);
                w.Write(m_totalProgressInGame);
                w.Write(m_kgsOfExplosivesUsed);
                w.Write(m_bulletsThatHit);
                w.Write(m_carsCrushed);
                w.Write(m_headsPopped);
                w.Write(m_wantedStarsAttained);
                w.Write(m_wantedStarsEvaded);
                w.Write(m_timesArrested);
                w.Write(m_timesDied);
                w.Write(m_daysPassed);
                w.Write(m_safeHouseVisits);
                w.Write(m_sprayings);
                w.Write(m_maximumJumpDistance);
                w.Write(m_maximumJumpHeight);
                w.Write(m_maximumJumpFlips);
                w.Write(m_maximumJumpSpins);
                w.Write((uint) m_bestStuntJump);
                w.Write(m_numberOfUniqueJumpsFound);
                w.Write(m_totalNumberOfUniqueJumps);
                w.Write(m_missionsGiven);
                w.Write(m_passengersDroppedOffWithTaxi);
                w.Write(m_moneyMadeWithTaxi);
                w.WriteBoolean32(m_industrialPassed);
                w.WriteBoolean32(m_commercialPassed);
                w.WriteBoolean32(m_suburbanPassed);
                w.WriteBoolean32(m_pamphletMissionPassed);
                w.WriteBoolean32(_m_noMoreHurricanes);
                w.Write(m_distanceTravelledOnFoot);
                w.Write(m_distanceTravelledByCar);
                w.Write(m_distanceTravelledByBike);
                w.Write(m_distanceTravelledByBoat);
                w.Write(_m_distanceTravelledByPlane);
                w.Write(m_livesSavedWithAmbulance);
                w.Write(m_criminalsCaught);
                w.Write(m_firesExtinguished);
                w.Write(m_highestLevelVigilanteMission);
                w.Write(m_highestLevelAmbulanceMission);
                w.Write(m_highestLevelFireMission);
                w.Write(m_photosTaken);
                w.Write(m_numberKillFrenziesPassed);
                w.Write(m_maxSecondsOnCarnageLeft);
                w.Write(m_maxKillsOnRcTriad);
                w.Write(m_totalNumberKillFrenzies);
                w.Write(m_totalNumberMissions);
                w.Write(m_timesDrowned);
                w.Write(m_seagullsKilled);
                w.Write(m_weaponBudget);
                w.Write(_m_loanSharks);
                w.Write(_m_movieStunts);
                w.Write(m_pizzasDelivered);
                w.Write(m_noodlesDelivered);
                w.Write(m_moneyMadeFromTourist);
                w.Write(m_touristsTakenToSpots);
                w.Write(_m_garbagePickups);
                w.Write(_m_iceCreamSold);
                w.Write(_m_topShootingRangeScore);
                w.Write(_m_shootingRank);
                w.Write(m_topScrapyardChallengeScore);
                w.Write(m_top9mmMayhemScore);
                w.Write(m_topScooterShooterScore);
                w.Write(m_topWichitaWipeoutScore);
                w.Write(m_longestWheelie);
                w.Write(m_longestStoppie);
                w.Write(m_longest2Wheel);
                w.Write(m_longestWheelieDist);
                w.Write(m_longestStoppieDist);
                w.Write(m_longest2WheelDist);
                w.Write(m_longestFacePlantDist);
                w.Write(m_autoPaintingBudget);
                w.Write(m_propertyDestroyed);
                w.Write(_m_numPropertyOwned);
                w.Write((ushort) m_unlockedCostumes);
                w.Write(m_bloodringKills);
                w.Write(m_bloodringTime);
                for (int i = 0; i < _m_propertyOwned.Length; i++) {
                    w.Write(_m_propertyOwned[i]);
                }
                w.Write(m_highestChaseValue);
                for (int i = 0; i < _m_fastestTimes.Length; i++) {
                    w.Write(_m_fastestTimes[i]);
                }
                for (int i = 0; i < _m_bestPositions.Length; i++) {
                    w.Write(_m_bestPositions[i]);
                }
                for (int i = 0; i < _m_highestScores.Length; i++) {
                    w.Write(_m_highestScores[i]);
                }
                w.Write(m_killsSinceLastCheckpoint);
                w.Write(m_totalLegitimateKills);
                w.WriteString(m_lastMissionPassedName, 8);
                w.Write(m_cheatedCount);
                w.Write(m_carsSold);
                w.Write(m_moneyMadeWithCarSales);
                w.Write(m_bikesSold);
                w.Write(m_moneyMadeWithBikeSales);
                w.Write(m_numberOfExportedCars);
                w.Write(m_totalNumberOfCarExport);
                w.Write(m_highestLevelSlashTv);
                w.Write(m_moneyMadeWithSlashTv);
                w.Write(m_totalKillsOnSlashTv);
                w.Write(_m_packagesSmuggled);
                w.Write(_m_smugglersWasted);
                w.Write(_m_fastestSmugglingTime);
                w.Write(_m_moneyMadeInCoach);
                w.Write(m_cashMadeCollectingTrash);
                w.Write(m_hitmenKilled);
                w.Write(m_highestGuarduanAngelJustuceDished);
                w.Write(m_guardianAngelMissionsPassed);
                w.Write(m_guardianAngelHighestLevelIndustrial);
                w.Write(m_guardianAngelHighestLevelCommercial);
                w.Write(m_guardianAngelHighestLevelSuburban);
                w.Write(m_mostTimeLeftTrainRace);
                w.Write(m_bestTimeGoGoFaggio);
                w.Write(m_dirtBikeMostAir);
                w.Write(_m_highestTrainCashEarned);
                w.Write(_m_fastestHeliRaceTime);
                w.Write(_m_bestHeliRacePosition);
                w.Write(m_numberOutfitChanges);
                Serialize(m_bestBanditLapTimes, stream);
                Serialize(m_bestBanditPositions, stream);
                Serialize(m_bestStreetRacePositions, stream);
                Serialize(m_fastestStreetRaceLapTimes, stream);
                Serialize(m_fastestStreetRaceTimes, stream);
                Serialize(m_fastestDirtBikeLapTimes, stream);
                Serialize(m_fastestDirtBikeTimes, stream);
                Serialize(m_favoriteRadioStationList, stream);
            }

            return stream.Position - start;
        }
    }
}
