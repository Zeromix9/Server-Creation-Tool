using Server_creation_tool.classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server_creation_tool
{
    public partial class FAQFrm : Form
    {
        public FAQFrm(MainForm callingFrm1)
        {
            callingFrm = callingFrm1;
            InitializeComponent();
        }
        MainForm callingFrm;
        funcsClass funcs = new funcsClass();
        DataTable controlsLang = new DataTable();
        private void FAQFrm_Load(object sender, EventArgs e)
        {
            //set lang
            if (callingFrm.cTry(() =>
            {
                StringReader ctrlReader = new StringReader(funcs.readEmbeddedRes("Server_creation_tool.Language_files." + Properties.Settings.Default.lang + ".controls.FAQFrm.lang") as string);
                controlsLang.ReadXml(ctrlReader);
                //Debug.WriteLine(controlsLang.Rows[2][1].ToString());
            }, true, "There was a problem when loading the language files", "Failed to load language files", true, "ERROR READING LANGUAGE FILES (FAQ)"))
            {
                callingFrm.setControlsLang(controlsLang, this);
            }
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void baseFormUsrCtrl1_Load(object sender, EventArgs e)
        {

        }

        private void bodyLbl_Click(object sender, EventArgs e)
        {

        }

        private void OpenFoldBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://portforward.com/router.htm");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/79eKCs5fW8");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/groups/ServerTool");
        }

        private void FAQFrm_TextChanged(object sender, EventArgs e)
        {
            baseFormUsrCtrl1.Title = this.Text;
        }


    }
}
