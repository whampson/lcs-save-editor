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
using LcsSaveEditor.Resources;
using System.Collections.Generic;
using System.Windows.Data;

namespace LcsSaveEditor.ViewModels
{
    public partial class WeaponsViewModel : PageViewModel
    {
        private bool m_hasMelee;
        private bool m_hasProjectile;
        private bool m_hasPistol;
        private bool m_hasShotgun;
        private bool m_hasSmg;
        private bool m_hasAssault;
        private bool m_hasHeavy;
        private bool m_hasSniper;
        private bool m_hasSpecial;

        private Weapon? m_hand;
        private Weapon? m_melee;
        private Weapon? m_projectile;
        private Weapon? m_pistol;
        private Weapon? m_shotgun;
        private Weapon? m_smg;
        private Weapon? m_assault;
        private Weapon? m_heavy;
        private Weapon? m_sniper;
        private Weapon? m_special;

        private uint m_meleeAmmo;
        private uint m_projectileAmmo;
        private uint m_pistolAmmo;
        private uint m_shotgunAmmo;
        private uint m_smgAmmo;
        private uint m_assaultAmmo;
        private uint m_heavyAmmo;
        private uint m_sniperAmmo;
        private uint m_specialAmmo;

        private ListCollectionView m_handList;
        private ListCollectionView m_meleeList;
        private ListCollectionView m_pistolList;
        private ListCollectionView m_projectileList;
        private ListCollectionView m_shotgunList;
        private ListCollectionView m_smgList;
        private ListCollectionView m_assaultList;
        private ListCollectionView m_heavyList;
        private ListCollectionView m_sniperList;
        private ListCollectionView m_specialList;

        private Dictionary<Weapon?, int> m_ammoIndexMap;
        private FullyObservableCollection<ScriptVariable> m_globals;
        private bool m_suppressRefresh;

        public WeaponsViewModel(MainViewModel mainViewModel)
            : base(FrontendResources.Main_Page_Weapons, PageVisibility.WhenFileLoaded, mainViewModel)
        {
            m_ammoIndexMap = new Dictionary<Weapon?, int>();
            m_suppressRefresh = false;

            InitWeaponLists();

            MainViewModel.DataLoaded += MainViewModel_DataLoaded;
            MainViewModel.DataClosing += MainViewModel_DataClosing;
        }

        public bool HasMelee
        {
            get { return m_hasMelee; }
            set {
                m_hasMelee = value;
                if (m_hasMelee) {
                    m_melee = (Weapon?) m_meleeList.GetItemAt(0);
                    m_meleeAmmo = 1;
                }
                else {
                    m_melee = null;
                    m_meleeAmmo = 0;
                }
                WriteMeleeSlot();
                OnPropertyChanged();
                OnPropertyChanged(nameof(Melee));
            }
        }

        public bool HasProjectile
        {
            get { return m_hasProjectile; }
            set {
                m_hasProjectile = value;
                if (m_hasProjectile) {
                    m_projectile = (Weapon?) m_projectileList.GetItemAt(0);
                    m_projectileAmmo = 1;
                }
                else {
                    m_projectile = null;
                    m_projectileAmmo = 0;
                }
                WriteProjectileSlot();
                OnPropertyChanged();
                OnPropertyChanged(nameof(Projectile));
                OnPropertyChanged(nameof(ProjectileAmmo));
            }
        }

        public bool HasPistol
        {
            get { return m_hasPistol; }
            set {
                m_hasPistol = value;
                if (m_hasPistol) {
                    m_pistol = (Weapon?) m_pistolList.GetItemAt(0);
                    m_pistolAmmo = 1;
                }
                else {
                    m_pistol = null;
                    m_pistolAmmo = 0;
                }
                WritePistolSlot();
                OnPropertyChanged();
                OnPropertyChanged(nameof(Pistol));
                OnPropertyChanged(nameof(PistolAmmo));
            }
        }

