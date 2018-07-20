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
using System.Windows.Forms;

namespace WHampson.LcsSaveEditor.UI
{
    // TODO: if not active page, listen on weapons.PropertyChanged and reload pageq
    public partial class WeaponsPage : Page
    {
        private Weapons weapons;
        private bool suppressEventListeners;

        public WeaponsPage()
            : base("Weapons", PageVisibility.VisibleWhenFileLoadedOnly)
        {
            InitializeComponent();

            weapons = null;
            suppressEventListeners = true;
            numProjectileAmmo.Maximum = int.MaxValue;
            numPistolAmmo.Maximum = int.MaxValue;
            numShotgunAmmo.Maximum = int.MaxValue;
            numSmgAmmo.Maximum = int.MaxValue;
            numAssaultAmmo.Maximum = int.MaxValue;
            numHeavyAmmo.Maximum = int.MaxValue;
            numSniperAmmo.Maximum = int.MaxValue;
            numSpecialAmmo.Maximum = int.MaxValue;
        }

        public void UpdateMeleeSlot()
        {
            if (weapons == null) {
                return;
            }

            weapons.ChiselEquipped =
                chkMelee.Checked && cmbMelee.SelectedIndex == 0;
            weapons.AxeEquipped =
                chkMelee.Checked && cmbMelee.SelectedIndex == 1;
            weapons.HockeyStickEquipped =
                chkMelee.Checked && cmbMelee.SelectedIndex == 2;
            weapons.NightStickEquipped =
                chkMelee.Checked && cmbMelee.SelectedIndex == 3;
            weapons.BaseballBatEquipped =
                chkMelee.Checked && cmbMelee.SelectedIndex == 4;
            weapons.CleaverEquipped =
                chkMelee.Checked && cmbMelee.SelectedIndex == 5;
            weapons.KatanaEquipped =
                chkMelee.Checked && cmbMelee.SelectedIndex == 6;
            weapons.KnifeEquipped =
                chkMelee.Checked && cmbMelee.SelectedIndex == 7;
            weapons.MacheteEquipped =
                chkMelee.Checked && cmbMelee.SelectedIndex == 8;
            weapons.ChainsawEquipped =
                chkMelee.Checked && cmbMelee.SelectedIndex == 9;
        }

        private int ProjectileSlot
        {
            get {
                if (weapons == null) {
                    return 0;
                }
                int value;
                switch (cmbProjectile.SelectedIndex) {
                    case 0:
                        value = weapons.GrenadesAmmo;
                        break;
                    case 1:
                        value = weapons.RemoteGrenadesAmmo;
                        break;
                    case 2:
                        value = weapons.MolotovAmmo;
                        break;
                    case 3:
                        value = weapons.TearGasAmmo;
                        break;
                    default:
                        value = 0;
                        break;
                }
                return value;
            }

            set {
                if (weapons == null) {
                    return;
                }
                switch (cmbProjectile.SelectedIndex) {
                    case 0:
                        weapons.GrenadesAmmo = value;
                        break;
                    case 1:
                        weapons.RemoteGrenadesAmmo = value;
                        break;
                    case 2:
                        weapons.MolotovAmmo = value;
                        break;
                    case 3:
                        weapons.TearGasAmmo = value;
                        break;
                }
            }
        }

        private int PistolSlot
        {
            get {
                if (weapons == null) {
                    return 0;
                }
                int value;
                switch (cmbPistol.SelectedIndex) {
                    case 0:
                        value = weapons.PistolAmmo;
                        break;
                    case 1:
                        value = weapons.PythonAmmo;
                        break;
                    default:
                        value = 0;
                        break;
                }
                return value;
            }
            
            set {
                if (weapons == null) {
                    return;
                }
                switch (cmbPistol.SelectedIndex) {
                    case 0:
                        weapons.PistolAmmo = value;
                        break;
                    case 1:
                        weapons.PythonAmmo = value;
                        break;
                }
            }
        }

