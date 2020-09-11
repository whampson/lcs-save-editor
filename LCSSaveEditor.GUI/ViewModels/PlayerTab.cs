using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GTASaveData.LCS;
using GTASaveData.Types;
using LCSSaveEditor.Core;
using LCSSaveEditor.GUI.Types;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class PlayerTab : TabPageBase
    {
        public static readonly Point MapOrigin = new Point(1024, 1024);
        public static readonly Point MapScale = new Point(0.512, -0.512);

        private bool m_isReadingWeaponSlot;
        private bool m_isWritingWeaponSlot;
        private bool m_suppressWritingSelectedWeapon;
        private Weapon? m_currentWeapon;
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
        private ObservableCollection<Weapon?> m_inventory;
        private ObservableCollection<UIElement> m_mapOverlays;
        private Point m_centerOffset;
        private Point m_mouseOffset;
        private Point m_mouseCoords;
        private double m_zoomLevel;

        public PlayerOutfit Outfit
        {
            get { return (PlayerOutfit) TheEditor.GetGlobal(GlobalVariable.PlayerOutfit); }
            set { TheEditor.SetGlobal(GlobalVariable.PlayerOutfit, (int) value); OnPropertyChanged(); }
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

        public Weapon? Weapon
        {
            get { return m_currentWeapon; }
            set { m_currentWeapon = value; WriteSelectedWeapon(); OnPropertyChanged(); }
        }

        // 1 Portland: 1138.803 -242.4054 22.0082
        // 2 Staunton: 272.1489 -417.604 60.1342 
        // 3 Shorside: -845.29 304.41 40.95

        public Vector3D SpawnPoint
        {
            get
            {
                return new Vector3D()
                {
                    X = TheEditor.GetGlobalAsFloat(GlobalVariable.PlayerX),
                    Y = TheEditor.GetGlobalAsFloat(GlobalVariable.PlayerY),
                    Z = TheEditor.GetGlobalAsFloat(GlobalVariable.PlayerZ),
                };
            }

            set
            {
                TheEditor.SetGlobal(GlobalVariable.PlayerX, value.X);
                TheEditor.SetGlobal(GlobalVariable.PlayerY, value.Y);
                TheEditor.SetGlobal(GlobalVariable.PlayerZ, value.Z);
                UpdateSpawnPoint();
                OnPropertyChanged();
            }
        }

        public int SpawnHeading
        {
            get { return (int) TheEditor.GetGlobalAsFloat(GlobalVariable.PlayerHeading); }
            set { TheEditor.SetGlobal(GlobalVariable.PlayerHeading, (float) value); OnPropertyChanged(); }
        }

        public int SpawnInterior
        {
            get { return (int) TheEditor.GetGlobal(GlobalVariable.CurrentInterior); }
            set { TheEditor.SetGlobal(GlobalVariable.CurrentInterior, value); OnPropertyChanged(); }
        }

        public int Slot1Ammo
        {
            get { return m_slot1Ammo; }
            set { m_slot1Ammo = value; WriteSlot(1); OnPropertyChanged(); }
        }

        public int Slot2Ammo
        {
            get { return m_slot2Ammo; }
            set { m_slot2Ammo = value; WriteSlot(2); OnPropertyChanged(); }
        }

        public int Slot3Ammo
        {
            get { return m_slot3Ammo; }
            set { m_slot3Ammo = value; WriteSlot(3); OnPropertyChanged(); }
        }

        public int Slot4Ammo
        {
            get { return m_slot4Ammo; }
            set { m_slot4Ammo = value; WriteSlot(4); OnPropertyChanged(); }
        }

        public int Slot5Ammo
        {
            get { return m_slot5Ammo; }
            set { m_slot5Ammo = value; WriteSlot(5); OnPropertyChanged(); }
        }

        public int Slot6Ammo
        {
            get { return m_slot6Ammo; }
            set { m_slot6Ammo = value; WriteSlot(6); OnPropertyChanged(); }
        }

        public int Slot7Ammo
        {
            get { return m_slot7Ammo; }
            set { m_slot7Ammo = value; WriteSlot(7); OnPropertyChanged(); }
        }

        public int Slot8Ammo
        {
            get { return m_slot8Ammo; }
            set { m_slot8Ammo = value; WriteSlot(8); OnPropertyChanged(); }
        }

        public int Slot9Ammo
        {
            get { return m_slot9Ammo; }
            set { m_slot9Ammo = value; WriteSlot(9); OnPropertyChanged(); }
        }

        public Weapon? Slot0Weapon
        {
            get { return m_slot0Weapon; }
            set { m_slot0Weapon = value; WriteSlot(0); OnPropertyChanged(); }
        }

        public Weapon? Slot1Weapon
        {
            get { return m_slot1Weapon; }
            set { m_slot1Weapon = value; WriteSlot(1); OnPropertyChanged(); }
        }

        public Weapon? Slot2Weapon
        {
            get { return m_slot2Weapon; }
            set { m_slot2Weapon = value; WriteSlot(2); OnPropertyChanged(); }
        }

        public Weapon? Slot3Weapon
        {
            get { return m_slot3Weapon; }
            set { m_slot3Weapon = value; WriteSlot(3); OnPropertyChanged(); }
        }

        public Weapon? Slot4Weapon
        {
            get { return m_slot4Weapon; }
            set { m_slot4Weapon = value; WriteSlot(4); OnPropertyChanged(); }
        }

        public Weapon? Slot5Weapon
        {
            get { return m_slot5Weapon; }
            set { m_slot5Weapon = value; WriteSlot(5); OnPropertyChanged(); }
        }

        public Weapon? Slot6Weapon
        {
            get { return m_slot6Weapon; }
            set { m_slot6Weapon = value; WriteSlot(6); OnPropertyChanged(); }
        }

        public Weapon? Slot7Weapon
        {
            get { return m_slot7Weapon; }
            set { m_slot7Weapon = value; WriteSlot(7); OnPropertyChanged(); }
        }

        public Weapon? Slot8Weapon
        {
            get { return m_slot8Weapon; }
            set { m_slot8Weapon = value; WriteSlot(8); OnPropertyChanged(); }
        }

        public Weapon? Slot9Weapon
        {
            get { return m_slot9Weapon; }
            set { m_slot9Weapon = value; WriteSlot(9); OnPropertyChanged(); }
        }

        public ObservableCollection<Weapon?> Inventory
        {
            get { return m_inventory; }
            set { m_inventory = value; OnPropertyChanged(); }
        }

        public ObservableCollection<UIElement> MapOverlays
        {
            get { return m_mapOverlays; }
            set { m_mapOverlays = value; OnPropertyChanged(); }
        }

        public Point CenterOffset
        {
            get { return m_centerOffset; }
            set { m_centerOffset = value; OnPropertyChanged(); }
        }

        public Point MouseOffset
        {
            get { return m_mouseOffset; }
            set { m_mouseOffset = value; OnPropertyChanged(); }
        }

        public Point MouseCoords
        {
            get { return m_mouseCoords; }
            set { m_mouseCoords = value; OnPropertyChanged(); }
        }

        public double ZoomLevel
        {
            get { return m_zoomLevel; }
            set { m_zoomLevel = value; OnPropertyChanged(); }
        }


        public PlayerTab(MainWindow window)
            : base("Player", TabPageVisibility.WhenFileIsOpen, window)
        {
            Inventory = new ObservableCollection<Weapon?>();
            MapOverlays = new ObservableCollection<UIElement>();
        }

        public override void Load()
        {
            base.Load();

            m_suppressWritingSelectedWeapon = true;
            Weapon = (Weapon?) TheEditor.GetGlobal(GlobalVariable.PlayerWeapon);
            m_suppressWritingSelectedWeapon = false;

            TheSave.Scripts.GlobalVariables.CollectionChanged += GlobalVariables_CollectionChanged;

            ReadSlot(0);
            ReadSlot(1);
            ReadSlot(2);
            ReadSlot(3);
            ReadSlot(4);
            ReadSlot(5);
            ReadSlot(6);
            ReadSlot(7);
            ReadSlot(8);
            ReadSlot(9);
            UpdateInventory();
            UpdateOutfit();
            UpdateSpawnPoint();

            OnPropertyChanged(nameof(Armor));
            OnPropertyChanged(nameof(Money));
            OnPropertyChanged(nameof(Weapon));
            OnPropertyChanged(nameof(SpawnPoint));
        }

        public override void Unload()
        {
            base.Unload();

            TheSave.Scripts.GlobalVariables.CollectionChanged -= GlobalVariables_CollectionChanged;
        }

        public void UpdateSpawnPoint()
        {
            MapOverlays.Clear();
            MapOverlays.Add(MakeTargetSprite(SpawnPoint.Get2DComponent(), scale: 32));
            OnPropertyChanged(nameof(SpawnHeading));
            OnPropertyChanged(nameof(SpawnInterior));
        }

        private UIElement MakeTargetSprite(Vector2D loc,
            int scale = 4, string toolTip = null)
        {
            const double Size = 4;
            double actualSize = Size * scale;

            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(@"pack://application:,,,/Resources/target.png");
            bmp.EndInit();

            Image img = new Image()
            {
                Source = bmp,
                Width = bmp.Width,
                Height = bmp.Height,
                ToolTip = toolTip
            };

            ApplyBlipTransform(img, loc, actualSize);
            return img;
        }

        private static void ApplyBlipTransform(FrameworkElement e, Vector2D loc,
            double size = 1)
        {
            double centerX = size / 2;
            double centerY = size;
            double scaleX = size / e.Width;
            double scaleY = size / e.Width;

            Point p = new Point()
            {
                X = (loc.X * MapScale.X) + MapOrigin.X,
                Y = (loc.Y * MapScale.Y) + MapOrigin.Y,
            };
            Matrix m = Matrix.Identity;
            m.OffsetX = p.X - centerX;
            m.OffsetY = p.Y - centerY;
            m.ScalePrepend(scaleX, scaleY);

            e.RenderTransform = new MatrixTransform(m);
        }

        public void ReadSlot(int index)
        {
            m_isReadingWeaponSlot = true;

            if (index == 0)
            {
                bool hasBrassKnuckles = TheEditor.GetGlobal(WeaponVars[Types.Weapon.BrassKnuckles]) != 0;
                Slot0Weapon = (hasBrassKnuckles) ? Types.Weapon.BrassKnuckles : Types.Weapon.Fists;
                goto Cleanup;
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

            Weapon? weapon = null;
            int ammo = 0;

            foreach (Weapon w in slotWeapons)
            {
                int a = TheEditor.GetGlobal(WeaponVars[w]);
                if (a != 0)
                {
                    ammo = a;
                    weapon = w;
                    break;
                }
            }

            switch (index)
            {
                case 1: Slot1Weapon = weapon; Slot1Ammo = ammo; break;
                case 2: Slot2Weapon = weapon; Slot2Ammo = ammo; break;
                case 3: Slot3Weapon = weapon; Slot3Ammo = ammo; break;
                case 4: Slot4Weapon = weapon; Slot4Ammo = ammo; break;
                case 5: Slot5Weapon = weapon; Slot5Ammo = ammo; break;
                case 6: Slot6Weapon = weapon; Slot6Ammo = ammo; break;
                case 7: Slot7Weapon = weapon; Slot7Ammo = ammo; break;
                case 8: Slot8Weapon = weapon; Slot8Ammo = ammo; break;
                case 9: Slot9Weapon = weapon; Slot9Ammo = ammo; break;
            }

        Cleanup:
            m_isReadingWeaponSlot = false;
        }

        public void WriteSlot(int index)
        {
            IReadOnlyList<Weapon> slotWeapons;
            Weapon? weapon;
            int ammo;

            if (m_isReadingWeaponSlot || m_isWritingWeaponSlot)
            {
                return;
            }

            m_isWritingWeaponSlot = true;

            if (index == 0)
            {
                TheEditor.SetGlobal(WeaponVars[Types.Weapon.BrassKnuckles], (Slot0Weapon == Types.Weapon.BrassKnuckles) ? 1 : 0);
                goto Cleanup;
            }

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

            foreach (Weapon w in slotWeapons)
            {
                TheEditor.SetGlobal(WeaponVars[w], (w == weapon) ? ammo : 0);
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

        Cleanup:
            UpdateInventory();
            m_isWritingWeaponSlot = false;
        }

        private void WriteSelectedWeapon()
        {
            if (!m_suppressWritingSelectedWeapon)
            {
                TheEditor.SetGlobal(GlobalVariable.PlayerWeapon, (int) (Weapon ?? Types.Weapon.Fists));
            }
        }

        public void UpdateInventory()
        {
            if (!Weapon.HasValue || !WeaponSlots.TryGetValue(Weapon.Value, out int selectedSlot))
            {
                selectedSlot = -1;
            }

            var temp = new ObservableCollection<Weapon?>
            {
                Slot0Weapon,
                Slot1Weapon,
                Slot2Weapon,
                Slot3Weapon,
                Slot4Weapon,
                Slot5Weapon,
                Slot6Weapon,
                Slot7Weapon,
                Slot8Weapon,
                Slot9Weapon
            };

            Inventory = new ObservableCollection<Weapon?>(temp.Except(temp.Where(x => x == null)));

            if (selectedSlot != -1)
            {
                Weapon? w;
                while ((w = temp[selectedSlot]) == null && selectedSlot > -1)
                {
                    selectedSlot--;
                }

                if (!m_isWritingWeaponSlot)
                {
                    m_suppressWritingSelectedWeapon = true;
                    Weapon = w;
                    m_suppressWritingSelectedWeapon = false;
                }
                else
                {
                    Weapon = w;
                }
            }
        }

        public void UpdateOutfit()
        {
            OnPropertyChanged(nameof(Outfit));
        }

        private void GlobalVariables_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            GlobalVariable g = TheEditor.GetGlobalId(e.NewStartingIndex);

            switch (g)
            {
                case GlobalVariable.PlayerX:
                case GlobalVariable.PlayerY:
                case GlobalVariable.PlayerZ:
                    OnPropertyChanged(nameof(SpawnPoint));
                    break;
                case GlobalVariable.PlayerOutfit:
                    OnPropertyChanged(nameof(Outfit));
                    break;
                case GlobalVariable.PlayerArmor:
                    OnPropertyChanged(nameof(Armor));
                    break;
                case GlobalVariable.PlayerMoney:
                    OnPropertyChanged(nameof(Money));
                    break;
                case GlobalVariable.PlayerWeapon:
                    OnPropertyChanged(nameof(Weapon));
                    break;
            }

            if (m_isWritingWeaponSlot) return;
            switch (g)
            {
                case GlobalVariable.Weapon1Ammo:        // Brass Knuckles
                    ReadSlot(0);
                    break;
                case GlobalVariable.Weapon2Ammo:        // Melee
                case GlobalVariable.Weapon3Ammo:
                case GlobalVariable.Weapon4Ammo:
                case GlobalVariable.Weapon5Ammo:
                case GlobalVariable.Weapon6Ammo:
                case GlobalVariable.Weapon7Ammo:
                case GlobalVariable.Weapon8Ammo:
                case GlobalVariable.Weapon9Ammo:
                case GlobalVariable.Weapon10Ammo:
                case GlobalVariable.Weapon11Ammo:
                    ReadSlot(1);
                    break;
                case GlobalVariable.Weapon12Ammo:       // Projectiles
                case GlobalVariable.Weapon13Ammo:
                case GlobalVariable.Weapon14Ammo:
                case GlobalVariable.Weapon15Ammo:
                    ReadSlot(2);
                    break;
                case GlobalVariable.Weapon17Ammo:       // Pistols
                case GlobalVariable.Weapon18Ammo:
                    ReadSlot(3);
                    break;
                case GlobalVariable.Weapon19Ammo:       // Shotguns
                case GlobalVariable.Weapon20Ammo:
                case GlobalVariable.Weapon21Ammo:
                    ReadSlot(4);
                    break;
                case GlobalVariable.Weapon22Ammo:       // Sub-machine Guns
                case GlobalVariable.Weapon23Ammo:
                case GlobalVariable.Weapon24Ammo:
                case GlobalVariable.Weapon25Ammo:
                    ReadSlot(5);
                    break;
                case GlobalVariable.Weapon26Ammo:       // Assault Rifles
                case GlobalVariable.Weapon27Ammo:
                    ReadSlot(6);
                    break;
                case GlobalVariable.Weapon28Ammo:       // Sniper Rifles
                case GlobalVariable.Weapon29Ammo:
                    ReadSlot(8);
                    break;
                case GlobalVariable.Weapon30Ammo:       // Heavy Weapons
                case GlobalVariable.Weapon31Ammo:
                case GlobalVariable.Weapon32Ammo:
                case GlobalVariable.Weapon33Ammo:
                    ReadSlot(7);
                    break;
                case GlobalVariable.Weapon36Ammo:       // Special
                    ReadSlot(9);
                    break;
            }
        }

        public static IReadOnlyList<Weapon> Slot0Weapons = new List<Weapon>()
        {
            Types.Weapon.Fists,
            Types.Weapon.BrassKnuckles,
        };

        public static IReadOnlyList<Weapon> Slot1Weapons = new List<Weapon>()
        {
            Types.Weapon.Chisel,
            Types.Weapon.Axe,
            Types.Weapon.HockeyStick,
            Types.Weapon.NightStick,
            Types.Weapon.BaseballBat,
            Types.Weapon.Cleaver,
            Types.Weapon.Katana,
            Types.Weapon.Knife,
            Types.Weapon.Machete,
            Types.Weapon.Chainsaw
        };

        public static IReadOnlyList<Weapon> Slot2Weapons = new List<Weapon>()
        {
            Types.Weapon.Molotovs,
            Types.Weapon.Grenades,
            Types.Weapon.RemoteGrenades,
            Types.Weapon.TearGas,
        };

        public static IReadOnlyList<Weapon> Slot3Weapons = new List<Weapon>()
        {
            Types.Weapon.Pistol,
            Types.Weapon.Python,
        };

        public static IReadOnlyList<Weapon> Slot4Weapons = new List<Weapon>()
        {
            Types.Weapon.Shotgun,
            Types.Weapon.StubbyShotgun,
            Types.Weapon.Spas12,
        };

        public static IReadOnlyList<Weapon> Slot5Weapons = new List<Weapon>()
        {
            Types.Weapon.Tec9,
            Types.Weapon.Mac10,
            Types.Weapon.MP5,
            Types.Weapon.MicroSMG
        };

        public static IReadOnlyList<Weapon> Slot6Weapons = new List<Weapon>()
        {
            Types.Weapon.AK,
            Types.Weapon.M4,
        };

        public static IReadOnlyList<Weapon> Slot7Weapons = new List<Weapon>()
        {
            Types.Weapon.FlameThrower,
            Types.Weapon.RocketLauncher,
            Types.Weapon.Minigun,
            Types.Weapon.M60,
        };

        public static IReadOnlyList<Weapon> Slot8Weapons = new List<Weapon>()
        {
            Types.Weapon.Sniper,
            Types.Weapon.LaserSniper,
        };

        public static IReadOnlyList<Weapon> Slot9Weapons = new List<Weapon>()
        {
            Types.Weapon.Camera,
        };

        public static IReadOnlyDictionary<Weapon, int> WeaponSlots = new Dictionary<Weapon, int>()
        {
            { Types.Weapon.Fists, 0 },
            { Types.Weapon.BrassKnuckles, 0 },
            { Types.Weapon.Chisel, 1 },
            { Types.Weapon.Axe, 1 },
            { Types.Weapon.HockeyStick, 1 },
            { Types.Weapon.NightStick, 1 },
            { Types.Weapon.BaseballBat, 1 },
            { Types.Weapon.Cleaver, 1 },
            { Types.Weapon.Katana, 1 },
            { Types.Weapon.Knife, 1 },
            { Types.Weapon.Machete, 1 },
            { Types.Weapon.Chainsaw, 1 },
            { Types.Weapon.Grenades, 2 },
            { Types.Weapon.Molotovs, 2 },
            { Types.Weapon.TearGas, 2 },
            { Types.Weapon.RemoteGrenades, 2 },
            { Types.Weapon.Pistol, 3 },
            { Types.Weapon.Python, 3 },
            { Types.Weapon.Shotgun, 4 },
            { Types.Weapon.Spas12, 4 },
            { Types.Weapon.StubbyShotgun, 4 },
            { Types.Weapon.Tec9, 5 },
            { Types.Weapon.Mac10, 5 },
            { Types.Weapon.MicroSMG, 5 },
            { Types.Weapon.MP5, 5 },
            { Types.Weapon.AK, 6 },
            { Types.Weapon.M4, 6 },
            { Types.Weapon.RocketLauncher, 7 },
            { Types.Weapon.M60, 7 },
            { Types.Weapon.FlameThrower, 7 },
            { Types.Weapon.Minigun, 7 },
            { Types.Weapon.Sniper, 8 },
            { Types.Weapon.LaserSniper, 8 },
            { Types.Weapon.Camera, 9 },
        };

        public static IReadOnlyDictionary<Weapon, GlobalVariable> WeaponVars = new Dictionary<Weapon, GlobalVariable>()
        {
            { Types.Weapon.Camera, GlobalVariable.Weapon36Ammo },
            { Types.Weapon.BrassKnuckles, GlobalVariable.Weapon1Ammo },
            { Types.Weapon.Chisel, GlobalVariable.Weapon2Ammo },
            { Types.Weapon.Axe, GlobalVariable.Weapon7Ammo },
            { Types.Weapon.HockeyStick, GlobalVariable.Weapon3Ammo },
            { Types.Weapon.NightStick, GlobalVariable.Weapon4Ammo },
            { Types.Weapon.BaseballBat, GlobalVariable.Weapon6Ammo },
            { Types.Weapon.Cleaver, GlobalVariable.Weapon8Ammo },
            { Types.Weapon.Katana, GlobalVariable.Weapon10Ammo },
            { Types.Weapon.Knife, GlobalVariable.Weapon5Ammo },
            { Types.Weapon.Machete, GlobalVariable.Weapon9Ammo },
            { Types.Weapon.Chainsaw, GlobalVariable.Weapon11Ammo },
            { Types.Weapon.Grenades, GlobalVariable.Weapon12Ammo },
            { Types.Weapon.Molotovs, GlobalVariable.Weapon15Ammo },
            { Types.Weapon.TearGas, GlobalVariable.Weapon14Ammo },
            { Types.Weapon.RemoteGrenades, GlobalVariable.Weapon13Ammo },
            { Types.Weapon.Pistol, GlobalVariable.Weapon17Ammo },
            { Types.Weapon.Python, GlobalVariable.Weapon18Ammo },
            { Types.Weapon.Shotgun, GlobalVariable.Weapon19Ammo },
            { Types.Weapon.Spas12, GlobalVariable.Weapon20Ammo },
            { Types.Weapon.StubbyShotgun, GlobalVariable.Weapon21Ammo },
            { Types.Weapon.Tec9, GlobalVariable.Weapon22Ammo },
            { Types.Weapon.Mac10, GlobalVariable.Weapon23Ammo },
            { Types.Weapon.MicroSMG, GlobalVariable.Weapon24Ammo },
            { Types.Weapon.MP5, GlobalVariable.Weapon25Ammo },
            { Types.Weapon.AK, GlobalVariable.Weapon27Ammo },
            { Types.Weapon.M4, GlobalVariable.Weapon26Ammo },
            { Types.Weapon.RocketLauncher, GlobalVariable.Weapon30Ammo },
            { Types.Weapon.M60, GlobalVariable.Weapon32Ammo },
            { Types.Weapon.FlameThrower, GlobalVariable.Weapon31Ammo },
            { Types.Weapon.Minigun, GlobalVariable.Weapon33Ammo },
            { Types.Weapon.Sniper, GlobalVariable.Weapon28Ammo },
            { Types.Weapon.LaserSniper, GlobalVariable.Weapon29Ammo },
        };

        public static IReadOnlyList<PlayerOutfit> Outfits = Enum.GetValues(typeof(PlayerOutfit)) as IReadOnlyList<PlayerOutfit>;
    }
}
