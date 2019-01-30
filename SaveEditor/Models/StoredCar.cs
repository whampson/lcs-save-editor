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

using System.IO;
using System.Text;
using WHampson.LcsSaveEditor.Helpers;
using WHampson.LcsSaveEditor.Models.GameDataTypes;

namespace WHampson.LcsSaveEditor.Models
{
    public class StoredCar : SerializableObject
    {
        private uint m_modelId;
        private Vector3d m_location;
        private Vector2d m_rotation;
        private float m_pitch;
        private float m_handlingMultiplier;
        private uint m_attributes;
        private byte m_color1;
        private byte m_color2;
        private byte m_radioStation;
        private sbyte m_extra1;
        private sbyte m_extra2;

        public StoredCar()
        {
            m_location = new Vector3d();
            m_rotation = new Vector2d();
        }

        public uint ModelId
        {
            get { return m_modelId; }
            set { m_modelId = value; OnPropertyChanged(); }
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

        public float HandlingMultiplier
        {
            get { return m_handlingMultiplier; }
            set { m_handlingMultiplier = value; OnPropertyChanged(); }
        }

        public StoredCarProperties Attributes
        {
            get { return (StoredCarProperties) m_attributes; }
            set { m_attributes = (uint) value; }
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
            get { return (RadioStation) m_radioStation; }
            set { m_radioStation = (byte) value; }
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
                m_modelId = r.ReadUInt32();
                Deserialize(stream, out m_location);
                Deserialize(stream, out m_rotation);
                m_pitch = r.ReadSingle();
                m_handlingMultiplier = r.ReadSingle();
                m_attributes = r.ReadUInt32();
                m_color1 = r.ReadByte();
                m_color2 = r.ReadByte();
                m_radioStation = r.ReadByte();
                m_extra1 = r.ReadSByte();
                m_extra2 = r.ReadSByte();
                r.ReadByte();       // Align byte
                r.ReadByte();       // Align byte
                r.ReadByte();       // Align byte
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write(m_modelId);
                Serialize(m_location, stream);
                Serialize(m_rotation, stream);
                w.Write(m_pitch);
                w.Write(m_handlingMultiplier);
                w.Write(m_attributes);
                w.Write(m_color1);
                w.Write(m_color2);
                w.Write(m_radioStation);
                w.Write(m_extra1);
                w.Write(m_extra2);
                w.Write((byte) 0);      // Align byte
                w.Write((byte) 0);      // Align byte
                w.Write((byte) 0);      // Align byte
            }

            return stream.Position - start;
        }
    }
}
