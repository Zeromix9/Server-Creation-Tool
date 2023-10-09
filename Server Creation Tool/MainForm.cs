using ByteSizeLib;
using Server_Creation_Tool.myClasses;
using statServer_Creation_Tool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Transitions;

namespace Server_Creation_Tool
{
    public partial class MainForm : Form
    {
        //stuff for moving the mainForm around when click-dragging it
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr handle, int nCmdShow);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool IsIconic(IntPtr handle);
        [System.Runtime.InteropServices.DllImport("user32.dll")]

        public static extern bool ReleaseCapture();
        const int SW_RESTORE = 9;
        //-------
        public MainForm()
        {
            InitializeComponent();
        }
        Color serverBtnSelectedColor = Color.FromArgb(88, 111, 150);
        Color serverBtnNormalColor = Color.Transparent;
        funcClass funcs = new funcClass();
        global_Variables gVars = new global_Variables();
        serverFuncs srvFuncs = new serverFuncs();


        public string srvInstDir;
        private void MainForm_Load(object sender, EventArgs e)
        {

            srvInstDir = Properties.Settings.Default.LastSavedSrvInstDir;
            //save buttons from the server selection panel into a list variable so we don't have to loop through the panel each time we want to access the buttons
            saveSrvList();
            //check for updates and select last used language
            int i = funcs.getLangNum();
            if (i == 0)
            { engBtn.PerformClick(); }
            else if (i == 1)
            { gerBtn.PerformClick(); }
            quickTimer_Start();
            slowerTimer_Start();
            updtCheck(false);
            //background worker for saving log file every few seconds(we use it as a timer)
            logSaver.DoWork += new DoWorkEventHandler(logSaver_DoWork);
            logSaver.RunWorkerAsync();
            //List all server buttons alphabetically
            searchGameInSrvList(steamSrvListPnl, " ");
            try
            { favSrvs = funcs.loadFavList(); }
            catch { }
            funcs.StartThread(() =>
            { hasInternet(true); });
            Thread.Sleep(1000);
        }

        private void mainFormDrag(object sender, MouseEventArgs e)
        {
            //move the window around by clicking on panel1, since we have disabled the traditional windows-style border
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void minimizeFormBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private async void closeFormBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            notifyIcon1.Visible = false;
            await allTimers_stop();
            //  Application.Exit();
            Process.GetCurrentProcess().Kill();
        }

        private void searchSrvTxtBox_Click(object sender, EventArgs e)
        {
            Transition.run(searchSrvTxtBox, "BackColor", Color.FromArgb(23, 31, 51), new TransitionType_Linear(150));
        }
        private void searchSrvTxtBox_TextChanged(object sender, EventArgs e)
        {
            searchGameInSrvList(steamSrvListPnl, searchSrvTxtBox.Text);
        }
        private void searchSrvTxtBox_Leave(object sender, EventArgs e)
        {
            Transition.run(searchSrvTxtBox, "BackColor", Color.FromArgb(39, 54, 84), new TransitionType_Linear(200));
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void openCloseMorePnlBtn_Click(object sender, EventArgs e)
        {
            //open-close the "MORE" help section 
            if (openCloseMorePnlBtn.Tag.ToString() == "push")
            {
                openCloseMorePnlBtn.Tag = "pull";
                Transition.run(this, "Width", 737, new TransitionType_EaseInEaseOut(600));
                openCloseMorePnlBtn.ImageNormal = Properties.Resources.moreNormal180D;
                openCloseMorePnlBtn.ImageHover = Properties.Resources.moreHover180D;
                openCloseMorePnlBtn.imageDown = Properties.Resources.morePressed180D;
                openCloseMorePnlBtn.BackgroundImage = Properties.Resources.moreNormal180D;
            }
            else if (openCloseMorePnlBtn.Tag.ToString() == "pull")
            {
                openCloseMorePnlBtn.Tag = "push";
                Transition.run(this, "Width", 627, new TransitionType_EaseInEaseOut(600));
                openCloseMorePnlBtn.ImageNormal = Properties.Resources.moreNormal;
                openCloseMorePnlBtn.ImageHover = Properties.Resources.moreHover;
                openCloseMorePnlBtn.imageDown = Properties.Resources.morePressed;
                openCloseMorePnlBtn.BackgroundImage = Properties.Resources.moreNormal;
            }
        }

        private void selectSrvInstFoldBtn_Click(object sender, EventArgs e)
        {
            if (askUserInstallDir() == true)
            {
                quickTimer_Start();
                slowerTimer_Start();
                try { selectedSrvBtn.PerformClick(); } catch { }
            }
        }
        aboutFrm frm = new aboutFrm();
        private void ToolGrpSiteBtn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/groups/ServerTool");
        }

        private void langBtn_Click(object sender, EventArgs e)
        {
            //first argument is the menu strip to show and the second argument is on what control the menu strip will be shown
            funcs.showMenuStripAtBtn(langMenuStrip, langBtn);
        }

        private void AboutBtn_Click(object sender, EventArgs e)
        {
            frm.Show();
        }

