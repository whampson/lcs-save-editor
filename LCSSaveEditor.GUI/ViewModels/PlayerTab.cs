using System;
using System.Collections.Generic;
using System.Linq;
using LCSSaveEditor.Core;
using LCSSaveEditor.GUI.Types;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class PlayerTab : TabPageBase
    {
        private bool m_isReading;
        private bool m_isWriting;
        private Weapon? m_slot0Weapon;
        private Weapon? m_slot1Weapon;
        private Weapon? m_slot2Weapon;
        private Weapon? m_slot3Weapon;
        private Weapon? m_slot4Weapon;
        private Weapon? m_slot5Weapon;
        private Weapon? m_slot6Weapon;
        private Weapon? m_slot7Weapon;
        private Weapon? m_slot8Weapon;
        private Weapon? m_slot9Weapon;
        private int m_slot1Ammo;
        private int m_slot2Ammo;
        private int m_slot3Ammo;
        private int m_slot4Ammo;
        private int m_slot5Ammo;
        private int m_slot6Ammo;
        private int m_slot7Ammo;
        private int m_slot8Ammo;
        private int m_slot9Ammo;

        public int CurrentOutfit
        {
            get { return TheEditor.GetGlobal(GlobalVariable.PlayerCostume); }
            set { TheEditor.SetGlobal(GlobalVariable.PlayerCostume, value); OnPropertyChanged(); }
        }

        public int Armor
        {
            get { return TheEditor.GetGlobal(GlobalVariable.PlayerArmor); }
            set { TheEditor.SetGlobal(GlobalVariable.PlayerArmor, value); OnPropertyChanged(); }
        }

        public int Money
        {
            get { return TheEditor.GetGlobal(GlobalVariable.PlayerMoney); }
            set
            {
                TheSave.PlayerInfo.Money = value;
                TheSave.PlayerInfo.MoneyOnScreen = value;
                TheEditor.SetGlobal(GlobalVariable.PlayerMoney, value);
                OnPropertyChanged();
            }
        }

        public int Slot1Ammo
        {
            get { return m_slot1Ammo; }
            set { m_slot1Ammo = value; OnPropertyChanged(); WriteSlot(1); }
        }

        public int Slot2Ammo
        {
            get { return m_slot2Ammo; }
            set { m_slot2Ammo = value; OnPropertyChanged(); WriteSlot(2); }
        }

        public int Slot3Ammo
        {
            get { return m_slot3Ammo; }
            set { m_slot3Ammo = value; OnPropertyChanged(); WriteSlot(3); }
        }

        public int Slot4Ammo
        {
            get { return m_slot4Ammo; }
            set { m_slot4Ammo = value; OnPropertyChanged(); WriteSlot(4); }
        }

        public int Slot5Ammo
        {
            get { return m_slot5Ammo; }
            set { m_slot5Ammo = value; OnPropertyChanged(); WriteSlot(5); }
        }

        public int Slot6Ammo
        {
            get { return m_slot6Ammo; }
            set { m_slot6Ammo = value; OnPropertyChanged(); WriteSlot(6); }
        }

        public int Slot7Ammo
        {
            get { return m_slot7Ammo; }
            set { m_slot7Ammo = value; OnPropertyChanged(); WriteSlot(7); }
        }

        public int Slot8Ammo
        {
            get { return m_slot8Ammo; }
            set { m_slot8Ammo = value; OnPropertyChanged(); WriteSlot(8); }
        }

        public int Slot9Ammo
        {
            get { return m_slot9Ammo; }
            set { m_slot9Ammo = value; OnPropertyChanged(); WriteSlot(9); }
        }

        public Weapon? Slot0Weapon
        {
            get { return m_slot0Weapon; }
            set { m_slot0Weapon = value; OnPropertyChanged(); WriteSlot(0); }
        }

        public Weapon? Slot1Weapon
        {
            get { return m_slot1Weapon; }
            set { m_slot1Weapon = value; OnPropertyChanged(); WriteSlot(1); }
        }

        public Weapon? Slot2Weapon
        {
            get { return m_slot2Weapon; }
            set { m_slot2Weapon = value; OnPropertyChanged(); WriteSlot(2); }
        }

        public Weapon? Slot3Weapon
        {
            get { return m_slot3Weapon; }
            set { m_slot3Weapon = value; OnPropertyChanged(); WriteSlot(3); }
        }

        public Weapon? Slot4Weapon
        {
            get { return m_slot4Weapon; }
            set { m_slot4Weapon = value; OnPropertyChanged(); WriteSlot(4); }
        }

        public Weapon? Slot5Weapon
        {
            get { return m_slot5Weapon; }
            set { m_slot5Weapon = value; OnPropertyChanged(); WriteSlot(5); }
        }

        public Weapon? Slot6Weapon
        {
            get { return m_slot6Weapon; }
            set { m_slot6Weapon = value; OnPropertyChanged(); WriteSlot(6); }
        }

        public Weapon? Slot7Weapon
        {
            get { return m_slot7Weapon; }
            set { m_slot7Weapon = value; OnPropertyChanged(); WriteSlot(7); }
        }

        public Weapon? Slot8Weapon
        {
            get { return m_slot8Weapon; }
            set { m_slot8Weapon = value; OnPropertyChanged(); WriteSlot(8); }
        }

        public Weapon? Slot9Weapon
        {
            get { return m_slot9Weapon; }
            set { m_slot9Weapon = value; OnPropertyChanged(); WriteSlot(9); }
        }



        // TODO: global variable change listener

        public PlayerTab(MainWindow window)
            : base("Player", TabPageVisibility.WhenFileIsOpen, window)
        { }

        public override void Update()
        {
            base.Update();

            m_isReading = true;

            var slot0 = ReadSlot(0);
            var slot1 = ReadSlot(1);
            var slot2 = ReadSlot(2);
            var slot3 = ReadSlot(3);
            var slot4 = ReadSlot(4);
            var slot5 = ReadSlot(5);
            var slot6 = ReadSlot(6);
            var slot7 = ReadSlot(7);
            var slot8 = ReadSlot(8);
            var slot9 = ReadSlot(9);

            Slot0Weapon = slot0.Item1;
            Slot1Weapon = slot1.Item1;
            Slot2Weapon = slot2.Item1;
            Slot3Weapon = slot3.Item1;
            Slot4Weapon = slot4.Item1;
            Slot5Weapon = slot5.Item1;
            Slot6Weapon = slot6.Item1;
            Slot7Weapon = slot7.Item1;
            Slot8Weapon = slot8.Item1;
            Slot9Weapon = slot9.Item1;

            Slot1Ammo = slot1.Item2;
            Slot2Ammo = slot2.Item2;
            Slot3Ammo = slot3.Item2;
            Slot4Ammo = slot4.Item2;
            Slot5Ammo = slot5.Item2;
            Slot6Ammo = slot6.Item2;
            Slot7Ammo = slot7.Item2;
            Slot8Ammo = slot8.Item2;
            Slot9Ammo = slot9.Item2;

            m_isReading = false;
        }

        private (Weapon?, int) ReadSlot(int index)
        {
            Weapon? weapon = null;
            int ammo = 0;

            if (index == 0)
            {
                bool hasBrassKnuckles = TheEditor.GetGlobal(WeaponSlotVars[Weapon.BrassKnuckles]) != 0;
                return ((hasBrassKnuckles) ? Weapon.BrassKnuckles : Weapon.Fists, 1);
            }

            IReadOnlyList<Weapon> slotWeapons = index switch
            {
                1 => Slot1Weapons,
                2 => Slot2Weapons,
                3 => Slot3Weapons,
                4 => Slot4Weapons,
                5 => Slot5Weapons,
                6 => Slot6Weapons,
                7 => Slot7Weapons,
                8 => Slot8Weapons,
                9 => Slot9Weapons,
                _ => throw new InvalidOperationException($"Bad weapon slot number: {index}"),
            };

            foreach (Weapon w in slotWeapons)
            {
                int a = TheEditor.GetGlobal(WeaponSlotVars[w]);
                if (a != 0)
                {
                    ammo = a;
                    weapon = w;
                    break;
                }
            }

            return (weapon, ammo);
        }

        private void WriteSlot(int index)
        {
            IReadOnlyList<Weapon> slotWeapons;
            Weapon? weapon;
            int ammo;

            if (m_isReading || m_isWriting)
            {
                return;
            }

            if (index == 0)
            {
                TheEditor.SetGlobal(WeaponSlotVars[Weapon.BrassKnuckles], (Slot0Weapon == Weapon.BrassKnuckles) ? 1 : 0);
                return;
            }

            m_isWriting = true;
            switch (index)
            {
                case 1: slotWeapons = Slot1Weapons; weapon = Slot1Weapon; ammo = Slot1Ammo; break;
                case 2: slotWeapons = Slot2Weapons; weapon = Slot2Weapon; ammo = Slot2Ammo; break;
                case 3: slotWeapons = Slot3Weapons; weapon = Slot3Weapon; ammo = Slot3Ammo; break;
                case 4: slotWeapons = Slot4Weapons; weapon = Slot4Weapon; ammo = Slot4Ammo; break;
                case 5: slotWeapons = Slot5Weapons; weapon = Slot5Weapon; ammo = Slot5Ammo; break;
                case 6: slotWeapons = Slot6Weapons; weapon = Slot6Weapon; ammo = Slot6Ammo; break;
                case 7: slotWeapons = Slot7Weapons; weapon = Slot7Weapon; ammo = Slot7Ammo; break;
                case 8: slotWeapons = Slot8Weapons; weapon = Slot8Weapon; ammo = Slot8Ammo; break;
                case 9: slotWeapons = Slot9Weapons; weapon = Slot9Weapon; ammo = Slot9Ammo; break;
                default: throw new InvalidOperationException($"Bad weapon slot number: {index}");
            }
            
            if (weapon == null && ammo > 0)
            {
                weapon = slotWeapons.First();
            }
            foreach (Weapon w in slotWeapons)
            {
                TheEditor.SetGlobal(WeaponSlotVars[w], (w == weapon) ? ammo : 0);
            }
            if (ammo == 0)
            {
                weapon = null;
            }

            switch (index)
            {
                case 1: Slot1Weapon = weapon; break;
                case 2: Slot2Weapon = weapon; break;
                case 3: Slot3Weapon = weapon; break;
                case 4: Slot4Weapon = weapon; break;
                case 5: Slot5Weapon = weapon; break;
                case 6: Slot6Weapon = weapon; break;
                case 7: Slot7Weapon = weapon; break;
                case 8: Slot8Weapon = weapon; break;
                case 9: Slot9Weapon = weapon; break;
            }
            m_isWriting = false;
        }

        public static IReadOnlyList<Weapon> Slot0Weapons = new List<Weapon>()
        {
            Weapon.Fists,
            Weapon.BrassKnuckles,
        };

        public static IReadOnlyList<Weapon> Slot1Weapons = new List<Weapon>()
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
        };

        public static IReadOnlyList<Weapon> Slot2Weapons = new List<Weapon>()
        {
            Weapon.Molotovs,
            Weapon.Grenades,
            Weapon.RemoteGrenades,
            Weapon.TearGas,
        };

        public static IReadOnlyList<Weapon> Slot3Weapons = new List<Weapon>()
        {
            Weapon.Pistol,
            Weapon.Python,
        };

        public static IReadOnlyList<Weapon> Slot4Weapons = new List<Weapon>()
        {
            Weapon.Shotgun,
            Weapon.StubbyShotgun,
            Weapon.Spas12,
        };

        public static IReadOnlyList<Weapon> Slot5Weapons = new List<Weapon>()
        {
            Weapon.Tec9,
            Weapon.Mac10,
            Weapon.MP5,
            Weapon.MicroSMG
        };

        public static IReadOnlyList<Weapon> Slot6Weapons = new List<Weapon>()
        {
            Weapon.AK,
            Weapon.M4,
        };

        public static IReadOnlyList<Weapon> Slot7Weapons = new List<Weapon>()
        {
            Weapon.FlameThrower,
            Weapon.RocketLauncher,
            Weapon.Minigun,
            Weapon.M60,
        };

        public static IReadOnlyList<Weapon> Slot8Weapons = new List<Weapon>()
        {
            Weapon.Sniper,
            Weapon.LaserSniper,
        };

        public static IReadOnlyList<Weapon> Slot9Weapons = new List<Weapon>()
        {
            Weapon.Camera,
        };

        public static IReadOnlyDictionary<Weapon, GlobalVariable> WeaponSlotVars = new Dictionary<Weapon, GlobalVariable>()
        {
            { Weapon.Camera, GlobalVariable.Weapon36Ammo },
            { Weapon.BrassKnuckles, GlobalVariable.Weapon1Ammo },
            { Weapon.Chisel, GlobalVariable.Weapon2Ammo },
            { Weapon.Axe, GlobalVariable.Weapon7Ammo },
            { Weapon.HockeyStick, GlobalVariable.Weapon3Ammo },
            { Weapon.NightStick, GlobalVariable.Weapon4Ammo },
            { Weapon.BaseballBat, GlobalVariable.Weapon6Ammo },
            { Weapon.Cleaver, GlobalVariable.Weapon8Ammo },
            { Weapon.Katana, GlobalVariable.Weapon10Ammo },
            { Weapon.Knife, GlobalVariable.Weapon5Ammo },
            { Weapon.Machete, GlobalVariable.Weapon9Ammo },
            { Weapon.Chainsaw, GlobalVariable.Weapon11Ammo },
            { Weapon.Grenades, GlobalVariable.Weapon12Ammo },
            { Weapon.Molotovs, GlobalVariable.Weapon15Ammo },
            { Weapon.TearGas, GlobalVariable.Weapon14Ammo },
            { Weapon.RemoteGrenades, GlobalVariable.Weapon13Ammo },
            { Weapon.Pistol, GlobalVariable.Weapon17Ammo },
            { Weapon.Python, GlobalVariable.Weapon18Ammo },
            { Weapon.Shotgun, GlobalVariable.Weapon19Ammo },
            { Weapon.Spas12, GlobalVariable.Weapon20Ammo },
            { Weapon.StubbyShotgun, GlobalVariable.Weapon21Ammo },
            { Weapon.Tec9, GlobalVariable.Weapon22Ammo },
            { Weapon.Mac10, GlobalVariable.Weapon23Ammo },
            { Weapon.MicroSMG, GlobalVariable.Weapon24Ammo },
            { Weapon.MP5, GlobalVariable.Weapon25Ammo },
            { Weapon.AK, GlobalVariable.Weapon27Ammo },
            { Weapon.M4, GlobalVariable.Weapon26Ammo },
            { Weapon.RocketLauncher, GlobalVariable.Weapon30Ammo },
            { Weapon.M60, GlobalVariable.Weapon32Ammo },
            { Weapon.FlameThrower, GlobalVariable.Weapon31Ammo },
            { Weapon.Minigun, GlobalVariable.Weapon33Ammo },
            { Weapon.Sniper, GlobalVariable.Weapon28Ammo },
            { Weapon.LaserSniper, GlobalVariable.Weapon29Ammo },
        };
    }
}
