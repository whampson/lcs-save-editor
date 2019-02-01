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

using System;
using System.ComponentModel;

namespace LcsSaveEditor.DataTypes
{
    /// <summary>
    /// Outfits that Toni can wear.
    /// </summary>
    [Flags]
    public enum Costumes : ushort
    {
        [Description("Casual Clothes")]
        CasualClothes = 1 << 0,

        [Description("Leone's Suit")]
        LeonesSuit = 1 << 1,

        [Description("Overalls")]
        Overalls = 1 << 2,

        [Description("Avenging Angels Fatigues")]
        AvengingAngelsFatigues = 1 << 3,

        [Description("Chauffeur's Clothes")]
        ChauffeursClothes = 1 << 4,

        [Description("Lawyer's Suit")]
        LawyersSuit = 1 << 5,

        [Description("Tuxedo")]
        Tuxedo = 1 << 6,

        [Description("'The King' Jumpsuit")]
        TheKingJumpsuit = 1 << 7,

        [Description("Cox Mascot Suit")]
        CoxMascotSuit = 1 << 8,

        [Description("Underwear")]
        Underwear = 1 << 9,

        [Description("Hero Garb")]
        HeroGarb = 1 << 10,

        [Description("Dragon Jumpsuit")]
        DragonJumpsuit = 1 << 11,

        [Description("Antonio")]
        Antonio = 1 << 12,

        [Description("Sweats")]
        Sweats = 1 << 13,

        [Description("Goodfella")]
        Goodfella = 1 << 14,

        [Description("Wiseguy")]
        Wiseguy = 1 << 15
    }
}