        public bool HasShotgun
        {
            get { return m_hasShotgun; }
            set {
                m_hasShotgun = value;
                if (m_hasShotgun) {
                    m_shotgun = (Weapon?) m_shotgunList.GetItemAt(0);
                    m_shotgunAmmo = 1;
                }
                else {
                    m_shotgun = null;
                    m_shotgunAmmo = 0;
                }
                WriteShotgunSlot();
                OnPropertyChanged();
                OnPropertyChanged(nameof(Shotgun));
                OnPropertyChanged(nameof(ShotgunAmmo));
            }
        }

        public bool HasSmg
        {
            get { return m_hasSmg; }
            set {
                m_hasSmg = value;
                if (m_hasSmg) {
                    m_smg = (Weapon?) m_smgList.GetItemAt(0);
                    m_smgAmmo = 1;
                }
                else {
                    m_smg = null;
                    m_smgAmmo = 0;
                }
                WriteSmgSlot();
                OnPropertyChanged();
                OnPropertyChanged(nameof(Smg));
                OnPropertyChanged(nameof(SmgAmmo));
            }
        }

        public bool HasAssault
        {
            get { return m_hasAssault; }
            set {
                m_hasAssault = value;
                if (m_hasAssault) {
                    m_assault = (Weapon?) m_assaultList.GetItemAt(0);
                    m_assaultAmmo = 1;
                }
                else {
                    m_assault = null;
                    m_assaultAmmo = 0;
                }
                WriteAssaultSlot();
                OnPropertyChanged();
                OnPropertyChanged(nameof(Assault));
                OnPropertyChanged(nameof(AssaultAmmo));
            }
        }

        public bool HasHeavy
        {
            get { return m_hasHeavy; }
            set {
                m_hasHeavy = value;
                if (m_hasHeavy) {
                    m_heavy = (Weapon?) m_heavyList.GetItemAt(0);
                    m_heavyAmmo = 1;
                }
                else {
                    m_heavy = null;
                    m_heavyAmmo = 0;
                }
                WriteHeavySlot();
                OnPropertyChanged();
                OnPropertyChanged(nameof(Heavy));
                OnPropertyChanged(nameof(HeavyAmmo));
            }
        }

        public bool HasSniper
        {
            get { return m_hasSniper; }
            set {
                m_hasSniper = value;
                if (m_hasSniper) {
                    m_sniper = (Weapon?) m_sniperList.GetItemAt(0);
                    m_sniperAmmo = 1;
                }
                else {
                    m_sniper = null;
                    m_sniperAmmo = 0;
                }
                WriteSniperSlot();
                OnPropertyChanged();
                OnPropertyChanged(nameof(Sniper));
                OnPropertyChanged(nameof(SniperAmmo));
            }
        }

        public bool HasSpecial
        {
            get { return m_hasSpecial; }
            set {
                m_hasSpecial = value;
                if (m_hasSpecial) {
                    m_special = (Weapon?) m_specialList.GetItemAt(0);
                    m_specialAmmo = 1;
                }
                else {
                    m_special = null;
                    m_specialAmmo = 0;
                }
                WriteSpecialSlot();
                OnPropertyChanged();
                OnPropertyChanged(nameof(Special));
                OnPropertyChanged(nameof(SpecialAmmo));
            }
        }

        public Weapon? Hand
        {
            get { return m_hand; }
            set {
                m_hand = value;
                WriteHandSlot();
                OnPropertyChanged();
            }
        }

        public Weapon? Melee
        {
            get { return m_melee; }
            set {
                m_melee = value;
                WriteMeleeSlot();
                OnPropertyChanged();
            }
        }

        public Weapon? Projectile
        {
            get { return m_projectile; }
            set {
                m_projectile = value;
                WriteProjectileSlot();
                OnPropertyChanged();
            }
        }

        public Weapon? Pistol
        {
            get { return m_pistol; }
            set {
                m_pistol = value;
                WritePistolSlot();
                OnPropertyChanged();
            }
        }

        public Weapon? Shotgun
        {
            get { return m_shotgun; }
            set {
                m_shotgun = value;
                WriteShotgunSlot();
                OnPropertyChanged();
            }
        }

