using Server_Creation_Tool.myClasses;
using System.Diagnostics;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Server_creation_tool.classes;
using System.Data;

namespace Server_creation_tool.reusable_controls.messageBox
{
    public class messageBox
    {
        public messageBox(Form parentForm)
        {
            parentFrm = parentForm;
        }
        Form parentFrm;
        funcsClass funcs = new funcsClass();
        DataTable generalLang = new DataTable();
        //user friendly functions
        public DialogResult Show(string body, string title, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon MessageBoxIcon = MessageBoxIcon.None, Image img = null)
        {
            DialogResult diagRes;
            msgBox MsgBox = new msgBox();
           // MsgBox.AutoSize = true;
           // MsgBox.baseFormUsrCtrl1.AutoSize = true;
            diagRes = MsgBox.Show(parentFrm, body, title, buttons, MessageBoxIcon, img);
            try
            {
                MsgBox.Dispose();
            }
            catch { }
            return diagRes;
        }
    
        public DialogResult Dontshow(ref bool dontshowAgain, string body, string title, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon MessageBoxIcon = MessageBoxIcon.None, Image img = null)
        {
            if (dontshowAgain) return DialogResult.None;
            DialogResult diagRes;
            msgBox MsgBox = new msgBox();
            MsgBox.dont_show_again_checkbox = true;
            diagRes = MsgBox.Show(parentFrm, body, title, buttons, MessageBoxIcon, img);
            dontshowAgain = MsgBox.chkBox.Checked;
            MsgBox.Dispose();
            return diagRes;

        }
        public void quickMsg(string title, string body, int extrLength = 0)
        {

            string extraLength = "";
            for (int i = 0; i < extrLength; i++)
            {
                extraLength += " ";
            }
            Debug.WriteLine(body);
            Show(body + extraLength, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
