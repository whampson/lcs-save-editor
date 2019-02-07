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
    public class FavoriteRadioStationListPS2PSP : FavoriteRadioStationList
    {
        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_headRadioListenTime = r.ReadSingle();
                m_doubleClefFMListenTime = r.ReadSingle();
                m_kJahListenTime = r.ReadSingle();
                m_riseFMListenTime = r.ReadSingle();
                m_lips106ListenTime = r.ReadSingle();
                m_radioDelMundoListenTime = r.ReadSingle();
                m_msx98ListenTime = r.ReadSingle();
                m_flashbackFMListenTime = r.ReadSingle();
                m_theLibertyJamListenTime = r.ReadSingle();
                m_lcfrListenTime = r.ReadSingle();
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write(m_headRadioListenTime);
                w.Write(m_doubleClefFMListenTime);
                w.Write(m_kJahListenTime);
                w.Write(m_riseFMListenTime);
                w.Write(m_lips106ListenTime);
                w.Write(m_radioDelMundoListenTime);
                w.Write(m_msx98ListenTime);
                w.Write(m_flashbackFMListenTime);
                w.Write(m_theLibertyJamListenTime);
                w.Write(m_lcfrListenTime);
            }

            return stream.Position - start;
        }
    }
}