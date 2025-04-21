using Server_creation_tool.classes;
using Server_creation_tool.reusable_controls.messageBox;
using Server_Creation_Tool.myClasses;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Server_creation_tool
{
    public partial class settingsForm : Form
    {
        public settingsForm(MainForm callingFrm1)
        {
            callingFrm = callingFrm1;
            InitializeComponent();
        }
        MainForm callingFrm;
        messageBox MsgBox;
        funcsClass funcs = new funcsClass();
        DataTable controlsLang = new DataTable();
        DataTable generalLang = new DataTable();
        private string[] getGeneralLang(string str)
        {
            return callingFrm.getStrFromDtronRes(generalLang, str);
        }
        private void settingsForm_Load(object sender, EventArgs e)
        {
            MsgBox = new messageBox(this);
            defaultTextEditorLbl.Text = Path.GetFileName(Properties.Settings.Default.defaultTextEditor);
            defaultTextEditorLbl.Tag = Properties.Settings.Default.defaultTextEditor;
            //set lang

            try
            {
                StringReader ctrlReader = new StringReader(funcs.readEmbeddedRes("Server_creation_tool.Language_files." + Properties.Settings.Default.lang + ".controls.settingsForm.lang") as string);
                StringReader generalReader = new StringReader(funcs.readEmbeddedRes("Server_creation_tool.Language_files." + Properties.Settings.Default.lang + ".general.settingsForm.lang") as string);
                Debug.WriteLine(funcs.readEmbeddedRes("Server_creation_tool.Language_files." + Properties.Settings.Default.lang + ".general.settingsForm.lang") as string);
                controlsLang.ReadXml(ctrlReader);
                generalLang.ReadXml(generalReader);
                controlsLang.PrimaryKey = new[] { controlsLang.Columns[0] };
                generalLang.PrimaryKey = new[] { generalLang.Columns[0] };
            }
            catch (Exception ex)
            {
                log.Append("ERROR READING LANGUAGE FILES (settings). Current lang: " + Properties.Settings.Default.lang + ". Exception data: " + ex.ToString());
                MsgBox.Show(callingFrm.getGeneralLang("lang_error")[1], callingFrm.getGeneralLang("lang_error")[2], MessageBoxButtons.OK,MessageBoxIcon.Error);
                Application.Exit();
            }
            callingFrm.setControlsLang(controlsLang, this);

        }

        private void saveExitBtn_Click(object sender, EventArgs e)
        {
            if (MsgBox.Show(getGeneralLang("save_exit")[1], getGeneralLang("save_exit")[2], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Properties.Settings.Default.defaultTextEditor = defaultTextEditorLbl.Tag.ToString();
                Properties.Settings.Default.Save();
                this.Close();
            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void chooseTextEditorbtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Title = getGeneralLang("choose_editor")[1];
            diag.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            diag.Filter = "Executables|*.exe";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                defaultTextEditorLbl.Text = diag.SafeFileName;
                defaultTextEditorLbl.Tag = diag.FileName;
            }
        }

        private void customSmoothBtn1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void customSmoothBtn2_Click(object sender, EventArgs e)
        {
            contexMenuStripFrm contexMenu = new contexMenuStripFrm();
            contexMenu.addBtn(getGeneralLang("reset_to_default")[1], () =>
            {
                defaultTextEditorLbl.Text = "notepad.exe";
                defaultTextEditorLbl.Tag = "notepad.exe";
            }, false, Properties.Resources.refresh16x16);
            funcs.showMenuStripAtBtn(contexMenu, defaultTextEditorMoreBtn, this);
        }

        private void settingsForm_TextChanged(object sender, EventArgs e)
        {
            baseFormUsrCtrl1.Title = this.Text;
        }

        private void baseFormUsrCtrl1_Load(object sender, EventArgs e)
        {

        }
    }
}
