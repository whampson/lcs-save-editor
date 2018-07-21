﻿#region License
/* Copyright(c) 2016-2018 Wes Hampson
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

using WHampson.Cascara;

namespace WHampson.LcsSaveEditor.Models.FileStructure
{
    public class PlayerBlock : DataBlock
    {
        private readonly Primitive<int> money;
        private readonly Primitive<int> moneyOnScreen;
        private readonly Primitive<Bool32> neverGetsTired;
        private readonly Primitive<Bool32> fastReload;
        private readonly Primitive<Bool32> fireProof;
        private readonly Primitive<byte> maxHealth;
        private readonly Primitive<byte> maxArmor;
        private readonly Primitive<Bool32> getOutOfJailFree;
        private readonly Primitive<Bool32> freeHealthCare;

        public PlayerBlock()
        {
            money = new Primitive<int>(null, 0);
            moneyOnScreen = new Primitive<int>(null, 0);
            neverGetsTired = new Primitive<Bool32>(null, 0);
            fastReload = new Primitive<Bool32>(null, 0);
            fireProof = new Primitive<Bool32>(null, 0);
            maxHealth = new Primitive<byte>(null, 0);
            maxArmor = new Primitive<byte>(null, 0);
            getOutOfJailFree = new Primitive<Bool32>(null, 0);
            freeHealthCare = new Primitive<Bool32>(null, 0);
        }

        public int Money
        {
            get { return money.Value; }
            set { money.Value = value; }
        }

        public int MoneyOnScreen
        {
            get { return moneyOnScreen.Value; }
            set { moneyOnScreen.Value = value; }
        }

        public Bool32 NeverGetsTired
        {
            get { return neverGetsTired.Value; }
            set { neverGetsTired.Value = value; }
        }

        public Bool32 FastReload
        {
            get { return fastReload.Value; }
            set { fastReload.Value = value; }
        }

        public Bool32 FireProof
        {
            get { return fireProof.Value; }
            set { fireProof.Value = value; }
        }

        public byte MaxHealth
        {
            get { return maxHealth.Value; }
            set { maxHealth.Value = value; }
        }

        public byte MaxArmor
        {
            get { return maxArmor.Value; }
            set { maxArmor.Value = value; }
        }

        public Bool32 GetOutOfJailFree
        {
            get { return getOutOfJailFree.Value; }
            set { getOutOfJailFree.Value = value; }
        }

        public Bool32 FreeHealthCare
        {
            get { return freeHealthCare.Value; }
            set { freeHealthCare.Value = value; }
        }
    }
}
