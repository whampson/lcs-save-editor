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
    /// Garage object for the PlayStation Portable version of the game.
    /// </summary>
    public class GaragePSP : Garage
    {
        private byte m_unknown03;
        private uint m_unknown04;
        private uint m_unknown08;
        private uint m_unknown10;
        private byte m_unknown14;
        private byte m_unknown15;
        private byte m_unknown16;
        private byte m_unknown17;
        private byte m_unknown18;
        private byte m_unknown1B;
        private uint m_unknown3C;
        private uint m_unknown40;
        private uint m_unknown44;
        private uint m_unknown48;
        private uint m_unknown4C;
        private uint m_unknown50;
        private uint m_unknown6C;
        private uint m_unknown70;
        private float m_unknown74;
        private float m_unknown78;
        private uint m_unknown84;
        private uint m_unknown88;
        private uint m_unknown90;
        private uint m_unknown98;
        private uint m_unknown9C;
        private uint m_unknownA0;
        private uint m_unknownA4;
        private uint m_unknownA8;
        private uint m_unknownAC;
        private uint m_unknownB0;
        private uint m_unknownB4;
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
                m_doorObjectPointer = r.ReadUInt32();
                m_unknown10 = r.ReadUInt32();
                m_unknown14 = r.ReadByte();
                m_unknown15 = r.ReadByte();
                m_unknown16 = r.ReadByte();
                m_unknown17 = r.ReadByte();
                m_unknown18 = r.ReadByte();
                m_rotatingDoor = r.ReadBoolean();
                m_specialCamera = r.ReadBoolean();
                m_unknown1B = r.ReadByte();
                m_location = Deserialize<Vector3d>(stream);
                m_rotation = Deserialize<Quaternion>(stream);
                m_ceilingZ = r.ReadSingle();
                m_unknown3C = r.ReadUInt32();
                m_unknown40 = r.ReadUInt32();
                m_unknown44 = r.ReadUInt32();
                m_unknown48 = r.ReadUInt32();
                m_unknown4C = r.ReadUInt32();
                m_unknown50 = r.ReadUInt32();
                m_doorCurrentHeight = r.ReadSingle();
                m_doorMaxHeight = r.ReadSingle();
                m_x1 = r.ReadSingle();
                m_x2 = r.ReadSingle();
                m_y1 = r.ReadSingle();
                m_y2 = r.ReadSingle();
                m_unknown6C = r.ReadUInt32();
                m_unknown70 = r.ReadUInt32();
                m_unknown74 = r.ReadSingle();
                m_unknown78 = r.ReadSingle();
                m_doorX = r.ReadSingle();
                m_doorY = r.ReadSingle();
                m_unknown84 = r.ReadUInt32();
                m_unknown88 = r.ReadUInt32();
                m_doorZ = r.ReadSingle();
                m_unknown90 = r.ReadUInt32();
                m_timer = r.ReadUInt32();
                m_unknown98 = r.ReadUInt32();
                m_unknown9C = r.ReadUInt32();
                m_unknownA0 = r.ReadUInt32();
                m_unknownA4 = r.ReadUInt32();
                m_unknownA8 = r.ReadUInt32();
                m_unknownAC = r.ReadUInt32();
                m_unknownB0 = r.ReadUInt32();
                m_unknownB4 = r.ReadUInt32();
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
                w.Write(m_doorObjectPointer);
                w.Write(m_unknown10);
                w.Write(m_unknown14);
                w.Write(m_unknown15);
                w.Write(m_unknown16);
                w.Write(m_unknown17);
                w.Write(m_unknown18);
                w.Write(m_rotatingDoor);
                w.Write(m_specialCamera);
                w.Write(m_unknown1B);
                Serialize(m_location, stream);
                Serialize(m_rotation, stream);
                w.Write(m_ceilingZ);
                w.Write(m_unknown3C);
                w.Write(m_unknown40);
                w.Write(m_unknown44);
                w.Write(m_unknown48);
                w.Write(m_unknown4C);
                w.Write(m_unknown50);
                w.Write(m_doorCurrentHeight);
                w.Write(m_doorMaxHeight);
                w.Write(m_x1);
                w.Write(m_x2);
                w.Write(m_y1);
                w.Write(m_y2);
                w.Write(m_unknown6C);
                w.Write(m_unknown70);
                w.Write(m_unknown74);
                w.Write(m_unknown78);
                w.Write(m_doorX);
                w.Write(m_doorY);
                w.Write(m_unknown84);
                w.Write(m_unknown88);
                w.Write(m_doorZ);
                w.Write(m_unknown90);
                w.Write(m_timer);
                w.Write(m_unknown98);
                w.Write(m_unknown9C);
                w.Write(m_unknownA0);
                w.Write(m_unknownA4);
                w.Write(m_unknownA8);
                w.Write(m_unknownAC);
                w.Write(m_unknownB0);
                w.Write(m_unknownB4);
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
            }

            return stream.Position - start;
        }
    }
}
