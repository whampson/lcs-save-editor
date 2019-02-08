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

using LcsSaveEditor.DataTypes;
using LcsSaveEditor.Models;
using LcsSaveEditor.Resources;
using System.Collections.Generic;
using System.Windows.Data;

namespace LcsSaveEditor.ViewModels
{
    public class WeaponsViewModel : PageViewModelBase
    {
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

        public WeaponsViewModel()
            : base(Strings.PageHeaderWeapons)
        {
            InitWeaponLists();
        }

        public WeaponsViewModel(SaveData saveData)
            : this()
        { }

        public Weapon? Hand
        {
            get { return m_hand; }
            set { m_hand = value; OnPropertyChanged(); }
        }

        public Weapon? MeleeWeapon
        {
            get { return m_melee; }
            set { m_melee = value; OnPropertyChanged(); }
        }

        public Weapon? Projectile
        {
            get { return m_projectile; }
            set { m_projectile = value; OnPropertyChanged(); }
        }

        public Weapon? Pistol
        {
            get { return m_pistol; }
            set { m_pistol = value; OnPropertyChanged(); }
        }

        public Weapon? Shotgun
        {
            get { return m_shotgun; }
            set { m_shotgun = value; OnPropertyChanged(); }
        }

        public Weapon? Smg
        {
            get { return m_smg; }
            set { m_smg = value; OnPropertyChanged(); }
        }

        public Weapon? Assault
        {
            get { return m_assault; }
            set { m_assault = value; OnPropertyChanged(); }
        }

        public Weapon? Heavy
        {
            get { return m_heavy; }
            set { m_heavy = value; OnPropertyChanged(); }
        }

        public Weapon? Sniper
        {
            get { return m_sniper; }
            set { m_sniper = value; OnPropertyChanged(); }
        }

        public Weapon? Special
        {
            get { return m_special; }
            set { m_special = value; OnPropertyChanged(); }
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

        private void InitWeaponLists()
        {
            m_handList = new ListCollectionView(new List<Weapon>
            {
                Weapon.Fists,
                Weapon.BrassKnuckles
            });

            m_meleeList = new ListCollectionView(new List<Weapon>
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

            m_projectileList = new ListCollectionView(new List<Weapon>
            {
                Weapon.Molotovs,
                Weapon.Grenades,
                Weapon.RemoteGrenades,
                Weapon.TearGas,
            });

            m_pistolList = new ListCollectionView(new List<Weapon>
            {
                Weapon.Pistol,
                Weapon.Python
            });

            m_shotgunList = new ListCollectionView(new List<Weapon>
            {
                Weapon.Shotgun,
                Weapon.StubbyShotgun,
                Weapon.Spas12,
            });

            m_smgList = new ListCollectionView(new List<Weapon>
            {
                Weapon.Tec9,
                Weapon.Mac10,
                Weapon.Mp5,
                Weapon.MicroSmg
            });

            m_assaultList = new ListCollectionView(new List<Weapon>
            {
                Weapon.Ak,
                Weapon.M4
            });

            m_heavyList = new ListCollectionView(new List<Weapon>
            {
                Weapon.FlameThrower,
                Weapon.RocketLauncher,
                Weapon.Minigun,
                Weapon.M60,
            });

            m_sniperList = new ListCollectionView(new List<Weapon>
            {
                Weapon.SniperRifle,
                Weapon.LaserSightedSniperRifle
            });

            m_specialList = new ListCollectionView(new List<Weapon>
            {
                Weapon.Camera
            });
        }
    }
}
