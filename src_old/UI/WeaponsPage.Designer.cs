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

namespace WHampson.LcsSaveEditor.UI
{
    partial class WeaponsPage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel = new System.Windows.Forms.Panel();
            this.chkHand = new System.Windows.Forms.CheckBox();
            this.chkMelee = new System.Windows.Forms.CheckBox();
            this.chkProjectile = new System.Windows.Forms.CheckBox();
            this.chkPistol = new System.Windows.Forms.CheckBox();
            this.chkShotgun = new System.Windows.Forms.CheckBox();
            this.chkSmg = new System.Windows.Forms.CheckBox();
            this.chkAssault = new System.Windows.Forms.CheckBox();
            this.chkHeavy = new System.Windows.Forms.CheckBox();
            this.chkSniper = new System.Windows.Forms.CheckBox();
            this.chkSpecial = new System.Windows.Forms.CheckBox();
            this.cmbHand = new System.Windows.Forms.ComboBox();
            this.cmbMelee = new System.Windows.Forms.ComboBox();
            this.cmbProjectile = new System.Windows.Forms.ComboBox();
            this.cmbPistol = new System.Windows.Forms.ComboBox();
            this.cmbShotgun = new System.Windows.Forms.ComboBox();
            this.cmbSmg = new System.Windows.Forms.ComboBox();
            this.cmbAssault = new System.Windows.Forms.ComboBox();
            this.cmbHeavy = new System.Windows.Forms.ComboBox();
            this.cmbSniper = new System.Windows.Forms.ComboBox();
            this.cmbSpecial = new System.Windows.Forms.ComboBox();
            this.numProjectileAmmo = new System.Windows.Forms.NumericUpDown();
            this.numPistolAmmo = new System.Windows.Forms.NumericUpDown();
            this.numShotgunAmmo = new System.Windows.Forms.NumericUpDown();
            this.numSmgAmmo = new System.Windows.Forms.NumericUpDown();
            this.numAssaultAmmo = new System.Windows.Forms.NumericUpDown();
            this.numHeavyAmmo = new System.Windows.Forms.NumericUpDown();
            this.numSniperAmmo = new System.Windows.Forms.NumericUpDown();
            this.numSpecialAmmo = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel.SuspendLayout();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numProjectileAmmo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPistolAmmo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numShotgunAmmo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSmgAmmo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAssaultAmmo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeavyAmmo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSniperAmmo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpecialAmmo)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Controls.Add(this.panel, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 555F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 555F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 555F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 555F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 555F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 555F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 555F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 555F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 555F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(460, 408);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // panel
            // 
            this.panel.Controls.Add(this.chkHand);
            this.panel.Controls.Add(this.chkMelee);
            this.panel.Controls.Add(this.chkProjectile);
            this.panel.Controls.Add(this.chkPistol);
            this.panel.Controls.Add(this.chkShotgun);
            this.panel.Controls.Add(this.chkSmg);
            this.panel.Controls.Add(this.chkAssault);
            this.panel.Controls.Add(this.chkHeavy);
            this.panel.Controls.Add(this.chkSniper);
            this.panel.Controls.Add(this.chkSpecial);
            this.panel.Controls.Add(this.cmbHand);
            this.panel.Controls.Add(this.cmbMelee);
            this.panel.Controls.Add(this.cmbProjectile);
            this.panel.Controls.Add(this.cmbPistol);
            this.panel.Controls.Add(this.cmbShotgun);
            this.panel.Controls.Add(this.cmbSmg);
            this.panel.Controls.Add(this.cmbAssault);
            this.panel.Controls.Add(this.cmbHeavy);
            this.panel.Controls.Add(this.cmbSniper);
            this.panel.Controls.Add(this.cmbSpecial);
            this.panel.Controls.Add(this.numProjectileAmmo);
            this.panel.Controls.Add(this.numPistolAmmo);
            this.panel.Controls.Add(this.numShotgunAmmo);
            this.panel.Controls.Add(this.numSmgAmmo);
            this.panel.Controls.Add(this.numAssaultAmmo);
            this.panel.Controls.Add(this.numHeavyAmmo);
            this.panel.Controls.Add(this.numSniperAmmo);
            this.panel.Controls.Add(this.numSpecialAmmo);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(3, 3);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(454, 402);
            this.panel.TabIndex = 0;
            // 
            // chkHand
            // 
            this.chkHand.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkHand.Checked = true;
            this.chkHand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHand.Enabled = false;
            this.chkHand.Location = new System.Drawing.Point(49, 15);
            this.chkHand.Margin = new System.Windows.Forms.Padding(15);
            this.chkHand.Name = "chkHand";
            this.chkHand.Size = new System.Drawing.Size(52, 17);
            this.chkHand.TabIndex = 0;
            this.chkHand.Text = "Hand";
            this.chkHand.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkHand.UseVisualStyleBackColor = true;
            // 
            // chkMelee
            // 
            this.chkMelee.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkMelee.Location = new System.Drawing.Point(46, 54);
            this.chkMelee.Margin = new System.Windows.Forms.Padding(15);
            this.chkMelee.Name = "chkMelee";
            this.chkMelee.Size = new System.Drawing.Size(55, 17);
            this.chkMelee.TabIndex = 2;
            this.chkMelee.Text = "Melee";
            this.chkMelee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkMelee.UseVisualStyleBackColor = true;
            this.chkMelee.CheckedChanged += new System.EventHandler(this.ChkMelee_CheckedChanged);
            // 
            // chkProjectile
            // 
            this.chkProjectile.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkProjectile.Location = new System.Drawing.Point(32, 95);
            this.chkProjectile.Margin = new System.Windows.Forms.Padding(15);
            this.chkProjectile.Name = "chkProjectile";
            this.chkProjectile.Size = new System.Drawing.Size(69, 17);
            this.chkProjectile.TabIndex = 28;
            this.chkProjectile.Text = "Projectile";
            this.chkProjectile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkProjectile.UseVisualStyleBackColor = true;
            this.chkProjectile.CheckedChanged += new System.EventHandler(this.ChkProjectile_CheckedChanged);
            // 
            // chkPistol
            // 
            this.chkPistol.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPistol.Location = new System.Drawing.Point(50, 134);
            this.chkPistol.Margin = new System.Windows.Forms.Padding(15);
            this.chkPistol.Name = "chkPistol";
            this.chkPistol.Size = new System.Drawing.Size(51, 17);
            this.chkPistol.TabIndex = 4;
            this.chkPistol.Text = "Pistol";
            this.chkPistol.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPistol.UseVisualStyleBackColor = true;
            this.chkPistol.CheckedChanged += new System.EventHandler(this.ChkPistol_CheckedChanged);
            // 
            // chkShotgun
            // 
            this.chkShotgun.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkShotgun.Location = new System.Drawing.Point(35, 171);
            this.chkShotgun.Margin = new System.Windows.Forms.Padding(15);
            this.chkShotgun.Name = "chkShotgun";
            this.chkShotgun.Size = new System.Drawing.Size(66, 17);
            this.chkShotgun.TabIndex = 7;
            this.chkShotgun.Text = "Shotgun";
            this.chkShotgun.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkShotgun.UseVisualStyleBackColor = true;
            this.chkShotgun.CheckedChanged += new System.EventHandler(this.ChkShotgun_CheckedChanged);
            // 
            // chkSmg
            // 
            this.chkSmg.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSmg.Location = new System.Drawing.Point(51, 210);
            this.chkSmg.Margin = new System.Windows.Forms.Padding(15);
            this.chkSmg.Name = "chkSmg";
            this.chkSmg.Size = new System.Drawing.Size(50, 17);
            this.chkSmg.TabIndex = 10;
            this.chkSmg.Text = "SMG";
            this.chkSmg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSmg.UseVisualStyleBackColor = true;
            this.chkSmg.CheckedChanged += new System.EventHandler(this.ChkSmg_CheckedChanged);
            // 
            // chkAssault
            // 
            this.chkAssault.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkAssault.Location = new System.Drawing.Point(41, 252);
            this.chkAssault.Margin = new System.Windows.Forms.Padding(15);
            this.chkAssault.Name = "chkAssault";
            this.chkAssault.Size = new System.Drawing.Size(60, 17);
            this.chkAssault.TabIndex = 13;
            this.chkAssault.Text = "Assault";
            this.chkAssault.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkAssault.UseVisualStyleBackColor = true;
            this.chkAssault.CheckedChanged += new System.EventHandler(this.ChkAssault_CheckedChanged);
            // 
            // chkHeavy
            // 
            this.chkHeavy.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkHeavy.Location = new System.Drawing.Point(44, 291);
            this.chkHeavy.Margin = new System.Windows.Forms.Padding(15);
            this.chkHeavy.Name = "chkHeavy";
            this.chkHeavy.Size = new System.Drawing.Size(57, 17);
            this.chkHeavy.TabIndex = 16;
            this.chkHeavy.Text = "Heavy";
            this.chkHeavy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkHeavy.UseVisualStyleBackColor = true;
            this.chkHeavy.CheckedChanged += new System.EventHandler(this.ChkHeavy_CheckedChanged);
            // 
            // chkSniper
            // 
            this.chkSniper.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSniper.Location = new System.Drawing.Point(45, 331);
            this.chkSniper.Margin = new System.Windows.Forms.Padding(15);
            this.chkSniper.Name = "chkSniper";
            this.chkSniper.Size = new System.Drawing.Size(56, 17);
            this.chkSniper.TabIndex = 19;
            this.chkSniper.Text = "Sniper";
            this.chkSniper.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSniper.UseVisualStyleBackColor = true;
            // 
            // chkSpecial
            // 
            this.chkSpecial.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSpecial.Location = new System.Drawing.Point(40, 371);
            this.chkSpecial.Margin = new System.Windows.Forms.Padding(15);
            this.chkSpecial.Name = "chkSpecial";
            this.chkSpecial.Size = new System.Drawing.Size(61, 17);
            this.chkSpecial.TabIndex = 25;
            this.chkSpecial.Text = "Special";
            this.chkSpecial.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSpecial.UseVisualStyleBackColor = true;
            this.chkSpecial.CheckedChanged += new System.EventHandler(this.ChkSpecial_CheckedChanged);
            // 
            // cmbHand
            // 
            this.cmbHand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbHand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHand.FormattingEnabled = true;
            this.cmbHand.Items.AddRange(new object[] {
            "Fists",
            "Brass Kunckles"});
            this.cmbHand.Location = new System.Drawing.Point(131, 13);
            this.cmbHand.Margin = new System.Windows.Forms.Padding(15);
            this.cmbHand.Name = "cmbHand";
            this.cmbHand.Size = new System.Drawing.Size(171, 21);
            this.cmbHand.TabIndex = 1;
            this.cmbHand.SelectedIndexChanged += new System.EventHandler(this.CmbHand_SelectedIndexChanged);
            // 
            // cmbMelee
            // 
            this.cmbMelee.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbMelee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMelee.FormattingEnabled = true;
            this.cmbMelee.Items.AddRange(new object[] {
            "Chisel",
            "Axe",
            "Hockey Stick",
            "Nightstick",
            "Baseball Bat",
            "Cleaver",
            "Katana",
            "Knife",
            "Machete",
            "Chainsaw"});
            this.cmbMelee.Location = new System.Drawing.Point(131, 52);
            this.cmbMelee.Margin = new System.Windows.Forms.Padding(15);
            this.cmbMelee.Name = "cmbMelee";
            this.cmbMelee.Size = new System.Drawing.Size(171, 21);
            this.cmbMelee.TabIndex = 3;
            this.cmbMelee.SelectedIndexChanged += new System.EventHandler(this.CmbMelee_SelectedIndexChanged);
            // 
            // cmbProjectile
            // 
            this.cmbProjectile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbProjectile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProjectile.FormattingEnabled = true;
            this.cmbProjectile.Items.AddRange(new object[] {
            "Grenades",
            "Remote Grenades",
            "Molotov Cocktails",
            "Tear Gas"});
            this.cmbProjectile.Location = new System.Drawing.Point(131, 91);
            this.cmbProjectile.Margin = new System.Windows.Forms.Padding(15);
            this.cmbProjectile.Name = "cmbProjectile";
            this.cmbProjectile.Size = new System.Drawing.Size(171, 21);
            this.cmbProjectile.TabIndex = 29;
            this.cmbProjectile.SelectedIndexChanged += new System.EventHandler(this.CmbProjectile_SelectedIndexChanged);
            // 
            // cmbPistol
            // 
            this.cmbPistol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPistol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPistol.FormattingEnabled = true;
            this.cmbPistol.Items.AddRange(new object[] {
            "Pistol",
            ".357"});
            this.cmbPistol.Location = new System.Drawing.Point(131, 130);
            this.cmbPistol.Margin = new System.Windows.Forms.Padding(15);
            this.cmbPistol.Name = "cmbPistol";
            this.cmbPistol.Size = new System.Drawing.Size(171, 21);
            this.cmbPistol.TabIndex = 5;
            this.cmbPistol.SelectedIndexChanged += new System.EventHandler(this.CmbPistol_SelectedIndexChanged);
            // 
            // cmbShotgun
            // 
            this.cmbShotgun.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbShotgun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShotgun.FormattingEnabled = true;
            this.cmbShotgun.Items.AddRange(new object[] {
            "Shotgun",
            "Stubby Shotgun",
            "SPAS 12"});
            this.cmbShotgun.Location = new System.Drawing.Point(131, 169);
            this.cmbShotgun.Margin = new System.Windows.Forms.Padding(15);
            this.cmbShotgun.Name = "cmbShotgun";
            this.cmbShotgun.Size = new System.Drawing.Size(171, 21);
            this.cmbShotgun.TabIndex = 8;
            this.cmbShotgun.SelectedIndexChanged += new System.EventHandler(this.CmbShotgun_SelectedIndexChanged);
            // 
            // cmbSmg
            // 
            this.cmbSmg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSmg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSmg.FormattingEnabled = true;
            this.cmbSmg.Items.AddRange(new object[] {
            "Tec-9",
            "Mac-10",
            "Micro SMG",
            "MP5k"});
            this.cmbSmg.Location = new System.Drawing.Point(131, 208);
            this.cmbSmg.Margin = new System.Windows.Forms.Padding(15);
            this.cmbSmg.Name = "cmbSmg";
            this.cmbSmg.Size = new System.Drawing.Size(171, 21);
            this.cmbSmg.TabIndex = 11;
            this.cmbSmg.SelectedIndexChanged += new System.EventHandler(this.CmbSmg_SelectedIndexChanged);
            // 
            // cmbAssault
            // 
            this.cmbAssault.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAssault.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssault.FormattingEnabled = true;
            this.cmbAssault.Items.AddRange(new object[] {
            "AK-47",
            "M4"});
            this.cmbAssault.Location = new System.Drawing.Point(131, 248);
            this.cmbAssault.Margin = new System.Windows.Forms.Padding(15);
            this.cmbAssault.Name = "cmbAssault";
            this.cmbAssault.Size = new System.Drawing.Size(171, 21);
            this.cmbAssault.TabIndex = 14;
            this.cmbAssault.SelectedIndexChanged += new System.EventHandler(this.CmbAssault_SelectedIndexChanged);
            // 
            // cmbHeavy
            // 
            this.cmbHeavy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbHeavy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHeavy.FormattingEnabled = true;
            this.cmbHeavy.Items.AddRange(new object[] {
            "M60",
            "Minigun",
            "Flamethrower",
            "Rocket Launcher"});
            this.cmbHeavy.Location = new System.Drawing.Point(131, 287);
            this.cmbHeavy.Margin = new System.Windows.Forms.Padding(15);
            this.cmbHeavy.Name = "cmbHeavy";
            this.cmbHeavy.Size = new System.Drawing.Size(171, 21);
            this.cmbHeavy.TabIndex = 17;
            this.cmbHeavy.SelectedIndexChanged += new System.EventHandler(this.CmbHeavy_SelectedIndexChanged);
            // 
            // cmbSniper
            // 
            this.cmbSniper.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSniper.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSniper.FormattingEnabled = true;
            this.cmbSniper.Items.AddRange(new object[] {
            "Sniper Rifle",
            "Laser Sighted Sniper Rifle"});
            this.cmbSniper.Location = new System.Drawing.Point(131, 327);
            this.cmbSniper.Margin = new System.Windows.Forms.Padding(15);
            this.cmbSniper.Name = "cmbSniper";
            this.cmbSniper.Size = new System.Drawing.Size(171, 21);
            this.cmbSniper.TabIndex = 20;
            this.cmbSniper.SelectedIndexChanged += new System.EventHandler(this.CmbSniper_SelectedIndexChanged);
            // 
            // cmbSpecial
            // 
            this.cmbSpecial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSpecial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSpecial.FormattingEnabled = true;
            this.cmbSpecial.Items.AddRange(new object[] {
            "Camera"});
            this.cmbSpecial.Location = new System.Drawing.Point(131, 369);
            this.cmbSpecial.Margin = new System.Windows.Forms.Padding(15);
            this.cmbSpecial.Name = "cmbSpecial";
            this.cmbSpecial.Size = new System.Drawing.Size(171, 21);
            this.cmbSpecial.TabIndex = 26;
            this.cmbSpecial.SelectedIndexChanged += new System.EventHandler(this.CmbSpecial_SelectedIndexChanged);
            // 
            // numProjectileAmmo
            // 
            this.numProjectileAmmo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numProjectileAmmo.Location = new System.Drawing.Point(332, 91);
            this.numProjectileAmmo.Margin = new System.Windows.Forms.Padding(15);
            this.numProjectileAmmo.Name = "numProjectileAmmo";
            this.numProjectileAmmo.Size = new System.Drawing.Size(86, 20);
            this.numProjectileAmmo.TabIndex = 30;
            this.numProjectileAmmo.ValueChanged += new System.EventHandler(this.NumProjectileAmmo_ValueChanged);
            // 
            // numPistolAmmo
            // 
            this.numPistolAmmo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numPistolAmmo.Location = new System.Drawing.Point(332, 130);
            this.numPistolAmmo.Margin = new System.Windows.Forms.Padding(15);
            this.numPistolAmmo.Name = "numPistolAmmo";
            this.numPistolAmmo.Size = new System.Drawing.Size(86, 20);
            this.numPistolAmmo.TabIndex = 6;
            this.numPistolAmmo.ValueChanged += new System.EventHandler(this.NumPistolAmmo_ValueChanged);
            // 
            // numShotgunAmmo
            // 
            this.numShotgunAmmo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numShotgunAmmo.Location = new System.Drawing.Point(332, 169);
            this.numShotgunAmmo.Margin = new System.Windows.Forms.Padding(15);
            this.numShotgunAmmo.Name = "numShotgunAmmo";
            this.numShotgunAmmo.Size = new System.Drawing.Size(86, 20);
            this.numShotgunAmmo.TabIndex = 9;
            this.numShotgunAmmo.ValueChanged += new System.EventHandler(this.NumShotgunAmmo_ValueChanged);
            // 
            // numSmgAmmo
            // 
            this.numSmgAmmo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numSmgAmmo.Location = new System.Drawing.Point(332, 208);
            this.numSmgAmmo.Margin = new System.Windows.Forms.Padding(15);
            this.numSmgAmmo.Name = "numSmgAmmo";
            this.numSmgAmmo.Size = new System.Drawing.Size(86, 20);
            this.numSmgAmmo.TabIndex = 12;
            this.numSmgAmmo.ValueChanged += new System.EventHandler(this.NumSmgAmmo_ValueChanged);
            // 
            // numAssaultAmmo
            // 
            this.numAssaultAmmo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numAssaultAmmo.Location = new System.Drawing.Point(332, 248);
            this.numAssaultAmmo.Margin = new System.Windows.Forms.Padding(15);
            this.numAssaultAmmo.Name = "numAssaultAmmo";
            this.numAssaultAmmo.Size = new System.Drawing.Size(86, 20);
            this.numAssaultAmmo.TabIndex = 15;
            this.numAssaultAmmo.ValueChanged += new System.EventHandler(this.NumAssaultAmmo_ValueChanged);
            // 
            // numHeavyAmmo
            // 
            this.numHeavyAmmo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numHeavyAmmo.Location = new System.Drawing.Point(332, 287);
            this.numHeavyAmmo.Margin = new System.Windows.Forms.Padding(15);
            this.numHeavyAmmo.Name = "numHeavyAmmo";
            this.numHeavyAmmo.Size = new System.Drawing.Size(86, 20);
            this.numHeavyAmmo.TabIndex = 18;
            this.numHeavyAmmo.ValueChanged += new System.EventHandler(this.NumHeavyAmmo_ValueChanged);
            // 
            // numSniperAmmo
            // 
            this.numSniperAmmo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numSniperAmmo.Location = new System.Drawing.Point(332, 327);
            this.numSniperAmmo.Margin = new System.Windows.Forms.Padding(15);
            this.numSniperAmmo.Name = "numSniperAmmo";
            this.numSniperAmmo.Size = new System.Drawing.Size(86, 20);
            this.numSniperAmmo.TabIndex = 21;
            this.numSniperAmmo.ValueChanged += new System.EventHandler(this.NumSniperAmmo_ValueChanged);
            // 
            // numSpecialAmmo
            // 
            this.numSpecialAmmo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numSpecialAmmo.Location = new System.Drawing.Point(332, 369);
            this.numSpecialAmmo.Margin = new System.Windows.Forms.Padding(15);
            this.numSpecialAmmo.Name = "numSpecialAmmo";
            this.numSpecialAmmo.Size = new System.Drawing.Size(86, 20);
            this.numSpecialAmmo.TabIndex = 27;
            this.numSpecialAmmo.ValueChanged += new System.EventHandler(this.NumSpecialAmmo_ValueChanged);
            // 
            // WeaponsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "WeaponsPage";
            this.Size = new System.Drawing.Size(460, 408);
            this.Load += new System.EventHandler(this.Page_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            this.panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numProjectileAmmo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPistolAmmo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numShotgunAmmo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSmgAmmo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAssaultAmmo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeavyAmmo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSniperAmmo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpecialAmmo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.NumericUpDown numSniperAmmo;
        private System.Windows.Forms.NumericUpDown numHeavyAmmo;
        private System.Windows.Forms.NumericUpDown numAssaultAmmo;
        private System.Windows.Forms.NumericUpDown numSmgAmmo;
        private System.Windows.Forms.NumericUpDown numShotgunAmmo;
        private System.Windows.Forms.NumericUpDown numPistolAmmo;
        private System.Windows.Forms.ComboBox cmbShotgun;
        private System.Windows.Forms.ComboBox cmbSniper;
        private System.Windows.Forms.ComboBox cmbHeavy;
        private System.Windows.Forms.ComboBox cmbAssault;
        private System.Windows.Forms.ComboBox cmbSmg;
        private System.Windows.Forms.ComboBox cmbPistol;
        private System.Windows.Forms.ComboBox cmbMelee;
        private System.Windows.Forms.CheckBox chkAssault;
        private System.Windows.Forms.CheckBox chkSniper;
        private System.Windows.Forms.CheckBox chkHeavy;
        private System.Windows.Forms.CheckBox chkSmg;
        private System.Windows.Forms.CheckBox chkShotgun;
        private System.Windows.Forms.CheckBox chkPistol;
        private System.Windows.Forms.CheckBox chkMelee;
        private System.Windows.Forms.ComboBox cmbHand;
        private System.Windows.Forms.CheckBox chkHand;
        private System.Windows.Forms.CheckBox chkSpecial;
        private System.Windows.Forms.ComboBox cmbSpecial;
        private System.Windows.Forms.NumericUpDown numSpecialAmmo;
        private System.Windows.Forms.CheckBox chkProjectile;
        private System.Windows.Forms.ComboBox cmbProjectile;
        private System.Windows.Forms.NumericUpDown numProjectileAmmo;
    }
}
