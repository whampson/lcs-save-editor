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
    public enum ExportCars : uint
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
        Landstalker     = 1 << 15
    }
}
