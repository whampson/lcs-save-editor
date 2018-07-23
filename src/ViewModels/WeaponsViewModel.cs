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

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Data;
using WHampson.LcsSaveEditor.DataTypes;
using WHampson.LcsSaveEditor.Helpers;
using WHampson.LcsSaveEditor.Models;

namespace WHampson.LcsSaveEditor.ViewModels
{
    public class WeaponsViewModel : PageViewModel
    {
        private readonly ArrayWrapper<uint> globals;
        private readonly Dictionary<Weapon, int> weaponAmmoIndex;

        private Weapon? _handWeapon;
        private Weapon? _meleeWeapon;
        private Weapon? _projectile;
        private Weapon? _pistol;

        private uint _projectileAmmo;
        private uint _pistolAmmo;

        private bool suppressGlobalVariablesChanged;

        public WeaponsViewModel(SaveDataFile gameState)
            : base("Weapons", gameState)
        {
            weaponAmmoIndex = new Dictionary<Weapon, int>();

            globals = gameState.Scripts.GlobalVariables;
            globals.CollectionChanged += GlobalVariables_CollectionChanged;
            suppressGlobalVariablesChanged = false;

            switch (gameState.FileType) {
                case GamePlatform.PS2:
                    InitPS2Weapons();
                    break;
            }

            InitWeaponLists();
            ReadHandWeaponSlot();
            ReadMeleeWeaponSlot();
            ReadProjectileSlot();
            ReadPistolSlot();
        }

        #region Hand Weapon Slot
        public Weapon? HandWeapon
        {
            get { return _handWeapon; }
            set {
                _handWeapon = value;
                OnPropertyChanged();
                WriteHandWeaponSlot();
            }
        }

        public void ReadHandWeaponSlot()
        {
            if (globals[weaponAmmoIndex[Weapon.BrassKnuckles]] != 0) {
                HandWeapon = Weapon.BrassKnuckles;
            }
            else {
                HandWeapon = Weapon.Fists;
            }
        }

        public void WriteHandWeaponSlot()
        {
            globals[weaponAmmoIndex[Weapon.BrassKnuckles]] =
                (HandWeapon == Weapon.BrassKnuckles) ? 1U : 0;
        }

        public CollectionView HandWeapons
        {
            get;
            private set;
        }
        #endregion

        #region Melee Slot
        public Weapon? MeleeWeapon
        {
            get { return _meleeWeapon; }
            set {
                _meleeWeapon = value;
                OnPropertyChanged();
                WriteMeleeWeaponSlot();
            }
        }

        public bool IsMeleeSlotEnabled
        {
            get { return MeleeWeapon != null; }
            set {
                if (value) {
                    MeleeWeapon = (Weapon) MeleeWeapons.GetItemAt(0);
                }
                else {
                    MeleeWeapon = null;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(ProjectileAmmo));
            }
        }

        public CollectionView MeleeWeapons
        {
            get;
            private set;
        }

        public void ReadMeleeWeaponSlot()
        {
            _meleeWeapon = null;
            foreach (Weapon w in MeleeWeapons) {
                uint val = globals[weaponAmmoIndex[w]];
                if (val > 0) {
                    _meleeWeapon = w;
                    break;
                }
            }
        }

        public void WriteMeleeWeaponSlot()
        {
            suppressGlobalVariablesChanged = true;
            foreach (Weapon w in MeleeWeapons) {
                globals[weaponAmmoIndex[w]] =
                    (w == MeleeWeapon) ? 1U : 0;
            }
            suppressGlobalVariablesChanged = false;
        }
        #endregion

        #region Projectile Slot
        public Weapon? Projectile
        {
            get { return _projectile; }
            set {
                _projectile = value;
                OnPropertyChanged();
                WriteProjectileSlot();
            }
        }

        public uint ProjectileAmmo
        {
            get { return _projectileAmmo; }
            set {
                _projectileAmmo = value;
                OnPropertyChanged();
                WriteProjectileSlot();
            }
        }

        public bool IsProjectileSlotEnabled
        {
            get { return Projectile != null; }
            set {
                if (value) {
                    _projectileAmmo = 1;
                    Projectile = (Weapon) Projectiles.GetItemAt(0);
                }
                else {
                    _projectileAmmo = 0;
                    Projectile = null;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(ProjectileAmmo));
            }
        }

        public CollectionView Projectiles
        {
            get;
            private set;
        }

        public void ReadProjectileSlot()
        {
            _projectileAmmo = 0;
            _projectile = null;
            foreach (Weapon w in Projectiles) {
                uint val = globals[weaponAmmoIndex[w]];
                if (val > 0) {
                    _projectile = w;
                    _projectileAmmo = val;
                    break;
                }
            }
        }

        public void WriteProjectileSlot()
        {
            suppressGlobalVariablesChanged = true;
            foreach (Weapon w in Projectiles) {
                globals[weaponAmmoIndex[w]] =
                    (w == Projectile) ? ProjectileAmmo : 0;
            }
            suppressGlobalVariablesChanged = false;
        }
        #endregion

        #region Pistol Slot
        public Weapon? Pistol
        {
            get { return _pistol; }
            set {
                _pistol = value;
                OnPropertyChanged();
                WritePistolSlot();
            }
        }

        public uint PistolAmmo
        {
            get { return _pistolAmmo; }
            set {
                _pistolAmmo = value;
                OnPropertyChanged();
                WritePistolSlot();
            }
        }

