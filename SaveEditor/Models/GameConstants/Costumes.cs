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

namespace WHampson.LcsSaveEditor.Models.GameDataTypes
{
    [Flags]
    public enum Costumes : ushort
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
}
