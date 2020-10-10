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
        private bool m_isProblematic;

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

        public bool IsProblematicVehicle
        {
            get { return m_isProblematic; }
            set { m_isProblematic = value; OnPropertyChanged(); }
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

        public void CheckProblematicVehicle()
        {
            IsProblematicVehicle = SelectedCar != null &&
               (SelectedCar.Model == (int) Vehicle.FERRY ||
                SelectedCar.Model == (int) Vehicle.TRAIN ||
                SelectedCar.Model == (int) Vehicle.ESCAPE ||
                SelectedCar.Model == (int) Vehicle.CHOPPER ||
                SelectedCar.Model == (int) Vehicle.AIRTRAIN ||
                SelectedCar.Model == (int) Vehicle.DEADDODO ||
                SelectedCar.Model == (int) Vehicle.RCBANDIT ||
                SelectedCar.Model == (int) Vehicle.RCGOBLIN ||
                SelectedCar.Model == (int) Vehicle.RCRAIDER);
        }
    }
}
