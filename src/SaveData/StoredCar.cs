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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHampson.Cascara;

namespace WHampson.LcsSaveEditor.SaveData
{
    internal class StoredCar
    {
        private readonly Primitive<uint> modelId;
        private readonly Vector3d location;
        private readonly Vector2d rotation;
        private readonly Primitive<float> pitch;
        private readonly Primitive<float> handlingMultiplier;
        private readonly Primitive<uint> attributes;
        private readonly Primitive<byte> color1;
        private readonly Primitive<byte> color2;
        private readonly Primitive<byte> radioStation;
        private readonly Primitive<sbyte> extra1;
        private readonly Primitive<sbyte> extra2;

        public StoredCar()
        {
            modelId = new Primitive<uint>(null, 0);
            location = new Vector3d();
            rotation = new Vector2d();
            pitch = new Primitive<float>(null, 0);
            handlingMultiplier = new Primitive<float>(null, 0);
            attributes = new Primitive<uint>(null, 0);
            color1 = new Primitive<byte>(null, 0);
            color2 = new Primitive<byte>(null, 0);
            radioStation = new Primitive<byte>(null, 0);
            extra1 = new Primitive<sbyte>(null, 0);
            extra2 = new Primitive<sbyte>(null, 0);
        }

        public uint ModelId
        {
            get { return modelId.Value; }
            set { modelId.Value = value; }
        }

        public Vector3d Location
        {
            get { return location; }
        }

        public Vector2d Rotation
        {
            get { return rotation; }
        }

        public float Pitch
        {
            get { return pitch.Value; }
            set { pitch.Value = value; }
        }

        public float HandlingMultiplier
        {
            get { return handlingMultiplier.Value; }
            set { handlingMultiplier.Value = value; }
        }

        public uint Attributes  // TODO: enum
        {
            get { return attributes.Value; }
            set { attributes.Value = value; }
        }

        public byte Color1
        {
            get { return color1.Value; }
            set { color1.Value = value; }
        }

        public byte Color2
        {
            get { return color2.Value; }
            set { color2.Value = value; }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public RadioStation RadioStation
        {
            get { return (RadioStation) radioStation.Value; }
            set { radioStation.Value = (byte) value; }
        }

        public sbyte Extra1
        {
            get { return extra1.Value; }
            set { extra1.Value = value; }
        }

        public sbyte Extra2
        {
            get { return extra2.Value; }
            set { extra2.Value = value; }
        }
    }
}
