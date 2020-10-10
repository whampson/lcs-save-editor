using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

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
            ColorInfo = new List<(Color, string)>();
        }

        public List<(Color, string)> ColorInfo { get; }

        public List<Color> Colors => ColorInfo.Select(x => x.Item1).ToList();
        public List<string> Names => ColorInfo.Select(x => x.Item2).ToList();


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

                    // Line format:
                    // r,g,b\t# index name\t class

                    string line;
                    while (buf.ReadLine().Trim() != "col") { }

                    int count = 0;
                    while ((line = buf.ReadLine().Trim()) != "end")
                    {
                        if (string.IsNullOrEmpty(line)) continue;

                        string[] colorComment = line.Split('#');
                        string[] rgb = colorComment[0].Split(',');
                        int.TryParse(rgb[0], out int r);
                        int.TryParse(rgb[1], out int g);
                        int.TryParse(rgb[2], out int b);
                        Color color = Color.FromArgb(r, g, b);
                        string name = "";

                        if (colorComment.Length > 1)
                        {
                            string[] indexNameClass = colorComment[1].Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            string[] indexName = indexNameClass[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            name = indexName.Skip(1).Aggregate((x, y) => x + " " + y);
                            name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);
                        }

                        ColorInfo.Add((color, name));
                        count++;
                    }

                    Log.Info($"Loaded {count} car colors.");
                }
            }
        }
    }
}
