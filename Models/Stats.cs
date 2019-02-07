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

        // TODO: proprties
    }

    public class Stats<T> : Stats
        where T : FavoriteRadioStationList, new()
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
                m_unlockedCostumes = r.ReadUInt16();
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
                m_bestBanditLapTimes = Deserialize<BanditRaceStat>(stream);
                m_bestBanditPositions = Deserialize<BanditRaceStat>(stream);
                m_bestStreetRacePositions = Deserialize<StreetRaceStat>(stream);
                m_fastestStreetRaceLapTimes = Deserialize<StreetRaceStat>(stream);
                m_fastestStreetRaceTimes = Deserialize<StreetRaceStat>(stream);
                m_fastestDirtBikeLapTimes = Deserialize<DirtBikeRaceStat>(stream);
                m_fastestDirtBikeTimes = Deserialize<DirtBikeRaceStat>(stream);
                m_favoriteRadioStationList = Deserialize<T>(stream);
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
                w.Write(m_unlockedCostumes);
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
