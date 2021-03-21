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
        Action instSrv;
        Action runSrv;
        Action forceStopSrv;
        Action updtSrv;
        Action reinstSrv;

        Action cfgFunc;
        Action batFunc;
        Action customAct1;
        Action customAct2;
        Action extr1;
        Action extr2;
        Action extr3;
        Action srvFinishInstFunc;


        //Create new instances of the classes
        METHODSandFUNCTIONS methClass = new METHODSandFUNCTIONS();
        steamGameInstallFunc steamSrvFunc = new steamGameInstallFunc();
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

        private void cleanTempActVars()
        {
            cfgFunc = null;
            batFunc = null;
            customAct1 = null;
            customAct2 = null;
            srvFinishInstFunc = null;
            extr1 = null;
            extr2 = null;
            extr3 = null;
        }

        //-----------------------------------------
        private void setGeneralCommandHandlers()
        {
            installSvrBtn.Click += (_, _2) => instSrv();
            updtSvrBtn.Click += (_, _2) => updtSrv();
            delBtn.Click += (_, _2) => delGeneralSteamSrv();
            reinstBtn.Click += (_, _2) => reinstGeneralSteamSrv();
            extraBtn1.Click += (_, _2) => extr1();
            extraBtn2.Click += (_, _2) => extr2();
            extraBtn3.Click += (_, _2) => extr3();
            button8.Click += new EventHandler(editGeneralCfgFiles);
            button9.Click += new EventHandler(editGeneralCfgFiles);
            button10.Click += new EventHandler(editGeneralCfgFiles);
            button12.Click += new EventHandler(editGeneralCfgFiles);
            //+= (_, _2) =>-
            //Other
            // ApplicationCommands.csgoEasyStgSet.Executed += (_, _2) => steamServerInstallFunc.createBatFlCSGO(steamCMDandServersFolder +"\\" + gVars.csgoRootFold);
        }
        private void resetActs()
        {
            updtSrv = () => updtGeneralSteamSrv();
            instSrv = () => instGeneralSteamSrv();
            reinstSrv = () => reinstGeneralSteamSrv();
            forceStopSrv = () => forceStopSteamSvrInstall();
            runSrv = () => runGeneralSteamSrv();
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

        string steamCMDandServersFolder = "";
        PleaseWaitFrm pleaseWaitFrom = new PleaseWaitFrm();
        private void downloadSteamAndInstServer(string steamSrvInstCode)
        {
            if (methClass.HasInternet() == true)
            {
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    if (System.IO.File.Exists(steamCMDandServersFolder + @"\steamcmd.exe"))
                    { beginInstallationSTEAMAsync(steamSrvInstCode); return; }
                    else
                    {
                        //CHECK IF THE PREVIOUSLY SELECTED FOLDER EXISTS AND DOWNLOAD STEAMCMD. OTHERWISE ASK USER TO CREATE OR SELECT FOLDER
                        if (!Directory.Exists(steamCMDandServersFolder))
                        {
                            steamCMDandServersFolder = methClass.askUserInstallDir();
                            //if we get "" then it means the user canceled
                            if (steamCMDandServersFolder.Trim() == "" || steamCMDandServersFolder.Trim() == "empty") { return; }
                            Elegant.Ui.MessageBox.Show(steamCMDandServersFolder.ToString(), lang.selectedPath[gVars.lgNum]);
                            Properties.Settings.Default.lastSavedSteamCMDAndServerLoc = steamCMDandServersFolder;
                            Properties.Settings.Default.Save();
                        }
                        if (Directory.Exists(steamCMDandServersFolder + "\\" + selectedGameRoot))
                        {
                            if (Elegant.Ui.MessageBox.Show(lang.foldContainsSrvAlready[gVars.lgNum], serverPnl.Text, Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Question) == DialogResult.No)
                            { return; }
                        }
                        if (!System.IO.File.Exists(steamCMDandServersFolder + @"\steamcmd.exe"))
                        {
                            try
                            {
                                pleaseWaitFrom.Show();
                                disableControls();
                                client.DownloadFileAsync(new Uri("https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip"), steamCMDandServersFolder + @"\steamcmd.zip");
                                client.DownloadFileCompleted += new AsyncCompletedEventHandler(steamCMDInst);
                            }
                            catch (Exception a)
                            {
                                log.LogAppend(a.ToString());
                                Elegant.Ui.MessageBox.Show(lang.downloadFailedHelp[gVars.lgNumQuad], lang.downloadFailedHelp[gVars.lgNumQuad + 1], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error);
                                pleaseWaitFrom.Hide();
                                enableControls();
                                return;
                            }
                        }
                        else
                        { beginInstallationSTEAMAsync(steamSrvInstCode); return; }
                    }
                }
            }
            else
            {
                Elegant.Ui.MessageBox.Show(lang.downloadFailed[gVars.lgNumQuad], lang.downloadFailed[gVars.lgNumQuad + 1], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error);
                pleaseWaitFrom.Hide();
                enableControls();
                return;
            }
        }
        string installCode = null;
        public void steamCMDInst(object sender, AsyncCompletedEventArgs e)
        {
            //nun wurde die zip-Datei fertig gedownloadet.
            string steamCMDzip = Path.Combine(steamCMDandServersFolder + @"\steamcmd.zip");
            //try to extract the zip file
            try
            { ZipFile.ExtractToDirectory(steamCMDzip, steamCMDandServersFolder); }
            catch (Exception a)
            {
                log.LogAppend(a.ToString());
                Elegant.Ui.MessageBox.Show(lang.downloadFailedHelp[gVars.lgNumQuad], lang.downloadFailedHelp[gVars.lgNumQuad + 1], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error);
                pleaseWaitFrom.Hide();
                enableControls();
                return;
            }
            //try to delete the zip file
            try
            { File.Delete(steamCMDzip); }
            catch { }
            pleaseWaitFrom.Hide();
            beginInstallationSTEAMAsync(installCode);
        }
        string currentOprName;
        private void setCurrentOpr(bool isEnabled)
        {
            if (isEnabled)
            {
                currentTaskRibGrp.Visible = true;
                currentTaskNameLbl.Text = currentOprName;
            }
            else
            { currentTaskRibGrp.Visible = false; }
            this.Refresh();
        }
        private void SrvInstallingSet()
        {
            installSvrBtn.Text = lang.forceStop[gVars.lgNum];
            installSvrBtn.ForeColor = System.Drawing.Color.Red;
            installSvrBtn.DefaultSmallImage = Properties.Resources._1263;
            methClass.RemoveClickEvent(installSvrBtn);
            installSvrBtn.Click += (_, _2) => forceStopSrv();
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
                    //Complete installation
                    if (!usrStopped)
                    {
                        if (steamSrvFunc.checkIfGameInstalledSTEAM(steamCMDandServersFolder, selectedGameRoot))
                        { srvFinishInstFunc(); }
                        else
                        { log.LogAppend("Server installation failed: server folder size does not meet the required value"); Elegant.Ui.MessageBox.Show(lang.instFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); }
                    }
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
            optionsRibbon.Enabled = false;
            selectInstFoldBtn.Enabled = false;
            groupBox7.Enabled = false;
            this.Refresh();
        }
        public void enableControls()
        {
            groupBox2.Enabled = true;
            optionsRibbon.Enabled = true;
            selectInstFoldBtn.Enabled = true;
            groupBox7.Enabled = true;
            this.BringToFront();
            this.Refresh();
        }

        string[] updateMessage = { };
        private void MainForm_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            //load last saved settings
            steamCMDandServersFolder = Properties.Settings.Default.lastSavedSteamCMDAndServerLoc;
            if (Properties.Settings.Default.lastSavedServerTab == "steam")
            { tabControl1.SelectedTabPage = tabPage1; }
            else if (Properties.Settings.Default.lastSavedServerTab == "non_steam")
            { tabControl1.SelectedTabPage = tabPage2; }

            //check for updates and select last used language
            int i = methClass.getLangNum();
            if (i == 0)
            { engBtn.PerformClick(); }
            else if (i == 1)
            { gerBtn.PerformClick(); }
            ToolUpdateCheck();
            setGeneralCommandHandlers();
            gameInstChckTimerStart();
        }
        private void ToolUpdateCheck()
        {
            Thread t2 = new Thread(delegate ()
            {
                if (methClass.check4Updates(gVars.updateCheckingInfo[0], int.Parse(gVars.updateCheckingInfo[2])) == true)
                {
                    if (MessageBox.Show(lang.toolUpdtAvailb[gVars.lgNumQuad], lang.toolUpdtAvailb[gVars.lgNumQuad + 1], MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    { Process.Start(gVars.updateCheckingInfo[1]); }
                }
            });
            t2.Start();
            Thread.Sleep(400);
        }
        private void generalTimer_Tick(object sender, EventArgs e)
        {
            // if (steamCMDandServersFolder == "" || steamCMDandServersFolder == "emtpy")
            //  { await srvInstChckTimerStop(); }
            //  else
            //  { gameInstChckTimerStart(); }
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

        private void setLang(string lang, int lgNum1, int lgNumQuad1)
        {
            Properties.Settings.Default.language = lang;
            Properties.Settings.Default.Save();
            gVars.lgNum = lgNum1;
            gVars.lgNumQuad = lgNumQuad1;
            setCtrlLang();
            welcomeBtn_Click(new object(), new EventArgs());
            welcomeBtn.Select();
        }

        private void gerBtn_Click_1(object sender, EventArgs e)
        {
            setLang("german", 1, 2);
        }

        private void engBtn_Click(object sender, EventArgs e)
        {
            setLang("english", 0, 0);
        }
        private void setCtrlLang()
        {
            aboutBtn.Text = lang.aboutB[gVars.lgNum];
            donateBtn.Text = lang.donateB[gVars.lgNum];
            changeLogBtn.Text = lang.chngLogB[gVars.lgNum];
            appLogsbtn.Text = lang.appLogs[gVars.lgNum];
            plannedFeaturesBtn.Text = lang.plannedFeatB[gVars.lgNum];
            serverGroupBtn.Text = lang.serverGrpB[gVars.lgNum];
            variousStuffBtn.Text = lang.varStuff[gVars.lgNum];
            langBtn.Text = lang.langB[gVars.lgNum];
            styleBtn.Text = lang.styleB[gVars.lgNum];
            chck4UpdatesBtn.Text = lang.updCheckB[gVars.lgNum];
            selectInstFoldBtn.Text = lang.selectInstFoldB[gVars.lgNum];
            openSrvsFoldBtn.Text = lang.openSrvsFoldB[gVars.lgNum];
            groupBox2.Text = lang.grpBox2[gVars.lgNum];
            generalSvrOptGroup.Text = lang.generalSrvOptGr[gVars.lgNum];
            optionsRibbon.Text = lang.otherOtpGr[gVars.lgNum];
            currentTaskRibGrp.Text = lang.currentTaskGr[gVars.lgNum];
            helpInfGrpBox.Text = lang.helpInfGr[gVars.lgNum];
            mainTabPage.Text = lang.mainMenuTab[gVars.lgNum];
            helpInfTab.Text = lang.helpInfGr[gVars.lgNum];
            searchGameLbl.Text = lang.searchGameLb[gVars.lgNum];
            updtSvrBtn.Text = lang.updateB[gVars.lgNum];
            reinstBtn.Text = lang.reinstB[gVars.lgNum];
            openFoldBtn.Text = lang.openFoldB[gVars.lgNum];
            delBtn.Text = lang.deleteB[gVars.lgNum];
            editSettingsDropBtn.Text = lang.editStgB[gVars.lgNum];
            easyStgBtn.Text = lang.easyStgEditB[gVars.lgNum];
            instGuideBtn.Text = lang.instGuideB[gVars.lgNum];
            groupBox7.Text = lang.grpBx7SrvInfo[gVars.lgNum];
            srvSizeLbl.Text = lang.srvSizeLb[gVars.lgNum];
            welcomeLbl.Text = lang.welcomeLb[gVars.lgNum];
            welcomeDesclbl.Text = lang.welcomeDescLb[gVars.lgNum];
            basicInstrLbl.Text = lang.basicInstrLb[gVars.lgNum];
            portFwRouterBtn.Text = lang.portFwRouterB[gVars.lgNum];
            basicInstrTxtBox.Text = lang.basicInstruct[gVars.lgNum];


            //set size and locations
            welcomeLbl.Location = new Point(lang.welcLbLoc[gVars.lgNumQuad], lang.welcLbLoc[gVars.lgNumQuad + 1]);
            welcomeDesclbl.Location = new Point(lang.welcDescLbLoc[gVars.lgNumQuad], lang.welcDescLbLoc[gVars.lgNumQuad + 1]);
            // installSvrBtn.Font = new Font("Microsoft Sans Serif", lang.svrRunInstStopBtnFontSz[lgNum]);
        }
        private void plannedFeaturesBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://trello.com/b/9dsN6TIT/server-creation-tool");
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
            "You can do this in the linked Group.","FAQ" };
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
            "Dies ist möglich in der verlinken Gruppe","FAQ" };
            }
            Elegant.Ui.MessageBox.Show(msgBox[0], msgBox[1], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Information);
        }

        private void refreshConnBtn_Click(object sender, EventArgs e)
        {

        }

        private void openServersFoldBtn_Click(object sender, EventArgs e)
        {
            try
            { Process.Start(steamCMDandServersFolder); }
            catch (Exception)
            { Elegant.Ui.MessageBox.Show(lang.generalError[gVars.lgNumQuad], lang.generalError[gVars.lgNumQuad + 1], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Warning); log.LogAppend(e.ToString()); }
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
            serverPnl.Visible = false;
        }

        private void gameChckResultPanelSet(bool installed, Elegant.Ui.ToggleButton t)
        {
            if (t.Pressed)
            {
                if (installed)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        installSvrBtn.Text = lang.run[gVars.lgNum];
                        installSvrBtn.ForeColor = Color.Green;
                        installSvrBtn.DefaultSmallImage = Properties.Resources._1334;
                        methClass.RemoveClickEvent(installSvrBtn);
                        installSvrBtn.Click += (_, _2) => runSrv();
                        actionsGrpBox.Enabled = true;
                        foreach (Control ctrl in extraGrpBox.Controls) { ctrl.Enabled = true; }
                        //show server size to txt box
                        new Thread(() =>
                        {
                            Thread.CurrentThread.IsBackground = true;
                            try
                            {
                                if (selectedGameRoot == ".emtpyF")
                                { return; }
                                var v = ByteSize.FromMegaBytes(methClass.ShowFolderSize(steamCMDandServersFolder + "\\" + selectedGameRoot));
                                string v2 = v.MegaBytes.ToString().Trim();
                                int CommaIndex = v2.IndexOf(",");

                                if (v2.Substring(0, CommaIndex).Length < 4)
                                {
                                    Invoke((MethodInvoker)delegate
                                    { srvSizeTxtBox.Text = v2.Substring(0, CommaIndex) + " MB"; });
                                }
                                else
                                {
                                    Invoke((MethodInvoker)delegate
                                    { srvSizeTxtBox.Text = v.GigaBytes.ToString().Substring(0, CommaIndex) + " GB"; });
                                }
                            }
                            catch (Exception a)
                            { log.LogAppend(a.ToString()); }
                        }).Start();

                    });
                }
                else
                {
                    Invoke((MethodInvoker)delegate
                    {
                        installSvrBtn.Text = lang.installB[gVars.lgNum];
                        installSvrBtn.ForeColor = Color.Green;
                        installSvrBtn.DefaultSmallImage = Properties.Resources.output_onlinepngtools3;
                        methClass.RemoveClickEvent(installSvrBtn);
                        installSvrBtn.Click += (_, _2) => instSrv();
                        actionsGrpBox.Enabled = false;
                        foreach (Control ctrl in extraGrpBox.Controls) { if (ctrl.Name.ToString() != "instGuideBtn") { ctrl.Enabled = false; } }
                        instGuideBtn.Enabled = true;
                        srvSizeTxtBox.Text = "";
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
            if (t.Tag.ToString() != "noTag")//notag is the tag of buttons, like the "welcome" button, which are not servers or are unfinished. We don't want to include it
            {
                if (steamSrvFunc.checkIfGameInstalledSTEAM(steamCMDandServersFolder, t.Tag.ToString()) == true)
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
                while (!eventsStopped) await Task.Delay(50);
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
                try { await Task.Delay(6000, token); }
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
            int i = methClass.getLangNum();
            if (i == 0)
            { System.Diagnostics.Process.Start(guideLink[0]); }
            else if (i == 1)
            { System.Diagnostics.Process.Start(guideLink[1]); }
        }

        //SERVER PANEL FUNCTIONS
        #region server Panel Functions
        private void srvNotAvailb()
        {
            Elegant.Ui.MessageBox.Show(lang.srvNotAvailb[gVars.lgNum], "Install " + serverPnl.Text + " server", Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Information);
        }
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
        private void customizeStgEditBtns(bool easyStgEditBtnEnabled)
        {
            Elegant.Ui.Button[] btns = { button8, button9, button10, button12 };
            //make all buttons invisible
            foreach (Elegant.Ui.Button b in btns)
            { b.Visible = false; }
            //show the amount of buttons proportional to the amount of config files and set their name with the name of the config files
            int indx = 0;
            try
            {
                if (cfgFilesLoc == null)
                { return; }
                foreach (string s in cfgFilesLoc)
                {
                    string cfgFileName = methClass.getFlName(s);
                    if (cfgFileName.Contains("Run.bat"))
                    {
                        if (System.IO.File.Exists(steamCMDandServersFolder + "\\" + selectedGameRoot + s))
                        { btns[indx].Text = lang.edit[gVars.lgNum] + cfgFileName; btns[indx].Visible = true; }
                    }
                    else
                    { btns[indx].Text = lang.edit[gVars.lgNum] + cfgFileName; btns[indx].Visible = true; }
                    indx = indx + 1;
                }
                easyStgBtn.Visible = easyStgEditBtnEnabled;
                this.Refresh();
            }
            catch (Exception a) { log.LogAppend(a.ToString()); }
        }
        bool easyStgEdit = false;
        private void setActPnlCtrls(bool actBtn1 = false, bool easySettigEdit = false)
        {
            easyStgEdit = easySettigEdit;
            customizeStgEditBtns(easyStgEdit);
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
            serverPnl.Visible = true;
        }
        private void openFoldBtn_Click(object sender, EventArgs e)
        {
            try { Process.Start(steamCMDandServersFolder + @"\" + selectedGameRoot); }
            catch (Exception a) { log.LogAppend(a.ToString()); }
        }
        bool usrStopped = false;
        public void forceStopSteamSvrInstall()
        {
            if (Elegant.Ui.MessageBox.Show(lang.forceStopInstSteamMsg[gVars.lgNumQuad], lang.forceStopInstSteamMsg[gVars.lgNumQuad + 1], Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Question) == DialogResult.Yes)
            {
                usrStopped = true;
                try
                {
                    installSteamSrvProcess.Kill();
                    installSteamSrvProcess.Close();
                }
                catch (Exception a) { log.LogAppend(a.ToString()); }
                finally
                {
                    methClass.RemoveClickEvent(installSvrBtn);
                    installSvrBtn.Click += (_, _2) => instSrv();
                }
            }
        }
        public void instGeneralSteamSrv()
        {
            currentOprName = lang.installing[gVars.lgNum] + serverPnl.Text;
            //install server
            downloadSteamAndInstServer(steamCmdInstCode);
        }
        public void updtGeneralSteamSrv()
        {
            if (Elegant.Ui.MessageBox.Show(lang.updtSrvMsg[gVars.lgNum], "Update server", Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Question) == DialogResult.Yes)
            {
                currentOprName = lang.updating[gVars.lgNum] + serverPnl.Text;
                //install server
                downloadSteamAndInstServer(steamCmdInstCode);
            }
        }
        public void reinstGeneralSteamSrv()
        {
            if (Elegant.Ui.MessageBox.Show(lang.reinstSrvAsk[gVars.lgNumQuad], lang.reinstSrvAsk[gVars.lgNumQuad + 1], Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                currentOprName = lang.reinstalling[gVars.lgNum] + serverPnl.Text;
                //run the deleting process on seperate thread and wait to finish
                Task t = Task.Run(() => { Directory.Delete(steamCMDandServersFolder + @"\" + selectedGameRoot, true); });
                t.Wait();
                //install server
                downloadSteamAndInstServer(steamCmdInstCode);
            }
        }
        public async void delGeneralSteamSrv()
        {
            if (Elegant.Ui.MessageBox.Show(lang.delSrv[gVars.lgNumQuad], lang.delSrv[gVars.lgNumQuad + 1], Elegant.Ui.MessageBoxButtons.YesNo, Elegant.Ui.MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                currentOprName = lang.deleting[gVars.lgNum] + serverPnl.Text;
                await srvInstChckTimerStop();
                disableControls();
                setCurrentOpr(true);
                Task t = Task.Run(() =>
                {
                    try
                    {
                        Directory.Delete(steamCMDandServersFolder + @"\" + selectedGameRoot, true);
                        Elegant.Ui.MessageBox.Show(lang.srvDelSuccess[gVars.lgNumQuad], lang.srvDelSuccess[gVars.lgNumQuad + 1], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Information);
                    }
                    catch (Exception a)
                    { log.LogAppend(a.ToString()); MessageBox.Show(lang.srvDelFail[gVars.lgNumQuad], lang.srvDelFail[gVars.lgNumQuad + 1], MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                });
                t.Wait();
                enableControls();
                setCurrentOpr(false);
                gameInstChckTimerStart();
            }
        }
        public void editGeneralCfgFiles(object sender, EventArgs e)
        {
            Elegant.Ui.Button btn = (Elegant.Ui.Button)sender;
            int indx = int.Parse(btn.Tag.ToString());
            if (indx == 0)
            { }//9 is a special button. Not used now 
            //check if selected cfg file is .exe or editable text file
            //we subtract 1 from indx because the tags of the buttons do not align with the array size of the cfg files (0 is taken by the easy settings editor button and thus is not thrown into the calculation by removing 1 )
            else if (cfgFilesLoc[indx - 1].Substring(cfgFilesLoc[indx - 1].LastIndexOf(".") + 1) == "exe")
            {
                try
                { Process.Start(steamCMDandServersFolder + @"\" + selectedGameRoot + @"\" + cfgFilesLoc[indx - 1]); }
                catch (Exception a) { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(lang.fileNotFound[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); }
            }
            else
            {
                try
                { Process.Start("notepad.exe", steamCMDandServersFolder + @"\" + selectedGameRoot + @"\" + cfgFilesLoc[indx - 1]); }
                catch (Exception a) { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(lang.fileNotFound[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); }
            }
        }
        Process p = new Process();
        private void runGeneralSteamSrv()
        {
            string gameFold = steamCMDandServersFolder + @"\" + selectedGameRoot;
            int i = 0;
            //checks if srvRunFiles variable contains more than one string. If it does, that means that a server can be started from an additional file

            if (srvRunFiles.Count() > 1)
            {
                if (System.IO.File.Exists(gameFold + @"\" + srvRunFiles[1])) { i = 1; }
                else { i = 0; }
            }
            p.StartInfo.FileName = srvRunFiles[i].Substring(srvRunFiles[i].LastIndexOf(@"\") + 1);
            p.StartInfo.WorkingDirectory = Path.GetDirectoryName(gameFold + srvRunFiles[i]);
            p.StartInfo.UseShellExecute = true;
            try
            { p.Start(); }
            catch (Exception a)
            {
                if (srvRunFiles[0].Contains(".bat") && srvRunFiles.Count() < 2)
                {
                    //   MessageBox.Show("piss");            
                    batFunc();
                    try { p.Start(); }
                    catch (Exception b) { log.LogAppend(b.ToString()); Elegant.Ui.MessageBox.Show(lang.srvStartFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); }
                }
                else
                { log.LogAppend(a.ToString()); Elegant.Ui.MessageBox.Show(lang.srvStartFail[gVars.lgNum], lang.error[gVars.lgNum], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); }
            }
            p.Close();
        }
        #endregion
        private void daysToDieBtn_Click_1(object sender, EventArgs e)
        {
            resetActs();
            setGeneralCtrls();
            //Set Variables
            cleanTempActVars();
            selectedGameRoot = gVars.days7RootFold;
            guideLink = gVars.days7GuideLink;
            steamCmdInstCode = gVars.days7InstCode;
            srvRunFiles = gVars.days7StartFilesLoc;
            cfgFilesLoc = gVars.days7ConfFilesLoc;
            srvFinishInstFunc = () => steamSrvFunc.finishInstallation7days();
            //--------currently UNUSED(put that into a seperate function)---------//
            Elegant.Ui.ToggleButton self = (Elegant.Ui.ToggleButton)sender;
            Properties.Settings.Default.lastSelectedNavBarGame = self.TabIndex;
            Properties.Settings.Default.Save();
            //--------------------------------------------------------------------//
            //---------CUSTOMIZE PANEL-----------//
            //because the timer checks if all games are installed every few seconds, this piece of code checks immediately only the game that you clicked
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            //1)logo size: "rec" means rectangle and "longRec" a wider rectangle. Use "cube" for cubic logo size 2)Game logo image
            setGamePicBox("rec", Properties.Resources._7DaysToDie_logo);
            serverPnl.Text = "7 Days to Die";
            //enable-disable extra panel buttons. Leave arguments empty to disable buttons by default. 1)Installation Guide 2)Create Additional files 3,4)Customizable extra buttons
            setExtrPnlCtrls(true);
            //1)Enable customizable action button 2)Enable Easy setting edit button
            setActPnlCtrls();
            //-----------------------------------//
        }

        private void arkBtn_Click(object sender, EventArgs e)
        {
            resetActs();
            setGeneralCtrls();
            //Set Variables
            cleanTempActVars();
            selectedGameRoot = gVars.arkRootFold;
            guideLink = gVars.arkGuideLink;
            steamCmdInstCode = gVars.arkInstCode;
            srvRunFiles = gVars.arkStartFilesLoc;
            cfgFilesLoc = gVars.arkConfFilesLoc;
            srvFinishInstFunc = () => steamSrvFunc.finishInstallationARK(steamCMDandServersFolder, selectedGameRoot);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("cube", Properties.Resources.ark_survival_evolved_logo_FULL);
            serverPnl.Text = "ARK: Survival Evolved";
            setExtrPnlCtrls(true);
            setActPnlCtrls();
        }

        private void CODblackOpsThreeBtn_Click(object sender, EventArgs e)
        {
            resetActs();
            setGeneralCtrls();
            cleanTempActVars();
            //Set Variables
            selectedGameRoot = gVars.bo3RootFold;
            guideLink = gVars.bo3GuideLink;
            steamCmdInstCode = gVars.bo3InstCode;
            srvRunFiles = gVars.bo3StartFilesLoc;
            cfgFilesLoc = gVars.bo3ConfFilesLoc;
            srvFinishInstFunc = () => steamSrvFunc.finishInstallationCodBO3(steamCMDandServersFolder, selectedGameRoot);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("rec", Properties.Resources.CoD_BO3_logo);
            serverPnl.Text = "Call of Duty: Black Ops 3";
            setExtrPnlCtrls(true);
            setActPnlCtrls();
        }

        private void csOnePointsixBtn_Click(object sender, EventArgs e)
        {
            resetActs();
            setGeneralCtrls();
            //Set Variables
            cleanTempActVars();
            selectedGameRoot = gVars.csRootFold;
            guideLink = gVars.csGuideLink;
            steamCmdInstCode = gVars.csInstCode;
            srvRunFiles = gVars.csStartFilesLoc;
            cfgFilesLink = gVars.csCfgFilelink;
            cfgFilesLoc = gVars.csConfFilesLoc;
            srvFinishInstFunc = () => steamSrvFunc.finishInstallationCS16(steamCMDandServersFolder, selectedGameRoot);
            cfgFunc = () => steamSrvFunc.createCfgFlCS16(steamCMDandServersFolder + "\\" + selectedGameRoot, cfgFilesLink[methClass.getLangNum()], true, true);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("cube", Properties.Resources.cs_logo);
            serverPnl.Text = "Counter Strike 1.6";
            setExtrPnlCtrls(true, true);
            extraBtn1.Text = lang.premadeCfg[gVars.lgNum];
            extr1 = () => cfgFunc();
            setActPnlCtrls();
        }

        private void CSsourceBtn_Click(object sender, EventArgs e)
        {
            resetActs();
            setGeneralCtrls();
            //Set Variables
            cleanTempActVars();
            selectedGameRoot = gVars.cssRootFold;
            guideLink = gVars.cssGuideLink;
            steamCmdInstCode = gVars.cssInstCode;
            srvRunFiles = gVars.cssStartFilesLoc;
            cfgFilesLoc = gVars.cssConfFilesLoc;
            cfgFilesLink = gVars.cssCfgFilelink;
            srvFinishInstFunc = () => steamSrvFunc.finishInstallationCSSOURCE(steamCMDandServersFolder, selectedGameRoot);
            batFunc = () => steamSrvFunc.createBatFlCSS(steamCMDandServersFolder + "\\" + selectedGameRoot);
            cfgFunc = () => steamSrvFunc.createCfgFlCSS(steamCMDandServersFolder + "\\" + selectedGameRoot, cfgFilesLink[methClass.getLangNum()], true, true);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("cube", Properties.Resources.css_logo);
            serverPnl.Text = "Counter Strike: Source";
            setExtrPnlCtrls(true, true);
            extraBtn1.Text = lang.premadeCfg[gVars.lgNum];
            extr1 = () => cfgFunc();
            setActPnlCtrls();
        }

        private void CSGOBtn_Click(object sender, EventArgs e)
        {
            resetActs();
            setGeneralCtrls();
            //Set Variables
            cleanTempActVars();
            selectedGameRoot = gVars.csgoRootFold;
            guideLink = gVars.csgoGuideLink;
            steamCmdInstCode = gVars.csgoInstCode;
            srvRunFiles = gVars.csgoStartFilesLoc;
            cfgFilesLoc = gVars.csgoConfFilesLoc;
            cfgFilesLink = gVars.CSGOCfgFilelink;
            srvFinishInstFunc = () => steamSrvFunc.finishInstallationCSGO(steamCMDandServersFolder, selectedGameRoot);
            batFunc = () => steamSrvFunc.createBatFlCSGO(steamCMDandServersFolder + "\\" + selectedGameRoot);
            cfgFunc = () => steamSrvFunc.createCfgFlCSGO(steamCMDandServersFolder + "\\" + selectedGameRoot, cfgFilesLink[methClass.getLangNum()], true, true);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("rec", Properties.Resources.csgo_logo);
            serverPnl.Text = "Counter Strike: Global Offensive";
            setExtrPnlCtrls(true, true);
            extraBtn1.Text = lang.premadeCfg[gVars.lgNum];
            extr1 += () => cfgFunc();
            easyStgBtn.Click += (_, _2) => batFunc();
            setActPnlCtrls(false, true);
        }

        private void garrysModBtn_Click(object sender, EventArgs e)
        {
            resetActs();
            setGeneralCtrls();
            //Set Variables
            cleanTempActVars();
            selectedGameRoot = gVars.gmodRootFold;
            guideLink = gVars.gmodGuideLink;
            steamCmdInstCode = gVars.gmodInstCode;
            srvRunFiles = gVars.gmodStartFilesLoc;
            cfgFilesLoc = gVars.gmodConfFilesLoc;
            cfgFilesLink = gVars.gmodCfgFilelink;
            srvFinishInstFunc = () => steamSrvFunc.finishInstallationGmod(steamCMDandServersFolder, selectedGameRoot);
            batFunc = () => steamSrvFunc.createBatFlGMOD(steamCMDandServersFolder + "\\" + selectedGameRoot);
            cfgFunc = () => steamSrvFunc.createCfgFlGMOD(steamCMDandServersFolder + "\\" + selectedGameRoot, cfgFilesLink[methClass.getLangNum()], true, true);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("longRec", Properties.Resources.gmod_logo);
            serverPnl.Text = "Garry's Mod";
            setExtrPnlCtrls(true, true);
            extraBtn1.Text = lang.premadeCfg[gVars.lgNum];
            extr1 = () => cfgFunc();
            setActPnlCtrls();
        }
        private void hurtWorldBtn_Click(object sender, EventArgs e)
        {
            resetActs();
            setGeneralCtrls();
            //Set Variables
            cleanTempActVars();
            selectedGameRoot = gVars.hurtworldRootFold;
            guideLink = gVars.hurtworldGuideLink;
            steamCmdInstCode = gVars.hurtworldInstCode;
            srvRunFiles = gVars.hurtworldStartFilesLoc;
            cfgFilesLoc = gVars.hurtworldConfFilesLoc;
            srvFinishInstFunc = () => steamSrvFunc.finishInstallationHurtworld(steamCMDandServersFolder, selectedGameRoot);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("longRec", Properties.Resources.hurtworld_logo);
            serverPnl.Text = "Hurtworld";
            setExtrPnlCtrls(true);
            setActPnlCtrls();
        }

        private void KillingFloorTwoBtn_Click(object sender, EventArgs e)
        {
            resetActs();
            setGeneralCtrls();
            //Set Variables
            cleanTempActVars();
            selectedGameRoot = gVars.kf2RootFold;
            guideLink = gVars.kf2GuideLink;
            steamCmdInstCode = gVars.kf2InstCode;
            srvRunFiles = gVars.kf2StartFilesLoc;
            cfgFilesLoc = gVars.kf2ConfFilesLoc;
            srvFinishInstFunc = () => steamSrvFunc.finishInstallationKf2(steamCMDandServersFolder, selectedGameRoot);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("cube", Properties.Resources.kf2_logo);
            serverPnl.Text = "Killing Floor 2";
            setExtrPnlCtrls(true);
            setActPnlCtrls();
        }

        private void l4dBtn_Click(object sender, EventArgs e)
        {
            resetActs();
            setGeneralCtrls();
            //Set Variables
            cleanTempActVars();
            selectedGameRoot = gVars.l4dRootFold;
            guideLink = gVars.l4dGuideLink;
            steamCmdInstCode = gVars.l4dInstCode;
            srvRunFiles = gVars.l4dStartFilesLoc;
            cfgFilesLoc = gVars.l4dConfFilesLoc;
            cfgFilesLink = gVars.l4dCfgFilelink;
            srvFinishInstFunc = () => steamSrvFunc.finishInstallationL4d(steamCMDandServersFolder, selectedGameRoot);
            batFunc = () => steamSrvFunc.createBatFll4d(steamCMDandServersFolder + "\\" + selectedGameRoot);
            cfgFunc = () => steamSrvFunc.createCfgFll4d(steamCMDandServersFolder + "\\" + selectedGameRoot, cfgFilesLink[methClass.getLangNum()], true, true);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("rec", Properties.Resources.l4d_logo);
            serverPnl.Text = "Left 4 Dead";
            setExtrPnlCtrls(true, true);
            extraBtn1.Text = lang.premadeCfg[gVars.lgNum];
            extr1 = () => cfgFunc();
            setActPnlCtrls();
        }

        private void l4d2Btn_Click(object sender, EventArgs e)
        {
            resetActs();
            setGeneralCtrls();
            //Set Variables
            cleanTempActVars();
            selectedGameRoot = gVars.l4d2RootFold;
            guideLink = gVars.l4d2GuideLink;
            steamCmdInstCode = gVars.l4d2InstCode;
            srvRunFiles = gVars.l4d2StartFilesLoc;
            cfgFilesLoc = gVars.l4d2ConfFilesLoc;
            cfgFilesLink = gVars.l4d2CfgFilelink;
            srvFinishInstFunc = () => steamSrvFunc.finishInstallationL4d2(steamCMDandServersFolder, selectedGameRoot);
            batFunc = () => steamSrvFunc.createBatFll4d2(steamCMDandServersFolder + "\\" + selectedGameRoot);
            cfgFunc = () => steamSrvFunc.createCfgFll4d2(steamCMDandServersFolder + "\\" + selectedGameRoot, cfgFilesLink[methClass.getLangNum()], true, true);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("rec", Properties.Resources.l4d2_logo);
            serverPnl.Text = "Left 4 Dead 2";
            setExtrPnlCtrls(true, true);
            extraBtn1.Text = lang.premadeCfg[gVars.lgNum];
            extr1 = () => cfgFunc();
            setActPnlCtrls();
        }
        private void RustBtn_Click(object sender, EventArgs e)
        {
            resetActs();
            setGeneralCtrls();
            //Set Variables
            cleanTempActVars();
            selectedGameRoot = gVars.rustRootFold;
            guideLink = gVars.rustGuideLink;
            steamCmdInstCode = gVars.rustInstCode;
            srvRunFiles = gVars.rustStartFilesLoc;
            cfgFilesLoc = gVars.rustConfFilesLoc;
            cfgFilesLink = gVars.rustCfgFilelink;
            srvFinishInstFunc = () => steamSrvFunc.finishInstallationRust(steamCMDandServersFolder, selectedGameRoot);
            batFunc = () => steamSrvFunc.createBatFlRust(steamCMDandServersFolder + "\\" + selectedGameRoot);
            cfgFunc = () => steamSrvFunc.createCfgFlRust(steamCMDandServersFolder + "\\" + selectedGameRoot, cfgFilesLink[methClass.getLangNum()], true, true);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("longRec", Properties.Resources.rust_logo);
            serverPnl.Text = "Rust";
            setExtrPnlCtrls(true, true);
            extraBtn1.Text = lang.premadeCfg[gVars.lgNum];
            extr1 = () => cfgFunc();
            setActPnlCtrls();
        }

        private void SvenCoopBtn_Click(object sender, EventArgs e)
        {
            resetActs();
            setGeneralCtrls();
            //Set Variables
            cleanTempActVars();
            selectedGameRoot = gVars.svenRootFold;
            guideLink = gVars.svenGuideLink;
            steamCmdInstCode = gVars.svenInstCode;
            srvRunFiles = gVars.svenStartFilesLoc;
            cfgFilesLoc = gVars.svenConfFilesLoc;
            srvFinishInstFunc = () => steamSrvFunc.finishInstallationSvenCoop(steamCMDandServersFolder, selectedGameRoot);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("cube", Properties.Resources.svenCoop_logo);
            serverPnl.Text = "Sven Coop";
            setExtrPnlCtrls(true);
            setActPnlCtrls();
        }

        private void unturnedBtn_Click(object sender, EventArgs e)
        {
            resetActs();
            setGeneralCtrls();
            //Set Variables
            cleanTempActVars();
            selectedGameRoot = gVars.unturnedRootFold;
            steamCmdInstCode = gVars.unturnedInstCode;
            srvRunFiles = gVars.unturnedStartFilesLoc;
            cfgFilesLoc = gVars.unturnedConfFilesLoc;
            srvFinishInstFunc = () => steamSrvFunc.finishInstallationUnturned();
            batFunc = () => steamSrvFunc.createBatFlUnturned(steamCMDandServersFolder + "\\" + selectedGameRoot);
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("cube", Properties.Resources.unturned);
            serverPnl.Text = "Unturned";
            setExtrPnlCtrls(false, true);
            extraBtn1.Text = "Help Configure server";
            extr1 = () => Process.Start(gVars.unturnedExtrLink[0]);
            setActPnlCtrls();
        }

        private void atlasBtn_Click(object sender, EventArgs e)
        {
            resetActs();
            setGeneralCtrls();
            //Set Variables
            cleanTempActVars();
            selectedGameRoot = gVars.atlasRootFold;
            guideLink = gVars.atlasGuideLink;
            steamCmdInstCode = gVars.atlasInstCode;
            srvRunFiles = gVars.atlasStartFilesLoc;
            cfgFilesLoc = gVars.atlasConfFilesLoc;
            instSrv = () => srvNotAvailb();
            srvFinishInstFunc = () => steamSrvFunc.finishInstallationATLAS();
            //CUSTOMIZE PANEL
            singleGameChck((sender as Elegant.Ui.ToggleButton));
            setGamePicBox("cube", Properties.Resources.atlas_logo);
            serverPnl.Text = "ATLAS";
            setExtrPnlCtrls(true);
            setActPnlCtrls();
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

        private void appendLog()
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + gVars.logs;
            if (!System.IO.Directory.Exists(path))
            { System.IO.Directory.CreateDirectory(path); }
            File.AppendAllText(path + "\\" + DateTime.Today.ToString("dd-MM-yyyy") + "-log.txt", log.sb.ToString());
            log.sb.Clear();
        }
        private void logTimer_Tick(object sender, EventArgs e)
        {
            appendLog();
            //customizeStgEditBtns(easyStgEdit);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            slowTimer.Enabled = false;
            appendLog();
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            Process.Start(Path.GetDirectoryName(Application.ExecutablePath) + "\\" + gVars.logs);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/paypalme/lonewolfco");
        }

        private void donateBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/paypalme/lonewolfco");
        }

        private void delBtn_Click(object sender, EventArgs e)
        {

        }

        private void chck4UpdatesBtn_Click(object sender, EventArgs e)
        {
            if (methClass.check4Updates(gVars.updateCheckingInfo[0], int.Parse(gVars.updateCheckingInfo[2])) == true)
            {
                if (MessageBox.Show(lang.toolUpdtAvailb[gVars.lgNumQuad], lang.toolUpdtAvailb[gVars.lgNumQuad + 1], MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                { Process.Start(gVars.updateCheckingInfo[1]); }
            }
            else { Elegant.Ui.MessageBox.Show(lang.noUpdtAvail[gVars.lgNumQuad], lang.noUpdtAvail[gVars.lgNumQuad + 1], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Exclamation); }
        }

        private void basicInstrTxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }
    }
}

