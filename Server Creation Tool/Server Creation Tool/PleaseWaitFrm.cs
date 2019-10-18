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
    public partial class PleaseWaitFrm : Form
    {
        public PleaseWaitFrm()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (label1.Text == "Please wait...")
            {
                label1.Text = "Please wait";

              
            }
            else
            {
                label1.Text = label1.Text + ".";
            }
        }
    }
}
