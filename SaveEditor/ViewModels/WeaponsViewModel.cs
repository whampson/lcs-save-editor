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
using WHampson.LcsSaveEditor.Models;
using WHampson.LcsSaveEditor.Models.GameDataTypes;

namespace WHampson.LcsSaveEditor.ViewModels
{
    public class WeaponsViewModel : PageViewModelBase
    {
        //private readonly ArrayWrapper<uint> globals;
        private readonly Dictionary<Weapon, int> weaponAmmoIndexMap;

        private Weapon? _handWeapon;
        private Weapon? _meleeWeapon;
        private Weapon? _projectile;
        private Weapon? _pistol;
        private Weapon? _shotgun;
        private Weapon? _smg;
        private Weapon? _assault;
        private Weapon? _heavy;
        private Weapon? _sniper;
        private Weapon? _special;

        private uint _projectileAmmo;
        private uint _pistolAmmo;
        private uint _shotgunAmmo;
        private uint _smgAmmo;
        private uint _assaultAmmo;
        private uint _heavyAmmo;
        private uint _sniperAmmo;
        private uint _specialAmmo;

        private bool suppressGlobalVariablesChanged;

        public WeaponsViewModel(SaveDataFile gameState)
            : base("Weapons")
        {
            weaponAmmoIndexMap = new Dictionary<Weapon, int>();

            //globals = gameState.Scripts.GlobalVariables;
            //globals.CollectionChanged += GlobalVariables_CollectionChanged;
            suppressGlobalVariablesChanged = false;

            switch (gameState.FileType) {
                case GamePlatform.Android:
                case GamePlatform.IOS:
                    InitMobileWeaponVars();
                    break;
                case GamePlatform.PlayStation2:
                    InitPS2WeaponVars();
                    break;
                default:
                    throw new NotSupportedException();
            }

            InitWeaponLists();
            ReadHandWeaponSlot();
            ReadMeleeWeaponSlot();
            ReadProjectileSlot();
            ReadPistolSlot();
            ReadShotgunSlot();
            ReadSmgSlot();
            ReadAssaultSlot();
            ReadHeavySlot();
            ReadSniperSlot();
            ReadSpecialSlot();
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
            if (globals[weaponAmmoIndexMap[Weapon.BrassKnuckles]] != 0) {
                HandWeapon = Weapon.BrassKnuckles;
            }
            else {
                HandWeapon = Weapon.Fists;
            }
        }

        public void WriteHandWeaponSlot()
        {
            globals[weaponAmmoIndexMap[Weapon.BrassKnuckles]] =
                (HandWeapon == Weapon.BrassKnuckles) ? 1U : 0;
        }

        public ListCollectionView HandWeapons
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

        public ListCollectionView MeleeWeapons
        {
            get;
            private set;
        }

