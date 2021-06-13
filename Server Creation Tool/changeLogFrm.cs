using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server_Creation_Tool
{
    public partial class changelogFrm : Form
    {
        public changelogFrm()
        {
            InitializeComponent();
        }

        private void changelogFrm_Load(object sender, EventArgs e)
        {
            
        }

        private void changelogFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }
    }
}
