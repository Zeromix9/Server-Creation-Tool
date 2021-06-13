using statServer_Creation_Tool;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Server_Creation_Tool.myClasses
{
    class serverFuncs
    {
        global_Variables gVars = new global_Variables();
        funcClass funcs = new funcClass();
        public void createBatFilecs16(string batFilePath)
        {
            string lanOrInternetCommand = null;
            string InternetOrLan(string text, string caption)
            {
                Form prompt = new Form();
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
            string internetOrLanUserChoice = InternetOrLan(funcs.lg(lang.createBatFileAskInternetOrLan), funcs.lg(lang.createBatFile, 1));
            //check what user entered
            if (internetOrLanUserChoice == null)
            { return; }
            else if (internetOrLanUserChoice == "lan")
            { lanOrInternetCommand = "start hlds -game cstrike -console -insecure -nomaster +sv_lan 1 +maxplayers 12 +map de_dust"; }
            else if (internetOrLanUserChoice == "internet")
            { lanOrInternetCommand = "start hlds -game cstrike -console +maxplayers 12 +map de_dust"; }

            try
            {
                System.IO.File.WriteAllText(batFilePath, lanOrInternetCommand); //Write Batch file   
                MessageBox.Show(funcs.lg(lang.done), funcs.lg(lang.createBatFile, 1), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            { MessageBox.Show(funcs.lg(lang.batFail), funcs.lg(lang.createBatFile, 1), MessageBoxButtons.OK, MessageBoxIcon.Error); log.Append(e.ToString()); }
        }
        public void createBatFileSven(string batFilePath)
        {
            string lanOrInternetCommand = null;
            string InternetOrLan(string text, string caption)
            {
                Form prompt = new Form();
                prompt.FormBorderStyle = FormBorderStyle.FixedSingle;
                prompt.MaximizeBox = false;
                prompt.MinimizeBox = false;
                prompt.Width = 343;
                prompt.Height = 140;
                prompt.Text = caption;
                Label textLabel = new Label() { Left = 3, Top = 10, Text = text, Width = 500, Height = 40 };
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
            string internetOrLanUserChoice = InternetOrLan(funcs.lg(lang.createBatFileAskInternetOrLan), funcs.lg(lang.createBatFile, 1));
            //check what user entered
            if (internetOrLanUserChoice == null)
            {  return; }
            else if (internetOrLanUserChoice == "lan")
            { lanOrInternetCommand = "start SvenDS -console -port 27015 +maxplayers 8 -insecure -nomaster +sv_lan 1 +map _server_start"; }
            else if (internetOrLanUserChoice == "internet")
            { lanOrInternetCommand = "start SvenDS -console -port 27015 +maxplayers 8 +log on +map _server_start"; }
            //Write Batch file
            try
            {
                System.IO.File.WriteAllText(batFilePath, lanOrInternetCommand);//Write Batch file  
                MessageBox.Show(funcs.lg(lang.done), funcs.lg(lang.createBatFile, 1), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            { MessageBox.Show(funcs.lg(lang.batFail), funcs.lg(lang.createBatFile, 1), MessageBoxButtons.OK, MessageBoxIcon.Error); log.Append(e.ToString()); }
        }
        public void createBatFileCSGO(string batFilePath)
        {
            string askToken(string text, string caption)
            {
                Form prompt = new Form();
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
            string serverToken = askToken(funcs.lg(lang.createBatFileAskToken), funcs.lg(lang.createBatFile,1));
            string askGamemode(string caption)
            {
                Form prompt = new Form();
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
            try
            {
                //Batch file
                System.IO.File.WriteAllText(batFilePath, "start srcds -game csgo -console -usercon " + gamemodeBatchCommand + " +sv_setsteamaccount " + serverToken + " -maxplayers_override 24"); //Write Batch file   
                MessageBox.Show(funcs.lg(lang.done), funcs.lg(lang.createBatFile, 1), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            { MessageBox.Show(funcs.lg(lang.batFail), funcs.lg(lang.createBatFile, 1), MessageBoxButtons.OK, MessageBoxIcon.Error); log.Append(e.ToString()); }
        }
    }
}
