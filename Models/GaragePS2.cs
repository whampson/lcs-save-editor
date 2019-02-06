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
using LcsSaveEditor.Extensions;
using System.IO;
using System.Text;

namespace LcsSaveEditor.Models
{
    /// <summary>
    /// Garage object for the PlayStation 2 version of the game.
    /// </summary>
    public class GaragePS2 : Garage
    {
        private byte m_unknown03;
        private uint m_unknown04;
        private uint m_unknown08;
        private uint m_unknown0C;
        private uint m_unknown10;
        private uint m_unknown14;
        private uint m_unknown1C;
        private uint m_unknown20;
        private uint m_unknown24;
        private uint m_unknown28;
        private uint m_unknown2C;
        private uint m_unknown58;
        private uint m_unknown5C;
        private uint m_unknown60;
        private uint m_unknown64;
        private uint m_unknown68;
        private uint m_unknown6C;
        private uint m_unknown70;
        private uint m_unknown8C;
        private uint m_unknown90;
        private float m_unknown94;
        private float m_unknown98;
        private uint m_unknownA4;
        private uint m_unknownA8;
        private uint m_unknownB0;
        private uint m_unknownB8;
        private uint m_unknownBC;
        private uint m_unknownC0;
        private uint m_unknownC4;
        private uint m_unknownC8;
        private uint m_unknownCC;
        private uint m_unknownD0;
        private uint m_unknownD4;
        private uint m_unknownD8;
        private uint m_unknownDC;
        private uint m_unknownE0;
        private uint m_unknownE4;
        private uint m_unknownE8;
        private uint m_unknownEC;
        private uint m_unknownF0;
        private uint m_unknownF4;
        private uint m_unknownF8;
        private uint m_unknownFC;

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_type = (GarageType) r.ReadByte();
                m_state = (GarageState) r.ReadByte();
                m_maxCarsAllowed = r.ReadByte();
                m_unknown03 = r.ReadByte();
                m_unknown04 = r.ReadUInt32();
                m_unknown08 = r.ReadUInt32();
                m_unknown0C = r.ReadUInt32();
                m_unknown10 = r.ReadUInt32();
                m_unknown14 = r.ReadUInt32();
                m_doorObjectPointer = r.ReadUInt32();
                m_unknown1C = r.ReadUInt32();
                m_unknown20 = r.ReadUInt32();
                m_unknown24 = r.ReadUInt32();
                m_unknown28 = r.ReadUInt32();
                m_unknown2C = r.ReadUInt32();
                m_rotatingDoor = r.ReadBoolean32();
                m_specialCamera = r.ReadBoolean32();
                m_location = Deserialize<Vector3d>(stream);
                m_rotation = Deserialize<Quaternion>(stream);
                m_ceilingZ = r.ReadSingle();
                m_unknown58 = r.ReadUInt32();
                m_unknown5C = r.ReadUInt32();
                m_unknown60 = r.ReadUInt32();
                m_unknown64 = r.ReadUInt32();
                m_unknown68 = r.ReadUInt32();
                m_unknown6C = r.ReadUInt32();
                m_unknown70 = r.ReadUInt32();
                m_doorCurrentHeight = r.ReadSingle();
                m_doorMaxHeight = r.ReadSingle();
                m_x1 = r.ReadSingle();
                m_x2 = r.ReadSingle();
                m_y1 = r.ReadSingle();
                m_y2 = r.ReadSingle();
                m_unknown8C = r.ReadUInt32();
                m_unknown90 = r.ReadUInt32();
                m_unknown94 = r.ReadSingle();
                m_unknown98 = r.ReadSingle();
                m_doorX = r.ReadSingle();
                m_doorY = r.ReadSingle();
                m_unknownA4 = r.ReadUInt32();
                m_unknownA8 = r.ReadUInt32();
                m_doorZ = r.ReadSingle();
                m_unknownB0 = r.ReadUInt32();
                m_timer = r.ReadUInt32();
                m_unknownB8 = r.ReadUInt32();
                m_unknownBC = r.ReadUInt32();
                m_unknownC0 = r.ReadUInt32();
                m_unknownC4 = r.ReadUInt32();
                m_unknownC8 = r.ReadUInt32();
                m_unknownCC = r.ReadUInt32();
                m_unknownD0 = r.ReadUInt32();
                m_unknownD4 = r.ReadUInt32();
                m_unknownD8 = r.ReadUInt32();
                m_unknownDC = r.ReadUInt32();
                m_unknownE0 = r.ReadUInt32();
                m_unknownE4 = r.ReadUInt32();
                m_unknownE8 = r.ReadUInt32();
                m_unknownEC = r.ReadUInt32();
                m_unknownF0 = r.ReadUInt32();
                m_unknownF4 = r.ReadUInt32();
                m_unknownF8 = r.ReadUInt32();
                m_unknownFC = r.ReadUInt32();
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write((byte) m_type);
                w.Write((byte) m_state);
                w.Write(m_maxCarsAllowed);
                w.Write(m_unknown03);
                w.Write(m_unknown04);
                w.Write(m_unknown08);
                w.Write(m_unknown0C);
                w.Write(m_unknown10);
                w.Write(m_unknown14);
                w.Write(m_doorObjectPointer);
                w.Write(m_unknown1C);
                w.Write(m_unknown20);
                w.Write(m_unknown24);
                w.Write(m_unknown28);
                w.Write(m_unknown2C);
                w.WriteBoolean32(m_rotatingDoor);
                w.WriteBoolean32(m_specialCamera);
                Serialize(m_location, stream);
                Serialize(m_rotation, stream);
                w.Write(m_ceilingZ);
                w.Write(m_unknown58);
                w.Write(m_unknown5C);
                w.Write(m_unknown60);
                w.Write(m_unknown64);
                w.Write(m_unknown68);
                w.Write(m_unknown6C);
                w.Write(m_unknown70);
                w.Write(m_doorCurrentHeight);
                w.Write(m_doorMaxHeight);
                w.Write(m_x1);
                w.Write(m_x2);
                w.Write(m_y1);
                w.Write(m_y2);
                w.Write(m_unknown8C);
                w.Write(m_unknown90);
                w.Write(m_unknown94);
                w.Write(m_unknown98);
                w.Write(m_doorX);
                w.Write(m_doorY);
                w.Write(m_unknownA4);
                w.Write(m_unknownA8);
                w.Write(m_doorZ);
                w.Write(m_unknownB0);
                w.Write(m_timer);
                w.Write(m_unknownB8);
                w.Write(m_unknownBC);
                w.Write(m_unknownC0);
                w.Write(m_unknownC4);
                w.Write(m_unknownC8);
                w.Write(m_unknownCC);
                w.Write(m_unknownD0);
                w.Write(m_unknownD4);
                w.Write(m_unknownD8);
                w.Write(m_unknownDC);
                w.Write(m_unknownE0);
                w.Write(m_unknownE4);
                w.Write(m_unknownE8);
                w.Write(m_unknownEC);
                w.Write(m_unknownF0);
                w.Write(m_unknownF4);
                w.Write(m_unknownF8);
                w.Write(m_unknownFC);
            }

            return stream.Position - start;
        }
    }
}