        private void plannedFeaturesBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://trello.com/b/9dsN6TIT/server-creation-tool");
        }

        changelogFrm chngLogFrm = new changelogFrm();
        private void changelogBtn_Click(object sender, EventArgs e)
        {
            chngLogFrm.ShowDialog();
        }

        private void extrHelpBtn_Click(object sender, EventArgs e)
        {
            funcs.showMenuStripAtBtn(extrHelpMenuStrip, extrHelpBtn);
        }

        private void welcomeBtn_Click_1(object sender, EventArgs e)
        {
            srvCodename = "";
            goToWelcome();
        }
        private void paypalBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/paypalme/zeromix");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //refresh the server list panel every few seconds because when the user is scrolling it leaves behind some weird black marks 
            steamSrvListPnl.Invalidate();
        }
        #region various functions
        private void setLang(string lang, int lgNum1, int lgNumQuad1)
        {
            Properties.Settings.Default.language = lang;
            Properties.Settings.Default.Save();
            gVars.lgNum = lgNum1;
            gVars.lgNumQuad = lgNumQuad1;
            goToWelcome();
            setCtrlLang();
        }
        private void setCtrlLang()
        {
            //change the language for each control

            //infoHelpPnl
            selectSrvInstFoldBtn.Text = lg(lang.selectInstFoldB);
            chck4UpdtBtn.Text = lg(lang.updCheckB);
            langBtn.Text = lg(lang.langB);
            ToolGrpSiteBtn.Text = lg(lang.toolGrpB);
            AboutBtn.Text = lg(lang.aboutB);
            //------------

            //moreStuffPnl
            MORElbl.Text = lg(lang.MORELb);
            appLogsBtn.Text = lg(lang.appLogs);
            changelogBtn.Text = lg(lang.chngLogB);
            plannedFeaturesBtn.Text = lg(lang.plannedFeatB);
            discordSrvBtn.Text = lg(lang.discordSrvB);
            extrHelpBtn.Text = lg(lang.extrHelpB);
            steamCMDstartBtn.Text = lg(lang.Start).Trim();
            clearCacheBtn.Text = lg(lang.clearCache);
            portForwardBtn.Text = lg(lang.portFwRouterB);
            //------------

            //welcome screen
            welcomeToToolLbl.Text = lg(lang.welcomeLb);
            welcomeToToolLbl.Location = lgPos(lang.welcLbLoc);
            welcomeDescLbl.Text = lg(lang.welcomeDescLb);
            welcomeDescLbl.Location = lgPos(lang.welcDescLbLoc);
            basicInstrLbl.Text = lg(lang.basicInstrLb);
            basicInstrTxtBox.Text = lg(lang.basicInstruct);
            //------------

            //Other Controls
            searchSrvLbl.Text = lg(lang.searchGameLb);
            //------------

            //mainSrvPanel
            sizeLbl.Text = lg(lang.srvSizeLb);
            //lastRanLbl.Text = lg(lang.srvSizeLb);
            extrPnlLbl.Text = lg(lang.extraLb);
            instGuideBtn.Text = lg(lang.instGuideB);
            actPnlLbl.Text = lg(lang.actionLb);
            OpenFoldBtn.Text = lg(lang.openFoldB);
            delBtn.Text = lg(lang.deleteB);
            editStgBtn.Text = lg(lang.editStgB);
            toolTip1.SetToolTip(addSrvFav, lg(lang.addToFav));
            toolTip1.SetToolTip(showFavSrvsBtn, lg(lang.showFavList));
            toolTip1.SetToolTip(noInternetBtn, lg(lang.noInternetConn) + lg(lang.click2Refresh));
            //------------
        }
        public string lg(string[] stringArray, int pos = 0)//pos(position in array) only used for arrays with more than 1 word per language, like messageboxes
        {
            if (stringArray.Length == 2)
            { return stringArray[gVars.lgNum]; }
            else if (stringArray.Length == 4)
            { return stringArray[gVars.lgNumQuad + pos]; }
            return "Missing word";
        }
        public Point lgPos(int[] intArray)//change size or position of a control when a language is selected, because some languages have longer sentences and words
        {
            return new Point(intArray[gVars.lgNumQuad], intArray[gVars.lgNumQuad + 1]);
        }
        public void enableControls(bool state, string type)//enable-disable controls
        {
            //always disabled  controls
            foreach (Control btn in extrPnl.Controls)
            { if (btn is Button) btn.Enabled = state; }
            actPnl.Enabled = state;
            if (type == "noSrvDir")//disable some controls if the server install dir is not found
            {
                steamCMDBtn.Enabled = state;
                instGuideBtn.Enabled = true; //LEAVE THIS BUTTON ALWAYS ENABLED
            }
            else if (type == "srvOperation")//disable the main group of controls that are related to the server so the user won't mess things up while the server is being installed, or deleted etc
            {
                steamSrvListPnl.Enabled = state;
                selectSrvInstFoldBtn.Enabled = state;
                chck4UpdtBtn.Enabled = state;
                langBtn.Enabled = state;
                changelogBtn.Enabled = state;
                searchSrvTxtBox.Enabled = state;
                steamCMDBtn.Enabled = state;
            }
            else if (type == "srvNotInstalled")//controls to disable if a server is not installed
            {
                instGuideBtn.Enabled = true; //lEAVE THIS BUTTON ALWAYS ENABLED
            }
        }
        public void goToWelcome()
        {
            mainSrvPanel.Visible = false;
            foreach (Button b in srvListBtns)
            {
                if (b.BackColor == serverBtnSelectedColor)
                { b.BackColor = serverBtnNormalColor; }
            }
            welcomeBtn.BackColor = serverBtnSelectedColor;
            welcomeBtn.ColorNormal = serverBtnSelectedColor;
        }
        private void updtCheck(bool notAvailNotify = true)
        {
            Thread t2 = new Thread(delegate ()
            {
                if (funcs.check4Updates(gVars.updateCheckingInfo[0], int.Parse(gVars.updateCheckingInfo[2])) == true)
                {
                    if (MessageBox.Show(lg(lang.toolUpdtAvailb), lg(lang.toolUpdtAvailb, 1), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    { Process.Start(gVars.updateCheckingInfo[1]); }
                }
                else
                {
                    //show or don't show the message that there is no update available
                    if (notAvailNotify)
                    { MessageBox.Show(lg(lang.noUpdtAvail), lg(lang.noUpdtAvail, 1), MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                }
            });
            t2.Start();
            Thread.Sleep(400);
        }
        private bool hasInternet(bool showErrorMsg = false)
        {
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                funcs.InvokeIfRequired(noInternetBtn, () =>
                { noInternetBtn.Visible = false; });
                // reply.Status == IPStatus.Success;
                return true;
            }
            catch (Exception)
            {
                funcs.InvokeIfRequired(noInternetBtn, () =>
                { noInternetBtn.Visible = true; });
                if (showErrorMsg) MessageBox.Show(lg(lang.noInternetConn), lg(lang.installSrv), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private string srvPath()
        {
            return srvInstDir + "\\" + srvRootFold;
        }
        #endregion

        #region workers-timers
        BackgroundWorker logSaver = new BackgroundWorker();
        void logSaver_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                log.Save();  //write log to file
                Thread.Sleep(10000);  // Wait 10 seconds
            }
        }
        bool quickTimerWorkDone = false;
        bool slowTimerWorkDone = false;
        bool infiniteLoopEnabled = true;
        void quickTimer_Start()
        {
            Color prevSrvColor = SystemColors.ControlDark;
            string prevSrvCodename = "";
            infiniteLoopEnabled = true;
            funcs.StartThread(() => //do all actions in a thread so it doesn't affect the responsiveness of the UI
            {
                while (infiniteLoopEnabled)
                {
                    quickTimerWorkDone = false;
                    //---
                    //check if the installation folder is selected or exists
                    if (!System.IO.Directory.Exists(srvInstDir))
                    {
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            enableControls(false, "noSrvDir");
                            setInstBtn("install");
                            foreach (Button btn in srvListBtns)
                            {
                                if (btn.Tag.ToString() != "notSrv")
                                { btn.ForeColor = SystemColors.ControlDark; }
                            }
                        });
                        quickTimerWorkDone = true;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        allTimers_stop();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        return;
                    }
                    //get the tag from all buttons one by one and then check if each server is installed.(the tag contains the server codename)
                    foreach (Button btn in srvListBtns)
                    {
                        if (btn.Tag.ToString() == "notSrv") continue;
                        if (chckIfSrvInstalled(btn) == true)
                        { btn.ForeColor = SystemColors.ControlLightLight; }
                        else
                        { btn.ForeColor = SystemColors.ControlDark; }
                    }
                    //check if the user has selected a server
                    if (selectedSrvBtn != null)
                    {
                        if (selectedSrvBtn.ForeColor != prevSrvColor && srvCodename == prevSrvCodename)
                        { funcs.InvokeIfRequired(selectedSrvBtn, () => { selectedSrvBtn.PerformClick(); }); }
                        prevSrvColor = selectedSrvBtn.ForeColor;
                        prevSrvCodename = srvCodename;
                    }
                    //---
                    Thread.Sleep(2000);  // Wait 2 seconds
                    quickTimerWorkDone = true;
                }
            });
        }
        void slowerTimer_Start()
        {
            funcs.StartThread(() =>
            {
                while (infiniteLoopEnabled)
                {
                    slowTimerWorkDone = false;
                    //---

                    //check if a server has been selected
                    if (String.IsNullOrEmpty(srvCodename))
                    { slowTimerWorkDone = true; Thread.Sleep(50); continue; }
                    //show size of selected server
                    if (selectedSrvBtn.ForeColor == SystemColors.ControlLightLight)
                    { showSrvSize(); }
                    else
                    { funcs.InvokeIfRequired(srvSizeLbl, () => { srvSizeLbl.Text = ""; }); }
                    //---
                    Thread.Sleep(7000);  // Wait 7 seconds
                    slowTimerWorkDone = true;
                }
            });
        }
        private async Task allTimers_stop()//stops "quick" and "slower" timers
        {
            infiniteLoopEnabled = false;
            while (!quickTimerWorkDone & !slowTimerWorkDone) await Task.Delay(50); //wait till the timers finish executing their code
        }
        #endregion

        #region main functions
        public List<Button> srvListBtns = new List<Button>();
        public string srvType;
        public string srvDownloadLink;
        public string srvCodename;
        public string srvSteamID;
        public string srvRootFold;
        public string srvCfgLink;
        public string[] startFileLoc;
        public string[] cfgFileLoc;
        public string steamCMDCode;
        public string guideLink;
        public Button selectedSrvBtn;
        Action installAct;
        Action startAct;
        Action forceStopAct;

        public void saveSrvList()
        {
            //steam server panel
            List<Button> tmp = new List<Button>();
            foreach (Button btn in steamSrvListPnl.Controls) { tmp.Add(btn); }
            srvListBtns = tmp.OrderBy(btn => btn.Text).ToList();
            //non steam server panel

        }
        //server searching method for panel button lists
        List<string> favSrvs = new List<string>();
        private void addSrvFav_Click(object sender, EventArgs e)
        {
            if (addSrvFav.Tag.ToString() == "unchecked")
            {
                addSrvFav.Tag = "checked";
                addSrvFav.BackgroundImage = Properties.Resources.starChecked;
                favSrvs.Add(srvCodename);
                funcs.saveFavList(favSrvs);
            }
            else if (addSrvFav.Tag.ToString() == "checked")
            {
                addSrvFav.Tag = "unchecked";
                addSrvFav.BackgroundImage = Properties.Resources.starUnChecked;
                favSrvs.Remove(srvCodename);
                funcs.saveFavList(favSrvs);
            }
            //refresh list
            if (showFavSrvsBtn.Tag.ToString() == "checked")
            {
                if (favSrvs.Any())
                {
                    showFavSrvsBtn.Tag = "unchecked";
                    showFavSrvsBtn.PerformClick();
                }
                else
                { showFavSrvsBtn.PerformClick(); }
            }
        }
        private void showFavSrvsBtn_Click(object sender, EventArgs e)
        {
            List<Button> filteredBtns = new List<Button>();
            if (showFavSrvsBtn.Tag.ToString() == "unchecked")
            {
                foreach (Button btn in srvListBtns)
                {
                    foreach (string srvCode in favSrvs)
                    {
                        if (btn.Tag.ToString().Trim() == srvCode)
                        { filteredBtns.Add(btn); }
                        else { btn.Visible = false; }
                    }
                }
                filterSrvBtns(filteredBtns);
                showFavSrvsBtn.Tag = "checked";
                showFavSrvsBtn.BackgroundImage = Properties.Resources.starChecked_16;
            }
            else if (showFavSrvsBtn.Tag.ToString() == "checked")
            {
                arrangeAllSrvBtns();
                showFavSrvsBtn.Tag = "unchecked";
                showFavSrvsBtn.BackgroundImage = Properties.Resources.starUnChecked_gray_16;
            }
        }
        private void arrangeAllSrvBtns()
        {
            int btnStartPointY = 0;
            btnStartPointY = btnStartPointY + 39;
            foreach (Button btn in srvListBtns)
            {
                btn.Visible = true;
                if (btn.Tag.ToString() == "notSrv") { continue; }
                btn.Location = new Point(btn.Location.X, btnStartPointY);
                btnStartPointY = btnStartPointY + btn.Size.Height;
            }
        }
        private void filterSrvBtns(List<Button> filteredBtns)
        {
            int btnStartPointY = 0;
            foreach (Button btn in filteredBtns)
            {
                btn.Visible = true;
                btn.Location = new Point(btn.Location.X, btnStartPointY);
                btnStartPointY = btnStartPointY + btn.Size.Height;
            }
        }
        public void searchGameInSrvList(Panel SvrListPnl, string txtToSearch)
        {
            if (showFavSrvsBtn.Tag.ToString() == "checked") return;
            SvrListPnl.VerticalScroll.Value = 0;
            List<Button> filteredBtns = new List<Button>();
            if (String.IsNullOrEmpty(txtToSearch.Trim()))
            {
                arrangeAllSrvBtns();
                return;
            }
            foreach (Button btn in srvListBtns)
            {
                string tag = btn.Tag.ToString();
                //check if button contains what the user searched for
                if (btn.Text.Trim().ToLower().Contains(txtToSearch.Trim().ToLower()) && tag != "notSrv" || tag.Trim().ToLower().Contains(txtToSearch.Trim().ToLower()) && tag != "notSrv")
                {
                    filteredBtns.Add(btn);
                    continue;
                }
                btn.Visible = false;
            }
            filterSrvBtns(filteredBtns);
        }
        private void setInstBtn(string state)
        {
            customSmoothBtn b = installSrvBtn;
            funcs.RemoveClickEvent(b);
            if (state == "install")
            {
                b.Text = lg(lang.install);
                b.ColorHover = Color.FromArgb(74, 164, 255);
                b.ColorNormal = Color.DodgerBlue;
                b.ColorPressed = Color.FromArgb(74, 164, 255);
                b.BackColor = Color.DodgerBlue;
                b.Image = Properties.Resources.install_img;
                b.Click += (_, _2) => installAct();
            }
            else if (state == "start")
            {
                b.Text = lg(lang.START);
                b.ColorHover = Color.FromArgb(89, 204, 42);
                b.ColorNormal = Color.FromArgb(42, 199, 73);
                b.ColorPressed = Color.FromArgb(89, 204, 42);
                b.BackColor = Color.FromArgb(42, 199, 73);
                b.Image = Properties.Resources.start_img;
                b.Click += (_, _2) => startAct();
            }
            else if (state == "f_stop")
            {
                b.Text = lg(lang.forceStop);
                b.ColorHover = Color.FromArgb(247, 50, 40);
                b.ColorNormal = Color.FromArgb(222, 45, 35);
                b.ColorPressed = Color.FromArgb(247, 50, 40);
                b.BackColor = Color.FromArgb(222, 45, 35);
                b.Image = Properties.Resources.stop_img;
                b.Click += (_, _2) => forceStopAct();
            }
        }
        private void customizeBtn(Button btn, string txt, Action action, bool clearImg = true)//image is optional. Preferably a 16x16 image or a little bigger
        {
            if (txt != null) btn.Text = txt;
            if (clearImg == true)
            { btn.Image = null; }
            btn.ImageAlign = ContentAlignment.MiddleLeft;
            funcs.RemoveClickEvent(btn);
            btn.Click += (_, _2) => action();
            btn.Visible = true;
        }
        private void setEditSettingBtn()
        {
            editSettingsMenuStrip.Items.Clear();//clear all previous items
            //read file locations from the cfgFileLoc array and put the filenames one by one in the menu strip
            foreach (string str in cfgFileLoc)
            {
                ToolStripItem item = editSettingsMenuStrip.Items.Add(funcs.extractFileName(str));
                item.Click += (_, _2) => openSettingFile(str);
            }
        }
        private void openSettingFile(string filePath)
        {
            string fileExtension = filePath.Substring(filePath.LastIndexOf(".") + 1);
            Process proc = new Process();
            string finalFilePath;
            if (filePath.Substring(0, 1) != @"\")
            { finalFilePath = filePath; }
            else { finalFilePath = srvPath() + filePath; }
            if (fileExtension == "exe")
            {
                proc.StartInfo.FileName = finalFilePath;
            }
            else
            {
                proc.StartInfo.Arguments = finalFilePath;
                proc.StartInfo.FileName = "notepad.exe";
            }
            try
            {
                proc.Start();
                proc.Close();
            }
            catch (Exception a) { log.Append(a.ToString()); MessageBox.Show(lg(lang.generalError), lg(lang.generalError, 1), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void writeBatchFl(bool ask = false)
        {
            if (ask)
            {
                if (MessageBox.Show(lg(lang.createBatFile), lg(lang.createBatFile, 1), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                { return; }
            }
            if (!funcs.writeTxtFile(srvPath() + startFileLoc[0], funcs.getVarByStr(srvCodename + "StartBatFileCode")))
            { MessageBox.Show(lg(lang.batFail), lg(lang.error), MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else
            {
                if (ask)
                { MessageBox.Show(lg(lang.done)); }
            }
        }
        private async void writeCfgFl(int cfgFilePos, bool ask = true)
        {
            if (ask)
            {
                if (MessageBox.Show(lg(lang.askCreateCfgFile), lg(lang.createBatFile, 1), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                { return; }
            }
            await funcs.downloadnWriteFile(srvCfgLink, srvPath() + cfgFileLoc[cfgFilePos]);
            while (funcs.downloadCompleted == false) await Task.Delay(50);
            if (ask) MessageBox.Show(lg(lang.done), lg(lang.customConfigFile), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void setPremadeCfgBtn(Button btn, int cfgFilePos)//cfg file position in array
        {
            customizeBtn(btn, lg(lang.customConfigFile), new Action(() => writeCfgFl(cfgFilePos, true)));
        }
        private void setCustomBatFileBtn(Button btn, Action act = null)
        {
            Action act2;
            if (act == null) act2 = () => writeBatchFl(true);
            else act2 = act;
            customizeBtn(btn, lg(lang.customBatFile), act2, false);
        }

        private bool askUserInstallDir()
        {
            // Create a new instance of FolderBrowserDialog.
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            //set description language
            folderBrowserDialog1.Description = lg(lang.askSrvInstDirDesc);
            // A new folder button will display in FolderBrowserDialog.
            folderBrowserDialog1.ShowNewFolderButton = true;
            //inform user and then show FolderBrowserDialog
            MessageBox.Show(lg(lang.installSrvUnderRoot), lg(lang.installSrvUnderRoot, 1), MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult dlgResult = folderBrowserDialog1.ShowDialog();
            if (dlgResult == DialogResult.OK)
            {
                srvInstDir = folderBrowserDialog1.SelectedPath;
                Properties.Settings.Default.LastSavedSrvInstDir = srvInstDir;
                Properties.Settings.Default.Save();
                return true;
            }
            else
            { return false; }
        }
        private void resetAfterSrvOpr(bool enableTimers = true)
        {
            if (enableTimers == true)
            {
                quickTimer_Start();
                slowerTimer_Start();
            }
            BeginInvoke((MethodInvoker)delegate ()
            {
                enableControls(true, "srvOperation");
                showTaskInProg(false);
                selectedSrvBtn.PerformClick();
                installSrvBtn.Enabled = true;
            });
        }

        private void showSrvSize()
        {
            funcs.InvokeIfRequired(srvSizeLbl, () => { srvSizeLbl.Text = lg(lang.calculating); });
            funcs.StartThread(() =>
            {
                var v = ByteSize.FromBytes(funcs.ShowFolderSize(srvPath()));
                //we use the server size converted to MB as a reference size
                double unitValue;
                string unit;
                if (v.Bytes >= 1024 * 1024 * 1024)
                {
                    unitValue = System.Convert.ToDouble(v.Bytes) / (1024 * 1024 * 1024);
                    unit = "GB";
                }
                else if (v.Bytes >= 1024 * 1024)
                {
                    unitValue = System.Convert.ToDouble(v.Bytes) / (1024 * 1024);
                    unit = "MB";
                }
                else if (v.Bytes >= 1024)
                {
                    unitValue = System.Convert.ToDouble(v.Bytes) / 1024;
                    unit = "KB";
                }
                else
                {
                    unitValue = v.Bytes;
                    unit = "0";
                }
                string strUnitValue = unitValue.ToString();
                string result = string.Format("{0} {1}", strUnitValue.Remove(strUnitValue.IndexOf(',') + 2, strUnitValue.Length - strUnitValue.IndexOf(',') - 2), unit);
                funcs.InvokeIfRequired(srvSizeLbl, () => { srvSizeLbl.Text = result; });

            });
        }
        private bool chckIfSrvInstalled(Button b)
        {
            string rootFold = funcs.getVarByStr(b.Tag.ToString() + "RootFold");
            if (System.IO.Directory.Exists(srvInstDir + @"\" + rootFold))
            {
                if (System.IO.File.Exists(srvInstDir + @"\" + rootFold + @"\inst_fl.inf"))//inst_fl.inf shows if the installation has successfully finished or not
                { return false; }
                else
                { return true; }
            }
            else
            { return false; }
        }

        Process steamCMDProc;
        string operation; //will be used for real time interfacing with steamCMD. UNUSED CURRENTLY
        void startSteamCMD()
        {

            setInstBtn("f_stop");
            steamCMDProc = new Process();
            // steamCMDProc.EnableRaisingEvents = true;
            //steamCMDProc.OutputDataReceived += new DataReceivedEventHandler(steamCMD_OutputDataReceived);
            // steamCMDProc.ErrorDataReceived += new DataReceivedEventHandler(steamCMD_ErrorDataReceived);
            // steamCMDProc.Exited += new System.EventHandler(steamCMDProcExited);

            steamCMDProc.StartInfo.FileName = srvInstDir + @"\steamcmd.exe";
            steamCMDProc.StartInfo.Arguments = steamCMDCode;
            //  steamCMDProc.StartInfo.UseShellExecute = false;
            // steamCMDProc.StartInfo.RedirectStandardError = true;
            //  steamCMDProc.StartInfo.RedirectStandardOutput = true;
            //  steamCMDProc.StartInfo.RedirectStandardInput = true;
            //  steamCMDProc.StartInfo.ErrorDialog = false;
            // steamCMDProc.StartInfo.CreateNoWindow = true;
            //  steamCMDProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            try
            { steamCMDProc.Start(); }
            catch (Exception a)
            {
                quickTimer_Start();
                slowerTimer_Start();
                showTaskInProg(false);
                log.Append(a.ToString());
                MessageBox.Show(lg(lang.generalError), lg(lang.generalError, 1), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // inputWriter = steamCMDProc.StandardInput;
            funcs.StartThread(steamCMDProcExited);
            //  steamCMDProc.BeginErrorReadLine();
            //  steamCMDProc.BeginOutputReadLine();

        }

        void steamCMD_ErrorDataReceived(object sender, DataReceivedEventArgs e)//CURRENTLY DISABLED
        {
            funcs.InvokeIfRequired(textBox2, () =>
          { textBox2.AppendText(e.Data + Environment.NewLine); });
        }
        void steamCMD_OutputDataReceived(object sendingProcess, DataReceivedEventArgs e)//CURRENTLY DISABLED
        {
            //check if recieved output is emtpy. If its emtpy don't do anything
            if (!String.IsNullOrEmpty(e.Data))
            {
                funcs.InvokeIfRequired(textBox2, () =>
                { textBox2.AppendText(e.Data + Environment.NewLine); });
                if (operation == "install")
                {

                }
                if (e.Data.ToLower().Contains("downloading, progress:"))//checks if server is downloading and show the current task
                {
                    funcs.InvokeIfRequired(currentTaskLbl, () =>
                    { currentTaskLbl.Text = lg(lang.downloading) + srvNamePnlLbl.Text; });//show progress in percentage
                    string progress = e.Data.Substring(e.Data.IndexOf(":") + 1);
                    string progPercentDouble = progress.Substring(0, progress.IndexOf("("));
                    string progPercent = progPercentDouble.Substring(0, progPercentDouble.IndexOf(".")).Trim();
                    string progSize = progress.Substring(progress.IndexOf("(")).Trim('(', ')');
                    funcs.InvokeIfRequired(taskProgBar, () =>
                    { taskProgBar.Value = int.Parse(progPercent); });//advance progress bar
                    funcs.InvokeIfRequired(extrTaskInfLbl, () =>
                    { extrTaskInfLbl.Text = progPercent + "%"; });//show progress in percentage

                }
                else if (e.Data.ToLower().Contains("success!"))//checks if server is done installing
                {
                    //    MessageBox.Show("done");
                    steamCMDProc.StandardInput.Flush();
                    //  steamCMDProc.StandardInput.WriteLine("quit");
                    // steamCMDProc.Close();
                }
            }
        }
        void steamCMDProcExited()
        {
            while (true)
            {
                if (steamCMDProc.HasExited)
                {
                    //reset and enable all controls
                    resetAfterSrvOpr();
                    steamCMDProc.Close();
                    return;
                }
            }
        }
        private void setSrvPicBox(string size)
        {
            int width = 0;
            int height = 0;
            int locX = 0;
            int locY = 0;
            if (size == "rec")
            {
                width = 299;
                height = 68;
                locX = 24;
                locY = 13;
            }
            else if (size == "longRec")
            {
                width = 334;
                height = 68;
                locX = 7;
                locY = 13;
            }
            else if (size == "cube")
            {
                width = 82;
                height = 80;
                locX = 127;
                locY = 1;
            }
            //change picture box logo
            var property = typeof(Properties.Resources).GetProperty(srvCodename + "_logo", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            Bitmap img = property.GetValue(null, null) as Bitmap;
            srvImgPicBox.BackgroundImage = new Bitmap(img);
            //set dimensions
            srvImgPicBox.Location = new Point(locX, locY);
            srvImgPicBox.Size = new Size(width, height);
        }
        private void serverBtnClicked(Button btn, string picboxSize, string srvFullName)
        {
            //read selected server info and set the variables
            selectedSrvBtn = btn;
            installSrvBtn.Enabled = true;
            srvCodename = btn.Tag.ToString();
            srvType = funcs.getVarByStr(srvCodename + "SrvType");
            srvRootFold = funcs.getVarByStr(srvCodename + "RootFold");
            startFileLoc = funcs.getArVarByStr(srvCodename + "StartFilesLoc");
            cfgFileLoc = funcs.getArVarByStr(srvCodename + "ConfFilesLoc");
            try { srvCfgLink = lg(funcs.getArVarByStr(srvCodename + "CfgFilelink")); }
            catch { }
            //When the user clicks a server, highlight the clicked button and un-highlight the others
            srvListBtnSet(btn);
            //check if server is installed
            if (btn.ForeColor == SystemColors.ControlLightLight)
            {
                setSrvPnlCtrls("srvInstalled");
                //show server size
                showSrvSize();
            }
            else if (btn.ForeColor == SystemColors.ControlDark)
            {
                setSrvPnlCtrls("srvNotInstalled");
                srvSizeLbl.Text = "   ";
            }
            if (srvType == "steam")//steam 
            {
                srvSteamID = funcs.getVarByStr(srvCodename + "SteamID");
                steamCMDCode = " +force_install_dir ./" + srvRootFold + "/ +login anonymous +app_update " + srvSteamID.Trim() + " validate";
                guideLink = lg(funcs.getArVarByStr(srvCodename + "GuideLink"));
                customizeBtn(actBtn1, lg(lang.repairUpdt), repairUpdtSrv);
            }
            else if (srvType == "non_steam")//non steam 
            {
                srvDownloadLink = funcs.getVarByStr(srvCodename + "DownloadLink");
                customizeBtn(actBtn1, lg(lang.update), repairUpdtSrv);
            }
            actBtn1.Image = Properties.Resources.progressBar;
            welcomeBtn.BackColor = serverBtnNormalColor;
            welcomeBtn.ColorNormal = serverBtnNormalColor;
            mainSrvPanel.Visible = true;
            foreach (Button b in steamSrvListPnl.Controls)
            {
                if (b.BackColor == serverBtnSelectedColor)
                { b.BackColor = serverBtnNormalColor; }
            }
            btn.BackColor = serverBtnSelectedColor;
            srvNamePnlLbl.Text = srvFullName;
            instGuideBtn.Text = lg(lang.instGuideB);
            setSrvPicBox(picboxSize);
            toolTip1.SetToolTip(srvInfBtn, null);
            //check if it is favourite or not
            if (favSrvs.Contains(srvCodename))
            {
                addSrvFav.BackgroundImage = Properties.Resources.starChecked;
                addSrvFav.Tag = "checked";
            }
            else
            {
                addSrvFav.BackgroundImage = Properties.Resources.starUnChecked;
                addSrvFav.Tag = "unchecked";
            }
        }
        private void setInstBtnAct(Action instAction = null, Action startAction = null, Action forceStopAction = null)
        {
            //use common actions or custom ones
            //if an argument is left null, then it means that the actions are not changed and the common ones are used
            if (instAction == null)
            { installAct = installServer; }
            else { installAct = instAction; }
            if (startAction == null)
            { startAct = startSrv; }
            else { startAct = startAction; }
            if (forceStopAction == null)
            { forceStopAct = forceStopSteamSrvInst; }
            else { forceStopAct = forceStopAction; }
        }
        private void showTaskInProg(bool visible, bool extrInfLblVisible = false, bool unknownProgress = false)
        {
            currentTaskLbl.Text = lg(lang.plsWait) + "...";
            taskLbl.Visible = visible;
            currentTaskLbl.Visible = visible;
            taskProgBar.Visible = visible;
            extrTaskInfLbl.Visible = extrInfLblVisible;
            extrTaskInfLbl.Text = "";
            if (unknownProgress)//if the progress of the current task is not known, just show a marquee which indicates that the task is progressing
            {
                taskProgBar.Style = ProgressBarStyle.Marquee;
                taskProgBar.Value = 30;
            }
            else
            {
                taskProgBar.Style = ProgressBarStyle.Continuous;
                taskProgBar.Value = 6;
            }
        }

        private void srvListBtnSet(Button btn)
        {
            if (chckIfSrvInstalled(btn) == true)
            { btn.ForeColor = SystemColors.ControlLightLight; }
            else
            { btn.ForeColor = SystemColors.ControlDark; }

        }
        private void setSrvPnlCtrls(string action)
        {
            foreach (Control b in extrPnl.Controls)
            { if (b is customSmoothBtn) { b.Visible = false; } }
            actBtn1.Visible = false;
            actBtn2.Visible = false;
            actBtn3.Visible = false;
            instGuideBtn.Visible = true;
            if (action == "resetPnl")
            { return; }
            else if (action == "srvInstalled")
            {
                setInstBtn("start");
                enableControls(true, "srvNotInstalled");
            }
            else if (action == "srvNotInstalled")
            {
                setInstBtn("install");
                enableControls(false, "srvNotInstalled");
            }
        }
        async void prepSteamCMDStart(string taskTitle)
        {
            enableControls(false, "srvOperation");
            showTaskInProg(true, false, true);
            currentTaskLbl.Text = lg(lang.checkingInternetConn);
            if (!hasInternet(true))
            {
                enableControls(true, "srvOperation");
                installSrvBtn.Enabled = true;
                showTaskInProg(false);
                return;
            }
            this.Enabled = false;
            await allTimers_stop();
            this.Enabled = true;
            srvSizeLbl.Text = "";
            //check if steamCMD exists
            if (!System.IO.File.Exists(srvInstDir + "\\steamcmd.exe"))
            {
                currentTaskLbl.Text = lg(lang.downloading) + "SteamCMD";
                installSrvBtn.Enabled = false;
                await funcs.downloadSteamCMD(srvInstDir);
                installSrvBtn.Enabled = true;
                //check if steamCMD was successfully downloaded
                if (!funcs.steamCMDDownloadDone)
                {
                    MessageBox.Show(lg(lang.downloadFailedHelp), lg(lang.downloadFailedHelp, 1), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    BeginInvoke((MethodInvoker)delegate () { resetAfterSrvOpr(); });
                    return;
                }
                //start steamCMD
                currentTaskLbl.Text = taskTitle;
                startSteamCMD();
            }
            else// if steamCMD exists continue without downloading it
            {
                //start steamCMD
                currentTaskLbl.Text = taskTitle;
                startSteamCMD();
            }
        }
        private void installServer()
        {
            if (!System.IO.Directory.Exists(srvInstDir))
            {
                if (!askUserInstallDir())
                {
                    BeginInvoke((MethodInvoker)delegate () { resetAfterSrvOpr(); });
                    return;
                }
                else
                { resetAfterSrvOpr(); }
            }
            if (System.IO.Directory.Exists(srvInstDir + @"\" + srvRootFold))
            {
                if (MessageBox.Show(lg(lang.foldContainsSrv), lg(lang.install).Trim(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                { return; }
            }
            if (srvType == "steam")
            {
                MessageBox.Show(lg(lang.typeQuitNotify), lg(lang.notice), MessageBoxButtons.OK, MessageBoxIcon.Information);
                prepSteamCMDStart(lg(lang.downloading) + srvNamePnlLbl.Text);
            }
            else if (srvType == "non_steam")
            {
                installNON_STEAM_Server(lg(lang.downloading) + srvNamePnlLbl.Text, lg(lang.installDone));
            }
        }
        private async void installNON_STEAM_Server(string taskTitle, string doneMsg)
        {
            enableControls(false, "srvOperation");
            installSrvBtn.Enabled = false;
            showTaskInProg(true, false, true);
            currentTaskLbl.Text = lg(lang.checkingInternetConn);
            if (!hasInternet(true))
            {
                enableControls(true, "srvOperation");
                installSrvBtn.Enabled = true;
                showTaskInProg(false);
                return;
            }
            this.Enabled = false;
            await allTimers_stop();
            this.Enabled = true;
            srvSizeLbl.Text = "";
            if (!System.IO.Directory.Exists(srvInstDir + @"\" + srvRootFold))
            { System.IO.Directory.CreateDirectory(srvInstDir + @"\" + srvRootFold); }
            currentTaskLbl.Text = taskTitle;
            await funcs.downloadnWriteFile(srvDownloadLink, srvInstDir + @"\" + srvRootFold + @"\" + Path.GetFileName(new Uri(srvDownloadLink).AbsolutePath));
            while (funcs.downloadCompleted == false) await Task.Delay(50);
            if (!funcs.unzipFileAndDel(srvInstDir + @"\" + srvRootFold + @"\" + Path.GetFileName(new Uri(srvDownloadLink).AbsolutePath), srvInstDir + @"\" + srvRootFold))
            {
                MessageBox.Show(lg(lang.generalError), lg(lang.generalError, 1), MessageBoxButtons.OK, MessageBoxIcon.Error);
                showNotif(lg(lang.generalError), srvNamePnlLbl.Text);
            }
            else
            { showNotif(doneMsg, srvNamePnlLbl.Text); }
            resetAfterSrvOpr();
        }
        private void startSrv()
        {
            Process srv = new Process();
            int startFileLocCount = startFileLoc.Count();
            srv.StartInfo.WorkingDirectory = srvPath() + Path.GetDirectoryName(startFileLoc[0]);
            if (System.IO.File.Exists(srvPath() + startFileLoc[0]))//check if the first file for starting the server exists as a file (not just in the array). The first file is very likely to not exist because many server don't need a batch file to start and creating it is optional.
            { srv.StartInfo.FileName = srvPath() + startFileLoc[0]; }
            else if (startFileLocCount != 1)//if the first file doesn't exist and there is a second file in the array, use that. The second file might not exist because of a failed installation. If it doesn't exist, it will throw an exception when starting the server
            { srv.StartInfo.FileName = srvPath() + startFileLoc[1]; }
            try { srv.Start(); }//start server
            catch (Exception a) { log.Append(a.ToString()); MessageBox.Show(lg(lang.cantStartSrv), lg(lang.cantStartSrv, 1)); }
        }
        private void createBatFileSrvStart(Action customFunc = null)
        {
            if (!System.IO.File.Exists(srvPath() + startFileLoc[0]))//check if batch file already exists
            {
                if (customFunc == null) writeBatchFl();
                else customFunc();
            }
            startSrv();
        }
        private async void delSrv()
        {
            installSrvBtn.Enabled = false;
            enableControls(false, "srvOperation");
            showTaskInProg(true, false, true);
            this.Enabled = false;
            await allTimers_stop();
            this.Enabled = true;
            srvSizeLbl.Text = "";
            currentTaskLbl.Text = lang.deleting[gVars.lgNum] + srvNamePnlLbl.Text;
            funcs.StartThread(() => //do all actions in a thread so it doesn't affect the responsiveness of the UI
            {
                try
                {
                    System.IO.Directory.Delete(srvPath(), true);//delete server folder
                    showNotif(lg(lang.srvDelSuccess), srvNamePnlLbl.Text);
                    MessageBox.Show(lg(lang.srvDelSuccess), lg(lang.srvDelSuccess, 1), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { log.Append(ex.ToString()); MessageBox.Show(lg(lang.srvDelFail), lg(lang.srvDelFail, 1), MessageBoxButtons.OK, MessageBoxIcon.Error); showNotif(lg(lang.srvDelFail), srvNamePnlLbl.Text, ToolTipIcon.Error); }
                resetAfterSrvOpr(false);
            });
        }
        private void showNotif(string text, string gameName, ToolTipIcon icon = ToolTipIcon.Info, int showLength = 7000)
        {
            notifyIcon1.BalloonTipIcon = icon;
            if (gameName == "")
            { notifyIcon1.BalloonTipTitle = "Server Creation Tool"; }
            else
            { notifyIcon1.BalloonTipTitle = gameName; }
            notifyIcon1.BalloonTipText = text;
            notifyIcon1.ShowBalloonTip(showLength);
        }
        private void forceStopSteamSrvInst()
        {
            if (MessageBox.Show(lg(lang.forceStopInstSteamMsg), lg(lang.forceStopInstSteamMsg, 1), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            { steamCMDProc.Kill(); }
        }
        private bool checkIfProcRunning(Process proc)
        {
            try
            {
                if (proc.HasExited)
                { return false; }
                else { return true; }
            }
            catch { return false; }
        }

        #endregion

        #region Action panel buttons
        private void OpenFoldBtn_Click(object sender, EventArgs e)
        {
            Process.Start(srvPath());
        }
        private void delBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(lg(lang.delSrv), lg(lang.delSrv, 1), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            { delSrv(); }
        }
        private void editStgBtn_Click(object sender, EventArgs e)
        {
            setEditSettingBtn();
            funcs.showMenuStripAtBtn(editSettingsMenuStrip, editStgBtn);
        }
        private void repairUpdtSrv()
        {
            if (MessageBox.Show(lg(lang.repairUpdateSrvMsg), lg(lang.repairUpdt).Trim(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (srvType == "steam")//steam 
                {
                    prepSteamCMDStart(lg(lang.repairUpdt).Trim() + " " + srvNamePnlLbl.Text);
                    //steamCMDCode = "+app blablabla"; //Set custom code
                }
                else if (srvType == "non_steam")//steam 
                {
                    installNON_STEAM_Server(lg(lang.updating) + srvNamePnlLbl.Text, lg(lang.RepairUpdtDone));
                }
            }
        }

        #endregion

        private void instGuideBtn_Click(object sender, EventArgs e)
        {
            Process.Start(guideLink);
        }

        #region server buttons
        private void days7Btn_Click_1(object sender, EventArgs e)
        {
            setInstBtnAct();
            serverBtnClicked(sender as Button, "rec", "7 days to die"); //"rec" is the size of the picturebox. There is also "longRec" and "cube".
            instGuideBtn.Visible = true;//If we need the "Installation Guide" button we must set 'visible' to 'true' because each time a user selects a server the  "Installation Guide" hides.
            customizeBtn(actBtn1, lg(lang.repairUpdt), new Action(() =>
            {
                if (MessageBox.Show(lg(lang.days7repairUpdateSrvMsg), lg(lang.repairUpdt).Trim(), MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                { repairUpdtSrv(); }
            }), false);
        }
        private void arkBtn_Click(object sender, EventArgs e)
        {
            setInstBtnAct(null, () => createBatFileSrvStart());
            serverBtnClicked(sender as Button, "cube", "ARK: Survival Evolved");
            instGuideBtn.Visible = true;
        }

        private void codbo3Btn_Click(object sender, EventArgs e)
        {
            setInstBtnAct();
            serverBtnClicked(sender as Button, "longRec", "Call of Duty: Black Ops 3");
            instGuideBtn.Visible = true;
        }
        private void cs16Btn_Click(object sender, EventArgs e)
        {
            setInstBtnAct();
            serverBtnClicked(sender as Button, "cube", "Counter Strike 1.6");
            instGuideBtn.Visible = true;
            setPremadeCfgBtn(extrBtn1, 1);
            setCustomBatFileBtn(extrBtn2, () => srvFuncs.createBatFilecs16(srvPath() + startFileLoc[0]));
        }
        private void csgoBtn_Click(object sender, EventArgs e)
        {
            setInstBtnAct(null, () => createBatFileSrvStart(() => srvFuncs.createBatFileCSGO(srvPath() + startFileLoc[0])));
            serverBtnClicked(sender as Button, "rec", "Counter Strike: Global Offensive");
            instGuideBtn.Visible = true;
            setPremadeCfgBtn(extrBtn1, 1);
            setCustomBatFileBtn(extrBtn2, () => srvFuncs.createBatFileCSGO(srvPath() + startFileLoc[0]));
        }
        private void gmodBtn_Click(object sender, EventArgs e)
        {
            setInstBtnAct(null, () => createBatFileSrvStart());
            serverBtnClicked(sender as Button, "longRec", "Garry's Mod");
            instGuideBtn.Visible = true;
            setPremadeCfgBtn(extrBtn1, 1);
        }
        private void hurtworldBtn_Click(object sender, EventArgs e)
        {
            setInstBtnAct();
            serverBtnClicked(sender as Button, "longRec", "Hurtworld");
            instGuideBtn.Visible = true;
        }
        private void cssBtn_Click(object sender, EventArgs e)
        {
            setInstBtnAct(null, () => createBatFileSrvStart());
            serverBtnClicked(sender as Button, "cube", "Counter Strike: Source");
            instGuideBtn.Visible = true;
            setPremadeCfgBtn(extrBtn1, 1);
        }
        private void kf2Btn_Click(object sender, EventArgs e)
        {
            setInstBtnAct();
            serverBtnClicked(sender as Button, "cube", "Killing Floor 2");
            instGuideBtn.Visible = true;
        }
        private void l4dBtn_Click(object sender, EventArgs e)
        {
            setInstBtnAct(null, () => createBatFileSrvStart());
            serverBtnClicked(sender as Button, "rec", "Left 4 Dead");
            instGuideBtn.Visible = true;
            setPremadeCfgBtn(extrBtn1, 1);
        }
        private void l4d2Btn_Click(object sender, EventArgs e)
        {
            setInstBtnAct(null, () => createBatFileSrvStart());
            serverBtnClicked(sender as Button, "rec", "Left 4 Dead 2");
            instGuideBtn.Visible = true;
            setPremadeCfgBtn(extrBtn1, 1);
        }
        private void rustBtn_Click(object sender, EventArgs e)
        {
            setInstBtnAct(null, () => createBatFileSrvStart());
            serverBtnClicked(sender as Button, "rec", "Rust");
            instGuideBtn.Visible = true;
            setPremadeCfgBtn(extrBtn1, 1);
        }
        private void svenBtn_Click(object sender, EventArgs e)
        {
            setInstBtnAct(null, () => createBatFileSrvStart(() => srvFuncs.createBatFileSven(srvPath() + startFileLoc[0])));
            serverBtnClicked(sender as Button, "cube", "Sven Coop");
            instGuideBtn.Visible = true;
            setCustomBatFileBtn(extrBtn1, () => srvFuncs.createBatFileSven(srvPath() + startFileLoc[0]));
        }
        private void unturnedBtn_Click(object sender, EventArgs e)
        {
            setInstBtnAct(null, () => createBatFileSrvStart());
            serverBtnClicked(sender as Button, "cube", "Unturned");
            instGuideBtn.Text = lg(lang.configHelp);
            instGuideBtn.Visible = true;
            guideLink = gVars.unturnedSrvConfigHelpLink[0];
        }
        Process advancedManage = new Process();
        private void beamngBtn_Click(object sender, EventArgs e)
        {
            setInstBtnAct();
            serverBtnClicked(sender as Button, "longRec", "BeamNG.Drive");
            instGuideBtn.Visible = true;
            instGuideBtn.Text = lg(lang.configHelp);
            guideLink = gVars.beamngSrvConfigHelpLink[0];
            toolTip1.SetToolTip(srvInfBtn, "The BeamMP server is not an official server. It has been created by the community and not the BeamNG.Drive creators.");
            customizeBtn(extrBtn1, lg(lang.srvAdvancedManage), new Action(() =>
            {
                if (checkIfProcRunning(advancedManage))
                {
                    IntPtr handle = advancedManage.MainWindowHandle;
                    if (IsIconic(handle)) { ShowWindow(handle, SW_RESTORE); }
                    SetForegroundWindow(handle);
                }
                else
                {
                    if (System.IO.File.Exists(srvInstDir + @"\" + srvRootFold + @"\BeamMP Server Settings Tool.exe"))
                    {
                        advancedManage = new Process();
                        advancedManage.StartInfo.FileName = srvInstDir + @"\" + srvRootFold + @"\BeamMP Server Settings Tool.exe";
                        advancedManage.StartInfo.WorkingDirectory = srvInstDir + @"\" + srvRootFold;
                        advancedManage.Start();
                    }
                    else
                    { Process.Start("https://www.mediafire.com/folder/42o3auj9o9lgx/BeamMP+Server+Management+tool"); }
                }
            }), false);
        }
        private void craftopiaBtn_Click(object sender, EventArgs e)
        {
            setInstBtnAct();
            serverBtnClicked(sender as Button, "rec", "Craftopia");
            instGuideBtn.Visible = true;
            setCustomBatFileBtn(extrBtn1, () => writeBatchFl(true));
        }
        private void nmrihBtn_Click(object sender, EventArgs e)
        {
            setInstBtnAct(null, () => createBatFileSrvStart());
            serverBtnClicked(sender as Button, "rec", "No More Room in Hell");
            instGuideBtn.Visible = true;

        }
        #endregion
        private void portForwardBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://portforward.com/router.htm");
        }

        private void appLogsBtn_Click(object sender, EventArgs e)
        {
            try { Process.Start(Application.StartupPath + "\\" + gVars.logsDir); }
            catch { MessageBox.Show(lg(lang.error)); }
        }

        private void chck4UpdtBtn_Click(object sender, EventArgs e)
        {
            updtCheck();
        }

        private void gerBtn_Click(object sender, EventArgs e)
        {
            setLang("german", 1, 2);
        }
        private void engBtn_Click(object sender, EventArgs e)
        {
            setLang("english", 0, 0);
        }

        private void faqBtn_Click(object sender, EventArgs e)
        {
            //show FAQ
            MessageBox.Show(lg(lang.faq), "FAQ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //test code for making fail file
            string filePath = srvPath() + @"\inst_fl.inf";
            if (!System.IO.File.Exists(filePath))
            { File.Create(filePath); }
        }

        private void steamCMDBtn_Click(object sender, EventArgs e)
        {
            funcs.showMenuStripAtBtn(steamCMDmenuStrip, steamCMDBtn);
        }

        private void steamCMDstartBtn_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(srvInstDir + @"\steamcmd.exe"))
            {
                //start steamCMD
                Process p = new Process();
                p.StartInfo.WorkingDirectory = srvInstDir;
                p.StartInfo.FileName = srvInstDir + @"\steamcmd.exe";
                p.Start();
                //free all resources but keep steamCMD running
                p.Close();
            }
        }

        private void clearCacheBtn_Click(object sender, EventArgs e)
        {
            if (System.IO.Directory.Exists(srvInstDir + @"\appcache"))
            {
                showTaskInProg(true, false, true);
                enableControls(false, "srvOperation");
                currentTaskLbl.Text = "Cleaning cache";
                funcs.StartThread(() =>
                {
                    try
                    {
                        System.IO.Directory.Delete(srvInstDir + @"\appcache", true);
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            enableControls(true, "srvOperation");
                            showTaskInProg(false);
                        });
                    }
                    catch (Exception ex)
                    {
                        log.Append(ex.ToString());
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            enableControls(true, "srvOperation");
                            showTaskInProg(false);
                        });
                        MessageBox.Show(lg(lang.cacheCleanFail), lg(lang.cacheCleanFail, 1), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                });
            }
        }

        private void discordSrvBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/79eKCs5fW8");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        Process aproc;
        private void button1_Click_1(object sender, EventArgs e)
        {
            aproc = new Process();
            aproc.StartInfo.FileName = srvInstDir + @"\steamcmd.exe";
            aproc.StartInfo.Arguments = steamCMDCode;
            aproc.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            aproc.Kill();
        }

        private void noInternetBtn_Click(object sender, EventArgs e)
        {
            noInternetBtn.Enabled = false;
            showTaskInProg(true, false, true);
            enableControls(false, "srvOperation");
            currentTaskLbl.Text = lg(lang.checkingInternetConn);
            installSrvBtn.Enabled = false;
            funcs.StartThread(() =>
            {
                if (hasInternet(true))
                {
                    funcs.InvokeIfRequired(noInternetBtn, () =>
                    { noInternetBtn.Visible = false; });
                }
                BeginInvoke((MethodInvoker)delegate ()
                {
                    showTaskInProg(false);
                    enableControls(true, "srvOperation");
                    installSrvBtn.Enabled = true;
                    noInternetBtn.Enabled = true;
                });
            });
        }
    }
}
