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
    public partial class WeaponsPage : Page
    {
        private Weapons weapons;

        public WeaponsPage()
            : base("Weapons", PageVisibility.VisibleWhenFileLoadedOnly)
        {
            weapons = null;
            InitializeComponent();
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            weapons = GameState.ActiveGameState.Player.Weapons;

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
            else if (weapons.BatEquipped) {
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
            }

            // Shotgun
            if (weapons.ShotgunAmmo > 0) {
                cmbShotgun.SelectedIndex = 0;
                chkShotgun.Checked = true;
            }
            else if (weapons.SpasShotgunAmmo > 0) {
                cmbShotgun.SelectedIndex = 1;
                chkShotgun.Checked = true;
            }
            else if (weapons.StubbyShotgunAmmo > 0) {
                cmbShotgun.SelectedIndex = 2;
                chkShotgun.Checked = true;
            }
            else {
                cmbShotgun.SelectedIndex = -1;
                chkShotgun.Checked = false;
            }


            CmbHand_SelectedIndexChanged(this, new EventArgs());
            CmbPistol_SelectedIndexChanged(this, new EventArgs());
            CmbShotgun_SelectedIndexChanged(this, new EventArgs());

            ChkMelee_CheckedChanged(this, new EventArgs());
            ChkPistol_CheckedChanged(this, new EventArgs());
            ChkShotgun_CheckedChanged(this, new EventArgs());
        }

        private void CmbHand_SelectedIndexChanged(object sender, EventArgs e)
        {
            weapons.BrassKnucklesEquipped = (cmbHand.SelectedIndex == 1);
        }

        private void ChkMelee_CheckedChanged(object sender, EventArgs e)
        {
            cmbMelee.Enabled = chkMelee.Checked;
        }

        private void CmbMelee_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbPistol.SelectedIndex) {
                case 0:
                    weapons.ChiselEquipped = true;
                    break;
                case 1:
                    weapons.AxeEquipped = true;
                    break;
                case 2:
                    weapons.HockeyStickEquipped = true;
                    break;
                case 3:
                    weapons.NightStickEquipped = true;
                    break;
                case 4:
                    weapons.BatEquipped = true;
                    break;
                case 5:
                    weapons.CleaverEquipped = true;
                    break;
                case 6:
                    weapons.KatanaEquipped = true;
                    break;
                case 7:
                    weapons.KnifeEquipped = true;
                    break;
                case 8:
                    weapons.MacheteEquipped = true;
                    break;
                case 9:
                    weapons.ChainsawEquipped = true;
                    break;
            }
        }

        private void ChkPistol_CheckedChanged(object sender, EventArgs e)
        {
            cmbPistol.Enabled = chkPistol.Checked;
            numPistolAmmo.Enabled = chkPistol.Checked;
        }

        private void CmbPistol_SelectedIndexChanged(object sender, EventArgs e)
        {
            object dataSource;

            switch (cmbPistol.SelectedIndex) {
                case 0:
                    dataSource = weapons.PistolAmmo;
                    break;
                case 1:
                    dataSource = weapons.PythonAmmo;
                    break;
                default:
                    dataSource = null;
                    break;
            }

            if (dataSource != null) {
                numPistolAmmo.DataBindings.Clear();
                numPistolAmmo.DataBindings.Add(
                    "Value",
                    dataSource,
                    "",
                    true,
                    DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        private void ChkShotgun_CheckedChanged(object sender, EventArgs e)
        {
            cmbShotgun.Enabled = chkShotgun.Checked;
            numShotgunAmmo.Enabled = chkShotgun.Checked;
        }

        private void CmbShotgun_SelectedIndexChanged(object sender, EventArgs e)
        {
            object dataSource;

            switch (cmbShotgun.SelectedIndex) {
                case 0:
                    dataSource = weapons.ShotgunAmmo;
                    break;
                case 1:
                    dataSource = weapons.SpasShotgunAmmo;
                    break;
                case 2:
                    dataSource = weapons.StubbyShotgunAmmo;
                    break;
                default:
                    dataSource = null;
                    break;
            }

            if (dataSource != null) {
                numShotgunAmmo.DataBindings.Clear();
                numShotgunAmmo.DataBindings.Add(
                    "Value",
                    dataSource,
                    "",
                    true,
                    DataSourceUpdateMode.OnPropertyChanged);
            }
        }
    }
}
