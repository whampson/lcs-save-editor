﻿#region License
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

using System.ComponentModel;

namespace WHampson.LcsSaveEditor.Models.GameDataTypes
{
    public enum Weapon
    {
        [Description("Fists")]
        Fists,

        [Description("Brass Knuckles")]
        BrassKnuckles,

        [Description("Chisel")]
        Chisel,

        [Description("Hockey Stick")]
        HockeyStick,

        [Description("Nightstick")]
        NightStick,

        [Description("Knife")]
        Knife,

        [Description("Baseball Bat")]
        BaseballBat,

        [Description("Axe")]
        Axe,

        [Description("Meat Cleaver")]
        Cleaver,

        [Description("Machete")]
        Machete,

        [Description("Katana")]
        Katana,

        [Description("Chainsaw")]
        Chainsaw,

        [Description("Grenades")]
        Grenades,

        [Description("Remote Grenades")]
        RemoteGrenades,

        [Description("Tear Gas")]
        TearGas,

        [Description("Molotov Cocktail")]
        MolotovCocktail,

        [Description("Pistol")]
        Pistol = 17,

        [Description(".357")]
        Python,

        [Description("Shotgun")]
        Shotgun,

        [Description("S.P.A.S. Shotgun")]
        SpasShotgun,

        [Description("Stubby Shotgun")]
        StubbyShotgun,

        [Description("Tec-9")]
        Tec9,

        [Description("Mac 10")]
        Mac10,

        [Description("Micro SMG")]
        MicroSmg,

        [Description("MP5k")]
        Mp5k,

        [Description("M4")]
        M4,

        [Description("AK-47")]
        Ak47,

        [Description("Sniper Rifle")]
        SniperRifle,

        [Description("Laser-Sighted Sniper Rifle")]
        LaserSightedSniperRifle,

        [Description("Rocket Launcher")]
        RocketLauncher,

        [Description("Flamethrower")]
        FlameThrower,

        [Description("M60")]
        M60,

        [Description("Minigun")]
        MiniGun,

        [Description("Camera")]
        Camera = 36
    }
}