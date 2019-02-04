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
    public class SimpleVarsAndroidIOS : SimpleVars
    {
        private uint m_unknown00;
        private uint m_unknown04;
        private uint m_unknown08;
        private uint[] m_unknown1C;
        private ushort m_unknown7C;
        private ushort m_unknown7E;
        private ushort m_unknown80;
        private ushort m_unknown82;
        private float m_unknownD4;
        private uint m_unknownD8;
        private uint m_unknownDC;
        private float m_unknownE0;
        private uint m_unknown100;
        private uint m_unknown104;
        private uint m_unknown108;
        private uint m_unknown10C;
        private uint m_unknown110;
        private uint m_unknown114;
        private uint m_unknown118;
        private uint m_unknown11C;
        private uint m_unknown120;
        private uint m_unknown124;
        private float m_unknown128;
        private ushort m_unknown12C;

        public SimpleVarsAndroidIOS()
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
                m_saveNameGxt = r.ReadWideString(8);
                for (int i = 0; i < m_unknown1C.Length; i++) { m_unknown1C[i] = r.ReadUInt32(); }
                m_unknown7C = r.ReadUInt16();
                m_unknown7E = r.ReadUInt16();
                m_unknown80 = r.ReadUInt16();
                m_unknown82 = r.ReadUInt16();
                _m_currLevel = r.ReadUInt32();
                _m_currArea = r.ReadUInt32();
                m_prefsLanguage = (Language) r.ReadUInt32();
                m_millisecondsPerGameMinute = r.ReadUInt32();
                m_lastClockTick = r.ReadUInt32();
                m_gameClockHours = r.ReadByte();
                m_gameClockMinutes = r.ReadByte();
                _m_gameClockSeconds = r.ReadUInt16();
                m_totalTimePlayedInMilliseconds = r.ReadUInt32();
                _m_timeScale = r.ReadSingle();
                _m_timeStep = r.ReadSingle();
                _m_timeStepNonClipped = r.ReadSingle();
                _m_framesPerUpdate = r.ReadSingle();
                _m_frameCounter = r.ReadUInt32();
                m_prevWeatherType = (Weather) r.ReadInt16();
                m_currWeatherType = (Weather) r.ReadInt16();
                m_forcedWeatherType = (Weather) r.ReadInt16();
                r.ReadBytes(2);     // align bytes
                m_weatherTypeInList = r.ReadUInt32();
                _m_interpolationValue = r.ReadSingle();
                m_cameraPosition = Deserialize<Vector3d>(stream);
                m_prefsVehicleCameraMode = (VehicleCameraMode) r.ReadSingle();
                m_unknownD4 = r.ReadSingle();
                m_unknownD8 = r.ReadUInt32();
                m_unknownDC = r.ReadUInt32();
                m_unknownE0 = r.ReadSingle();
                m_prefsBrightness = r.ReadUInt32();
                m_prefsDisplayHud = r.ReadBoolean();
                m_prefsShowSubtitles = r.ReadBoolean();
                r.ReadBytes(2);     // align bytes
                m_prefsRadarMode = (RadarMode) r.ReadUInt32();
                m_blurOn = r.ReadBoolean();
                r.ReadBytes(3);     // align bytes
                m_prefsRadioVolume = r.ReadUInt32();
                m_prefsSfxVolume = r.ReadUInt32();
                _m_prefsRadioStation = (RadioStation) r.ReadByte();
                _m_prefsOutput = r.ReadByte();
                r.ReadBytes(2);     // align bytes
                m_unknown100 = r.ReadUInt32();
                m_unknown104 = r.ReadUInt32();
                m_unknown108 = r.ReadUInt32();
                m_unknown10C = r.ReadUInt32();
                m_unknown110 = r.ReadUInt32();
                m_unknown114 = r.ReadUInt32();
                m_unknown118 = r.ReadUInt32();
                m_unknown11C = r.ReadUInt32();
                m_unknown120 = r.ReadUInt32();
                m_unknown124 = r.ReadUInt32();
                m_unknown128 = r.ReadSingle();
                m_unknown12C = r.ReadUInt16();
                m_prefsInvertLook = r.ReadBoolean();
                _m_prefsSwapNippleAndDPad = r.ReadBoolean();
                m_playerHasCheated = r.ReadBoolean();
                _m_allTaxisHaveNitro = r.ReadBoolean();
                m_targetIsOn = r.ReadBoolean();
                r.ReadBytes(1);     // align byte
                m_targetPosition = Deserialize<Vector2d>(stream);
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write(m_unknown00);
                w.Write(m_unknown04);
                w.Write(m_unknown08);
                w.WriteWideString(m_saveNameGxt, 8);
                for (int i = 0; i < m_unknown1C.Length; i++) { w.Write(m_unknown1C[i]); }
                w.Write(m_unknown7C);
                w.Write(m_unknown7E);
                w.Write(m_unknown80);
                w.Write(m_unknown82);
                w.Write(_m_currLevel);
                w.Write(_m_currArea);
                w.Write((uint) m_prefsLanguage);
                w.Write(m_millisecondsPerGameMinute);
                w.Write(m_lastClockTick);
                w.Write(m_gameClockHours);
                w.Write(m_gameClockMinutes);
                w.Write(_m_gameClockSeconds);
                w.Write(m_totalTimePlayedInMilliseconds);
                w.Write(_m_timeScale);
                w.Write(_m_timeStep);
                w.Write(_m_timeStepNonClipped);
                w.Write(_m_framesPerUpdate);
                w.Write(_m_frameCounter);
                w.Write((short) m_prevWeatherType);
                w.Write((short) m_currWeatherType);
                w.Write((short) m_forcedWeatherType);
                w.Write(new byte[2]);       // align bytes
                w.Write(m_weatherTypeInList);
                w.Write(_m_interpolationValue);
                Serialize(m_cameraPosition, stream);
                w.Write((float) m_prefsVehicleCameraMode);
                w.Write(m_unknownD4);
                w.Write(m_unknownD8);
                w.Write(m_unknownDC);
                w.Write(m_unknownE0);
                w.Write(m_prefsBrightness);
                w.Write(m_prefsDisplayHud);
                w.Write(m_prefsShowSubtitles);
                w.Write(new byte[2]);       // align bytes
                w.Write((uint) m_prefsRadarMode);
                w.Write(m_blurOn);
                w.Write(new byte[3]);       // align bytes
                w.Write(m_prefsRadioVolume);
                w.Write(m_prefsSfxVolume);
                w.Write((byte) _m_prefsRadioStation);
                w.Write(_m_prefsOutput);
                w.Write(new byte[2]);       // align bytes
                w.Write(m_unknown100);
                w.Write(m_unknown104);
                w.Write(m_unknown108);
                w.Write(m_unknown10C);
                w.Write(m_unknown110);
                w.Write(m_unknown114);
                w.Write(m_unknown118);
                w.Write(m_unknown11C);
                w.Write(m_unknown120);
                w.Write(m_unknown124);
                w.Write(m_unknown128);
                w.Write(m_unknown12C);
                w.Write(m_prefsInvertLook);
                w.Write(_m_prefsSwapNippleAndDPad);
                w.Write(m_playerHasCheated);
                w.Write(_m_allTaxisHaveNitro);
                w.Write(m_targetIsOn);
                w.Write(new byte[1]);       // align byte
                Serialize(m_targetPosition, stream);
            }

            return stream.Position - start;
        }
    }
}
