using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LCSSaveEditor.GUI.ViewModels;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.Views
{
    /// <summary>
    /// Interaction logic for MapWindow.xaml
    /// </summary>
    public partial class MapWindow : ChildWindowBase
    {
        public new ViewModels.MapWindow ViewModel
        {
            get { return (ViewModels.MapWindow) DataContext; }
            set { DataContext = value; }
        }

        public MapWindow()
        {
            InitializeComponent();
            HideOnClose = true;
        }

        public ICommand ResetMapCommand => new RelayCommand
        (
            () => m_map.Reset()
        );

        protected override void WindowLoaded()
        {
            base.WindowLoaded();
            m_map.Reset();
        }

        private void Packages_Checked(object sender, RoutedEventArgs e)
        {
            ViewModel.PlotAll(BlipType.Package);
        }

        private void Packages_Unchecked(object sender, RoutedEventArgs e)
        {
            ViewModel.HideAll(BlipType.Package);
        }

        private void Rampages_Checked(object sender, RoutedEventArgs e)
        {
            ViewModel.PlotAll(BlipType.Rampage);
        }

        private void Rampages_Unchecked(object sender, RoutedEventArgs e)
        {
            ViewModel.HideAll(BlipType.Rampage);
        }

        private void StuntJumps_Checked(object sender, RoutedEventArgs e)
        {
            ViewModel.PlotAll(BlipType.StuntJump);
        }

        private void StuntJumps_Unchecked(object sender, RoutedEventArgs e)
        {
            ViewModel.HideAll(BlipType.StuntJump);
        }

        private void IsShowingCollected_Checked(object sender, RoutedEventArgs e)
        {
            var allCollected = ViewModel.Blips.Where(x => x.IsCollected == true);
            var toEnable = allCollected.Where(x => x.Type == BlipType.Package && ViewModel.IsShowingPackages)
                .Concat(allCollected.Where(x => x.Type == BlipType.Rampage && ViewModel.IsShowingRampages))
                .Concat(allCollected.Where(x => x.Type == BlipType.StuntJump && ViewModel.IsShowingStuntJumps));

            foreach (var b in toEnable)
            {
                b.IsEnabled = true;
            }
        }

        private void IsShowingCollected_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var b in ViewModel.Blips.Where(x => x.IsCollected == true))
            {
                b.IsEnabled = false;
            }
        }
    }
}
