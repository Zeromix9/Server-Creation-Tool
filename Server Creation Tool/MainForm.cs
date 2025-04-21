using Microsoft.WindowsAPICodePack.Dialogs;
using Server_creation_tool.classes;
using Server_creation_tool.reusable_controls.messageBox;
using Server_Creation_Tool;
using Server_Creation_Tool.myClasses;
using SteamCMD.ConPTY;
using SteamCMD.ConPTY.Interop.Definitions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Security.Policy;
using System.Threading;
using System.Windows.Forms;
using Transitions;
//using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace Server_creation_tool
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
        private void mainFormDrag(object sender, MouseEventArgs e)
        {
            //move the window around by dragging on panel1, since we have disabled the traditional windows-style border
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        public MainForm()
        {
            InitializeComponent();
        }

        //various info
        public string server_data_files_path = "Server_creation_tool.Server_data_files";
        public string server_lang_files_path = "Server_creation_tool.Language_files";
        public string toolFolder = System.Windows.Forms.Application.StartupPath;
        public string serversInstDir = Properties.Settings.Default.serversDir;
        //string steamCMDPath = Properties.Settings.Default.serversDir + "\\steamcmd.conpty.exe"; 
        public string steamCMDPath = Properties.Settings.Default.serversDir + "\\steamcmd.exe";
        ResourceManager srvNamesResMan = new ResourceManager(typeof(Server_data_files._srvNames));

        //initialize classes, forms etc.
        funcsClass funcs = new funcsClass();
        public messageBox MsgBox;

        private void Form1_Load(object sender, EventArgs e)
        {
            MsgBox = new messageBox(this);
            aTimer();
            //  if (Debugger.IsAttached)
            //Properties.Settings.Default.Reset();

            //set properties
            this.Width = 627;
            //language stuff
            ApplyLang(Properties.Settings.Default.lang);

            if (Properties.Settings.Default.favServers == null) Properties.Settings.Default.favServers = new List<string>();
            //Add the button for each server in the server selection list
            addSrvBtns();
            notify4Updates();
        }


        private void MainForm_Shown(object sender, EventArgs e)
        {
            //exclude the buttons that we have added to the action and extras panels in designer view from being deleted.
            foreach (customSmoothBtn btn in extraPnl.Controls.OfType<customSmoothBtn>())
            { extraBtnsNODelete.Add(btn); }
            foreach (customSmoothBtn btn in actPnl.Controls.OfType<customSmoothBtn>())
            { ActBtnsNODelete.Add(btn); }

            //CHECK IF PERFORMCLICK WORKS WHEN NEW UPDATE MESSAGEBOX SHOWS
        }
        //----------------------------------
        #region Various controls
        private void langBtn_Click(object sender, EventArgs e)
        {
            contexMenuStripFrm contexMenu = new contexMenuStripFrm();
            contexMenu.addBtn("English", () => { changeLang("en"); }, true, Properties.Resources.icons8_great_britain_24);
            contexMenu.addBtn("German", () => { changeLang("de"); }, false, Properties.Resources.icons8_germany_24);
            //    contexMenu.addBtn("Greek", () => { changeLang("gr"); }, false, Properties.Resources.icons8_greece_24);
            funcs.showMenuStripAtBtn(contexMenu, langBtn, this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            //MessageBox.Show(s);
        }
        private void customBtn1_Click(object sender, EventArgs e)
        {
            Process.Start("https://paypal.me/Zeromix");
        }

        private void discordSrvBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/79eKCs5fW8");
        }

        aboutFrm aboutfrm = null;
        private void AboutBtn_Click(object sender, EventArgs e)
        {
            try
            {
                aboutfrm.Show();
                aboutfrm.Focus();
            }
            catch
            {
                aboutfrm = new aboutFrm(this);
                aboutfrm.Show();
            }
        }

        private void openCloseMorePnlBtn_Click(object sender, EventArgs e)
        {
            //open-close the "MORE" section 
            if (openCloseMorePnlBtn.Toggled)
            {
                Transition.run(this, "Width", 737, new TransitionType_EaseInEaseOut(600));
                openCloseMorePnlBtn.ImageNormal = Properties.Resources.moreNormal180D;
                openCloseMorePnlBtn.ImageHover = Properties.Resources.moreHover180D;
                openCloseMorePnlBtn.imageDown = Properties.Resources.morePressed180D;
                openCloseMorePnlBtn.BackgroundImage = Properties.Resources.moreNormal180D;
            }
            else
            {
                Transition.run(this, "Width", 627, new TransitionType_EaseInEaseOut(600));
                openCloseMorePnlBtn.ImageNormal = Properties.Resources.moreNormal;
                openCloseMorePnlBtn.ImageHover = Properties.Resources.moreHover;
                openCloseMorePnlBtn.imageDown = Properties.Resources.morePressed;
                openCloseMorePnlBtn.BackgroundImage = Properties.Resources.moreNormal;
            }
        }

        bool toolExiting = false;
        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancelActions = true;
            toolExiting = true;
            log.Save();
            Properties.Settings.Default.Save();
            System.Windows.Forms.Application.Exit();
        }
        private void customBtn3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void ToolGrpSiteBtn_Click(object sender, EventArgs e)
        {
            Process.Start("http://steamcommunity.com/groups/ServerTool");
        }

        private void plannedFeaturesBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://trello.com/b/9dsN6TIT/server-creation-tool");
        }


        private void changelogBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://portforward.com/router.htm");
        }

        FAQFrm faqForm;
        private void faqBtn_Click(object sender, EventArgs e)
        {
            if (faqForm == null || faqForm.IsDisposed) faqForm = new FAQFrm(this);
            faqForm.Show();
            faqForm.Focus();
        }
        private void selectSrvInstFoldBtn_Click(object sender, EventArgs e)
        {
            selectSrvInstDir();
            showInstalledSrvs();
            welcomeBtn.PerformClick();
        }
        private void steamCMDBtn_Click(object sender, EventArgs e)
        {
            contexMenuStripFrm contexMenu = new contexMenuStripFrm();
            contexMenu.addBtn(getGeneralLang("start")[1], () =>
            {
                cTry(() => { Process.Start(steamCMDPath); }, true, getGeneralLang("steamcmd_launch_fail")[1], getGeneralLang("error")[1], true);
            }, true, Properties.Resources.SquaregreenArrowRight);
            contexMenu.addBtn(getGeneralLang("clear_cache")[1], () => { funcsClass.DeleteDirectory(serversInstDir + @"\appcache"); MsgBox.quickMsg(getGeneralLang("done")[1], getGeneralLang("done")[1]); }, false, Properties.Resources.x1616);
            funcs.showMenuStripAtBtn(contexMenu, steamCMDBtn, this);
        }

        private void extrHelpBtn_Click(object sender, EventArgs e)
        {
            // contexMenuStripFrm contexMenu = new contexMenuStripFrm();

            //  contexMenu.addBtn("How to port forward", () => Process.Start("https://portforward.com/router.htm"), true, Properties.Resources.port);
            // contexMenuStripFrm contexSubMenu = new contexMenuStripFrm();
            //  contexSubMenu.parent = contexMenu;
            //   contexSubMenu.Its_submenu = true;
            //    contexSubMenu.addBtn("Amogus", () => MessageBox.Show("yes"));
            //    contexSubMenu.addBtn("UYSSUYUUUU", () => MessageBox.Show("AGMAG"),false);
            //    contexMenu.addSubMenuBtn("This is a stupid submenu", contexSubMenu, false);
            //   funcs.showMenuStripAtBtn(contexMenu, extrHelpBtn, this);
            Process.Start("https://github.com/Zeromix9/Server-Creation-Tool");
        }

        private void welcomeBtn_Click(object sender, EventArgs e)
        {
            welcomeBtn.BackColor = Color.FromArgb(54, 76, 114);
            welcomeBtn.ColorNormal = Color.FromArgb(54, 76, 114);
            clearSrvPnlSelection();
            mainSrvPanel.Hide();
            clickedSrvBtn = null;

        }
        private void searchSrvTxtBox_Click(object sender, EventArgs e)
        {
            Transition.run(searchSrvTxtBox, "BackColor", Color.FromArgb(23, 31, 51), new TransitionType_Linear(130));
        }

        private void searchSrvTxtBox_Leave(object sender, EventArgs e)
        {
            Transition.run(searchSrvTxtBox, "BackColor", Color.FromArgb(39, 54, 84), new TransitionType_Linear(200));
        }

        private void showFavSrvsBtn_Click(object sender, EventArgs e)
        {
            if (showFavSrvsBtn.Toggled)
            {
                showFavSrvsBtn.Image = Properties.Resources.starChecked_16;
                showFavs();
            }
            else
            {
                showFavSrvsBtn.Image = Properties.Resources.starUnChecked_gray_16;
                restoreSrvListPnlBtns();
            }
        }
        private void customSmoothBtn1_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void appLogsBtn_Click(object sender, EventArgs e)
        {
            Process.Start(toolFolder + "\\" + "sc_tool_logs");
        }

        #endregion
        //----------------------------------ENDREGION

        //----------------------------------
        #region language system
        DataTable controlsLang = new DataTable();
        DataTable generalLang = new DataTable();
        private void ApplyLang(string lang)
        {
            Properties.Settings.Default.lang = lang;
            Properties.Settings.Default.Save();
            if (!loadLangFiles())
            {
                Application.Exit();
            }
            setControlsLang(controlsLang, this);

        }

        bool loadLangFiles()
        {
            return cTry(() =>
              {
                  //MUST SET RESOURCES AS EMBEDDED FOR THIS TO WORK
                  StringReader ctrlReader = new StringReader(funcs.readEmbeddedRes("Server_creation_tool.Language_files." + Properties.Settings.Default.lang + ".controls.lang") as string);
                  StringReader generalReader = new StringReader(funcs.readEmbeddedRes("Server_creation_tool.Language_files." + Properties.Settings.Default.lang + ".general.lang") as string);
                  controlsLang.ReadXml(ctrlReader);
                  generalLang.ReadXml(generalReader);
                  controlsLang.PrimaryKey = new[] { controlsLang.Columns[0] };
                  generalLang.PrimaryKey = new[] { generalLang.Columns[0] };
                  //Debug.WriteLine(controlsLang.Rows[2][1].ToString());
              }, true, "Critical error. Language file error. Please contact us on Discord, steam or Github about the details of this issue. The Server Creation Tool will close now", "Language file error", false, "ERROR READING LANGUAGE FILES");
        }

        public void setControlsLang(DataTable table, Form frm)
        {
            foreach (DataRow row in table.Rows)
            {
                // Console.WriteLine(row[0].ToString());
                Control ctrl;
                if (row[0].ToString().Trim() == "FORM_TITLE")
                {
                    frm.Text = row[1].ToString();
                }
                else
                {
                    try
                    {
                        ctrl = frm.Controls.Find(row[0].ToString(), true)[0];
                    }
                    catch { Debug.WriteLine("Control from lang file not found: " + row[0].ToString()); continue; }
                    if (ctrl.Name == "welcomeToToolLbl")
                    {
                        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                        System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                        //  Debug.WriteLine(fvi.FileVersion);
                        ctrl.Text = row[1].ToString() + " " + fvi.FileVersion.ToString();
                    }
                    else if (row[1].ToString().Contains("!&"))
                    {
                        //to be continued...
                    }
                    else
                    {
                        ctrl.Text = row[1].ToString();
                    }
                    string[] xy = row[2].ToString().Split(',');
                    if (xy.Length == 2)
                    {
                        int xoffset;
                        int yoffset;
                        if (int.TryParse(xy[0], out xoffset) && int.TryParse(xy[1], out yoffset))
                        {
                            ctrl.Location = new Point(ctrl.Location.X + xoffset, ctrl.Location.Y + yoffset);
                        }
                    }
                }

            }
        }
        private void changeLang(string lang)
        {
            Properties.Settings.Default.lang = lang;
            Properties.Settings.Default.Save();
            string[] langChanged = getStrFromDtronRes(dtronResReader(server_lang_files_path + "." + lang + ".general.lang"), "languageHasBeenChanged");
            if (MsgBox.Show(langChanged[1], langChanged[2], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                Application.Restart();
            }
        }
        public string[] getControlsLang(string entryName)
        {
            return getStrFromDtronRes(controlsLang, entryName);
            //  return controlsLang.Rows.Find(entryName).ItemArray.Cast<string>().ToArray();
        }
        public string[] getGeneralLang(string entryName)
        {
            return getStrFromDtronRes(generalLang, entryName);
        }

        //THESE SHOULD BE MOVED ELSEWHERE WHEN I FINISH THE CUSTOM RESOURCE FILE SYSTEM
        public DataTable dtronResReader(string resPath)
        {
            DataTable dataTabl = new DataTable();
            StringReader Reader = new StringReader(funcs.readEmbeddedRes(resPath) as string);
            dataTabl.ReadXml(Reader);
            dataTabl.PrimaryKey = new[] { dataTabl.Columns[0] };
            return dataTabl;
        }
        public object[] getObjFromDtronRes(DataTable dtronRes, string strToSearch, int getSingleObjIndex = -1)//single object is at index 0
        {
            object[] objArr;
            try
            {
                objArr = dtronRes.Rows.Find(strToSearch).ItemArray.Cast<object>().ToArray();

            }
            catch
            {
                objArr = null;
                log.Append("Error in getObjFromDtronRes function: Failed to get resource entry");
            }
            if (objArr != null && objArr.Length > 0)
            {
                if (getSingleObjIndex >= 0)
                {
                    objArr = new object[] { objArr[getSingleObjIndex] };
                }
                for (int i = 0; i < objArr.Length; i++)
                {
                    object obj2 = objArr[i];
                    if (obj2 is string)
                    {
                        string str2 = obj2 as string;
                        str2 = str2.Trim();
                        if (str2.Substring(0, 2) == "~[" && str2[str2.Length - 1] == ']')//get the value from other entry
                        {
                            try
                            {
                                string[] parts = str2.Split(']');
                                parts[0] = parts[0].Substring(2);
                                parts[1] = parts[1].Substring(1);
                                int valueIndex = int.Parse(parts[1]);
                                object obj3 = getObjFromDtronRes(dtronRes, parts[0], valueIndex)[0];
                                objArr[i] = obj3;

                            }
                            catch
                            { objArr[i] = null; log.Append("Failed to get value from other entry in getObjFromDtronRes function. Entry name: " + objArr[0] as string); }
                        }
                    }

                }
            }

            return objArr;
        }
        public string[] getStrFromDtronRes(DataTable dtronRes, string strToSearch, int getSingleObjIndex = -1)
        {
            try
            {
                object[] objArr = getObjFromDtronRes(dtronRes, strToSearch, getSingleObjIndex);
                return Array.ConvertAll(objArr, x => x.ToString());
            }
            catch (Exception ex)
            {
                log.Append("getStrFromDtronRes ERROR. strToSearch = " + strToSearch);
                return funcs.Populate(new string[10], "!LANG_ERROR!") as string[];
            }

        }
        public string snprintf(string str, string[] str2add)//replace every [~] found in str with each string from the str2add array
        {
            for (int i = 0; i < str.Length; i++)
            {
                int pos = str.IndexOf("[~]");
                if (pos <= 0)//no other [~] found
                {
                    return str;
                }
                str = str.Substring(0, pos) + str2add[i] + str.Substring(pos + "[~]".Length);
            }
            return str;
        }
        #endregion
        //------------------------------------ENDREGION

        //----------------------------------
        #region srvList_favs_and_msg_boxes_and_other_misc
        private void searchSrv()
        {
            int scrollPnlheight = 0;
            foreach (Button btn in scrollPnlSrvList.EditablePanel.Controls)
            {
                if (btn.Tag.ToString() != "notSrv")
                {
                    if (btn.Text.Trim().ToLower().Contains(searchSrvTxtBox.Text.Trim().ToLower()) || btn.Tag.ToString().Trim().ToLower() == searchSrvTxtBox.Text.Trim().ToLower())
                    { btn.Visible = true; scrollPnlheight += btn.Height; }
                    else btn.Visible = false;
                }
                else btn.Visible = false;
            }
            scrollPnlSrvList.scrollToTop();
            scrollPnlSrvList.resizeScrollPnl(scrollPnlheight);
        }
        private void searchSrvTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (searchSrvTxtBox.Text.Trim() == "")
            { restoreSrvListPnlBtns(); }
            else
            { searchSrv(); }
        }
        private void restoreSrvListPnlBtns()
        {
            foreach (Button btn in scrollPnlSrvList.EditablePanel.Controls)
            { btn.Visible = true; }
            scrollPnlSrvList.scrollToTop();
            scrollPnlSrvList.resizeScrollPnl(scrollPnlHeightALLSERVERS);
        }

        private void showFavs()
        {
            int scrollPnlHeight = 0;
            if (Properties.Settings.Default.favServers.Any())
            {
                scrollPnlSrvList.scrollToTop();
                foreach (Button btn in scrollPnlSrvList.EditablePanel.Controls)
                {
                    if (Properties.Settings.Default.favServers.Contains(btn.Tag.ToString()))
                    {
                        btn.Visible = true;
                        scrollPnlHeight += btn.Height;
                    }
                    else
                    {
                        btn.Visible = false;
                    }
                }
                scrollPnlSrvList.resizeScrollPnl(scrollPnlHeight);
            }
            else
            {
                MsgBox.Show("You don't have any favourite servers!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                showFavSrvsBtn.PerformClick();
            }
        }
        private void addFav()
        {

            Properties.Settings.Default.favServers.Add(codename);
            Properties.Settings.Default.Save();
            if (showFavSrvsBtn.Toggled) showFavs();
        }
        private void RemFav()
        {
            Properties.Settings.Default.favServers.Remove(codename);
            Properties.Settings.Default.Save();
            if (showFavSrvsBtn.Toggled) showFavs();
        }
        private void clearSrvPnlSelection()
        {
            foreach (Control b in scrollPnlSrvList.EditablePanel.Controls)
            {
                if (b is Button)//check if control is button. If it isn't it means its the welcome button which is a customSmoothBtn
                {
                    if (b.BackColor == Properties.Settings.Default.serverBtnSelectedColor)
                    { b.BackColor = Color.Transparent; }
                }
            }
        }
        public bool cTry(Action action, bool showMsg = true, string errorBody = "", string errorTitle = "Error", bool combinedMsg = false, string logFileErrorDesc = "")//Leaving errorBody empty shows a generic error message. //DO NOT FORGET TO ADD DESCRIPTIONS
        {
            try
            {
                action();
                return true;
            }
            catch (Exception ex)
            {
                log.Append("ERROR: " + logFileErrorDesc + ex.ToString());
                if (showMsg)
                {
                    BeginInvoke((MethodInvoker)delegate ()//run the messageboxes in the UI thread. so that it freezes when they show.
                    {
                        if (errorBody == "")
                        { MsgBox.Show(getGeneralLang("cTry_generic_msg")[1], getGeneralLang("error_occured")[1], MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        else if (errorBody != "" && combinedMsg == true)
                        { MsgBox.Show(errorBody + Environment.NewLine + Environment.NewLine + getGeneralLang("cTry_generic_msg")[1], errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        else if (errorBody != "" && combinedMsg == false)
                        { MsgBox.Show(errorBody, errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    });
                }
                return false;
            }
        }
        public void taskStarted(string msg = "Running Task...", bool showExtraTaskInf = false, bool knownProgress = false)
        {
            BeginInvoke((MethodInvoker)delegate ()
            {
                if (!knownProgress)//if the progress of the current task is not known, just show a marquee which indicates that the task is progressing
                {
                    taskProgBar.Style = ProgressBarStyle.Marquee;
                    taskProgBar.Value = 30;
                }
                else
                {
                    taskProgBar.Style = ProgressBarStyle.Continuous;
                    taskProgBar.Value = 6;
                }
                taskLbl.Visible = true;
                currentTaskLbl.Text = msg;
                currentTaskLbl.Visible = true;
                taskProgBar.Visible = true;
                extrTaskInfLbl.Visible = showExtraTaskInf;
                extrTaskInfLbl.Text = "";
            });

        }
        public void taskEnded()
        {
            BeginInvoke((MethodInvoker)delegate ()
            {
                taskLbl.Visible = false;
                currentTaskLbl.Visible = false;
                taskProgBar.Visible = false;
                extrTaskInfLbl.Visible = false;
            });
        }
        public void methodInvoke(Action act)
        {
            BeginInvoke((MethodInvoker)delegate ()
            { act(); });
        }

        #endregion
        //------------------------------------ENDREGION

        //------------------------------------
        #region various_funcs

        private bool selectSrvInstDir()
        {
            bool dontShow = Properties.Settings.Default.disableServerInstDirNotif;
            MsgBox.Dontshow(ref dontShow, getGeneralLang("select_server_dir_note")[1], getGeneralLang("select_server_dir_note")[2], MessageBoxButtons.OK, MessageBoxIcon.Information);
            Properties.Settings.Default.disableServerInstDirNotif = dontShow;
            Properties.Settings.Default.Save();
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            if (serversInstDir != "" && Directory.Exists(serversInstDir))
            {
                dialog.InitialDirectory = serversInstDir;
            }
            else dialog.InitialDirectory = toolFolder;
            dialog.IsFolderPicker = true;
            dialog.Title = getGeneralLang("select_server_dir_note")[2];
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                MsgBox.Show(getGeneralLang("you_selected")[1] + ": " + dialog.FileName, getGeneralLang("select_server_dir_note")[2], MessageBoxButtons.OK, MessageBoxIcon.Information);
                steamCMDPath = dialog.FileName + "\\steamcmd.exe";
                serversInstDir = dialog.FileName;
                Properties.Settings.Default.serversDir = serversInstDir;
                Properties.Settings.Default.Save();
                return true;
            }
            else return false;
        }
        public bool checkifCurrentSrvInstDirExists()
        {
            if (Directory.Exists(serversInstDir))
            { return true; }
            else return false;
        }
        public bool setupSteamCMD(bool forceRedownload = false, bool showTask = true)
        {
            if (!checkifCurrentSrvInstDirExists())
            {
                bool selected = false;
                bool done = false;
                methodInvoke(() =>
                {

                    selected = selectSrvInstDir();
                    done = true;
                });
                while (!done) Thread.Sleep(200);
                if (!selected) return false;
                //MessageBox.Show("lol");
            }
            if (File.Exists(steamCMDPath) && forceRedownload == false)
            { return true; }
            else
            {
                if (showTask) taskStarted(getGeneralLang("downloading_steamcmd")[1], true, true);
                //DOWNLOAD ZIP
                fileDownloader downloader = new fileDownloader(taskProgBar, extrTaskInfLbl);
                string steamCMDziploc = serversInstDir + "\\steamcmd.zip";
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                downloader.download("https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip", steamCMDziploc);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Thread.Sleep(1000);
                while (!downloader.downloadCompleted && !downloader.failed)
                { Thread.Sleep(200); }
                if (downloader.failed || downloader.downloadProgPercent[0] < 99)
                {
                    if (showTask) { taskEnded(); }
                    MsgBox.Show(getGeneralLang("steamcmd_download_failed")[1], getGeneralLang("steamcmd_download_failed")[2], MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                //EXTRACT ZIP
                bool extractSuccess = cTry(() =>
                {
                    funcs.extractZip(steamCMDziploc, serversInstDir);
                }, true, getGeneralLang("steamcmd_zip_extract_failed")[1], getGeneralLang("steamcmd_zip_extract_failed")[2], true);
                if (!extractSuccess) { if (showTask) { taskEnded(); } return false; }
                //DELETE ZIP
                cTry(() => File.Delete(steamCMDziploc));
                taskEnded();
                return true;
            }
        }

        public void disableMainPnl(bool disable = true, bool leaveInstGuide = true) //disable when game is not installed
        {
            methodInvoke(() =>
            {
                actPnl.Enabled = !disable;
                foreach (Control ctrl in extraPnl.Controls)
                {
                    if (!(ctrl is Label) && ctrl.Name != "instGuideBtn") ctrl.Enabled = !disable;
                }
            });

        }
        public void enableMainPnl()
        {
            disableMainPnl(false);
        }
        public void disableUI(bool disable = true, bool leaveMainPanel = false)//disable almost the entire UI when executing somethin
        {
            bool done = false;
            methodInvoke(() =>
            {
                if (!leaveMainPanel) disableMainPnl(disable);
                scrollPnlSrvList.Enabled = !disable;
                searchSrvTxtBox.Enabled = !disable;
                showFavSrvsBtn.Enabled = !disable;
                selectSrvInstFoldBtn.Enabled = !disable;
                settingsBtn.Enabled = !disable;
                langBtn.Enabled = !disable;
                steamCMDBtn.Enabled = !disable;
                chck4UpdtBtn.Enabled = !disable;
                done = true;
            });

            if (scrollPnlSrvList.InvokeRequired) { while (!done) Thread.Sleep(50); }
            if (false == false)//if it runs on a seperate thread, it means that methodInvoke content will run on its own (the UI thread) and the disableUI function will finish without the contents of the methodInvoke actually having finished executing, so we wait till they are done
            {
                //delete this if the above works
            }
        }
        public void enableUI(bool leaveMainPanel = false)
        {
            disableUI(false, leaveMainPanel);
        }

        private void showInstalledSrvs()//returns the button of that matches the given codename
        {
            foreach (Control btn in scrollPnlSrvList.EditablePanel.Controls)
            {
                if (btn is Button && btn.Tag.ToString() != "notSrv")
                {
                    //if (codenm != "" && btn.Tag.ToString() == codenm) buton = btn as Button;
                    if (checkIfSrvInstalled(btn.Tag.ToString()))
                    {
                        btn.ForeColor = SystemColors.ControlLightLight;
                    }
                    else
                    {
                        btn.ForeColor = Color.FromArgb(138, 138, 138);
                    }
                }
            }
        }
        public bool checkIfSrvInstalled(string codenm = "")//checks if atleast one instance of the server exists
        {
            //ENABLE AFTER SERVER INSTANCE SYSTEM IS FINISHED
            //if (codenm == "") codenm = codename;
            //try
            //{
            //    string defaultRoot1 = getSrvRootFold(codenm);
            //    string[] dirs = Directory.GetDirectories(serversInstDir, "*", SearchOption.TopDirectoryOnly);
            //    foreach (string dir in dirs)
            //    {
            //        string dirName = new DirectoryInfo(dir).Name;
            //        if (dirName.Contains(defaultRoot1)) //CHANGE TO THIS WHEN INSTANCE SYSTEM IS READY -->>  if (dirName.Contains(defaultRoot1))
            //        { return true; }
            //    }
            //}
            //catch { }
            //return false;

            //REMOVE THIS AFTER SERVER INSTANCE SYSTEM IS DONE
            if (codenm == "") codenm = codename;
            try
            {
                if (System.IO.Directory.Exists(serversInstDir + "\\" + getSpecificSrvRootFold(codenm)))
                {
                    return true;
                }
            }
            catch { }
            return false;
        }
        private int thereIsUpdate()//if there is update, it returns the number, if there is not or it failed to check, it returns 0
        {
            string githubAPIUrl = "https://api.github.com/repos/Zeromix9/Server-Creation-Tool/tags";
            string apiOutput;
            int currentVersion = getCurrentVer();
            try
            {
                //get latest version from github
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(githubAPIUrl);
                req.AllowAutoRedirect = false;
                req.AuthenticationLevel = System.Net.Security.AuthenticationLevel.None;
                req.Timeout = 10000;
                req.UseDefaultCredentials = true;
                req.MaximumAutomaticRedirections = 5;
                req.Accept = "application/json";
                //req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36";
                req.CookieContainer = new CookieContainer();
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                Stream dataStream = res.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                apiOutput = reader.ReadToEnd();
                int namePropertyIndx = apiOutput.IndexOf("name");//add 6 to have the index start at the end of the property "name"
                apiOutput = apiOutput.Substring(namePropertyIndx, apiOutput.IndexOf(",", namePropertyIndx) - namePropertyIndx);
                apiOutput = apiOutput.Substring(apiOutput.IndexOf(":") + 1);
                apiOutput = apiOutput.Replace(@"""", "").Replace(".", "").Replace("v", "").Trim();
                int latestVer = int.Parse(apiOutput);
                if (latestVer > currentVersion)
                { return latestVer; }
                else
                { return 0; }
            }
            catch (Exception a)
            { log.Append("Failed to check for updates!" + a.ToString()); return 0; }
        }
        public int getCurrentVer()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string ver = fvi.FileVersion.Replace(".", "").Trim();
            return int.Parse(ver);
        }
        private void notify4Updates(bool showNoUpdateFound = false, bool overrideDontShow = false)
        {
            funcs.StartThread(() =>
            {
                int updateVer = thereIsUpdate();

                if (updateVer != 0 && !Properties.Settings.Default.disableUpdateNotif || updateVer != 0 && Properties.Settings.Default.notifDisabledAtVersion < updateVer || updateVer != 0 && overrideDontShow)//in the second part of the OR statement, we check if a new version was released since the last time the user chose to not recieve new update notifications. If there is a newer version, then show the notification anyways
                {
                    methodInvoke(() =>
                    {
                        bool dontShow = false;
                        if (MsgBox.Dontshow(ref dontShow, getGeneralLang("new_update_available")[1], getGeneralLang("new_update_available")[2], MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            Process.Start("https://github.com/Zeromix9/Server-Creation-Tool");
                        }
                        Properties.Settings.Default.disableUpdateNotif = dontShow;
                        if (dontShow)
                        { Properties.Settings.Default.notifDisabledAtVersion = updateVer; }
                        Properties.Settings.Default.Save();
                    });
                }
                else if (updateVer == 0 && showNoUpdateFound)
                {
                    methodInvoke(() =>
                    {
                        MsgBox.Show(getGeneralLang("no_update_available")[1], getGeneralLang("no_update_available")[2], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    });
                }
            });
        }
        public void refresh()
        {
            methodInvoke(() =>
            {
                //DO NOT FORGET
                //try
                // {
                srvBtnClicked(clickedSrvBtn);
                //   }
                // catch { }
                //clickedSrvBtn.PerformClick();
            });
            // showInstalledSrvs();
        }

        private void aTimer()
        {
            funcs.StartThread(() =>
                {
                    while (!toolExiting)
                    {
                        Thread.Sleep(4000);
                        log.Save();
                    }
                });
        }
        private void showInstanceFolderSize()
        {
            funcs.InvokeIfRequired(srvSizeLbl, () => srvSizeLbl.Text = "CALCULATING...");
            funcs.StartThread(() =>
            {
                bool cancel = false;
                funcs.StartThread(() =>
                {
                    while (!cancelActions && !cancel)
                    { Thread.Sleep(100); }
                    //if cancelActions is true then stop calculating server size
                    cancel = true;
                });
                long folderBytes = funcsClass.DirSize(new DirectoryInfo(getCurrentInstancePath()), ref cancel);
                int power;
                string unitSuffix;
                int decimalPoints;
                if (folderBytes >= Math.Pow(10, 9))
                {
                    power = 9;//10^9 is gigabytes
                    unitSuffix = " GB";
                    decimalPoints = 2;
                }
                else
                {
                    power = 6;//10^6 is megabytes
                    unitSuffix = " MB";
                    decimalPoints = 0;
                }
                string size = Math.Round(folderBytes / Math.Pow(10, power), decimalPoints).ToString() + unitSuffix;
                cancel = true;//signal the cancel checking loop to exit
                funcs.InvokeIfRequired(srvSizeLbl, () => srvSizeLbl.Text = size);
            });
        }
        public string getServerDataStr(string name)
        {
            return getStrFromRes(name, srvDat_res_man);
        }
        public object getServerDataObj(string name)
        {
            return getObjFromRes(name, srvDat_res_man);
        }
        public string getStrFromRes(string name, ResourceManager rm)
        {
            string str = rm.GetString(name, CultureInfo.CurrentUICulture);
            if (str != null)
            {
                if (str.Trim() != "")
                {
                    str = str.Trim();
                    // Debug.WriteLine(str.Substring(0, 2));
                    // Debug.WriteLine(str[str.Length - 1]);
                    if (str.Substring(0, 2) == "~[" && str[str.Length - 1] == ']')
                    {
                        str = str.Substring(2, str.Length - 3);
                        str = getStrFromRes(str, rm);
                    }

                }

            }
            return str;
        }
        public object getObjFromRes(string name, ResourceManager rm)
        {
            return rm.GetObject(name, CultureInfo.CurrentUICulture);
        }
        public string getSpecificSrvRootFold(string codename)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(Program));
            ResourceManager resm = new ResourceManager(server_data_files_path + "." + codename + "." + codename, assembly);
            return getStrFromRes("root_folder", resm);
        }
        //first bool returns the result of the invoked method, second returns if the method exists
        public (bool, object) runSrvMethod(string methodname)//if it returns false it means the method was not found
        {
            return runSrvMethodArgs(methodname, null);
        }
        public (bool, object) runSrvMethodArgs(string methodname, object[] args)//idk if the args are actually passed by reference. If you dont care about reference it works tho.NOTE: wtf am i saying here?
        {
            //  MethodInfo method = type.GetMethod(methodname, BindingFlags.Static | BindingFlags.Public);
            //  method.Invoke(null, null);
            MethodInfo mi;
            try
            {
                mi = srv_funcs.GetMethod(methodname);
            }
            catch (Exception ex) { mi = null; Debug.WriteLine(@"ERROR runSrvMethodArgs('" + methodname + "'): " + ex.ToString()); return (false, null); }
            object returnVal = null;
            returnVal = mi.Invoke(srv_funcs_instance, args);
            return (true, returnVal);

        }
        public string getSrvNameFromDir(string instancedirName = "", string codenm = "")//if left empty, it fetches the current instanceDirName
        {
            string name;
            if (instancedirName == "") { name = SrvInstanceRootFold; }
            else { name = instancedirName; }
            if (codenm == "") codenm = codename;

            try
            {
                name = name.Substring(0, name.LastIndexOf(getSpecificSrvRootFold(codenm))).Trim();
                name = name.Replace("-", " ").Trim();
            }
            catch { name = ""; }
            if (name == "")
            { return clickedSrvBtn.Text; }
            else return name;
        }
        public string srvName()
        {
            return clickedSrvBtn.Text.Trim();
        }
        public string getSrvType()
        {
            return getServerDataStr("server_type");
        }
        public string getCurrentInstancePath()
        {
            return serversInstDir + "\\" + SrvInstanceRootFold;
        }
        #endregion
        //------------------------------------ENDREGION

        #region server main panel
        private void installSrvBtn_Click(object sender, EventArgs e)
        {
            string btnState = installSrvBtn.Tag.ToString();
            if (btnState == "install")
            {
                setSrvInstBtn("stop");
                startSrvInstall();
            }
            else if (btnState == "start")
            {
                setSrvInstBtn("starting");
                startServer();
            }
            else if (btnState == "stop")
            {
                stopSrvInstall();
            }
        }
        private void startSrvInstall(string updateOrInstall = "install")//should be either "install" or "update"
        {
            funcs.StartThread(() =>
            {
                disableUI();
                if (getServerDataStr("custom_install_func") != null || getSrvType() == "non_steam")//a steam server could also have custom install function also. i dunno why but it could if the situation asks for it
                {
                    object[] args = new object[1];
                    args[0] = updateOrInstall;
                    //NOTE: since non steam servers do not have a standardized installation process, we write everything manually in the installServer method
                    runSrvMethodArgs("installServer", args); //ref not needed?
                                                             //wait and do UI stuff here or in the runSrvFunc? while ()
                }
                else if (getSrvType() == "steam")
                {
                    if (setupSteamCMD()) installSteamServer(updateOrInstall);
                }
                taskEnded();
                steamCMDProcClear();
                enableUI(true);//DO NOT FORGET
                refresh();
            });
        }

        private void startServer()
        {
            void end()
            {
                taskEnded();
                setSrvInstBtn("start");
            }
            taskStarted("Starting " + srvName() + " server");
            if (getServerDataStr("start_bat_file_required") != null)
            {
                if (!File.Exists(getCurrentInstancePath() + getServerDataStr("start_bat_file_path")))//FIX THIS. DO NOT FORGET
                {
                    createStartBatFile();
                }
            }

            Process process = null;
            if (getServerDataStr("custom_start_func") != null)
            { process = null; runSrvMethod("startServer"); }
            else
            {
                if (getServerDataStr("run_custom_start_func_first") != null)
                {
                    runSrvMethod("startServer");
                }
                //setup window where the user will choose which starting file to launch
                string startFileLoc = "";
                int i = 0;
                int filesFound = 0;
                string lastFileFoundPath = "";
                startSrvForm startFrm = new startSrvForm(this);
                while (true)
                {
                    string ending = "";
                    if (i != 0)
                    {
                        ending = "_" + i;
                    }
                    string startFilePath = getServerDataStr("start_file_path" + ending);
                    if (startFilePath != null)
                    {
                        if (File.Exists(getCurrentInstancePath() + startFilePath))//if file is not found, skip it and check for the next one
                        {
                            startFrm.addBtn(getGeneralLang("start_server_from")[1] + " " + Path.GetFileName(startFilePath), getCurrentInstancePath() + startFilePath);
                            filesFound++;
                            lastFileFoundPath = getCurrentInstancePath() + startFilePath;
                        }
                    }
                    else break;//no other possible start files present
                    i++;
                }
                if (filesFound == 1)
                {
                    startFrm.Dispose();//only 1 start file found. There is no need to show the user any option to choose from
                    startFileLoc = lastFileFoundPath;
                    process = new Process();
                    process.StartInfo.FileName = startFileLoc;
                    process.StartInfo.WorkingDirectory = new FileInfo(startFileLoc).Directory.FullName;
                }
                else
                {
                    //show form with start file options 
                    startFrm.ShowDialog();
                    if (startFrm.DialogResult == DialogResult.Cancel || startFrm.DialogResult == DialogResult.None) { end(); return; };
                    process = startFrm.returnProc;
                }
                //start the server process
                cTry(() =>
                {
                    process.Start();
                }, true, snprintf(getGeneralLang("srv_start_fail")[1], new[] { srvName() }), getGeneralLang("srv_start_fail")[2], true, "Failed to start the " + srvName() + " server");
            }
            //wait a bit so that users can't spam the start button
            funcs.StartThread(() =>
            {
                int i = 0;
                try
                {
                    if (process == null)
                    {
                        while (i < 20)
                        {
                            Thread.Sleep(200);
                            i++;
                        }
                    }
                    else
                    {
                        while (!process.HasExited && i < 20)
                        {
                            Thread.Sleep(200);
                            i++;
                        }
                    }
                }
                catch { }
                end();
            });
        }

        private void createStartBatFile()// DO NOT FORGET. delete?
        {
            if (getServerDataStr("start_bat_file_create_custom_func") != null)
            {
                runSrvMethod("createBatchFile");
            }
            else if (getServerDataStr("start_bat_file_code") != null)
            {
                funcs.writeTxtFile(getCurrentInstancePath() + getServerDataStr("start_file_path"), getServerDataStr("start_bat_file_code"), "ERROR: Failed to write " + srvName() + " server batch file");
            }
        }
        //disable_UI = "" does nothing, disable_UI = "all" disables everything, disable_UI = "serverPanel" disables only the server panel
        public bool easyDownloadFile(string source, string dest, bool showTaskProg = true, string disable_IU = "", string errorMsg = "", string errorTitle = "", string doneMsg = "", string doneTitle = "")
        {
            if (disable_IU == "all")
            {
                disableUI();
            }
            else if (disable_IU == "serverPanel")
            {
                disableMainPnl();
            }
            // taskStarted("Downloading custom config file...", true, true);
            fileDownloader downloader;
            if (showTaskProg)
            {
                downloader = new fileDownloader(taskProgBar, extrTaskInfLbl);
            }
            else
            {
                downloader = new fileDownloader();
            }
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            downloader.download(source, dest, true);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Thread.Sleep(1000);
            while (!downloader.downloadCompleted && !downloader.failed)
            { Thread.Sleep(200); }
            if (disable_IU == "all")
            {
                enableUI();
            }
            else if (disable_IU == "serverPanel")
            {
                enableMainPnl();
            }
            if (showTaskProg) taskEnded();
            if (downloader.failed || downloader.downloadProgPercent[0] < 99)
            {
                if (errorTitle != "") MsgBox.Show(errorMsg, errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.Append("Download action failed: " + errorMsg);
                return false;
            }
            if (doneTitle != "") MsgBox.Show(doneMsg, doneTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }
        private bool stopSrvInstall()
        {
            if (serverType == "steam")
            {
                if (MsgBox.Show(getGeneralLang("ask_force_stop_install_steam")[1], getGeneralLang("ask_force_stop_install_steam")[2], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return false;
                }
            }
            else
            {
                if (MsgBox.Show(getGeneralLang("ask_force_stop_install_general")[1], getGeneralLang("ask_force_stop_install_general")[2], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return false;
                }
            }
            try
            {
                if (getServerDataStr("custom_install_func") != null)
                {
                    return (bool)runSrvMethod("stopServerInstall").Item2;
                }
                else
                {
                    steamCMDProc.Kill();
                    steamCMDProcClear();
                }

            }
            catch { return false; }
            // taskEnded();
            // enableUI();
            //refresh();
            return true;
        }
        Process steamCMDProc;
        void steamCMDProcClear()
        {
            try { steamCMDProc.Dispose(); } catch { }
            steamCMDProc = null;
        }

        steamCMDout steamCMDoutputFrm;
        private void installSteamServer(string updateOrInstall = "install", string extraArgs = "")
        {

            string str;
            if (updateOrInstall == "install")
            {
                str = getGeneralLang("installing_srv")[1];
            }
            else
            {
                str = getGeneralLang("srv_updating_repairing")[1];
            }
            taskStarted(str);
            steamCMDProc = new Process();
            steamCMDProc.StartInfo.FileName = steamCMDPath;
            steamCMDProc.StartInfo.Arguments = " +force_install_dir ./" + SrvInstanceRootFold + "/ +login anonymous +app_update " + getServerDataStr("steamID") + " validate";
            bool res = cTry(() =>
             {
                 steamCMDProc.Start();
                 try
                 {
                     while (!steamCMDProc.HasExited)
                     {
                         Thread.Sleep(100);
                     }
                 }
                 catch { }
             }, false);
            if (!res)//check if the installation started successfully or not
            {
                string[] message = new string[2];
                message[1] = getGeneralLang("info")[0];
                if ( updateOrInstall == "update")
                {
                    message[0] = getGeneralLang("srv_update_repair_failed")[1];
                }
                else
                {
                    message[0] = getGeneralLang("srv_install_failed")[1];
                }
                MsgBox.Show(message[0], message[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            taskEnded();
        }
        private void installSteamServer_unused(string updateOrInstall = "install", string extraArgs = "")
        {

            string str;
            if (updateOrInstall == "install")
            {
                str = "Installing " + srvNameANDinstancesDropDownBtn.Text + " server...";
            }
            else
            {
                str = @"Updating\Repairing " + srvNameANDinstancesDropDownBtn.Text + " server...";
            }
            taskStarted(str);
            var steamCMDConPTY = new SteamCMDConPTY();
            steamCMDConPTY.Arguments = " +force_install_dir ./" + SrvInstanceRootFold + "/ +login anonymous +app_update " + getServerDataStr("steamID") + " validate";
            steamCMDConPTY.WorkingDirectory = serversInstDir;
            steamCMDConPTY.FilterControlSequences = true;
            steamCMDConPTY.TitleReceived += (sender, data) =>
            {
                if (!String.IsNullOrEmpty(data))
                {
                    funcs.InvokeIfRequired(steamCMDoutputFrm.textBox1, () => steamCMDoutputFrm.textBox1.AppendText(data + Environment.NewLine));
                }
            };
            steamCMDConPTY.OutputDataReceived += (sender, data) =>
            {
                if (!String.IsNullOrEmpty(data))
                {
                    funcs.InvokeIfRequired(steamCMDoutputFrm.textBox1, () => steamCMDoutputFrm.textBox1.AppendText(data + Environment.NewLine));
                }
            };
            steamCMDConPTY.Exited += (sender, exitCode) =>
            {
                steamCMDProcClear();
                taskEnded();
                refresh();
            };
            // steamCMDProc = new Process();
            // steamCMDProc.StartInfo.WorkingDirectory = serversInstDir;
            // steamCMDProc.StartInfo.FileName = steamCMDPath;
            // steamCMDProc.StartInfo.Arguments = " +force_install_dir ./" + SrvInstanceRootFold + "/ +login anonymous +app_update " + getServerDataStr("steamID") + " validate";
            //
            // steamCMDProc.StartInfo.UseShellExecute = false;
            // // steamCMDProc.StartInfo.CreateNoWindow = true;
            //  steamCMDProc.StartInfo.RedirectStandardOutput = true;
            //  steamCMDProc.StartInfo.RedirectStandardError = true;
            // steamCMDProc.StartInfo.RedirectStandardInput = true;
            // steamCMDProc.StartInfo.StandardOutputEncoding = Encoding.Default;
            // steamCMDProc.OutputDataReceived += p_OutputDataReceived;
            // steamCMDProc.ErrorDataReceived += p_OutputDataReceived;
            // steamCMDProc.EnableRaisingEvents = true;
            //
            if (!cTry(() =>
            {
                steamCMDoutputFrm = new steamCMDout();
                ProcessInfo processInfo;
                methodInvoke(() =>
                {
                    steamCMDoutputFrm.Show();
                });
                processInfo = steamCMDConPTY.Start((short)steamCMDoutputFrm.Width, (short)steamCMDoutputFrm.Height);
                steamCMDProc = Process.GetProcessById(processInfo.dwProcessId);
                steamCMDProc.WaitForExit();
            }, true, "Failed to " + updateOrInstall + " the " + srvNameANDinstancesDropDownBtn.Text + " server!", "Failed to " + updateOrInstall + " server!", true, updateOrInstall + " steam server"))
            { steamCMDProcClear(); taskEnded(); refresh(); return; }
        }
        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            // Prepend line numbers to each line of the output.
            if (!String.IsNullOrEmpty(e.Data))
            {
                //  lineCount++;
                funcs.InvokeIfRequired(steamCMDoutputFrm.textBox1, () => steamCMDoutputFrm.textBox1.AppendText(e.Data + Environment.NewLine));


            }
        }
        private void setSrvInstBtn(string state = "install")
        {
            methodInvoke(() =>
            {
                installSrvBtn.Enabled = true;
                if (state == "install")
                {
                    installSrvBtn.ColorHover = Color.FromArgb(74, 164, 255);
                    installSrvBtn.ColorNormal = Color.DodgerBlue;
                    installSrvBtn.ColorPressed = Color.FromArgb(74, 164, 255);
                    installSrvBtn.BackColor = Color.DodgerBlue;
                    installSrvBtn.Image = Properties.Resources.install_img;
                    installSrvBtn.Text = "   Install";
                    installSrvBtn.Tag = "install";
                }
                else if (state == "stop")
                {
                    installSrvBtn.ColorHover = Color.FromArgb(247, 50, 40);
                    installSrvBtn.ColorNormal = Color.FromArgb(222, 45, 35);
                    installSrvBtn.ColorPressed = Color.FromArgb(247, 50, 40);
                    installSrvBtn.BackColor = Color.FromArgb(222, 45, 35);
                    installSrvBtn.Image = Properties.Resources.stop_img;
                    installSrvBtn.Text = "    FORCE STOP";
                    installSrvBtn.Tag = "stop";
                }
                else if (state == "start")
                {
                    installSrvBtn.ColorHover = Color.FromArgb(89, 204, 42);
                    installSrvBtn.ColorNormal = Color.FromArgb(41, 184, 69);
                    installSrvBtn.ColorPressed = Color.FromArgb(89, 204, 42);
                    installSrvBtn.BackColor = Color.FromArgb(41, 184, 69);
                    installSrvBtn.Image = Properties.Resources.start_img;
                    installSrvBtn.Text = "   Start";
                    installSrvBtn.Tag = "start";
                }
                else if (state == "starting")
                {
                    installSrvBtn.ColorHover = Color.FromArgb(89, 204, 42);
                    installSrvBtn.ColorNormal = Color.FromArgb(42, 199, 73);
                    installSrvBtn.ColorPressed = Color.FromArgb(89, 204, 42);
                    installSrvBtn.Enabled = false;
                    installSrvBtn.BackColor = Color.FromArgb(42, 199, 73);
                    installSrvBtn.Image = Properties.Resources.start_img;
                    installSrvBtn.Text = "   Starting";
                    installSrvBtn.Tag = "starting";
                }
            });

        }

        public bool setMainPanelstate(string installedOrNotInstalled = "")//checks if server is installed and disabled/enables stuff accordingly. returns if server is installed or not
        {
            bool installed;
            if (installedOrNotInstalled == "installed") installed = true;
            else if (installedOrNotInstalled == "notinstalled") installed = false;
            else installed = checkIfSrvInstalled();
            if (installed)
            {
                setSrvInstBtn("start");
                enableMainPnl();
                return true;
            }
            else { disableMainPnl(); setSrvInstBtn("install"); return false; }
        }
        private void addSrvFav_Click(object sender, EventArgs e)
        {
            if (addSrvFavBtn.Toggled)
            {
                addSrvFavBtn.Image = Properties.Resources.starChecked;
                addFav();
            }
            else
            {
                addSrvFavBtn.Image = Properties.Resources.starUnChecked;
                RemFav();
            }
        }
        List<customSmoothBtn> extraBtnsNODelete = new List<customSmoothBtn>();
        List<customSmoothBtn> ActBtnsNODelete = new List<customSmoothBtn>();
        private void clearExtrAndActPanels()
        {
            lastExtrBtn = null;
            lastActBtn = null;
            moreBtnAddedExtr = false;
            moreBtnAddedAct = false;
            if (moreContexMenuExtr != null) moreContexMenuExtr.Dispose();
            moreContexMenuExtr = null;
            if (moreContexMenuAct != null) moreContexMenuAct.Dispose();
            moreContexMenuAct = null;
            if (stgContexMenuAct != null) stgContexMenuAct.Dispose();
            stgContexMenuAct = null;
            foreach (var btn in extraPnl.Controls.OfType<customSmoothBtn>().Except(extraBtnsNODelete).ToList())
            { extraPnl.Controls.Remove(btn); }
            foreach (var btn in actPnl.Controls.OfType<customSmoothBtn>().Except(ActBtnsNODelete).ToList())
            { actPnl.Controls.Remove(btn); }
        }
        contexMenuStripFrm moreContexMenuExtr = null;
        contexMenuStripFrm moreContexMenuAct = null;
        contexMenuStripFrm stgContexMenuAct = null;
        bool moreBtnAddedExtr = false;
        bool moreBtnAddedAct = false;
        customSmoothBtn lastExtrBtn = null;
        customSmoothBtn lastActBtn = null;
        public void addExtraBtn(string text, Action action, Image img = null, bool doubleTextRows = false, bool isLastBtn = false)//doubleTextRows does not do anything when the buttons are added in the contex menu strip, since it can dynamically adjust its length based on the longest button text
        {
            int btnHeight;
            if (doubleTextRows) btnHeight = 35;
            else btnHeight = instGuideBtn.Height;
            customSmoothBtn moreBtn = instGuideBtn.CloneCtrl();
            extraPnl.Controls.Remove(moreBtn);//cloning the control also adds automatically it so we must remove it first.
            lastExtrBtn = moreBtn;
            moreBtn.Image = img;
            moreBtn.Name = "extraBtn" + lastExtrBtn.Bottom;
            moreBtn.Margin = new Padding(0);
            if (lastExtrBtn.Bottom + btnHeight >= extraPnl.Height && (!isLastBtn || moreContexMenuExtr != null))//check if the next to be added button will come close to getting out of bounds. If yes add a "more" dropdown button and add the rest of the buttons in there
            {
                if (!moreBtnAddedExtr)
                {
                    moreContexMenuExtr = new contexMenuStripFrm();
                    moreContexMenuExtr.hideInsteadOfClose = true;//hide it instead of closing it because after closing it we would have to add all the buttons again and in this case we can't
                    moreBtn.Text = getGeneralLang("moreBtn")[1];
                    moreBtn.Click += (_, _2) => funcs.showMenuStripAtBtn(moreContexMenuExtr, moreBtn, this);
                    extraPnl.Controls.Add(moreBtn);
                    moreBtnAddedExtr = true;
                }
                moreContexMenuExtr.addBtn(text, action, !isLastBtn, img);
            }
            else
            {
                moreBtn.Height = btnHeight;
                moreBtn.Text = text;
                moreBtn.Click += (_, _2) => action();
                extraPnl.Controls.Add(moreBtn);
            }
        }

        private void addSettingInActPnl(string text, Action action, Image img = null, bool isLastBtn = false)
        {
            if (stgContexMenuAct == null)
            {
                stgContexMenuAct = new contexMenuStripFrm();
                stgContexMenuAct.hideInsteadOfClose = true;//hide it instead of closing it because after closing it we would have to add all the buttons again and in this case we can't
                editStgBtn.Click += (_, _2) => funcs.showMenuStripAtBtn(stgContexMenuAct, editStgBtn, this);
            }
            stgContexMenuAct.addBtn(text, action, !isLastBtn, img);
        }
        public void addActionBtn(string text, Action action, Image img = null, bool doubleTextRows = false, bool isLastBtn = false)//make the actions and extra functions INTO 1!!!!!
        {
            int btnHeight;
            if (doubleTextRows) btnHeight = 35;
            else btnHeight = delBtn.Height;
            customSmoothBtn moreBtn = delBtn.CloneCtrl();
            actPnl.Controls.Remove(moreBtn);//cloning the control also adds automatically it so we must remove it first.
            lastActBtn = moreBtn;
            moreBtn.Image = img;
            moreBtn.Name = "extraBtn" + lastActBtn.Bottom;
            moreBtn.Margin = new Padding(0);
            if (lastActBtn.Bottom + btnHeight >= actPnl.Height && (!isLastBtn || moreContexMenuAct != null))//check if the next to be added button will come close to getting out of bounds. If yes add a "more" dropdown button and add the rest of the buttons in there
            {
                if (!moreBtnAddedAct)
                {
                    moreContexMenuAct = new contexMenuStripFrm();
                    moreContexMenuAct.hideInsteadOfClose = true;//hide it instead of closing it because after closing it we would have to add all the buttons again and in this case we can't
                    moreBtn.Text = getGeneralLang("moreBtn")[1];
                    moreBtn.Click += (_, _2) => funcs.showMenuStripAtBtn(moreContexMenuAct, moreBtn, this);
                    actPnl.Controls.Add(moreBtn);
                    moreBtnAddedAct = true;
                }
                moreContexMenuAct.addBtn(text, action, !isLastBtn, img);
            }
            else
            {
                moreBtn.Height = btnHeight;
                moreBtn.Text = text;
                moreBtn.Click += (_, _2) => action();
                actPnl.Controls.Add(moreBtn);
            }
        }
        private void setSrvImage()
        {
            try
            {
                srvImgPicBox.BackgroundImage = (getServerDataObj("logo") as Image);
                string sizeStr = getServerDataStr("logo_size");
                List<int> sizeDat = sizeStr.Split(',').Select(int.Parse).ToList();
                int w = sizeDat[0];
                int h = sizeDat[1];
                int offset = 0;
                if (sizeStr.Count(f => f == ',') > 1)//more than 1 comma means there is a Y offset parameter
                {
                    offset = sizeDat[2];
                }
                srvImgPicBox.Size = new Size(w, h);
                int Xhalf = w / 2;
                int Yhalf = h / 2;
                int xloc = picboxCenter.Location.X - Xhalf;
                int yloc = picboxCenter.Location.Y - Yhalf;
                srvImgPicBox.Location = new Point(xloc, yloc + offset);
            }
            catch
            {
                srvImgPicBox.BackgroundImage = Properties.Resources.icons8_server_48__1_;
                srvImgPicBox.Size = new Size(85, 80);// DO NOT FORGET TO CHANGE AFTER CHANGING IT IN THE DESIGNER
            }
        }
        List<Button> srvListButtonsSorted = new List<Button>();
        int scrollPnlHeightALLSERVERS;
        private void addSrvBtns()
        {

            scrollPnlHeightALLSERVERS = welcomeBtn.Height + reloadSrvsBtn.Height;
            ResourceSet resourceSet = srvNamesResMan.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
            {
                Button btn = createSrvBtn(entry.Value.ToString(), entry.Key.ToString());
                if (checkIfSrvInstalled(entry.Key.ToString()))
                {
                    btn.ForeColor = SystemColors.ControlLightLight;
                }
                else
                {
                    btn.ForeColor = Color.FromArgb(138, 138, 138);
                }
                srvListButtonsSorted.Add(btn);
                scrollPnlHeightALLSERVERS += srvListButtonsSorted.Last().Height;
            }
            srvListButtonsSorted = srvListButtonsSorted.OrderBy(x => x.Text).ToList();
            scrollPnlSrvList.EditablePanel.Controls.AddRange(srvListButtonsSorted.ToArray());
            scrollPnlSrvList.resizeScrollPnl(scrollPnlHeightALLSERVERS);

        }
        private Button createSrvBtn(string text, string gameCode)
        {
            Button btn = new Button();
            btn.Location = welcomeBtn.Location;
            btn.Text = text;
            btn.Tag = gameCode;
            btn.Font = new Font("Segoe UI", 9);
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = Color.Transparent;
            btn.ForeColor = SystemColors.ControlDarkDark;
            btn.Margin = new Padding(0, 0, 0, 0);
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Properties.Settings.Default.ColorHover;
            btn.FlatAppearance.MouseDownBackColor = Properties.Settings.Default.colorPressed;
            btn.Width = scrollPnlSrvList.EditablePanel.Width - 5;
            cTry(() =>
            {
                // EventHandler eh = (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), this, "srvBtn_Click", false);
                // btn.Click += new EventHandler(eh);
                btn.Click += (_, _2) => srvBtnClicked(btn);
            });
            return btn;
        }
        Button clickedSrvBtn = null;
        string serverType = "";
        string SrvInstanceRootFold = ""; //An empty string means the default instance is selected
        List<string> serverInstances = new List<string>();
        public string codename;
        public string defaultRootFold;
        public ResourceManager srvDat_res_man = null;
        public Type srv_funcs;
        public object srv_funcs_instance;
        bool cancelActions = false;//used when switching server

        private void srvBtnClicked(Button clickedBtn, string codenm = "")
        {
            if (clickedBtn == null) return;
            enableMainPnl();
            cancelActions = true;
            clickedSrvBtn = clickedBtn;
            SrvInstanceRootFold = "";
            serverInstances.Clear();
            welcomeBtn.BackColor = Color.Transparent;
            welcomeBtn.ColorNormal = Color.Transparent;
            if (clickedBtn == null)
            {
                codename = codenm;
            }
            else codename = clickedBtn.Tag.ToString();
            showInstalledSrvs();
            Assembly assembly = Assembly.GetAssembly(typeof(Program));
            //create resource manager to read data from the server's resource file
            srvDat_res_man = new ResourceManager(server_data_files_path + "." + codename + "." + codename, assembly);//find and load the selected server's data
            //load the server's funcs class
            //Debug.WriteLine(server_data_files_path + "." + codename + "." + codename + "_funcs");
            try
            {
                srv_funcs = assembly.GetType(server_data_files_path + "." + codename + "." + codename + "_funcs"); // search type (class) inside assembly
                srv_funcs_instance = Activator.CreateInstance(srv_funcs, this);
            }
            catch { srv_funcs = null; }

            //visual stuff
            clearSrvPnlSelection();
            clearExtrAndActPanels();//clear all the stuff from the previous server to make way for the stuff of the new server
            clickedSrvBtn.BackColor = Properties.Settings.Default.serverBtnSelectedColor;
            setSrvImage();
            //CHECK how many instances of the selected servers there are
            if (!cTry(() => defaultRootFold = getServerDataStr("root_folder"), true, "", "Couldn't load server"))
            { welcomeBtn.PerformClick(); defaultRootFold = ""; return; }
            serverType = getServerDataStr("server_type");
            SrvInstanceRootFold = defaultRootFold;
            //ENABLE THIS CODE WHEN THE INSTANCE SYSTEM IS READY
            //string[] dirs = Directory.GetDirectories(serversInstDir, "*", SearchOption.TopDirectoryOnly);
            //string firstDirFound = "";
            //foreach (string dir in dirs)
            //{
            //    string dirName = new DirectoryInfo(dir).Name;
            //    try
            //    {
            //        if (dirName.Contains(defaultRootFold))
            //        {
            //            serverInstances.Add(dirName);
            //            if (firstDirFound == "") firstDirFound = dirName;
            //            if (dirName.EndsWith("ds"))//the default instance ends with "ds". while a non default may end like that: "ds1", "ds3" etc.
            //            {
            //                SrvInstanceRootFold = dirName;
            //            }
            //        }
            //    }
            //    catch { }
            //}
            //if (firstDirFound != "")// that means at least one instance was found
            //{
            //    enableMainPnl();
            //    if (SrvInstanceRootFold == "")//If the default instance folder is empty aka not found, randomly select the first instance found
            //    {
            //        SrvInstanceRootFold = firstDirFound;
            //    }
            //    srvNameANDinstancesDropDownBtn.Text = getSrvName();
            //}
            //else { disableMainPnl(); srvNameANDinstancesDropDownBtn.Text = clickedBtn.Text; }

            //DISABLE THIS CODE WHEN THE INSTANCE SYSTEM IS READY
            srvNameANDinstancesDropDownBtn.Text = clickedBtn.Text;
            mainSrvPanel.Visible = true;
            //-----------------------

            //set guide button URL 
            funcs.RemoveClickEvent(instGuideBtn);
            if (getServerDataStr("guide_link_en") != null)
            {
                string selectedLanguage = "en";//DO NOT FORGET this is temporary. Will be replaced by the currently selected language variable
                if (getServerDataStr("guide_link_" + selectedLanguage) == null)
                {
                    instGuideBtn.Click += (_, _2) => Process.Start(getServerDataStr("guide_link_" + "en"));//choose en which is the default lang
                }
                else if (getServerDataStr("guide_link_" + selectedLanguage).Trim() == "")
                {
                    instGuideBtn.Click += (_, _2) => Process.Start(getServerDataStr("guide_link_" + "en"));//choose en which is the default lang
                }
                else
                {
                    instGuideBtn.Click += (_, _2) => Process.Start(getServerDataStr("guide_link_" + selectedLanguage));//DO NOT FORGET TO CHANGE WHEN I FINISH LANGUAGE SYSTEM 
                }
                instGuideBtn.Visible = true;
            }
            else instGuideBtn.Visible = false;
            if (Properties.Settings.Default.favServers != null)//check if its been added to the favourites to make the star in the main panel light up or not.
            {
                if (Properties.Settings.Default.favServers.Contains(codename))
                {
                    addSrvFavBtn.Image = Properties.Resources.starChecked;
                    addSrvFavBtn.Toggled = true;
                }
                else
                { addSrvFavBtn.Image = Properties.Resources.starUnChecked; addSrvFavBtn.Toggled = false; }
            }
            //set the custom batch file button in the extra panel
            Action action = null;
            if (getServerDataStr("start_bat_file_create_custom_func") != null)
            {
                if (getServerDataStr("start_bat_file_required") != null)
                {
                    if (getServerDataStr("start_bat_file_required") == "showCreateBtn")
                    {
                        action = () => runSrvMethod("createBatchFile");
                    }
                }
                else
                {
                    action = () => runSrvMethod("createBatchFile");
                }
            }
            else if (getServerDataStr("start_bat_file_code") != null && getServerDataStr("start_bat_file_required") == null)
            {
                action = () =>
                {
                    if (funcs.writeTxtFile(getCurrentInstancePath() + getServerDataStr("start_file_path"), getServerDataStr("start_bat_file_code"), "ERROR: Failed to write " + srvName() + " server batch file"))
                    {
                        MsgBox.quickMsg(getControlsLang("create_custom_start_bat_file")[1], getGeneralLang("done")[1], 32);
                    }
                    else
                    {
                        //error msg
                        MsgBox.Show(getGeneralLang("error_occured")[1], getControlsLang("create_custom_start_bat_file")[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
            }
            if (action != null)
            {
                addExtraBtn(getControlsLang("create_custom_start_bat_file")[1], () =>
                {
                    if (MsgBox.Show(getGeneralLang("create_custom_start_bat_file_msg")[1], getControlsLang("create_custom_start_bat_file")[1], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        action();
                    }
                }, null, true);
            }
            //set the downloadable config file button in the extra panel
            if (getServerDataStr("conf_file_link_en") != null || getServerDataStr("custom_conf_file_create_func") != null)
            {
                string lang = Properties.Settings.Default.lang;
                string link = "";
                Action action2 = () => { };
                if (getServerDataStr("conf_file_link_" + lang) != null)//if config file for that language does not exist, use english version
                {
                    link = getServerDataStr("conf_file_link_" + lang);
                }
                else if (getServerDataStr("conf_file_link_en") != null)
                {
                    link = getServerDataStr("conf_file_link_en");
                }
                if (link != "")
                {
                    action2 = () => funcs.StartThread(() =>
                    {
                        funcs.InvokeIfRequired(installSrvBtn,()=> installSrvBtn.Enabled = false);
                        
                        taskStarted(getGeneralLang("downloading_cfg_file")[1], true, true);
                        easyDownloadFile(link, getCurrentInstancePath() + getServerDataStr("conf_file_link_path"), true, "all", getGeneralLang("cfg_file_download_failed")[1], getGeneralLang("error")[1], getGeneralLang("done")[1] + "                           ", getControlsLang("create_custom_conf_file")[1]);
                        funcs.InvokeIfRequired(installSrvBtn, () => installSrvBtn.Enabled = true);

                    });
                }
                //
                if (getServerDataStr("custom_conf_file_create_func") != null)
                {
                    action2 = () => runSrvMethod("createConfFile");
                }
                addExtraBtn(getControlsLang("create_custom_conf_file")[1], () =>
                {
                    if (MsgBox.Show(getGeneralLang("create_confg_file_ask")[1], getControlsLang("create_custom_conf_file")[1], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        action2();
                    }
                }, null, false);

            }
            //see if various config files exist and add them to the edit settings menu
            int i = 0;
            bool isLastBtn = false;
            while (true)
            {
                string ending = "";
                if (i != 0)
                {
                    ending = "_" + i;
                }
                string cfgFilePath = getServerDataStr("conf_file_path" + ending);
                if (cfgFilePath == null) break;
                if (getServerDataStr("conf_file_path_" + (i + 1)) == null) isLastBtn = true;//if it returns null it means its the last config file.
                addSettingInActPnl("Edit " + Path.GetFileName(cfgFilePath), () =>
                {
                    cTry(() => Process.Start(Properties.Settings.Default.defaultTextEditor, getCurrentInstancePath() + cfgFilePath), true, getGeneralLang("file_not_found")[1], "Edit Settings", true);
                }, null, isLastBtn);

                i++;
            }
            //DISABLE THIS CODE WHEN THE INSTANCE SYSTEM IS READY
            runSrvMethod("setup");//MUST BE LAST. 
            setMainPanelstate();
            //----------------
            cancelActions = false;
            //object[] args = new object[1];
            // args[0] = this;
            showInstanceFolderSize();
        }

        private void OpenFoldBtn_Click_1(object sender, EventArgs e)
        {
            Process.Start(serversInstDir + @"\" + SrvInstanceRootFold);
        }
        private void delBtn_Click_1(object sender, EventArgs e)
        {
            if (MsgBox.Show(getGeneralLang("ask_delete_server")[1], snprintf(getGeneralLang("ask_delete_server")[2], new[] { srvNameANDinstancesDropDownBtn.Text }), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                taskStarted(getGeneralLang("deleting")[1] + srvName() + "...");
                disableUI();
                installSrvBtn.Enabled = false;
                funcs.StartThread(() =>
                {
                    cTry(() =>
                    {
                        funcsClass.DeleteDirectory(serversInstDir + "\\" + SrvInstanceRootFold);

                    }, true, getGeneralLang("srv_delete_fail")[1], snprintf(getGeneralLang("ask_delete_server")[2], new[] { srvNameANDinstancesDropDownBtn.Text }), true);
                    taskEnded();
                    enableUI();
                    methodInvoke(() => { installSrvBtn.Enabled = true; });
                    refresh();
                    methodInvoke(() => MsgBox.quickMsg(snprintf(getGeneralLang("ask_delete_server")[2], new[] { srvNameANDinstancesDropDownBtn.Text }), getGeneralLang("done")[1], 55));
                });
            }
        }
        private void updateBtn_Click(object sender, EventArgs e)
        {
            setSrvInstBtn("stop");
            startSrvInstall("update");
        }
        private void srvNameAndInstanceOptions_Click_1(object sender, EventArgs e)
        {
            MsgBox.Show("Multiple server instances coming soon!", "Coming soon", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
            Func<contexMenuStripFrm, contexMenuStripFrm> instanceOptionsMenuCreator = (menuStrp) =>
            {
                //More Instance Options menu strip
                contexMenuStripFrm instanceOptionsMenu = new contexMenuStripFrm();
                instanceOptionsMenu.parent = menuStrp;
                instanceOptionsMenu.addBtn("Make this instance default", null, true, Properties.Resources.icons8_plus_math_16__2_);
                instanceOptionsMenu.addBtn("Rename", () => Debug.Write("test"), false, Properties.Resources.icons8_plus_math_16__2_);
                return instanceOptionsMenu;
            };

            //Server Instances menu strip
            contexMenuStripFrm srvInstancesMenu = new contexMenuStripFrm();
            srvInstancesMenu.addLabel("-Server Instance options-");
            srvInstancesMenu.addBtn("Create new server instance", null, true, Properties.Resources.icons8_plus_math_16__2_);
            srvInstancesMenu.addSubMenuBtn("More Instance Options", instanceOptionsMenuCreator, false, Properties.Resources.icons8_plus_math_16__2_);
            srvInstancesMenu.addLabel("-Server Instances-");
            int i = 1;
            foreach (string dir in serverInstances)
            {
                bool addSeparator = true;
                string extrStr = "";
                if (i >= serverInstances.Count) addSeparator = false;
                if (dir.EndsWith(defaultRootFold)) extrStr = " (Default)";
                srvInstancesMenu.addBtn(getSrvNameFromDir(dir) + extrStr, null, addSeparator, Properties.Resources.icons8_dot_24);
                i++;
            }
            funcs.showMenuStripAtBtn(srvInstancesMenu, srvNameANDinstancesDropDownBtn, this);

        }

        bool doNotFireTextChanged = false;
        private void srvNameANDinstancesDropDownBtn_TextChanged(object sender, EventArgs e)
        {
            //ENABLE THIS AFTER FINISHING THE SERVER INSTANCE SYSTEM
            //if (doNotFireTextChanged) { doNotFireTextChanged = false; return; }
            //doNotFireTextChanged = true;
            //srvNameANDinstancesDropDownBtn.Text += "   ▼";
            Size sz = TextRenderer.MeasureText(srvNameANDinstancesDropDownBtn.Text, srvNameANDinstancesDropDownBtn.Font);
            srvNameANDinstancesDropDownBtn.Width = sz.Width + 10;

        }
        #endregion

        private void basicInstrTxtBox_MouseHover(object sender, EventArgs e)
        {
            basicInstrScrollPanel.Focus();
        }

        private void settingsBtn_Click(object sender, EventArgs e)
        {
            settingsForm settingsFrm = new settingsForm(this); //remove callingform
            settingsFrm.ShowDialog();
            //scrollPnlHeightALLSERVERS += 20;
            //resizeScrollPnl(scrollPnlHeightALLSERVERS);
        }


        private void chck4UpdtBtn_Click(object sender, EventArgs e)
        {
            notify4Updates(true, true);
        }


        private void srvInfBtn_Click(object sender, EventArgs e)
        {
            //  Debug.WriteLine(getSrvNameFromInstanceDirName("a-ark_ds1"));
            // clickedSrvBtn.PerformClick();
        }

        private void MainForm_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
