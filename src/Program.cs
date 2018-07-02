﻿#region License
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
using System;
using System.IO;
using System.Windows.Forms;
using WHampson.Cascara;
using WHampson.LcsSaveEditor.SaveData;

namespace WHampson.LcsSaveEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new Form1());

            string scriptPath = "../../resources/scripts/ps2save.xml";
            LayoutScript script = LayoutScript.Load(scriptPath);

            string savePath = "../../test/data/ps2/1 The Sicilian Gambit";
            BinaryData data = BinaryData.Load(savePath);

            TextWriter writer = new StringWriter();
            data.RunLayoutScript(script, writer);

            DeserializationFlags flags = 0;
            flags |= DeserializationFlags.Fields;
            flags |= DeserializationFlags.NonPublic;
            SaveGame sg = data.Deserialize<SaveGame>(flags);

            MessageBox.Show(writer.ToString().Replace('\0', ' '));
            object outObj = sg;
            File.WriteAllText("out.json", outObj.ToString());

        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }

    class SaveGame
    {
        private SimpleVarsBlock simpleVars;
        private ScriptsBlock scripts;
        private GaragesBlock garages;
        private PlayerBlock player;
        private StatsBlock stats;
        private Primitive<uint> checksum;

        public SaveGame()
        {
            simpleVars = new SimpleVarsBlock();
            scripts = new ScriptsBlock();
            garages = new GaragesBlock();
            player = new PlayerBlock();
            stats = new StatsBlock();
            checksum = new Primitive<uint>(null, 0);
        }

        public SimpleVarsBlock SimpleVars
        {
            get { return simpleVars; }
        }

        public ScriptsBlock Scripts
        {
            get { return scripts; }
        }

        public GaragesBlock Garages
        {
            get { return garages; }
        }

        public PlayerBlock Player
        {
            get { return player; }
        }

        public StatsBlock Stats
        {
            get { return stats; }
        }

        public uint Checksum
        {
            get { return checksum.Value; }
            set { checksum.Value = value; }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
