using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LCSSaveEditor.Core;
using LCSSaveEditor.GUI.Types;
using LCSSaveEditor.GUI.ViewModels;

using PlayerTabViewModel = LCSSaveEditor.GUI.ViewModels.PlayerTab;

namespace LCSSaveEditor.GUI.Views
{
    /// <summary>
    /// Interaction logic for PlayerTab.xaml
    /// </summary>
    public partial class PlayerTab : TabPageBase<PlayerTabViewModel>
    {
        public static readonly DependencyProperty CurrentOutfitImageProperty = DependencyProperty.Register(
            nameof(CurrentOutfitImage), typeof(BitmapImage), typeof(PlayerTab));

        public BitmapImage CurrentOutfitImage
        {
            get { return (BitmapImage) GetValue(CurrentOutfitImageProperty); }
            set { SetValue(CurrentOutfitImageProperty, value); }
        }

        public PlayerTab()
        {
            InitializeComponent();
        }

        private void LoadOutfitImage(int id)
        {
            if (OutfitUris.Count > id)
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.DecodePixelWidth = 420;
                img.UriSource = new Uri(OutfitUris[id]);
                img.EndInit();
                CurrentOutfitImage = img;
            }
            else
            {
                CurrentOutfitImage = null;
            }
        }

        private void OnSlotAmmoChaged(int index)
        {
            IEnumerable<Weapon> weapons;
            Weapon? weapon, oldWeapon;
            bool weaponNeedsUpdate;
            int ammo;

            switch (index)
            {
                case 1: weapons = PlayerTabViewModel.Slot1Weapons; oldWeapon = ViewModel.Slot1Weapon; ammo = ViewModel.Slot1Ammo; break;
                case 2: weapons = PlayerTabViewModel.Slot2Weapons; oldWeapon = ViewModel.Slot2Weapon; ammo = ViewModel.Slot2Ammo; break;
                case 3: weapons = PlayerTabViewModel.Slot3Weapons; oldWeapon = ViewModel.Slot3Weapon; ammo = ViewModel.Slot3Ammo; break;
                case 4: weapons = PlayerTabViewModel.Slot4Weapons; oldWeapon = ViewModel.Slot4Weapon; ammo = ViewModel.Slot4Ammo; break;
                case 5: weapons = PlayerTabViewModel.Slot5Weapons; oldWeapon = ViewModel.Slot5Weapon; ammo = ViewModel.Slot5Ammo; break;
                case 6: weapons = PlayerTabViewModel.Slot6Weapons; oldWeapon = ViewModel.Slot6Weapon; ammo = ViewModel.Slot6Ammo; break;
                case 7: weapons = PlayerTabViewModel.Slot7Weapons; oldWeapon = ViewModel.Slot7Weapon; ammo = ViewModel.Slot7Ammo; break;
                case 8: weapons = PlayerTabViewModel.Slot8Weapons; oldWeapon = ViewModel.Slot8Weapon; ammo = ViewModel.Slot8Ammo; break;
                case 9: weapons = PlayerTabViewModel.Slot9Weapons; oldWeapon = ViewModel.Slot9Weapon; ammo = ViewModel.Slot9Ammo; break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }

            weapon = null;
            weaponNeedsUpdate = false;
            if (oldWeapon == null && ammo != 0)
            {
                weapon = weapons.First();
                weaponNeedsUpdate = true;
            }
            else if (oldWeapon != null && ammo == 0)
            {
                weapon = null;
                weaponNeedsUpdate = true;
            }

            if (weaponNeedsUpdate)
            {
                switch (index)
                {
                    case 1: ViewModel.Slot1Weapon = weapon; break;
                    case 2: ViewModel.Slot2Weapon = weapon; break;
                    case 3: ViewModel.Slot3Weapon = weapon; break;
                    case 4: ViewModel.Slot4Weapon = weapon; break;
                    case 5: ViewModel.Slot5Weapon = weapon; break;
                    case 6: ViewModel.Slot6Weapon = weapon; break;
                    case 7: ViewModel.Slot7Weapon = weapon; break;
                    case 8: ViewModel.Slot8Weapon = weapon; break;
                    case 9: ViewModel.Slot9Weapon = weapon; break;
                }
            }
        }

