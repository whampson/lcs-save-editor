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

using System.ComponentModel;
using LcsSaveEditor.Core;
using LcsSaveEditor.Models.DataTypes;

namespace LcsSaveEditor.Models
{
    /// <summary>
    /// Miscellaneous game variables.
    /// Theses are stored in block 0 of the save data file.
    /// </summary>
    public abstract class SimpleVars : SerializableObject
    {
        protected string m_saveNameGxt;
        protected uint _m_currLevel;
        protected uint _m_currArea;
        protected Language m_prefsLanguage;
        protected uint m_millisecondsPerGameMinute;
        protected uint m_lastClockTick;
        protected byte m_gameTimeHours;
        protected byte m_gameTimeMinutes;
        protected ushort _m_gameClockSeconds;
        protected uint m_totalTimePlayedInMilliseconds;
        protected float _m_timeScale;
        protected float _m_timeStep;
        protected float _m_timeStepNonClipped;
        protected float _m_framesPerUpdate;
        protected uint m_frameCounter;
        protected Weather m_prevWeatherType;
        protected Weather m_currWeatherType;
        protected Weather m_forcedWeatherType;
        protected uint m_weatherTypeInList;
        protected float _m_interpolationValue;
        protected Vector3d m_cameraPosition;
        protected VehicleCameraMode m_prefsVehicleCameraMode;
        protected PlayerCameraMode m_prefsPlayerCameraMode;
        protected uint m_prefsBrightness;
        protected bool m_prefsDisplayHud;
        protected bool m_prefsShowSubtitles;
        protected RadarMode m_prefsRadarMode;
        protected bool m_blurOn;
        protected bool m_prefsUseWideScreen;
        protected uint m_prefsRadioVolume;
        protected uint m_prefsSfxVolume;
        protected RadioStation _m_prefsRadioStation;
        protected byte _m_prefsOutput;
        protected ControllerConfig m_prefsControllerConfig;
        protected bool m_prefsDisableInvertLook;
        protected bool m_prefsInvertLook;
        protected bool m_prefsUseVibration;
        protected bool m_prefsSwapNippleAndDPad;
        protected bool m_hasPlayerCheated;
        protected bool m_allTaxisHaveNitro;
        protected bool m_targetIsOn;
        protected Vector2d m_targetPosition;
        protected Timestamp m_saveTime;

        public SimpleVars()
        {
            m_prefsDisableInvertLook = true;
        }

        public string SaveNameGxt
        {
            get { return m_saveNameGxt; }
            set { m_saveNameGxt = value; OnPropertyChanged(); }
        }

        public Language Language
        {
            get { return m_prefsLanguage; }
            set { m_prefsLanguage = value; OnPropertyChanged(); }
        }

        public uint MillisecondsPerGameMinute
        {
            get { return m_millisecondsPerGameMinute; }
            set { m_millisecondsPerGameMinute = value; OnPropertyChanged(); }
        }

        public uint LastClockTick
        {
            get { return m_lastClockTick; }
            set { m_lastClockTick = value; OnPropertyChanged(); }
        }

        public byte GameTimeHours
        {
            get { return m_gameTimeHours; }
            set { m_gameTimeHours = value; OnPropertyChanged(); }
        }

        public byte GameTimeMinutes
        {
            get { return m_gameTimeMinutes; }
            set { m_gameTimeMinutes = value; OnPropertyChanged(); }
        }

        public uint TotalTimePlayedInMilliseconds
        {
            get { return m_totalTimePlayedInMilliseconds; }
            set { m_totalTimePlayedInMilliseconds = value; OnPropertyChanged(); }
        }

        public uint FrameCounter
        {
            get { return m_frameCounter; }
            set { m_frameCounter = value; OnPropertyChanged(); }
        }

        public Weather PreviousWeather
        {
            get { return m_prevWeatherType; }
            set { m_prevWeatherType = value; OnPropertyChanged(); }
        }

        public Weather CurrentWeather
        {
            get { return m_currWeatherType; }
            set { m_currWeatherType = value; OnPropertyChanged(); }
        }

        public Weather ForcedWeather
        {
            get { return m_forcedWeatherType; }
            set { m_forcedWeatherType = value; OnPropertyChanged(); }
        }

        public uint WeatherIndex
        {
            get { return m_weatherTypeInList; }
            set { m_weatherTypeInList = value; OnPropertyChanged(); }
        }

