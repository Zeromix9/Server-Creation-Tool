using Server_creation_tool.classes;
using System.Windows.Forms;

namespace Server_creation_tool.Server_data_files.sven
{
    internal class sven_funcs
    {
        funcsClass funcs = new funcsClass();

        public sven_funcs(MainForm mainForm)
        {
            mainFrm = mainForm;
        }
        MainForm mainFrm;
        public void setup()
        {

        }
        public bool createConfFile()
        {
            return false;
        }
        public bool startServer()
        {
            return false;
        }
        public bool createBatchFile()
        {
            string lanOrInternetCommand = null;
            string InternetOrLan(string text, string caption)
            {
                Form prompt = new Form();
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.FormBorderStyle = FormBorderStyle.FixedSingle;
                prompt.MaximizeBox = false;
                prompt.MinimizeBox = false;
                prompt.Width = 343;
                prompt.Height = 140;
                prompt.Text = caption;
                Label textLabel = new Label() { Left = 4, Top = 10, Text = text, Width = 500, Height = 40 };
                Button Lan = new Button() { Text = "LAN", Left = 170, Width = 100, Top = 70 };
                Button Internet = new Button() { Text = "Internet", Left = 50, Width = 100, Top = 70 };
                string returnString = null;
                Lan.FlatStyle = FlatStyle.System;
                //Lan
                Lan.Click += (sender, e) =>
                {
                    returnString = "lan";
                    prompt.Close();
                };
                //Internet
                Internet.Click += (sender, e) =>
                {
                    returnString = "internet";
                    prompt.Close();
                };
                Internet.FlatStyle = FlatStyle.System;
                prompt.Controls.Add(Lan);
                prompt.Controls.Add(Internet);
                prompt.Controls.Add(textLabel);
                prompt.ShowDialog();
                return (string)returnString;
            }
            string internetOrLanUserChoice = InternetOrLan("Choose whether the server will be open to the whole internet \nor only in the local network(LAN)", "Choose type of batch file");
            //check what user entered
            if (internetOrLanUserChoice == null)
            { return false; }
            else if (internetOrLanUserChoice == "lan")
            { lanOrInternetCommand = "start SvenDS -console -port 27015 +maxplayers 8 -insecure -nomaster +sv_lan 1 +map _server_start"; }
            else if (internetOrLanUserChoice == "internet")
            { lanOrInternetCommand = "start SvenDS -console -port 27015 +maxplayers 8 +log on +map _server_start"; }
            if (funcs.writeTxtFile(mainFrm.getCurrentInstancePath() + mainFrm.getServerDataStr("start_file_path"), lanOrInternetCommand, "ERROR: Failed to write " + mainFrm.srvName() + " server batch file"))
            {
                mainFrm.MsgBox.quickMsg(mainFrm.getControlsLang("create_custom_start_bat_file")[1], mainFrm.getGeneralLang("done")[1],40);
                return true;
            }
            else
            {
                mainFrm.MsgBox.Show("Failed to write " + mainFrm.srvName() + " server batch file","Create batch file",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }

        }
        public bool stopServerInstall()
        {
            return false;
        }
    }
}
