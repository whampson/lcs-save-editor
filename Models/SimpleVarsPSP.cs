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

using LcsSaveEditor.Models.DataTypes;
using System.IO;
using System.Text;

namespace LcsSaveEditor.Models
{
    /// <summary>
    /// Miscellaneous game variables formatted for the
    /// APlayStation Portable version of the game.
    /// </summary>
    public class SimpleVarsPSP : SimpleVars
    {
        private ushort m_unknown00;
        private ushort m_unknown02;
        private ushort m_unknown04;
        private ushort m_unknown06;
        private ushort m_unknown3E;
        private uint m_unknown5C;
        private uint m_unknown60;
        private float m_unknown64;
        private ushort m_unknown6E;
        private byte m_unknown75;
        private byte m_unknown76;
        private byte m_unknown77;
        private ushort m_unknown82;
        private uint m_unknown84;
        private uint m_unknown88;
        private uint m_unknown8C;
        private uint m_unknown90;
        private uint m_unknown94;
        private uint m_unknown98;
        private uint m_unknown9C;
        private uint m_unknownA0;
        private uint m_unknownA4;
        private uint m_unknownA8;
        private byte m_unknownB3;

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_unknown00 = r.ReadUInt16();
                m_unknown02 = r.ReadUInt16();
                m_unknown04 = r.ReadUInt16();
                m_unknown06 = r.ReadUInt16();
                _m_currLevel = r.ReadUInt32();
                _m_currArea = r.ReadUInt32();
                m_prefsLanguage = (Language) r.ReadUInt32();
                m_millisecondsPerGameMinute = r.ReadUInt32();
                m_lastClockTick = r.ReadUInt32();
                m_gameTimeHours = r.ReadByte();
                m_gameTimeMinutes = r.ReadByte();
                _m_gameClockSeconds = r.ReadUInt16();
                m_totalTimePlayedInMilliseconds = r.ReadUInt32();
                _m_timeScale = r.ReadSingle();
                _m_timeStep = r.ReadSingle();
                _m_timeStepNonClipped = r.ReadSingle();
                _m_framesPerUpdate = r.ReadSingle();
                m_frameCounter = r.ReadUInt32();
                m_prevWeatherType = (Weather) r.ReadInt16();
                m_currWeatherType = (Weather) r.ReadInt16();
                short forced = r.ReadInt16();
                m_forcedWeatherType = (forced != -1) ? (Weather?) forced : null;
                m_unknown3E = r.ReadUInt16();
                m_weatherTypeInList = r.ReadUInt32();
                _m_interpolationValue = r.ReadSingle();
                CameraPosition = Deserialize<Vector3d>(stream);
                m_prefsVehicleCameraMode = (VehicleCameraMode) r.ReadSingle();
                m_prefsPlayerCameraMode = (PlayerCameraMode) r.ReadSingle();
                m_unknown5C = r.ReadUInt32();
                m_unknown60 = r.ReadUInt32();
                m_unknown64 = r.ReadSingle();
                m_prefsBrightness = r.ReadUInt32();
                m_prefsDisplayHud = r.ReadBoolean();
                m_prefsShowSubtitles = r.ReadBoolean();
                m_unknown6E = r.ReadUInt16();
                m_prefsRadarMode = (RadarMode) r.ReadUInt32();
                m_blurOn = r.ReadBoolean();
                m_unknown75 = r.ReadByte();
                m_unknown76 = r.ReadByte();
                m_unknown77 = r.ReadByte();
                m_prefsRadioVolume = r.ReadUInt32();
                m_prefsSfxVolume = r.ReadUInt32();
                _m_prefsRadioStation = (RadioStation) r.ReadByte();
                _m_prefsOutput = r.ReadByte();
                m_unknown82 = r.ReadUInt16();
                m_unknown84 = r.ReadUInt32();
                m_unknown88 = r.ReadUInt32();
                m_unknown8C = r.ReadUInt32();
                m_unknown90 = r.ReadUInt32();
                m_unknown94 = r.ReadUInt32();
                m_unknown98 = r.ReadUInt32();
                m_unknown9C = r.ReadUInt32();
                m_unknownA0 = r.ReadUInt32();
                m_unknownA4 = r.ReadUInt32();
                m_unknownA8 = r.ReadUInt32();
                ushort ctrl = r.ReadUInt16();
                // Yeah, this is goofy on PSP: values 0 and 1 are setup2, values 2 and 3 are setup1
                m_prefsControllerConfig = ((ctrl / 2) == 0) ? ControllerConfig.Setup2 : ControllerConfig.Setup1;
                m_prefsDisableInvertLook = r.ReadBoolean();
                m_prefsSwapNippleAndDPad = r.ReadBoolean();
                m_hasPlayerCheated = r.ReadBoolean();
                m_allTaxisHaveNitro = r.ReadBoolean();
                m_targetIsOn = r.ReadBoolean();
                m_unknownB3 = r.ReadByte();
                WaypointPosition = Deserialize<Vector2d>(stream);
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write(m_unknown00);
                w.Write(m_unknown02);
                w.Write(m_unknown04);
                w.Write(m_unknown06);
                w.Write(_m_currLevel);
                w.Write(_m_currArea);
                w.Write((uint) m_prefsLanguage);
                w.Write(m_millisecondsPerGameMinute);
                w.Write(m_lastClockTick);
                w.Write(m_gameTimeHours);
                w.Write(m_gameTimeMinutes);
                w.Write(_m_gameClockSeconds);
                w.Write(m_totalTimePlayedInMilliseconds);
                w.Write(_m_timeScale);
                w.Write(_m_timeStep);
                w.Write(_m_timeStepNonClipped);
                w.Write(_m_framesPerUpdate);
                w.Write(m_frameCounter);
                w.Write((short) m_prevWeatherType);
                w.Write((short) m_currWeatherType);
                w.Write((short?) m_forcedWeatherType ?? -1);
                w.Write(m_unknown3E);
                w.Write(m_weatherTypeInList);
                w.Write(_m_interpolationValue);
                Serialize(m_cameraPosition, stream);
                w.Write((float) m_prefsVehicleCameraMode);
                w.Write((float) m_prefsPlayerCameraMode);
                w.Write(m_unknown5C);
                w.Write(m_unknown60);
                w.Write(m_unknown64);
                w.Write(m_prefsBrightness);
                w.Write(m_prefsDisplayHud);
                w.Write(m_prefsShowSubtitles);
                w.Write(m_unknown6E);
                w.Write((uint) m_prefsRadarMode);
                w.Write(m_blurOn);
                w.Write(m_unknown75);
                w.Write(m_unknown76);
                w.Write(m_unknown77);
                w.Write(m_prefsRadioVolume);
                w.Write(m_prefsSfxVolume);
                w.Write((byte) _m_prefsRadioStation);
                w.Write(_m_prefsOutput);
                w.Write(m_unknown82);
                w.Write(m_unknown84);
                w.Write(m_unknown88);
                w.Write(m_unknown8C);
                w.Write(m_unknown90);
                w.Write(m_unknown94);
                w.Write(m_unknown98);
                w.Write(m_unknown9C);
                w.Write(m_unknownA0);
                w.Write(m_unknownA4);
                w.Write(m_unknownA8);
                // Again... goofy. See the comment in DeserializeObject() for an explanation.
                if (m_prefsControllerConfig == ControllerConfig.Setup2) {
                    w.Write((ushort) 0);
                }
                else {
                    w.Write((ushort) 2);
                }
                w.Write(m_prefsDisableInvertLook);
                w.Write(m_prefsSwapNippleAndDPad);
                w.Write(m_hasPlayerCheated);
                w.Write(m_allTaxisHaveNitro);
                w.Write(m_targetIsOn);
                w.Write(m_unknownB3);
                Serialize(m_targetPosition, stream);
            }

            return stream.Position - start;
        }
    }
}