        public Vector3d CameraPosition
        {
            get { return m_cameraPosition; }
            set {
                if (m_cameraPosition != null) {
                    m_cameraPosition.PropertyChanged -= CameraPosition_PropertyChanged;
                }
                m_cameraPosition = value;
                m_cameraPosition.PropertyChanged += CameraPosition_PropertyChanged;
                OnPropertyChanged();
            }
        }

        public VehicleCameraMode VehicleCamera
        {
            get { return m_prefsVehicleCameraMode; }
            set { m_prefsVehicleCameraMode = value; OnPropertyChanged(); }
        }

        public PlayerCameraMode PlayerCamera
        {
            get { return m_prefsPlayerCameraMode; }
            set { m_prefsPlayerCameraMode = value; OnPropertyChanged(); }
        }

        public uint Brightness
        {
            get { return m_prefsBrightness; }
            set { m_prefsBrightness = value; OnPropertyChanged(); }
        }

        public bool ShowHud
        {
            get { return m_prefsDisplayHud; }
            set { m_prefsDisplayHud = value; OnPropertyChanged(); }
        }

        public bool ShowSubtitles
        {
            get { return m_prefsShowSubtitles; }
            set { m_prefsShowSubtitles = value; OnPropertyChanged(); }
        }

        public RadarMode RadarMode
        {
            get { return m_prefsRadarMode; }
            set { m_prefsRadarMode = value; OnPropertyChanged(); }
        }

        public bool EnhancedColors
        {
            get { return !m_blurOn; }
            set { m_blurOn = !value; OnPropertyChanged(); }
        }

        public bool WidescreenEnabled
        {
            get { return m_prefsUseWideScreen; }
            set { m_prefsUseWideScreen = value; OnPropertyChanged(); }
        }

        public uint RadioVolume
        {
            get { return m_prefsRadioVolume; }
            set { m_prefsRadioVolume = value; OnPropertyChanged(); }
        }

        public uint SfxVolume
        {
            get { return m_prefsSfxVolume; }
            set { m_prefsSfxVolume = value; OnPropertyChanged(); }
        }

        public ControllerConfig ControllerConfiguration
        {
            get { return m_prefsControllerConfig; }
            set { m_prefsControllerConfig = value; OnPropertyChanged(); }
        }

        public bool InvertLook
        {
            get { return m_prefsInvertLook || !m_prefsDisableInvertLook; }
            set {
                m_prefsInvertLook = value;
                m_prefsDisableInvertLook = !value;
                OnPropertyChanged();
            }
        }

        public bool VibrationEnabled
        {
            get { return m_prefsUseVibration; }
            set { m_prefsUseVibration = value; OnPropertyChanged(); }
        }

        public bool SwapAnalogAndDPad
        {
            get { return m_prefsSwapNippleAndDPad; }
            set { m_prefsSwapNippleAndDPad = value; OnPropertyChanged(); }
        }

        public bool HasPlayerCheated
        {
            get { return m_hasPlayerCheated; }
            set { m_hasPlayerCheated = value; OnPropertyChanged(); }
        }

        public bool TaxiBounceEnabled
        {
            get { return m_allTaxisHaveNitro; }
            set { m_allTaxisHaveNitro = value; OnPropertyChanged(); }
        }

        public bool ShowWaypoint
        {
            get { return m_targetIsOn; }
            set { m_targetIsOn = value; OnPropertyChanged(); }
        }

        public Vector2d WaypointPosition
        {
            get { return m_targetPosition; }
            set {
                if (m_targetPosition != null) {
                    m_targetPosition.PropertyChanged -= WaypointPosition_PropertyChanged;
                }
                m_targetPosition = value;
                m_targetPosition.PropertyChanged += WaypointPosition_PropertyChanged;
                OnPropertyChanged();
            }
        }

        public Timestamp Timestamp
        {
            get { return m_saveTime; }
            set {
                if (m_saveTime != null) {
                    m_saveTime.PropertyChanged -= Timestamp_PropertyChanged;
                }
                m_saveTime = value;
                m_saveTime.PropertyChanged += Timestamp_PropertyChanged;
                OnPropertyChanged();
            }
        }

        private void CameraPosition_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CameraPosition));
        }

        private void WaypointPosition_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(WaypointPosition));
        }

        private void Timestamp_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Timestamp));
        }
    }
}
