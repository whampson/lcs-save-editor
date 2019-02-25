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

using LcsSaveEditor.Models;
using LcsSaveEditor.Resources;
using System.Windows.Data;

namespace LcsSaveEditor.ViewModels
{
    public partial class GeneralViewModel : PageViewModel
    {
        private const int PS2PSPBrightnessMinValue = 128;
        private const int PS2PSPBrightnessMaxValue = 384;
        private const int AndroidIOSBrightnessMinValue = 180;
        private const int AndroidIOSBrightnessMaxValue = 700;

        private SimpleVars m_simpleVars;
        private Stats m_stats;

        private int m_brightnessMinValue;
        private int m_brightnessMaxValue;

        private ListCollectionView m_controlsList;
        private ListCollectionView m_onFootCameraList;
        private ListCollectionView m_inVehicleCameraList;
        private ListCollectionView m_languageList;
        private ListCollectionView m_radarModeList;
        private ListCollectionView m_weatherList;
        private ListCollectionView m_forcedWeatherList;

        public GeneralViewModel(MainViewModel mainViewModel)
            : base(FrontendResources.Main_Page_General, PageVisibility.WhenFileLoaded, mainViewModel)
        {
            InitLists();

            MainViewModel.DataClosing += MainViewModel_DataClosing;
            MainViewModel.DataLoaded += MainViewModel_DataLoaded;
        }

        public SimpleVars SimpleVars
        {
            get { return m_simpleVars; }
            set { m_simpleVars = value; OnPropertyChanged(); }
        }

        public Stats Stats
        {
            get { return m_stats; }
            set { m_stats = value; OnPropertyChanged(); }
        }

        public int BrightnessMinValue
        {
            get { return m_brightnessMinValue; }
            set { m_brightnessMinValue = value; OnPropertyChanged(); }
        }

        public int BrightnessMaxValue
        {
            get { return m_brightnessMaxValue; }
            set { m_brightnessMaxValue = value; OnPropertyChanged(); }
        }

        public ListCollectionView ControlsList
        {
            get { return m_controlsList; }
            set { m_controlsList = value; OnPropertyChanged(); }
        }

        public ListCollectionView OnFootCameraList
        {
            get { return m_onFootCameraList; }
            set { m_onFootCameraList = value; OnPropertyChanged(); }
        }

        public ListCollectionView InVehicleCameraList
        {
            get { return m_inVehicleCameraList; }
            set { m_inVehicleCameraList = value; OnPropertyChanged(); }
        }

        public ListCollectionView LanguageList
        {
            get { return m_languageList; }
            set { m_languageList = value; OnPropertyChanged(); }
        }

        public ListCollectionView RadarModeList
        {
            get { return m_radarModeList; }
            set { m_radarModeList = value; OnPropertyChanged(); }
        }

        public ListCollectionView WeatherList
        {
            get { return m_weatherList; }
            set { m_weatherList = value; OnPropertyChanged(); }
        }

        public ListCollectionView ForcedWeatherList
        {
            get { return m_forcedWeatherList; }
            set { m_forcedWeatherList = value; OnPropertyChanged(); }
        }
    }
}
