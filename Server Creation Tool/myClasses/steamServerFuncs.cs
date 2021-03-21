using Server_Creation_Tool.myClasses;
using statServer_Creation_Tool;
using System;
using System.Windows.Forms;


namespace Server_Creation_Tool
{
    class steamGameInstallFunc
    {
        METHODSandFUNCTIONS methClass = new METHODSandFUNCTIONS();
        global_Variables gVars = new global_Variables();

        //--------------check if a game is installed--------------
      //  public double srvDirSize;
        public bool checkIfGameInstalledSTEAM(string steamCMDandServersFolder, string gameFolder)
        {
            string gamefoldPath = steamCMDandServersFolder + @"\" + gameFolder;
            if (System.IO.Directory.Exists(gamefoldPath))
            { return true; }
            else { return false; }

        }
        //-------------------------------------------------------



        //-----------------Counter Strike 1.6---------------
        public void finishInstallationCS16(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            MessageBox.Show(lang.done + lang.instFinishInfo[gVars.lgNum], "Counter Strike 1.6", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------GMOD----------------------
        public void finishInstallationGmod(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            createBatFlGMOD(serverFolderpath);
            MessageBox.Show(lang.done + lang.instFinishInfo[gVars.lgNum], "Garry's Mod", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------Left 4 Dead 2------------------------
        public void finishInstallationL4d2(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            createBatFll4d2(serverFolderpath);
            MessageBox.Show(lang.done + lang.instFinishInfo[gVars.lgNum], "Left 4 Dead 2", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------Counter Strike: Source--------------------
        public void finishInstallationCSSOURCE(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            createBatFlCSS(serverFolderpath);
            MessageBox.Show(lang.done + lang.instFinishInfo[gVars.lgNum], "Counter Strike: Source", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------CS:GO----------------------
        public void finishInstallationCSGO(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            createBatFlCSGO(serverFolderpath);
            MessageBox.Show(lang.done + lang.instFinishInfo[gVars.lgNum], "Counter Strike: Global Offensive", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------Sven Coop----------------------
        public void finishInstallationSvenCoop(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            createBatFlSven(serverFolderpath);
            MessageBox.Show(lang.done + lang.instFinishInfo[gVars.lgNum], "Sven Coop", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------Call Of Duty Black Ops 3-------------------
        public void finishInstallationCodBO3(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            MessageBox.Show(lang.done + lang.instFinishInfo[gVars.lgNum], "Call of Duty: Black Ops 3", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------Hurtworld--------------------
        public void finishInstallationHurtworld(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            MessageBox.Show(lang.done + lang.instFinishInfo[gVars.lgNum], "Hurtworld", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------Killing Floor 2--------------------
        public void finishInstallationKf2(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            MessageBox.Show(lang.done + lang.instFinishInfo[gVars.lgNum], "Killing Floor 2", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------Left 4 Dead-------------------
        public void finishInstallationL4d(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            createBatFll4d(serverFolderpath);
            MessageBox.Show(lang.done + lang.instFinishInfo[gVars.lgNum], "Left 4 Dead", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------Rust-----------------
        public void finishInstallationRust(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            createCfgFlRust(serverFolderpath, gVars.rustCfgFilelink[methClass.getLangNum()]);
            MessageBox.Show(lang.done + lang.instFinishInfo[gVars.lgNum], "Rust", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------ARK------------------
        public void finishInstallationARK(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            createBatFlARK(serverFolderpath);
            MessageBox.Show(lang.done + lang.instFinishInfo[gVars.lgNum], "ARK: Survival Evolved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //-----------------7 Days to die-----------------
        public void finishInstallation7days()
        {
            MessageBox.Show(lang.done + lang.instFinishInfo[gVars.lgNum], "7 days to die", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //-----------------Unturned-----------------
        public void finishInstallationUnturned()
        {
            MessageBox.Show(lang.done + lang.runUnturnedSrvOnceMsg[gVars.lgNum] + lang.instFinishInfo[gVars.lgNum], "Unturned", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //-----------------ATLAS-----------------
        public void finishInstallationATLAS()
        {
            MessageBox.Show(lang.done + lang.instFinishInfo[gVars.lgNum], "ATLAS", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        #region additonal files functions
        //------------------------Create additional files-------------------------
        public bool createBatFlARK(string serverFolderpath)
        {
            try  //Batch file
            { System.IO.File.WriteAllText(serverFolderpath + gVars.arkStartFilesLoc[0], "start ShooterGameServer " + @"""TheIsland?listen?SessionName=Mein_Server?ServerPassword=TEST?ServerAdminPassword=RCONADMIN?MaxPlayers=50""" + Environment.NewLine + "exit"); return true; }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(lang.batFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createCfgFlRust(string serverFolderpath, string cfgLink, bool ask = false, bool notify = false)
        {
            try
            {
                if (ask) { if (Elegant.Ui.MessageBox.Show(lang.askCreateCfgFile[gVars.lgNum], "Rust", Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Question) != DialogResult.Yes) { return false; } }
                if (!methClass.downloadnWriteFile(cfgLink, serverFolderpath + gVars.rustConfFilesLoc[1]))
                { Elegant.Ui.MessageBox.Show(lang.cfgFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
                if (notify)
                { Elegant.Ui.MessageBox.Show("Done!", "Rust", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Information); }
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(lang.cfgFail[gVars.lgNum], lang.error[gVars.lgNum]); return false; }
        }
        public bool createBatFlRust(string serverFolderpath)
        {
            try
            {
                //Batch file
                System.IO.File.WriteAllText(serverFolderpath + gVars.rustStartFilesLoc[0], "start RustDedicated.exe -batchmode +server.port 28015 +server.level " + @"""Procedural Map""" + " server.seed 1234 +server.worldsize 4000 +server.maxplayers 10");
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(lang.batFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createCfgFll4d(string serverFolderpath, string cfgLink, bool ask = false, bool notify = false)
        {
            try
            {
                if (ask) { if (Elegant.Ui.MessageBox.Show(lang.askCreateCfgFile[gVars.lgNum], "Left 4 Dead", Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Question) != DialogResult.Yes) { return false; } }
                if (!methClass.downloadnWriteFile(cfgLink, serverFolderpath + gVars.l4dConfFilesLoc[1]))
                { Elegant.Ui.MessageBox.Show(lang.cfgFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
                if (notify)
                { Elegant.Ui.MessageBox.Show("Done!", "Left 4 Dead", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Information); }
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(lang.cfgFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createBatFll4d(string serverFolderpath)
        {
            try
            {
                System.IO.File.WriteAllText(serverFolderpath + gVars.l4dStartFilesLoc[0], "start srcds.exe -console -game left4dead -port 27015 +map l4d_hospital01_apartment +mp_gamemode coop");
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(lang.batFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createCfgFll4d2(string serverFolderpath, string cfgLink, bool ask = false, bool notify = false)
        {
            try
            {
                if (ask) { if (Elegant.Ui.MessageBox.Show(lang.askCreateCfgFile[gVars.lgNum], "Left 4 Dead 2", Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Question) != DialogResult.Yes) { return false; } }
                if (!methClass.downloadnWriteFile(cfgLink, serverFolderpath + gVars.l4d2ConfFilesLoc[1]))
                { Elegant.Ui.MessageBox.Show(lang.cfgFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
                if (notify)
                { Elegant.Ui.MessageBox.Show("Done!", "Left 4 Dead 2", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Information); }
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(lang.cfgFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createBatFll4d2(string serverFolderpath)
        {
            try
            {
                System.IO.File.WriteAllText(serverFolderpath + gVars.l4d2StartFilesLoc[0], "start srcds.exe -console -game left4dead2 -secure +maxplayers 4  +map c1m1_hotel");
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(lang.batFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createBatFlSven(string serverFolderpath)
        {
            try
            {
                System.IO.File.WriteAllText(serverFolderpath + gVars.svenStartFilesLoc[0], "start SvenDS -console -port 27015 +maxplayers 8 -insecure -nomaster +sv_lan 1 +map _server_start");
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(lang.batFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createCfgFlCSS(string serverFolderpath, string cfgLink, bool ask = false, bool notify = false)
        {
            try
            {
                if (ask) { if (Elegant.Ui.MessageBox.Show(lang.askCreateCfgFile[gVars.lgNum], "Counter Strike: Source", Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Question) != DialogResult.Yes) { return false; } }
                if (!methClass.downloadnWriteFile(cfgLink, serverFolderpath + gVars.cssConfFilesLoc[1]))
                { Elegant.Ui.MessageBox.Show(lang.cfgFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
                if (notify)
                { Elegant.Ui.MessageBox.Show("Done!", "Counter Strike: Source", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Information); }
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(lang.cfgFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createBatFlCSS(string serverFolderpath)
        {
            try
            {
                System.IO.File.WriteAllText(serverFolderpath + gVars.cssStartFilesLoc[0], "start srcds.exe -console -game cstrike -secure +maxplayers 22 +map de_dust");
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(lang.batFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createCfgFlCSGO(string serverFolderpath, string cfgLink, bool ask = false, bool notify = false)
        {
            try
            {
                if (ask) { if (Elegant.Ui.MessageBox.Show(lang.askCreateCfgFile[gVars.lgNum], "Counter Strike: Global Offensive", Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Question) != DialogResult.Yes) { return false; } }
                if (!methClass.downloadnWriteFile(cfgLink, serverFolderpath + gVars.csgoConfFilesLoc[1]))
                { Elegant.Ui.MessageBox.Show(lang.cfgFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
                if (notify)
                { Elegant.Ui.MessageBox.Show("Done!", "Counter Strike: Global Offensive", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Information); }
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(lang.cfgFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createBatFlCSGO(string serverFolderpath)
        {
            Elegant.Ui.MessageBox.Show("Please note that the easy settings editor for this game is not fully completed. Its advised to edit the files manually to do more in depth changes", "Easy Settings Editor", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Exclamation);
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
                //Write batch file
                System.IO.File.WriteAllText(serverFolderpath + gVars.csgoStartFilesLoc[0], "start srcds -game csgo -console -usercon " + gamemodeBatchCommand + " -maxplayers_override 24");
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(lang.batFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createCfgFlGMOD(string serverFolderpath, string cfgLink, bool ask = false, bool notify = false)
        {
            try
            {
                if (ask) { if (Elegant.Ui.MessageBox.Show(lang.askCreateCfgFile[gVars.lgNum], "Garry's Mod", Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Question) != DialogResult.Yes) { return false; } }
                if (!methClass.downloadnWriteFile(cfgLink, serverFolderpath + gVars.gmodConfFilesLoc[1]))
                { Elegant.Ui.MessageBox.Show(lang.cfgFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
                if (notify)
                { Elegant.Ui.MessageBox.Show("Done!", "Garry's Mod", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Information); }
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(lang.cfgFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createBatFlGMOD(string serverFolderpath)
        {
            try
            {
                System.IO.File.WriteAllText(serverFolderpath + gVars.gmodStartFilesLoc[0], "start srcds.exe -console -game cstrike -secure +maxplayers 22  +map gm_flatgrass");
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(lang.batFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createCfgFlCS16(string serverFolderpath, string cfgLink, bool ask = false, bool notify = false)
        {
            try
            {
                if (ask) { if (Elegant.Ui.MessageBox.Show(lang.askCreateCfgFile[gVars.lgNum], "Counter Strike 1.6", Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Question) != DialogResult.Yes) { return false; } }
                if (!methClass.downloadnWriteFile(cfgLink, serverFolderpath + gVars.csConfFilesLoc[1]))
                { Elegant.Ui.MessageBox.Show(lang.cfgFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
                if (notify)
                { Elegant.Ui.MessageBox.Show("Done!", "Counter Strike 1.6", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Information); }
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(lang.cfgFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createBatFlUnturned(string serverFolderpath)
        {
            try
            {
                System.IO.File.WriteAllText(serverFolderpath + gVars.unturnedStartFilesLoc[0], @"start Unturned.exe - port:25444 - players:20 - nographics - pei - gold - nosync - pve - sv");
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(lang.batFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }

        #endregion
    }
}
