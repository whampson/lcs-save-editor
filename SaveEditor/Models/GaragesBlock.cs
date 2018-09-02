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

using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using WHampson.LcsSaveEditor.Helpers;
using WHampson.LcsSaveEditor.Models.GameDataTypes;

namespace WHampson.LcsSaveEditor.Models
{
    public class GaragesBlock : SerializableObject
    {
        private uint m_numGarages;
        private bool m_bombsAreFree;
        private bool m_respraysAreFree;
        private uint m_carsCollected;           // not used
        private uint m_bankVansCollected;       // not used
        private uint m_policeCarsCollected;     // not used
        private uint m_carTypesCollected;
        private uint m_carTypesCollected2;      // not used
        private uint m_carTypesCollected3;      // not used
        private uint m_carTypesCollected4;      // not used
        private uint m_timeLastHelpMessage;
        private StoredCar[] m_carsInSafeHouse;

        public GaragesBlock()
        {
            m_carsInSafeHouse = new StoredCar[48];
        }

        public bool BombsAreFree
        {
            get { return m_bombsAreFree; }
            set { m_bombsAreFree = value; FirePropertyChanged(); }
        }

        public bool RespraysAreFree
        {
            get { return m_respraysAreFree; }
            set { m_respraysAreFree = value; FirePropertyChanged(); }
        }

        public ExportCars LoveMediaCarsCollected
        {
            get { return (ExportCars) m_carTypesCollected; }
            set { m_carTypesCollected = (uint) value; FirePropertyChanged(); }
        }

        public ObservableCollection<StoredCar> StoredCars
        {
            get;
            private set;
        }

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_numGarages = r.ReadUInt32();
                m_bombsAreFree = r.ReadUInt32() != 0;
                m_respraysAreFree = r.ReadUInt32() != 0;
                m_carsCollected = r.ReadUInt32();
                m_bankVansCollected = r.ReadUInt32();
                m_policeCarsCollected = r.ReadUInt32();
                m_carTypesCollected = r.ReadUInt32();
                m_carTypesCollected2 = r.ReadUInt32();
                m_carTypesCollected3 = r.ReadUInt32();
                m_carTypesCollected4 = r.ReadUInt32();
                m_timeLastHelpMessage = r.ReadUInt32();

                for (int i = 0; i < m_carsInSafeHouse.Length; i++) {
                    m_carsInSafeHouse[i] = new StoredCar();
                    m_carsInSafeHouse[i].Deserialize(stream);
                }
                StoredCars = new ObservableCollection<StoredCar>(m_carsInSafeHouse);

                // TODO: garages
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write(m_numGarages);
                w.Write(m_bombsAreFree ? 1U : 0);
                w.Write(m_respraysAreFree ? 1U : 0);
                w.Write(m_carsCollected);
                w.Write(m_bankVansCollected);
                w.Write(m_policeCarsCollected);
                w.Write(m_carTypesCollected);
                w.Write(m_carTypesCollected2);
                w.Write(m_carTypesCollected3);
                w.Write(m_carTypesCollected4);
                w.Write(m_timeLastHelpMessage);

                foreach (StoredCar car in m_carsInSafeHouse) {
                    car.Serialize(stream);
                }

                // TODO: garages
            }

            return stream.Position - start;
        }
    }
}
