using Server_Creation_Tool.myClasses;
using statServer_Creation_Tool;
using System;
using System.Text;
using System.Windows.Forms;


namespace Server_Creation_Tool
{
    class steamGameInstallFunc
    {
        METHODSandFUNCTIONS methClass = new METHODSandFUNCTIONS();
        global_Variables gVars = new global_Variables();

        //--------------check if a game is installed--------------
        public double srvDirSize;
        public bool checkIfGameInstalledSTEAM(string steamCMDandServersFolder, string gameFolder)
        {
            switch (gameFolder)
            {
                case "css":
                    //Counter Strike Source
                    srvDirSize = methClass.ShowFolderSize(steamCMDandServersFolder + @"\" + gVars.cssRootFold);
                    if (System.IO.Directory.Exists(steamCMDandServersFolder + @"\" + gVars.cssRootFold + @"\cstrike") && System.IO.Directory.Exists(steamCMDandServersFolder + @"\" + gVars.cssRootFold + @"\hl2") && System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.cssRootFold + @"\srcds.exe") && srvDirSize > gVars.cssFoldSize)
                    { return true; }
                    else { return false; }

                case "sven":
                    //Sven Coop
                    srvDirSize = methClass.ShowFolderSize(steamCMDandServersFolder + @"\" + gVars.svenRootFold);
                    if (System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.svenRootFold + @"\svends.exe") && System.IO.Directory.Exists(steamCMDandServersFolder + @"\" + gVars.svenRootFold + @"\svencoop") && System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.svenRootFold + @"\steamclient.dll") && srvDirSize > gVars.svenFoldSize)
                    { return true; }
                    else { return false; }

                case "ark":
                    //ARK: Survival Evolved
                    srvDirSize = methClass.ShowFolderSize(steamCMDandServersFolder + @"\" + gVars.arkRootFold);
                    if (System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.arkRootFold + @"\ShooterGame\Binaries\Win64\ShooterGameServer.exe") && System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.arkRootFold + @"\steamclient.dll") && srvDirSize > gVars.arkFoldSize)
                    { return true; }
                    else { return false; }

                case "csgo":
                    //CS:GO
                    srvDirSize = methClass.ShowFolderSize(steamCMDandServersFolder + @"\" + gVars.csgoRootFold);
                    if (System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.csgoRootFold + @"\srcds.exe") && System.IO.Directory.Exists(steamCMDandServersFolder + @"\" + gVars.csgoRootFold + @"\csgo") && System.IO.Directory.Exists(steamCMDandServersFolder + @"\" + gVars.csgoRootFold + @"\platform") && srvDirSize > gVars.csgoFoldSize)
                    { return true; }
                    else { return false; }

                case "l4d":
                    //Left 4 dead
                    srvDirSize = methClass.ShowFolderSize(steamCMDandServersFolder + @"\" + gVars.l4dRootFold);
                    if (System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.l4dRootFold + @"\srcds.exe") & System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.l4dRootFold + @"\left4dead.exe") && System.IO.Directory.Exists(steamCMDandServersFolder + @"\" + gVars.l4dRootFold + @"\left4dead") && System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.l4dRootFold + @"\steamclient.dll") && System.IO.Directory.Exists(steamCMDandServersFolder + @"\" + gVars.l4dRootFold + @"\hl2") && srvDirSize > gVars.l4dFoldSize)
                    { return true; }
                    else { return false; }

                case "gmod":
                    //Garry's Mod
                    srvDirSize = methClass.ShowFolderSize(steamCMDandServersFolder + @"\" + gVars.gmodRootFold);
                    if (System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.gmodRootFold + @"\srcds.exe") && System.IO.Directory.Exists(steamCMDandServersFolder + @"\" + gVars.gmodRootFold + @"\garrysmod") && System.IO.Directory.Exists(steamCMDandServersFolder + @"\" + gVars.gmodRootFold + @"\sourceengine") && srvDirSize > gVars.gmodFoldSize)
                    { return true; }
                    else { return false; }

                case "l4d2":
                    //Left 4 Dead 2
                    srvDirSize = methClass.ShowFolderSize(steamCMDandServersFolder + @"\" + gVars.l4d2RootFold);
                    if (System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.l4d2RootFold + @"\srcds.exe") && System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.l4d2RootFold + @"\left4dead2.exe") && System.IO.Directory.Exists(steamCMDandServersFolder + @"\" + gVars.l4d2RootFold + @"\left4dead2") && System.IO.Directory.Exists(steamCMDandServersFolder + @"\" + gVars.l4d2RootFold + @"\hl2") && srvDirSize > gVars.l4d2FoldSize)
                    { return true; }
                    else { return false; }

                case "hurtworld":
                    //HurtWorld
                    srvDirSize = methClass.ShowFolderSize(steamCMDandServersFolder + @"\" + gVars.hurtworldRootFold);
                    if (System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.hurtworldRootFold + @"\Hurtworld.exe") && System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.hurtworldRootFold + @"\steamclient.dll") && System.IO.Directory.Exists(steamCMDandServersFolder + @"\" + gVars.hurtworldRootFold + @"\Hurtworld_Data") && System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.hurtworldRootFold + @"\host.bat") && srvDirSize > gVars.hurtworldFoldSize)
                    { return true; }
                    else { return false; }

                case "cs":
                    //Couter Strike 1.6
                    srvDirSize = methClass.ShowFolderSize(steamCMDandServersFolder + @"\" + gVars.cs16RootFold);
                    if (System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.cs16RootFold + @"\hlds.exe") && System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.cs16RootFold + @"\steamclient.dll") && System.IO.Directory.Exists(steamCMDandServersFolder + @"\" + gVars.cs16RootFold + @"\platform") && System.IO.Directory.Exists(steamCMDandServersFolder + @"\" + gVars.cs16RootFold + @"\cstrike") && srvDirSize > gVars.cs16FoldSize)
                    { return true; }
                    else { return false; }

                case "rust":
                    //Rust
                    srvDirSize = methClass.ShowFolderSize(steamCMDandServersFolder + @"\" + gVars.rustRootFold);
                    if (System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.rustRootFold + @"\RustDedicated.exe") && System.IO.Directory.Exists(steamCMDandServersFolder + @"\" + gVars.rustRootFold + @"\RustDedicated_Data") && srvDirSize > gVars.rustFoldSize)
                    { return true; }
                    else { return false; }

                case "7days":
                    //7 days to die
                    srvDirSize = methClass.ShowFolderSize(steamCMDandServersFolder + @"\" + gVars.days7RootFold);
                    if (System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.days7RootFold + @"\" + gVars.days7StartFilesLoc[0]) && srvDirSize > gVars.days7FoldSize)
                    { return true; }
                    else { return false; }

                case "bo3":
                    //CoD Black Ops 3
                    srvDirSize = methClass.ShowFolderSize(steamCMDandServersFolder + @"\" + gVars.bo3RootFold);
                    if (System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.bo3RootFold + @"\UnrankedServer\Launch_Server.bat") && srvDirSize > gVars.bo3FoldSize)
                    { return true; }
                    else { return false; }

                case "kf2":
                    //Killing Floor 2
                    srvDirSize = methClass.ShowFolderSize(steamCMDandServersFolder + @"\" + gVars.kf2RootFold);
                    if (System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.kf2RootFold + @"\KF2Server.bat") && System.IO.File.Exists(steamCMDandServersFolder + @"\" + gVars.kf2RootFold + @"\steamclient.dll") && System.IO.Directory.Exists(steamCMDandServersFolder + @"\" + gVars.kf2RootFold + @"\Engine") && srvDirSize > gVars.kf2FoldSize)
                    { return true; }
                    else { return false; }

                case "synergy":
                    //Synergy
                    if (System.IO.File.Exists(""))
                    { return true; }
                    else { return false; }
            }
            return false;
        }
        //-------------------------------------------------------



        //________________________________LOAD SERVER INSTALLATION FINAL STEPS____________________________________///
        string installFailMsg;
        public string[] cfgFail;
        public string[] batFail;
        public string errorTitle;
        public string[] importantNoteMsgBox;
        public string askCreateCfgFile;
        public void setMsgLang()
        {
            int language = methClass.getLangNum();
            if (language == 0)
            {
                installFailMsg = "Installation failed! Please retry the installation or join our steam group for help";
                errorTitle = "Error";
                cfgFail = new string[] { "Could not create the .cfg file! Close any other programs that might be using it or restart your PC and then retry. You can also create it manually by visiting the server's installation guide", "Could not create the .cfg file!" };
                batFail = new string[] { "Could not create the .bat file! Close any other programs that might be using it or restart your PC and then retry. You can also create it manually by visiting the server's installation guide", "Could not create the .bat file!" };
                importantNoteMsgBox = new string[] { "Done! For information and help, visit the ", " installation guide or join our steam Group chat" };
                askCreateCfgFile = "Are you sure that you want to create a new premade .cfg file? This file will have some settings preconfigured and explanations on how they work. Continuing will overwrite any existing file.";
            }
            else if (language == 1)
            {
                installFailMsg = "Installation fehlgeschlagen! Bitte versuche es erneut oder tritt unserer Steam Gruppe für Hilfe bei.";
                errorTitle = "Fehler";
                cfgFail = new string[] { "Die CFG Datei konnte nicht erstellt werden! Beende andere Programme, die gearde eventuell darauf zugreifen könnten oder starte deinen PC neu und versuche es erneut. Die Datei kann auch selbst erstellt werden anhand des Server Guides.", "" };
                batFail = new string[] { "Die BAT Datei konnte nicht erstellt werden! Beende andere Programme, die gearde eventuell darauf zugreifen könnten oder starte deinen PC neu und versuche es erneut. Die Datei kann auch selbst erstellt werden anhand des Server Guides.", "" };
                importantNoteMsgBox = new string[] { "Fertig! Für weitere Informationen und Hilfe, besuche ", " Installations Guide oder tritt unserer Steam Gruppe bei." };
                askCreateCfgFile = "Bist du dir sicher, dass du eine neue fertige Konfigurationsdatei erstellen willst? Diese Datei wird voreingestellt sein und erklären, wofür welcher Befehl steht. Fortfahren wird die alte Daten überschreiben!";
            }
        }
        public bool finishInstallationSTEAM(string steamCMDandServersFolder, string serverFolderName)
        {
            setMsgLang();
            //check if server was installed
            if (checkIfGameInstalledSTEAM(steamCMDandServersFolder, serverFolderName) != true)
            { Elegant.Ui.MessageBox.Show(installFailMsg, errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }

            //here is a switch statement. it works like the IF statement
            switch (serverFolderName)
            {
                //COUNTER STRIKE 1.6
                case "cs":
                    finishInstallationCS16(steamCMDandServersFolder, serverFolderName);
                    return true;
                //GMOD
                case "gmod":
                    finishInstallationGmod(steamCMDandServersFolder, serverFolderName);
                    return true;
                //LEFT 4 DEAD 2
                case "l4d2":
                    finishInstallationL4d2(steamCMDandServersFolder, serverFolderName);
                    return true;
                //COUNTER STRIKE SOURCE
                case "css":
                    finishInstallationCSSOURCE(steamCMDandServersFolder, serverFolderName);
                    return true;
                //CS:GO
                case "csgo":
                    finishInstallationCSGO(steamCMDandServersFolder, serverFolderName);
                    return true;
                //Sven Coop
                case "svencoop":
                    finishInstallationSvenCoop(steamCMDandServersFolder, serverFolderName);
                    return true;
                //Call of Duty Black ops 3
                case "bo3":
                    finishInstallationCodBO3(steamCMDandServersFolder, serverFolderName);
                    return true;
                //Rust
                case "rust":
                    finishInstallationRust(steamCMDandServersFolder, serverFolderName);
                    return true;
                //Killing floor 2
                case "kf2":
                    finishInstallationKf2(steamCMDandServersFolder, serverFolderName);
                    return true;
                //Left 4 dead
                case "l4d":
                    finishInstallationL4d(steamCMDandServersFolder, serverFolderName);
                    return true;
                //ARK
                case "ark":
                    finishInstallationARK(steamCMDandServersFolder, serverFolderName);
                    return true;
                //7 days to die
                case "7days":
                    finishInstallation7days(steamCMDandServersFolder, serverFolderName);
                    return true;
                //Hurtworld
                case "hurtworld":
                    finishInstallationHurtworld(steamCMDandServersFolder, serverFolderName);
                    return true;
                //Synergy
                case "synergy":
                    //EMPTY
                    return true;

            }
            return false;
        }


        //-----------------Counter Strike 1.6---------------
        public void finishInstallationCS16(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            MessageBox.Show(importantNoteMsgBox[0], importantNoteMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------GMOD----------------------
        public void finishInstallationGmod(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            createBatFlGMOD(serverFolderpath);
            MessageBox.Show(importantNoteMsgBox[0] + "Garry's Mod" + importantNoteMsgBox[1], "Garry's Mod", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------Left 4 Dead 2------------------------
        public void finishInstallationL4d2(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            createBatFll4d2(serverFolderpath);
            MessageBox.Show(importantNoteMsgBox[0] + "Left 4 Dead 2" + importantNoteMsgBox[1], "Left 4 Dead 2", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------Counter Strike: Source--------------------
        public void finishInstallationCSSOURCE(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            createBatFlCSS(serverFolderpath);
            MessageBox.Show(importantNoteMsgBox[0] + "Counter Strike: Source" + importantNoteMsgBox[1], "Counter Strike: Source", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------CS:GO----------------------
        public void finishInstallationCSGO(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            createBatFlCSGO(serverFolderpath);
            MessageBox.Show(importantNoteMsgBox[0] + "Counter Strike: Global Offensive" + importantNoteMsgBox[1], "Counter Strike: Global Offensive", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------Sven Coop----------------------
        public void finishInstallationSvenCoop(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            createBatFlSven(serverFolderpath);
            MessageBox.Show(importantNoteMsgBox[0] + "Sven Coop" + importantNoteMsgBox[1], "Sven Coop", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------Call Of Duty Black Ops 3-------------------
        public void finishInstallationCodBO3(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            MessageBox.Show(importantNoteMsgBox[0] + "Call of Duty: Black Ops 3" + importantNoteMsgBox[1], "Call of Duty: Black Ops 3", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------Hurtworld--------------------
        public void finishInstallationHurtworld(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            MessageBox.Show(importantNoteMsgBox[0] + "Hurtworld" + importantNoteMsgBox[1], "Hurtworld", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------Killing Floor 2--------------------
        public void finishInstallationKf2(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            MessageBox.Show(importantNoteMsgBox[0] + "Killing Floor 2" + importantNoteMsgBox[1], "Killing Floor 2", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------Left 4 Dead-------------------
        public void finishInstallationL4d(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            createBatFll4d(serverFolderpath);
            MessageBox.Show(importantNoteMsgBox[0] + "Left 4 Dead" + importantNoteMsgBox[1], "Left 4 Dead", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------Rust-----------------
        public void finishInstallationRust(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            createCfgFlRust(serverFolderpath, gVars.rustCfgFilelink[methClass.getLangNum()]);
            MessageBox.Show(importantNoteMsgBox[0] + "Rust" + importantNoteMsgBox[1], "Rust", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //-----------------ARK------------------
        public void finishInstallationARK(string steamCMDandServersFolder, string serverFolderName)
        {
            string serverFolderpath = steamCMDandServersFolder + @"\" + serverFolderName;
            createBatFlARK(serverFolderpath);
            MessageBox.Show(importantNoteMsgBox[0] + "ARK: Survival Evolved" + importantNoteMsgBox[1], "ARK: Survival Evolved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //-----------------7 Days to die-----------------
        public void finishInstallation7days(string steamCMDandServersFolder, string serverFolderName)
        {
            MessageBox.Show(importantNoteMsgBox[0] + "7 days to die" + importantNoteMsgBox[1], "7 days to die", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }




        #region additonal files functions
        //------------------------Create additional files-------------------------
        public bool createBatFlARK(string serverFolderpath)
        {
            try  //Batch file
            { System.IO.File.WriteAllText(serverFolderpath + gVars.arkConfFilesLoc[0], "start ShooterGameServer " + @"""TheIsland?listen?SessionName=Mein_Server?ServerPassword=TEST?ServerAdminPassword=RCONADMIN?MaxPlayers=50""" + Environment.NewLine + "exit"); return true; }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(batFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createCfgFlRust(string serverFolderpath, string cfgLink, bool ask = false, bool notify = false)
        {
            try
            {
                if (ask) { if (Elegant.Ui.MessageBox.Show(askCreateCfgFile, "Rust", Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Question) != DialogResult.Yes) { return false; } }
                if (!methClass.downloadnWriteFile(cfgLink, serverFolderpath + gVars.rustConfFilesLoc[1]))
                { Elegant.Ui.MessageBox.Show(cfgFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
                if (notify)
                { Elegant.Ui.MessageBox.Show("Done!", "Rust", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Information); }
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(cfgFail[0], errorTitle); return false; }
        }
        public bool createBatFlRust(string serverFolderpath)
        {
            try
            {
                //Batch file
                System.IO.File.WriteAllText(serverFolderpath + gVars.rustConfFilesLoc[0], "start RustDedicated.exe -batchmode +server.port 28015 +server.level " + @"""Procedural Map""" + " server.seed 1234 +server.worldsize 4000 +server.maxplayers 10");
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(batFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createCfgFll4d(string serverFolderpath, string cfgLink, bool ask = false, bool notify = false)
        {
            try
            {
                if (ask) { if (Elegant.Ui.MessageBox.Show(askCreateCfgFile, "Left 4 Dead", Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Question) != DialogResult.Yes) { return false; } }
                if (!methClass.downloadnWriteFile(cfgLink, serverFolderpath + gVars.l4dConfFilesLoc[1]))
                { Elegant.Ui.MessageBox.Show(cfgFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
                if (notify)
                { Elegant.Ui.MessageBox.Show("Done!", "Left 4 Dead", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Information); }
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(cfgFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createBatFll4d(string serverFolderpath)
        {
            try
            {
                System.IO.File.WriteAllText(serverFolderpath + gVars.l4dConfFilesLoc[0], "start srcds.exe -console -game left4dead -port 27015 +map l4d_hospital01_apartment +mp_gamemode coop");
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(batFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createCfgFll4d2(string serverFolderpath, string cfgLink, bool ask = false, bool notify = false)
        {
            try
            {
                if (ask) { if (Elegant.Ui.MessageBox.Show(askCreateCfgFile, "Left 4 Dead 2", Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Question) != DialogResult.Yes) { return false; } }
                if (!methClass.downloadnWriteFile(cfgLink, serverFolderpath + gVars.l4d2ConfFilesLoc[1]))
                { Elegant.Ui.MessageBox.Show(cfgFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
                if (notify)
                { Elegant.Ui.MessageBox.Show("Done!", "Left 4 Dead 2", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Information); }
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(cfgFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createBatFll4d2(string serverFolderpath)
        {
            try
            {
                System.IO.File.WriteAllText(serverFolderpath + gVars.l4d2ConfFilesLoc[0], "start srcds.exe -console -game left4dead2 -secure +maxplayers 4  +map c1m1_hotel");
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(batFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createBatFlSven(string serverFolderpath)
        {
            try
            {
                System.IO.File.WriteAllText(serverFolderpath + gVars.svenConfFilesLoc[0], "start SvenDS -console -port 27015 +maxplayers 8 -insecure -nomaster +sv_lan 1 +map _server_start");
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(batFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createCfgFlCSS(string serverFolderpath, string cfgLink, bool ask = false, bool notify = false)
        {
            try
            {
                if (ask) { if (Elegant.Ui.MessageBox.Show(askCreateCfgFile, "Counter Strike: Source", Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Question) != DialogResult.Yes) { return false; } }
                if (!methClass.downloadnWriteFile(cfgLink, serverFolderpath + gVars.cssConfFilesLoc[1]))
                { Elegant.Ui.MessageBox.Show(cfgFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
                if (notify)
                { Elegant.Ui.MessageBox.Show("Done!", "Counter Strike: Source", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Information); }
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(cfgFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createBatFlCSS(string serverFolderpath)
        {
            try
            {
                System.IO.File.WriteAllText(serverFolderpath + gVars.cssConfFilesLoc[0], "start srcds.exe -console -game cstrike -secure +maxplayers 22 +map de_dust");
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(batFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createCfgFlCSGO(string serverFolderpath, string cfgLink, bool ask = false, bool notify = false)
        {
            try
            {
                if (ask) { if (Elegant.Ui.MessageBox.Show(askCreateCfgFile, "Counter Strike: Global Offensive", Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Question) != DialogResult.Yes) { return false; } }
                if (!methClass.downloadnWriteFile(cfgLink, serverFolderpath + gVars.csgoConfFilesLoc[1]))
                { Elegant.Ui.MessageBox.Show(cfgFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
                if (notify)
                { Elegant.Ui.MessageBox.Show("Done!", "Counter Strike: Global Offensive", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Information); }
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(cfgFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
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
                System.IO.File.WriteAllText(serverFolderpath + gVars.csgoConfFilesLoc[0], "start srcds -game csgo -console -usercon " + gamemodeBatchCommand + " -maxplayers_override 24");
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(batFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createCfgFlGMOD(string serverFolderpath, string cfgLink, bool ask = false, bool notify = false)
        {
            try
            {
                if (ask) { if (Elegant.Ui.MessageBox.Show(askCreateCfgFile, "Garry's Mod", Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Question) != DialogResult.Yes) { return false; } }
                if (!methClass.downloadnWriteFile(cfgLink, serverFolderpath + gVars.gmodConfFilesLoc[1]))
                { Elegant.Ui.MessageBox.Show(cfgFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
                if (notify)
                { Elegant.Ui.MessageBox.Show("Done!", "Garry's Mod", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Information); }
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(cfgFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createBatFlGMOD(string serverFolderpath)
        {
            try
            {
                System.IO.File.WriteAllText(serverFolderpath + gVars.l4d2ConfFilesLoc[0], "start srcds.exe -console -game cstrike -secure +maxplayers 22  +map gm_flatgrass");
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(batFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        public bool createCfgFlCS16(string serverFolderpath, string cfgLink, bool ask = false, bool notify = false)
        {
            try
            {
                if (ask) { if (Elegant.Ui.MessageBox.Show(askCreateCfgFile, "Counter Strike 1.6", Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Question) != DialogResult.Yes) { return false; } }
                if (!methClass.downloadnWriteFile(cfgLink, serverFolderpath + gVars.cs16ConfFilesLoc[1]))
                { Elegant.Ui.MessageBox.Show(cfgFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
                if (notify)
                { Elegant.Ui.MessageBox.Show("Done!", "Counter Strike 1.6", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Information); }
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(cfgFail[0], errorTitle, Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); return false; }
        }
        #endregion
    }
}
