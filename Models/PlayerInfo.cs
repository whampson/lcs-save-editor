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
    public abstract class PlayerInfo : SerializableObject
    {
        protected int m_money;
        protected int m_moneyOnScreen;
        protected uint m_numHiddenPackagesFound;
        protected byte m_maxHealth;
        protected byte m_maxArmor;
        protected bool m_neverGetsTired;
        protected bool m_fastReload;
        protected bool m_fireProof;
        protected bool m_getOutOfJailFree;
        protected bool m_freeHealthCare;
        protected bool m_canDoDriveBy;

        public int Money
        {
            get { return m_money; }
            set { m_money = value; OnPropertyChanged(); }
        }

        public int MoneyOnScreen
        {
            get { return m_moneyOnScreen; }
            set { m_moneyOnScreen = value; OnPropertyChanged(); }
        }

        public uint NumHiddenPackagesFound
        {
            get { return m_numHiddenPackagesFound; }
            set { m_numHiddenPackagesFound = value; OnPropertyChanged(); }
        }

        public byte MaxHealth
        {
            get { return m_maxHealth; }
            set { m_maxHealth = value; OnPropertyChanged(); }
        }

        public byte MaxArmor
        {
            get { return m_maxArmor; }
            set { m_maxArmor = value; OnPropertyChanged(); }
        }

        public bool HasInfiniteSprint
        {
            get { return m_neverGetsTired; }
            set { m_neverGetsTired = value; OnPropertyChanged(); }
        }

        public bool HasFastReload
        {
            get { return m_fastReload; }
            set { m_fastReload = value; OnPropertyChanged(); }
        }

        public bool IsFireProof
        {
            get { return m_fireProof; }
            set { m_fireProof = value; OnPropertyChanged(); }
        }

        public bool HasGetOutOfJailFree
        {
            get { return m_getOutOfJailFree; }
            set { m_getOutOfJailFree = value; OnPropertyChanged(); }
        }

        public bool HasFreeHealthcare
        {
            get { return m_freeHealthCare; }
            set { m_freeHealthCare = value; OnPropertyChanged(); }
        }

        public bool CanDoDriveBy
        {
            get { return m_canDoDriveBy; }
            set { m_canDoDriveBy = value; OnPropertyChanged(); }
        }
    }
}
