using Server_creation_tool.classes;
using Server_Creation_Tool;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Server_creation_tool
{
    public partial class startSrvForm : Form
    {
        public startSrvForm(MainForm _mainForm, bool autoSetup = false)
        {
            InitializeComponent();
            mainFrm = _mainForm;
            autosetup = autoSetup;
        }
        MainForm mainFrm;
        bool autosetup;
        public Process returnProc = null;
        private void startSrvForm_Load(object sender, EventArgs e)
        {
            
            if (autosetup)
            {
                autoAddBtns();
            }
        }
        int combinedBtnHeight = 0;
        public void addBtn(string text, string startFilePath, Action startAction = null)
        {
            customSmoothBtn newBtn = dummycustomSmoothBtn1.CloneCtrl();
            newBtn.Visible = true;
            newBtn.Text = text;
            if (startFilePath == "" && startAction != null)
            {
                newBtn.Click += (_, _2) => { startAction(); this.DialogResult = DialogResult.OK;  };
            }
            else
            {
                newBtn.Click += (_, _2) => { returnProc = prepareProcess(startFilePath); this.DialogResult = DialogResult.OK; };
            }
            combinedBtnHeight += newBtn.Height + 8;
            scrollPnl.EditablePanel.Controls.Add(newBtn);
            scrollPnl.resizeScrollPnl(combinedBtnHeight);
        }
        private void autoAddBtns()
        {
            int i = 0;
            while (true)
            {
                string ending = "";
                if (i != 0)
                {
                    ending = "_" + i;
                }
                string startFilePath = mainFrm.getServerDataStr("start_file_path" + ending);
                if (startFilePath != null)
                {
                    addBtn("Start server from " + Path.GetFileName(startFilePath), mainFrm.getCurrentInstancePath() + startFilePath);
                }
                else break;
                i++;
            }
        }
        private Process prepareProcess(string path)
        {
            Process process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.WorkingDirectory = new FileInfo(path).Directory.FullName;
            return process;
        }
        private void saveExitBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        private void startSrvForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void scrollPnl_EditablePanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void scrollPnl_Load(object sender, EventArgs e)
        {

        }

        private void startSrvForm_TextChanged(object sender, EventArgs e)
        {
            baseFormUsrCtrl1.Title = this.Text;
        }

        private void dummycustomSmoothBtn1_Click(object sender, EventArgs e)
        {

        }
    }
}
