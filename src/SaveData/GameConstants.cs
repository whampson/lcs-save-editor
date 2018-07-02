using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHampson.LcsSaveEditor.SaveData
{
    internal enum WeatherType : short
    {
        Sunny,
        Cloudy,
        Rainy,
        Foggy,
        ExtraSunny,
        Hurricane,
        ExtraColors,
        Snow
    }

    internal enum RadioStation : byte
    {
        HeadRadio,
        DoubleClefFm,
        KJah,
        RiseFm,
        Lips106,
        RadioDelMundo,
        Msx98,
        FlashbackFm,
        TheLibertyJam,
        Lcfr,
        //MyMix,
        RadioOff
    }

    internal enum RadarMode : uint
    {
        MapAndBlips,
        BlipsOnly,
        RadarOff
    }

    internal enum ControllerConfig : ushort
    {
        Setup1,
        Setup2
    }

    internal enum Language : uint
    {
        English,
        French,
        German,
        Italian,
        Spanish,
        Russian,
        Japanese
    }

    [Flags]
    internal enum LoveMediaCars : uint
    {
        Hearse          = 1 << 0,
        Faggio          = 1 << 1,
        Freeway         = 1 << 2,
        DeimosSP        = 1 << 3,
        Manana          = 1 << 4,
        HellenbachGT    = 1 << 5,
        PhobosVT        = 1 << 6,
        V8Ghost         = 1 << 7,
        ThunderRodd     = 1 << 8,
        Pcj600          = 1 << 9,
        Sentinel        = 1 << 10,
        Infernus        = 1 << 11,
        Banshee         = 1 << 12,
        Patriot         = 1 << 13,
        BFInjection     = 1 << 14,
        Landstalker     = 1 << 15,
    }

    [Flags]
    internal enum VehicleFlags : uint
    {
        BulletProof     = 1 << 0,
        FireProof       = 1 << 1,
        ExplosionProof  = 1 << 2,
        CollisionProof  = 1 << 3,
        MeleeProof      = 1 << 4,
        PopProof        = 1 << 5,
        QuadDamage      = 1 << 6,
        Heavy           = 1 << 7,
        PermanentColor  = 1 << 8,
        TimebombFitted  = 1 << 9,
        TipProof        = 1 << 10,
        _Unknown        = 1 << 15
    }

    [Flags]
    internal enum Costumes : ushort
    {
        CasualClothes           = 1 << 0,
        LeonesSuit              = 1 << 1,
        Overalls                = 1 << 2,
        AvengingAngelsFatigues  = 1 << 3,
        ChauffeursClothes       = 1 << 4,
        LawyersSuit             = 1 << 5,
        Tuxedo                  = 1 << 6,
        TheKingJumpsuit         = 1 << 7,
        CoxMascotSuit           = 1 << 8,
        Underwear               = 1 << 9,
        HeroGarb                = 1 << 10,
        DragonJumpsuit          = 1 << 11,
        Antonio                 = 1 << 12,
        Sweats                  = 1 << 13,
        Goodfella               = 1 << 14,
        Wiseguy                 = 1 << 15
    }

    internal enum InsaneStuntJump : uint
    {
        NoInstaneStuntsCompleted,
        InsaneStunt,
        PerfectInsaneStunt,
        DoubleInsaneStunt,
        PerfectDoubleInsaneStunt,
        TripleInsaneStunt,
        PerfectTripleInsaneStunt,
        QuadrupleInsaneStunt,
        PerfectQuadrupleInsaneStunt
    }
}
