using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LCSSaveEditor.GUI.Types
{
    public enum Weapon
    {
        [Description("(empty)")]
        None = -1,

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

        [Description("Molotovs")]
        Molotovs,

        [Description("Pistol")]
        Pistol = 17,

        [Description(".357 Magnum")]
        Python,

        [Description("Shotgun")]
        Shotgun,

        [Description("SPAS 12")]
        Spas12,

        [Description("Stubby Shotgun")]
        StubbyShotgun,

        [Description("Tec-9")]
        Tec9,

        [Description("Mac-10")]
        Mac10,

        [Description("Micro SMG")]
        MicroSMG,

        [Description("MP5")]
        MP5,

        [Description("M4")]
        M4,

        [Description("AK-47")]
        AK,

        [Description("Sniper")]
        Sniper,

        [Description("Laser Sniper")]
        LaserSniper,

        [Description("Rocket Launcher")]
        RocketLauncher,

        [Description("Flame Thrower")]
        FlameThrower,

        [Description("M60")]
        M60,

        [Description("Minigun")]
        Minigun,

        [Description("Camera")]
        Camera = 36
    }
}
