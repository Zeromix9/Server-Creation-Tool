using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Server_Creation_Tool
{


    public partial class AboutFrm : Form
    {
        Form MainForm11 = null;
        public AboutFrm()
        {
            InitializeComponent();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/id/zeromix");
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.youtube.com/channel/UCA5MGs8_on46jz-e5HZoTPQ");
        }

        private void AboutFrm_Load(object sender, EventArgs e)
        {

        }
        private void AboutFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
         //   var mainform = new MainForm();
            e.Cancel = true;
            this.Hide();
           
            
        }

    }
}
