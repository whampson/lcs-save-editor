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
using LcsSaveEditor.Extensions;
using System.IO;
using System.Text;

namespace LcsSaveEditor.Models
{
    /// <summary>
    /// Miscellaneous game variables formatted for the
    /// Android and iOS versions of the game.
    /// </summary>
    public class SimpleVarsMobile : SimpleVars
    {
        private uint m_unknown00;
        private uint m_unknown04;
        private uint m_unknown08;
        private uint[] m_unknown1C;

        public SimpleVarsMobile()
        {
            m_unknown1C = new uint[24];
        }

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_unknown00 = r.ReadUInt32();
                m_unknown04 = r.ReadUInt32();
                m_unknown08 = r.ReadUInt32();

                m_saveNameGxt = r.ReadWideString();

                //m_unknown00 = r.ReadUInt32();
                //_m_currLevel = r.ReadUInt32();
                //_m_currArea = r.ReadUInt32();
                //m_prefsLanguage = (Language) r.ReadUInt32();
                //m_millisecondsPerGameMinute = r.ReadUInt32();
                //m_lastClockTick = r.ReadUInt32();
                //m_gameClockHours = r.ReadByte();
                //m_gameClockMinutes = r.ReadByte();
                //_m_gameClockSeconds = r.ReadUInt16();
                //m_totalTimePlayedInMilliseconds = r.ReadUInt32();
                //_m_timeScale = r.ReadSingle();
                //_m_timeStep = r.ReadSingle();
                //_m_timeStepNonClipped = r.ReadSingle();
                //_m_framesPerUpdate = r.ReadSingle();
                //_m_frameCounter = r.ReadUInt32();
                //m_prevWeatherType = (Weather) r.ReadInt16();
                //m_currWeatherType = (Weather) r.ReadInt16();
                //m_forcedWeatherType = (Weather) r.ReadInt16();
                //r.ReadUInt16();     // align bytes
                //m_weatherTypeInList = r.ReadUInt32();
                //_m_interpolationValue = r.ReadSingle();
                //m_cameraPosition = Deserialize<Vector3d>(stream);
                //m_prefsVehicleCameraMode = (VehicleCameraMode) r.ReadSingle();
                //m_unknown58 = r.ReadSingle();
                //m_unknown5C = r.ReadUInt32();
                //m_unknown60 = r.ReadUInt32();
                //m_unknown64 = r.ReadSingle();
                //m_prefsBrightness = r.ReadUInt32();
                //m_prefsDisplayHud = r.ReadBoolean();
                //m_prefsShowSubtitles = r.ReadBoolean();
                //r.ReadUInt16();     // align bytes
                //m_prefsRadarMode = (RadarMode) r.ReadUInt32();
                //m_blurOn = (r.ReadUInt32() != 0);
                //m_unknown78 = r.ReadUInt32();
                //m_unknown7C = r.ReadUInt32();
                //m_prefsUseWideScreen = r.ReadBoolean();
                //m_unknown81 = r.ReadByte();
                //m_unknown82 = r.ReadByte();
                //m_unknown83 = r.ReadByte();
                //m_prefsRadioVolume = r.ReadUInt32();
                //m_prefsSfxVolume = r.ReadUInt32();
                //_m_prefsRadioStation = (RadioStation) r.ReadByte();
                //_m_prefsOutput = r.ReadByte();
                //m_unknown8E = r.ReadByte();
                //m_unknown8F = r.ReadByte();
                //m_unknown90 = r.ReadUInt32();
                //m_unknown94 = r.ReadUInt32();
                //m_unknown98 = r.ReadUInt32();
                //m_unknown9C = r.ReadUInt32();
                //m_unknownA0 = r.ReadUInt32();
                //m_unknownA4 = r.ReadUInt32();
                //m_unknownA8 = r.ReadUInt32();
                //m_unknownAC = r.ReadUInt32();
                //m_unknownB0 = r.ReadUInt32();
                //m_unknownB4 = r.ReadUInt32();
                //m_unknownB8 = r.ReadSingle();
                //m_prefsControllerConfig = (ControllerConfig) r.ReadUInt16();
                //m_unknownBE = r.ReadUInt16();
                //m_prefsDisableInvertLook = (r.ReadUInt32() != 0);
                //m_prefsUseVibration = r.ReadBoolean();
                //m_unknownC5 = r.ReadByte();
                //m_unknownC6 = r.ReadByte();
                //m_unknownC7 = r.ReadByte();
                //m_playerHasCheated = (r.ReadUInt32() != 0);
                //_m_allTaxisHaveNitro = (r.ReadUInt32() != 0);
                //m_targetIsOn = (r.ReadUInt32() != 0);
                //m_targetPosition = Deserialize<Vector2d>(stream);
                //m_unknownDC = r.ReadUInt32();
                //m_saveTime = Deserialize<Timestamp>(stream);
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                //w.Write(m_unknown00);
                //w.Write(m_unknown02);
                //w.Write(m_unknown04);
                //w.Write(m_unknown06);
                //w.Write(_m_currLevel);
                //w.Write(_m_currArea);
                //w.Write((uint) m_prefsLanguage);
                //w.Write(m_millisecondsPerGameMinute);
                //w.Write(m_lastClockTick);
                //w.Write(m_gameClockHours);
                //w.Write(m_gameClockMinutes);
                //w.Write(_m_gameClockSeconds);
                //w.Write(m_totalTimePlayedInMilliseconds);
                //w.Write(_m_timeScale);
                //w.Write(_m_timeStep);
                //w.Write(_m_timeStepNonClipped);
                //w.Write(_m_framesPerUpdate);
                //w.Write(_m_frameCounter);
                //w.Write((short) m_prevWeatherType);
                //w.Write((short) m_currWeatherType);
                //w.Write((short) m_forcedWeatherType);
                //w.Write((ushort) 0);    // align bytes
                //w.Write(m_weatherTypeInList);
                //w.Write(_m_interpolationValue);
                //Serialize(m_cameraPosition, stream);
                //w.Write((float) m_prefsVehicleCameraMode);
                //w.Write(m_unknown58);
                //w.Write(m_unknown5C);
                //w.Write(m_unknown60);
                //w.Write(m_unknown64);
                //w.Write(m_prefsBrightness);
                //w.Write(m_prefsDisplayHud);
                //w.Write(m_prefsShowSubtitles);
                //w.Write((ushort) 0);    // align bytes
                //w.Write((uint) m_prefsRadarMode);
                //w.Write(m_blurOn ? 1U : 0U);
                //w.Write(m_unknown78);
                //w.Write(m_unknown7C);
                //w.Write(m_prefsUseWideScreen);
                //w.Write(m_unknown81);
                //w.Write(m_unknown82);
                //w.Write(m_unknown83);
                //w.Write(m_prefsRadioVolume);
                //w.Write(m_prefsSfxVolume);
                //w.Write((byte) _m_prefsRadioStation);
                //w.Write(_m_prefsOutput);
                //w.Write(m_unknown8E);
                //w.Write(m_unknown8F);
                //w.Write(m_unknown90);
                //w.Write(m_unknown94);
                //w.Write(m_unknown98);
                //w.Write(m_unknown9C);
                //w.Write(m_unknownA0);
                //w.Write(m_unknownA4);
                //w.Write(m_unknownA8);
                //w.Write(m_unknownAC);
                //w.Write(m_unknownB0);
                //w.Write(m_unknownB4);
                //w.Write(m_unknownB8);
                //w.Write((ushort) m_prefsControllerConfig);
                //w.Write(m_unknownBE);
                //w.Write(m_prefsDisableInvertLook ? 1U : 0U);
                //w.Write(m_prefsUseVibration);
                //w.Write(m_unknownC5);
                //w.Write(m_unknownC6);
                //w.Write(m_unknownC7);
                //w.Write(m_playerHasCheated ? 1U : 0U);
                //w.Write(_m_allTaxisHaveNitro ? 1U : 0U);
                //w.Write(m_targetIsOn ? 1U : 0U);
                //Serialize(m_targetPosition, stream);
                //w.Write(m_unknownDC);
                //Serialize(m_saveTime, stream);
            }

            return stream.Position - start;
        }
    }
}
