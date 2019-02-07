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

using LcsSaveEditor.Infrastructure;

namespace LcsSaveEditor.Models
{
    public abstract class FavoriteRadioStationList : SerializableObject
    {
        protected float m_headRadioListenTime;
        protected float m_doubleClefFMListenTime;
        protected float m_kJahListenTime;
        protected float m_riseFMListenTime;
        protected float m_lips106ListenTime;
        protected float m_radioDelMundoListenTime;
        protected float m_msx98ListenTime;
        protected float m_flashbackFMListenTime;
        protected float m_theLibertyJamListenTime;
        protected float m_lcfrListenTime;
        protected float m_mixTapeListenTime;

        public float HeadRadioListenTime
        {
            get { return m_headRadioListenTime; }
            set { m_headRadioListenTime = value; OnPropertyChanged(); }
        }

        public float DoubleClefFMListenTime
        {
            get { return m_doubleClefFMListenTime; }
            set { m_doubleClefFMListenTime = value; OnPropertyChanged(); }
        }

        public float KJahListenTime
        {
            get { return m_kJahListenTime; }
            set { m_kJahListenTime = value; OnPropertyChanged(); }
        }

        public float RiseFMListenTime
        {
            get { return m_riseFMListenTime; }
            set { m_riseFMListenTime = value; OnPropertyChanged(); }
        }

        public float Lips106ListenTime
        {
            get { return m_lips106ListenTime; }
            set { m_lips106ListenTime = value; OnPropertyChanged(); }
        }

        public float RadioDelMundoListenTime
        {
            get { return m_radioDelMundoListenTime; }
            set { m_radioDelMundoListenTime = value; OnPropertyChanged(); }
        }

        public float Msx98ListenTime
        {
            get { return m_msx98ListenTime; }
            set { m_msx98ListenTime = value; OnPropertyChanged(); }
        }

        public float FlashbackFMListenTime
        {
            get { return m_flashbackFMListenTime; }
            set { m_flashbackFMListenTime = value; OnPropertyChanged(); }
        }

        public float TheLibertyJamListenTime
        {
            get { return m_theLibertyJamListenTime; }
            set { m_theLibertyJamListenTime = value; OnPropertyChanged(); }
        }

        public float LcfrListenTime
        {
            get { return m_lcfrListenTime; }
            set { m_lcfrListenTime = value; OnPropertyChanged(); }
        }

        public float MixTapeListenTime
        {
            get { return m_mixTapeListenTime; }
            set { m_mixTapeListenTime = value; OnPropertyChanged(); }
        }

        public override string ToString()
        {
            return string.Format("{0} = {1}, {2} = {3}, {4} = {5}, {6} = {7}, {8} = {9}, {10} = {11}, " +
                                 "{12} = {13}, {14} = {15}, {16} = {17}, {18} = {19}, {20} = {21}",
                nameof(HeadRadioListenTime), HeadRadioListenTime,
                nameof(DoubleClefFMListenTime), DoubleClefFMListenTime,
                nameof(KJahListenTime), KJahListenTime,
                nameof(RiseFMListenTime), RiseFMListenTime,
                nameof(Lips106ListenTime), Lips106ListenTime,
                nameof(RadioDelMundoListenTime), RadioDelMundoListenTime,
                nameof(Msx98ListenTime), Msx98ListenTime,
                nameof(FlashbackFMListenTime), FlashbackFMListenTime,
                nameof(TheLibertyJamListenTime), TheLibertyJamListenTime,
                nameof(LcfrListenTime), LcfrListenTime,
                nameof(MixTapeListenTime), MixTapeListenTime);
        }
    }
}