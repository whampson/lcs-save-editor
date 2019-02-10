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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace LcsSaveEditor.ViewModels
{
    public partial class WeaponsViewModel
    {
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

        private void InitAndroidIOSWeaponVars()
        {
            m_ammoIndexMap[Weapon.Camera] = 125;
            m_ammoIndexMap[Weapon.BrassKnuckles] = 126;
            m_ammoIndexMap[Weapon.Chisel] = 127;
            m_ammoIndexMap[Weapon.Axe] = 128;
            m_ammoIndexMap[Weapon.HockeyStick] = 129;
            m_ammoIndexMap[Weapon.NightStick] = 130;
            m_ammoIndexMap[Weapon.BaseballBat] = 131;
            m_ammoIndexMap[Weapon.Cleaver] = 132;
            m_ammoIndexMap[Weapon.Katana] = 133;
            m_ammoIndexMap[Weapon.Knife] = 134;
            m_ammoIndexMap[Weapon.Machete] = 135;
            m_ammoIndexMap[Weapon.Chainsaw] = 136;
            m_ammoIndexMap[Weapon.Grenades] = 137;
            m_ammoIndexMap[Weapon.Molotovs] = 138;
            m_ammoIndexMap[Weapon.TearGas] = 139;
            m_ammoIndexMap[Weapon.RemoteGrenades] = 140;
            m_ammoIndexMap[Weapon.Pistol] = 141;
            m_ammoIndexMap[Weapon.Python] = 142;
            m_ammoIndexMap[Weapon.Shotgun] = 143;
            m_ammoIndexMap[Weapon.Spas12] = 144;
            m_ammoIndexMap[Weapon.StubbyShotgun] = 145;
            m_ammoIndexMap[Weapon.Tec9] = 146;
            m_ammoIndexMap[Weapon.Mac10] = 147;
            m_ammoIndexMap[Weapon.MicroSmg] = 148;
            m_ammoIndexMap[Weapon.Mp5] = 149;
            m_ammoIndexMap[Weapon.Ak] = 150;
            m_ammoIndexMap[Weapon.M4] = 151;
            m_ammoIndexMap[Weapon.RocketLauncher] = 152;
            m_ammoIndexMap[Weapon.M60] = 153;
            m_ammoIndexMap[Weapon.FlameThrower] = 154;
            m_ammoIndexMap[Weapon.Minigun] = 155;
            m_ammoIndexMap[Weapon.SniperRifle] = 156;
            m_ammoIndexMap[Weapon.LaserSightedSniperRifle] = 157;
        }

        private void InitPS2PSPWeaponVars()
        {
            m_ammoIndexMap[Weapon.Camera] = 124;
            m_ammoIndexMap[Weapon.BrassKnuckles] = 125;
            m_ammoIndexMap[Weapon.Chisel] = 126;
            m_ammoIndexMap[Weapon.Axe] = 127;
            m_ammoIndexMap[Weapon.HockeyStick] = 128;
            m_ammoIndexMap[Weapon.NightStick] = 129;
            m_ammoIndexMap[Weapon.BaseballBat] = 130;
            m_ammoIndexMap[Weapon.Cleaver] = 131;
            m_ammoIndexMap[Weapon.Katana] = 132;
            m_ammoIndexMap[Weapon.Knife] = 133;
            m_ammoIndexMap[Weapon.Machete] = 134;
            m_ammoIndexMap[Weapon.Chainsaw] = 135;
            m_ammoIndexMap[Weapon.Grenades] = 136;
            m_ammoIndexMap[Weapon.Molotovs] = 137;
            m_ammoIndexMap[Weapon.TearGas] = 138;
            m_ammoIndexMap[Weapon.RemoteGrenades] = 139;
            m_ammoIndexMap[Weapon.Pistol] = 140;
            m_ammoIndexMap[Weapon.Python] = 141;
            m_ammoIndexMap[Weapon.Shotgun] = 142;
            m_ammoIndexMap[Weapon.Spas12] = 143;
            m_ammoIndexMap[Weapon.StubbyShotgun] = 144;
            m_ammoIndexMap[Weapon.Tec9] = 145;
            m_ammoIndexMap[Weapon.Mac10] = 146;
            m_ammoIndexMap[Weapon.MicroSmg] = 147;
            m_ammoIndexMap[Weapon.Mp5] = 148;
            m_ammoIndexMap[Weapon.Ak] = 149;
            m_ammoIndexMap[Weapon.M4] = 150;
            m_ammoIndexMap[Weapon.RocketLauncher] = 151;
            m_ammoIndexMap[Weapon.M60] = 152;
            m_ammoIndexMap[Weapon.FlameThrower] = 153;
            m_ammoIndexMap[Weapon.Minigun] = 154;
            m_ammoIndexMap[Weapon.SniperRifle] = 155;
            m_ammoIndexMap[Weapon.LaserSightedSniperRifle] = 156;
        }

        private void ReadHandSlot()
        {
            if (m_globals[m_ammoIndexMap[Weapon.BrassKnuckles]] > 0) {
                m_hand = Weapon.BrassKnuckles;
            }
            else {
                m_hand = Weapon.Fists;
            }
        }

        private void ReadMeleeSlot()
        {
            m_hasMelee = false;
            m_melee = null;
            m_meleeAmmo = 0;

            foreach (Weapon w in m_meleeList) {
                uint val = m_globals[m_ammoIndexMap[w]];
                if (val > 0) {
                    m_hasMelee = true;
                    m_melee = w;
                    m_meleeAmmo = val;
                    break;
                }
            }
        }

        private void ReadProjectileSlot()
        {
            m_hasProjectile = false;
            m_projectile = null;
            m_projectileAmmo = 0;

            foreach (Weapon w in m_projectileList) {
                uint val = m_globals[m_ammoIndexMap[w]];
                if (val > 0) {
                    m_hasProjectile = true;
                    m_projectile = w;
                    m_projectileAmmo = val;
                    break;
                }
            }
        }

        private void ReadPistolSlot()
        {
            m_hasPistol = false;
            m_pistol = null;
            m_pistolAmmo = 0;

            foreach (Weapon w in m_pistolList) {
                uint val = m_globals[m_ammoIndexMap[w]];
                if (val > 0) {
                    m_hasPistol = true;
                    m_pistol = w;
                    m_pistolAmmo = val;
                    break;
                }
            }
        }

        private void ReadShotgunSlot()
        {
            m_hasShotgun = false;
            m_shotgun = null;
            m_shotgunAmmo = 0;

            foreach (Weapon w in m_shotgunList) {
                uint val = m_globals[m_ammoIndexMap[w]];
                if (val > 0) {
                    m_hasShotgun = true;
                    m_shotgun = w;
                    m_shotgunAmmo = val;
                    break;
                }
            }
        }

        private void ReadSmgSlot()
        {
            m_hasSmg = false;
            m_smg = null;
            m_smgAmmo = 0;

            foreach (Weapon w in m_smgList) {
                uint val = m_globals[m_ammoIndexMap[w]];
                if (val > 0) {
                    m_hasSmg = true;
                    m_smg = w;
                    m_smgAmmo = val;
                    break;
                }
            }
        }

        private void ReadAssaultSlot()
        {
            m_hasAssault = false;
            m_assault = null;
            m_assaultAmmo = 0;

            foreach (Weapon w in m_assaultList) {
                uint val = m_globals[m_ammoIndexMap[w]];
                if (val > 0) {
                    m_hasAssault = true;
                    m_assault = w;
                    m_assaultAmmo = val;
                    break;
                }
            }
        }

        private void ReadHeavySlot()
        {
            m_hasHeavy = false;
            m_heavy = null;
            m_heavyAmmo = 0;

            foreach (Weapon w in m_heavyList) {
                uint val = m_globals[m_ammoIndexMap[w]];
                if (val > 0) {
                    m_hasHeavy = true;
                    m_heavy = w;
                    m_heavyAmmo = val;
                    break;
                }
            }
        }

        private void ReadSniperSlot()
        {
            m_hasSniper = false;
            m_sniper = null;
            m_sniperAmmo = 0;

            foreach (Weapon w in m_sniperList) {
                uint val = m_globals[m_ammoIndexMap[w]];
                if (val > 0) {
                    m_hasSniper = true;
                    m_sniper = w;
                    m_sniperAmmo = val;
                    break;
                }
            }
        }

        private void ReadSpecialSlot()
        {
            m_hasSpecial = false;
            m_special = null;
            m_specialAmmo = 0;

            foreach (Weapon w in m_specialList) {
                uint val = m_globals[m_ammoIndexMap[w]];
                if (val > 0) {
                    m_hasSpecial = true;
                    m_special = w;
                    m_specialAmmo = val;
                    break;
                }
            }
        }

        private void WriteHandSlot()
        {
            m_suppressRefresh = true;
            if (m_hand == Weapon.BrassKnuckles) {
                m_globals[m_ammoIndexMap[Weapon.BrassKnuckles]] = 1;
            }
            else {
                m_globals[m_ammoIndexMap[Weapon.BrassKnuckles]] = 0;
            }
            m_suppressRefresh = false;
        }

        private void WriteMeleeSlot()
        {
            m_suppressRefresh = true;
            foreach (Weapon w in m_meleeList) {
                uint ammo = 0;
                if (w == m_melee) {
                    ammo = m_meleeAmmo;
                }
                m_globals[m_ammoIndexMap[w]] = ammo;
            }
            m_suppressRefresh = false;
        }

        private void WriteProjectileSlot()
        {
            m_suppressRefresh = true;
            foreach (Weapon w in m_projectileList) {
                uint ammo = 0;
                if (w == m_projectile) {
                    ammo = m_projectileAmmo;
                }
                m_globals[m_ammoIndexMap[w]] = ammo;
            }
            m_suppressRefresh = false;
        }

        private void WritePistolSlot()
        {
            m_suppressRefresh = true;
            foreach (Weapon w in m_pistolList) {
                uint ammo = 0;
                if (w == m_pistol) {
                    ammo = m_pistolAmmo;
                }
                m_globals[m_ammoIndexMap[w]] = ammo;
            }
        }

        private void WriteShotgunSlot()
        {
            m_suppressRefresh = true;
            foreach (Weapon w in m_shotgunList) {
                uint ammo = 0;
                if (w == m_shotgun) {
                    ammo = m_shotgunAmmo;
                }
                m_globals[m_ammoIndexMap[w]] = ammo;
            }
            m_suppressRefresh = false;
        }

        private void WriteSmgSlot()
        {
            m_suppressRefresh = true;
            foreach (Weapon w in m_smgList) {
                uint ammo = 0;
                if (w == m_smg) {
                    ammo = m_smgAmmo;
                }
                m_globals[m_ammoIndexMap[w]] = ammo;
            }
            m_suppressRefresh = false;
        }

        private void WriteAssaultSlot()
        {
            m_suppressRefresh = true;
            foreach (Weapon w in m_assaultList) {
                uint ammo = 0;
                if (w == m_assault) {
                    ammo = m_assaultAmmo;
                }
                m_globals[m_ammoIndexMap[w]] = ammo;
            }
            m_suppressRefresh = false;
        }

        private void WriteHeavySlot()
        {
            m_suppressRefresh = true;
            foreach (Weapon w in m_heavyList) {
                uint ammo = 0;
                if (w == m_heavy) {
                    ammo = m_heavyAmmo;
                }
                m_globals[m_ammoIndexMap[w]] = ammo;
            }
            m_suppressRefresh = false;
        }

        private void WriteSniperSlot()
        {
            m_suppressRefresh = true;
            foreach (Weapon w in m_sniperList) {
                uint ammo = 0;
                if (w == m_sniper) {
                    ammo = m_sniperAmmo;
                }
                m_globals[m_ammoIndexMap[w]] = ammo;
            }
            m_suppressRefresh = false;
        }

        private void WriteSpecialSlot()
        {
            m_suppressRefresh = true;
            foreach (Weapon w in m_specialList) {
                uint ammo = 0;
                if (w == m_special) {
                    ammo = m_specialAmmo;
                }
                m_globals[m_ammoIndexMap[w]] = ammo;
            }
            m_suppressRefresh = false;
        }

        private void RefreshWeaponSlot(int globalVarIndex)
        {
            if (m_suppressRefresh) {
                return;
            }

            Weapon? changedWeapon = m_ammoIndexMap.FirstOrDefault(x => x.Value == globalVarIndex).Key;
            switch (changedWeapon) {
                case Weapon.BrassKnuckles:
                    ReadHandSlot();
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
                    ReadMeleeSlot();
                    break;
                case Weapon.Molotovs:
                case Weapon.Grenades:
                case Weapon.RemoteGrenades:
                case Weapon.TearGas:
                    ReadProjectileSlot();
                    break;
                case Weapon.Pistol:
                case Weapon.Python:
                    ReadPistolSlot();
                    break;
                case Weapon.Shotgun:
                case Weapon.StubbyShotgun:
                case Weapon.Spas12:
                    ReadShotgunSlot();
                    break;
                case Weapon.Tec9:
                case Weapon.Mac10:
                case Weapon.Mp5:
                case Weapon.MicroSmg:
                    ReadSmgSlot();
                    break;
                case Weapon.Ak:
                case Weapon.M4:
                    ReadAssaultSlot();
                    break;
                case Weapon.FlameThrower:
                case Weapon.RocketLauncher:
                case Weapon.Minigun:
                case Weapon.M60:
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