        private void OnSlotWeaponChanged(int index)
        {
            Weapon? weapon;
            int ammo, oldAmmo;
            bool ammoNeedsUpdate;

            switch (index)
            {
                case 1: weapon = ViewModel.Slot1Weapon; oldAmmo = ViewModel.Slot1Ammo; break;
                case 2: weapon = ViewModel.Slot2Weapon; oldAmmo = ViewModel.Slot2Ammo; break;
                case 3: weapon = ViewModel.Slot3Weapon; oldAmmo = ViewModel.Slot3Ammo; break;
                case 4: weapon = ViewModel.Slot4Weapon; oldAmmo = ViewModel.Slot4Ammo; break;
                case 5: weapon = ViewModel.Slot5Weapon; oldAmmo = ViewModel.Slot5Ammo; break;
                case 6: weapon = ViewModel.Slot6Weapon; oldAmmo = ViewModel.Slot6Ammo; break;
                case 7: weapon = ViewModel.Slot7Weapon; oldAmmo = ViewModel.Slot7Ammo; break;
                case 8: weapon = ViewModel.Slot8Weapon; oldAmmo = ViewModel.Slot8Ammo; break;
                case 9: weapon = ViewModel.Slot9Weapon; oldAmmo = ViewModel.Slot9Ammo; break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }

            ammo = 0;
            ammoNeedsUpdate = false;
            if (weapon != null && oldAmmo == 0)
            {
                ammo = 1;
                ammoNeedsUpdate = true;
            }

            if (ammoNeedsUpdate)
            {
                switch (index)
                {
                    case 1: ViewModel.Slot1Ammo = ammo; break;
                    case 2: ViewModel.Slot2Ammo = ammo; break;
                    case 3: ViewModel.Slot3Ammo = ammo; break;
                    case 4: ViewModel.Slot4Ammo = ammo; break;
                    case 5: ViewModel.Slot5Ammo = ammo; break;
                    case 6: ViewModel.Slot6Ammo = ammo; break;
                    case 7: ViewModel.Slot7Ammo = ammo; break;
                    case 8: ViewModel.Slot8Ammo = ammo; break;
                    case 9: ViewModel.Slot9Ammo = ammo; break;
                }
            }
        }

        private void CurrentOutfitImage_SelectionChanged(object sender, SelectionChangedEventArgs e) => LoadOutfitImage(ViewModel.CurrentOutfit);

        private void Slot1_AmmoChanged(object sender, RoutedPropertyChangedEventArgs<object> e) => OnSlotAmmoChaged(1);
        private void Slot2_AmmoChanged(object sender, RoutedPropertyChangedEventArgs<object> e) => OnSlotAmmoChaged(2);
        private void Slot3_AmmoChanged(object sender, RoutedPropertyChangedEventArgs<object> e) => OnSlotAmmoChaged(3);
        private void Slot4_AmmoChanged(object sender, RoutedPropertyChangedEventArgs<object> e) => OnSlotAmmoChaged(4);
        private void Slot5_AmmoChanged(object sender, RoutedPropertyChangedEventArgs<object> e) => OnSlotAmmoChaged(5);
        private void Slot6_AmmoChanged(object sender, RoutedPropertyChangedEventArgs<object> e) => OnSlotAmmoChaged(6);
        private void Slot7_AmmoChanged(object sender, RoutedPropertyChangedEventArgs<object> e) => OnSlotAmmoChaged(7);
        private void Slot8_AmmoChanged(object sender, RoutedPropertyChangedEventArgs<object> e) => OnSlotAmmoChaged(8);
        private void Slot9_AmmoChanged(object sender, RoutedPropertyChangedEventArgs<object> e) => OnSlotAmmoChaged(9);

        private void Slot1_WeaponChanged(object sender, SelectionChangedEventArgs e) => OnSlotWeaponChanged(1);
        private void Slot2_WeaponChanged(object sender, SelectionChangedEventArgs e) => OnSlotWeaponChanged(2);
        private void Slot3_WeaponChanged(object sender, SelectionChangedEventArgs e) => OnSlotWeaponChanged(3);
        private void Slot4_WeaponChanged(object sender, SelectionChangedEventArgs e) => OnSlotWeaponChanged(4);
        private void Slot5_WeaponChanged(object sender, SelectionChangedEventArgs e) => OnSlotWeaponChanged(5);
        private void Slot6_WeaponChanged(object sender, SelectionChangedEventArgs e) => OnSlotWeaponChanged(6);
        private void Slot7_WeaponChanged(object sender, SelectionChangedEventArgs e) => OnSlotWeaponChanged(7);
        private void Slot8_WeaponChanged(object sender, SelectionChangedEventArgs e) => OnSlotWeaponChanged(8);
        private void Slot9_WeaponChanged(object sender, SelectionChangedEventArgs e) => OnSlotWeaponChanged(9);

        private static readonly IReadOnlyList<string> OutfitUris = new List<string>()
        {
            @"pack://application:,,,/Resources/costume00.png",
            @"pack://application:,,,/Resources/costume01.png",
            @"pack://application:,,,/Resources/costume02.png",
            @"pack://application:,,,/Resources/costume03.png",
            @"pack://application:,,,/Resources/costume04.png",
            @"pack://application:,,,/Resources/costume05.png",
            @"pack://application:,,,/Resources/costume06.png",
            @"pack://application:,,,/Resources/costume07.png",
            @"pack://application:,,,/Resources/costume08.png",
            @"pack://application:,,,/Resources/costume09.png",
            @"pack://application:,,,/Resources/costume10.png",
            @"pack://application:,,,/Resources/costume11.png",
            @"pack://application:,,,/Resources/costume12.png",
            @"pack://application:,,,/Resources/costume13.png",
            @"pack://application:,,,/Resources/costume14.png",
            @"pack://application:,,,/Resources/costume15.png",
        };
    }
}
