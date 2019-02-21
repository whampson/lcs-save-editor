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

namespace LcsSaveEditor.Models.DataTypes
{
    /// <summary>
    /// Cars that are wanted for export at Love Media.
    /// </summary>
    [Flags]
    public enum ExportCars : uint
    {
        [Description("Hearse")]
        Hearse = 1 << 0,

        [Description("Faggio")]
        Faggio = 1 << 1,

        [Description("Freeway")]
        Freeway = 1 << 2,

        [Description("Deimos SP")]
        DeimosSP = 1 << 3,

        [Description("Manana")]
        Manana = 1 << 4,

        [Description("Hellenbach GT")]
        HellenbachGT = 1 << 5,

        [Description("Phobos VT")]
        PhobosVT = 1 << 6,

        [Description("V8 Ghost")]
        V8Ghost = 1 << 7,

        [Description("Thunder Rodd")]
        ThunderRodd = 1 << 8,

        [Description("PCJ-600")]
        Pcj600 = 1 << 9,

        [Description("Sentinel")]
        Sentinel = 1 << 10,

        [Description("Infernus")]
        Infernus = 1 << 11,

        [Description("Banshee")]
        Banshee = 1 << 12,

        [Description("Patriot")]
        Patriot = 1 << 13,

        [Description("BF Injection")]
        BFInjection = 1 << 14,

        [Description("Landstalker")]
        Landstalker = 1 << 15
    }
}