        private int ShotgunSlot
        {
            get {
                if (weapons == null) {
                    return 0;
                }
                int value;
                switch (cmbShotgun.SelectedIndex) {
                    case 0:
                        value = weapons.ShotgunAmmo;
                        break;
                    case 1:
                        value = weapons.StubbyShotgunAmmo;
                        break;
                    case 2:
                        value = weapons.SpasShotgunAmmo;
                        break;
                    default:
                        value = 0;
                        break;
                }
                return value;
            }

            set {
                if (weapons == null) {
                    return;
                }
                switch (cmbShotgun.SelectedIndex) {
                    case 0:
                        weapons.ShotgunAmmo = value;
                        break;
                    case 1:
                        weapons.StubbyShotgunAmmo = value;
                        break;
                    case 2:
                        weapons.SpasShotgunAmmo = value;
                        break;
                }
            }
        }

        private int SmgSlot
        {
            get {
                if (weapons == null) {
                    return 0;
                }
                int value;
                switch (cmbSmg.SelectedIndex) {
                    case 0:
                        value = weapons.Tec9Ammo;
                        break;
                    case 1:
                        value = weapons.Mac10Ammo;
                        break;
                    case 2:
                        value = weapons.MicroSmgAmmo;
                        break;
                    case 3:
                        value = weapons.Mp5kAmmo;
                        break;
                    default:
                        value = 0;
                        break;
                }
                return value;
            }

            set {
                if (weapons == null) {
                    return;
                }
                switch (cmbSmg.SelectedIndex) {
                    case 0:
                        weapons.Tec9Ammo = value;
                        break;
                    case 1:
                        weapons.Mac10Ammo = value;
                        break;
                    case 2:
                        weapons.MicroSmgAmmo = value;
                        break;
                    case 3:
                        weapons.Mp5kAmmo = value;
                        break;
                }
            }
        }

        private int AssaultSlot
        {
            get {
                if (weapons == null) {
                    return 0;
                }
                int value;
                switch (cmbAssault.SelectedIndex) {
                    case 0:
                        value = weapons.Ak47Ammo;
                        break;
                    case 1:
                        value = weapons.M4Ammo;
                        break;
                    default:
                        value = 0;
                        break;
                }
                return value;
            }

            set {
                if (weapons == null) {
                    return;
                }
                switch (cmbAssault.SelectedIndex) {
                    case 0:
                        weapons.Ak47Ammo = value;
                        break;
                    case 1:
                        weapons.M4Ammo = value;
                        break;
                }
            }
        }

        private int HeavySlot
        {
            get {
                if (weapons == null) {
                    return 0;
                }
                int value;
                switch (cmbHeavy.SelectedIndex) {
                    case 0:
                        value = weapons.M60Ammo;
                        break;
                    case 1:
                        value = weapons.MiniGunAmmo;
                        break;
                    case 2:
                        value = weapons.FlameThrowerAmmo; ;
                        break;
                    case 3:
                        value = weapons.RocketLauncherAmmo;
                        break;
                    default:
                        value = 0;
                        break;
                }
                return value;
            }

            set {
                if (weapons == null) {
                    return;
                }
                switch (cmbHeavy.SelectedIndex) {
                    case 0:
                        weapons.M60Ammo = value;
                        break;
                    case 1:
                        weapons.MiniGunAmmo = value;
                        break;
                    case 2:
                        weapons.FlameThrowerAmmo = value;
                        break;
                    case 3:
                        weapons.RocketLauncherAmmo = value;
                        break;
                }
            }
        }

        private int SniperSlot
        {
            get {
                if (weapons == null) {
                    return 0;
                }
                int value;
                switch (cmbSniper.SelectedIndex) {
                    case 0:
                        value = weapons.SniperRifleAmmo;
                        break;
                    case 1:
                        value = weapons.LaserSightedSniperRifleAmmo;
                        break;
                    default:
                        value = 0;
                        break;
                }
                return value;
            }

            set {
                if (weapons == null) {
                    return;
                }
                switch (cmbSniper.SelectedIndex) {
                    case 0:
                        weapons.SniperRifleAmmo = value;
                        break;
                    case 1:
                        weapons.LaserSightedSniperRifleAmmo = value;
                        break;
                }
            }
        }

