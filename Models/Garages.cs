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
using System.IO;
using System.Text;
using LcsSaveEditor.DataTypes;
using LcsSaveEditor.Extensions;
using LcsSaveEditor.Infrastructure;

namespace LcsSaveEditor.Models
{
    /// <summary>
    /// Keeps track of saved vehicles, garage objects, and
    /// the progress of the Love Media car exports.
    /// </summary>
    public abstract class Garages : SerializableObject
    {
        protected uint m_numGarages;
        protected bool m_bombsAreFree;
        protected bool m_respraysAreFree;
        protected uint _m_carsCollected;
        protected uint _m_bankVansCollected;
        protected uint _m_policeCarsCollected;
        protected ExportCars m_carTypesCollected;
        protected uint _m_carTypesCollected2;
        protected uint _m_carTypesCollected3;
        protected uint _m_carTypesCollected4;
        protected uint m_lastTimeHelpMessage;
        protected StoredCar[] m_storedCars;
        protected Garage[] m_garageArray;
        protected byte[] m_unknown;     // padding?

        public Garages()
        {
            m_storedCars = new StoredCar[48];
            m_garageArray = new Garage[32];
            m_unknown = new byte[344];
        }

        public uint NumberOfGarages
        {
            get { return m_numGarages; }
            set { m_numGarages = value; OnPropertyChanged(); }
        }

        public bool BombsAreFree
        {
            get { return m_bombsAreFree; }
            set { m_bombsAreFree = value; OnPropertyChanged(); }
        }

        public bool RespraysAreFree
        {
            get { return m_respraysAreFree; }
            set { m_respraysAreFree = value; OnPropertyChanged(); }
        }

        public ExportCars CarTypesCollected
        {
            get { return m_carTypesCollected; }
            set { m_carTypesCollected = value; OnPropertyChanged(); }
        }

        public uint LastTimeHelpMessage
        {
            get { return m_lastTimeHelpMessage; }
            set { m_lastTimeHelpMessage = value; OnPropertyChanged(); }
        }

        public StoredCar[] StoredCars
        {
            get { return m_storedCars; }
            set { m_storedCars = value; OnPropertyChanged(); }
        }

        public Garage[] GarageArray
        {
            get { return m_garageArray; }
            set { m_garageArray = value; OnPropertyChanged(); }
        }
    }

    public class Garages<T> : Garages
        where T : Garage, new()
    {
        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_numGarages = r.ReadUInt32();
                m_bombsAreFree = r.ReadBoolean32();
                m_respraysAreFree = r.ReadBoolean32();
                _m_carsCollected = r.ReadUInt32();
                _m_bankVansCollected = r.ReadUInt32();
                _m_policeCarsCollected = r.ReadUInt32();
                m_carTypesCollected = (ExportCars) r.ReadUInt32();
                _m_carTypesCollected2 = r.ReadUInt32();
                _m_carTypesCollected3 = r.ReadUInt32();
                _m_carTypesCollected4 = r.ReadUInt32();
                m_lastTimeHelpMessage = r.ReadUInt32();
                for (int i = 0; i < m_storedCars.Length; i++) {
                    m_storedCars[i] = Deserialize<StoredCar>(stream);
                }
                for (int i = 0; i < m_garageArray.Length; i++) {
                    m_garageArray[i] = Deserialize<T>(stream);
                }
                for (int i = 0; i < m_unknown.Length; i++) {
                    m_unknown[i] = r.ReadByte();
                }
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write(m_numGarages);
                w.WriteBoolean32(m_bombsAreFree);
                w.WriteBoolean32(m_respraysAreFree);
                w.Write(_m_carsCollected);
                w.Write(_m_bankVansCollected);
                w.Write(_m_policeCarsCollected);
                w.Write((uint) m_carTypesCollected);
                w.Write(_m_carTypesCollected2);
                w.Write(_m_carTypesCollected3);
                w.Write(_m_carTypesCollected4);
                w.Write(m_lastTimeHelpMessage);
                for (int i = 0; i < m_storedCars.Length; i++) {
                    Serialize(m_storedCars[i], stream);
                }
                for (int i = 0; i < m_garageArray.Length; i++) {
                    Serialize(m_garageArray[i], stream);
                }
                w.Write(m_unknown);
            }

            return stream.Position - start;
        }
    }
}
