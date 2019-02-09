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
using System.IO;
using System.Text;

namespace LcsSaveEditor.Models
{
    /// <summary>
    /// Garage object for the Android and iOS versions of the game.
    /// </summary>
    public class GarageAndroidIOS : Garage
    {
        private uint m_unknown04;
        private uint m_unknown08;
        private uint m_unknown0C;
        private uint m_unknown10;
        private uint m_unknown14;
        private uint m_unknown18;
        private uint m_unknown1C;
        private uint m_unknown20;
        private uint m_unknown24;
        private uint m_unknown28;
        private uint m_unknown2C;
        private uint m_unknown30;
        private uint m_unknown34;
        private uint m_unknown38;
        private uint m_unknown3C;
        private uint m_unknown40;
        private uint m_unknown44;
        private uint m_unknown48;
        private uint m_unknown4C;
        private uint m_unknown50;
        private uint m_unknown54;
        private uint m_unknown58;
        private float m_unknown5C;
        private uint m_unknown60;
        private uint m_unknown84;
        private uint m_unknownA0;
        private float m_unknownA4;
        private float m_unknownA8;
        private float m_unknownB4;
        private float m_unknownB8;
        private float m_unknownC0;
        private byte m_unknownCB;
        private byte m_unknownCC;
        private byte m_unknownCD;
        private byte m_unknownCE;
        private byte m_unknownCF;
        private byte m_unknownD0;
        private byte m_unknownD1;
        private byte m_unknownD2;
        private byte m_unknownD3;
        private byte m_unknownD6;
        private byte m_unknownD7;
        private uint m_unknownD8;
        private uint m_unknownDC;

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_doorObjectPointer = r.ReadUInt32();
                m_unknown04 = r.ReadUInt32();
                m_unknown08 = r.ReadUInt32();
                m_unknown0C = r.ReadUInt32();
                m_unknown10 = r.ReadUInt32();
                m_unknown14 = r.ReadUInt32();
                m_unknown18 = r.ReadUInt32();
                m_unknown1C = r.ReadUInt32();
                m_unknown20 = r.ReadUInt32();
                m_unknown24 = r.ReadUInt32();
                m_unknown28 = r.ReadUInt32();
                m_unknown2C = r.ReadUInt32();
                m_unknown30 = r.ReadUInt32();
                m_unknown34 = r.ReadUInt32();
                m_unknown38 = r.ReadUInt32();
                m_unknown3C = r.ReadUInt32();
                m_unknown40 = r.ReadUInt32();
                m_unknown44 = r.ReadUInt32();
                m_unknown48 = r.ReadUInt32();
                m_unknown4C = r.ReadUInt32();
                m_unknown50 = r.ReadUInt32();
                m_unknown54 = r.ReadUInt32();
                m_unknown58 = r.ReadUInt32();
                m_unknown5C = r.ReadSingle();
                m_unknown60 = r.ReadUInt32();
                Location = Deserialize<Vector3d>(stream);
                Rotation = Deserialize<Quaternion>(stream);
                m_ceilingZ = r.ReadSingle();
                m_unknown84 = r.ReadUInt32();
                m_doorCurrentHeight = r.ReadSingle();
                m_doorMaxHeight = r.ReadSingle();
                m_x1 = r.ReadSingle();
                m_x2 = r.ReadSingle();
                m_y1 = r.ReadSingle();
                m_y2 = r.ReadSingle();
                m_unknownA0 = r.ReadUInt32();
                m_unknownA4 = r.ReadSingle();
                m_unknownA8 = r.ReadSingle();
                m_doorX = r.ReadSingle();
                m_doorY = r.ReadSingle();
                m_unknownB4 = r.ReadSingle();
                m_unknownB8 = r.ReadSingle();
                m_doorZ = r.ReadSingle();
                m_unknownC0 = r.ReadSingle();
                m_timer = r.ReadUInt32();
                m_type = (GarageType) r.ReadByte();
                m_state = (GarageState) r.ReadByte();
                m_maxCarsAllowed = r.ReadByte();
                m_unknownCB = r.ReadByte();
                m_unknownCC = r.ReadByte();
                m_unknownCD = r.ReadByte();
                m_unknownCE = r.ReadByte();
                m_unknownCF = r.ReadByte();
                m_unknownD0 = r.ReadByte();
                m_unknownD1 = r.ReadByte();
                m_unknownD2 = r.ReadByte();
                m_unknownD3 = r.ReadByte();
                m_rotatingDoor = r.ReadBoolean();
                m_specialCamera = r.ReadBoolean();
                m_unknownD6 = r.ReadByte();
                m_unknownD7 = r.ReadByte();
                m_unknownD8 = r.ReadUInt32();
                m_unknownDC = r.ReadUInt32();
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write(m_doorObjectPointer);
                w.Write(m_unknown04);
                w.Write(m_unknown08);
                w.Write(m_unknown0C);
                w.Write(m_unknown10);
                w.Write(m_unknown14);
                w.Write(m_unknown18);
                w.Write(m_unknown1C);
                w.Write(m_unknown20);
                w.Write(m_unknown24);
                w.Write(m_unknown28);
                w.Write(m_unknown2C);
                w.Write(m_unknown30);
                w.Write(m_unknown34);
                w.Write(m_unknown38);
                w.Write(m_unknown3C);
                w.Write(m_unknown40);
                w.Write(m_unknown44);
                w.Write(m_unknown48);
                w.Write(m_unknown4C);
                w.Write(m_unknown50);
                w.Write(m_unknown54);
                w.Write(m_unknown58);
                w.Write(m_unknown5C);
                w.Write(m_unknown60);
                Serialize(m_location, stream);
                Serialize(m_rotation, stream);
                w.Write(m_ceilingZ);
                w.Write(m_unknown84);
                w.Write(m_doorCurrentHeight);
                w.Write(m_doorMaxHeight);
                w.Write(m_x1);
                w.Write(m_x2);
                w.Write(m_y1);
                w.Write(m_y2);
                w.Write(m_unknownA0);
                w.Write(m_unknownA4);
                w.Write(m_unknownA8);
                w.Write(m_doorX);
                w.Write(m_doorY);
                w.Write(m_unknownB4);
                w.Write(m_unknownB8);
                w.Write(m_doorZ);
                w.Write(m_unknownC0);
                w.Write(m_timer);
                w.Write((byte) m_type);
                w.Write((byte) m_state);
                w.Write(m_maxCarsAllowed);
                w.Write(m_unknownCB);
                w.Write(m_unknownCC);
                w.Write(m_unknownCD);
                w.Write(m_unknownCE);
                w.Write(m_unknownCF);
                w.Write(m_unknownD0);
                w.Write(m_unknownD1);
                w.Write(m_unknownD2);
                w.Write(m_unknownD3);
                w.Write(m_rotatingDoor);
                w.Write(m_specialCamera);
                w.Write(m_unknownD6);
                w.Write(m_unknownD7);
                w.Write(m_unknownD8);
                w.Write(m_unknownDC);
            }

            return stream.Position - start;
        }
    }
}
