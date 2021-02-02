/*
 * Created by SharpDevelop.
 * User: Zeromix
 * Date: 28.05.2016
 * Time: 16:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using ByteSizeLib;
using Server_Creation_Tool.myClasses;
using statServer_Creation_Tool;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server_Creation_Tool
{

    /// <summary>
    /// Description of MainForm.
    /// </summary>
    ///

    public partial class MainForm : Form
    {
        Process installSteamSrvProcess;
        private CancellationTokenSource cancellationSource;
        private bool eventsStopped;
        //selectedGameRoot is essentially the name of the server's root folder
        string selectedGameRoot = null;
        string[] guideLink;
        string[] srvRunFiles;
        string[] cfgFilesLoc;
        string steamCmdInstCode;
        string[] cfgFilesLink;
        Action cfgFunc;
        Action batFunc;


        //Create new instances of the classes
        METHODSandFUNCTIONS methClass = new METHODSandFUNCTIONS();
        steamGameInstallFunc steamSrvInstallFunc = new steamGameInstallFunc();
        global_Variables gVars = new global_Variables();
        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            //load UI theme
            if (Properties.Settings.Default.theme == "black")
            { Elegant.Ui.SkinManager.LoadEmbeddedTheme(Elegant.Ui.EmbeddedTheme.Office2007Black, Elegant.Ui.Product.Ribbon); }
            else
            { Elegant.Ui.SkinManager.LoadEmbeddedTheme(Elegant.Ui.EmbeddedTheme.Office2007Blue, Elegant.Ui.Product.Ribbon); }
            noInternetTab.Visible = false;
        }

        private void cleanHandlers()
        {
            ApplicationCommands.ForceStopInstall.Executed -= forceStopSteamSvrInstall;
            ApplicationCommands.InstallServerBtn.Executed -= instGeneralSteamSrv;
            ApplicationCommands.runSrv.Executed -= runGeneralSteamSrv;
            ApplicationCommands.updateSrv.Executed -= updtGeneralSteamSrv;
            ApplicationCommands.deleteSrv.Executed -= delGeneralSteamSrv;
            ApplicationCommands.reinstallSrv.Executed -= reinstGeneralSteamSrv;
            ApplicationCommands.editConfSrv.Executed -= editGeneralCfgFiles;
            methClass.RemoveClickEvent(extraBtn1);
            methClass.RemoveClickEvent(extraBtn2);
            methClass.RemoveClickEvent(extraBtn3);
            methClass.RemoveClickEvent(extraBtn4);

        }
        //-----------------------------------------
        private void setGeneralCommandHandlers()
        {
            cleanHandlers();
            ApplicationCommands.ForceStopInstall.Executed += new Elegant.Ui.CommandExecutedEventHandler(forceStopSteamSvrInstall);
            ApplicationCommands.InstallServerBtn.Executed += new Elegant.Ui.CommandExecutedEventHandler(instGeneralSteamSrv);
            ApplicationCommands.runSrv.Executed += new Elegant.Ui.CommandExecutedEventHandler(runGeneralSteamSrv);
            ApplicationCommands.updateSrv.Executed += new Elegant.Ui.CommandExecutedEventHandler(updtGeneralSteamSrv);
            ApplicationCommands.deleteSrv.Executed += new Elegant.Ui.CommandExecutedEventHandler(delGeneralSteamSrv);
            ApplicationCommands.reinstallSrv.Executed += new Elegant.Ui.CommandExecutedEventHandler(reinstGeneralSteamSrv);
            ApplicationCommands.editConfSrv.Executed += new Elegant.Ui.CommandExecutedEventHandler(editGeneralCfgFiles);
            //+= (_, _2) =>-
            //Other
            // ApplicationCommands.csgoEasyStgSet.Executed += (_, _2) => steamServerInstallFunc.createBatFlCSGO(steamCMDandServersFolder +"\\" + gVars.csgoRootFold);
        }

        public async void beginInstallationSTEAMAsync(string steamSrvInstCode)
        {
            await srvInstChckTimerStop();
            installSteamSrvProcess = new Process();
            installSteamSrvProcess.StartInfo.FileName = steamCMDandServersFolder + @"\steamcmd.exe";
            installSteamSrvProcess.StartInfo.Arguments = steamSrvInstCode;
            installSteamSrvProcess.Start();
            SteamServertimer.Enabled = true;
            SrvInstallingSet();
            disableControls();
        }

        string steamCMDandServersFolder;
        PleaseWaitFrm pleaseWaitFrom = new PleaseWaitFrm();
        private void downloadSteamAndInstServer(string steamSrvInstCode)
        {
            if (methClass.HasInternet() == true)
            {
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    if (System.IO.File.Exists(steamCMDandServersFolder + @"\steamcmd.exe"))
                    {
                        beginInstallationSTEAMAsync(steamSrvInstCode);
                        return;
                    }
                    else
                    {
                        steamCMDandServersFolder = methClass.askUserInstallDir();
                        //if we get "" then it means the user canceled
                        if (steamCMDandServersFolder.Trim() == "" || steamCMDandServersFolder.Trim() == "empty") { return; }
                        Elegant.Ui.MessageBox.Show(steamCMDandServersFolder.ToString(), "Selected Path");
                        try
                        {
                            pleaseWaitFrom.Show();
                            disableControls();
                            client.DownloadFileAsync(new Uri("https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip"), steamCMDandServersFolder + @"\steamcmd.zip");
                            client.DownloadFileCompleted += new AsyncCompletedEventHandler(downloadSteamCMDandInstall);
                        }
                        catch (Exception a)
                        {
                            log.LogAppend(a.ToString());
                            Elegant.Ui.MessageBox.Show(downloadFailed2[0], downloadFailed2[1], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error);
                            pleaseWaitFrom.Hide();
                            enableControls();
                            steamCMDandServersFolder = "";
                            return;
                        }
                    }
                }
            }
            else
            {
                Elegant.Ui.MessageBox.Show(downloadFailed[0], downloadFailed[1], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error);
                pleaseWaitFrom.Hide();
                enableControls();
                return;
            }
        }
        string installCode = null;
        public void downloadSteamCMDandInstall(object sender, AsyncCompletedEventArgs e)
        {
            //nun wurde die zip-Datei fertig gedownloadet.
            string steamCMDzip = Path.Combine(steamCMDandServersFolder + @"\steamcmd.zip");
            //try to extract the zip file
            try
            {
                ZipFile.ExtractToDirectory(steamCMDzip, steamCMDandServersFolder);
            }
            catch (Exception a)
            {
                log.LogAppend(a.ToString());
                Elegant.Ui.MessageBox.Show(downloadFailed3[0], downloadFailed3[1], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error);
                pleaseWaitFrom.Hide();
                enableControls();
                return;
            }
            //try to delete the zip file
            try
            {
                File.Delete(steamCMDzip);
            }
            catch { }
            Properties.Settings.Default.lastSavedSteamCMDAndServerLoc = steamCMDandServersFolder;
            Properties.Settings.Default.Save();
            pleaseWaitFrom.Hide();
            beginInstallationSTEAMAsync(installCode);
        }
        string currentOprName;
        private void setCurrentOpr(bool isEnabled)
        {
            if (isEnabled)
            {
                currentOperRibGrp.Visible = true;
                currentOperNameLbl.Text = currentOprName;
            }
            else
            { currentOperRibGrp.Visible = false; }
            this.Refresh();
        }
        private void SrvInstallingSet()
        {
            installSvrBtn.Text = "  Force Stop";
            installSvrBtn.ForeColor = System.Drawing.Color.Red;
            installSvrBtn.DefaultSmallImage = Properties.Resources._1263;
            installSvrBtn.Command = ApplicationCommands.ForceStopInstall;
            setCurrentOpr(true);
        }

        private void steamGame_Tick(object sender, EventArgs e)
        {
            try
            {
                if (installSteamSrvProcess.HasExited)
                {
                    setCurrentOpr(false);
                    SteamServertimer.Enabled = false;
                    enableControls();
                    //Complete installation and ask user to create batch-config file
                    if (!isUpdate)
                    { steamSrvInstallFunc.finishInstallationSTEAM(steamCMDandServersFolder, selectedGameRoot); }
                    else { }  //actions will be added here in the future       
                    gameInstChckTimerStart();
                }
            }
            catch (InvalidOperationException)
            {
                setCurrentOpr(false);
                SteamServertimer.Enabled = false;
                gameInstChckTimerStart();
                enableControls();
            }
        }

        public void disableControls()
        {
            groupBox2.Enabled = false;
            langRibbon.Enabled = false;
            optionsRibbon.Enabled = false;
            changeSrvrInstFoldBtn.Enabled = false;
            actionsGrpBox.Enabled = false;
            extraGrpBox.Enabled = false;
            instGuideBtn.Enabled = true;
            groupBox7.Enabled = false;
            this.Refresh();
        }
        public void enableControls()
        {
            groupBox2.Enabled = true;
            langRibbon.Enabled = true;
            optionsRibbon.Enabled = true;
            changeSrvrInstFoldBtn.Enabled = true;
            actionsGrpBox.Enabled = true;
            extraGrpBox.Enabled = true;
            groupBox7.Enabled = true;
            this.BringToFront();
            this.Refresh();
        }

        string[] updateMessage = { };
        private void MainForm_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            steamCMDandServersFolder = Properties.Settings.Default.lastSavedSteamCMDAndServerLoc;
            //load last saved settings
            if (Properties.Settings.Default.lastSavedServerTab == "steam")
            { tabControl1.SelectedTabPage = tabPage1; }
            else if (Properties.Settings.Default.lastSavedServerTab == "non_steam")
            { tabControl1.SelectedTabPage = tabPage2; }

            //check for updates and select last used language
            if (Properties.Settings.Default.language == "english")
            {
                engBtn.PerformClick();
                updateMessage = new string[] { "An new update is available! Do you want to download it?", "Update Available" };
            }
            else if (Properties.Settings.Default.language == "german")
            {
                gerBtn.PerformClick();
                updateMessage = new string[] { "Ein Update ist verfügbar! Möchtest du es downloaden?", "Update verfügbar" };
            }

            Thread t2 = new Thread(delegate () { updateAvlbl = methClass.check4Updates(gVars.updateCheckingInfo[0], int.Parse(gVars.updateCheckingInfo[2])); });
            t2.Start();
            steamSrvInstallFunc.setMsgLang();
            setGeneralCommandHandlers();
            gameInstChckTimerStart();
        }

        private void generalTimer_Tick(object sender, EventArgs e)
        {

        }

        private void serverGroupBtn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/groups/ServerTool");
        }


        Form changeLogForm = new changeLogFrm();
        private void changeLogBtn_Click_1(object sender, EventArgs e)
        {
            changeLogForm.Show();
            changeLogForm.BringToFront();
        }


        private void serverGroupBtn_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/groups/ServerTool");
        }


        public bool aboutFormIsVisible = false;
        private AboutFrm aboutform = new AboutFrm();
        private void button6_Click(object sender, EventArgs e)
        {
            //check if the about form is show or not and open it or close it
            aboutform.Show();
            aboutform.BringToFront();
        }

        string[] downloadFailed;
        string[] downloadFailed2;
        string[] downloadFailed3;
        string[] forceStopInstSteamMsg;
        private void gerBtn_Click_1(object sender, EventArgs e)
        {
            Properties.Settings.Default.language = "german";
            Properties.Settings.Default.Save();

            //change UI text
            plannedFeaturesBtn.Text = "Geplante Features/Funktionen";
            changeLogBtn.Text = "Changelog";
            serverGroupBtn.Text = "Server Gruppe";
            basicInstrTxtBox.Text = "1. Klicke auf " + @"""Download SteamCMD""" + " (Sollte sie nicht bereits vorhanden sein.)" + Environment.NewLine + "2. Lokalisiere deine SteamCMD (Wenn diese nicht bereits lokalisiert sein sollte)" + Environment.NewLine + "3. Wähle deinen Server aus." + Environment.NewLine + "4. Tippe " + @"""quit""" + " ein, wenn der Download beendet wurde." + Environment.NewLine + "5. Erstelle Server Dateien (.bat und .cfg) (Für manche Server optional.)" + Environment.NewLine + @"6. Editiere deine Servereinstellungen (Optional) (Editiere die .cfg and .bat Datei)" + Environment.NewLine + "7. Starte den Server und los geht´s!" + Environment.NewLine + Environment.NewLine + "Hinweis: Dieses Programm installiert NUR die Server, es wird keine Ports für dich freigeben!";
            downloadFailed = new string[] { "Download fehlgeschladen. Bitte stelle sicher, dass du mit dem INternet verbunden bist.", "Download Server" };
            downloadFailed2 = new string[] { "Download fehlgeschlagen! Fehler Code 2." + Environment.NewLine + "Tritt unserer Steam Gruppe bei, um Hilfe zu erhalten.", "Download" };
            downloadFailed3 = new string[] { "Download fehlgeschlagen! Fehler Code 3." + Environment.NewLine + "Tritt unserer Steam Gruppe bei, um Hilfe zu erhalten.", "Download" };
            forceStopInstSteamMsg = new string[] { "Bist du sicher, dass du die Beendigung der Installation erzwingen möchtest? Es könnte Fortschritt verloren gehen.", "Erzwinge anhalten der Installation." };

        }

        private void engBtn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.language = "english";
            Properties.Settings.Default.Save();

            //change UI text
            plannedFeaturesBtn.Text = "Planned Features";
            changeLogBtn.Text = "Changelog";
            serverGroupBtn.Text = "Server Group";
            basicInstrTxtBox.Text = "1. Click " + @"""Download SteamCMD""" + " (If you haven't)" + Environment.NewLine + "2. Locate your SteamCMD (If it isn't located)" + Environment.NewLine + "3. Choose your server" + Environment.NewLine + "4. Type" + @"""quit""" + "if the download was successful" + Environment.NewLine + "5. Create server files (Optional for some games)" + Environment.NewLine + @"6. Edit your server's settings (Optional) (Edit .cfg and .bat files)" + Environment.NewLine + "7. Start the server and have fun!" + Environment.NewLine + Environment.NewLine + "Note: This Tool will ONLY install the Server, it can´t forward any ports for you!";
            downloadFailed = new string[] { "Download failed. Please check your internet connection!", "Download Server" };
            downloadFailed2 = new string[] { "Download failed! Error Code 2" + Environment.NewLine + "Join our group on steam for more help", "Download" };
            downloadFailed3 = new string[] { "Download failed! Error Code 3" + Environment.NewLine + "Join our group on steam for more help", "Download" };
            forceStopInstSteamMsg = new string[] { "Are you sure that you want to force the installation to quit? That could result in progress loss and you will possibly have to restart the installation from the beggining", "Force Stop Installation" };

        }

        private void plannedFeaturesBtn_Click(object sender, EventArgs e)
        {
            Elegant.Ui.MessageBox.Show("More Servers" +
              Environment.NewLine +
              "Installing SourceMod etc." +
               Environment.NewLine +
              "Maybe skins", "Planned Features");
        }

        private void button5_Click(object sender, EventArgs e)
        {

            string[] msgBox = null;
            if (methClass.getLangNum() == 0)
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
            else if (methClass.getLangNum() == 1)
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
            Elegant.Ui.MessageBox.Show(msgBox[0], msgBox[1], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Information);
        }

        private void refreshConnBtn_Click(object sender, EventArgs e)
        {

        }

        private void openServersFoldBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(steamCMDandServersFolder);
            }
            catch (Exception)
            {
                Elegant.Ui.MessageBox.Show("The servers folder could not be found! Please install a server first!", "Warning", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Warning);
            }
        }

        bool updateAvlbl = false;
        private void check4UpdatesBtn_Click(object sender, EventArgs e)
        {
            //type the version without comma!
            updtTimer.Enabled = true;
            Thread t2 = new Thread(delegate ()
            {
                updateAvlbl = methClass.check4Updates(gVars.updateCheckingInfo[0], int.Parse(gVars.updateCheckingInfo[2]));
            });
            t2.Start();
            Thread.Sleep(1000);
        }
        private void updtTimer_Tick(object sender, EventArgs e)
        {
            if (updateAvlbl == true)//3.0
            {
                updtTimer.Enabled = false;
                if (Elegant.Ui.MessageBox.Show(updateMessage[0], updateMessage[1], Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Process.Start(gVars.updateCheckingInfo[1]);
                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTabPage.TabIndex == 0)
            {
                methClass.searchGameInNavBar(steamSvrNavBar, steamGameSrchBox.Text);
            }
            else if (tabControl1.SelectedTabPage.TabIndex == 1)
            {
                //i will enable this when we add non-steam server support
                //  methodsClass.searchGameInNavBar(steamSvrNavBar, steamGameSrchBox.Text);
            }
            steamGameSrchBox.Focus();
        }

        private void tabControl1_SelectedTabPageChanged(object sender, Elegant.Ui.TabPageChangedEventArgs e)
        {
            if (tabControl1.SelectedTabPage.TabIndex == 0)
            {
                //save current tabpage
                Properties.Settings.Default.lastSavedServerTab = "steam";
                Properties.Settings.Default.Save();
            }
            else if (tabControl1.SelectedTabPage.TabIndex == 1)
            {
                //save current tabpage
                Properties.Settings.Default.lastSavedServerTab = "non_steam";
                Properties.Settings.Default.Save();
            }
        }

        private void button34_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://portforward.com/router.htm");
        }

        private void welcomeBtn_Click(object sender, EventArgs e)
        {
            srvPanelGrpBox.Visible = false;
        }

        private void gameChckResultPanelSet(bool installed, Elegant.Ui.ToggleButton t)
        {
            if (t.Pressed)
            {
                if (installed == true)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        installSvrBtn.Text = "    Run Server";
                        installSvrBtn.ForeColor = Color.Green;
                        installSvrBtn.DefaultSmallImage = Properties.Resources._1334;
                        installSvrBtn.Command = ApplicationCommands.runSrv;
                        actionsGrpBox.Enabled = true;
                        extraGrpBox.Enabled = true;
                        //show server size to txt box
                        var v = ByteSize.FromMegaBytes(steamSrvInstallFunc.srvDirSize);
                        string v2 = v.MegaBytes.ToString().Trim();
                        int CommaIndex = v2.IndexOf(",");
                        if (v2.Substring(0, CommaIndex).Length < 4)
                        { srvTxtBox.Text = v2.Substring(0, CommaIndex) + " MB"; }
                        else
                        { srvTxtBox.Text = v.GigaBytes.ToString().Substring(0, CommaIndex) + " GB"; }
                    });
                }
                else
                {
                    Invoke((MethodInvoker)delegate
                    {
                        installSvrBtn.Text = "    Install Server";
                        installSvrBtn.ForeColor = Color.Green;
                        installSvrBtn.DefaultSmallImage = Properties.Resources.output_onlinepngtools3;
                        installSvrBtn.Command = ApplicationCommands.InstallServerBtn;
                        actionsGrpBox.Enabled = false;
                        extraGrpBox.Enabled = false;
                        instGuideBtn.Enabled = true;
                        srvTxtBox.Text = "";
                    });
                }
            }
        }
        private void gameChck(Elegant.Ui.NavigationBar navBar)
        {
            foreach (Elegant.Ui.ToggleButton t in navBar.Controls)
            {
                singleGameChck(t);
                steamSvrNavBar.Refresh();
            }
        }
        private void singleGameChck(Elegant.Ui.ToggleButton t)
        {
            if (t.Tag.ToString() != "noTag")//notag is the tagof the "welcome" toggle button. We don't want to include it
            {
                if (steamSrvInstallFunc.checkIfGameInstalledSTEAM(steamCMDandServersFolder, t.Tag.ToString()) == true)
                {
                    t.Invoke((MethodInvoker)delegate
                    {
                        t.UseVisualThemeForForeground = false;
                        t.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                        t.ForeColor = Color.Green;
                    });
                    gameChckResultPanelSet(true, t);
                }
                else
                {
                    t.Invoke((MethodInvoker)delegate
                    {
                        t.UseVisualThemeForForeground = true;
                        t.Font = new Font("Segoe UI", 8, FontStyle.Regular);
                    });
                    gameChckResultPanelSet(false, t);
                }
            }
        }

        #region Seperate thread timers
        private void gameInstChckTimerStart()
        {
            if (cancellationSource == null)
            {
                eventsStopped = false;
                cancellationSource = new CancellationTokenSource();
                StartTimedEvent(cancellationSource.Token);
            }
        }
        private async Task srvInstChckTimerStop()
        {
            if (cancellationSource != null)
            {
                var cs = cancellationSource;
                cancellationSource = null;

                cs.Cancel();
                while (!eventsStopped) await Task.Delay(30);
                cs.Dispose();
            }
        }
        private async void StartTimedEvent(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                //CHECK IF GAME IS INSTALLED OR NOT AND ACT ACCORDINGLY
                if (tabControl1.SelectedTabPage.TabIndex == 0)
                {
                    gameChck(steamSvrNavBar);
                }
                else if (tabControl1.SelectedTabPage.TabIndex == 1)
                {
                    //gameChck(non_steamSvrNavBar);
                }
                try { await Task.Delay(4000, token); }
                catch (TaskCanceledException) { break; }
            }
            eventsStopped = true;
        }
        #endregion

        private void changeSrvrInstFoldBtn_Click(object sender, EventArgs e)
        {
            string selectedLoc = methClass.askUserInstallDir();
            if (selectedLoc != "")
            {
                steamCMDandServersFolder = selectedLoc;
                Properties.Settings.Default.lastSavedSteamCMDAndServerLoc = selectedLoc;
                Properties.Settings.Default.Save();
            }
        }

        private void instGuideBtn_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.language == "english")
            { System.Diagnostics.Process.Start(guideLink[0]); }
            else if (Properties.Settings.Default.language == "german")
            { System.Diagnostics.Process.Start(guideLink[1]); }
        }

        //SERVER PANEL FUNCTIONS
        #region server Panel Functions
        private void setGamePicBox(string size, Bitmap bmp)
        {
            int width = 0;
            int height = 0;
            int locX = 0;
            int locY = 0;
            if (size == "rec")
            {
                width = 253;
                height = 69;
                locX = 51;
                locY = 13;
            }
            else if (size == "cube")
            {
                width = 112;
                height = 110;
                locX = 119;
                locY = 13;
            }
            else if (size == "longRec")
            {
                width = 322;
                height = 69;
                locX = 17;
                locY = 12;
            }
            gameImgPicBox.Location = new Point(locX, locY);
            gameImgPicBox.BackgroundImage = new Bitmap(bmp);
            gameImgPicBox.Size = new Size(width, height);

        }
        private void customizeSettingsEditBtns(bool easyStgEditBtnEnabled)
        {
            Elegant.Ui.Button[] btns = { button10, button12, button13, button14 };
            //make all buttons invisible
            foreach (Elegant.Ui.Button b in btns)
            { b.Visible = false; }
            //show the amount of buttons proportional to the amount of config files and set their name with the name of the config files
            int indx = 0;
            foreach (string s in cfgFilesLoc)
            {
                string cfgFileName = methClass.getFlName(s);
                if (cfgFileName.Contains("Run.bat"))
                {
                    if (System.IO.File.Exists(steamCMDandServersFolder + "\\" + selectedGameRoot + s))
                    { btns[indx].Text = "Edit " + cfgFileName; btns[indx].Visible = true; }
                }
                else
                { btns[indx].Text = "Edit " + cfgFileName; btns[indx].Visible = true; }
                indx = indx + 1;
            }
            easyStgBtn.Visible = easyStgEditBtnEnabled;
            this.Refresh();
        }
        bool easyStgEdit = false;
        private void setActPnlCtrls(bool actBtn1 = false, bool easySettigEdit = false)
        {
            easyStgEdit = easySettigEdit;
            customizeSettingsEditBtns(easyStgEdit);
            act_Btn1.Visible = actBtn1;
            if (act_Btn1.Visible) { actionsGrpBox.Height = 162; }
            else { actionsGrpBox.Height = 138; }
        }
        private void setExtrPnlCtrls(bool installGuideBtn = false, bool extrBtn1 = false, bool extrBtn2 = false, bool extrBtn3 = false, bool extrBtn4 = false)
        {
            Elegant.Ui.Button[] btns = { instGuideBtn, extraBtn1, extraBtn2, extraBtn3, extraBtn4 };
            bool[] btnsState = { installGuideBtn, extrBtn1, extrBtn2, extrBtn3, extrBtn4 };
            //show-hide the necessary buttons
            int i = 0;
            foreach (Elegant.Ui.Button b in btns)
            { b.Visible = btnsState[i]; i = i + 1; }
            //place each button in the groupBox evenly
            int heightLoc = 13;
            foreach (Elegant.Ui.Button b in btns)
            { if (b.Visible) { b.Location = new Point(3, heightLoc); if (b.Height > 23) { heightLoc = heightLoc + b.Height + 1; } else { heightLoc = heightLoc + 24; } } }
            //set extra groupbox size
            int GrpBoxheight = 19;
            foreach (Elegant.Ui.Button b in btns)
            { if (b.Visible) { GrpBoxheight = GrpBoxheight + b.Height; } }
            extraGrpBox.Height = GrpBoxheight;
        }
        private void setGeneralCtrls()
        {
            editSettingsDropBtn.Visible = true;
            srvPanelGrpBox.Visible = true;
        }
        private void openFoldBtn_Click(object sender, EventArgs e)
        {
            try { Process.Start(steamCMDandServersFolder + @"\" + selectedGameRoot); }
            catch (Exception a) { log.LogAppend(a.ToString()); }
        }
        public void forceStopSteamSvrInstall(object sender, Elegant.Ui.CommandExecutedEventArgs e)
        {
            if (Elegant.Ui.MessageBox.Show(forceStopInstSteamMsg[0], forceStopInstSteamMsg[1], Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    installSteamSrvProcess.Kill();
                    installSteamSrvProcess.Close();
                }
                finally
                {
                    installSvrBtn.Command = ApplicationCommands.InstallServerBtn;
                    //steamCMD timer throws an exception and then i catch it and execute there all the neseccary actions 
                }
            }
        }
        public void instGeneralSteamSrv(object sender, Elegant.Ui.CommandExecutedEventArgs e)
        {
            currentOprName = "Installing " + srvPanelGrpBox.Text;
            isUpdate = false;
            //install server
            downloadSteamAndInstServer(steamCmdInstCode);
        }
        bool isUpdate = false;
        public void updtGeneralSteamSrv(object sender, Elegant.Ui.CommandExecutedEventArgs e)
        {
            currentOprName = "Updating " + srvPanelGrpBox.Text;
            isUpdate = true;
            //install server
            downloadSteamAndInstServer(steamCmdInstCode);
        }
        public void reinstGeneralSteamSrv(object sender, Elegant.Ui.CommandExecutedEventArgs e)
        {
            if (Elegant.Ui.MessageBox.Show("Continue? Reinstalling the server will delete all of your settings and progress. Make sure to backup what you need and continue", "Reinstall Server", Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                isUpdate = false;
                currentOprName = "Reinstalling " + srvPanelGrpBox.Text;
                //run the deleting process on seperate thread and wait to finish
                Task t = Task.Run(() => { Directory.Delete(steamCMDandServersFolder + @"\" + selectedGameRoot, true); });
                t.Wait();
                //install server
                downloadSteamAndInstServer(steamCmdInstCode);
            }
        }
        public async void delGeneralSteamSrv(object sender, Elegant.Ui.CommandExecutedEventArgs e)
        {
            if (Elegant.Ui.MessageBox.Show("Are you sure that you want to delete this server? Make sure to backup everything you need", "Delete Server", Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                currentOprName = "Deleting " + srvPanelGrpBox.Text;
                await srvInstChckTimerStop();
                disableControls();
                setCurrentOpr(true);
                Task t = Task.Run(() =>
                {
                    try
                    {
                        Directory.Delete(steamCMDandServersFolder + @"\" + selectedGameRoot, true);
                        Elegant.Ui.MessageBox.Show("The server has been sucessfully deleted", "Delete Server", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Information);
                    }
                    catch (Exception a)
                    { log.LogAppend(a.ToString()); MessageBox.Show("A program is probably using some of the server's files which couldn't be deleted", "Delete Server", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                });
                t.Wait();
                enableControls();
                setCurrentOpr(false);
                gameInstChckTimerStart();
            }
        }
        public void editGeneralCfgFiles(object sender, Elegant.Ui.CommandExecutedEventArgs e)
        {
            int indx = clickedEditCfgBtn - 1;
            if (clickedEditCfgBtn == 0)
            { }//9 is a special button. Not used now 
            //check if selected cfg file is .exe or editable text file
            else if (cfgFilesLoc[indx].Substring(cfgFilesLoc[indx].LastIndexOf(".") + 1) == "exe")
            {
                try
                { Process.Start(steamCMDandServersFolder + @"\" + selectedGameRoot + @"\" + cfgFilesLoc[indx]); }
                catch { MessageBox.Show("File not found"); }
            }
            else
            {
                try
                { Process.Start("notepad.exe", steamCMDandServersFolder + @"\" + selectedGameRoot + @"\" + cfgFilesLoc[indx]); }
                catch { MessageBox.Show("File not found"); }
            }
        }
        Process p = new Process();
        private void runGeneralSteamSrv(object sender, Elegant.Ui.CommandExecutedEventArgs e)
        {
            string gameFold = steamCMDandServersFolder + @"\" + selectedGameRoot;
            int i = 0;
            //checks if srvRunFiles variable contains more than one string. If it does, that means that a server can be started from an additional file

            if (srvRunFiles.Count() > 1)
            { if (System.IO.File.Exists(gameFold + @"\" + srvRunFiles[1])) { i = 1; } }
            else { i = 0; }

            p.StartInfo.FileName = srvRunFiles[i].Substring(srvRunFiles[i].LastIndexOf(@"\") + 1);
            p.StartInfo.WorkingDirectory = Path.GetDirectoryName(gameFold + srvRunFiles[i]);
            p.StartInfo.UseShellExecute = true;
            try
            { p.Start(); }
            catch (Exception a)
            {
                if (srvRunFiles[0].Contains(".bat") && srvRunFiles.Count() < 1)
                {
                    batFunc();
                    try { p.Start(); }
                    catch (Exception b) { log.LogAppend(b.ToString()); Elegant.Ui.MessageBox.Show("Failed to start the server!", "Error", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); }
                }
                else
                { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show("Failed to start the server!", "Error", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); }
            }
            p.Close();
        }
        #endregion
        private void daysToDieBtn_Click_1(object sender, EventArgs e)
        {
            setGeneralCommandHandlers();
            setGeneralCtrls();
            //Set Variables
            selectedGameRoot = gVars.days7RootFold;
            guideLink = gVars.days7GuideLink;
            steamCmdInstCode = gVars.days7InstCode;
            srvRunFiles = gVars.days7StartFilesLoc;
            cfgFilesLoc = gVars.days7ConfFilesLoc;
            //--------currently UNUSED(put that into a seperate function)---------//
            Elegant.Ui.ToggleButton self = (Elegant.Ui.ToggleButton)sender;
            Properties.Settings.Default.lastSelectedNavBarGame = self.TabIndex;
            Properties.Settings.Default.Save();
            //--------------------------------------------------------------------//
            //---------CUSTOMIZE PANEL-----------//
            //because the timer checks if all games are installed every 4 seconds, this piece of code checks immediately only the game that you clicked
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            //1)logos: "rec" means rectangle and "longRec" a wider rectangle. Use "cube" for cubic logo size 2)Game logo image
            setGamePicBox("rec", Properties.Resources._7DaysToDie_logo);
            srvPanelGrpBox.Text = "7 Days to Die";
            //enable-disable extra panel buttons. Leave arguments empty to disable buttons by default. 1)Installation Guide 2)Create Additional files 3,4)Customizable extra buttons
            setExtrPnlCtrls(true);
            //1)Enable customizable action button 2)Enable Easy setting edit button
            setActPnlCtrls();
            //-----------------------------------//
        }

        private void arkBtn_Click(object sender, EventArgs e)
        {
            setGeneralCommandHandlers();
            setGeneralCtrls();
            //Set Variables
            selectedGameRoot = gVars.arkRootFold;
            guideLink = gVars.arkGuideLink;
            steamCmdInstCode = gVars.arkInstCode;
            srvRunFiles = gVars.arkStartFilesLoc;
            cfgFilesLoc = gVars.arkConfFilesLoc;
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("cube", Properties.Resources.ark_survival_evolved_logo_FULL);
            srvPanelGrpBox.Text = "ARK: Survival Evolved";
            setExtrPnlCtrls(true);
            setActPnlCtrls();

        }

        private void CODblackOpsThreeBtn_Click(object sender, EventArgs e)
        {
            setGeneralCommandHandlers();
            setGeneralCtrls();
            //Set Variables
            selectedGameRoot = gVars.bo3RootFold;
            guideLink = gVars.bo3GuideLink;
            steamCmdInstCode = gVars.bo3InstCode;
            srvRunFiles = gVars.bo3StartFilesLoc;
            cfgFilesLoc = gVars.bo3ConfFilesLoc;
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("rec", Properties.Resources.CoD_BO3_logo);
            srvPanelGrpBox.Text = "Call of Duty: Black Ops 3";
            setExtrPnlCtrls(true);
            setActPnlCtrls();
        }

        private void csOnePointsixBtn_Click(object sender, EventArgs e)
        {
            setGeneralCommandHandlers();
            setGeneralCtrls();
            //Set Variables
            selectedGameRoot = gVars.cs16RootFold;
            guideLink = gVars.cs16GuideLink;
            steamCmdInstCode = gVars.cs16InstCode;
            srvRunFiles = gVars.cs16StartFilesLoc;
            cfgFilesLoc = gVars.cs16ConfFilesLoc;
            cfgFilesLink = gVars.cs16CfgFilelink;
            batFunc = null;
            cfgFunc = () => steamSrvInstallFunc.createCfgFlCS16(steamCMDandServersFolder + "\\" + selectedGameRoot, cfgFilesLink[methClass.getLangNum()], true, true);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("cube", Properties.Resources.cs_logo);
            srvPanelGrpBox.Text = "Counter Strike 1.6";
            setExtrPnlCtrls(true, true);
            extraBtn1.Text = "Premade server.cfg file";
            extraBtn1.Click += (_, _2) => cfgFunc();
            setActPnlCtrls();
        }

        private void CSsourceBtn_Click(object sender, EventArgs e)
        {
            setGeneralCommandHandlers();
            setGeneralCtrls();
            //Set Variables
            selectedGameRoot = gVars.cssRootFold;
            guideLink = gVars.cssGuideLink;
            steamCmdInstCode = gVars.cssInstCode;
            srvRunFiles = gVars.cssStartFilesLoc;
            cfgFilesLoc = gVars.cssConfFilesLoc;
            cfgFilesLink = gVars.cssCfgFilelink;
            batFunc = () => steamSrvInstallFunc.createBatFlCSS(steamCMDandServersFolder + "\\" + selectedGameRoot);
            cfgFunc = () => steamSrvInstallFunc.createCfgFlCSS(steamCMDandServersFolder + "\\" + selectedGameRoot, cfgFilesLink[methClass.getLangNum()], true, true);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("cube", Properties.Resources.css_logo);
            srvPanelGrpBox.Text = "Counter Strike: Source";
            setExtrPnlCtrls(true, true);
            extraBtn1.Text = "Premade server.cfg file";
            extraBtn1.Click += (_, _2) => cfgFunc();
            setActPnlCtrls();
        }

        private void CSGOBtn_Click(object sender, EventArgs e)
        {
            setGeneralCommandHandlers();
            setGeneralCtrls();
            //Set Variables
            selectedGameRoot = gVars.csgoRootFold;
            guideLink = gVars.csgoGuideLink;
            steamCmdInstCode = gVars.csgoInstCode;
            srvRunFiles = gVars.csgoStartFilesLoc;
            cfgFilesLoc = gVars.csgoConfFilesLoc;
            cfgFilesLink = gVars.CSGOCfgFilelink;
            batFunc = () => steamSrvInstallFunc.createBatFlCSGO(steamCMDandServersFolder + "\\" + selectedGameRoot);
            cfgFunc = () => steamSrvInstallFunc.createCfgFlCSGO(steamCMDandServersFolder + "\\" + selectedGameRoot, cfgFilesLink[methClass.getLangNum()], true, true);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("rec", Properties.Resources.csgo_logo);
            srvPanelGrpBox.Text = "Counter Strike: Global Offensive";
            setExtrPnlCtrls(true, true);
            extraBtn1.Text = "Premade server.cfg file";
            extraBtn1.Click += (_, _2) => cfgFunc();
            easyStgBtn.Click += (_, _2) => batFunc();
            setActPnlCtrls(false, true);
        }

        private void garrysModBtn_Click(object sender, EventArgs e)
        {
            setGeneralCommandHandlers();
            setGeneralCtrls();
            //Set Variables
            selectedGameRoot = gVars.gmodRootFold;
            guideLink = gVars.gmodGuideLink;
            steamCmdInstCode = gVars.gmodInstCode;
            srvRunFiles = gVars.gmodStartFilesLoc;
            cfgFilesLoc = gVars.gmodConfFilesLoc;
            cfgFilesLink = gVars.gmodCfgFilelink;
            batFunc = () => steamSrvInstallFunc.createBatFlGMOD(steamCMDandServersFolder + "\\" + selectedGameRoot);
            cfgFunc = () => steamSrvInstallFunc.createCfgFlGMOD(steamCMDandServersFolder + "\\" + selectedGameRoot, cfgFilesLink[methClass.getLangNum()], true, true);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("longRec", Properties.Resources.gmod_logo);
            srvPanelGrpBox.Text = "Garry's Mod";
            setExtrPnlCtrls(true, true);
            extraBtn1.Text = "Premade server.cfg file";
            extraBtn1.Click += (_, _2) => cfgFunc();
            setActPnlCtrls();
        }
        private void hurtWorldBtn_Click(object sender, EventArgs e)
        {
            setGeneralCommandHandlers();
            setGeneralCtrls();
            //Set Variables
            selectedGameRoot = gVars.hurtworldRootFold;
            guideLink = gVars.hurtworldGuideLink;
            steamCmdInstCode = gVars.hurtworldInstCode;
            srvRunFiles = gVars.hurtworldStartFilesLoc;
            cfgFilesLoc = gVars.hurtworldConfFilesLoc;
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("longRec", Properties.Resources.hurtworld_logo);
            srvPanelGrpBox.Text = "Hurtworld";
            setExtrPnlCtrls(true);
            setActPnlCtrls();
        }

        private void KillingFloorTwoBtn_Click(object sender, EventArgs e)
        {
            setGeneralCommandHandlers();
            setGeneralCtrls();
            //Set Variables
            selectedGameRoot = gVars.kf2RootFold;
            guideLink = gVars.kf2GuideLink;
            steamCmdInstCode = gVars.kf2InstCode;
            srvRunFiles = gVars.kf2StartFilesLoc;
            cfgFilesLoc = gVars.kf2ConfFilesLoc;
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("cube", Properties.Resources.kf2_logo);
            srvPanelGrpBox.Text = "Killing Floor 2";
            setExtrPnlCtrls(true);
            setActPnlCtrls();
        }

        private void l4dBtn_Click(object sender, EventArgs e)
        {
            setGeneralCommandHandlers();
            setGeneralCtrls();
            //Set Variables
            selectedGameRoot = gVars.l4dRootFold;
            guideLink = gVars.l4dGuideLink;
            steamCmdInstCode = gVars.l4dInstCode;
            srvRunFiles = gVars.l4dStartFilesLoc;
            cfgFilesLoc = gVars.l4dConfFilesLoc;
            cfgFilesLink = gVars.l4dCfgFilelink;
            batFunc = () => steamSrvInstallFunc.createBatFll4d(steamCMDandServersFolder + "\\" + selectedGameRoot);
            cfgFunc = () => steamSrvInstallFunc.createCfgFll4d(steamCMDandServersFolder + "\\" + selectedGameRoot, cfgFilesLink[methClass.getLangNum()], true, true);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("rec", Properties.Resources.l4d_logo);
            srvPanelGrpBox.Text = "Left 4 Dead";
            setExtrPnlCtrls(true, true);
            extraBtn1.Text = "Premade server.cfg file";
            extraBtn1.Click += (_, _2) => cfgFunc();
            setActPnlCtrls();
        }

        private void l4d2Btn_Click(object sender, EventArgs e)
        {
            setGeneralCommandHandlers();
            setGeneralCtrls();
            //Set Variables
            selectedGameRoot = gVars.l4d2RootFold;
            guideLink = gVars.l4d2GuideLink;
            steamCmdInstCode = gVars.l4d2InstCode;
            srvRunFiles = gVars.l4d2StartFilesLoc;
            cfgFilesLoc = gVars.l4d2ConfFilesLoc;
            cfgFilesLink = gVars.l4d2CfgFilelink;
            batFunc = () => steamSrvInstallFunc.createBatFll4d2(steamCMDandServersFolder + "\\" + selectedGameRoot);
            cfgFunc = () => steamSrvInstallFunc.createCfgFll4d2(steamCMDandServersFolder + "\\" + selectedGameRoot, cfgFilesLink[methClass.getLangNum()], true, true);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("rec", Properties.Resources.l4d2_logo);
            srvPanelGrpBox.Text = "Left 4 Dead 2";
            setExtrPnlCtrls(true, true);
            extraBtn1.Text = "Premade server.cfg file";
            extraBtn1.Click += (_, _2) => cfgFunc();
            setActPnlCtrls();
        }
        private void RustBtn_Click(object sender, EventArgs e)
        {
            setGeneralCommandHandlers();
            setGeneralCtrls();
            //Set Variables
            selectedGameRoot = gVars.rustRootFold;
            guideLink = gVars.rustGuideLink;
            steamCmdInstCode = gVars.rustInstCode;
            srvRunFiles = gVars.rustStartFilesLoc;
            cfgFilesLoc = gVars.rustConfFilesLoc;
            cfgFilesLink = gVars.rustCfgFilelink;
            batFunc = () => steamSrvInstallFunc.createBatFlRust(steamCMDandServersFolder + "\\" + selectedGameRoot);
            cfgFunc = () => steamSrvInstallFunc.createCfgFlRust(steamCMDandServersFolder + "\\" + selectedGameRoot, cfgFilesLink[methClass.getLangNum()], true, true);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("longRec", Properties.Resources.rust_logo);
            srvPanelGrpBox.Text = "Rust";
            setExtrPnlCtrls(true, true);
            extraBtn1.Text = "Premade server.cfg file";
            extraBtn1.Click += (_, _2) => cfgFunc();
            setActPnlCtrls();
        }

        private void SvenCoopBtn_Click(object sender, EventArgs e)
        {
            setGeneralCommandHandlers();
            setGeneralCtrls();
            //Set Variables
            selectedGameRoot = gVars.svenRootFold;
            guideLink = gVars.svenGuideLink;
            steamCmdInstCode = gVars.svenInstCode;
            srvRunFiles = gVars.svenStartFilesLoc;
            cfgFilesLoc = gVars.svenConfFilesLoc;
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("cube", Properties.Resources.svenCoop_logo);
            srvPanelGrpBox.Text = "Sven Coop";
            setExtrPnlCtrls(true);
            setActPnlCtrls();
        }

        int clickedEditCfgBtn;
        private void button10_Click(object sender, EventArgs e)
        {
            clickedEditCfgBtn = 1;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            clickedEditCfgBtn = 2;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            clickedEditCfgBtn = 3;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            clickedEditCfgBtn = 4;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            clickedEditCfgBtn = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.theme = "blue";
            Properties.Settings.Default.Save();
            Application.Restart();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.theme = "black";
            Properties.Settings.Default.Save();
            Application.Restart();
        }

        private void button32_Click(object sender, EventArgs e)
        {
            Process.Start("https://steamcommunity.com/groups/ServerTool");
        }

        private void createAdditFilesBtn_Click(object sender, EventArgs e)
        {
            Elegant.Ui.MessageBox.Show("This will overwrite your previous additional files", "Create additional files", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Warning);
        }

        private void editSettingsDropBtn_Click(object sender, EventArgs e)
        {
            customizeSettingsEditBtns(easyStgEdit);
        }

        private void appendLog()
        {
            if (!System.IO.Directory.Exists(steamCMDandServersFolder + gVars.logs))
            { System.IO.Directory.CreateDirectory(steamCMDandServersFolder + gVars.logs); }
            File.AppendAllText(steamCMDandServersFolder + gVars.logs + "\\" + DateTime.Today.ToString("dd-MM-yyyy") + "-log.txt", log.sb.ToString());
            log.sb.Clear();
        }
        private void logTimer_Tick(object sender, EventArgs e)
        {
            appendLog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            logTimer.Enabled = false;
            appendLog();
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            Process.Start(steamCMDandServersFolder + gVars.logs);
        }
    }
}

