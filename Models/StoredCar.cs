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

using LcsSaveEditor.DataTypes;
using LcsSaveEditor.Infrastructure;
using System.IO;
using System.Text;

namespace LcsSaveEditor.Models
{
    /// <summary>
    /// Represents a car stored in one of the safehouse garages.
    /// </summary>
    public class StoredCar : SerializableObject
    {
        private Vehicle m_vehicle;
        private Vector3d m_location;
        private Vector2d m_rotation;
        private float m_pitch;
        private float m_handlingMultiplier;
        private StoredCarPerks m_perks;
        private byte m_color1;
        private byte m_color2;
        private RadioStation m_radioStation;
        private sbyte m_extra1;
        private sbyte m_extra2;

        public Vehicle Vehicle
        {
            get { return m_vehicle; }
            set { m_vehicle = value; OnPropertyChanged(); }
        }

        public Vector3d Location
        {
            get { return m_location; }
            set { m_location = value; OnPropertyChanged(); }
        }

        public Vector2d Rotation
        {
            get { return m_rotation; }
            set { m_rotation = value; OnPropertyChanged(); }
        }

        public float Pitch
        {
            get { return m_pitch; }
            set { m_pitch = value; OnPropertyChanged(); }
        }

        public float HandlingMultplier
        {
            get { return m_handlingMultiplier; }
            set { m_handlingMultiplier = value; OnPropertyChanged(); }
        }

        public StoredCarPerks Perks
        {
            get { return m_perks; }
            set { m_perks = value; OnPropertyChanged(); }
        }

        public byte Color1
        {
            get { return m_color1; }
            set { m_color1 = value; OnPropertyChanged(); }
        }

        public byte Color2
        {
            get { return m_color2; }
            set { m_color2 = value; OnPropertyChanged(); }
        }

        public RadioStation RadioStation
        {
            get { return m_radioStation; }
            set { m_radioStation = value; OnPropertyChanged(); }
        }

        public sbyte Extra1
        {
            get { return m_extra1; }
            set { m_extra1 = value; OnPropertyChanged(); }
        }

        public sbyte Extra2
        {
            get { return m_extra2; }
            set { m_extra2 = value; OnPropertyChanged(); }
        }

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_vehicle = (Vehicle) r.ReadUInt32();
                m_location = Deserialize<Vector3d>(stream);
                m_rotation = Deserialize<Vector2d>(stream);
                m_pitch = r.ReadSingle();
                m_handlingMultiplier = r.ReadSingle();
                m_perks = (StoredCarPerks) r.ReadUInt32();
                m_color1 = r.ReadByte();
                m_color2 = r.ReadByte();
                m_radioStation = (RadioStation) r.ReadByte();
                m_extra1 = r.ReadSByte();
                m_extra2 = r.ReadSByte();
                r.ReadBytes(3);     // align bytes
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write((uint) m_vehicle);
                Serialize(m_location, stream);
                Serialize(m_rotation, stream);
                w.Write(m_pitch);
                w.Write(m_handlingMultiplier);
                w.Write((uint) m_perks);
                w.Write(m_color1);
                w.Write(m_color2);
                w.Write((byte) m_radioStation);
                w.Write(m_extra1);
                w.Write(m_extra2);
                w.Write(new byte[3]);       // align bytes
            }

            return stream.Position - start;
        }

        public override string ToString()
        {
            return string.Format("{0} = {1}, {2} = {3}",
                nameof(Vehicle), Vehicle,
                nameof(Perks), Perks);
        }
    }
}