        public Weapon? Smg
        {
            get { return m_smg; }
            set {
                m_smg = value;
                WriteSmgSlot();
                OnPropertyChanged();
            }
        }

        public Weapon? Assault
        {
            get { return m_assault; }
            set {
                m_assault = value;
                WriteAssaultSlot();
                OnPropertyChanged();
            }
        }

        public Weapon? Heavy
        {
            get { return m_heavy; }
            set {
                m_heavy = value;
                WriteHeavySlot();
                OnPropertyChanged();
            }
        }

        public Weapon? Sniper
        {
            get { return m_sniper; }
            set {
                m_sniper = value;
                WriteSniperSlot();
                OnPropertyChanged();
            }
        }

        public Weapon? Special
        {
            get { return m_special; }
            set {
                m_special = value;
                WriteSpecialSlot();
                OnPropertyChanged();
            }
        }

        public uint ProjectileAmmo
        {
            get { return m_projectileAmmo; }
            set {
                m_projectileAmmo = value;
                WriteProjectileSlot();
                OnPropertyChanged();
            }
        }

        public uint PistolAmmo
        {
            get { return m_pistolAmmo; }
            set {
                m_pistolAmmo = value;
                WritePistolSlot();
                OnPropertyChanged();
            }
        }

        public uint ShotgunAmmo
        {
            get { return m_shotgunAmmo; }
            set {
                m_shotgunAmmo = value;
                WriteShotgunSlot();
                OnPropertyChanged();
            }
        }

        public uint SmgAmmo
        {
            get { return m_smgAmmo; }
            set {
                m_smgAmmo = value;
                WriteSmgSlot();
                OnPropertyChanged();
            }
        }

        public uint AssaultAmmo
        {
            get { return m_assaultAmmo; }
            set {
                m_assaultAmmo = value;
                WriteAssaultSlot();
                OnPropertyChanged();
            }
        }

        public uint HeavyAmmo
        {
            get { return m_heavyAmmo; }
            set {
                m_heavyAmmo = value;
                WriteHeavySlot();
                OnPropertyChanged();
            }
        }

        public uint SniperAmmo
        {
            get { return m_sniperAmmo; }
            set {
                m_sniperAmmo = value;
                WriteSniperSlot();
                OnPropertyChanged();
            }
        }

        public uint SpecialAmmo
        {
            get { return m_specialAmmo; }
            set {
                m_specialAmmo = value;
                WriteSpecialSlot();
                OnPropertyChanged();
            }
        }

        public ListCollectionView HandList
        {
            get { return m_handList; }
            set { m_handList = value; OnPropertyChanged(); }
        }

        public ListCollectionView MeleeList
        {
            get { return m_meleeList; }
            set { m_meleeList = value; OnPropertyChanged(); }
        }

        public ListCollectionView ProjectileList
        {
            get { return m_projectileList; }
            set { m_projectileList = value; OnPropertyChanged(); }
        }

        public ListCollectionView PistolList
        {
            get { return m_pistolList; }
            set { m_pistolList = value; OnPropertyChanged(); }
        }

        public ListCollectionView ShotgunList
        {
            get { return m_shotgunList; }
            set { m_shotgunList = value; OnPropertyChanged(); }
        }

        public ListCollectionView SmgList
        {
            get { return m_smgList; }
            set { m_smgList = value; OnPropertyChanged(); }
        }

        public ListCollectionView AssaultList
        {
            get { return m_assaultList; }
            set { m_assaultList = value; OnPropertyChanged(); }
        }

        public ListCollectionView HeavyList
        {
            get { return m_heavyList; }
            set { m_heavyList = value; OnPropertyChanged(); }
        }

        public ListCollectionView SniperList
        {
            get { return m_sniperList; }
            set { m_sniperList = value; OnPropertyChanged(); }
        }

        public ListCollectionView SpecialList
        {
            get { return m_specialList; }
            set { m_specialList = value; OnPropertyChanged(); }
        }
    }
}
