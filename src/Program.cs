using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WHampson.Cascara;

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

            MessageBox.Show(writer.ToString());
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
