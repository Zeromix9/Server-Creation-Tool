using statServer_Creation_Tool;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Server_Creation_Tool
{
    class gameFinishInstallFunc
    {
       METHODSandFUNCTIONS methodsClass = new METHODSandFUNCTIONS();



        //-----------------Counter Strike 1.6---------------
        public void finishInstallationCS16(string steamCmdSpeicherort, string serverFolderName)
        {
            if (methodsClass.HasInternet() != true)
            { MessageBox.Show("Internet Error","Error",MessageBoxButtons.OK,MessageBoxIcon.Error); }

                string serverFolderpath = Path.GetDirectoryName(steamCmdSpeicherort) + @"\" + serverFolderName;
            //Check Langouage

            string[] askUserToCreateConfigAndBatchFileMsgBox = null;
            string[] failedToCreateServerBatchFileMsgBox = null;
            string[] installationFailedMsgBox = null;
            string[] importantNoteMsgBox = null;
            string[] lanOrInternet = null;
            string configFileURLforEachLangouage = null;
            switch (Properties.Settings.Default.language)
            {
                //Second value of each variable is the title of the messagebox and the first value is the message
                case "english":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { "Do you want to create a batch and config file? The batch file will be created in the server's ROOT folder and the config file here: " + serverFolderpath + @"\cstrike\server.cfg"  + ". It is optional but it is very helpful for setting up the server", "Create batch and config file" };
                    failedToCreateServerBatchFileMsgBox = new string[] { "Failed to create batch and config files! You can create the files yourself using the code in the server's guide", "Create batch and config File" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Counter Strike 1.6 Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the batch and config file later to change player slots, map, password etc. For more info, go to the Counter Strike 1.6 Server guide", "Note" };
                    lanOrInternet = new string[] { "Please select if the server will run via lan or on the internet", "Create batch and config File" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/JbsxsQuE";
                    break;
                case "german":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { @"Möchte du eine Batch Datei, sowie eine Konfigurationsdatei erstellen? Die Batch Datei wird im Hauptverzeichnis des Servers erstellt und die Konfigurationsdatei hier: """ + serverFolderpath + @"\cstrike\server.cfg""" + ". Dies ist optional, jedoch sehr hilfreich beim aufsetzen eines Servers.", "Erstelle Batch-, sowie Konfigurationsdatei" };
                    failedToCreateServerBatchFileMsgBox = new string[] { "Die Erstellung der Dateien schlug fehl! Du kannst diese Dateien selbst erstellen mit dem Code, der in dem Guide zu finden ist.", "Erstelle Batch-, sowie Konfigurationsdatei" };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Sie Counter Strike 1.6 Server" };
                    importantNoteMsgBox = new string[] { "WICHTIGER HINWEIß: Du kannst die Batch-, sowie Konfigurationsdatei später ändern, um z.B. die Anzahl der Spieler, die Karte oder das Passwort zu ändern. Für mehr Informationen öffne den Counter-Strike 1.6 Server Guide.", "Notieren" };
                    lanOrInternet = new string[] { "Wähle aus, ob der Server im Lan oder im Internet verfügbar sein soll.", "Erstelle Batch-, sowie Konfigurationsdatei" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/XGtEw9Qp";
                    break;
            }
            //Check if installation was successfull
            if (methodsClass.checkIfGameInstalled(Path.GetDirectoryName(steamCmdSpeicherort), serverFolderName) == true)
            {
                if (MessageBox.Show(askUserToCreateConfigAndBatchFileMsgBox[0], askUserToCreateConfigAndBatchFileMsgBox[1], MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                    string internetOrLanUserChoice = InternetOrLan(lanOrInternet[0], lanOrInternet[1]);
                    //check what user entered
                    if (internetOrLanUserChoice == null)
                    {
                        MessageBox.Show("Canceled!");
                        return;
                    }
                    else if (internetOrLanUserChoice == "lan")
                    {
                        lanOrInternetCommand = "start hlds -game cstrike -console -insecure -nomaster +sv_lan 1 +maxplayers 12 +map de_dust";
                    }
                    else if (internetOrLanUserChoice == "internet")
                    {
                        lanOrInternetCommand = "start hlds -game cstrike -console +maxplayers 12 +map de_dust";
                    }
                   
                    try
                    {
                        //Write Batch file
                        System.IO.File.WriteAllText(serverFolderpath + "\\Run.bat", lanOrInternetCommand);
                        //DOWNLOAD CONFIG FILE AND WRITE IT
                        WebClient client = new WebClient();
                        Stream stream = client.OpenRead(configFileURLforEachLangouage);
                        StreamReader reader = new StreamReader(stream);
                        String content = reader.ReadToEnd();
                        System.IO.Directory.CreateDirectory(serverFolderpath + @"\cstrike");
                        System.IO.File.WriteAllText(serverFolderpath + @"\cstrike\server.cfg", content);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(failedToCreateServerBatchFileMsgBox[0], failedToCreateServerBatchFileMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    MessageBox.Show(importantNoteMsgBox[0], importantNoteMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                Process.Start(serverFolderpath);
            }
            else
            {
                MessageBox.Show(installationFailedMsgBox[0], installationFailedMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //-----------------GMOD----------------------
        public void finishInstallationGmod(string steamCmdSpeicherort, string serverFolderName)
        {
            if (methodsClass.HasInternet() != true)
            { MessageBox.Show("Internet Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            string serverFolderpath = Path.GetDirectoryName(steamCmdSpeicherort) + @"\" + serverFolderName;
            //Check Langouage

            string[] askUserToCreateConfigAndBatchFileMsgBox = null;
            string[] failedToCreateServerStartupFilesMsgBox = null;
            string[] installationFailedMsgBox = null;
            string[] importantNoteMsgBox = null;
            string configFileURLforEachLangouage = null;
            switch (Properties.Settings.Default.language)
            {
                //Second value of each variable is the title of the messagebox and the first value is the message
                case "english":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { @"You have to create a batch file and a configuration file in order to start the server.You want it to be created automatically or create it yourself?The Batch file will be created in the server's ROOT folder and the configuration file will be created at: " + serverFolderpath + @"\garrysmod\cfg\server.cfg", "Create server's startup files" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Failed to create the config and batch files!You can create the files yourself using the code in the server's guide", "Create server's config and batch files" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Gmod Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the batch and config file later to change player slots, map, password etc. For more info, go to the Gmod Server guide", "Note" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/vtdYsHLG";
                    break;

                case "german":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { "Eine Batch Datei wird benötigt, um den Server zu starten, sowie eine server.cfg. Sollen diese automatisch erstellt werden, oder nicht? Die Batch Datei wird im Server Verzeichnis erstellt und die Konfigurationsdatei in: " + serverFolderpath + @"\garrysmod\cfg\server.cfg", "Erstellen Sie die Startdateien des Servers" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Erstellung der Konfigurations- und Batchdatei fehlgeschlagen.! Sie können diese Dateien mithilfe des Codes des Server-Guides selbst erstellen.", "Erstelle Server Konfigurationsdatei, sowie die Batch Datei." };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Sie Gmod Server" };
                    importantNoteMsgBox = new string[] { "WICHTIG: Du kannst diese Dateien später noch editieren. Für mehr Informationen, öffne den Gmod Server Guide.", "Notieren" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/YzEbTBVy";
                    break;
            }
            //Check if installation was successfull
            if (methodsClass.checkIfGameInstalled(Path.GetDirectoryName(steamCmdSpeicherort), serverFolderName) == true)
            {
                if (MessageBox.Show(askUserToCreateConfigAndBatchFileMsgBox[0], askUserToCreateConfigAndBatchFileMsgBox[1], MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        //Batch File
                        System.IO.File.WriteAllText(serverFolderpath + "\\Run.bat", "start srcds.exe -console -game cstrike -secure +maxplayers 22  +map gm_flatgrass");
                        //DOWNLOAD CONFIG FILE AND WRITE IT
                        WebClient client = new WebClient();
                        Stream stream = client.OpenRead(configFileURLforEachLangouage);
                        StreamReader reader = new StreamReader(stream);
                        String content = reader.ReadToEnd();
                        System.IO.Directory.CreateDirectory(serverFolderpath + @"\garrysmod\cfg");
                        System.IO.File.WriteAllText(serverFolderpath + @"\garrysmod\cfg\server.cfg", content);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(failedToCreateServerStartupFilesMsgBox[0], failedToCreateServerStartupFilesMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    MessageBox.Show(importantNoteMsgBox[0], importantNoteMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                Process.Start(serverFolderpath);
            }
            else
            {
                MessageBox.Show(installationFailedMsgBox[0], installationFailedMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //-----------------Left 4 Dead 2------------------------
        public void finishInstallationL4d2(string steamCmdSpeicherort, string serverFolderName)
        {
            string serverFolderpath = Path.GetDirectoryName(steamCmdSpeicherort) + @"\" + serverFolderName;
            //Check Langouage
            string[] askUserToCreateConfigAndBatchFileMsgBox = null;
            string[] failedToCreateServerStartupFilesMsgBox = null;
            string[] installationFailedMsgBox = null;
            string[] importantNoteMsgBox = null;
            string configFileURLforEachLangouage = null;
            switch (Properties.Settings.Default.language)
            {
                //Second value of each variable is the title of the messagebox and the first value is the message
                case "english":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { @"It is recommended to create a batch file for starting the server.You want it to be created automatically or create it yourself?The Batch file will be created in the server's ROOT folder and the configuration file will be created at: " + serverFolderpath + @"\left4dead2\cfg\server.cfg", "Create server's startup files" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Failed to create the config and batch files!You can create the files yourself using the code in the server's guide", "Create server's config and batch files" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Left 4 Dead 2 Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the batch and config file later to change player slots, map, password etc. For more info, go to the Left 4 dead 2 Server guide", "Note" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/1i8xK3L4";
                    break;

                case "german":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { @"Es wird empfohlen eine Batch Datei zu erstellen. um den Server zu starten. Soll die Datei jetzt automatisch erstellt werden? Die Batch Datei wird im Server Verzeichnis erstellt und die Konfigurationsdatei in: " + serverFolderpath + @"\left4dead2\cfg\server.cfg", "Erstellen Sie die Startdateien des Servers" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Erstellung der Konfigurations- und Batchdatei fehlgeschlagen.! Sie können diese Dateien mithilfe des Codes des Server-Guides selbst erstellen.", "Erstelle Server Konfigurationsdatei, sowie die Batch Datei." };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Sie Left 4 dead 2 Server" };
                    importantNoteMsgBox = new string[] { "WICHTIG: Du kannst diese Dateien später noch editieren. Für mehr Informationen, öffne den Left 4 dead 2 Server Guide.", "Notieren" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/NWjDxe4G";
                    break;
            }
            //Check if installation was successfull
            if (methodsClass.checkIfGameInstalled(Path.GetDirectoryName(steamCmdSpeicherort), serverFolderName) == true)
            {
                if (MessageBox.Show(askUserToCreateConfigAndBatchFileMsgBox[0], askUserToCreateConfigAndBatchFileMsgBox[1], MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        //Batch file
                        System.IO.File.WriteAllText(serverFolderpath + "\\Run.bat", "start srcds.exe -console -game left4dead2 -secure +maxplayers 4  +map c1m1_hotel");
                        //DOWNLOAD CONFIG FILE AND WRITE IT
                        WebClient client = new WebClient();
                        Stream stream = client.OpenRead(configFileURLforEachLangouage);
                        StreamReader reader = new StreamReader(stream);
                        String content = reader.ReadToEnd();
                        System.IO.Directory.CreateDirectory(serverFolderpath + @"\left4dead2\cfg");
                        System.IO.File.WriteAllText(serverFolderpath + @"\left4dead2\cfg\server.cfg", content);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(failedToCreateServerStartupFilesMsgBox[0], failedToCreateServerStartupFilesMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    MessageBox.Show(importantNoteMsgBox[0], importantNoteMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                Process.Start(serverFolderpath);
            }
            else
            {
                MessageBox.Show(installationFailedMsgBox[0], installationFailedMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //-----------------Counter Strike: Source--------------------
        public void finishInstallationCSSOURCE(string steamCmdSpeicherort, string serverFolderName)
        {
            string serverFolderpath = Path.GetDirectoryName(steamCmdSpeicherort) + @"\" + serverFolderName;
            //Check Langouage

            string[] askUserToCreateConfigAndBatchFileMsgBox = null;
            string[] failedToCreateServerStartupFilesMsgBox = null;
            string[] installationFailedMsgBox = null;
            string[] importantNoteMsgBox = null;
            string configFileURLforEachLangouage = null;
            switch (Properties.Settings.Default.language)
            {
                //Second value of each variable is the title of the messagebox and the first value is the message
                case "english":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { @"You have to create a batch file and a configuration file in order to start the server.You want it to be created automatically or create it yourself?The Batch file will be created in the server's ROOT folder and the configuration file will be created at: " + serverFolderpath + @"\cstrike\cfg\server.cfg", "Create server's startup files" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Failed to create the config and batch files!You can create the files yourself using the code in the server's guide", "Create server's config and batch files" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Counter Strike Source Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the batch and config file later to change player slots, map, password etc. For more info, go to the Counter Strike Source Server guide", "Note" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/e3f89ijz";
                    break;

                case "german":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { "Eine Batch Datei wird benötigt, um den Server zu starten, sowie eine server.cfg. Sollen diese automatisch erstellt werden, oder nicht? Die Batch Datei wird im Server Verzeichnis erstellt und die Konfigurationsdatei in: " + serverFolderpath + @"\cstrike\cfg\server.cfg", "Erstellen Sie die Startdateien des Servers" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Erstellung der Konfigurations- und Batchdatei fehlgeschlagen.! Sie können diese Dateien mithilfe des Codes des Server-Guides selbst erstellen.", "Erstelle Server Konfigurationsdatei, sowie die Batch Datei." };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Sie Counter Strike Source Server" };
                    importantNoteMsgBox = new string[] { "WICHTIG: Du kannst diese Dateien später noch editieren. Für mehr Informationen, öffne den Counter Strike Source Server Guide.", "Notieren" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/mQCitqtv";
                    break;
            }
            //Check if installation was successfull
            if (methodsClass.checkIfGameInstalled(Path.GetDirectoryName(steamCmdSpeicherort), serverFolderName) == true)
            {
                if (MessageBox.Show(askUserToCreateConfigAndBatchFileMsgBox[0], askUserToCreateConfigAndBatchFileMsgBox[1], MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        //Batch file
                        System.IO.File.WriteAllText(serverFolderpath + "\\Run.bat", "start srcds.exe -console -game cstrike -secure +maxplayers 22 +map de_dust");
                        //DOWNLOAD CONFIG FILE AND WRITE IT
                        WebClient client = new WebClient();
                        Stream stream = client.OpenRead(configFileURLforEachLangouage);
                        StreamReader reader = new StreamReader(stream);
                        String content = reader.ReadToEnd();
                        System.IO.Directory.CreateDirectory(serverFolderpath + @"\cstrike\cfg");
                        System.IO.File.WriteAllText(serverFolderpath + @"\cstrike\cfg\server.cfg", content);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(failedToCreateServerStartupFilesMsgBox[0], failedToCreateServerStartupFilesMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    MessageBox.Show(importantNoteMsgBox[0], importantNoteMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                Process.Start(serverFolderpath);
            }
            else
            {
                MessageBox.Show(installationFailedMsgBox[0], installationFailedMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //-----------------CS:GO----------------------
        public void finishInstallationCSGO(string steamCmdSpeicherort, string serverFolderName)
        {
            string serverFolderpath = Path.GetDirectoryName(steamCmdSpeicherort) + @"\" + serverFolderName;
            //Check Language

            string[] askUserToCreateConfigAndBatchFileMsgBox = null;
            string[] failedToCreateServerStartupFilesMsgBox = null;
            string[] installationFailedMsgBox = null;
            string[] importantNoteMsgBox = null;
            string configFileURLforEachLangouage = null;
            string[] askTokenDialog = null;

            switch (Properties.Settings.Default.language)
            {
                //Second value of each variable is the title of the messagebox and the first value is the message
                case "english":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { @"You have to create a batch file and a configuration file in order to start the server.You want it to be created automatically or create it yourself?The Batch file will be created in the server's ROOT folder and the configuration file will be created at: " + serverFolderpath + @"\csgo\cfg\server.cfg", "Create server's startup files" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Failed to create the config and batch files!You can create the files yourself using the code in the server's guide", "Create server's config and batch files" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Counter Strike Global Offensive Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the batch and config file later to change player slots, map, password etc. For more info, go to the Counter Strike Global Offensive Server guide", "Note" };
                    askTokenDialog = new string[] { "Please enter a VALID server token to show" + System.Environment.NewLine + "your server in the server list(Optional)", "Create server's startup files" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/n1XcFu5F";
                    break;

                case "german":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { "Eine Batch Datei wird benötigt, um den Server zu starten, sowie eine server.cfg. Sollen diese automatisch erstellt werden, oder nicht? Die Batch Datei wird im Server Verzeichnis erstellt und die Konfigurationsdatei in: " + serverFolderpath + @"\csgo\cfg\server.cfg", "Erstellen Sie die Startdateien des Servers" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Erstellung der Konfigurations- und Batchdatei fehlgeschlagen.! Sie können diese Dateien mithilfe des Codes des Server-Guides selbst erstellen.", "Erstelle Server Konfigurationsdatei, sowie die Batch Datei." };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Sie Counter Strike Global Offensive Server" };
                    importantNoteMsgBox = new string[] { "WICHTIG: Du kannst diese Dateien später noch editieren. Für mehr Informationen, öffne den Counter Strike Global Offensive Server Guide.", "Notieren" };
                    askTokenDialog = new string[] { "Bitte geben Sie ein GÜLTIGES Server-Token ein, um Ihren" + System.Environment.NewLine + "Server in der Serverliste anzuzeigen(optional)", "Erstellen Sie die Startdateien des Servers" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/n1XcFu5F";
                    break;
            }


            //Check if installation was successfull
            if (methodsClass.checkIfGameInstalled(Path.GetDirectoryName(steamCmdSpeicherort), serverFolderName) == true)
            {
                if (MessageBox.Show(askUserToCreateConfigAndBatchFileMsgBox[0], askUserToCreateConfigAndBatchFileMsgBox[1], MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                    string serverToken = askToken(askTokenDialog[0], askTokenDialog[1]).Trim();

                    string askGamemode(string caption)
                    {
                        Form prompt = new Form();
                        prompt.FormBorderStyle = FormBorderStyle.FixedSingle;
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
                            {
                                checkedButton = classicCasual.Name;
                            }
                            else if (ClassicCompetitive.Checked == true)
                            {
                                checkedButton = ClassicCompetitive.Name;
                            }
                            else if (ArmsRace.Checked == true)
                            {
                                checkedButton = ArmsRace.Name;
                            }
                            else if (Demolition.Checked == true)
                            {
                                checkedButton = Demolition.Name;
                            }
                            else if (Deathmatch.Checked == true)
                            {
                                checkedButton = Deathmatch.Name;
                            }
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
                        System.IO.File.WriteAllText(serverFolderpath + "\\Run.bat", "start srcds -game csgo -console -usercon " + gamemodeBatchCommand + " +sv_setsteamaccount " + serverToken + " -maxplayers_override 24");
                        //DOWNLOAD CONFIG FILE AND WRITE IT
                        WebClient client = new WebClient();
                        Stream stream = client.OpenRead(configFileURLforEachLangouage);
                        StreamReader reader = new StreamReader(stream);
                        String content = reader.ReadToEnd();
                        System.IO.Directory.CreateDirectory(serverFolderpath + @"\csgo\cfg");
                        System.IO.File.WriteAllText(serverFolderpath + @"\csgo\cfg\server.cfg", content);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(failedToCreateServerStartupFilesMsgBox[0], failedToCreateServerStartupFilesMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    MessageBox.Show(importantNoteMsgBox[0], importantNoteMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                Process.Start(serverFolderpath);
            }
            else
            {
                MessageBox.Show(installationFailedMsgBox[0], installationFailedMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //-----------------Sven Coop----------------------
        public void finishInstallationSvenCoop(string steamCmdSpeicherort, string serverFolderName)
        {
            string serverFolderpath = Path.GetDirectoryName(steamCmdSpeicherort) + @"\" + serverFolderName;
            //Check Langouage

            string[] askUserToCreateConfigAndBatchFileMsgBox = null;
            string[] failedToCreateServerBatchFileMsgBox = null;
            string[] installationFailedMsgBox = null;
            string[] importantNoteMsgBox = null;
            string[] lanOrInternet = null;
            switch (Properties.Settings.Default.language)
            {
                //Second value of each variable is the title of the messagebox and the first value is the message
                case "english":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { "Do you want to create a batch file for starting the server?The Batch file will be created in the server's folder. It is optional.The server can run without it", "Create Batch file" };
                    failedToCreateServerBatchFileMsgBox = new string[] { "Failed to create Batch File! You can create the file yourself using the code in the server's guide", "Create Batch File" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Sven Coop Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the batch file and config file later to change player slots, map, password etc. For more info on how to edit the config file, go to the Sven Coop Server guide", "Note" };
                    lanOrInternet = new string[] { "Please select if the server will run via lan or on the internet", "Create batch file" };
                    break;
                case "german":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { "Möchtest du eine Batch Datei zum starten des Servers erstellen? Diese Datei wir in deinem Server Ordner erstellt. Dies ist optional, der Server kann auch ohne diese Datei starten.", "Erstellen Sie eine Batchdatei" };
                    failedToCreateServerBatchFileMsgBox = new string[] { "Erstellung der Batch Datei fehlgeschlagen! Du kannst diese auch selbst erstellen, mithilfe des Codes im Server-Guide.", "Erstellen Sie eine Batchdatei" };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Sie Sven Coop Server" };
                    importantNoteMsgBox = new string[] { "WICHTIGER HINWEIß: Du kannst sowohl die Batch Datei, als auch die Konfiguration Daten später immer noch ändern, um z.B. die Anzahl der Slots, die Karte oder das Passwort zu ändern. Für mehr Informationen wie du die Konfigurationsdatei bearbeitest, öffne den Sven Coop Guide.", "Notieren" };
                    lanOrInternet = new string[] { "Wähle aus, ob der Server im Lan oder im Internet verfügbar sein soll.", "Erstellen Sie eine Batchdatei" };
                    break;
            }
            //Check if installation was successfull
            if (methodsClass.checkIfGameInstalled(Path.GetDirectoryName(steamCmdSpeicherort), serverFolderName) == true)
            {
                if (MessageBox.Show(askUserToCreateConfigAndBatchFileMsgBox[0], askUserToCreateConfigAndBatchFileMsgBox[1], MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                    string internetOrLanUserChoice = InternetOrLan(lanOrInternet[0], lanOrInternet[1]);
                    //check what user entered
                    if (internetOrLanUserChoice == null)
                    {
                        MessageBox.Show("Canceled!");
                        return;
                    }
                    else if (internetOrLanUserChoice == "lan")
                    {
                        lanOrInternetCommand = "start SvenDS -console -port 27015 +maxplayers 8 -insecure -nomaster +sv_lan 1 +map _server_start";
                    }
                    else if (internetOrLanUserChoice == "internet")
                    {
                        lanOrInternetCommand = "start SvenDS -console -port 27015 +maxplayers 8 +log on +map _server_start";
                    }
                    //Write Batch file
                    try
                    {
                        System.IO.File.WriteAllText(serverFolderpath + "\\Run.bat", lanOrInternetCommand);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(failedToCreateServerBatchFileMsgBox[0], failedToCreateServerBatchFileMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    MessageBox.Show(importantNoteMsgBox[0], importantNoteMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                Process.Start(serverFolderpath);
            }
            else
            {
                MessageBox.Show(installationFailedMsgBox[0], installationFailedMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //-----------------Call Of Duty Black Ops 3-------------------
        public void finishInstallationCodBO3(string steamCmdSpeicherort, string serverFolderName)
        {
            string serverFolderpath = Path.GetDirectoryName(steamCmdSpeicherort) + @"\" + serverFolderName;
            //Check Language
            string[] startServerInfo = null;
            string[] installationFailedMsgBox = null;
            string[] importantNoteMsgBox = null;
            switch (Properties.Settings.Default.language)
            {
                //Second value of each variable is the title of the messagebox and the first value is the message
                case "english":
                    startServerInfo = new string[] { "You can start the server from here: " + serverFolderpath + @"\UnrankedServer\Launch_Server.bat", "Call of Duty Black Ops 3 Server" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Call of Duty Black Ops 3 Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the config file at: " + serverFolderpath + @"\machinecfg\playlists.info" + " to change player slots, map, password etc. For more info, go to the Call of Duty Black Ops 3 Server guide", "Note" };
                    break;
                case "german":
                    startServerInfo = new string[] { "Du kannst den Server hier ausführen:" + serverFolderpath + @"\UnrankedServer\Launch_Server.bat", "" };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Call of Duty Black Ops 3 Server" };
                    importantNoteMsgBox = new string[] { "Du findest die Config Datei hier: " + serverFolderpath + @"\machinecfg\playlists.info" + " Dort kannst du unter anderem die maximale Anzahl an Spielern ändern, sowie das Server Passwort und die Karte. Für mehr Informationen schau dir den Call of Duty Black Ops 3 Server Guide an.", "Notieren" };
                    break;
            }
            //Check if installation was successfull
            if (methodsClass.checkIfGameInstalled(Path.GetDirectoryName(steamCmdSpeicherort), serverFolderName) == true)
            {
                MessageBox.Show(startServerInfo[0], startServerInfo[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(importantNoteMsgBox[0], importantNoteMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(serverFolderpath);
            }
            else
            {
                MessageBox.Show(installationFailedMsgBox[0], installationFailedMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //-----------------Hurtworld--------------------
        public void finishInstallationHurtworld(string steamCmdSpeicherort, string serverFolderName)
        {
            string serverFolderpath = Path.GetDirectoryName(steamCmdSpeicherort) + @"\" + serverFolderName;
            //Check Language
            string[] startServerInfo = null;
            string[] installationFailedMsgBox = null;
            string[] importantNoteMsgBox = null;
            switch (Properties.Settings.Default.language)
            {
                //Second value of each variable is the title of the messagebox and the first value is the message
                case "english":
                    startServerInfo = new string[] { "You can start the server from here: " + serverFolderpath + @"\Host.bat", "Hurtworld Server" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Hurtworld Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the config file at: " + serverFolderpath + @"\autoexec_default.cfg" + " to change player slots, map, password etc. For more info, go to the Hurtworld Server guide", "Note" };
                    break;
                case "german":
                    startServerInfo = new string[] { "Du kannst den Server hier ausführen:" + serverFolderpath + @"\Host.bat", "Hurtworld Server" };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Hurtworld Server" };
                    importantNoteMsgBox = new string[] { "Du findest die Config Datei hier: " + serverFolderpath + @"\autoexec_default.cfg" + " Dort kannst du unter anderem die maximale Anzahl an Spielern ändern, sowie das Server Passwort und die Karte. Für mehr Informationen schau dir den Hurtworld Server Guide an.", "Notieren" };
                    break;
            }
            //Check if installation was successfull
            if (methodsClass.checkIfGameInstalled(Path.GetDirectoryName(steamCmdSpeicherort), serverFolderName) == true)
            {
                MessageBox.Show(startServerInfo[0], startServerInfo[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(importantNoteMsgBox[0], importantNoteMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(serverFolderpath);
            }
            else
            {
                MessageBox.Show(installationFailedMsgBox[0], installationFailedMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //-----------------Killing Floor 2--------------------
        public void finishInstallationKf2(string steamCmdSpeicherort, string serverFolderName)
        {
            string serverFolderpath = Path.GetDirectoryName(steamCmdSpeicherort) + @"\" + serverFolderName;
            //Check Langouage
            string[] startServerInfo = null;
            string[] installationFailedMsgBox = null;
            string[] importantNoteMsgBox = null;
            switch (Properties.Settings.Default.language)
            {
                //Second value of each variable is the title of the messagebox and the first value is the message
                case "english":
                    startServerInfo = new string[] { "You can start the server from here: " + serverFolderpath + @"\KF2Server.bat", "Killing Floor 2 Server" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Killing floor 2 Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the config file at: " + serverFolderpath + @"\KFGame\Config\DefaultGame.ini" + " to change player slots, map, password etc. For more info, go to the Killing floor 2 Server guide", "Note" };
                    break;
                case "german":
                    startServerInfo = new string[] { "Du kannst den Server hier ausführen:" + serverFolderpath + @"\KFGame\Config\DefaultGame.ini", "" };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Killing Floor 2 Server" };
                    importantNoteMsgBox = new string[] { "Du findest die Config Datei hier: " + serverFolderpath + @"\KFGame\Config\DefaultGame.ini" + " Dort kannst du unter anderem die maximale Anzahl an Spielern ändern, sowie das Server Passwort und die Karte. Für mehr Informationen schau dir den Killing floor 2 Server Guide an.", "Notieren" };
                    break;
            }
            //Check if installation was successfull
            if (methodsClass.checkIfGameInstalled(Path.GetDirectoryName(steamCmdSpeicherort), serverFolderName) == true)
            {
                MessageBox.Show(startServerInfo[0], startServerInfo[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(importantNoteMsgBox[0], importantNoteMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(serverFolderpath);
            }
            else
            {
                MessageBox.Show(installationFailedMsgBox[0], installationFailedMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //-----------------Left 4 Dead-------------------
        public void finishInstallationL4d(string steamCmdSpeicherort, string serverFolderName)
        {
            string serverFolderpath = Path.GetDirectoryName(steamCmdSpeicherort) + @"\" + serverFolderName;
            //Check Langouage
            string[] askUserToCreateConfigAndBatchFileMsgBox = null;
            string[] failedToCreateServerStartupFilesMsgBox = null;
            string[] installationFailedMsgBox = null;
            string[] importantNoteMsgBox = null;
            string configFileURLforEachLangouage = null;
            switch (Properties.Settings.Default.language)
            {
                //Second value of each variable is the title of the messagebox and the first value is the message
                case "english":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { @"It is recommended to create a batch file for starting the server. You want it to be created automatically or create it yourself?The Batch file will be created in the server's ROOT folder and the configuration file will be created at: " + serverFolderpath + @"\left4dead\cfg\server.cfg", "Create server's startup files" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Failed to create the config and batch files!You can create the files yourself using the code in the server's guide", "Create server's config and batch files" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Left 4 Dead Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the batch and config file later to change player slots, map, password etc. For more info, go to the Left 4 dead Server guide", "Note" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/d2CJ08wX";
                    break;

                case "german":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { @"Es wird empfohlen eine Batch Datei zu erstellen. um den Server zu starten. Soll die Datei jetzt automatisch erstellt werden? Die Batch Datei wird im Server Verzeichnis erstellt und die Konfigurationsdatei in: " + serverFolderpath + @"\left4dead\cfg\server.cfg", "Erstellen Sie die Startdateien des Servers" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Erstellung der Konfigurations- und Batchdatei fehlgeschlagen.! Sie können diese Dateien mithilfe des Codes des Server-Guides selbst erstellen.", "Erstelle Server Konfigurationsdatei, sowie die Batch Datei." };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Sie Left 4 dead Server" };
                    importantNoteMsgBox = new string[] { "WICHTIG: Du kannst diese Dateien später noch editieren. Für mehr Informationen, öffne den Left 4 dead Server Guide.", "Notieren" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/2EALxXDw";
                    break;
            }
            //Check if installation was successfull
            if (methodsClass.checkIfGameInstalled(Path.GetDirectoryName(steamCmdSpeicherort), serverFolderName) == true)
            {
                if (MessageBox.Show(askUserToCreateConfigAndBatchFileMsgBox[0], askUserToCreateConfigAndBatchFileMsgBox[1], MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        //Batch file
                        System.IO.File.WriteAllText(serverFolderpath + "\\Run.bat", "start srcds.exe -console -game left4dead -port 27015 +map l4d_hospital01_apartment +mp_gamemode coop");
                        //DOWNLOAD CONFIG FILE AND WRITE IT
                        WebClient client = new WebClient();
                        Stream stream = client.OpenRead(configFileURLforEachLangouage);
                        StreamReader reader = new StreamReader(stream);
                        String content = reader.ReadToEnd();
                        System.IO.Directory.CreateDirectory(serverFolderpath + @"\left4dead\cfg");
                        System.IO.File.WriteAllText(serverFolderpath + @"\left4dead\cfg\server.cfg", content);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(failedToCreateServerStartupFilesMsgBox[0], failedToCreateServerStartupFilesMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    MessageBox.Show(importantNoteMsgBox[0], importantNoteMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                Process.Start(serverFolderpath);
            }
            else
            {
                MessageBox.Show(installationFailedMsgBox[0], installationFailedMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //-----------------Rust-----------------
        public void finishInstallationRust(string steamCmdSpeicherort, string serverFolderName)
        {
            string serverFolderpath = Path.GetDirectoryName(steamCmdSpeicherort) + @"\" + serverFolderName;
            //Check Langouage

            string[] askUserToCreateConfigAndBatchFileMsgBox = null;
            string[] failedToCreateServerStartupFilesMsgBox = null;
            string[] installationFailedMsgBox = null;
            string[] importantNoteMsgBox = null;
            string configFileURLforEachLangouage = null;
            switch (Properties.Settings.Default.language)
            {
                //Second value of each variable is the title of the messagebox and the first value is the message
                case "english":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { @"You have to create a batch file and a configuration file in order to start the server. You want it to be created automatically or create it yourself? The Batch file will be created in the server's ROOT folder and the configuration file will be created at: " + serverFolderpath + @"\server\my_server_identity\cfg\server.cfg", "Create server's startup files" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Failed to create the config and batch files! You can create the files yourself using the code in the server's guide", "Create server's config and batch files" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Rust Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the batch and config file later to change player slots, map, password etc. For more info, go to the Rust Server guide", "Note" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/7AAK7itF";
                    break;

                case "german":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { "Eine Batch Datei wird benötigt, um den Server zu starten, sowie eine server.cfg. Sollen diese automatisch erstellt werden, oder nicht? Die Batch Datei wird im Server Verzeichnis erstellt und die Konfigurationsdatei in: " + serverFolderpath + @"\server\my_server_identity\cfg\server.cfg", "Erstellen Sie die Startdateien des Servers" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Erstellung der Konfigurations- und Batchdatei fehlgeschlagen.! Sie können diese Dateien mithilfe des Codes des Server-Guides selbst erstellen.", "Erstelle Server Konfigurationsdatei, sowie die Batch Datei." };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Sie Rust Server" };
                    importantNoteMsgBox = new string[] { "WICHTIG: Du kannst diese Dateien später noch editieren. Für mehr Informationen, öffne den Rust Server Guide.", "Notieren" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/7AAK7itF";
                    break;
            }
            //Check if installation was successfull
            if (methodsClass.checkIfGameInstalled(Path.GetDirectoryName(steamCmdSpeicherort), serverFolderName) == true)
            {
                if (MessageBox.Show(askUserToCreateConfigAndBatchFileMsgBox[0], askUserToCreateConfigAndBatchFileMsgBox[1], MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        //Batch file
                        System.IO.File.WriteAllText(serverFolderpath + "\\Run.bat", "start RustDedicated.exe -batchmode +server.port 28015 +server.level " + @"""Procedural Map""" + " server.seed 1234 +server.worldsize 4000 +server.maxplayers 10");
                        //DOWNLOAD CONFIG FILE AND WRITE IT
                        WebClient client = new WebClient();
                        Stream stream = client.OpenRead(configFileURLforEachLangouage);
                        StreamReader reader = new StreamReader(stream);
                        String content = reader.ReadToEnd();
                        System.IO.Directory.CreateDirectory(serverFolderpath + @"\server\my_server_identity\cfg");
                        System.IO.File.WriteAllText(serverFolderpath + @"\server\my_server_identity\cfg\server.cfg", content);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(failedToCreateServerStartupFilesMsgBox[0], failedToCreateServerStartupFilesMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    MessageBox.Show(importantNoteMsgBox[0], importantNoteMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                Process.Start(serverFolderpath);
            }
            else
            {
                MessageBox.Show(installationFailedMsgBox[0], installationFailedMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //-----------------ARK------------------
        public void finishInstallationARK(string steamCmdSpeicherort, string serverFolderName)
        {
            string serverFolderpath = Path.GetDirectoryName(steamCmdSpeicherort) + @"\" + serverFolderName;
            //Check Langouage

            string[] askUserToCreateConfigAndBatchFileMsgBox = null;
            string[] failedToCreateServerStartupFilesMsgBox = null;
            string[] installationFailedMsgBox = null;
            string[] importantNoteMsgBox = null;

            switch (Properties.Settings.Default.language)
            {
                //Second value of each variable is the title of the messagebox and the first value is the message
                case "english":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { @"You have to create a batch file in order to start the server.You want it to be created automatically or create it yourself? The Batch file will be created at: " + serverFolderpath + @"\ShooterGame\Binaries\Win64\Run.bat", "Create server's startup files" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Failed to create the config and batch files! You can create the files yourself using the code in the server's guide", "Create server's config and batch files" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Ark: Survival Evolved Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the batch and config file later to change player slots, map, password etc. The config file is located at:" + serverFolderpath + @"\ShooterGame\Saved\Config\WindowsServer\GameUserSettings.ini .For more info, go to the Ark:Survival Evolved Server guide", "Note" };
                    break;

                case "german":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { "Eine Batch Datei wird benötigt, um den Server zu starten. Sollen diese automatisch erstellt werden, oder nicht? Die Batch Datei befindet sich in: " + serverFolderpath + @"\ShooterGame\Binaries\Win64\Run.bat", "Erstellen Sie die Startdateien des Servers" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Erstellung der Konfigurations- und Batchdatei fehlgeschlagen.! Sie können diese Dateien mithilfe des Codes des Server-Guides selbst erstellen.", "Erstelle Server Konfigurationsdatei, sowie die Batch Datei." };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Sie Ark:Survival Evolved Server" };
                    importantNoteMsgBox = new string[] { "WICHTIG: Du kannst diese Dateien später noch editieren.Die Konfiguration findest du hier: " + serverFolderpath + @"\ShooterGame\Saved\Config\WindowsServer\GameUserSettings.ini. Für mehr Informationen, öffne den Ark:Survival Evolved.", "Notieren" };
                    break;
            }
            //Check if installation was successfull
            if (methodsClass.checkIfGameInstalled(Path.GetDirectoryName(steamCmdSpeicherort), serverFolderName) == true)
            {
                if (MessageBox.Show(askUserToCreateConfigAndBatchFileMsgBox[0], askUserToCreateConfigAndBatchFileMsgBox[1], MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        //Batch file
                        System.IO.File.WriteAllText(serverFolderpath + @"\ShooterGame\Binaries\Win64\Run.bat", "start ShooterGameServer " + @"""TheIsland?listen?SessionName=Mein_Server?ServerPassword=TEST?ServerAdminPassword=RCONADMIN?MaxPlayers=50""" + Environment.NewLine + "exit");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(failedToCreateServerStartupFilesMsgBox[0], failedToCreateServerStartupFilesMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    MessageBox.Show(importantNoteMsgBox[0], importantNoteMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                Process.Start(serverFolderpath + @"\ShooterGame\Binaries\Win64");
            }
            else
            {
                MessageBox.Show(installationFailedMsgBox[0], installationFailedMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //-----------------7 Days to die-----------------
        public void finishInstallation7days(string steamCmdSpeicherort, string serverFolderName)
        {
            string serverFolderpath = Path.GetDirectoryName(steamCmdSpeicherort) + @"\" + serverFolderName;
            //Check Language
            string[] startServerInfo = null;
            string[] installationFailedMsgBox = null;
            string[] importantNoteMsgBox = null;
            switch (Properties.Settings.Default.language)
            {
                //Second value of each variable is the title of the messagebox and the first value is the message
                case "english":
                    startServerInfo = new string[] { "You can start the server from here: " + serverFolderpath + @"\startdedicated.bat", "7 Days to Die Server" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install 7 Days to Die Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the config file at: " + serverFolderpath + @"\serverconfig.xml", "7 Days to Die Server" };
                    break;
                case "german":
                    startServerInfo = new string[] { "Du kannst den Server hier ausführen:" + serverFolderpath + @"\startdedicated.bat", "Installieren 7 Days to Die Server" };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren 7 Days to Die Server" };
                    importantNoteMsgBox = new string[] { "Du findest die Config Datei hier: " + serverFolderpath + @"\serverconfig.xml", "Installieren 7 Days to Die Server" };
                    break;
            }
            //Check if installation was successfull
            if (methodsClass.checkIfGameInstalled(Path.GetDirectoryName(steamCmdSpeicherort), serverFolderName) == true)
            {
                MessageBox.Show(startServerInfo[0], startServerInfo[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(importantNoteMsgBox[0], importantNoteMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(serverFolderpath);
            }
            else
            {
                MessageBox.Show(installationFailedMsgBox[0], installationFailedMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
