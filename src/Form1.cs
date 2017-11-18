using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WHampson.LcsSaveEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            tableLayoutPanel1.RowCount = 0;
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.AutoScroll = true;
            tableLayoutPanel1.AutoScrollMinSize = tableLayoutPanel1.Size;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
