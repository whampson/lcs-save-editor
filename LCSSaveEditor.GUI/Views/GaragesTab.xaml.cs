using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GTASaveData.Types;
using LCSSaveEditor.GUI.Controls;
using Xceed.Wpf.Toolkit.Core;

namespace LCSSaveEditor.GUI.Views
{
    /// <summary>
    /// Interaction logic for GaragesTab.xaml
    /// </summary>
    public partial class GaragesTab : TabPageBase<ViewModels.GaragesTab>
    {
        public GaragesTab()
        {
            InitializeComponent();
        }

        private void SafeHouseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.UpdateCurrentGarage();
        }

        private void Position_Changed(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedCar != null && e is PropertyChangedEventArgs<float> args)
            {
                Vector3D oldValue = ViewModel.SelectedCar.Position;
                Vector3D newValue = oldValue;

                switch (e.RoutedEvent.Name)
                {
                    case nameof(LocationPicker.XChanged): newValue = new Vector3D(args.NewValue, oldValue.Y, oldValue.Z); break;
                    case nameof(LocationPicker.YChanged): newValue = new Vector3D(oldValue.X, args.NewValue, oldValue.Z); break;
                    case nameof(LocationPicker.ZChanged): newValue = new Vector3D(oldValue.X, oldValue.Y, args.NewValue); break;
                }

                if (oldValue != newValue) ViewModel.SelectedCar.Position = newValue;
            }
        }

        private void Angle_Changed(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedCar != null && e is PropertyChangedEventArgs<float> args)
            {
                Vector3D oldValue = ViewModel.SelectedCar.Angle;
                Vector3D newValue = oldValue;

                switch (e.RoutedEvent.Name)
                {
                    case nameof(LocationPicker.XChanged): newValue = new Vector3D(args.NewValue, oldValue.Y, oldValue.Z); break;
                    case nameof(LocationPicker.YChanged): newValue = new Vector3D(oldValue.X, args.NewValue, oldValue.Z); break;
                    case nameof(LocationPicker.ZChanged): newValue = new Vector3D(oldValue.X, oldValue.Y, args.NewValue); break;
                }

                if (oldValue != newValue) ViewModel.SelectedCar.Angle = newValue;
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ViewModel.UpdateSlot();
        }
    }
}
