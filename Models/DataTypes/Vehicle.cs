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

using System.ComponentModel;

namespace LcsSaveEditor.Models.DataTypes
{
    /// <summary>
    /// Vehicles present in Grand Theft Auto: Liberty City Stories
    /// </summary>
    public enum Vehicle
    {
        [Description("Deimos SP")]
        DeimosSP = 130,

        [Description("Landstalker")]
        Landstalker,

        [Description("Idaho")]
        Idaho,

        [Description("Stinger")]
        Stinger,

        [Description("Linerunner")]
        Linerunner,

        [Description("Perennial")]
        Perennial,

        [Description("Sentinel")]
        Sentinel,

        [Description("Patriot")]
        Patriot,

        [Description("Firetruck")]
        Firetruck,

        [Description("Trashmaster")]
        Trashmaster,

        [Description("Stretch")]
        Stretch,

        [Description("Manana")]
        Manana,

        [Description("Infernus")]
        Infernus,

        [Description("Blista")]
        Blista,

        [Description("Pony")]
        Pony,

        [Description("Mule")]
        Mule,

        [Description("Cheetah")]
        Cheetah,

        [Description("Ambulance")]
        Ambulance,

        [Description("FBI Cruiser")]
        FbiCruiser,

        [Description("Moonbeam")]
        Moonbeam,

        [Description("Esperanto")]
        Esperanto,

        [Description("Taxi")]
        Taxi,

        [Description("Kuruma")]
        Kuruma,

        [Description("Bobcat")]
        Bobcat,

        [Description("Mr Whoopee")]
        MrWhoopee,

        [Description("BF Injection")]
        BFInjection,

        [Description("Hearse")]
        Hearse,

        [Description("Police")]
        Police,

        [Description("Enforcer")]
        Enforcer,

        [Description("Securicar")]
        Securicar,

        [Description("Banshee")]
        Banshee,

        [Description("Bus")]
        Bus,

        [Description("Rhino")]
        Rhino,

        [Description("Barracks OL")]
        BarracksOL,

        [Description("Dodo")]
        Dodo,               // can't move

        [Description("Coach")]
        Coach,

        [Description("Cabbie")]
        Cabbie,

        [Description("Stallion")]
        Stallion,

        [Description("Rumpo")]
        Rumpo,

        [Description("RC Bandit")]
        RCBandit,

        [Description("Triad Fish Van")]
        TriadFishVan,

        [Description("Mr Wongs")]
        MrWongs,

        [Description("Leone Sentinel")]
        LeoneSentinel,

        [Description("Yardie Lobo")]
        YardieLobo,

        [Description("Yakuza Stinger")]
        YakuzaStinger,

        [Description("Diablo Stallion")]
        DiabloStallion,

        [Description("Cartel Cruiser")]
        CartelCruiser,

        [Description("Hoods Rumpo XL")]
        HoodsRumpoXL,

        [Description("Panlantic")]
        Panlantic,

        [Description("Flatbed")]
        Flatbed,

        [Description("Yankee")]
        Yankee,

        [Description("Bickle'76")]
        Bickle76,

        [Description("TOYZ")]
        Toyz,

        [Description("Campaign Rumpo")]
        CampaignRumpo,

        [Description("Ballot Van")]
        BallotVan,

        [Description("Hellenbach GT")]
        HellenbachGT,

        [Description("Phobos VT")]
        PhobosVT,

        [Description("V8 Ghost")]
        V8Ghost,

        [Description("Barracks OL (Ammo Truck)")]
        AmmoTruck,

        [Description("Thunder-Rodd")]
        ThunderRodd,

        [Description("Sindacco Argento")]
        SindaccoArgento,

        [Description("Forelli Exsess")]
        ForelliExsess,

        [Description("Ferry")]
        Ferry,

        [Description("Ghost")]
        Ghost,

        [Description("Speeder")]
        Speeder,

        [Description("Reefer")]
        Reefer,

        [Description("Predator")]
        Predator,

        [Description("Train")]
        Train,                  // enterable?

        [Description("Escape")]
        Escape,                 // invisible, can't enter

        [Description("Helicopter")]
        Helicopter,             // invisible, enterable

        [Description("Airtrain")]
        Airtrain,               // can't enter -- touching the fuselage will kill Toni!

        [Description("DeadDodo")]
        DeadDodo,               // can't enter

        [Description("Angel")]
        Angel,

        [Description("Pizzaboy")]
        Pizzaboy,

        [Description("Noodleboy")]
        Noodleboy,

        [Description("PCJ-600")]
        Pcj600,

        [Description("Faggio")]
        Faggio,

        [Description("Freeway")]
        Freeway,

        [Description("Avenger")]
        Avenger,

        [Description("Manchez")]
        Manchez,

        [Description("Sanchez")]
        Sanchez,

        [Description("RC Goblin")]
        RCGoblin,

        [Description("RC Raider")]
        RCRaider,

        [Description("Hunter")]
        Hunter,

        [Description("Maverick")]
        Maverick,

        [Description("Police Maverick")]
        PoliceMaverick,

        [Description("News Maverick")]
        NewsMaverick
    }
}
