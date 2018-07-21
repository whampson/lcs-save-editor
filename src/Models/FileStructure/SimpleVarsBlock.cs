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

namespace WHampson.LcsSaveEditor.Models.FileStructure
{
    public class SimpleVarsBlock : DataBlock
    {
        private readonly Primitive<uint> currentLevel;
        private readonly Primitive<uint> currentArea;
        private readonly Primitive<uint> prefsLanguage;
        private readonly Primitive<uint> millisecondsPerGameMinute;
        private readonly Primitive<uint> lastClockTick;
        private readonly Primitive<byte> gameClockHours;
        private readonly Primitive<byte> gameClockMinutes;
        private readonly Primitive<uint> totalTimePlayedInMilliseconds;
        private readonly Primitive<uint> frameCounter;
        private readonly Primitive<short> previousWeather;
        private readonly Primitive<short> currentWeather;
        private readonly Primitive<short> forcedWeather;
        private readonly Primitive<uint> weatherIndex;
        private readonly Primitive<float> weatherInterpolation;
        private readonly Vector3d cameraPosition;
        private readonly Primitive<float> prefsVehicleCameraMode;
        private readonly Primitive<uint> prefsBrightness;
        private readonly Primitive<bool> prefsShowHud;
        private readonly Primitive<bool> prefsShowSubtitles;
        private readonly Primitive<uint> radarMode;
        private readonly Primitive<Bool32> blurOn;
        private readonly Primitive<bool> prefsUseWideScreen;
        private readonly Primitive<uint> prefsRadioVolume;
        private readonly Primitive<uint> prefsSfxVolume;
        private readonly Primitive<ushort> controllerConfiguration;
        private readonly Primitive<Bool32> prefsDisableInvertLook;
        private readonly Primitive<bool> prefsUseVibration;
        private readonly Primitive<Bool32> hasPlayerCheated;
        private readonly Primitive<Bool32> targetIsOn;
        private readonly Vector2d targetPosition;
        private readonly Timestamp timestamp;

        public SimpleVarsBlock()
        {
            currentLevel = new Primitive<uint>(null, 0);
            currentArea = new Primitive<uint>(null, 0);
            prefsLanguage = new Primitive<uint>(null, 0);
            millisecondsPerGameMinute = new Primitive<uint>(null, 0);
            lastClockTick = new Primitive<uint>(null, 0);
            gameClockHours = new Primitive<byte>(null, 0);
            gameClockMinutes = new Primitive<byte>(null, 0);
            totalTimePlayedInMilliseconds = new Primitive<uint>(null, 0);
            frameCounter = new Primitive<uint>(null, 0);
            previousWeather = new Primitive<short>(null, 0);
            currentWeather = new Primitive<short>(null, 0);
            forcedWeather = new Primitive<short>(null, 0);
            weatherIndex = new Primitive<uint>(null, 0);
            weatherInterpolation = new Primitive<float>(null, 0);
            cameraPosition = new Vector3d();
            prefsVehicleCameraMode = new Primitive<float>(null, 0);
            prefsBrightness = new Primitive<uint>(null, 0);
            prefsShowHud = new Primitive<bool>(null, 0);
            prefsShowSubtitles = new Primitive<bool>(null, 0);
            radarMode = new Primitive<uint>(null, 0);
            blurOn = new Primitive<Bool32>(null, 0);
            prefsUseWideScreen = new Primitive<bool>(null, 0);
            prefsRadioVolume = new Primitive<uint>(null, 0);
            prefsSfxVolume = new Primitive<uint>(null, 0);
            controllerConfiguration = new Primitive<ushort>(null, 0);
            prefsDisableInvertLook = new Primitive<Bool32>(null, 0);
            prefsUseVibration = new Primitive<bool>(null, 0);
            hasPlayerCheated = new Primitive<Bool32>(null, 0);
            targetIsOn = new Primitive<Bool32>(null, 0);
            targetPosition = new Vector2d();
            timestamp = new Timestamp();
        }

        public uint CurrentLevel
        {
            get { return currentLevel.Value; }
            set { currentLevel.Value = value; }
        }

