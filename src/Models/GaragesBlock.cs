#region License
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

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WHampson.Cascara;
using WHampson.LcsSaveEditor.DataTypes;

namespace WHampson.LcsSaveEditor.Models
{
    public class GaragesBlock : DataBlock
    {
        private readonly Primitive<uint> numGarages;
        private readonly Primitive<Bool32> bombsAreFree;
        private readonly Primitive<Bool32> respraysAreFree;
        private readonly Primitive<uint> carTypesCollected;
        private readonly Primitive<uint> timeLastHelpMessage;
        private readonly StoredCar[] carsInSafeHouse;

        public GaragesBlock()
        {
            numGarages = new Primitive<uint>(null, 0);
            bombsAreFree = new Primitive<Bool32>(null, 0);
            respraysAreFree = new Primitive<Bool32>(null, 0);
            carTypesCollected = new Primitive<uint>(null, 0);
            timeLastHelpMessage = new Primitive<uint>(null, 0);
            carsInSafeHouse = new StoredCar[0];
        }
        
        public uint GaragesCount
        {
            get { return numGarages.Value; }
            set { numGarages.Value = value; }
        }

        public Bool32 BombsAreFree
        {
            get { return bombsAreFree.Value; }
            set { bombsAreFree.Value = value; }
        }

        public Bool32 RespraysAreFree
        {
            get { return respraysAreFree.Value; }
            set { respraysAreFree.Value = value; }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public ExportCars CarTypesCollected
        {
            get { return (ExportCars) carTypesCollected.Value; }
            set { carTypesCollected.Value = (uint) value; }
        }

        public uint TimeLastHelpMessage
        {
            get { return timeLastHelpMessage.Value; }
            set { timeLastHelpMessage.Value = value; }
        }

        public StoredCar[] StoredCars
        {
            get { return carsInSafeHouse; }
        }
    }
}
