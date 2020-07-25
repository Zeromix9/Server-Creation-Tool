/*
 * Created by SharpDevelop.
 * User: Zeromix
 * Date: 28.05.2016
 * Time: 16:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using Microsoft.VisualBasic;
using statServer_Creation_Tool;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Windows.Forms;

namespace Server_Creation_Tool
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    ///
    public partial class MainForm : Form
    {
        //muss mal kurz googlen... war in letzter zeit nur in Java am entwickeln
        String steamCmdSpeicherort = null;
        Process installSrvProcess = new Process();
        string serverFolderName = null;

        //Create new instances of the classes
        METHODSandFUNCTIONS methodsClass = new METHODSandFUNCTIONS();
        gameFinishInstallFunc finInstallFunc = new gameFinishInstallFunc();
        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }
        string steamCMDFolder;
        PleaseWaitFrm pleaseWaitFrom = new PleaseWaitFrm();
        void Button1Click(object sender, EventArgs e)
        {


            if (methodsClass.HasInternet() == true)
            {
                using (System.Net.WebClient client = new System.Net.WebClient())
                {

                    // Create a new instance of FolderBrowserDialog.
                    FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                    //Check Language
                    if (Properties.Settings.Default.language == "english")
                    {
                        folderBrowserDialog.Description = "Please select where you want to download steamCMD:";
                    }

                    else if (Properties.Settings.Default.language == "german")
                    {
                        folderBrowserDialog.Description = "Wähle bitte aus, wohin die SteamCMD gedownloadet werden soll:";
                    }

                    // A new folder button will display in FolderBrowserDialog.
                    folderBrowserDialog.ShowNewFolderButton = true;
                    //Show FolderBrowserDialog
                    DialogResult dlgResult = folderBrowserDialog.ShowDialog();
                    if (dlgResult.Equals(DialogResult.OK))
                    {
                        //Send content to string
                        steamCMDFolder = folderBrowserDialog.SelectedPath;
                        MessageBox.Show(steamCMDFolder.ToString(), "Selected Path");
                        client.DownloadFileAsync(new Uri("https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip"), steamCMDFolder + "\\steamcmd.zip");
                        pleaseWaitFrom.Show();
                        this.Enabled = false;

                        steamCmdSpeicherort = Path.Combine(steamCMDFolder, "steamcmd.exe");
                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadSteamCmdZip);
                    }
                    else
                    {
                        //MessageBox.Show("No folder for storing the downloaded files selected. Can not continue.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Download failed. Please check your internet connection!", "Download SteamCMD", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        public void DownloadSteamCmdZip(object sender, AsyncCompletedEventArgs e)
        {

            //nun wurde die zip-Datei fertig gedownloadet.
            string steamCMDzip = Path.Combine(steamCMDFolder + @"\steamcmd.zip");
            //try to extract the zip file
            try
            {
                ZipFile.ExtractToDirectory(steamCMDzip, steamCMDFolder);
            }

            catch { }
            //try to delete the zip file
            try
            {
                File.Delete(steamCMDzip);
            }
            catch { }
            MessageBox.Show("Download completed!", "Download SteamCMD", MessageBoxButtons.OK, MessageBoxIcon.Information);
            pleaseWaitFrom.Hide();
            this.Enabled = true;
            this.BringToFront();
            if (File.Exists(steamCmdSpeicherort))
            {
                serverStartupFilesToolStripMenuItem.Enabled = true;
                groupBox1.Enabled = true;
                locateSteamCMDBtn.Text = "      SteamCMD Located";
                locateSteamCMDBtn.ForeColor = Color.Green;
                locateSteamCMDBtn.Image = Properties.Resources.icons8_checkmark_24;
                Properties.Settings.Default.lastSavedSteamCMDLoc = steamCmdSpeicherort;
                Properties.Settings.Default.Save();
                //   steamCmdSpeicherort = steamCMDFolder + @"\steamcmd.exe";
            }

        }

        public void ChangeLocateSteamCMDStatus(string englishErrorMessage, string MsgBoxtitle, string germanErrorMessage)
        {
            //Check Langouage
            if (Properties.Settings.Default.language == "english")
            {
                locateSteamCMDBtn.Text = "       Locate SteamCMD";
                MessageBox.Show(englishErrorMessage, MsgBoxtitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (Properties.Settings.Default.language == "german")
            {
                locateSteamCMDBtn.Text = "     Lokalisiere SteamCMD";
                MessageBox.Show(germanErrorMessage, MsgBoxtitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            groupBox1.Enabled = false;
            serverStartupFilesToolStripMenuItem.Enabled = false;
            locateSteamCMDBtn.ForeColor = Color.Red;
            locateSteamCMDBtn.Image = Properties.Resources.icons8_delete_24;
        }
        void Button2Click(object sender, EventArgs e)
        {
            serverFolderName = @"ark";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(arkBtn, sender, arkSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = Process.Start(steamCmdSpeicherort, Properties.Settings.Default.arkInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Ark: Survival Evolved Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }//ark

        void Button3Click(object sender, EventArgs e)
        {
            serverFolderName = @"css";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(CSsourceBtn, sender, CsSourceSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, Properties.Settings.Default.cssInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Counter Strike: Source Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }//Counter Strike: Source

        void Button4Click(object sender, EventArgs e)
        {
            serverFolderName = @"csgo";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(CSGOBtn, sender, csgoSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, Properties.Settings.Default.csgoInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Counter Strike: Global Offensive Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }//CS:GO

        void Button5Click(object sender, EventArgs e)
        {
            serverFolderName = @"cs";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(csOnePointsixBtn, sender, csOneSixSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = Process.Start(steamCmdSpeicherort, Properties.Settings.Default.cs16InstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Counter Strike 1.6 Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }//Counter Strike: 1.6

        void Button6Click(object sender, EventArgs e)
        {
            serverFolderName = @"gmod";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(garrysModBtn, sender, gmodSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = Process.Start(steamCmdSpeicherort, Properties.Settings.Default.gmodInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Garry's Mod Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }//Garry's Mod

        void Button7Click(object sender, EventArgs e)
        {
            try
            {
                if (OpenSteamCmd() == true)
                {
                    System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./7days/ +app_update 294420 validate");
                }
            }
            catch (Exception) {; } //7 Days to Die

        }

        void Button8Click(object sender, EventArgs e)
        {
            serverFolderName = @"kf2";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(KillingFloorTwoBtn, sender, Kf2SvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, Properties.Settings.Default.kf2InstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Killing Floor 2 Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }//Killing Floor 2

        void Button9Click(object sender, EventArgs e)
        {
            try
            {
                if (OpenSteamCmd() == true)
                {
                    System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./l4d/ +app_update 222840 validate");
                }
            }
            catch (Exception) {; } //Left 4 Dead

        }

        void Button10Click(object sender, EventArgs e)
        {
            serverFolderName = @"l4d2";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(l4dTwoBtn, sender, l4dTwoSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, Properties.Settings.Default.l4d2InstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Left 4 Dead 2 Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }//Left 4 Dead 2

        void Button11Click(object sender, EventArgs e)
        {
            serverFolderName = @"sven";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(SvenCoopBtn, sender, SvenSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, Properties.Settings.Default.svenInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Sven Coop Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }//Sven COOP

        void Button12Click(object sender, EventArgs e)
        {
            serverFolderName = @"rust";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(RustBtn, sender, RustSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, Properties.Settings.Default.rustInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Rust Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }//Rust

        void Button13Click(object sender, EventArgs e)
        {
            try
            {
                if (OpenSteamCmd() == true)
                {
                    System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./sniperelitev2/ +app_update 208050 validate");
                }
            }
            catch (Exception) {; } //Sniper Elite V2

        }

        void Button14Click(object sender, EventArgs e)
        {
            try
            {
                if (OpenSteamCmd() == true)
                {
                    System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./sniperelite3/ +app_update 266910 validate");
                }
            }
            catch (Exception) {; } //Sniper Elite 3
        }

        void Button15Click(object sender, EventArgs e)
        //DIS IS NOT WORKING.SYNERGY SERVER UNDER CONSTRUCTION
        {
            serverFolderName = @"synergy";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(synergyBtn, sender, SynergySvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = Process.Start(steamCmdSpeicherort, Properties.Settings.Default.synergyInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Synergy Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }//Synergy

        void Button16Click(object sender, EventArgs e)
        {
            try
            {
                if (OpenSteamCmd() == true)
                {
                    System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./tf2/ +app_update 232250 validate");
                }
            }
            catch (Exception) {; } //Team Fortress 2

        }


        void AboutToolStripMenuItem1Click(object sender, EventArgs e)
        {
            Environment.Exit(0); //Schließen des Programmes	
        }



        void ÖToolStripMenuItemClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=694475841");
            //ARK: Survival Evolved Guide English
        }

        void FToolStripMenuItemClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=397365275");
            //Counter-Strike: Source Guide English			
        }

        void ARKSurvivalEvolcvToolStripMenuItemClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=692129985");
            //ARK: Survival Evolved Guide German	
        }

        void CounterStrike16ToolStripMenuItemClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=710511353");
            //Counter-Strike 1.6 Guide German
        }

        void CounterStrikeSourceToolStripMenuItemClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=394849912");
            //Counter-Strike Source Guide German			
        }
        changeLogFrm changelogForm = new changeLogFrm();
        void ChangelogToolStripMenuItemClick(object sender, EventArgs e)
        {
            changelogForm.Show();
            changelogForm.BringToFront();
        }

        void GarrysModToolStripMenuItemClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=719731406");
            //Garry´s Mod Guide German	
        }

        void GarrysModToolStripMenuItem1Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=723644520");
            //Garry´s Mod Guide Englisch	
        }

        void CounterStrike16ToolStripMenuItem1Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=729345010");
            //Counter-Strike: 1.6 Guide Englisch
        }

        void CounterStrikeGlobalOffensiveToolStripMenuItemClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=711630778");
            //Counter-Strike: Global Offensive Guide German		
        }

        void CounterStrikeGlobalOffensiveToolStripMenuItem1Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=749588143");
            //Counter-Strike: Global Offensive Guide Englisch			
        }

        void FAQToolStripMenuItemClick(object sender, EventArgs e)
        {
            string[] msgBox = null;
            if (Properties.Settings.Default.language == "english")
            {
                msgBox = new string[] {"The download failed, what should I do?" +
            Environment.NewLine +
            "Mostly you just need to retry the download!" +
            Environment.NewLine +
            Environment.NewLine +
            "Where can I suggest a Server?" +
            Environment.NewLine +
            "You can do this in the linked Group." + Environment.NewLine + "If a server's button is green it means that the server is installed. Click the button to Update the server or Open the folder or Delete it etc.","FAQ" };
            }
            else if (Properties.Settings.Default.language == "german")
            {
                msgBox = new string[] {"Der Download ist fehlgeschlagen, was nun?" +
            Environment.NewLine +
            "Meisten reicht es den Download einfach erneut auszuführen!" +
            Environment.NewLine +
            Environment.NewLine +
            "Wo kann ich einen Server Vorschlag machen?" +
            Environment.NewLine +
            "Dies ist möglich in der verlinken Gruppe."+ Environment.NewLine + "Wenn ein Button grün angezeigt wird, so ist der Server installiert. Klicke auf diesen Button, um den Server zu updaten, den Order zu öffnen oder zu löschen.","FAQ" };////heeeere
            }
            MessageBox.Show(msgBox[0], msgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void Left4Dead2ToolStripMenuItemClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=764005219");
            //Left 4 Dead 2 Guide Englisch		
        }

        void Left4Dead2ToolStripMenuItem1Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=754702686");
            //Left 4 Dead 2 Guide German
        }

        void Button17Click(object sender, EventArgs e)
        {
            MessageBox.Show("More Servers" +
                            Environment.NewLine +
                            "Installing SourceMod etc." +
                             Environment.NewLine +
                            "Maybe skins", "Planned Features");
        }

        void KillingFloor2ToolStripMenuItemClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=775342119");
            //Killing Floor 2 Guide German			
        }

        void KillingFloor2ToolStripMenuItem1Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=804751117");
            //Killing Floor 2 Guide Englisch
        }

        void Button18Click(object sender, EventArgs e)
        {
            serverFolderName = @"bo3";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(CODblackOpsThreeBtn, sender, codBoThreeSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, Properties.Settings.Default.codbo3InstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Call of Duty: Black Ops 3 Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }//CoD Black Ops 3

        public bool aboutFormIsVisible = false;
        private AboutFrm aboutform = new AboutFrm();
        void AboutToolStripMenuItem2Click(object sender, EventArgs e)
        {


            //check if the about form is show or not and open it or close it
            aboutform.Show();
            aboutform.BringToFront();
        }

        void CallOfDutyBlackOps3ToolStripMenuItemClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=818635244");
            //Call of Duty: Black Ops 3 Guide German
        }

        void CallOfDutyBlackOps3ToolStripMenuItem1Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=819848285");
            //Call of Duty: Black Ops 3 Guide English	
        }

        void SvenCoopToolStripMenuItem1Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=874080531");
            //Sven Co-op Guide German	
        }

        void SvenCoopToolStripMenuItemClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=874293589");
            //Sven Co-op Guide English
        }

        Boolean OpenSteamCmd()
        {
            // wenn Dateiauswahldialog nur anzeigen, wenn kein Pfad, oder ungültige Datei oder nicht steamcmd.exe
            if (steamCmdSpeicherort == null ||
               !(File.Exists(steamCmdSpeicherort)) ||
               Directory.Exists(steamCmdSpeicherort) ||
               !Path.GetExtension(steamCmdSpeicherort).Equals("exe"))
            {
                //TODO Datei auswahl Dialog jetzt anzeigen
                OpenFileDialog auswahlDialog = new OpenFileDialog();
                if (steamCmdSpeicherort != null && Directory.Exists(steamCmdSpeicherort))
                {
                    auswahlDialog.InitialDirectory = steamCmdSpeicherort;
                }
                //TODO Filter auf *.exe setzen
                auswahlDialog.Filter = "Steamcmd executable|steamcmd.exe|Any executable (*.exe)|*.exe";
                auswahlDialog.FilterIndex = 1; //setzt die Standardauswahl auf steamcmd.exe
                auswahlDialog.RestoreDirectory = true;
                auswahlDialog.Multiselect = false; //erlaubt nur die Auswahl einer einzelnen Datei
                auswahlDialog.CheckFileExists = true;
                if (auswahlDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        String result = auswahlDialog.FileName;
                        if (!File.Exists(result))
                        {
                            MessageBox.Show("No file selected.", "Locate SteamCMD", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false; //Der Nutzer hat keine gültige Datei ausgewählt.
                        }
                        steamCmdSpeicherort = result;
                        //save steamCMD.exe location for the program to automatically locate it next time it starts up
                        Properties.Settings.Default.lastSavedSteamCMDLoc = steamCmdSpeicherort;
                        Properties.Settings.Default.Save();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error in OpenSteamCmd! Details: " + ex.Message);
                        return false;
                    }
                }
                else
                {
                    //nutzer hat keine Datei ausgewählt.
                    MessageBox.Show("No file selected.", "Locate SteamCMD", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            //laut dem if muss die Datei eine existierende .exe Datei sein. Wir vermuten, dass die steamcmd korrekt gewählt wurde.
            return true;
        }

        void ServerGroupToolStripMenuItemClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/groups/ServerTool");
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void forwardingPortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://portforward.com/router.htm");
        }

        private void rustToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://steamcommunity.com/sharedfiles/filedetails/?id=1436222938");
            //Rust Guide English
        }

        private void rUstToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://steamcommunity.com/sharedfiles/filedetails/?id=1444135543");
            //Rust Guide German
        }
        private void enableControls()
        {
            groupBox1.Enabled = true;
            downloadSteamCMDbtn.Enabled = true;
            locateSteamCMDBtn.Enabled = true;
            langouageToolStripMenuItem.Enabled = true;
            serverStartupFilesToolStripMenuItem.Enabled = true;
            this.BringToFront();
        }
        private void OutputReaderTimer_Tick(object sender, EventArgs e)
        {
            if (installSrvProcess.HasExited)
            {

                SteamCMDtimer.Enabled = false;
                enableControls();
                //Complete installation and ask user to create batch-config file
                finishInstallation();
            }
        }

        private void finishInstallation()
        {


            //here is a switch statement. it works like the IF statement
            switch (serverFolderName)
            {
                //COUNTER STRIKE 1.6
                case "cs":
                    finInstallFunc.finishInstallationCS16(steamCmdSpeicherort, serverFolderName);
                    serverFolderName = null;
                    break;
                //GMOD
                case "gmod":
                    finInstallFunc.finishInstallationGmod(steamCmdSpeicherort, serverFolderName);
                    serverFolderName = null;
                    break;
                //LEFT 4 DEAD 2
                case "l4d2":
                    finInstallFunc.finishInstallationL4d2(steamCmdSpeicherort, serverFolderName);
                    serverFolderName = null;
                    break;
                //COUNTER STRIKE SOURCE
                case "css":
                    finInstallFunc.finishInstallationCSSOURCE(steamCmdSpeicherort, serverFolderName);
                    serverFolderName = null;
                    break;
                //CS:GO
                case "csgo":
                    finInstallFunc.finishInstallationCSGO(steamCmdSpeicherort, serverFolderName);
                    serverFolderName = null;
                    break;
                //Sven Coop
                case "svencoop":
                    finInstallFunc.finishInstallationSvenCoop(steamCmdSpeicherort, serverFolderName);
                    serverFolderName = null;
                    break;
                //Call of Duty Black ops 3
                case "bo3":
                    finInstallFunc.finishInstallationCodBO3(steamCmdSpeicherort, serverFolderName);
                    serverFolderName = null;
                    break;
                //Rust
                case "rust":
                    finInstallFunc.finishInstallationRust(steamCmdSpeicherort, serverFolderName);
                    serverFolderName = null;
                    break;
                //Killing floor 2
                case "kf2":
                    finInstallFunc.finishInstallationKf2(steamCmdSpeicherort, serverFolderName);
                    serverFolderName = null;
                    break;
                //Left 4 dead
                case "l4d":
                    finInstallFunc.finishInstallationL4d(steamCmdSpeicherort, serverFolderName);
                    serverFolderName = null;
                    break;
                //ARK
                case "ark":
                    finInstallFunc.finishInstallationARK(steamCmdSpeicherort, serverFolderName);
                    serverFolderName = null;
                    break;
                //7 days to die
                case "7days":
                    finInstallFunc.finishInstallation7days(steamCmdSpeicherort, serverFolderName);
                    serverFolderName = null;
                    break;
                //Hurtworld
                case "hurtworld":
                    finInstallFunc.finishInstallationHurtworld(steamCmdSpeicherort, serverFolderName);
                    serverFolderName = null;
                    break;
                //Synergy
                case "synergy":
                    //EMPTY
                    serverFolderName = null;
                    break;

            }
        }
        private void disableControls()
        {
            //disable some controls while SteamCMD is installing the server
            groupBox1.Enabled = false;
            downloadSteamCMDbtn.Enabled = false;
            locateSteamCMDBtn.Enabled = false;
            langouageToolStripMenuItem.Enabled = false;
            serverStartupFilesToolStripMenuItem.Enabled = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //locate steamCMD automatically
            steamCmdSpeicherort = Properties.Settings.Default.lastSavedSteamCMDLoc;
            if (File.Exists(steamCmdSpeicherort))
            {
                steamCMDLocated();
            }

            //Message to show if update is found. variable
            string[] updateMessage = { };
            //
            if (Properties.Settings.Default.language == "english")
            {
                englishToolStripMenuItem1.PerformClick();
                //set text to the update variable
                updateMessage = new string[] { "An update is available! Do you want to download it?", "Update Available" };
            }
            else if (Properties.Settings.Default.language == "german")
            {
                germanToolStripMenuItem.PerformClick();
                updateMessage = new string[] { "Ein Update ist verfügbar! Möchtest du es downloaden?", "Update verfügbar" };
            }

            //get available version
            axcsCheck4Update.axMain oCheckClient = new axcsCheck4Update.axMain("https://drive.google.com/uc?export=download&id=1wB8JbiUU4Sd58YUXBV-elRl8befiEpey");

            int nMajor = oCheckClient.GetVersion(axcsCheck4Update.enVerion.EMajor);
            int nMinor = oCheckClient.GetVersion(axcsCheck4Update.enVerion.EMinor);
            int nBuild = oCheckClient.GetVersion(axcsCheck4Update.enVerion.EBuild);

            string avaliableVer = nMajor.ToString() + nMinor.ToString() + nBuild.ToString();
            string strPath = oCheckClient.GetNewVersionPath();

            // Get my own version's numbers
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            int nAppMajor = fileVersionInfo.FileMajorPart;
            int nAppMinor = fileVersionInfo.FileMinorPart;
            int nAppBuild = fileVersionInfo.FileBuildPart;
            string currentVer = nAppMajor.ToString() + nAppMinor.ToString() + nAppBuild.ToString();

            if (Int32.Parse(avaliableVer) > Int32.Parse(currentVer))
            {
                if (MessageBox.Show(updateMessage[0], updateMessage[1], MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Latest version of the tool download website
                    Process.Start(strPath);
                }
            }

        }

        void steamCMDLocated()
        {

            groupBox1.Enabled = true;
            serverStartupFilesToolStripMenuItem.Enabled = true;
            //Check Langouage
            if (Properties.Settings.Default.language == "english")
            {
                locateSteamCMDBtn.Text = "      SteamCMD Located";
            }
            else if (Properties.Settings.Default.language == "german")
            {
                locateSteamCMDBtn.Text = "     Lokalisiere SteamCMD";
            }
            locateSteamCMDBtn.ForeColor = Color.Green;
            locateSteamCMDBtn.Image = Properties.Resources.icons8_checkmark_24;
            int CMDnameLetterCount = steamCmdSpeicherort.Substring(steamCmdSpeicherort.LastIndexOf(@"\") + 1).Length;
            steamCMDFolder = steamCmdSpeicherort.Remove(steamCmdSpeicherort.Length - CMDnameLetterCount);
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            if (OpenSteamCmd() == true)
            {
                steamCMDLocated();
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            serverFolderName = @"l4d";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(l4dBtn, sender, l4dSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, Properties.Settings.Default.l4dInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Left 4 Dead Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }//Left 4 Dead 

        string[] serverNotInstalledCreateFiles;
        string[] serverBatchOrConfigFileDoesntExist;
        string[] serverBatchOrConfigFileDoesntExist2;
        string[] hurtworldRenameAutoexec;
        string[] svrCntxMnu;
        private void EnglishToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.language = "english";
            Properties.Settings.Default.Save();

            //change UI text
            downloadSteamCMDbtn.Text = "Download SteamCMD";
            plannedFeaturesBtn.Text = "Planned features";
            if (locateSteamCMDBtn.ForeColor == Color.Green) { locateSteamCMDBtn.Text = "      SteamCMD Located"; }
            else { locateSteamCMDBtn.Text = "       Locate SteamCMD"; }
            tutorialToolStripMenuItem.Text = "Guides";
            dataToolStripMenuItem.Text = "Data";
            changelogToolStripMenuItem.Text = "Changelog";
            serverGroupToolStripMenuItem.Text = "Server Group";
            closeToolStripMenuItem1.Text = "Exit";
            closeToolStripMenuItem1.ToolTipText = "Close the program";
            serverStartupFilesToolStripMenuItem.Text = "Create Server Files(.cfg and .bat)";
            richTextBox1.Text = "1. Click " + @"""Download SteamCMD""" + " (If you haven't)" + Environment.NewLine + "2. Locate your SteamCMD (If it isn't located)" + Environment.NewLine + "3. Choose your server" + Environment.NewLine + "4. Type" + @"""quit""" + "if the download was successful" + Environment.NewLine + "5. Create server files (Optional for some games)" + Environment.NewLine + @"6. Edit your server's settings (Optional) (Edit .cfg and .bat files)" + Environment.NewLine + "7. Start the server and have fun!" + Environment.NewLine + Environment.NewLine + "Note: This Tool will ONLY install the Server, it can´t forward any ports for you!";
            serverNotInstalledCreateFiles = new string[] { "The selected game is not installed!", "Create Server files" };
            //ServerMenuStrip
            svrCntxMnu = new string[] { "Start Server", "Update/Repair Server", "Open Server Folder", "Delete Server", "Edit Server Settings" };
            changeSrvMenuButtonLang(svrCntxMnu);
            //--------
            serverBatchOrConfigFileDoesntExist = new string[] { @"doesn't exist! Please create the file yourself or by clicking the ""Create server files"" button", "Edit" };
            serverBatchOrConfigFileDoesntExist2 = new string[] { @"doesn't exist! Create the file yourself or try repairing the server installation", "Edit" };
            hurtworldRenameAutoexec = new string[] { @"Please rename ""autoexec_default.cfg"" to ""autoexec.cfg"" for any changes in the server to take effect. You can find this file inside the server's ROOT folder", "Edit autoexec_default.cfg" };
        }

        private void GermanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.language = "german";
            Properties.Settings.Default.Save();

            //change UI text
            downloadSteamCMDbtn.Text = "Download SteamCMD";
            plannedFeaturesBtn.Text = "Geplante Features/Funktionen";
            if (locateSteamCMDBtn.ForeColor == Color.Green) { locateSteamCMDBtn.Text = "  SteamCMD lokalisiert"; }
            else { locateSteamCMDBtn.Text = "     Lokalisiere SteamCMD"; }
            tutorialToolStripMenuItem.Text = "Guides";
            dataToolStripMenuItem.Text = "Data";
            changelogToolStripMenuItem.Text = "Changelog";
            serverGroupToolStripMenuItem.Text = "Server Gruppe";
            closeToolStripMenuItem1.Text = "Ende";
            closeToolStripMenuItem1.ToolTipText = "Schließe das Programm";
            serverStartupFilesToolStripMenuItem.Text = "Erstelle Server Dateien(.bat und .cfg) ";
            richTextBox1.Text = "1. Klicke auf " + @"""Download SteamCMD""" + " (Sollte sie nicht bereits vorhanden sein.)" + Environment.NewLine + "2. Lokalisiere deine SteamCMD (Wenn diese nicht bereits lokalisiert sein sollte)" + Environment.NewLine + "3. Wähle deinen Server aus." + Environment.NewLine + "4. Tippe " + @"""quit""" + " ein, wenn der Download beendet wurde." + Environment.NewLine + "5. Erstelle Server Dateien (.bat und .cfg) (Für manche Server optional.)" + Environment.NewLine + @"6. Editiere deine Servereinstellungen (Optional) (Editiere die .cfg and .bat Datei)" + Environment.NewLine + "7. Starte den Server und los geht´s!" + Environment.NewLine + Environment.NewLine + "Hinweis: Dieses Programm installiert NUR die Server, es wird keine Ports für dich freigeben!";
            serverNotInstalledCreateFiles = new string[] { "The selected game is not installed!", "Create Server files" };
            //ServerMenuStrip
            svrCntxMnu = new string[] { "Starte Server", "Update/Repariere Server", "Öffne Order des Servers", "Lösche Server", "Editiere Server Einstellungen" };
            changeSrvMenuButtonLang(svrCntxMnu);
            //--------
            serverBatchOrConfigFileDoesntExist = new string[] { @"exestiert nicht! Bitte erstelle die Datei selbst mit einem Klick auf ""Erstelle Server Dateien""", "Editiere" };
            serverBatchOrConfigFileDoesntExist2 = new string[] { @"exestiert nicht! Erstelle die Datei selbst oder versuche die Installation zu reparieren.", "Edit" };
            hurtworldRenameAutoexec = new string[] { @"Bitte benne die Datei ""autoexec_default.cfg"" in ""autoexec.cfg"" um, damit die Änderungen übernommen werden. Du kannst diese Datei im Root Verzeichnes des Servers finden.", "Editiere autoexec_default.cfg" };
        }

        #region change Server Menu Strip Button language
        public void changeSrvMenuButtonLang(string[] translArrey)
        {
            //7 days to die
            ToolStripMenuItem93.Text = translArrey[0];
            ToolStripMenuItem94.Text = translArrey[1];
            ToolStripMenuItem95.Text = translArrey[2];
            ToolStripMenuItem96.Text = translArrey[3];
            ToolStripMenuItem97.Text = translArrey[4];
            //Counter Strike: Source
            toolStripMenuItem2.Text = translArrey[0];
            toolStripMenuItem3.Text = translArrey[1];
            toolStripMenuItem4.Text = translArrey[2];
            toolStripMenuItem5.Text = translArrey[3];
            toolStripMenuItem6.Text = translArrey[4];
            //Killing Floor 2
            toolStripMenuItem9.Text = translArrey[0];
            toolStripMenuItem10.Text = translArrey[1];
            toolStripMenuItem11.Text = translArrey[2];
            toolStripMenuItem12.Text = translArrey[3];
            toolStripMenuItem13.Text = translArrey[4];
            //Sven Coop
            toolStripMenuItem16.Text = translArrey[0];
            toolStripMenuItem17.Text = translArrey[1];
            toolStripMenuItem18.Text = translArrey[2];
            toolStripMenuItem19.Text = translArrey[3];
            toolStripMenuItem20.Text = translArrey[4];
            //ARK: Survival Evolved
            toolStripMenuItem23.Text = translArrey[0];
            toolStripMenuItem24.Text = translArrey[1];
            toolStripMenuItem25.Text = translArrey[2];
            toolStripMenuItem26.Text = translArrey[3];
            toolStripMenuItem27.Text = translArrey[4];
            //CS:GO
            toolStripMenuItem30.Text = translArrey[0];
            toolStripMenuItem31.Text = translArrey[1];
            toolStripMenuItem32.Text = translArrey[2];
            toolStripMenuItem33.Text = translArrey[3];
            toolStripMenuItem34.Text = translArrey[4];
            //Left 4 Dead
            toolStripMenuItem38.Text = translArrey[0];
            toolStripMenuItem39.Text = translArrey[1];
            toolStripMenuItem40.Text = translArrey[2];
            toolStripMenuItem41.Text = translArrey[3];
            toolStripMenuItem42.Text = translArrey[4];
            //CoD: Black Ops 3
            toolStripMenuItem44.Text = translArrey[0];
            toolStripMenuItem45.Text = translArrey[1];
            toolStripMenuItem46.Text = translArrey[2];
            toolStripMenuItem47.Text = translArrey[3];
            toolStripMenuItem48.Text = translArrey[4];
            //Garry's Mod
            toolStripMenuItem51.Text = translArrey[0];
            toolStripMenuItem52.Text = translArrey[1];
            toolStripMenuItem53.Text = translArrey[2];
            toolStripMenuItem54.Text = translArrey[3];
            toolStripMenuItem55.Text = translArrey[4];
            //Left 4 Dead 2
            toolStripMenuItem58.Text = translArrey[0];
            toolStripMenuItem59.Text = translArrey[1];
            toolStripMenuItem60.Text = translArrey[2];
            toolStripMenuItem61.Text = translArrey[3];
            toolStripMenuItem62.Text = translArrey[4];
            //Counter Strike 1.6
            toolStripMenuItem65.Text = translArrey[0];
            toolStripMenuItem66.Text = translArrey[1];
            toolStripMenuItem67.Text = translArrey[2];
            toolStripMenuItem68.Text = translArrey[3];
            toolStripMenuItem69.Text = translArrey[4];
            //Hurtworld
            toolStripMenuItem72.Text = translArrey[0];
            toolStripMenuItem73.Text = translArrey[1];
            toolStripMenuItem74.Text = translArrey[2];
            toolStripMenuItem75.Text = translArrey[3];
            toolStripMenuItem76.Text = translArrey[4];
            //Rust
            toolStripMenuItem79.Text = translArrey[0];
            toolStripMenuItem80.Text = translArrey[1];
            toolStripMenuItem81.Text = translArrey[2];
            toolStripMenuItem82.Text = translArrey[3];
            toolStripMenuItem83.Text = translArrey[4];
            //Synergy
            toolStripMenuItem86.Text = translArrey[0];
            toolStripMenuItem87.Text = translArrey[1];
            toolStripMenuItem88.Text = translArrey[2];
            toolStripMenuItem89.Text = translArrey[3];
            toolStripMenuItem90.Text = translArrey[4];

        }
        #endregion

        private void ARKSurvivalEvolvedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //check if the server is installed
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "ark") == true)
            {
                finInstallFunc.finishInstallationARK(steamCmdSpeicherort, "ark");
            }
            else
            {
                MessageBox.Show(serverNotInstalledCreateFiles[0], serverNotInstalledCreateFiles[1], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void CounterStrikeSourceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //check if the server is installed
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "css") == true)
            {
                finInstallFunc.finishInstallationCSSOURCE(steamCmdSpeicherort, "css");
            }
            else
            {
                MessageBox.Show(serverNotInstalledCreateFiles[0], serverNotInstalledCreateFiles[1], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void RustToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //check if the server is installed
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "rust") == true)
            {
                finInstallFunc.finishInstallationRust(steamCmdSpeicherort, "rust");
            }
            else
            {
                MessageBox.Show(serverNotInstalledCreateFiles[0], serverNotInstalledCreateFiles[1], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void CounterStrikeGlobalOffensiveToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //check if the server is installed
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "csgo") == true)
            {
                finInstallFunc.finishInstallationCSGO(steamCmdSpeicherort, "csgo");
            }
            else
            {
                MessageBox.Show(serverNotInstalledCreateFiles[0], serverNotInstalledCreateFiles[1], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void SvenCoopToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //check if the server is installed
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "sven") == true)
            {
                finInstallFunc.finishInstallationSvenCoop(steamCmdSpeicherort, "sven");
            }
            else
            {
                MessageBox.Show(serverNotInstalledCreateFiles[0], serverNotInstalledCreateFiles[1], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void CounterStrike16ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //check if the server is installed
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "cs") == true)
            {
                finInstallFunc.finishInstallationCS16(steamCmdSpeicherort, "cs");
            }
            else
            {
                MessageBox.Show(serverNotInstalledCreateFiles[0], serverNotInstalledCreateFiles[1], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void GarrysModToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //check if the server is installed
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "gmod") == true)
            {
                finInstallFunc.finishInstallationGmod(steamCmdSpeicherort, "gmod");
            }
            else
            {
                MessageBox.Show(serverNotInstalledCreateFiles[0], serverNotInstalledCreateFiles[1], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void Left4Dead2ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //check if the server is installed
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "l4d2") == true)
            {
                finInstallFunc.finishInstallationL4d2(steamCmdSpeicherort, "l4d2");
            }
            else
            {
                MessageBox.Show(serverNotInstalledCreateFiles[0], serverNotInstalledCreateFiles[1], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void Left4DeadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://steamcommunity.com/sharedfiles/filedetails/?id=1872593045");
            //Left 4 Dead Guide English
        }

        private void Left4DeadToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://steamcommunity.com/sharedfiles/filedetails/?id=1795236722");
            //Left 4 Dead Guide German
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            serverFolderName = @"7days";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(daysToDieBtn, sender, daysToDieSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = Process.Start(steamCmdSpeicherort, Properties.Settings.Default.daysInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install 7 Days to Die Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }//7 days to die

        private void DaysToDieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://steamcommunity.com/sharedfiles/filedetails/?id=1728855550");
            //7 Days to Die English Guide
        }

        private void DaysToDieToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://steamcommunity.com/sharedfiles/filedetails/?id=1731898014");
            //7 Days to Die German Guide

        }

        private void left4DeadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //check if the server is installed
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "l4d") == true)
            {
                finInstallFunc.finishInstallationL4d(steamCmdSpeicherort, "l4d");
            }
            else
            {
                MessageBox.Show(serverNotInstalledCreateFiles[0], serverNotInstalledCreateFiles[1], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            serverFolderName = @"hurtworld";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(hurtWorldBtn, sender, hurtworldSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, Properties.Settings.Default.hurtworldInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Hurtworld Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }//Hurtworld

        private void hurtworldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://steamcommunity.com/sharedfiles/filedetails/?id=1912242476");
            //Hurtworld Guide German	
        }

        private void hurtworldToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://steamcommunity.com/sharedfiles/filedetails/?id=1893124522");
            //Hurtworld Guide English	
        }

        //States of server buttons when a server is installed or not installed
        void gameInstBtnState(Button button)
        {
            button.FlatStyle = FlatStyle.Standard;
            button.BackColor = Color.LimeGreen;
        }
        void gameNotInstBtnState(Button button)
        {
            button.FlatStyle = FlatStyle.System;
            button.BackColor = SystemColors.Control;
        }
        //
        private void generalTimer_Tick(object sender, EventArgs e)
        {
            //7 days to die
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "7days"))
            {
                gameInstBtnState(daysToDieBtn);
            }
            else
            {
                gameNotInstBtnState(daysToDieBtn);
            }
            //Counter Strike Source
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "css"))
            {
                gameInstBtnState(CSsourceBtn);
            }
            else
            {
                gameNotInstBtnState(CSsourceBtn);
            }
            //Killing Floor 2
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "kf2"))
            {
                gameInstBtnState(KillingFloorTwoBtn);
            }
            else
            {
                gameNotInstBtnState(KillingFloorTwoBtn);
            }
            //Sven Coop
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "sven"))
            {
                gameInstBtnState(SvenCoopBtn);
            }
            else
            {
                gameNotInstBtnState(SvenCoopBtn);
            }
            //ARK: Survival Evolved
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "ark"))
            {
                gameInstBtnState(arkBtn);
            }
            else
            {
                gameNotInstBtnState(arkBtn);
            }
            //CS:GO
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "csgo"))
            {
                gameInstBtnState(CSGOBtn);
            }
            else
            {
                gameNotInstBtnState(CSGOBtn);
            }
            //Left 4 dead
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "l4d"))
            {
                gameInstBtnState(l4dBtn);
            }
            else
            {
                gameNotInstBtnState(l4dBtn);
            }
            //Call of Duty: Black Ops 3
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "bo3"))
            {
                gameInstBtnState(CODblackOpsThreeBtn);
            }
            else
            {
                gameNotInstBtnState(CODblackOpsThreeBtn);
            }
            //Garry's Mod
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "gmod"))
            {
                gameInstBtnState(garrysModBtn);
            }
            else
            {
                gameNotInstBtnState(garrysModBtn);
            }
            //Left 4 Dead 2
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "l4d2"))
            {
                gameInstBtnState(l4dTwoBtn);
            }
            else
            {
                gameNotInstBtnState(l4dTwoBtn);
            }
            //HurtWorld
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "hurtworld"))
            {
                gameInstBtnState(hurtWorldBtn);
            }
            else
            {
                gameNotInstBtnState(hurtWorldBtn);
            }
            //Couter Strike 1.6
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "cs"))
            {
                gameInstBtnState(csOnePointsixBtn);
            }
            else
            {
                gameNotInstBtnState(csOnePointsixBtn);
            }
            //Rust
            if (methodsClass.checkIfGameInstalled(steamCMDFolder, "rust"))
            {
                gameInstBtnState(RustBtn);
            }
            else
            {
                gameNotInstBtnState(RustBtn);
            }
            //Synergy
            if (System.IO.File.Exists(""))
            {
                gameInstBtnState(synergyBtn);
            }
            else
            {
                gameNotInstBtnState(synergyBtn);
            }

        }

        //Update/Repair Server
        private void updateServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serverFolderName = @"7days";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(daysToDieBtn, sender, daysToDieSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = Process.Start(steamCmdSpeicherort, Properties.Settings.Default.daysInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install 7 Days to Die Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }

        //Start Server
        Process daysToDieProc = new Process();
        private void startServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string gameFold = Path.Combine(steamCMDFolder + serverFolderName);
            daysToDieProc.StartInfo.WorkingDirectory = gameFold;
            daysToDieProc.StartInfo.FileName = gameFold + @"\startdedicated.bat";
            daysToDieProc.Start();
        }

        //Open Server Folder
        private void openServerFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(steamCMDFolder + serverFolderName));
        }

        //Delete Server
        private void deleteServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void editCONFIGFILEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\serverconfig.xml");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("serverconfig.xml " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " serverconfig.xml", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            serverFolderName = @"css";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(daysToDieBtn, sender, daysToDieSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, Properties.Settings.Default.cssInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Counter Strike: Source Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }

        private void Kf2SvrMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            serverFolderName = @"kf2";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(daysToDieBtn, sender, daysToDieSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = Process.Start(steamCmdSpeicherort, Properties.Settings.Default.kf2InstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Killing Floor 2 Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }

        private void toolStripMenuItem17_Click(object sender, EventArgs e)
        {
            serverFolderName = @"sven";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(daysToDieBtn, sender, daysToDieSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, Properties.Settings.Default.svenInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Sven Coop Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }

        private void toolStripMenuItem24_Click(object sender, EventArgs e)
        {
            serverFolderName = @"ark";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(SvenCoopBtn, sender, SvenSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = Process.Start(steamCmdSpeicherort, Properties.Settings.Default.arkInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install ARK: Survival Evolved Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }

        private void toolStripMenuItem31_Click(object sender, EventArgs e)
        {
            serverFolderName = @"csgo";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(SvenCoopBtn, sender, SvenSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, Properties.Settings.Default.csgoInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Counter Strike: Global Offensive Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }

        private void toolStripMenuItem38_Click(object sender, EventArgs e)
        {
            serverFolderName = @"l4d";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(SvenCoopBtn, sender, SvenSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, Properties.Settings.Default.l4dInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Left 4 dead Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }

        private void toolStripMenuItem45_Click(object sender, EventArgs e)
        {
            serverFolderName = @"bo3";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(SvenCoopBtn, sender, SvenSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, Properties.Settings.Default.codbo3InstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Call of duty: Black Ops 3 Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }

        private void toolStripMenuItem80_Click(object sender, EventArgs e)
        {
            serverFolderName = @"rust";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(daysToDieBtn, sender, daysToDieSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = Process.Start(steamCmdSpeicherort, Properties.Settings.Default.rustInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Rust Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }

        private void toolStripMenuItem59_Click(object sender, EventArgs e)
        {
            serverFolderName = @"l4d2";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(daysToDieBtn, sender, daysToDieSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = Process.Start(steamCmdSpeicherort, Properties.Settings.Default.l4d2InstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Left 4 dead 2 Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }

        private void toolStripMenuItem66_Click(object sender, EventArgs e)
        {
            serverFolderName = @"cs";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(daysToDieBtn, sender, daysToDieSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = Process.Start(steamCmdSpeicherort, Properties.Settings.Default.cs16InstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Counter Strike 1.6 Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }

        private void toolStripMenuItem73_Click(object sender, EventArgs e)
        {
            serverFolderName = @"hurtworld";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(daysToDieBtn, sender, daysToDieSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = Process.Start(steamCmdSpeicherort, Properties.Settings.Default.hurtworldInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Hurtworld Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }

        private void toolStripMenuItem87_Click(object sender, EventArgs e)
        {
            serverFolderName = @"synergy";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(daysToDieBtn, sender, daysToDieSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = Process.Start(steamCmdSpeicherort, Properties.Settings.Default.synergyInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Synergy Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }

        private void toolStripMenuItem52_Click(object sender, EventArgs e)
        {
            serverFolderName = @"gmod";
            switch (methodsClass.checkIfGameInstalledAndCheckSteamCMD(daysToDieBtn, sender, daysToDieSvrMenu, steamCmdSpeicherort))
            {
                case "NotInstalled":
                    installSrvProcess = Process.Start(steamCmdSpeicherort, Properties.Settings.Default.gmodInstallCmd);
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                    disableControls();
                    break;

                case "steamCMDNotFound":
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Garry's Mod Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    serverFolderName = null;
                    break;
            }
        }

        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(steamCMDFolder + serverFolderName));
        }

        private void SvenSvrMenu_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {

        }

        private void toolStripMenuItem25_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(steamCMDFolder + serverFolderName));
        }

        private void toolStripMenuItem32_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(steamCMDFolder + serverFolderName));
        }

        private void toolStripMenuItem39_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(steamCMDFolder + serverFolderName));
        }

        private void toolStripMenuItem46_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(steamCMDFolder + serverFolderName));
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(steamCMDFolder + serverFolderName));
        }

        private void toolStripMenuItem60_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(steamCMDFolder + serverFolderName));
        }

        private void toolStripMenuItem67_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(steamCMDFolder + serverFolderName));
        }

        private void toolStripMenuItem74_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(steamCMDFolder + serverFolderName));
        }

        private void toolStripMenuItem81_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(steamCMDFolder + serverFolderName));
        }

        private void toolStripMenuItem88_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(steamCMDFolder + serverFolderName));
        }

        private void toolStripMenuItem53_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(steamCMDFolder + serverFolderName));
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(steamCMDFolder + serverFolderName));
        }

        private void toolStripMenuItem21_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\Run.bat");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("Run.bat " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " Run.bat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem28_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\ShooterGame\Binaries\Win64\Run.bat");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("Run.bat " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " Run.bat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem29_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\ShooterGame\Saved\Config\WindowsServer\GameUserSettings.ini");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("GameUserSettings.ini " + serverBatchOrConfigFileDoesntExist2[0], serverBatchOrConfigFileDoesntExist2[1] + " GameUserSettings.ini", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem35_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\Run.bat");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("Run.bat " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " Run.bat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem36_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\csgo\cfg\server.cfg");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("server.cfg " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " server.cfg", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem42_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\Run.bat");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("Run.bat " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " Run.bat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem43_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\left4dead\cfg\server.cfg");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("server.cfg " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " server.cfg", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem50_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\machinecfg\playlists.info");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("playlists.info " + serverBatchOrConfigFileDoesntExist2[0], serverBatchOrConfigFileDoesntExist2[1] + " playlists.info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem22_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\svencoop\cfg\server.cfg");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("server.cfg " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " server.cfg", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\Run.bat");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("Run.bat " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " Run.bat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\cstrike\cfg\server.cfg");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("server.cfg " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " server.cfg", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem63_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\Run.bat");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("Run.bat " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " Run.bat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem64_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\left4dead\cfg\server.cfg");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("server.cfg " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " server.cfg", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem70_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\Run.bat");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("Run.bat " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " Run.bat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem71_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\cstrike\server.cfg");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("server.cfg " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " server.cfg", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem78_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\autoexec_default.cfg");
            string FileLoc2 = Path.Combine(steamCMDFolder + serverFolderName + @"\autoexec.cfg");
            bool defaultExists = true;
            bool renamedExists = true;
            if (File.Exists(FileLoc2))
            { Process.Start("notepad.exe", FileLoc2); }
            else { renamedExists = false; }
            if (System.IO.File.Exists(FileLoc))
            {
                try
                {
                    FileSystem.Rename(FileLoc, FileLoc2);
                    Process.Start("notepad.exe", FileLoc2);
                }
                catch
                {
                    MessageBox.Show(hurtworldRenameAutoexec[0], hurtworldRenameAutoexec[1], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Process.Start("notepad.exe", FileLoc);
                }
            }
            else { defaultExists = false; }

            if (defaultExists == false && renamedExists == false)
            {
                MessageBox.Show("autoexec_default.cfg " + serverBatchOrConfigFileDoesntExist2[0], serverBatchOrConfigFileDoesntExist2[1] + " autoexec_default.cfg", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void toolStripMenuItem84_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\Run.bat");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("Run.bat " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " Run.bat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem85_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\server\my_server_identity\cfg\server.cfg");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("server.cfg " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " server.cfg", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem91_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\Run.bat");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("Run.bat " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " Run.bat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem92_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"SYNERGY_CONFIG_FILE_LOCATION");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("server.cfg " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " server.cfg", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem56_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\Run.bat");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("Run.bat " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " Run.bat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem57_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\garrysmod\cfg\server.cfg");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("server.cfg " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " server.cfg", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            string FileLoc = Path.Combine(steamCMDFolder + serverFolderName + @"\KFGame\Config\DefaultGame.ini");
            if (System.IO.File.Exists(FileLoc))
            { Process.Start("notepad.exe", FileLoc); }
            else { MessageBox.Show("DefaultGame.ini " + serverBatchOrConfigFileDoesntExist[0], serverBatchOrConfigFileDoesntExist[1] + " DefaultGame.ini", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        Process cssProc = new Process();
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string gameFold = Path.Combine(steamCMDFolder + serverFolderName);
            cssProc.StartInfo.WorkingDirectory = gameFold;
            if (System.IO.File.Exists(gameFold + @"\Run.bat")) { cssProc.StartInfo.FileName = gameFold + @"\Run.bat"; }
            else { cssProc.StartInfo.FileName = gameFold + @"\srcds.exe"; }
            cssProc.Start();
        }

        Process kf2 = new Process();
        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            string gameFold = Path.Combine(steamCMDFolder + serverFolderName);
            kf2.StartInfo.WorkingDirectory = gameFold;
            kf2.StartInfo.FileName = gameFold + @"\KF2Server.bat";
            kf2.Start();
        }

        Process svenProc = new Process();
        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
            string gameFold = Path.Combine(steamCMDFolder + serverFolderName);
            svenProc.StartInfo.WorkingDirectory = gameFold;
            if (System.IO.File.Exists(gameFold + @"\Run.bat")) { svenProc.StartInfo.FileName = gameFold + @"\Run.bat"; }
            else { svenProc.StartInfo.FileName = gameFold + @"\svends.exe"; }
            svenProc.Start();
        }

        Process arkProc = new Process();
        private void toolStripMenuItem23_Click(object sender, EventArgs e)
        {
            string gameFold = Path.Combine(steamCMDFolder + serverFolderName + @"\ShooterGame\Binaries\Win64");
            arkProc.StartInfo.WorkingDirectory = gameFold;
            arkProc.StartInfo.FileName = gameFold + @"\Run.bat";
          arkProc.Start();
        }

        private void serverStartupFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void internetCheckTimer_Tick(object sender, EventArgs e)
        {
            if (methodsClass.HasInternet() != true)
            {
                serverStartupFilesToolStripMenuItem.Enabled = false;
                downloadSteamCMDbtn.Enabled = false;
                label1.Visible = true;
                refreshInternetBtn.Visible = true;
                internetCheckTimer.Enabled = false;
            }
            else
            {
                label1.Visible = false;
                refreshInternetBtn.Visible = false;
                serverStartupFilesToolStripMenuItem.Enabled = true;
                downloadSteamCMDbtn.Enabled = true;
            }
        }

        private void refreshInternetBtn_Click(object sender, EventArgs e)
        {
            internetCheckTimer.Enabled = true;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