        public uint CurrentArea
        {
            get { return currentArea.Value; }
            set { currentArea.Value = value; }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public Language PrefsLanguage
        {
            get { return (Language) prefsLanguage.Value; }
            set { prefsLanguage.Value = (uint) value; }
        }

        public uint MillisecondsPerGameMinute
        {
            get { return millisecondsPerGameMinute.Value; }
            set { millisecondsPerGameMinute.Value = value; }
        }

        public uint LastClockTick
        {
            get { return lastClockTick.Value; }
            set { lastClockTick.Value = value; }
        }

        public byte GameClockHours
        {
            get { return gameClockHours.Value; }
            set { gameClockHours.Value = value; }
        }

        public byte GameClockMinutes
        {
            get { return gameClockMinutes.Value; }
            set { gameClockMinutes.Value = value; }
        }

        public uint TotalTimePlayedInMilliseconds
        {
            get { return totalTimePlayedInMilliseconds.Value; }
            set { totalTimePlayedInMilliseconds.Value = value; }
        }

        public uint FrameCounter
        {
            get { return frameCounter.Value; }
            set { frameCounter.Value = value; }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public WeatherType PreviousWeather
        {
            get { return (WeatherType) previousWeather.Value; }
            set { previousWeather.Value = (short) value; }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public WeatherType CurrentWeather
        {
            get { return (WeatherType) currentWeather.Value; }
            set { currentWeather.Value = (short) value; }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public WeatherType ForcedWeather
        {
            get { return (WeatherType) forcedWeather.Value; }
            set { forcedWeather.Value = (short) value; }
        }

        public uint WeatherIndex
        {
            get { return weatherIndex.Value; }
            set { weatherIndex.Value = value; }
        }

        public float WeatherInterpolation
        {
            get { return weatherInterpolation.Value; }
            set { weatherInterpolation.Value = value; }
        }

        public Vector3d CameraPosition
        {
            get { return cameraPosition; }
        }

        public float PrefsVehicleCameraMode
        {
            get { return prefsVehicleCameraMode.Value; }
            set { prefsVehicleCameraMode.Value = value; }
        }

        public uint PrefsBrightness
        {
            get { return prefsBrightness.Value; }
            set { prefsBrightness.Value = value; }
        }

        public bool PrefsShowHud
        {
            get { return prefsShowHud.Value; }
            set { prefsShowHud.Value = value; }
        }

        public bool PrefsShowSubtitles
        {
            get { return prefsShowSubtitles.Value; }
            set { prefsShowSubtitles.Value = value; }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public RadarMode RadarMode
        {
            get { return (RadarMode) radarMode.Value; }
            set { radarMode.Value = (uint) value; }
        }

        public Bool32 BlurOn
        {
            get { return blurOn.Value; }
            set { blurOn.Value = value; }
        }

        public bool PrefsUseWideScreen
        {
            get { return prefsUseWideScreen.Value; }
            set { prefsUseWideScreen.Value = value; }
        }

        public uint PrefsRadioVolume
        {
            get { return prefsRadioVolume.Value; }
            set { prefsRadioVolume.Value = value; }
        }

        public uint PrefsSfxVolume
        {
            get { return prefsSfxVolume.Value; }
            set { prefsSfxVolume.Value = value; }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public ControllerConfig ControllerConfiguration
        {
            get { return (ControllerConfig) controllerConfiguration.Value; }
            set { controllerConfiguration.Value = (ushort) value; }
        }

        public Bool32 PrefsDisableInvertLook
        {
            get { return prefsDisableInvertLook.Value; }
            set { prefsDisableInvertLook.Value = value; }
        }

        public bool PrefsUseVibration
        {
            get { return prefsUseVibration.Value; }
            set { prefsUseVibration.Value = value; }
        }

        public Bool32 HasPlayerCheated
        {
            get { return hasPlayerCheated.Value; }
            set { hasPlayerCheated.Value = value; }
        }

        public Bool32 TargetIsOn
        {
            get { return targetIsOn.Value; }
            set { targetIsOn.Value = value; }
        }

        public Vector2d TargetPosition
        {
            get { return targetPosition; }
        }

        public Timestamp Timestamp
        {
            get { return timestamp; }
        }
    }
}
