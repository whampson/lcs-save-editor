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
        [Description("(none)")]
        None,

        [Description("Deimos SP")]
        SPIDER = 130,
        
        [Description("Landstalker")]
        LANDSTK = 131,
        
        [Description("Idaho")]
        IDAHO = 132,
        
        [Description("Stinger")]
        STINGER = 133,
        
        [Description("Linerunner")]
        LINERUN = 134,
        
        [Description("Perennial")]
        PEREN = 135,
        
        [Description("Sentinel")]
        SENTINL = 136,
        
        [Description("Patriot")]
        PATRIOT = 137,
        
        [Description("Firetruck")]
        FIRETRK = 138,
        
        [Description("Trashmaster")]
        TRASHM = 139,
        
        [Description("Stretch")]
        STRETCH = 140,
        
        [Description("Manana")]
        MANANA = 141,
        
        [Description("Infernus")]
        INFERNS = 142,
        
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
        MOONBM = 149,
        
        [Description("Esperanto")]
        ESPERAN = 150,
        
        [Description("Taxi")]
        TAXI = 151,
        
        [Description("Kuruma")]
        KURUMA = 152,
        
        [Description("Bobcat")]
        BOBCAT = 153,
        
        [Description("Mr Whoopee")]
        WHOOPEE = 154,
        
        [Description("BF Injection")]
        BFINJC = 155,
        
        [Description("Hearse")]
        HEARSE = 156,
        
        [Description("Police")]
        POLICAR = 157,
        
        [Description("Enforcer")]
        ENFORCR = 158,
        
        [Description("Securicar")]
        SECURI = 159,
        
        [Description("Banshee")]
        BANSHEE = 160,
        
        [Description("Bus")]
        BUS = 161,
        
        [Description("Rhino")]
        RHINO = 162,
        
        [Description("Barracks OL")]
        BARRCKS = 163,
        
        [Description("Dodo")]
        DODO = 164,
        
        [Description("Coach")]
        COACH = 165,
        
        [Description("Cabbie")]
        CABBIE = 166,
        
        [Description("Stallion")]
        STALION = 167,
        
        [Description("Rumpo")]
        RUMPO = 168,
        
        [Description("RC Bandit")]
        RCBANDT = 169,
        
        [Description("Triad Fish Van")]
        BELLYUP = 170,
        
        [Description("Mr Wongs")]
        MRWONGS = 171,
        
        [Description("Leone Sentinel")]
        MAFIACR = 172,
        
        [Description("Yardie Lobo")]
        YARDICR = 173,
        
        [Description("Yakuza Stinger")]
        YAKUZCR = 174,
        
        [Description("Diablo Stallion")]
        DIABLCR = 175,
        
        [Description("Cartel Cruiser")]
        COLOMCR = 176,
        
        [Description("Hoods Rumpo XL")]
        HOODSCR = 177,
        
        [Description("Panlantic")]
        PANLANT = 178,
        
        [Description("Flatbed")]
        FLATBED = 179,
        
        [Description("Yankee")]
        YANKEE = 180,
        
        [Description("Bickle'76")]
        BORGNIN = 181,
        
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
        
        [Description("Barracks OL (Cargo)")]
        BARRACK = 188,
        
        [Description("Thunder-Rodd")]
        HOTROD = 189,
        
        [Description("Sindacco Argento")]
        SINDACO = 190,
        
        [Description("Forelli ExSess")]
        FORELLI = 191,
        
        //FERRY = 192,
        
        [Description("Ghost")]
        GHOST = 193,
        
        [Description("Speeder")]
        SPEEDER = 194,
        
        [Description("Reefer")]
        REEFER = 195,
        
        [Description("Predator")]
        PREDATR = 196,
        
        // TRAIN = 197,
        
        // HELI = 198,
        
        // HELI = 199,
        
        // AEROPL = 200,
        
        // DODO = 201,
        
        [Description("Angel")]
        ANGEL = 202,
        
        [Description("Pizzaboy")]
        PIZZABO = 203,
        
        [Description("Noodleboy")]
        NOODLBO = 204,
        
        [Description("PCJ-600")]
        PCJ600 = 205,
        
        [Description("Faggio")]
        FAGGIO = 206,
        
        [Description("Freeway")]
        FREEWAY = 207,
        
        [Description("Avenger")]
        ANGEL2 = 208,
        
        [Description("Manchez")]
        SANCH2 = 209,
        
        [Description("Sanchez")]
        SANCHEZ = 210,
        
        // RCGOBLI = 211,
        
        // RCRAIDE = 212,
        
        [Description("Hunter")]
        HUNTER = 213,
        
        [Description("Maverick")]
        MAVERIC = 214,
        
        [Description("Police Maverick")]
        POLMAV = 215,
        
        [Description("VCN Maverick")]
        VCNMAV = 216,
    }
}
