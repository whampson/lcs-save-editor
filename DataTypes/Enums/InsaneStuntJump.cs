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

namespace LcsSaveEditor.DataTypes
{
    /// <summary>
    /// Insane Stunt Jump types.
    /// </summary>
    public enum InsaneStuntJump : uint
    {
        [Description("No Insane Stunts Completed")]
        NoInsaneStuntsCompleted,

        [Description("Insane Stunt")]
        InsaneStunt,

        [Description("Perfect Insane Stunt")]
        PerfectInsaneStunt,

        [Description("Double Insane Stunt")]
        DoubleInsaneStunt,

        [Description("Perfect Double Insane Stunt")]
        PerfectDoubleInsaneStunt,

        [Description("Triple Insane Stunt")]
        TripleInsaneStunt,

        [Description("Perfect Triple Insane Stunt")]
        PerfectTripleInsaneStunt,

        [Description("Quadruple Insane Stunt")]
        QuadrupleInsaneStunt,

        [Description("Perfect Quadruple Insane Stunt")]
        PerfectQuadrupleInsaneStunt
    }
}
