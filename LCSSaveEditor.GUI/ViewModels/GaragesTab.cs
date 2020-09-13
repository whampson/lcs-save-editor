using System.Collections.Generic;
using System.Linq;
using GTASaveData.LCS;
using LCSSaveEditor.GUI.Types;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class GaragesTab : TabPageBase
    {
        private List<StoredCar> m_garageContents;
        private ZoneLevel m_selectedSafeHouse;
        private StoredCar m_selectedCar;

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

        public override void Update()
        {
            base.Update();
            UpdateCurrentGarage();
        }

        public void UpdateCurrentGarage()
        {
            switch (SelectedSafeHouse)
            {
                case ZoneLevel.Industrial: GarageContents = Garages.StoredCarsPortland.ToList(); break;
                case ZoneLevel.Commercial: GarageContents = Garages.StoredCarsStaunton.ToList(); break;
                case ZoneLevel.Suburban: GarageContents = Garages.StoredCarsShoreside.ToList(); break;
            }
        }
    }
}
