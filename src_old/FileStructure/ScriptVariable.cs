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

using WHampson.Cascara;

namespace WHampson.LcsSaveEditor.FileStructure
{
    public class ScriptVariable
    {
        private readonly Primitive<int> valueAsInt;
        private readonly Primitive<float> valueAsFloat;
        private readonly Primitive<Bool32> valueAsBool;

        public ScriptVariable()
        {
            valueAsInt = new Primitive<int>(null, 0);
            valueAsFloat = new Primitive<float>(null, 0);
            valueAsBool = new Primitive<Bool32>(null, 0);
        }

        public int ValueAsInt
        {
            get { return valueAsInt.Value; }
            set { valueAsInt.Value = value; }
        }

        public float ValueAsFloat
        {
            get { return valueAsFloat.Value; }
            set { valueAsFloat.Value = value; }
        }

        public Bool32 ValueAsBool
        {
            get { return valueAsBool.Value; }
            set { valueAsBool.Value = value; }
        }

        public static implicit operator int(ScriptVariable v)
        {
            return v.ValueAsInt;
        }

        public static implicit operator float(ScriptVariable v)
        {
            return v.ValueAsFloat;
        }

        public static implicit operator bool(ScriptVariable v)
        {
            return v.ValueAsBool;
        }
    }
}
