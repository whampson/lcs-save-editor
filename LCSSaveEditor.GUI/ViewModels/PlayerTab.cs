using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        //private int m_slot0Ammo;
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
            get
            {
                return TheEditor.GetGlobal(GlobalVariable.PlayerMoney);
            }
            set
            {
                TheSave.PlayerInfo.Money = value;
                TheSave.PlayerInfo.MoneyOnScreen = value;
                TheEditor.SetGlobal(GlobalVariable.PlayerMoney, value);
                OnPropertyChanged();
            }
        }

        //public int Slot0Ammo
        //{
        //    get { return m_slot0Ammo; }
        //    set { m_slot0Ammo = value; OnPropertyChanged(); WriteSlot0(); }
        //}

        public int Slot1Ammo
        {
            get { return m_slot1Ammo; }
            set { m_slot1Ammo = value; OnPropertyChanged(); WriteSlot1(); }
        }

        public int Slot2Ammo
        {
            get { return m_slot2Ammo; }
            set { m_slot2Ammo = value; OnPropertyChanged(); WriteSlot2(); }
        }

        public int Slot3Ammo
        {
            get { return m_slot3Ammo; }
            set { m_slot3Ammo = value; OnPropertyChanged(); WriteSlot3(); }
        }

        public int Slot4Ammo
        {
            get { return m_slot4Ammo; }
            set { m_slot4Ammo = value; OnPropertyChanged(); WriteSlot4(); }
        }

        public int Slot5Ammo
        {
            get { return m_slot5Ammo; }
            set { m_slot5Ammo = value; OnPropertyChanged(); WriteSlot5(); }
        }

        public int Slot6Ammo
        {
            get { return m_slot6Ammo; }
            set { m_slot6Ammo = value; OnPropertyChanged(); WriteSlot6(); }
        }

        public int Slot7Ammo
        {
            get { return m_slot7Ammo; }
            set { m_slot7Ammo = value; OnPropertyChanged(); WriteSlot7(); }
        }

        public int Slot8Ammo
        {
            get { return m_slot8Ammo; }
            set { m_slot8Ammo = value; OnPropertyChanged(); WriteSlot8(); }
        }

        public int Slot9Ammo
        {
            get { return m_slot9Ammo; }
            set { m_slot9Ammo = value; OnPropertyChanged(); WriteSlot9(); }
        }

        public Weapon? Slot0Weapon
        {
            get { return m_slot0Weapon; }
            set { m_slot0Weapon = value; OnPropertyChanged(); WriteSlot0(); }
        }

        public Weapon? Slot1Weapon
        {
            get { return m_slot1Weapon; }
            set { m_slot1Weapon = value; OnPropertyChanged(); WriteSlot1(); }
        }

        public Weapon? Slot2Weapon
        {
            get { return m_slot2Weapon; }
            set { m_slot2Weapon = value; OnPropertyChanged(); WriteSlot2(); }
        }

        public Weapon? Slot3Weapon
        {
            get { return m_slot3Weapon; }
            set { m_slot3Weapon = value; OnPropertyChanged(); WriteSlot3(); }
        }

        public Weapon? Slot4Weapon
        {
            get { return m_slot4Weapon; }
            set { m_slot4Weapon = value; OnPropertyChanged(); WriteSlot4(); }
        }

        public Weapon? Slot5Weapon
        {
            get { return m_slot5Weapon; }
            set { m_slot5Weapon = value; OnPropertyChanged(); WriteSlot5(); }
        }

        public Weapon? Slot6Weapon
        {
            get { return m_slot6Weapon; }
            set { m_slot6Weapon = value; OnPropertyChanged(); WriteSlot6(); }
        }

        public Weapon? Slot7Weapon
        {
            get { return m_slot7Weapon; }
            set { m_slot7Weapon = value; OnPropertyChanged(); WriteSlot7(); }
        }

        public Weapon? Slot8Weapon
        {
            get { return m_slot8Weapon; }
            set { m_slot8Weapon = value; OnPropertyChanged(); WriteSlot8(); }
        }

        public Weapon? Slot9Weapon
        {
            get { return m_slot9Weapon; }
            set { m_slot9Weapon = value; OnPropertyChanged(); WriteSlot9(); }
        }



        // TODO: global variable change listener

        public PlayerTab(MainWindow window)
            : base("Player", TabPageVisibility.WhenFileIsOpen, window)
        {
        }

        public override void Update()
        {
            base.Update();

            m_isReading = true;
            ReadSlot0();
            ReadSlot1();
            ReadSlot2();
            ReadSlot3();
            ReadSlot4();
            ReadSlot5();
            ReadSlot6();
            ReadSlot7();
            ReadSlot8();
            ReadSlot9();
            m_isReading = false;
        }

        private void ReadSlot0()
        {
            Slot0Weapon = (TheEditor.GetGlobal(WeaponSlotVars[Weapon.BrassKnuckles]) != 0)
                ? Weapon.BrassKnuckles
                : Weapon.Fists;
        }

        private void ReadSlot1()
        {
            Slot1Weapon = null;
            Slot1Ammo = 0;

            foreach (Weapon w in Slot1Weapons)
            {
                int ammo = TheEditor.GetGlobal(WeaponSlotVars[w]);
                if (ammo != 0)
                {
                    Slot1Ammo = ammo;
                    Slot1Weapon = w;
                    break;
                }
            }
        }

        private void ReadSlot2()
        {
            Slot2Weapon = null;
            Slot2Ammo = 0;

            foreach (Weapon w in Slot2Weapons)
            {
                int ammo = TheEditor.GetGlobal(WeaponSlotVars[w]);
                if (ammo != 0)
                {
                    Slot2Ammo = ammo;
                    Slot2Weapon = w;
                    break;
                }
            }
        }

        private void ReadSlot3()
        {
            Slot3Weapon = null;
            Slot3Ammo = 0;

            foreach (Weapon w in Slot3Weapons)
            {
                int ammo = TheEditor.GetGlobal(WeaponSlotVars[w]);
                if (ammo != 0)
                {
                    Slot3Ammo = ammo;
                    Slot3Weapon = w;
                    break;
                }
            }
        }

        private void ReadSlot4()
        {
            Slot4Weapon = null;
            Slot4Ammo = 0;

            foreach (Weapon w in Slot4Weapons)
            {
                int ammo = TheEditor.GetGlobal(WeaponSlotVars[w]);
                if (ammo != 0)
                {
                    Slot4Ammo = ammo;
                    Slot4Weapon = w;
                    break;
                }
            }
        }

        private void ReadSlot5()
        {
            Slot5Weapon = null;
            Slot5Ammo = 0;

            foreach (Weapon w in Slot5Weapons)
            {
                int ammo = TheEditor.GetGlobal(WeaponSlotVars[w]);
                if (ammo != 0)
                {
                    Slot5Ammo = ammo;
                    Slot5Weapon = w;
                    break;
                }
            }
        }

        private void ReadSlot6()
        {
            Slot6Weapon = null;
            Slot6Ammo = 0;

            foreach (Weapon w in Slot6Weapons)
            {
                int ammo = TheEditor.GetGlobal(WeaponSlotVars[w]);
                if (ammo != 0)
                {
                    Slot6Ammo = ammo;
                    Slot6Weapon = w;
                    break;
                }
            }
        }

        private void ReadSlot7()
        {
            Slot7Weapon = null;
            Slot7Ammo = 0;

            foreach (Weapon w in Slot7Weapons)
            {
                int ammo = TheEditor.GetGlobal(WeaponSlotVars[w]);
                if (ammo != 0)
                {
                    Slot7Ammo = ammo;
                    Slot7Weapon = w;
                    break;
                }
            }
        }

        private void ReadSlot8()
        {
            Slot8Weapon = null;
            Slot8Ammo = 0;

            foreach (Weapon w in Slot8Weapons)
            {
                int ammo = TheEditor.GetGlobal(WeaponSlotVars[w]);
                if (ammo != 0)
                {
                    Slot8Ammo = ammo;
                    Slot8Weapon = w;
                    break;
                }
            }
        }

        private void ReadSlot9()
        {
            Slot9Weapon = null;
            Slot9Ammo = 0;

            foreach (Weapon w in Slot9Weapons)
            {
                int ammo = TheEditor.GetGlobal(WeaponSlotVars[w]);
                if (ammo != 0)
                {
                    Slot9Ammo = ammo;
                    Slot9Weapon = w;
                    break;
                }
            }
        }

        public void WriteSlot0()
        {
            if (!m_isReading)
            {
                TheEditor.SetGlobal(WeaponSlotVars[Weapon.BrassKnuckles], (Slot0Weapon == Weapon.BrassKnuckles) ? 1 : 0);
            }
        }

        public void WriteSlot1()
        {
            if (!m_isReading && !m_isWriting)
            {
                m_isWriting = true;
                if (Slot1Weapon == null && Slot1Ammo > 0) Slot1Weapon = Slot1Weapons.First();

                foreach (Weapon w in Slot1Weapons)
                {
                    TheEditor.SetGlobal(WeaponSlotVars[w], (w == Slot1Weapon) ? Slot1Ammo : 0);
                }

                if (Slot1Ammo == 0) Slot1Weapon = null;
                m_isWriting = false;
            }
        }

        public void WriteSlot2()
        {
            if (!m_isReading && !m_isWriting)
            {
                m_isWriting = true;
                if (Slot2Weapon == null && Slot2Ammo > 0) Slot2Weapon = Slot2Weapons.First();

                foreach (Weapon w in Slot2Weapons)
                {
                    TheEditor.SetGlobal(WeaponSlotVars[w], (w == Slot2Weapon) ? Slot2Ammo : 0);
                }

                if (Slot2Ammo == 0) Slot2Weapon = null;
                m_isWriting = false;
            }
        }

        public void WriteSlot3()
        {
            if (!m_isReading && !m_isWriting)
            {
                m_isWriting = true;
                if (Slot3Weapon == null && Slot3Ammo > 0) Slot3Weapon = Slot3Weapons.First();

                foreach (Weapon w in Slot3Weapons)
                {
                    TheEditor.SetGlobal(WeaponSlotVars[w], (w == Slot3Weapon) ? Slot3Ammo : 0);
                }

                if (Slot3Ammo == 0) Slot3Weapon = null;
                m_isWriting = false;
            }
        }

        public void WriteSlot4()
        {
            if (!m_isReading && !m_isWriting)
            {
                m_isWriting = true;
                if (Slot4Weapon == null && Slot4Ammo > 0) Slot4Weapon = Slot4Weapons.First();

                foreach (Weapon w in Slot4Weapons)
                {
                    TheEditor.SetGlobal(WeaponSlotVars[w], (w == Slot4Weapon) ? Slot4Ammo : 0);
                }

                if (Slot4Ammo == 0) Slot4Weapon = null;
                m_isWriting = false;
            }
        }

        public void WriteSlot5()
        {
            if (!m_isReading && !m_isWriting)
            {
                m_isWriting = true;
                if (Slot5Weapon == null && Slot5Ammo > 0) Slot5Weapon = Slot5Weapons.First();

                foreach (Weapon w in Slot5Weapons)
                {
                    TheEditor.SetGlobal(WeaponSlotVars[w], (w == Slot5Weapon) ? Slot5Ammo : 0);
                }

                if (Slot5Ammo == 0) Slot5Weapon = null;
                m_isWriting = false;
            }
        }

        public void WriteSlot6()
        {
            if (!m_isReading && !m_isWriting)
            {
                m_isWriting = true;
                if (Slot6Weapon == null && Slot6Ammo > 0) Slot6Weapon = Slot6Weapons.First();

                foreach (Weapon w in Slot6Weapons)
                {
                    TheEditor.SetGlobal(WeaponSlotVars[w], (w == Slot6Weapon) ? Slot6Ammo : 0);
                }

                if (Slot6Ammo == 0) Slot6Weapon = null;
                m_isWriting = false;
            }
        }

        public void WriteSlot7()
        {
            if (!m_isReading && !m_isWriting)
            {
                m_isWriting = true;
                if (Slot7Weapon == null && Slot7Ammo > 0) Slot7Weapon = Slot7Weapons.First();

                foreach (Weapon w in Slot7Weapons)
                {
                    TheEditor.SetGlobal(WeaponSlotVars[w], (w == Slot7Weapon) ? Slot7Ammo : 0);
                }

                if (Slot7Ammo == 0) Slot7Weapon = null;
                m_isWriting = false;
            }
        }

        public void WriteSlot8()
        {
            if (!m_isReading && !m_isWriting)
            {
                m_isWriting = true;
                if (Slot8Weapon == null && Slot8Ammo > 0) Slot8Weapon = Slot8Weapons.First();

                foreach (Weapon w in Slot8Weapons)
                {
                    TheEditor.SetGlobal(WeaponSlotVars[w], (w == Slot8Weapon) ? Slot8Ammo : 0);
                }

                if (Slot8Ammo == 0) Slot8Weapon = null;
                m_isWriting = false;
            }
        }

        public void WriteSlot9()
        {
            if (!m_isReading && !m_isWriting)
            {
                m_isWriting = true;
                if (Slot9Weapon == null && Slot9Ammo > 0) Slot9Weapon = Slot9Weapons.First();

                foreach (Weapon w in Slot9Weapons)
                {
                    TheEditor.SetGlobal(WeaponSlotVars[w], (w == Slot9Weapon) ? Slot9Ammo : 0);
                }

                if (Slot9Ammo == 0) Slot9Weapon = null;
                m_isWriting = false;
            }
        }

        public static IReadOnlyList<Weapon> Slot0Weapons = new List<Weapon>()
        {
            Weapon.Fists,
            Weapon.BrassKnuckles,
        };

        public static IReadOnlyList<Weapon> Slot1Weapons = new List<Weapon>()
        {
            //Weapon.None,
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
            //Weapon.None,
            Weapon.Molotovs,
            Weapon.Grenades,
            Weapon.RemoteGrenades,
            Weapon.TearGas,
        };

        public static IReadOnlyList<Weapon> Slot3Weapons = new List<Weapon>()
        {
            //Weapon.None,
            Weapon.Pistol,
            Weapon.Python,
        };

        public static IReadOnlyList<Weapon> Slot4Weapons = new List<Weapon>()
        {
            //Weapon.None,
            Weapon.Shotgun,
            Weapon.StubbyShotgun,
            Weapon.Spas12,
        };

        public static IReadOnlyList<Weapon> Slot5Weapons = new List<Weapon>()
        {
            //Weapon.None,
            Weapon.Tec9,
            Weapon.Mac10,
            Weapon.MP5,
            Weapon.MicroSMG
        };

        public static IReadOnlyList<Weapon> Slot6Weapons = new List<Weapon>()
        {
            //Weapon.None,
            Weapon.AK,
            Weapon.M4,
        };

        public static IReadOnlyList<Weapon> Slot7Weapons = new List<Weapon>()
        {
            //Weapon.None,
            Weapon.FlameThrower,
            Weapon.RocketLauncher,
            Weapon.Minigun,
            Weapon.M60,
        };

        public static IReadOnlyList<Weapon> Slot8Weapons = new List<Weapon>()
        {
            //Weapon.None,
            Weapon.Sniper,
            Weapon.LaserSniper,
        };

        public static IReadOnlyList<Weapon> Slot9Weapons = new List<Weapon>()
        {
            //Weapon.None,
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
