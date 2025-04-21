using Server_creation_tool.classes;
using Server_Creation_Tool.myClasses;
using System.Diagnostics;
using System;
using System.Windows.Forms;

namespace Server_creation_tool.Server_data_files.cs2
{
    internal class cs2_funcs
    {
        funcsClass funcs = new funcsClass();

        public cs2_funcs(MainForm mainForm)
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
            string askToken(string text, string caption)
            {
                Form prompt = new Form();
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.FormBorderStyle = FormBorderStyle.FixedSingle;
                prompt.MaximizeBox = false;
                prompt.MinimizeBox = false;
                prompt.Width = 330;
                prompt.Height = 140;
                prompt.Text = caption;
                Label textLabel = new Label() { Left = 30, Top = 10, Text = text, Width = 500, Height = 40 };
                TextBox inputBox = new TextBox() { Left = 30, Top = 50, Width = 250 };
                Button confirmation = new Button() { Text = "Ok", Left = 180, Width = 100, Top = 70 };
                LinkLabel tokenGenerator = new LinkLabel() { Text = "Server Token Generator Link", Left = 30, Top = 78, Width = 200 };
                tokenGenerator.Click += (sender, e) => { Process.Start("https://steamcommunity.com/dev/managegameservers"); };
                confirmation.FlatStyle = FlatStyle.System;
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(inputBox);
                prompt.Controls.Add(tokenGenerator);
                prompt.ShowDialog();
                return (string)inputBox.Text;
            }
            string serverToken = askToken("Please enter a VALID server token to show" + System.Environment.NewLine + "your server in the server list(Optional)", mainFrm.getControlsLang("create_custom_start_bat_file")[1]);
            string askGamemode(string caption)
            {
                Form prompt = new Form();
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.FormBorderStyle = FormBorderStyle.FixedSingle;
                prompt.ControlBox = false;
                prompt.MaximizeBox = false;
                prompt.MinimizeBox = false;
                prompt.Width = 200;
                prompt.Height = 220;
                prompt.Text = caption;
                //gamemode radiobuttons
                RadioButton classicCasual = new RadioButton() { Text = "Classic Casual", Name = "classicCasual", Left = 2, Top = 0, FlatStyle = FlatStyle.System };
                RadioButton ClassicCompetitive = new RadioButton() { Text = "Classic Competitive", Name = "ClassicCompetitive", Left = 2, Top = 24, Width = 500, FlatStyle = FlatStyle.System };
                RadioButton ArmsRace = new RadioButton() { Text = "Arms Race", Name = "ArmsRace", Left = 2, Top = 48, Width = 500, FlatStyle = FlatStyle.System };
                RadioButton Demolition = new RadioButton() { Text = "Demolition", Name = "Demolition", Left = 2, Top = 72, Width = 500, FlatStyle = FlatStyle.System };
                RadioButton Deathmatch = new RadioButton() { Text = "Deathmatch", Name = "Deathmatch", Left = 2, Top = 96, Width = 500, FlatStyle = FlatStyle.System };
                Button confirmation = new Button() { Text = "Ok", Left = 45, Width = 100, Top = 155, FlatStyle = FlatStyle.System };
                confirmation.FlatStyle = FlatStyle.System;
                string checkedButton = null;
                confirmation.Click += (sender, e) =>
                {
                    if (classicCasual.Checked == true)
                    { checkedButton = classicCasual.Name; }
                    else if (ClassicCompetitive.Checked == true)
                    { checkedButton = ClassicCompetitive.Name; }
                    else if (ArmsRace.Checked == true)
                    { checkedButton = ArmsRace.Name; }
                    else if (Demolition.Checked == true)
                    { checkedButton = Demolition.Name; }
                    else if (Deathmatch.Checked == true)
                    { checkedButton = Deathmatch.Name; }
                    prompt.Close();
                };
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(classicCasual);
                prompt.Controls.Add(ArmsRace);
                prompt.Controls.Add(ClassicCompetitive);
                prompt.Controls.Add(Demolition);
                prompt.Controls.Add(Deathmatch);
                classicCasual.Checked = true;
                prompt.ShowDialog();
                //get checked radionbutton
                return (string)checkedButton;
            }
            //check if user has entered valid info 
            string gamemodeBatchCommand = null;
            string gamemode = askGamemode("Select a Gamemode");
            switch (gamemode)
            {
                case "classicCasual":
                    gamemodeBatchCommand = "+game_type 0 +game_mode 0 +mapgroup mg_active +map de_dust2";
                    break;

                case "ArmsRace":
                    gamemodeBatchCommand = "+game_type 1 +game_mode 0 +mapgroup mg_armsrace +map ar_shoots";
                    break;

                case "ClassicCompetitive":
                    gamemodeBatchCommand = "+game_type 0 +game_mode 1 +mapgroup mg_active +map de_dust2";
                    break;

                case "Demolition":
                    gamemodeBatchCommand = "+game_type 1 +game_mode 1 +mapgroup mg_demolition +map de_lake";
                    break;

                case "Deathmatch":
                    gamemodeBatchCommand = "+game_type 1 +game_mode 2 +mapgroup mg_allclassic +map de_dust";
                    break;
            }
            if (funcs.writeTxtFile(mainFrm.getCurrentInstancePath() + mainFrm.getServerDataStr("start_file_path"), "start srcds -game csgo -console -usercon " + gamemodeBatchCommand + " +sv_setsteamaccount " + serverToken + " -maxplayers_override 24", "ERROR: Failed to write " + mainFrm.srvName() + " server batch file"))
            {
                mainFrm.MsgBox.quickMsg(mainFrm.getControlsLang("create_custom_start_bat_file")[1], mainFrm.getGeneralLang("done")[1], 40);
                return true;
            }
            else
            {
                mainFrm.MsgBox.Show("Failed to write " + mainFrm.srvName() + " server batch file", mainFrm.getControlsLang("create_custom_start_bat_file")[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
        public bool stopServerInstall()
        {
            return false;
        }
    }
}
