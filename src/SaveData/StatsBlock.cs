#region License
/* Copyright(c) 2016-2018 Wes Hampson
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

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WHampson.Cascara;

namespace WHampson.LcsSaveEditor.SaveData
{
    internal class StatsBlock : SaveDataBlock
    {
        private readonly Primitive<uint> peopleKilledByPlayer;
        private readonly Primitive<uint> peopleKilledByOthers;
        private readonly Primitive<uint> carsExploded;
        private readonly Primitive<uint> boatsExploded;
        private readonly Primitive<uint> tiresPopped;
        private readonly Primitive<uint> roundsFiredByPlayer;
        private readonly Primitive<uint> pedTypeWasted;
        private readonly Primitive<uint> helisDestroyed;
        private readonly Primitive<float> progressMade;
        private readonly Primitive<float> totalProgressInGame;
        private readonly Primitive<uint> kgsOfExplosivesUsed;
        private readonly Primitive<uint> bulletsThatHit;
        private readonly Primitive<uint> carsCrushed;
        private readonly Primitive<uint> headsPopped;
        private readonly Primitive<uint> wantedStarsAttained;
        private readonly Primitive<uint> wantedStarsEvaded;
        private readonly Primitive<uint> timesArrested;
        private readonly Primitive<uint> timesDied;
        private readonly Primitive<uint> daysPassed;
        private readonly Primitive<uint> safeHouseVisits;
        private readonly Primitive<uint> sprayings;
        private readonly Primitive<float> maximumJumpDistance;
        private readonly Primitive<float> maximumJumpHeight;
        private readonly Primitive<uint> maximumJumpFlips;
        private readonly Primitive<uint> maximumJumpSpins;
        private readonly Primitive<uint> bestInsaneStuntJump;
        private readonly Primitive<uint> numberOfUniqueStuntJumpsFound;
        private readonly Primitive<uint> totalNumberOfUniqueStuntJumps;
        private readonly Primitive<uint> missionsGiven;
        private readonly Primitive<uint> passengersDroppedOffWithTaxi;
        private readonly Primitive<uint> moneyMadeWithTaxi;
        private readonly Primitive<Bool32> industrialPassed;
        private readonly Primitive<Bool32> commercialPassed;
        private readonly Primitive<Bool32> suburbanPassed;
        private readonly Primitive<Bool32> pamphletMissionPassed;
        private readonly Primitive<Bool32> noMoreHurricanes;
        private readonly Primitive<float> distanceTravelledOnFoot;
        private readonly Primitive<float> distanceTravelledByCar;
        private readonly Primitive<float> distanceTravelledByBike;
        private readonly Primitive<float> distanceTravelledByBoat;
        private readonly Primitive<float> distanceTravelledByPlane;
        private readonly Primitive<uint> livesSavedWithAmbulance;
        private readonly Primitive<uint> criminalsCaught;
        private readonly Primitive<uint> firesExtinguished;
        private readonly Primitive<uint> highestLevelVigilanteMission;
        private readonly Primitive<uint> highestLevelAmbulanceMission;
        private readonly Primitive<uint> highestLevelFireMission;
        private readonly Primitive<uint> photosTaken;
        private readonly Primitive<uint> numberKillFrenziesPassed;
        private readonly Primitive<uint> maxSecondsOnCarnageLeft;
        private readonly Primitive<uint> maxKillsOnRcTriad;
        private readonly Primitive<uint> totalNumbeKillFrenzies;
        private readonly Primitive<uint> totalNumberMissions;
        private readonly Primitive<uint> timesDrowned;
        private readonly Primitive<uint> seagullsKilled;
        private readonly Primitive<float> weaponBudget;
        private readonly Primitive<float> pizzasDelivered;
        private readonly Primitive<float> noodlesDelivered;
        private readonly Primitive<float> moneyMadeFromTourist;
        private readonly Primitive<float> touristsTakenToSpots;
        private readonly Primitive<float> topScrapyardChallengeScore;
        private readonly Primitive<float> top9mmMayhemScore;
        private readonly Primitive<float> topScooterShooterScore;
        private readonly Primitive<float> topWichitaWipeoutScore;
        private readonly Primitive<uint> longestWheelieTime;
        private readonly Primitive<uint> longestStoppieTime;
        private readonly Primitive<uint> longest2WheelTime;
        private readonly Primitive<float> longestWheelieDist;
        private readonly Primitive<float> longestStoppieDist;
        private readonly Primitive<float> longest2WheelDist;
        private readonly Primitive<float> longestFacePlantDist;
        private readonly Primitive<float> autoPaitingBudget;
        private readonly Primitive<uint> propertyDestroyed;
        private readonly Primitive<ushort> unlockedCostumes;
        private readonly Primitive<float> highestChaseValue;
        private readonly Primitive<uint> killsSinceLastCheckpoint;
        private readonly Primitive<uint> totalLegitimateKills;
        private readonly Primitive<Char8> lastMissionPassedName;
        private readonly Primitive<uint> cheatedCount;
        private readonly Primitive<uint> carsSold;
        private readonly Primitive<uint> moneyMadeWithCarSales;
        private readonly Primitive<uint> bikesSold;
        private readonly Primitive<uint> moneyMadeWithBikeSales;
        private readonly Primitive<uint> numberOfExportedCars;
        private readonly Primitive<uint> totalNumberOfCarExport;
        private readonly Primitive<uint> highestLevelSlashTv;
        private readonly Primitive<uint> moneyMadeWithSlashTv;
        private readonly Primitive<uint> totalKillsOnSlashTv;
        private readonly Primitive<uint> cashMadeCollectingTrash;
        private readonly Primitive<uint> hitmenKilled;
        private readonly Primitive<uint> highestGuardianAngelJusticeDished;
        private readonly Primitive<uint> guardianAngelMissionsPassed;
        private readonly Primitive<uint> guardianAngelHighestLevelIndustrial;
        private readonly Primitive<uint> guardianAngelHighestLevelCommercial;
        private readonly Primitive<uint> guardianAngelHighestLevelSuburban;
        private readonly Primitive<uint> mostTimeLeftTrainRace;
        private readonly Primitive<uint> bestTimeGoGoFaggio;
        private readonly Primitive<uint> highestTrainCashEarned;
        private readonly Primitive<uint> dirtBikeMostAir;
        private readonly Primitive<uint> numberOutfitChanges;
        private readonly BanditRaceStat bestBanditLapTimes;
        private readonly BanditRaceStat bestBanditPositions;
        private readonly StreetRaceStat bestStreetRacePositions;
        private readonly StreetRaceStat bestStreetRaceLapTimes;
        private readonly StreetRaceStat bestStreetRaceTimes;
        private readonly DirtBikeRaceStat bestDirtBikeLapTimes;
        private readonly DirtBikeRaceStat bestDirtBikeTimes;
        private readonly Primitive<float> favoriteRadioStationList;

        ArrayWrapper<uint> pedTypeWastedWrapper;
        ArrayWrapper<float> favoriteRadioStationListWrapper;
        StringWrapper lastMissionPassedNameWrapper;

        public StatsBlock()
        {
            peopleKilledByPlayer = new Primitive<uint>(null, 0);
            peopleKilledByOthers = new Primitive<uint>(null, 0);
            carsExploded = new Primitive<uint>(null, 0);
            boatsExploded = new Primitive<uint>(null, 0);
            tiresPopped = new Primitive<uint>(null, 0);
            roundsFiredByPlayer = new Primitive<uint>(null, 0);
            pedTypeWasted = new Primitive<uint>(null, 0);
            helisDestroyed = new Primitive<uint>(null, 0);
            progressMade = new Primitive<float>(null, 0);
            totalProgressInGame = new Primitive<float>(null, 0);
            kgsOfExplosivesUsed = new Primitive<uint>(null, 0);
            bulletsThatHit = new Primitive<uint>(null, 0);
            carsCrushed = new Primitive<uint>(null, 0);
            headsPopped = new Primitive<uint>(null, 0);
            wantedStarsAttained = new Primitive<uint>(null, 0);
            wantedStarsEvaded = new Primitive<uint>(null, 0);
            timesArrested = new Primitive<uint>(null, 0);
            timesDied = new Primitive<uint>(null, 0);
            daysPassed = new Primitive<uint>(null, 0);
            safeHouseVisits = new Primitive<uint>(null, 0);
            sprayings = new Primitive<uint>(null, 0);
            maximumJumpDistance = new Primitive<float>(null, 0);
            maximumJumpHeight = new Primitive<float>(null, 0);
            maximumJumpFlips = new Primitive<uint>(null, 0);
            maximumJumpSpins = new Primitive<uint>(null, 0);
            bestInsaneStuntJump = new Primitive<uint>(null, 0);
            numberOfUniqueStuntJumpsFound = new Primitive<uint>(null, 0);
            totalNumberOfUniqueStuntJumps = new Primitive<uint>(null, 0);
            missionsGiven = new Primitive<uint>(null, 0);
            passengersDroppedOffWithTaxi = new Primitive<uint>(null, 0);
            moneyMadeWithTaxi = new Primitive<uint>(null, 0);
            industrialPassed = new Primitive<Bool32>(null, 0);
            commercialPassed = new Primitive<Bool32>(null, 0);
            suburbanPassed = new Primitive<Bool32>(null, 0);
            pamphletMissionPassed = new Primitive<Bool32>(null, 0);
            noMoreHurricanes = new Primitive<Bool32>(null, 0);
            distanceTravelledOnFoot = new Primitive<float>(null, 0);
            distanceTravelledByCar = new Primitive<float>(null, 0);
            distanceTravelledByBike = new Primitive<float>(null, 0);
            distanceTravelledByBoat = new Primitive<float>(null, 0);
            distanceTravelledByPlane = new Primitive<float>(null, 0);
            livesSavedWithAmbulance = new Primitive<uint>(null, 0);
            criminalsCaught = new Primitive<uint>(null, 0);
            firesExtinguished = new Primitive<uint>(null, 0);
            highestLevelVigilanteMission = new Primitive<uint>(null, 0);
            highestLevelAmbulanceMission = new Primitive<uint>(null, 0);
            highestLevelFireMission = new Primitive<uint>(null, 0);
            photosTaken = new Primitive<uint>(null, 0);
            numberKillFrenziesPassed = new Primitive<uint>(null, 0);
            maxSecondsOnCarnageLeft = new Primitive<uint>(null, 0);
            maxKillsOnRcTriad = new Primitive<uint>(null, 0);
            totalNumbeKillFrenzies = new Primitive<uint>(null, 0);
            totalNumberMissions = new Primitive<uint>(null, 0);
            timesDrowned = new Primitive<uint>(null, 0);
            seagullsKilled = new Primitive<uint>(null, 0);
            weaponBudget = new Primitive<float>(null, 0);
            pizzasDelivered = new Primitive<float>(null, 0);
            noodlesDelivered = new Primitive<float>(null, 0);
            moneyMadeFromTourist = new Primitive<float>(null, 0);
            touristsTakenToSpots = new Primitive<float>(null, 0);
            topScrapyardChallengeScore = new Primitive<float>(null, 0);
            top9mmMayhemScore = new Primitive<float>(null, 0);
            topScooterShooterScore = new Primitive<float>(null, 0);
            topWichitaWipeoutScore = new Primitive<float>(null, 0);
            longestWheelieTime = new Primitive<uint>(null, 0);
            longestStoppieTime = new Primitive<uint>(null, 0);
            longest2WheelTime = new Primitive<uint>(null, 0);
            longestWheelieDist = new Primitive<float>(null, 0);
            longestStoppieDist = new Primitive<float>(null, 0);
            longest2WheelDist = new Primitive<float>(null, 0);
            longestFacePlantDist = new Primitive<float>(null, 0);
            autoPaitingBudget = new Primitive<float>(null, 0);
            propertyDestroyed = new Primitive<uint>(null, 0);
            unlockedCostumes = new Primitive<ushort>(null, 0);
            highestChaseValue = new Primitive<float>(null, 0);
            killsSinceLastCheckpoint = new Primitive<uint>(null, 0);
            totalLegitimateKills = new Primitive<uint>(null, 0);
            lastMissionPassedName = new Primitive<Char8>(null, 0);
            cheatedCount = new Primitive<uint>(null, 0);
            carsSold = new Primitive<uint>(null, 0);
            moneyMadeWithCarSales = new Primitive<uint>(null, 0);
            bikesSold = new Primitive<uint>(null, 0);
            moneyMadeWithBikeSales = new Primitive<uint>(null, 0);
            numberOfExportedCars = new Primitive<uint>(null, 0);
            totalNumberOfCarExport = new Primitive<uint>(null, 0);
            highestLevelSlashTv = new Primitive<uint>(null, 0);
            moneyMadeWithSlashTv = new Primitive<uint>(null, 0);
            totalKillsOnSlashTv = new Primitive<uint>(null, 0);
            cashMadeCollectingTrash = new Primitive<uint>(null, 0);
            hitmenKilled = new Primitive<uint>(null, 0);
            highestGuardianAngelJusticeDished = new Primitive<uint>(null, 0);
            guardianAngelMissionsPassed = new Primitive<uint>(null, 0);
            guardianAngelHighestLevelIndustrial = new Primitive<uint>(null, 0);
            guardianAngelHighestLevelCommercial = new Primitive<uint>(null, 0);
            guardianAngelHighestLevelSuburban = new Primitive<uint>(null, 0);
            mostTimeLeftTrainRace = new Primitive<uint>(null, 0);
            bestTimeGoGoFaggio = new Primitive<uint>(null, 0);
            highestTrainCashEarned = new Primitive<uint>(null, 0);
            dirtBikeMostAir = new Primitive<uint>(null, 0);
            numberOutfitChanges = new Primitive<uint>(null, 0);
            bestBanditLapTimes = new BanditRaceStat();
            bestBanditPositions = new BanditRaceStat();
            bestStreetRacePositions = new StreetRaceStat();
            bestStreetRaceLapTimes = new StreetRaceStat();
            bestStreetRaceTimes = new StreetRaceStat();
            bestDirtBikeLapTimes = new DirtBikeRaceStat();
            bestDirtBikeTimes = new DirtBikeRaceStat();
            favoriteRadioStationList = new Primitive<float>(null, 0);
        }

        public uint PeopleKilledByPlayer
        {
            get { return peopleKilledByPlayer.Value; }
            set { peopleKilledByPlayer.Value = value; }
        }

        public uint PeopleKilledByOthers
        {
            get { return peopleKilledByOthers.Value; }
            set { peopleKilledByOthers.Value = value; }
        }

        public uint CarsExploded
        {
            get { return carsExploded.Value; }
            set { carsExploded.Value = value; }
        }

        public uint BoatsExploded
        {
            get { return boatsExploded.Value; }
            set { boatsExploded.Value = value; }
        }

        public uint TiresPopped
        {
            get { return tiresPopped.Value; }
            set { tiresPopped.Value = value; }
        }

        public uint RoundsFiredByPlayer
        {
            get { return roundsFiredByPlayer.Value; }
            set { roundsFiredByPlayer.Value = value; }
        }

        public ArrayWrapper<uint> PedTypeWasted
        {
            get {
                if (pedTypeWastedWrapper == null) {
                    pedTypeWastedWrapper = new ArrayWrapper<uint>(pedTypeWasted);
                }
                return pedTypeWastedWrapper;
            }
        }

        public uint HelisDestroyed
        {
            get { return helisDestroyed.Value; }
            set { helisDestroyed.Value = value; }
        }

        public float ProgressMade
        {
            get { return progressMade.Value; }
            set { progressMade.Value = value; }
        }

        public float TotalProgressInGame
        {
            get { return totalProgressInGame.Value; }
            set { totalProgressInGame.Value = value; }
        }

        public uint KgsOfExplosivesUsed
        {
            get { return kgsOfExplosivesUsed.Value; }
            set { kgsOfExplosivesUsed.Value = value; }
        }

        public uint BulletsThatHit
        {
            get { return bulletsThatHit.Value; }
            set { bulletsThatHit.Value = value; }
        }

        public uint CarsCrushed
        {
            get { return carsCrushed.Value; }
            set { carsCrushed.Value = value; }
        }

        public uint HeadsPopped
        {
            get { return headsPopped.Value; }
            set { headsPopped.Value = value; }
        }

        public uint WantedStarsAttained
        {
            get { return wantedStarsAttained.Value; }
            set { wantedStarsAttained.Value = value; }
        }

        public uint WantedStarsEvaded
        {
            get { return wantedStarsEvaded.Value; }
            set { wantedStarsEvaded.Value = value; }
        }

        public uint TimesArrested
        {
            get { return timesArrested.Value; }
            set { timesArrested.Value = value; }
        }

        public uint TimesDied
        {
            get { return timesDied.Value; }
            set { timesDied.Value = value; }
        }

        public uint DaysPassed
        {
            get { return daysPassed.Value; }
            set { daysPassed.Value = value; }
        }

        public uint SafeHouseVisits
        {
            get { return safeHouseVisits.Value; }
            set { safeHouseVisits.Value = value; }
        }

        public uint Sprayings
        {
            get { return sprayings.Value; }
            set { sprayings.Value = value; }
        }

        public float MaximumJumpDistance
        {
            get { return maximumJumpDistance.Value; }
            set { maximumJumpDistance.Value = value; }
        }

        public float MaximumJumpHeight
        {
            get { return maximumJumpHeight.Value; }
            set { maximumJumpHeight.Value = value; }
        }

        public uint MaximumJumpFlips
        {
            get { return maximumJumpFlips.Value; }
            set { maximumJumpFlips.Value = value; }
        }

        public uint MaximumJumpSpins
        {
            get { return maximumJumpSpins.Value; }
            set { maximumJumpSpins.Value = value; }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public InsaneStuntJump BestInsaneStuntJump
        {
            get { return (InsaneStuntJump) bestInsaneStuntJump.Value; }
            set { bestInsaneStuntJump.Value = (uint) value; }
        }

        public uint NumberOfUniqueStuntJumpsFound
        {
            get { return numberOfUniqueStuntJumpsFound.Value; }
            set { numberOfUniqueStuntJumpsFound.Value = value; }
        }

        public uint TotalNumberOfUniqueStuntJumps
        {
            get { return totalNumberOfUniqueStuntJumps.Value; }
            set { totalNumberOfUniqueStuntJumps.Value = value; }
        }

        public uint MissionsGiven
        {
            get { return missionsGiven.Value; }
            set { missionsGiven.Value = value; }
        }

        public uint PassengersDroppedOffWithTaxi
        {
            get { return passengersDroppedOffWithTaxi.Value; }
            set { passengersDroppedOffWithTaxi.Value = value; }
        }

        public uint MoneyMadeWithTaxi
        {
            get { return moneyMadeWithTaxi.Value; }
            set { moneyMadeWithTaxi.Value = value; }
        }

        public Bool32 IndustrialPassed
        {
            get { return industrialPassed.Value; }
            set { industrialPassed.Value = value; }
        }

        public Bool32 CommercialPassed
        {
            get { return commercialPassed.Value; }
            set { commercialPassed.Value = value; }
        }

        public Bool32 SuburbanPassed
        {
            get { return suburbanPassed.Value; }
            set { suburbanPassed.Value = value; }
        }

        public Bool32 PamphletMissionPassed
        {
            get { return pamphletMissionPassed.Value; }
            set { pamphletMissionPassed.Value = value; }
        }

        public Bool32 NoMoreHurricanes
        {
            get { return noMoreHurricanes.Value; }
            set { noMoreHurricanes.Value = value; }
        }

        public float DistanceTravelledOnFoot
        {
            get { return distanceTravelledOnFoot.Value; }
            set { distanceTravelledOnFoot.Value = value; }
        }

        public float DistanceTravelledByCar
        {
            get { return distanceTravelledByCar.Value; }
            set { distanceTravelledByCar.Value = value; }
        }

        public float DistanceTravelledByBike
        {
            get { return distanceTravelledByBike.Value; }
            set { distanceTravelledByBike.Value = value; }
        }

        public float DistanceTravelledByBoat
        {
            get { return distanceTravelledByBoat.Value; }
            set { distanceTravelledByBoat.Value = value; }
        }

        public float DistanceTravelledByPlane
        {
            get { return distanceTravelledByPlane.Value; }
            set { distanceTravelledByPlane.Value = value; }
        }

        public uint LivesSavedWithAmbulance
        {
            get { return livesSavedWithAmbulance.Value; }
            set { livesSavedWithAmbulance.Value = value; }
        }

        public uint CriminalsCaught
        {
            get { return criminalsCaught.Value; }
            set { criminalsCaught.Value = value; }
        }

        public uint FiresExtinguished
        {
            get { return firesExtinguished.Value; }
            set { firesExtinguished.Value = value; }
        }

        public uint HighestLevelVigilanteMission
        {
            get { return highestLevelVigilanteMission.Value; }
            set { highestLevelVigilanteMission.Value = value; }
        }

        public uint HighestLevelAmbulanceMission
        {
            get { return highestLevelAmbulanceMission.Value; }
            set { highestLevelAmbulanceMission.Value = value; }
        }

        public uint HighestLevelFireMission
        {
            get { return highestLevelFireMission.Value; }
            set { highestLevelFireMission.Value = value; }
        }

        public uint PhotosTaken
        {
            get { return photosTaken.Value; }
            set { photosTaken.Value = value; }
        }

        public uint NumberKillFrenziesPassed
        {
            get { return numberKillFrenziesPassed.Value; }
            set { numberKillFrenziesPassed.Value = value; }
        }

        public uint MaxSecondsOnCarnageLeft
        {
            get { return maxSecondsOnCarnageLeft.Value; }
            set { maxSecondsOnCarnageLeft.Value = value; }
        }

        public uint MaxKillsOnRcTriad
        {
            get { return maxKillsOnRcTriad.Value; }
            set { maxKillsOnRcTriad.Value = value; }
        }

        public uint TotalNumbeKillFrenzies
        {
            get { return totalNumbeKillFrenzies.Value; }
            set { totalNumbeKillFrenzies.Value = value; }
        }

        public uint TotalNumberMissions
        {
            get { return totalNumberMissions.Value; }
            set { totalNumberMissions.Value = value; }
        }

        public uint TimesDrowned
        {
            get { return timesDrowned.Value; }
            set { timesDrowned.Value = value; }
        }

        public uint SeagullsKilled
        {
            get { return seagullsKilled.Value; }
            set { seagullsKilled.Value = value; }
        }

        public float WeaponBudget
        {
            get { return weaponBudget.Value; }
            set { weaponBudget.Value = value; }
        }

        public float PizzasDelivered
        {
            get { return pizzasDelivered.Value; }
            set { pizzasDelivered.Value = value; }
        }

        public float NoodlesDelivered
        {
            get { return noodlesDelivered.Value; }
            set { noodlesDelivered.Value = value; }
        }

        public float MoneyMadeFromTourist
        {
            get { return moneyMadeFromTourist.Value; }
            set { moneyMadeFromTourist.Value = value; }
        }

        public float TouristsTakenToSpots
        {
            get { return touristsTakenToSpots.Value; }
            set { touristsTakenToSpots.Value = value; }
        }

        public float TopScrapyardChallengeScore
        {
            get { return topScrapyardChallengeScore.Value; }
            set { topScrapyardChallengeScore.Value = value; }
        }

        public float Top9mmMayhemScore
        {
            get { return top9mmMayhemScore.Value; }
            set { top9mmMayhemScore.Value = value; }
        }

        public float TopScooterShooterScore
        {
            get { return topScooterShooterScore.Value; }
            set { topScooterShooterScore.Value = value; }
        }

        public float TopWichitaWipeoutScore
        {
            get { return topWichitaWipeoutScore.Value; }
            set { topWichitaWipeoutScore.Value = value; }
        }

        public uint LongestWheelieTime
        {
            get { return longestWheelieTime.Value; }
            set { longestWheelieTime.Value = value; }
        }

        public uint LongestStoppieTime
        {
            get { return longestStoppieTime.Value; }
            set { longestStoppieTime.Value = value; }
        }

        public uint Longest2WheelTime
        {
            get { return longest2WheelTime.Value; }
            set { longest2WheelTime.Value = value; }
        }

        public float LongestWheelieDist
        {
            get { return longestWheelieDist.Value; }
            set { longestWheelieDist.Value = value; }
        }

        public float LongestStoppieDist
        {
            get { return longestStoppieDist.Value; }
            set { longestStoppieDist.Value = value; }
        }

        public float Longest2WheelDist
        {
            get { return longest2WheelDist.Value; }
            set { longest2WheelDist.Value = value; }
        }

        public float LongestFacePlantDist
        {
            get { return longestFacePlantDist.Value; }
            set { longestFacePlantDist.Value = value; }
        }

        public float AutoPaitingBudget
        {
            get { return autoPaitingBudget.Value; }
            set { autoPaitingBudget.Value = value; }
        }

        public uint PropertyDestroyed
        {
            get { return propertyDestroyed.Value; }
            set { propertyDestroyed.Value = value; }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public Costumes UnlockedCostumes
        {
            get { return (Costumes) unlockedCostumes.Value; }
            set { unlockedCostumes.Value = (ushort) value; }
        }

        public float HighestChaseValue
        {
            get { return highestChaseValue.Value; }
            set { highestChaseValue.Value = value; }
        }

        public uint KillsSinceLastCheckpoint
        {
            get { return killsSinceLastCheckpoint.Value; }
            set { killsSinceLastCheckpoint.Value = value; }
        }

        public uint TotalLegitimateKills
        {
            get { return totalLegitimateKills.Value; }
            set { totalLegitimateKills.Value = value; }
        }

        public string LastMissionPassedName
        {
            get {
                if (lastMissionPassedNameWrapper == null) {
                    lastMissionPassedNameWrapper = new StringWrapper(lastMissionPassedName);
                }
                return lastMissionPassedNameWrapper.Value;
            }
            set {
                if (lastMissionPassedNameWrapper == null) {
                    lastMissionPassedNameWrapper = new StringWrapper(lastMissionPassedName);
                }
                lastMissionPassedNameWrapper.Value = value;
            }
        }

        public uint CheatedCount
        {
            get { return cheatedCount.Value; }
            set { cheatedCount.Value = value; }
        }

        public uint CarsSold
        {
            get { return carsSold.Value; }
            set { carsSold.Value = value; }
        }

        public uint MoneyMadeWithCarSales
        {
            get { return moneyMadeWithCarSales.Value; }
            set { moneyMadeWithCarSales.Value = value; }
        }

        public uint BikesSold
        {
            get { return bikesSold.Value; }
            set { bikesSold.Value = value; }
        }

        public uint MoneyMadeWithBikeSales
        {
            get { return moneyMadeWithBikeSales.Value; }
            set { moneyMadeWithBikeSales.Value = value; }
        }

        public uint NumberOfExportedCars
        {
            get { return numberOfExportedCars.Value; }
            set { numberOfExportedCars.Value = value; }
        }

        public uint TotalNumberOfCarExport
        {
            get { return totalNumberOfCarExport.Value; }
            set { totalNumberOfCarExport.Value = value; }
        }

        public uint HighestLevelSlashTv
        {
            get { return highestLevelSlashTv.Value; }
            set { highestLevelSlashTv.Value = value; }
        }

        public uint MoneyMadeWithSlashTv
        {
            get { return moneyMadeWithSlashTv.Value; }
            set { moneyMadeWithSlashTv.Value = value; }
        }

        public uint TotalKillsOnSlashTv
        {
            get { return totalKillsOnSlashTv.Value; }
            set { totalKillsOnSlashTv.Value = value; }
        }

        public uint CashMadeCollectingTrash
        {
            get { return cashMadeCollectingTrash.Value; }
            set { cashMadeCollectingTrash.Value = value; }
        }

        public uint HitmenKilled
        {
            get { return hitmenKilled.Value; }
            set { hitmenKilled.Value = value; }
        }

        public uint HighestGuardianAngelJusticeDished
        {
            get { return highestGuardianAngelJusticeDished.Value; }
            set { highestGuardianAngelJusticeDished.Value = value; }
        }

        public uint GuardianAngelMissionsPassed
        {
            get { return guardianAngelMissionsPassed.Value; }
            set { guardianAngelMissionsPassed.Value = value; }
        }

        public uint GuardianAngelHighestLevelIndustrial
        {
            get { return guardianAngelHighestLevelIndustrial.Value; }
            set { guardianAngelHighestLevelIndustrial.Value = value; }
        }

        public uint GuardianAngelHighestLevelCommercial
        {
            get { return guardianAngelHighestLevelCommercial.Value; }
            set { guardianAngelHighestLevelCommercial.Value = value; }
        }

        public uint GuardianAngelHighestLevelSuburban
        {
            get { return guardianAngelHighestLevelSuburban.Value; }
            set { guardianAngelHighestLevelSuburban.Value = value; }
        }

        public uint MostTimeLeftTrainRace
        {
            get { return mostTimeLeftTrainRace.Value; }
            set { mostTimeLeftTrainRace.Value = value; }
        }

        public uint BestTimeGoGoFaggio
        {
            get { return bestTimeGoGoFaggio.Value; }
            set { bestTimeGoGoFaggio.Value = value; }
        }

        public uint HighestTrainCashEarned
        {
            get { return highestTrainCashEarned.Value; }
            set { highestTrainCashEarned.Value = value; }
        }

        public uint DirtBikeMostAir
        {
            get { return dirtBikeMostAir.Value; }
            set { dirtBikeMostAir.Value = value; }
        }

        public uint NumberOutfitChanges
        {
            get { return numberOutfitChanges.Value; }
            set { numberOutfitChanges.Value = value; }
        }

        public BanditRaceStat BestBanditLapTimes
        {
            get { return bestBanditLapTimes; }
        }

        public BanditRaceStat BestBanditPositions
        {
            get { return bestBanditPositions; }
        }

        public StreetRaceStat BestStreetRacePositions
        {
            get { return bestStreetRacePositions; }
        }

        public StreetRaceStat BestStreetRaceLapTimes
        {
            get { return bestStreetRaceLapTimes; }
        }

        public StreetRaceStat BestStreetRaceTimes
        {
            get { return bestStreetRaceTimes; }
        }

        public DirtBikeRaceStat BestDirtBikeLapTimes
        {
            get { return bestDirtBikeLapTimes; }
        }

        public DirtBikeRaceStat BestDirtBikeTimes
        {
            get { return bestDirtBikeTimes; }
        }

        public ArrayWrapper<float> FavoriteRadioStationList
        {
            get {
                if (favoriteRadioStationListWrapper == null) {
                    favoriteRadioStationListWrapper = new ArrayWrapper<float>(favoriteRadioStationList);
                }
                return favoriteRadioStationListWrapper;
            }
        }
    }
}
