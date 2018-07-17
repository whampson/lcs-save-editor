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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WHampson.LcsSaveEditor.FileStructure;

namespace WHampson.LcsSaveEditor
{
    public class Game : GameStateObject
    {
        public static Game ActiveGameState
        {
            get;
            set;
        }

        public Game(SaveDataFile data)
        {
            GameData = data ?? throw new ArgumentNullException(nameof(data));
            Player = new Player(data);
            Player.PropertyChanged += FirePropertyChanged;
        }

        public Player Player
        {
            get;
        }

        private SaveDataFile GameData
        {
            get;
        }

        public void SaveToFile(string path)
        {
            GameData.Store(path);
        }
    }

    public abstract class GameStateObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void FirePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }

        protected void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Player : GameStateObject
    {
        public Player(SaveDataFile data)
        {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }

            Weapons = new Weapons(data);
            Weapons.PropertyChanged += FirePropertyChanged;
        }

        public Weapons Weapons
        {
            get;
        }
    }

    public class Weapons : GameStateObject
    {
        private readonly ScriptVariable[] globals;
        private readonly Dictionary<Weapon, int> weaponAmmoIndex;

        public Weapons(SaveDataFile data)
        {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }

            globals = data.Scripts.GlobalVariables;
            weaponAmmoIndex = new Dictionary<Weapon, int>();

            switch (data.GameVersion) {
                case GameVersion.Ps2:
                    InitPs2Weapons();
                    break;
            }
        }

        private void InitPs2Weapons()
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
            weaponAmmoIndex[Weapon.Molotov] = 137;
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

        public int CameraAmmo
        {
            get { return globals[weaponAmmoIndex[Weapon.Camera]]; }
            set {
                globals[weaponAmmoIndex[Weapon.Camera]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public bool BrassKnucklesEquipped
        {
            get { return globals[weaponAmmoIndex[Weapon.BrassKnuckles]].ValueAsBool; }
            set {
                globals[weaponAmmoIndex[Weapon.BrassKnuckles]].ValueAsBool = value;
                OnPropertyChanged();
            }
        }

        public bool ChiselEquipped
        {
            get { return globals[weaponAmmoIndex[Weapon.Chisel]].ValueAsBool; }
            set {
                globals[weaponAmmoIndex[Weapon.Chisel]].ValueAsBool = value;
                OnPropertyChanged();
            }
        }

        public bool AxeEquipped
        {
            get { return globals[weaponAmmoIndex[Weapon.Axe]].ValueAsBool; }
            set {
                globals[weaponAmmoIndex[Weapon.Axe]].ValueAsBool = value;
                OnPropertyChanged();
            }
        }

        public bool HockeyStickEquipped
        {
            get { return globals[weaponAmmoIndex[Weapon.HockeyStick]].ValueAsBool; }
            set {
                globals[weaponAmmoIndex[Weapon.HockeyStick]].ValueAsBool = value;
                OnPropertyChanged();
            }
        }

        public bool NightStickEquipped
        {
            get { return globals[weaponAmmoIndex[Weapon.NightStick]].ValueAsBool; }
            set {
                globals[weaponAmmoIndex[Weapon.NightStick]].ValueAsBool = value;
                OnPropertyChanged();
            }
        }

        public bool BaseballBatEquipped
        {
            get { return globals[weaponAmmoIndex[Weapon.BaseballBat]].ValueAsBool; }
            set {
                globals[weaponAmmoIndex[Weapon.BaseballBat]].ValueAsBool = value;
                OnPropertyChanged();
            }
        }

        public bool CleaverEquipped
        {
            get { return globals[weaponAmmoIndex[Weapon.Cleaver]].ValueAsBool; }
            set {
                globals[weaponAmmoIndex[Weapon.Cleaver]].ValueAsBool = value;
                OnPropertyChanged();
            }
        }

        public bool KatanaEquipped
        {
            get { return globals[weaponAmmoIndex[Weapon.Katana]].ValueAsBool; }
            set {
                globals[weaponAmmoIndex[Weapon.Katana]].ValueAsBool = value;
                OnPropertyChanged();
            }
        }

        public bool KnifeEquipped
        {
            get { return globals[weaponAmmoIndex[Weapon.Knife]].ValueAsBool; }
            set {
                globals[weaponAmmoIndex[Weapon.Knife]].ValueAsBool = value;
                OnPropertyChanged();
            }
        }

        public bool MacheteEquipped
        {
            get { return globals[weaponAmmoIndex[Weapon.Machete]].ValueAsBool; }
            set {
                globals[weaponAmmoIndex[Weapon.Machete]].ValueAsBool = value;
                OnPropertyChanged();
            }
        }

        public bool ChainsawEquipped
        {
            get { return globals[weaponAmmoIndex[Weapon.Chisel]].ValueAsBool; }
            set {
                globals[weaponAmmoIndex[Weapon.Chainsaw]].ValueAsBool = value;
                OnPropertyChanged();
            }
        }

        public int GrenadesAmmo
        {
            get { return globals[weaponAmmoIndex[Weapon.Grenades]]; }
            set {
                globals[weaponAmmoIndex[Weapon.Grenades]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int MolotovAmmo
        {
            get { return globals[weaponAmmoIndex[Weapon.Molotov]]; }
            set {
                globals[weaponAmmoIndex[Weapon.Molotov]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int TearGasAmmo
        {
            get { return globals[weaponAmmoIndex[Weapon.TearGas]]; }
            set {
                globals[weaponAmmoIndex[Weapon.TearGas]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int RemoteGrenadesAmmo
        {
            get { return globals[weaponAmmoIndex[Weapon.RemoteGrenades]]; }
            set {
                globals[weaponAmmoIndex[Weapon.RemoteGrenades]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int PistolAmmo
        {
            get { return globals[weaponAmmoIndex[Weapon.Pistol]]; }
            set {
                globals[weaponAmmoIndex[Weapon.Pistol]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int PythonAmmo
        {
            get { return globals[weaponAmmoIndex[Weapon.Python]]; }
            set {
                globals[weaponAmmoIndex[Weapon.Python]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int ShotgunAmmo
        {
            get { return globals[weaponAmmoIndex[Weapon.Shotgun]]; }
            set {
                globals[weaponAmmoIndex[Weapon.Shotgun]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int SpasShotgunAmmo
        {
            get { return globals[weaponAmmoIndex[Weapon.SpasShotgun]]; }
            set {
                globals[weaponAmmoIndex[Weapon.SpasShotgun]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int StubbyShotgunAmmo
        {
            get { return globals[weaponAmmoIndex[Weapon.StubbyShotgun]]; }
            set {
                globals[weaponAmmoIndex[Weapon.StubbyShotgun]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int Tec9Ammo
        {
            get { return globals[weaponAmmoIndex[Weapon.Tec9]]; }
            set {
                globals[weaponAmmoIndex[Weapon.Tec9]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int Mac10Ammo
        {
            get { return globals[weaponAmmoIndex[Weapon.Mac10]]; }
            set {
                globals[weaponAmmoIndex[Weapon.Mac10]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int MicroSmgAmmo
        {
            get { return globals[weaponAmmoIndex[Weapon.MicroSmg]]; }
            set {
                globals[weaponAmmoIndex[Weapon.MicroSmg]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int Mp5kAmmo
        {
            get { return globals[weaponAmmoIndex[Weapon.Mp5k]]; }
            set {
                globals[weaponAmmoIndex[Weapon.Mp5k]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int Ak47Ammo
        {
            get { return globals[weaponAmmoIndex[Weapon.Ak47]]; }
            set {
                globals[weaponAmmoIndex[Weapon.Ak47]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int M4Ammo
        {
            get { return globals[weaponAmmoIndex[Weapon.M4]]; }
            set {
                globals[weaponAmmoIndex[Weapon.M4]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int RocketLauncherAmmo
        {
            get { return globals[weaponAmmoIndex[Weapon.RocketLauncher]]; }
            set {
                globals[weaponAmmoIndex[Weapon.RocketLauncher]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int M60Ammo
        {
            get { return globals[weaponAmmoIndex[Weapon.M60]]; }
            set {
                globals[weaponAmmoIndex[Weapon.M60]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int FlameThrowerAmmo
        {
            get { return globals[weaponAmmoIndex[Weapon.FlameThrower]]; }
            set {
                globals[weaponAmmoIndex[Weapon.FlameThrower]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int MiniGunAmmo
        {
            get { return globals[weaponAmmoIndex[Weapon.MiniGun]]; }
            set {
                globals[weaponAmmoIndex[Weapon.MiniGun]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int SniperRifleAmmo
        {
            get { return globals[weaponAmmoIndex[Weapon.SniperRifle]]; }
            set {
                globals[weaponAmmoIndex[Weapon.SniperRifle]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }

        public int LaserSightedSniperRifleAmmo
        {
            get { return globals[weaponAmmoIndex[Weapon.LaserSightedSniperRifle]]; }
            set {
                globals[weaponAmmoIndex[Weapon.LaserSightedSniperRifle]].ValueAsInt = value;
                OnPropertyChanged();
            }
        }
    }
}
