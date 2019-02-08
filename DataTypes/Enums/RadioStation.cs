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
    /// Radio stations.
    /// </summary>
    public enum RadioStation : byte
    {
        /* TODO: MyMix on Android/iOS shares the same value as RadioOff
           on PS2/PSP. Figure out how to handle this.
           RadioStationConveter for View? */

        [Description("Head Radio")]
        HeadRadio,

        [Description("Double Clef FM")]
        DoubleClefFM,

        [Description("K-Jah")]
        KJah,

        [Description("Rise FM")]
        RiseFM,

        [Description("Lips 106")]
        Lips106,

        [Description("Radio Del Mundo")]
        RadioDelMundo,

        [Description("MSX98")]
        Msx98,

        [Description("Flashback FM")]
        FlashbackFM,

        [Description("The Liberty Jam")]
        TheLibertyJam,

        [Description("LCFR")]
        Lcfr,

        //MyMix,

        [Description("Radio Off")]
        RadioOff
    }
}
