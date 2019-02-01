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
    /// Special properties that can be applied to any vehicle stored in a garage.
    /// </summary>
    [Flags]
    public enum StoredCarPerks : uint
    {
        [Description("Bullet-proof")]
        BulletProof     = 1 << 0,

        [Description("Fire-proof")]
        FireProof       = 1 << 1,

        [Description("Explosion-proof")]
        ExplosionProof  = 1 << 2,

        [Description("Collision-proof")]
        CollisionProof  = 1 << 3,

        [Description("Melee-proof")]
        MeleeProof      = 1 << 4,

        [Description("Pop-proof")]
        PopProof        = 1 << 5,

        [Description("Strong")]
        Strong          = 1 << 6,

        [Description("Heavy")]
        Heavy           = 1 << 7,

        [Description("Permanent Color")]
        PermanentColor  = 1 << 8,

        [Description("Timebomb Fitted")]
        TimebombFitted  = 1 << 9,

        [Description("Tip-proof")]
        TipProof        = 1 << 10,

        [Description("(unknown)")]
        _Unknown        = 1 << 15
    }
}
