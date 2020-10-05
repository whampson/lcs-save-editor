using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using LCSSaveEditor.Core;
using Xceed.Wpf.Toolkit;

namespace LCSSaveEditor.GUI.Types
{
    public static class VehicleInfo
    {
        public static ObservableCollection<ColorItem> CarColors => new ObservableCollection<ColorItem>(
            Carcols.TheCarcols.Colors.Select(x => new ColorItem(Color.FromRgb(x.R, x.G, x.B), "")));
    }

    public enum Vehicle
    {
        [Description("(empty)")]
        None,

        [Description("Deimos SP")]
        SPIDER = 130,
        
        [Description("Landstalker")]
        LANDSTAL = 131,
        
        [Description("Idaho")]
        IDAHO = 132,
        
        [Description("Stinger")]
        STINGER = 133,
        
        [Description("Linerunner")]
        LINERUN = 134,
        
        [Description("Perennial")]
        PEREN = 135,
        
        [Description("Sentinel")]
        SENTINEL = 136,
        
        [Description("Patriot")]
        PATRIOT = 137,
        
        [Description("Firetruck")]
        FIRETRUK = 138,
        
        [Description("Trashmaster")]
        TRASH = 139,
        
        [Description("Stretch")]
        STRETCH = 140,
        
        [Description("Manana")]
        MANANA = 141,
        
        [Description("Infernus")]
        INFERNUS = 142,
        
        [Description("Blista")]
        BLISTA = 143,
        
        [Description("Pony")]
        PONY = 144,
        
        [Description("Mule")]
        MULE = 145,
        
        [Description("Cheetah")]
        CHEETAH = 146,
        
        [Description("Ambulance")]
        AMBULAN = 147,
        
        [Description("FBI Cruiser")]
        FBICAR = 148,
        
        [Description("Moonbeam")]
        MOONBEAM = 149,
        
        [Description("Esperanto")]
        ESPERANT = 150,
        
        [Description("Taxi")]
        TAXI = 151,
        
        [Description("Kuruma")]
        KURUMA = 152,
        
        [Description("Bobcat")]
        BOBCAT = 153,
        
        [Description("Mr Whoopee")]
        MRWHOOP = 154,
        
        [Description("BF Injection")]
        BFINJECT = 155,
        
        [Description("Hearse")]
        HEARSE = 156,
        
        [Description("Police")]
        POLICE = 157,
        
        [Description("Enforcer")]
        ENFORCER = 158,
        
        [Description("Securicar")]
        SECURICA = 159,
        
        [Description("Banshee")]
        BANSHEE = 160,
        
        [Description("Bus")]
        BUS = 161,
        
        [Description("Rhino")]
        RHINO = 162,
        
        [Description("Barracks OL")]
        BARRACKS = 163,
        
        [Description("Dodo")]
        DODO = 164,
        
        [Description("Coach")]
        COACH = 165,
        
        [Description("Cabbie")]
        CABBIE = 166,
        
        [Description("Stallion")]
        STALLION = 167,
        
        [Description("Rumpo")]
        RUMPO = 168,
        
        [Description("RC Bandit")]
        RCBANDIT = 169,
        
        [Description("Triad Fish Van")]
        BELLYUP = 170,
        
        [Description("Mr Wongs")]
        MRWONGS = 171,
        
        [Description("Leone Sentinel")]
        MAFIA = 172,
        
        [Description("Yardie Lobo")]
        YARDIE = 173,
        
        [Description("Yakuza Stinger")]
        YAKUZA = 174,
        
        [Description("Diablo Stallion")]
        DIABLOS = 175,
        
        [Description("Cartel Cruiser")]
        COLUMB = 176,
        
        [Description("Hoods Rumpo XL")]
        HOODS = 177,
        
        [Description("Panlantic")]
        PANLANT = 178,
        
        [Description("Flatbed")]
        FLATBED = 179,
        
        [Description("Yankee")]
        YANKEE = 180,
        
        [Description("Bickle'76")]
        BORGNINE = 181,
        
        [Description("TOYZ")]
        TOYZ = 182,
        
        [Description("Campaign Rumpo")]
        CAMPVAN = 183,
        
        [Description("Ballot Van")]
        BALLOT = 184,
        
        [Description("Hellenbach GT")]
        SHELBY = 185,
        
        [Description("Phobos VT")]
        PONTIAC = 186,
        
        [Description("V8 Ghost")]
        ESPRIT = 187,
        
        [Description("Barracks OL (Ammo Truck)")]
        AMMOTRUK = 188,
        
        [Description("Thunder-Rodd")]
        HOTROD = 189,
        
        [Description("Sindacco Argento")]
        SINDACCO_CAR = 190,
        
        [Description("Forelli ExSess")]
        FORELLI_CAR = 191,

        [Description("Ferry")]
        FERRY = 192,

        [Description("Ghost")]
        GHOST = 193,
        
        [Description("Speeder")]
        SPEEDER = 194,
        
        [Description("Reefer")]
        REEFER = 195,
        
        [Description("Predator")]
        PREDATOR = 196,

        [Description("Train")]
        TRAIN = 197,

        [Description("Helicopter (Escape)")]
        ESCAPE = 198,

        [Description("Helicopter (Chopper)")]
        CHOPPER = 199,

        [Description("Aeroplane")]
        AIRTRAIN = 200,

        [Description("DeadDodo")]
        DEADDODO = 201,

        [Description("Angel")]
        ANGEL = 202,
        
        [Description("Pizzaboy")]
        PIZZABOY = 203,
        
        [Description("Noodleboy")]
        NOODLEBOY = 204,
        
        [Description("PCJ-600")]
        PCJ600 = 205,
        
        [Description("Faggio")]
        FAGGIO = 206,
        
        [Description("Freeway")]
        FREEWAY = 207,
        
        [Description("Avenger")]
        ANGEL2 = 208,
        
        [Description("Manchez")]
        SANCHEZ2 = 209,
        
        [Description("Sanchez")]
        SANCHEZ = 210,

        [Description("RC Goblin")]
        RCGOBLIN = 211,

        [Description("RC Raider")]
        RCRAIDER = 212,

        [Description("Hunter")]
        HUNTER = 213,
        
        [Description("Maverick")]
        MAVERICK = 214,
        
        [Description("Police Maverick")]
        POLMAV = 215,
        
        [Description("VCN Maverick")]
        VCNMAV = 216,
    }
}
