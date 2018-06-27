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
}
