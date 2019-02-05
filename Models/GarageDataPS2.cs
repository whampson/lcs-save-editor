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

namespace LcsSaveEditor.Models
{
    public class GarageDataPS2 : GarageData
    {
        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            base.DeserializeObject(stream);

            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                for (int i = 0; i < m_garages.Length; i++) {
                    m_garages[i] = Deserialize<GaragePS2>(stream);
                }
                for (int i = 0; i < m_unknown.Length; i++) {
                    m_unknown[i] = r.ReadByte();
                }
            }
            

            return stream.Position - start;
        }
    }
}
