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

using LcsSaveEditor.Core;
using LcsSaveEditor.Models;
using LcsSaveEditor.Models.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace LcsSaveEditor.ViewModels
{
    public partial class GeneralViewModel : PageViewModel
    {
        public ICommand SelectGxtKeyCommand
        {
            get { return new RelayCommand(SelectGxtKey); }
        }

        private void SelectGxtKey()
        {
            MainViewModel.ShowGxtSelectionDialog(GxtSelectionDialog_ResultAction);
        }

        private void InitLists()
        {
            m_controlsList = new ListCollectionView(new List<ControllerConfig>()
            {
                ControllerConfig.Setup1,
                ControllerConfig.Setup2
            });

            m_onFootCameraList = new ListCollectionView(new List<PlayerCameraMode>()
            {
                PlayerCameraMode.Near,
                PlayerCameraMode.Middle,
                PlayerCameraMode.Far
            });

            m_inVehicleCameraList = new ListCollectionView(new List<VehicleCameraMode>()
            {
                VehicleCameraMode.Bumper,
                VehicleCameraMode.Near,
                VehicleCameraMode.Middle,
                VehicleCameraMode.Far,
                VehicleCameraMode.Fixed,
                VehicleCameraMode.Cinematic
            });

            m_languageList = new ListCollectionView(new List<Language>()
            {
                Language.English,
                Language.French,
                Language.German,
                Language.Italian,
                Language.Spanish,
                Language.Russian,
                Language.Japanese
            });

            m_radarModeList = new ListCollectionView(new List<RadarMode>()
            {
                RadarMode.RadarOff,
                RadarMode.BlipsOnly,
                RadarMode.MapAndBlips
            });

            m_weatherList = new ListCollectionView(WeatherCycle.WeatherList.ToList());

            m_forcedWeatherList = new ListCollectionView(new List<Weather>()
            {
                Weather.Sunny,
                Weather.Cloudy,
                Weather.Rainy,
                Weather.Foggy,
                Weather.ExtraSunny,
                Weather.Hurricane,
                Weather.ExtraColors,
                Weather.Snowy
            });
        }

        private void UpdateDisabledItemsBasedOnPlatform(GamePlatform platform)
        {
            switch (platform) {
                case GamePlatform.Android:
                case GamePlatform.IOS:
                    IsControlsSetEnabled = false;
                    IsModificationTimeEnabled = false;
                    IsVibrationEnabled = false;
                    IsWidescreenEnabled = false;
                    IsSwapAnalogAndDPadEnabled = false;
                    break;
                case GamePlatform.PS2:
                    IsControlsSetEnabled = true;
                    IsModificationTimeEnabled = true;
                    IsVibrationEnabled = true;
                    IsWidescreenEnabled = true;
                    IsSwapAnalogAndDPadEnabled = false;
                    break;
                case GamePlatform.PSP:
                    IsControlsSetEnabled = true;
                    IsModificationTimeEnabled = false;
                    IsVibrationEnabled = false;
                    IsWidescreenEnabled = false;
                    IsSwapAnalogAndDPadEnabled = true;
                    break;
            }
        }
    }
}
