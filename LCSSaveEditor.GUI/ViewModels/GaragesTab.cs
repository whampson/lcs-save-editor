using System.Collections.Generic;
using System.Linq;
using GTASaveData.LCS;
using LCSSaveEditor.GUI.Types;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class GaragesTab : TabPageBase
    {
        private GarageData m_garageData;
        private List<StoredCar> m_garageContents;
        private ZoneLevel m_selectedSafeHouse;
        private StoredCar m_selectedCar;

        public GarageData GarageData
        {
            get { return m_garageData; }
            set { m_garageData = value; OnPropertyChanged(); }
        }

        public List<StoredCar> GarageContents
        {
            get { return m_garageContents; }
            set { m_garageContents = value; OnPropertyChanged(); }
        }

        public ZoneLevel SelectedSafeHouse
        {
            get { return m_selectedSafeHouse; }
            set { m_selectedSafeHouse = value; OnPropertyChanged(); }
        }

        public StoredCar SelectedCar
        {
            get { return m_selectedCar; }
            set { m_selectedCar = value; OnPropertyChanged(); }
        }

        public GaragesTab(MainWindow window)
            : base("Garages", TabPageVisibility.WhenFileIsOpen, window)
        {
            GarageContents = new List<StoredCar>();
            SelectedSafeHouse = ZoneLevel.Industrial;
        }

        public override void Load()
        {
            base.Load();
            GarageData = TheWindow.TheSave.Garages;
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void Update()
        {
            base.Update();
            UpdateCurrentGarage();
        }

        public void UpdateCurrentGarage()
        {
            switch (SelectedSafeHouse)
            {
                case ZoneLevel.Industrial: GarageContents = GarageData.StoredCarsPortland.ToList(); break;
                case ZoneLevel.Commercial: GarageContents = GarageData.StoredCarsStaunton.ToList(); break;
                case ZoneLevel.Suburban: GarageContents = GarageData.StoredCarsShoreside.ToList(); break;
            }
        }

    }
}