        public void ReadMeleeWeaponSlot()
        {
            _meleeWeapon = null;
            foreach (Weapon w in MeleeWeapons) {
                uint val = globals[weaponAmmoIndexMap[w]];
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
                globals[weaponAmmoIndexMap[w]] =
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

        public ListCollectionView Projectiles
        {
            get;
            private set;
        }

        public void ReadProjectileSlot()
        {
            _projectileAmmo = 0;
            _projectile = null;
            foreach (Weapon w in Projectiles) {
                uint val = globals[weaponAmmoIndexMap[w]];
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
                globals[weaponAmmoIndexMap[w]] =
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

        public ListCollectionView Pistols
        {
            get;
            private set;
        }

        public void ReadPistolSlot()
        {
            _pistol = null;
            _pistolAmmo = 0;
            foreach (Weapon w in Pistols) {
                uint val = globals[weaponAmmoIndexMap[w]];
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
                globals[weaponAmmoIndexMap[w]] =
                    (w == Pistol) ? PistolAmmo : 0;
            }
            suppressGlobalVariablesChanged = false;
        }
        #endregion

        #region Shotgun Slot
        public Weapon? Shotgun
        {
            get { return _shotgun; }
            set {
                _shotgun = value;
                OnPropertyChanged();
                WriteShotgunSlot();
            }
        }

        public uint ShotgunAmmo
        {
            get { return _shotgunAmmo; }
            set {
                _shotgunAmmo = value;
                OnPropertyChanged();
                WriteShotgunSlot();
            }
        }

        public bool IsShotgunSlotEnabled
        {
            get { return Shotgun != null; }
            set {
                if (value) {
                    _shotgunAmmo = 1;
                    Shotgun = (Weapon) Shotguns.GetItemAt(0);
                }
                else {
                    _shotgunAmmo = 0;
                    Shotgun = null;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShotgunAmmo));
            }
        }

        public ListCollectionView Shotguns
        {
            get;
            private set;
        }

        public void ReadShotgunSlot()
        {
            _shotgun = null;
            _shotgunAmmo = 0;
            foreach (Weapon w in Shotguns) {
                uint val = globals[weaponAmmoIndexMap[w]];
                if (val > 0) {
                    _shotgun = w;
                    _shotgunAmmo = val;
                    break;
                }
            }
        }

        public void WriteShotgunSlot()
        {
            suppressGlobalVariablesChanged = true;
            foreach (Weapon w in Shotguns) {
                globals[weaponAmmoIndexMap[w]] =
                    (w == Shotgun) ? ShotgunAmmo : 0;
            }
            suppressGlobalVariablesChanged = false;
        }
        #endregion

        #region SMG Slot
        public Weapon? Smg
        {
            get { return _smg; }
            set {
                _smg = value;
                OnPropertyChanged();
                WriteSmgSlot();
            }
        }

        public uint SmgAmmo
        {
            get { return _smgAmmo; }
            set {
                _smgAmmo = value;
                OnPropertyChanged();
                WriteSmgSlot();
            }
        }

        public bool IsSmgSlotEnabled
        {
            get { return Smg != null; }
            set {
                if (value) {
                    _smgAmmo = 1;
                    Smg = (Weapon) Smgs.GetItemAt(0);
                }
                else {
                    _smgAmmo = 0;
                    Smg = null;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(SmgAmmo));
            }
        }

        public ListCollectionView Smgs
        {
            get;
            private set;
        }

        public void ReadSmgSlot()
        {
            _smg = null;
            _smgAmmo = 0;
            foreach (Weapon w in Smgs) {
                uint val = globals[weaponAmmoIndexMap[w]];
                if (val > 0) {
                    _smg = w;
                    _smgAmmo = val;
                    break;
                }
            }
        }

        public void WriteSmgSlot()
        {
            suppressGlobalVariablesChanged = true;
            foreach (Weapon w in Smgs) {
                globals[weaponAmmoIndexMap[w]] =
                    (w == Smg) ? SmgAmmo : 0;
            }
            suppressGlobalVariablesChanged = false;
        }
        #endregion

        #region Assault Slot
        public Weapon? Assault
        {
            get { return _assault; }
            set {
                _assault = value;
                OnPropertyChanged();
                WriteAssaultSlot();
            }
        }

        public uint AssaultAmmo
        {
            get { return _assaultAmmo; }
            set {
                _assaultAmmo = value;
                OnPropertyChanged();
                WriteAssaultSlot();
            }
        }

        public bool IsAssaultSlotEnabled
        {
            get { return Assault != null; }
            set {
                if (value) {
                    _assaultAmmo = 1;
                    Assault = (Weapon) Assaults.GetItemAt(0);
                }
                else {
                    _assaultAmmo = 0;
                    Assault = null;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(AssaultAmmo));
            }
        }

        public ListCollectionView Assaults
        {
            get;
            private set;
        }

        public void ReadAssaultSlot()
        {
            _assault = null;
            _assaultAmmo = 0;
            foreach (Weapon w in Assaults) {
                uint val = globals[weaponAmmoIndexMap[w]];
                if (val > 0) {
                    _assault = w;
                    _assaultAmmo = val;
                    break;
                }
            }
        }

        public void WriteAssaultSlot()
        {
            suppressGlobalVariablesChanged = true;
            foreach (Weapon w in Assaults) {
                globals[weaponAmmoIndexMap[w]] =
                    (w == Assault) ? AssaultAmmo : 0;
            }
            suppressGlobalVariablesChanged = false;
        }
        #endregion

        #region Heavy Slot
        public Weapon? Heavy
        {
            get { return _heavy; }
            set {
                _heavy = value;
                OnPropertyChanged();
                WriteHeavySlot();
            }
        }

        public uint HeavyAmmo
        {
            get { return _heavyAmmo; }
            set {
                _heavyAmmo = value;
                OnPropertyChanged();
                WriteHeavySlot();
            }
        }

        public bool IsHeavySlotEnabled
        {
            get { return Heavy != null; }
            set {
                if (value) {
                    _heavyAmmo = 1;
                    Heavy = (Weapon) Heavys.GetItemAt(0);
                }
                else {
                    _heavyAmmo = 0;
                    Heavy = null;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(HeavyAmmo));
            }
        }

        public ListCollectionView Heavys
        {
            get;
            private set;
        }

        public void ReadHeavySlot()
        {
            _heavy = null;
            _heavyAmmo = 0;
            foreach (Weapon w in Heavys) {
                uint val = globals[weaponAmmoIndexMap[w]];
                if (val > 0) {
                    _heavy = w;
                    _heavyAmmo = val;
                    break;
                }
            }
        }

        public void WriteHeavySlot()
        {
            suppressGlobalVariablesChanged = true;
            foreach (Weapon w in Heavys) {
                globals[weaponAmmoIndexMap[w]] =
                    (w == Heavy) ? HeavyAmmo : 0;
            }
            suppressGlobalVariablesChanged = false;
        }
        #endregion

        #region Sniper Slot
        public Weapon? Sniper
        {
            get { return _sniper; }
            set {
                _sniper = value;
                OnPropertyChanged();
                WriteSniperSlot();
            }
        }

        public uint SniperAmmo
        {
            get { return _sniperAmmo; }
            set {
                _sniperAmmo = value;
                OnPropertyChanged();
                WriteSniperSlot();
            }
        }

        public bool IsSniperSlotEnabled
        {
            get { return Sniper != null; }
            set {
                if (value) {
                    _sniperAmmo = 1;
                    Sniper = (Weapon) Snipers.GetItemAt(0);
                }
                else {
                    _sniperAmmo = 0;
                    Sniper = null;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(SniperAmmo));
            }
        }

        public ListCollectionView Snipers
        {
            get;
            private set;
        }

        public void ReadSniperSlot()
        {
            _sniper = null;
            _sniperAmmo = 0;
            foreach (Weapon w in Snipers) {
                uint val = globals[weaponAmmoIndexMap[w]];
                if (val > 0) {
                    _sniper = w;
                    _sniperAmmo = val;
                    break;
                }
            }
        }

        public void WriteSniperSlot()
        {
            suppressGlobalVariablesChanged = true;
            foreach (Weapon w in Snipers) {
                globals[weaponAmmoIndexMap[w]] =
                    (w == Sniper) ? SniperAmmo : 0;
            }
            suppressGlobalVariablesChanged = false;
        }
        #endregion

        #region Special Slot
        public Weapon? Special
        {
            get { return _special; }
            set {
                _special = value;
                OnPropertyChanged();
                WriteSpecialSlot();
            }
        }

        public uint SpecialAmmo
        {
            get { return _specialAmmo; }
            set {
                _specialAmmo = value;
                OnPropertyChanged();
                WriteSpecialSlot();
            }
        }

        public bool IsSpecialSlotEnabled
        {
            get { return Special != null; }
            set {
                if (value) {
                    _specialAmmo = 1;
                    Special = (Weapon) Specials.GetItemAt(0);
                }
                else {
                    _specialAmmo = 0;
                    Special = null;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(SpecialAmmo));
            }
        }

        public ListCollectionView Specials
        {
            get;
            private set;
        }

        public void ReadSpecialSlot()
        {
            _special = null;
            _specialAmmo = 0;
            foreach (Weapon w in Specials) {
                uint val = globals[weaponAmmoIndexMap[w]];
                if (val > 0) {
                    _special = w;
                    _specialAmmo = val;
                    break;
                }
            }
        }

        public void WriteSpecialSlot()
        {
            suppressGlobalVariablesChanged = true;
            foreach (Weapon w in Specials) {
                globals[weaponAmmoIndexMap[w]] =
                    (w == Special) ? SpecialAmmo : 0;
            }
            suppressGlobalVariablesChanged = false;
        }
        #endregion

        private void InitWeaponLists()
        {
            HandWeapons = new ListCollectionView(new List<Weapon>
            {
                Weapon.Fists,
                Weapon.BrassKnuckles
            });

            MeleeWeapons = new ListCollectionView(new List<Weapon>
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

            Projectiles = new ListCollectionView(new List<Weapon>
            {
                Weapon.Grenades,
                Weapon.MolotovCocktail,
                Weapon.TearGas,
                Weapon.RemoteGrenades
            });

            Pistols = new ListCollectionView(new List<Weapon>
            {
                Weapon.Pistol,
                Weapon.Python
            });

            Shotguns = new ListCollectionView(new List<Weapon>
            {
                Weapon.Shotgun,
                Weapon.SpasShotgun,
                Weapon.StubbyShotgun
            });

            Smgs = new ListCollectionView(new List<Weapon>
            {
                Weapon.Tec9,
                Weapon.Mac10,
                Weapon.MicroSmg,
                Weapon.Mp5k
            });

            Assaults = new ListCollectionView(new List<Weapon>
            {
                Weapon.Ak47,
                Weapon.M4
            });

            Heavys = new ListCollectionView(new List<Weapon>
            {
                Weapon.RocketLauncher,
                Weapon.M60,
                Weapon.FlameThrower,
                Weapon.MiniGun
            });

            Snipers = new ListCollectionView(new List<Weapon>
            {
                Weapon.SniperRifle,
                Weapon.LaserSightedSniperRifle
            });

            Specials = new ListCollectionView(new List<Weapon>
            {
                Weapon.Camera
            });
        }

        private void InitMobileWeaponVars()
        {
            weaponAmmoIndexMap[Weapon.Camera] = 125;
            weaponAmmoIndexMap[Weapon.BrassKnuckles] = 126;
            weaponAmmoIndexMap[Weapon.Chisel] = 127;
            weaponAmmoIndexMap[Weapon.Axe] = 128;
            weaponAmmoIndexMap[Weapon.HockeyStick] = 129;
            weaponAmmoIndexMap[Weapon.NightStick] = 130;
            weaponAmmoIndexMap[Weapon.BaseballBat] = 131;
            weaponAmmoIndexMap[Weapon.Cleaver] = 132;
            weaponAmmoIndexMap[Weapon.Katana] = 133;
            weaponAmmoIndexMap[Weapon.Knife] = 134;
            weaponAmmoIndexMap[Weapon.Machete] = 135;
            weaponAmmoIndexMap[Weapon.Chainsaw] = 136;
            weaponAmmoIndexMap[Weapon.Grenades] = 137;
            weaponAmmoIndexMap[Weapon.MolotovCocktail] = 138;
            weaponAmmoIndexMap[Weapon.TearGas] = 139;
            weaponAmmoIndexMap[Weapon.RemoteGrenades] = 140;
            weaponAmmoIndexMap[Weapon.Pistol] = 141;
            weaponAmmoIndexMap[Weapon.Python] = 142;
            weaponAmmoIndexMap[Weapon.Shotgun] = 143;
            weaponAmmoIndexMap[Weapon.SpasShotgun] = 144;
            weaponAmmoIndexMap[Weapon.StubbyShotgun] = 145;
            weaponAmmoIndexMap[Weapon.Tec9] = 146;
            weaponAmmoIndexMap[Weapon.Mac10] = 147;
            weaponAmmoIndexMap[Weapon.MicroSmg] = 148;
            weaponAmmoIndexMap[Weapon.Mp5k] = 149;
            weaponAmmoIndexMap[Weapon.Ak47] = 150;
            weaponAmmoIndexMap[Weapon.M4] = 151;
            weaponAmmoIndexMap[Weapon.RocketLauncher] = 152;
            weaponAmmoIndexMap[Weapon.M60] = 153;
            weaponAmmoIndexMap[Weapon.FlameThrower] = 154;
            weaponAmmoIndexMap[Weapon.MiniGun] = 155;
            weaponAmmoIndexMap[Weapon.SniperRifle] = 156;
            weaponAmmoIndexMap[Weapon.LaserSightedSniperRifle] = 157;
        }

        private void InitPS2WeaponVars()
        {
            weaponAmmoIndexMap[Weapon.Camera] = 124;
            weaponAmmoIndexMap[Weapon.BrassKnuckles] = 125;
            weaponAmmoIndexMap[Weapon.Chisel] = 126;
            weaponAmmoIndexMap[Weapon.Axe] = 127;
            weaponAmmoIndexMap[Weapon.HockeyStick] = 128;
            weaponAmmoIndexMap[Weapon.NightStick] = 129;
            weaponAmmoIndexMap[Weapon.BaseballBat] = 130;
            weaponAmmoIndexMap[Weapon.Cleaver] = 131;
            weaponAmmoIndexMap[Weapon.Katana] = 132;
            weaponAmmoIndexMap[Weapon.Knife] = 133;
            weaponAmmoIndexMap[Weapon.Machete] = 134;
            weaponAmmoIndexMap[Weapon.Chainsaw] = 135;
            weaponAmmoIndexMap[Weapon.Grenades] = 136;
            weaponAmmoIndexMap[Weapon.MolotovCocktail] = 137;
            weaponAmmoIndexMap[Weapon.TearGas] = 138;
            weaponAmmoIndexMap[Weapon.RemoteGrenades] = 139;
            weaponAmmoIndexMap[Weapon.Pistol] = 140;
            weaponAmmoIndexMap[Weapon.Python] = 141;
            weaponAmmoIndexMap[Weapon.Shotgun] = 142;
            weaponAmmoIndexMap[Weapon.SpasShotgun] = 143;
            weaponAmmoIndexMap[Weapon.StubbyShotgun] = 144;
            weaponAmmoIndexMap[Weapon.Tec9] = 145;
            weaponAmmoIndexMap[Weapon.Mac10] = 146;
            weaponAmmoIndexMap[Weapon.MicroSmg] = 147;
            weaponAmmoIndexMap[Weapon.Mp5k] = 148;
            weaponAmmoIndexMap[Weapon.Ak47] = 149;
            weaponAmmoIndexMap[Weapon.M4] = 150;
            weaponAmmoIndexMap[Weapon.RocketLauncher] = 151;
            weaponAmmoIndexMap[Weapon.M60] = 152;
            weaponAmmoIndexMap[Weapon.FlameThrower] = 153;
            weaponAmmoIndexMap[Weapon.MiniGun] = 154;
            weaponAmmoIndexMap[Weapon.SniperRifle] = 155;
            weaponAmmoIndexMap[Weapon.LaserSightedSniperRifle] = 156;
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
            Weapon changedWeapon = weaponAmmoIndexMap.FirstOrDefault(x => x.Value == changedVar).Key;

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
                case Weapon.Shotgun:
                case Weapon.SpasShotgun:
                case Weapon.StubbyShotgun:
                    ReadShotgunSlot();
                    break;
                case Weapon.Tec9:
                case Weapon.Mac10:
                case Weapon.MicroSmg:
                case Weapon.Mp5k:
                    ReadSmgSlot();
                    break;
                case Weapon.Ak47:
                case Weapon.M4:
                    ReadAssaultSlot();
                    break;
                case Weapon.RocketLauncher:
                case Weapon.M60:
                case Weapon.FlameThrower:
                case Weapon.MiniGun:
                    ReadHeavySlot();
                    break;
                case Weapon.SniperRifle:
                case Weapon.LaserSightedSniperRifle:
                    ReadSniperSlot();
                    break;
                case Weapon.Camera:
                    ReadSpecialSlot();
                    break;
            }
        }
    }
}
