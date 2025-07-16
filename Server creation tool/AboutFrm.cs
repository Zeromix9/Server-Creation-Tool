using Server_creation_tool;
using Server_creation_tool.classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server_Creation_Tool
{
    public partial class aboutFrm : Form
    {
        public aboutFrm(MainForm callingFrm1)
        {
            callingFrm = callingFrm1;
            InitializeComponent();
        }
        MainForm callingFrm;
        DataTable controlsLang = new DataTable();
        funcsClass funcs = new funcsClass();
        private void aboutFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
 
        }

        private void baseFormUsrCtrl1_Load(object sender, EventArgs e)
        {

        }

        private void aboutFrm_Load(object sender, EventArgs e)
        {
            richTextBox1.Rtf = Server_creation_tool.Properties.Resources.changelog;
            //set lang
            if (callingFrm.cTry(() =>
            {
                StringReader ctrlReader = new StringReader(funcs.readEmbeddedRes("Server_creation_tool.Language_files." + Server_creation_tool.Properties.Settings.Default.lang + ".controls.AboutFrm.lang") as string);
                controlsLang.ReadXml(ctrlReader);
            }, true, "There was a problem when loading the language files", "Failed to load language files", true, "ERROR READING LANGUAGE FILES (AboutFrm)"))
            {
                callingFrm.setControlsLang(controlsLang, this);
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://steamcommunity.com/id/zeromix/");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.youtube.com/channel/UCA5MGs8_on46jz-e5HZoTPQ");

        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://steamcommunity.com/profiles/76561198351612219");
        }

        private void aboutFrm_TextChanged(object sender, EventArgs e)
        {
            baseFormUsrCtrl1.Title = this.Text;
        }
    }
}
