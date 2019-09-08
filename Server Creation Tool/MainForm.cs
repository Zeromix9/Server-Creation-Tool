/*
 * Created by SharpDevelop.
 * User: Zeromix
 * Date: 28.05.2016
 * Time: 16:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;
namespace Server_Creation_Tool
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        //muss mal kurz googlen... war in letzter zeit nur in Java am entwickeln
        String steamCmdSpeicherort = null;
        Process installSrvProcess = new Process();
        string serverFolderName = null;
        // string gameServerName;
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
        String folder;
        PleaseWaitFrm pleaseWaitFrom = new PleaseWaitFrm();
        void Button1Click(object sender, EventArgs e)
        {

            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
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
                        folder = folderBrowserDialog.SelectedPath;
                        MessageBox.Show(folder.ToString(), "Selected Path");
                        client.DownloadFileAsync(new Uri("https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip"), folder + "\\steamcmd.zip");
                        pleaseWaitFrom.Show();
                        this.Enabled = false;

                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadSteamCmdZip);
                        steamCmdSpeicherort = Path.Combine(folder, "steamcmd.exe");
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
            string steamCMDzip = Path.Combine(folder + @"\steamcmd.zip");
            //try to extract the zip file
            try
            {
                ZipFile.ExtractToDirectory(steamCMDzip, folder);
            }

            catch { }
            //try to delete the zip file
            try
            {
                File.Delete(steamCMDzip);
            }
            catch { }
            MessageBox.Show("Download completed!", "Download SteamCMD", MessageBoxButtons.OK, MessageBoxIcon.Information);
            pleaseWaitFrom.Close();
            this.Enabled = true;
            this.BringToFront();
            if (File.Exists(folder + @"\steamcmd.exe"))
            {
                serverStartupFilesToolStripMenuItem.Enabled = true;
                groupBox1.Enabled = true;
                locateSteamCMDBtn.Text = "      SteamCMD Located";
                locateSteamCMDBtn.ForeColor = Color.Green;
                locateSteamCMDBtn.Image = Properties.Resources.icons8_checkmark_24;
                steamCmdSpeicherort = folder + @"\steamcmd.exe";
            }

        }

        private void ChangeLocateSteamCMDStatus(string englishErrorMessage, string MsgBoxtitle, string germanErrorMessage)
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

            try
            {
                if (steamCmdSpeicherort != null)
                {
                    //check if steamCMD exists
                    if (File.Exists(steamCmdSpeicherort))
                    {
                        installSrvProcess = Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./ark/ +app_update 376030 validate");
                        SteamCMDtimer.Enabled = true;
                        SteamCMDtimer.Start();
                        serverFolderName = @"ark";
                        disableControls();

                    }
                    else
                    {

                        ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Ark: Survival Evolved Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                    }

                }

            }
            catch (Exception) {; } //ARK: Survivel Evolved  
        }

        void Button3Click(object sender, EventArgs e)
        {
            try
            {
                //check if steamCMD exists
                if (File.Exists(steamCmdSpeicherort))
                {
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./css/ +app_update 232330 validate");
                    disableControls();
                    serverFolderName = "css";
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                }
                else
                {
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Counter Strike: Source Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");

                }
            }
            catch (Exception) {; } //CS:S

        }

        void Button4Click(object sender, EventArgs e)
        {
            try
            {
                //check if steamCMD exists
                if (File.Exists(steamCmdSpeicherort))
                {
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./csgo/ +app_update 740 validate");
                    disableControls();
                    serverFolderName = "csgo";
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                }
                else
                {
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Counter Strike: Global Offensive Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");
                }
            }
            catch (Exception) {; } //CS:GO

        }

        void Button5Click(object sender, EventArgs e)
        {
            try
            {
                //check if steamCMD exists
                if (File.Exists(steamCmdSpeicherort))
                {
                    installSrvProcess = Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./cs/ +app_update 90 validate");
                    disableControls();
                    serverFolderName = "cs";
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();

                }
                else
                {
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Counter Strike 1.6 Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");

                }
            }
            catch (Exception) {; } //CS 1.6

        }

        void Button6Click(object sender, EventArgs e)
        {
            try
            {
                //check if steamCMD exists
                if (File.Exists(steamCmdSpeicherort))
                {
                    installSrvProcess = Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./gmod/ +app_update 4020 validate");
                    disableControls();
                    serverFolderName = "gmod";
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                }
                else
                {
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Garry's mod Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");

                }
            }
            catch (Exception) {; } //Garry´s Mod

        }

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
            try
            {
                //check if steamCMD exists
                if (File.Exists(steamCmdSpeicherort))
                {
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./kf2/ +app_update 232130 validate");
                    disableControls();
                    serverFolderName = "kf2";
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                }
                else
                {
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Killing Floor 2 Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");

                }
            }
            catch (Exception) {; } //Killing Floor 2

        }

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
            try
            {
                //check if steamCMD exists
                if (File.Exists(steamCmdSpeicherort))
                {
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./l4d2/ +app_update 222860 validate");
                    disableControls();
                    serverFolderName = "l4d2";
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                }
                else
                {
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Left 4 Dead 2 Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");

                }
            }
            catch (Exception) {; } //Left 4 Dead 2
        }

        void Button11Click(object sender, EventArgs e)
        {
            try
            {
                //check if steamCMD exists
                if (File.Exists(steamCmdSpeicherort))
                {
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./svencoop/ +app_update 276060 validate");
                    disableControls();
                    serverFolderName = "svencoop";
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                }
                else
                {
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Sven Coop Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");

                }
            }
            catch (Exception) {; } //Sven Coop

        }

        void Button12Click(object sender, EventArgs e)
        {
            try
            {
                //check if steamCMD exists
                if (File.Exists(steamCmdSpeicherort))
                {
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./rust/ +app_update 258550 validate");
                    disableControls();
                    serverFolderName = "rust";
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                }
                else
                {
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Rust Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");

                }

            }
            catch (Exception) {; } //Rust


        }

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
            try
            {
                //check if steamCMD exists
                if (File.Exists(steamCmdSpeicherort))
                {
                    installSrvProcess = Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./synergy/ +app_update 17525 validate");
                    disableControls();
                    serverFolderName = "";
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                }
                else
                {
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Synergy Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");

                }
            }
            catch (Exception) {; } //Synergy

        }

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
            "You can do this in the linked Group.","FAQ" };
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
            "Dies ist möglich in der verlinken Gruppe.","FAQ" };
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
            try
            {
                //check if steamCMD exists
                if (File.Exists(steamCmdSpeicherort))
                {
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./bo3/ +app_update 545990 validate");
                    disableControls();
                    serverFolderName = "bo3";
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();
                }
                else
                {
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Rust Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");

                }
            }
            catch (Exception) {; } //Call of Duty: Black Ops 3		
        }
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
                // outputReaderTimer.Stop;
                SteamCMDtimer.Enabled = false;
                enableControls();
                //Complete installation and ask user to create batch-config file
                finishInstallation();
            }
        }
        private void finishInstallationCS16()
        {
            string serverFolderpath = steamCmdSpeicherort.Substring(steamCmdSpeicherort.IndexOf(":") - 1, steamCmdSpeicherort.LastIndexOf("\\")) + "\\" + serverFolderName;
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
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { "Do you want to create a batch file for starting the server?The Batch file will be created in the server's folder.It is optional.The server can run without it", "Create Batch file" };
                    failedToCreateServerBatchFileMsgBox = new string[] { "Failed to create Batch File!You can create the file yourself using the code in the server's guide", "Create Batch File" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Counter Strike 1.6 Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the batch file later to change player slots, map, password etc. For more info, go to the Counter Strike 1.6 Server guide", "Note" };
                    lanOrInternet = new string[] { "Please select if the server will run via lan or on the internet", "Create batch file" };
                    break;
                case "german":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { "Möchtest du eine Batch Datei zum starten des Servers erstellen? Diese Datei wir in deinem Server Ordner erstellt. Dies ist optional, der Server kann auch ohne diese Datei starten.", "Erstellen Sie eine Batchdatei" };
                    failedToCreateServerBatchFileMsgBox = new string[] { "Erstellung der Batch Datei fehlgeschlagen! Du kannst diese auch selbst erstellen, mithilfe des Codes im Server-Guide.", "Erstellen Sie eine Batchdatei" };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Sie Counter Strike 1.6 Server" };
                    importantNoteMsgBox = new string[] { "WICHTIG:Sie können die Batchdatei später bearbeiten, um die Spieleranzahl, Karten usw. zu ändern. Für weitere Informationen öffnen Sie die, öffne den Counter Strike 1.6 Guide.", "Notieren" };
                    lanOrInternet = new string[] { "Wähle aus, ob der Server im Lan oder im Internet verfügbar sein soll.", "Erstellen Sie eine Batchdatei" };
                    break;
            }
            //Check if server's exectable exists
            if (System.IO.File.Exists(serverFolderpath + @"\hlds.exe"))
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
                    //Write Batch file
                    try
                    {
                        System.IO.File.WriteAllText(serverFolderpath + "\\Run.bat", lanOrInternetCommand);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(failedToCreateServerBatchFileMsgBox[0], failedToCreateServerBatchFileMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
                        serverFolderName = null;
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
            serverFolderName = null;
        }
        private void finishInstallationGmod()
        {
            string serverFolderpath = steamCmdSpeicherort.Substring(steamCmdSpeicherort.IndexOf(":") - 1, steamCmdSpeicherort.LastIndexOf("\\")) + "\\" + serverFolderName;
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
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { @"You have to create a batch file and a configuration file in order to start the server.You want it to be created automatically or create it yourself?The Batch file will be created in the server's folder and the configuration file will be created at: " + serverFolderpath + @"\garrysmod\cfg\server.cfg", "Create server's startup files" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Failed to create server's startup files!You can create the files yourself using the code in the server's guide", "Create server's startup files" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Gmod Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the batch and config file later to change player slots, map, password etc. For more info, go to the Gmod Server guide", "Note" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/vtdYsHLG";
                    break;

                case "german":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { "Eine Batch Datei wird benötigt, um den Server zu starten, sowie eine server.cfg. Sollen diese automatisch erstellt werden, oder nicht? Die Batch Datei wird im Server Ordner generiert und die server.cfg befindet sich in: " + serverFolderpath + @"\garrysmod\cfg\server.cfg", "Erstellen Sie die Startdateien des Servers" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Fehler beim Erstellen der Server-Startdateien! Sie können diese Dateien mithilfe des Codes des Server-Guides selbst erstellen.", "Erstellen Sie die Startdateien des Servers" };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Sie Gmod Server" };
                    importantNoteMsgBox = new string[] { "WICHTIG: Du kannst diese Dateien später noch editieren. Für mehr Informationen, öffne den Gmod Server Guide.", "Notieren" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/YzEbTBVy";
                    break;
            }
            //Check if server's exectable exists
            if (System.IO.File.Exists(serverFolderpath + @"\srcds.exe"))
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
            serverFolderName = null;
        }

        private void finishInstallationLeft4dead2()
        {
            string serverFolderpath = steamCmdSpeicherort.Substring(steamCmdSpeicherort.IndexOf(":") - 1, steamCmdSpeicherort.LastIndexOf("\\")) + "\\" + serverFolderName;
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
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { @"You have to create a batch file and a configuration file in order to start the server.You want it to be created automatically or create it yourself?The Batch file will be created in the server's folder and the configuration file will be created at: " + serverFolderpath + @"\l4d2\cfg\server.cfg", "Create server's startup files" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Failed to create server's startup files!You can create the files yourself using the code in the server's guide", "Create server's startup files" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Left 4 Dead 2 Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the batch and config file later to change player slots, map, password etc. For more info, go to the Left 4 dead 2 Server guide", "Note" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/1i8xK3L4";
                    break;

                case "german":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { "Eine Batch Datei wird benötigt, um den Server zu starten, sowie eine server.cfg. Sollen diese automatisch erstellt werden, oder nicht? Die Batch Datei wird im Server Ordner generiert und die server.cfg befindet sich in: " + serverFolderpath + @"\l4d2\cfg\server.cfg", "Erstellen Sie die Startdateien des Servers" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Fehler beim Erstellen der Server-Startdateien! Sie können diese Dateien mithilfe des Codes des Server-Guides selbst erstellen.", "Erstellen Sie die Startdateien des Servers" };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Sie Left 4 dead 2 Server" };
                    importantNoteMsgBox = new string[] { "WICHTIG: Du kannst diese Dateien später noch editieren. Für mehr Informationen, öffne den Left 4 dead 2 Server Guide.", "Notieren" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/NWjDxe4G";
                    break;
            }
            //Check if server's exectable exists
            if (System.IO.File.Exists(serverFolderpath + @"\srcds.exe"))
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
                        System.IO.Directory.CreateDirectory(serverFolderpath + @"\l4d2\cfg");
                        System.IO.File.WriteAllText(serverFolderpath + @"\l4d2\cfg\server.cfg", content);
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
            serverFolderName = null;

        }
        private void finishInstallationCSSOURCE()
        {
            string serverFolderpath = steamCmdSpeicherort.Substring(steamCmdSpeicherort.IndexOf(":") - 1, steamCmdSpeicherort.LastIndexOf("\\")) + "\\" + serverFolderName;
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
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { @"You have to create a batch file and a configuration file in order to start the server.You want it to be created automatically or create it yourself?The Batch file will be created in the server's folder and the configuration file will be created at: " + serverFolderpath + @"\cstrike\cfg\server.cfg", "Create server's startup files" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Failed to create server's startup files!You can create the files yourself using the code in the server's guide", "Create server's startup files" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Counter Strike Source Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the batch and config file later to change player slots, map, password etc. For more info, go to the Counter Strike Source Server guide", "Note" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/e3f89ijz";
                    break;

                case "german":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { "Eine Batch Datei wird benötigt, um den Server zu starten, sowie eine server.cfg. Sollen diese automatisch erstellt werden, oder nicht? Die Batch Datei wird im Server Ordner generiert und die server.cfg befindet sich in: " + serverFolderpath + @"\cstrike\cfg\server.cfg", "Erstellen Sie die Startdateien des Servers" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Fehler beim Erstellen der Server-Startdateien! Sie können diese Dateien mithilfe des Codes des Server-Guides selbst erstellen.", "Erstellen Sie die Startdateien des Servers" };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Sie Counter Strike Source Server" };
                    importantNoteMsgBox = new string[] { "WICHTIG: Du kannst diese Dateien später noch editieren. Für mehr Informationen, öffne den Counter Strike Source Server Guide.", "Notieren" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/mQCitqtv";
                    break;
            }
            //Check if server's exectable exists
            if (System.IO.File.Exists(serverFolderpath + @"\srcds.exe"))
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
            serverFolderName = null;

        }

        public void finishInstallationCSGO()
        {
            string serverFolderpath = steamCmdSpeicherort.Substring(steamCmdSpeicherort.IndexOf(":") - 1, steamCmdSpeicherort.LastIndexOf("\\")) + "\\" + serverFolderName;
            //Check Langouage

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
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { @"You have to create a batch file and a configuration file in order to start the server.You want it to be created automatically or create it yourself?The Batch file will be created in the server's folder and the configuration file will be created at: " + serverFolderpath + @"\csgo\cfg\server.cfg", "Create server's startup files" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Failed to create server's startup files!You can create the files yourself using the code in the server's guide", "Create server's startup files" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Counter Strike Global Offensive Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the batch and config file later to change player slots, map, password etc. For more info, go to the Counter Strike Global Offensive Server guide", "Note" };
                    askTokenDialog = new string[] { "Please enter a VALID server token to show" + System.Environment.NewLine + "your server in the server list(Optional)", "Create server's startup files" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/n1XcFu5F";
                    break;

                case "german":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { "Eine Batch Datei wird benötigt, um den Server zu starten, sowie eine server.cfg. Sollen diese automatisch erstellt werden, oder nicht? Die Batch Datei wird im Server Ordner generiert und die server.cfg befindet sich in: " + serverFolderpath + @"\csgo\cfg\server.cfg", "Erstellen Sie die Startdateien des Servers" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Fehler beim Erstellen der Server-Startdateien! Sie können diese Dateien mithilfe des Codes des Server-Guides selbst erstellen.", "Erstellen Sie die Startdateien des Servers" };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Sie Counter Strike Global Offensive Server" };
                    importantNoteMsgBox = new string[] { "WICHTIG: Du kannst diese Dateien später noch editieren. Für mehr Informationen, öffne den Counter Strike Global Offensive Server Guide.", "Notieren" };
                    askTokenDialog = new string[] { "Bitte geben Sie ein GÜLTIGES Server-Token ein, um Ihren" + System.Environment.NewLine + "Server in der Serverliste anzuzeigen(optional)", "Erstellen Sie die Startdateien des Servers" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/n1XcFu5F";
                    break;
            }


            //Check if server's exectable exists
            if (System.IO.File.Exists(serverFolderpath + @"\srcds.exe"))
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
        private void finishInstallationSvenCoop()
        {
            string serverFolderpath = steamCmdSpeicherort.Substring(steamCmdSpeicherort.IndexOf(":") - 1, steamCmdSpeicherort.LastIndexOf("\\")) + "\\" + serverFolderName;
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
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { "Do you want to create a batch file for starting the server?The Batch file will be created in the server's folder.It is optional.The server can run without it", "Create Batch file" };
                    failedToCreateServerBatchFileMsgBox = new string[] { "Failed to create Batch File!You can create the file yourself using the code in the server's guide", "Create Batch File" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Sven Coop Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the batch file later to change player slots, map, password etc. For more info, go to the Sven Coop Server guide", "Note" };
                    lanOrInternet = new string[] { "Please select if the server will run via lan or on the internet", "Create batch file" };
                    break;
                case "german":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { "Möchtest du eine Batch Datei zum starten des Servers erstellen? Diese Datei wir in deinem Server Ordner erstellt. Dies ist optional, der Server kann auch ohne diese Datei starten.", "Erstellen Sie eine Batchdatei" };
                    failedToCreateServerBatchFileMsgBox = new string[] { "Erstellung der Batch Datei fehlgeschlagen! Du kannst diese auch selbst erstellen, mithilfe des Codes im Server-Guide.", "Erstellen Sie eine Batchdatei" };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Sie Sven Coop Server" };
                    importantNoteMsgBox = new string[] { "WICHTIG:Sie können die Batchdatei später bearbeiten, um die Spieleranzahl, Karten usw. zu ändern. Für weitere Informationen öffnen Sie die, öffne den Sven Coop Guide.", "Notieren" };
                    lanOrInternet = new string[] { "Wähle aus, ob der Server im Lan oder im Internet verfügbar sein soll.", "Erstellen Sie eine Batchdatei" };
                    break;
            }
            //Check if server's exectable exists
            if (System.IO.File.Exists(serverFolderpath + @"\SvenDS.exe"))
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
                        serverFolderName = null;
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
            serverFolderName = null;
        }
        private void finishInstallationCodBO3()
        {
            string serverFolderpath = steamCmdSpeicherort.Substring(steamCmdSpeicherort.IndexOf(":") - 1, steamCmdSpeicherort.LastIndexOf("\\")) + "\\" + serverFolderName;
            //Check Langouage
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
            //Check if server's exectable exists
            if (System.IO.File.Exists(serverFolderpath + @"\UnrankedServer\Launch_Server.bat"))
            {
                MessageBox.Show(startServerInfo[0], startServerInfo[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(importantNoteMsgBox[0], importantNoteMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(serverFolderpath);
            }
            else
            {
                MessageBox.Show(installationFailedMsgBox[0], installationFailedMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            serverFolderName = null;
        }

        private void finishInstallationKf2()
        {
            string serverFolderpath = steamCmdSpeicherort.Substring(steamCmdSpeicherort.IndexOf(":") - 1, steamCmdSpeicherort.LastIndexOf("\\")) + "\\" + serverFolderName;
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
            //Check if server's exectable exists
            if (System.IO.File.Exists(serverFolderpath + @"\KF2Server.bat"))
            {
                MessageBox.Show(startServerInfo[0], startServerInfo[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(importantNoteMsgBox[0], importantNoteMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(serverFolderpath);
            }
            else
            {
                MessageBox.Show(installationFailedMsgBox[0], installationFailedMsgBox[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            serverFolderName = null;
        }
        private void finishInstallationL4d()
        {
            string serverFolderpath = steamCmdSpeicherort.Substring(steamCmdSpeicherort.IndexOf(":") - 1, steamCmdSpeicherort.LastIndexOf("\\")) + "\\" + serverFolderName;
            Process.Start(serverFolderpath);
        }

        private void finishInstallationRust()
        {
            string serverFolderpath = steamCmdSpeicherort.Substring(steamCmdSpeicherort.IndexOf(":") - 1, steamCmdSpeicherort.LastIndexOf("\\")) + "\\" + serverFolderName;
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
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { @"You have to create a batch file and a configuration file in order to start the server.You want it to be created automatically or create it yourself?The Batch file will be created in the server's folder and the configuration file will be created at: " + serverFolderpath + @"\server\my_server_identity\cfg\server.cfg", "Create server's startup files" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Failed to create server's startup files!You can create the files yourself using the code in the server's guide", "Create server's startup files" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Rust Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the batch and config file later to change player slots, map, password etc. For more info, go to the Rust Server guide", "Note" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/7AAK7itF";
                    break;

                case "german":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { "Eine Batch Datei wird benötigt, um den Server zu starten, sowie eine server.cfg. Sollen diese automatisch erstellt werden, oder nicht? Die Batch Datei wird im Server Ordner generiert und die server.cfg befindet sich in: " + serverFolderpath + @"\server\my_server_identity\cfg\server.cfg", "Erstellen Sie die Startdateien des Servers" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Fehler beim Erstellen der Server-Startdateien! Sie können diese Dateien mithilfe des Codes des Server-Guides selbst erstellen.", "Erstellen Sie die Startdateien des Servers" };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Sie Rust Server" };
                    importantNoteMsgBox = new string[] { "WICHTIG: Du kannst diese Dateien später noch editieren. Für mehr Informationen, öffne den Rust Server Guide.", "Notieren" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/7AAK7itF";
                    break;
            }
            //Check if server's exectable exists
            if (System.IO.File.Exists(serverFolderpath + @"\RustDedicated.exe"))
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
            serverFolderName = null;
        }

        private void finishInstallationARK()
        {
            string serverFolderpath = steamCmdSpeicherort.Substring(steamCmdSpeicherort.IndexOf(":") - 1, steamCmdSpeicherort.LastIndexOf("\\")) + "\\" + serverFolderName;
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
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { @"You have to create a batch file in order to start the server.You want it to be created automatically or create it yourself?The Batch file will be created at: " + serverFolderpath + @"\ShooterGame\Binaries\Win64\Run.bat", "Create server's startup files" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Failed to create server's startup files!You can create the files yourself using the code in the server's guide", "Create server's startup files" };
                    installationFailedMsgBox = new string[] { "It looks like the installation failed!", "Install Ark:Survival Evolved Server" };
                    importantNoteMsgBox = new string[] { "IMPORTANT NOTE: You can edit the batch and config file later to change player slots, map, password etc. The config file is located at:" + serverFolderpath + @"\ShooterGame\Saved\Config\WindowsServer\GameUserSettings.ini .For more info, go to the Ark:Survival Evolved Server guide", "Note" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/e3f89ijz";
                    break;

                case "german":
                    askUserToCreateConfigAndBatchFileMsgBox = new string[] { "Eine Batch Datei wird benötigt, um den Server zu starten. Sollen diese automatisch erstellt werden, oder nicht? Die Batch Datei befindet sich in: " + serverFolderpath + @"\ShooterGame\Binaries\Win64\Run.bat", "Erstellen Sie die Startdateien des Servers" };
                    failedToCreateServerStartupFilesMsgBox = new string[] { "Fehler beim Erstellen der Server-Startdateien! Sie können diese Dateien mithilfe des Codes des Server-Guides selbst erstellen.", "Erstellen Sie die Startdateien des Servers" };
                    installationFailedMsgBox = new string[] { "Die Installation ist fehlgeschlagen!", "Installieren Sie Ark:Survival Evolved Server" };
                    importantNoteMsgBox = new string[] { "WICHTIG: Du kannst diese Dateien später noch editieren.Die Konfiguration findest du hier: " + serverFolderpath + @"\ShooterGame\Saved\Config\WindowsServer\GameUserSettings.ini. Für mehr Informationen, öffne den Ark:Survival Evolved.", "Notieren" };
                    configFileURLforEachLangouage = "https://pastebin.com/raw/mQCitqtv";
                    break;
            }
            //Check if server's exectable exists
            if (System.IO.File.Exists(serverFolderpath + @"\ShooterGame\Binaries\Win64\ShooterGameServer.exe"))
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
            serverFolderName = null;

        }
        private void finishInstallation()
        {
            switch (serverFolderName)
            {
                //COUNTER STRIKE 1.6
                case "cs":
                    finishInstallationCS16();
                    break;
                //GMOD
                case "gmod":
                    finishInstallationGmod();
                    break;
                //LEFT 4 DEAD 2
                case "l4d2":
                    finishInstallationLeft4dead2();
                    break;
                //COUNTER STRIKE SOURCE
                case "css":
                    finishInstallationCSSOURCE();
                    break;
                case "csgo":
                    finishInstallationCSGO();
                    break;
                case "svencoop":
                    finishInstallationSvenCoop();
                    break;
                case "bo3":
                    finishInstallationCodBO3();
                    break;
                case "rust":
                    finishInstallationRust();
                    break;
                case "kf2":
                    finishInstallationKf2();
                    break;
                case "l4d":
                    finishInstallationL4d();
                    break;
                case "ark":
                    finishInstallationARK();
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
            if (Properties.Settings.Default.language == "english")
            {
                englishToolStripMenuItem1.PerformClick();
            }
            else if (Properties.Settings.Default.language == "german")
            {
                germanToolStripMenuItem.PerformClick();
            }

            //check for Update and notify the user
            string availableUpdateVersion;
            //the version of the tool
            string toolVersion = "2.7";
            //
            WebClient client = new WebClient();
            availableUpdateVersion = client.DownloadString("https://pastebin.com/raw/mVYcG7tc");
            //If the version from the pastebin does not equal with the tool's, version notify the user
            if (availableUpdateVersion != toolVersion)
            {
                if (MessageBox.Show("An update is available! Do you want to download it?", "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //Latest version of the tool download website
                    Process.Start("https://github.com/Zeromix9/Server-Creation-Tool/releases");
                }
            }
            //
        }
        string steamCMDFolder;
        private void Button7_Click(object sender, EventArgs e)
        {
            if (OpenSteamCmd() == true)
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
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            try
            {
                //check if steamCMD exists
                if (File.Exists(steamCmdSpeicherort))
                {
                    installSrvProcess = System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./l4d/ +app_update 222840 validate");
                    disableControls();
                    serverFolderName = "l4d";
                    SteamCMDtimer.Enabled = true;
                    SteamCMDtimer.Start();

                }
                else
                {
                    ChangeLocateSteamCMDStatus("SteamCMD could not be found!Please select its location again", "Install Left 4 Dead Server", "SteamCMD konnte nicht gefunden werden. Bitte lokalisieren sie erneut.");

                }
            }
            catch (Exception) {; } //Left 4 Dead 2
        }
        private void LangouageToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

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
            serverStartupFilesToolStripMenuItem.Text = "Server Startup Files";
            richTextBox1.Text = "1.Click " + @"""Download SteamCMD""" + " (If you haven't)" + Environment.NewLine + "2.Locate your SteamCMD(If it isn't located)" + Environment.NewLine + "3.Choose your server" + Environment.NewLine + "4.Type" + @"""quit,""" + "if the download was successful" + Environment.NewLine + "5.Create server startup files(Optional for some games)" + Environment.NewLine + Environment.NewLine + "Note: This Tool will ONLY install the Server, it can´t forward any ports for you!";
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
            serverStartupFilesToolStripMenuItem.Text = "Server Start Dateien";
            richTextBox1.Text = "1.Klicke auf " + @"""Download SteamCMD""" + " (Sollte sie nicht bereits vorhanden sein.)" + Environment.NewLine + "2.Lokalisiere deine SteamCMD(Wenn diese nicht bereits lokalisiert sein sollte)" + Environment.NewLine + "3.Wähle deinen Server aus." + Environment.NewLine + "4.Tippe " + @"""quit""" + " ein, wenn der Download beendet wurde." + Environment.NewLine + "5.Erstelle Server Start Dateien. (Für manche Server optional.)" + Environment.NewLine + Environment.NewLine + "Hinweis: Dieses Programm installiert NUR die Server, es wird keine Ports für dich freigeben!";
        }

        private void ARKSurvivalEvolvedToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            serverFolderName = "ark";
            finishInstallationARK();
        }

        private void CounterStrikeSourceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            serverFolderName = "css";
            finishInstallationCSSOURCE();
        }


        private void ServerStartupFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void RustToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            serverFolderName = "rust";
            finishInstallationRust();
        }

        private void CounterStrikeGlobalOffensiveToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            serverFolderName = "csgo";
            finishInstallationCSGO();
        }

        private void SvenCoopToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            serverFolderName = "svencoop";
            finishInstallationSvenCoop();
        }

        private void CounterStrike16ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            serverFolderName = "cs";
            finishInstallationCS16();
        }

        private void GarrysModToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            serverFolderName = "gmod";
            finishInstallationGmod();
        }

        private void Left4Dead2ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            serverFolderName = "l4d2";
            finishInstallationLeft4dead2();
        }
    }
}