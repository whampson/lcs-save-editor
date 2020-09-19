using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace LCSSaveEditor.Core
{
    public class Carcols
    {
        public static Carcols TheCarcols { get; }

        static Carcols()
        {
            TheCarcols = new Carcols();
        }

        public Carcols()
        {
            Colors = new List<Color>();
        }

        public List<Color> Colors { get; }

        public void Load(string path)
        {
            Load(File.ReadAllBytes(path));
        }

        public void Load(byte[] data)
        {
            using (MemoryStream m = new MemoryStream(data))
            {
                using (StreamReader buf = new StreamReader(m))
                {
                    Log.Info("Loading carcols...");

                    string line;
                    while (buf.ReadLine().Trim() != "col") { }

                    int count = 0;
                    while ((line = buf.ReadLine().Trim()) != "end")
                    {
                        if (string.IsNullOrEmpty(line)) continue;

                        string[] rgb = line.Split(' ', '\t', ',');
                        int.TryParse(rgb[0], out int r);
                        int.TryParse(rgb[1], out int g);
                        int.TryParse(rgb[2], out int b);

                        Colors.Add(Color.FromArgb(r, g, b));
                        count++;
                    }

                    Log.Info($"Loaded {count} car colors.");
                }
            }
        }
    }
}