        private int SpecialSlot
        {
            get {
                if (weapons == null) {
                    return 0;
                }
                int value;
                switch (cmbSpecial.SelectedIndex) {
                    case 0:
                        value = weapons.CameraAmmo;
                        break;
                    default:
                        value = 0;
                        break;
                }
                return value;
            }

            set {
                if (weapons == null) {
                    return;
                }
                switch (cmbSpecial.SelectedIndex) {
                    case 0:
                        weapons.CameraAmmo = value;
                        break;
                }
            }
        }

        private void CmbHand_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            weapons.BrassKnucklesEquipped = (cmbHand.SelectedIndex == 1);
        }

        private void ChkMelee_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            cmbMelee.Enabled = chkMelee.Checked;
            if (cmbMelee.SelectedIndex != -1) {
                UpdateMeleeSlot();
            }
        }

        private void CmbMelee_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            UpdateMeleeSlot();
        }

        private void ChkProjectile_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            cmbProjectile.Enabled = chkProjectile.Checked;
            numProjectileAmmo.Enabled = chkProjectile.Checked && cmbProjectile.SelectedIndex != -1;
            if (chkProjectile.Checked) {
                numProjectileAmmo.Minimum = 1;
            }
            else {
                numProjectileAmmo.Minimum = 0;
                numProjectileAmmo.Value = 0;
            }
        }

        private void CmbProjectile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            numProjectileAmmo.Enabled = cmbProjectile.SelectedIndex != -1;
            ProjectileSlot = (int) numProjectileAmmo.Value;
        }

        private void NumProjectileAmmo_ValueChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            ProjectileSlot = (int) numProjectileAmmo.Value;
            chkProjectile.Checked = numProjectileAmmo.Value > 0;
        }

        private void ChkPistol_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            cmbPistol.Enabled = chkPistol.Checked;
            numPistolAmmo.Enabled = chkPistol.Checked && cmbPistol.SelectedIndex != -1;
            if (chkPistol.Checked) {
                numPistolAmmo.Minimum = 1;
            }
            else {
                numPistolAmmo.Minimum = 0;
                numPistolAmmo.Value = 0;
            }
        }

        private void CmbPistol_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            numPistolAmmo.Enabled = cmbPistol.SelectedIndex != -1;
            PistolSlot = (int) numPistolAmmo.Value;
        }

        private void NumPistolAmmo_ValueChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            PistolSlot = (int) numPistolAmmo.Value;
            chkPistol.Checked = numPistolAmmo.Value > 0;
        }

        private void ChkShotgun_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            cmbShotgun.Enabled = chkShotgun.Checked;
            numShotgunAmmo.Enabled = chkShotgun.Checked && cmbShotgun.SelectedIndex != -1;
            if (chkShotgun.Checked) {
                numShotgunAmmo.Minimum = 1;
            }
            else {
                numShotgunAmmo.Minimum = 0;
                numShotgunAmmo.Value = 0;
            }
        }

        private void CmbShotgun_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            numShotgunAmmo.Enabled = cmbShotgun.SelectedIndex != -1;
            ShotgunSlot = (int) numShotgunAmmo.Value;
        }

        private void NumShotgunAmmo_ValueChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            ShotgunSlot = (int) numShotgunAmmo.Value;
        }

        private void ChkSmg_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            cmbSmg.Enabled = chkSmg.Checked;
            numSmgAmmo.Enabled = chkSmg.Checked && cmbSmg.SelectedIndex != -1;
            if (chkSmg.Checked) {
                numSmgAmmo.Minimum = 1;
            }
            else {
                numSmgAmmo.Minimum = 0;
                numSmgAmmo.Value = 0;
            }
        }

        private void CmbSmg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            numSmgAmmo.Enabled = cmbSmg.SelectedIndex != -1;
            SmgSlot = (int) numSmgAmmo.Value;
        }

        private void NumSmgAmmo_ValueChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            SmgSlot = (int) numSmgAmmo.Value;
        }

        private void ChkAssault_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            cmbAssault.Enabled = chkAssault.Checked;
            numAssaultAmmo.Enabled = chkAssault.Checked && cmbAssault.SelectedIndex != -1;
            if (chkAssault.Checked) {
                numAssaultAmmo.Minimum = 1;
            }
            else {
                numAssaultAmmo.Minimum = 0;
                numAssaultAmmo.Value = 0;
            }
        }

        private void CmbAssault_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            numAssaultAmmo.Enabled = cmbAssault.SelectedIndex != -1;
            AssaultSlot = (int) numAssaultAmmo.Value;
        }

        private void NumAssaultAmmo_ValueChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            AssaultSlot = (int) numAssaultAmmo.Value;
        }

        private void ChkHeavy_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            cmbHeavy.Enabled = chkHeavy.Checked;
            numHeavyAmmo.Enabled = chkHeavy.Checked && cmbHeavy.SelectedIndex != -1;
            if (chkHeavy.Checked) {
                numHeavyAmmo.Minimum = 1;
            }
            else {
                numHeavyAmmo.Minimum = 0;
                numHeavyAmmo.Value = 0;
            }
        }

        private void CmbHeavy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            numHeavyAmmo.Enabled = cmbHeavy.SelectedIndex != -1;
            HeavySlot = (int) numHeavyAmmo.Value;
        }

        private void NumHeavyAmmo_ValueChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            HeavySlot = (int) numHeavyAmmo.Value;
        }

        private void ChkSniper_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            cmbSniper.Enabled = chkSniper.Checked;
            numSniperAmmo.Enabled = chkSniper.Checked && cmbSniper.SelectedIndex != -1;
            if (chkSniper.Checked) {
                numSniperAmmo.Minimum = 1;
            }
            else {
                numSniperAmmo.Minimum = 0;
                numSniperAmmo.Value = 0;
            }
        }

        private void CmbSniper_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            numSniperAmmo.Enabled = cmbSniper.SelectedIndex != -1;
            SniperSlot = (int) numSniperAmmo.Value;
        }

        private void NumSniperAmmo_ValueChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            SniperSlot = (int) numSniperAmmo.Value;
        }

        private void ChkSpecial_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            cmbSpecial.Enabled = chkSpecial.Checked;
            numSpecialAmmo.Enabled = chkSpecial.Checked && cmbSpecial.SelectedIndex != -1;
            if (chkSpecial.Checked) {
                numSpecialAmmo.Minimum = 1;
            }
            else {
                numSpecialAmmo.Minimum = 0;
                numSpecialAmmo.Value = 0;
            }
        }

        private void CmbSpecial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            numSpecialAmmo.Enabled = cmbSpecial.SelectedIndex != -1;
            SpecialSlot = (int) numSpecialAmmo.Value;
        }

        private void NumSpecialAmmo_ValueChanged(object sender, EventArgs e)
        {
            if (suppressEventListeners) {
                return;
            }

            SpecialSlot = (int) numSpecialAmmo.Value;
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            suppressEventListeners = true;
            weapons = Game.ActiveGameState.Player.Weapons;

            // Hand
            if (weapons.BrassKnucklesEquipped) {
                cmbHand.SelectedIndex = 1;
            }
            else {
                cmbHand.SelectedIndex = 0;
            }

            // Melee
            if (weapons.ChiselEquipped) {
                cmbMelee.SelectedIndex = 0;
                chkMelee.Checked = true;
            }
            else if (weapons.AxeEquipped) {
                cmbMelee.SelectedIndex = 1;
                chkMelee.Checked = true;
            }
            else if (weapons.HockeyStickEquipped) {
                cmbMelee.SelectedIndex = 2;
                chkMelee.Checked = true;
            }
            else if (weapons.NightStickEquipped) {
                cmbMelee.SelectedIndex = 3;
                chkMelee.Checked = true;
            }
            else if (weapons.BaseballBatEquipped) {
                cmbMelee.SelectedIndex = 4;
                chkMelee.Checked = true;
            }
            else if (weapons.CleaverEquipped) {
                cmbMelee.SelectedIndex = 5;
                chkMelee.Checked = true;
            }
            else if (weapons.KatanaEquipped) {
                cmbMelee.SelectedIndex = 6;
                chkMelee.Checked = true;
            }
            else if (weapons.KnifeEquipped) {
                cmbMelee.SelectedIndex = 7;
                chkMelee.Checked = true;
            }
            else if (weapons.MacheteEquipped) {
                cmbMelee.SelectedIndex = 8;
                chkMelee.Checked = true;
            }
            else if (weapons.ChainsawEquipped) {
                cmbMelee.SelectedIndex = 9;
                chkMelee.Checked = true;
            }
            else {
                cmbMelee.SelectedIndex = -1;
                chkMelee.Checked = false;
            }

            // Projectile
            if (weapons.GrenadesAmmo > 0) {
                cmbProjectile.SelectedIndex = 0;
                chkProjectile.Checked = true;
            }
            else if (weapons.RemoteGrenadesAmmo > 0) {
                cmbProjectile.SelectedIndex = 1;
                chkProjectile.Checked = true;
            }
            else if (weapons.MolotovAmmo > 0) {
                cmbProjectile.SelectedIndex = 2;
                chkProjectile.Checked = true;
            }
            else if (weapons.TearGasAmmo > 0) {
                cmbProjectile.SelectedIndex = 3;
                chkProjectile.Checked = true;
            }
            else {
                cmbProjectile.SelectedIndex = -1;
                chkProjectile.Checked = false;
                numProjectileAmmo.Minimum = 0;
            }

            // Pistol
            if (weapons.PistolAmmo > 0) {
                cmbPistol.SelectedIndex = 0;
                chkPistol.Checked = true;
            }
            else if (weapons.PythonAmmo > 0) {
                cmbPistol.SelectedIndex = 1;
                chkPistol.Checked = true;
            }
            else {
                cmbPistol.SelectedIndex = -1;
                chkPistol.Checked = false;
                numPistolAmmo.Minimum = 0;
            }

            // Shotgun
            if (weapons.ShotgunAmmo > 0) {
                cmbShotgun.SelectedIndex = 0;
                chkShotgun.Checked = true;
            }
            else if (weapons.StubbyShotgunAmmo > 0) {
                cmbShotgun.SelectedIndex = 1;
                chkShotgun.Checked = true;
            }
            else if (weapons.SpasShotgunAmmo > 0) {
                cmbShotgun.SelectedIndex = 2;
                chkShotgun.Checked = true;
            }
            else {
                cmbShotgun.SelectedIndex = -1;
                chkShotgun.Checked = false;
                numShotgunAmmo.Minimum = 0;
            }

            // SMG
            if (weapons.Tec9Ammo > 0) {
                cmbSmg.SelectedIndex = 0;
                chkSmg.Checked = true;
            }
            else if (weapons.Mac10Ammo> 0) {
                cmbSmg.SelectedIndex = 1;
                chkSmg.Checked = true;
            }
            else if (weapons.MicroSmgAmmo > 0) {
                cmbSmg.SelectedIndex = 2;
                chkSmg.Checked = true;
            }
            else if (weapons.Mp5kAmmo > 0) {
                cmbSmg.SelectedIndex = 3;
                chkSmg.Checked = true;
            }
            else {
                cmbSmg.SelectedIndex = -1;
                chkSmg.Checked = false;
                numSmgAmmo.Minimum = 0;
            }

            // Assault
            if (weapons.Ak47Ammo > 0) {
                cmbAssault.SelectedIndex = 0;
                chkAssault.Checked = true;
            }
            else if (weapons.M4Ammo > 0) {
                cmbAssault.SelectedIndex = 1;
                chkAssault.Checked = true;
            }
            else {
                cmbAssault.SelectedIndex = -1;
                chkAssault.Checked = false;
                numAssaultAmmo.Minimum = 0;
            }

            // Heavy
            if (weapons.M60Ammo > 0) {
                cmbHeavy.SelectedIndex = 0;
                chkHeavy.Checked = true;
            }
            else if (weapons.MiniGunAmmo > 0) {
                cmbHeavy.SelectedIndex = 1;
                chkHeavy.Checked = true;
            }
            else if (weapons.FlameThrowerAmmo > 0) {
                cmbHeavy.SelectedIndex = 2;
                chkHeavy.Checked = true;
            }
            else if (weapons.RocketLauncherAmmo > 0) {
                cmbHeavy.SelectedIndex = 3;
                chkHeavy.Checked = true;
            }
            else {
                cmbHeavy.SelectedIndex = -1;
                chkHeavy.Checked = false;
                numHeavyAmmo.Minimum = 0;
            }

            // Sniper
            if (weapons.SniperRifleAmmo > 0) {
                cmbSniper.SelectedIndex = 0;
                chkSniper.Checked = true;
            }
            else if (weapons.LaserSightedSniperRifleAmmo > 0) {
                cmbSniper.SelectedIndex = 1;
                chkSniper.Checked = true;
            }
            else {
                cmbSniper.SelectedIndex = -1;
                chkSniper.Checked = false;
                numSniperAmmo.Minimum = 0;
            }

            // Special
            if (weapons.CameraAmmo > 0) {
                cmbSpecial.SelectedIndex = 0;
                chkSpecial.Checked = true;
            }
            else {
                cmbSpecial.SelectedIndex = -1;
                chkSpecial.Checked = false;
                numSpecialAmmo.Minimum = 0;
            }

            numProjectileAmmo.Value = ProjectileSlot;
            numPistolAmmo.Value = PistolSlot;
            numShotgunAmmo.Value = ShotgunSlot;
            numSmgAmmo.Value = SmgSlot;
            numAssaultAmmo.Value = AssaultSlot;
            numHeavyAmmo.Value = HeavySlot;
            numSniperAmmo.Value = SniperSlot;
            numSpecialAmmo.Value = SpecialSlot;

            suppressEventListeners = false;

            CmbHand_SelectedIndexChanged(this, new EventArgs());
            CmbMelee_SelectedIndexChanged(this, new EventArgs());
            CmbProjectile_SelectedIndexChanged(this, new EventArgs());
            CmbPistol_SelectedIndexChanged(this, new EventArgs());
            CmbShotgun_SelectedIndexChanged(this, new EventArgs());
            CmbSmg_SelectedIndexChanged(this, new EventArgs());
            CmbAssault_SelectedIndexChanged(this, new EventArgs());
            CmbHeavy_SelectedIndexChanged(this, new EventArgs());
            CmbSniper_SelectedIndexChanged(this, new EventArgs());
            CmbSpecial_SelectedIndexChanged(this, new EventArgs());

            ChkMelee_CheckedChanged(this, new EventArgs());
            ChkProjectile_CheckedChanged(this, new EventArgs());
            ChkPistol_CheckedChanged(this, new EventArgs());
            ChkShotgun_CheckedChanged(this, new EventArgs());
            ChkSmg_CheckedChanged(this, new EventArgs());
            ChkAssault_CheckedChanged(this, new EventArgs());
            ChkHeavy_CheckedChanged(this, new EventArgs());
            ChkSniper_CheckedChanged(this, new EventArgs());
            ChkSpecial_CheckedChanged(this, new EventArgs());
        }
    }
}
