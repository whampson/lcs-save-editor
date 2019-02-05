using LcsSaveEditor.Infrastructure;
using System.IO;
using System.Text;

namespace LcsSaveEditor.Models
{
    /// <summary>
    /// Represents a quaternion.
    /// </summary>
    public class Quaternion : SerializableObject
    {
        private float m_x;
        private float m_y;
        private float m_z;
        private float m_w;

        public float X
        {
            get { return m_x; }
            set { m_x = value; OnPropertyChanged(); }
        }

        public float Y
        {
            get { return m_y; }
            set { m_y = value; OnPropertyChanged(); }
        }

        public float Z
        {
            get { return m_z; }
            set { m_z = value; OnPropertyChanged(); }
        }

        public float W
        {
            get { return m_w; }
            set { m_w = value; OnPropertyChanged(); }
        }

        protected override long DeserializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryReader r = new BinaryReader(stream, Encoding.Default, true)) {
                m_x = r.ReadSingle();
                m_y = r.ReadSingle();
                m_z = r.ReadSingle();
                m_w = r.ReadSingle();
            }

            return stream.Position - start;
        }

        protected override long SerializeObject(Stream stream)
        {
            long start = stream.Position;
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.Default, true)) {
                w.Write(m_x);
                w.Write(m_y);
                w.Write(m_z);
                w.Write(m_w);
            }

            return stream.Position - start;
        }

        public override string ToString()
        {
            return string.Format("<{0},{1},{2},{3}>", m_x, m_y, m_z, m_w);
        }
    }
}
