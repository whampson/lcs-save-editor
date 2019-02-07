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

using LcsSaveEditor.DataTypes;
using LcsSaveEditor.Infrastructure;

namespace LcsSaveEditor.Models
{
    public abstract class Stats : SerializableObject
    {
        public Stats()
        {
            _m_propertyOwned = new byte[15];
            _m_fastestTimes = new uint[23];
            _m_highestScores = new uint[5];
            _m_bestPositions = new uint[1];
        }

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
        protected ushort m_unlockedCostumes;
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
        protected DirtBikeRaceStat m_fastestDirtBikeRaceTimes;
        protected DirtBikeRaceStat m_fastestDirtBikeLapTimes;
        protected DirtBikeRaceStat m_fastestDirtBikeTimes;
        protected FavoriteRadioStationList m_favoriteRadioStationList;
    }
}