        public bool IsPistolSlotEnabled
        {
            get { return Pistol != null; }
            set {
                if (value) {
                    _pistolAmmo = 1;
                    Pistol = (Weapon) Pistols.GetItemAt(0);
                }
                else {
                    _pistolAmmo = 0;
                    Pistol = null;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(PistolAmmo));
            }
        }

        public CollectionView Pistols
        {
            get;
            private set;
        }

        public void ReadPistolSlot()
        {
            _pistol = null;
            _pistolAmmo = 0;
            foreach (Weapon w in Pistols) {
                uint val = globals[weaponAmmoIndex[w]];
                if (val > 0) {
                    _pistol = w;
                    _pistolAmmo = val;
                    break;
                }
            }
        }

        public void WritePistolSlot()
        {
            suppressGlobalVariablesChanged = true;
            foreach (Weapon w in Pistols) {
                globals[weaponAmmoIndex[w]] =
                    (w == Pistol) ? PistolAmmo : 0;
            }
            suppressGlobalVariablesChanged = false;
        }
        #endregion

        private void InitWeaponLists()
        {
            HandWeapons = new CollectionView(new List<Weapon>
            {
                Weapon.Fists,
                Weapon.BrassKnuckles
            });

            MeleeWeapons = new CollectionView(new List<Weapon>
            {
                Weapon.Chisel,
                Weapon.Axe,
                Weapon.HockeyStick,
                Weapon.NightStick,
                Weapon.BaseballBat,
                Weapon.Cleaver,
                Weapon.Katana,
                Weapon.Knife,
                Weapon.Machete,
                Weapon.Chainsaw
            });

            Projectiles = new CollectionView(new List<Weapon>
            {
                Weapon.Grenades,
                Weapon.MolotovCocktail,
                Weapon.TearGas,
                Weapon.RemoteGrenades
            });

            Pistols = new CollectionView(new List<Weapon>
            {
                Weapon.Pistol,
                Weapon.Python
            });
        }

        private void InitPS2Weapons()
        {
            weaponAmmoIndex[Weapon.Camera] = 124;
            weaponAmmoIndex[Weapon.BrassKnuckles] = 125;
            weaponAmmoIndex[Weapon.Chisel] = 126;
            weaponAmmoIndex[Weapon.Axe] = 127;
            weaponAmmoIndex[Weapon.HockeyStick] = 128;
            weaponAmmoIndex[Weapon.NightStick] = 129;
            weaponAmmoIndex[Weapon.BaseballBat] = 130;
            weaponAmmoIndex[Weapon.Cleaver] = 131;
            weaponAmmoIndex[Weapon.Katana] = 132;
            weaponAmmoIndex[Weapon.Knife] = 133;
            weaponAmmoIndex[Weapon.Machete] = 134;
            weaponAmmoIndex[Weapon.Chainsaw] = 135;
            weaponAmmoIndex[Weapon.Grenades] = 136;
            weaponAmmoIndex[Weapon.MolotovCocktail] = 137;
            weaponAmmoIndex[Weapon.TearGas] = 138;
            weaponAmmoIndex[Weapon.RemoteGrenades] = 139;
            weaponAmmoIndex[Weapon.Pistol] = 140;
            weaponAmmoIndex[Weapon.Python] = 141;
            weaponAmmoIndex[Weapon.Shotgun] = 142;
            weaponAmmoIndex[Weapon.SpasShotgun] = 143;
            weaponAmmoIndex[Weapon.StubbyShotgun] = 144;
            weaponAmmoIndex[Weapon.Tec9] = 145;
            weaponAmmoIndex[Weapon.Mac10] = 146;
            weaponAmmoIndex[Weapon.MicroSmg] = 147;
            weaponAmmoIndex[Weapon.Mp5k] = 148;
            weaponAmmoIndex[Weapon.Ak47] = 149;
            weaponAmmoIndex[Weapon.M4] = 150;
            weaponAmmoIndex[Weapon.RocketLauncher] = 151;
            weaponAmmoIndex[Weapon.M60] = 152;
            weaponAmmoIndex[Weapon.FlameThrower] = 153;
            weaponAmmoIndex[Weapon.MiniGun] = 154;
            weaponAmmoIndex[Weapon.SniperRifle] = 155;
            weaponAmmoIndex[Weapon.LaserSightedSniperRifle] = 156;
        }

        private void GlobalVariables_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (suppressGlobalVariablesChanged) {
                return;
            }

            if (e.Action != NotifyCollectionChangedAction.Replace) {
                return;
            }

            // Detect external changes to weapons vars

            int changedVar = e.NewStartingIndex;
            Weapon changedWeapon = weaponAmmoIndex.FirstOrDefault(x => x.Value == changedVar).Key;

            switch (changedWeapon) {
                case Weapon.BrassKnuckles:
                    ReadHandWeaponSlot();
                    break;
                case Weapon.Chisel:
                case Weapon.Axe:
                case Weapon.HockeyStick:
                case Weapon.NightStick:
                case Weapon.BaseballBat:
                case Weapon.Cleaver:
                case Weapon.Katana:
                case Weapon.Knife:
                case Weapon.Machete:
                case Weapon.Chainsaw:
                    ReadMeleeWeaponSlot();
                    break;
                case Weapon.Grenades:
                case Weapon.MolotovCocktail:
                case Weapon.TearGas:
                case Weapon.RemoteGrenades:
                    ReadProjectileSlot();
                    break;
                case Weapon.Pistol:
                case Weapon.Python:
                    ReadPistolSlot();
                    break;
            }
        }
    }
}
