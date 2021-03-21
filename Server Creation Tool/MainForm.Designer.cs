/*
 * Created by SharpDevelop.
 * User: Zeromix
 * Date: 28.05.2016
 * Time: 16:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Server_Creation_Tool
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Elegant.Ui.ThemeSelector themeSelector;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.SteamServertimer = new System.Windows.Forms.Timer(this.components);
            this.generalTimer = new System.Windows.Forms.Timer(this.components);
            this.ribbon1 = new Elegant.Ui.Ribbon();
            this.applicationMenu1 = new Elegant.Ui.ApplicationMenu(this.components);
            this.noInternetTab = new Elegant.Ui.RibbonContextualTabGroup(this.components);
            this.noInternetTab2 = new Elegant.Ui.RibbonTabPage();
            this.ribbonGroup6 = new Elegant.Ui.RibbonGroup();
            this.refreshConnBtn = new Elegant.Ui.Button();
            this.noInternetTxtBox = new Elegant.Ui.TextBox();
            this.mainTabPage = new Elegant.Ui.RibbonTabPage();
            this.generalSvrOptGroup = new Elegant.Ui.RibbonGroup();
            this.openSrvsFoldBtn = new Elegant.Ui.Button();
            this.selectInstFoldBtn = new Elegant.Ui.Button();
            this.optionsRibbon = new Elegant.Ui.RibbonGroup();
            this.chck4UpdatesBtn = new Elegant.Ui.Button();
            this.styleBtn = new Elegant.Ui.DropDown();
            this.popupMenu2 = new Elegant.Ui.PopupMenu(this.components);
            this.button2 = new Elegant.Ui.Button();
            this.button11 = new Elegant.Ui.Button();
            this.langBtn = new Elegant.Ui.DropDown();
            this.popupMenu1 = new Elegant.Ui.PopupMenu(this.components);
            this.engBtn = new Elegant.Ui.Button();
            this.gerBtn = new Elegant.Ui.Button();
            this.currentTaskRibGrp = new Elegant.Ui.RibbonGroup();
            this.currentTaskNameLbl = new Elegant.Ui.Label();
            this.progressBar1 = new Elegant.Ui.ProgressBar();
            this.helpInfTab = new Elegant.Ui.RibbonTabPage();
            this.helpInfGrpBox = new Elegant.Ui.RibbonGroup();
            this.variousStuffBtn = new Elegant.Ui.DropDown();
            this.popupMenu3 = new Elegant.Ui.PopupMenu(this.components);
            this.portFwRouterBtn = new Elegant.Ui.Button();
            this.faqBtn = new Elegant.Ui.Button();
            this.serverGroupBtn = new Elegant.Ui.Button();
            this.separator1 = new Elegant.Ui.Separator();
            this.plannedFeaturesBtn = new Elegant.Ui.Button();
            this.appLogsbtn = new Elegant.Ui.Button();
            this.changeLogBtn = new Elegant.Ui.Button();
            this.donateBtn = new Elegant.Ui.Button();
            this.aboutBtn = new Elegant.Ui.Button();
            this.formFrameSkinner = new Elegant.Ui.FormFrameSkinner();
            this.ribbonContextualTabGroup2 = new Elegant.Ui.RibbonContextualTabGroup(this.components);
            this.ribbonGroup4 = new Elegant.Ui.RibbonGroup();
            this.groupBox2 = new Elegant.Ui.GroupBox();
            this.steamGameSrchBox = new Elegant.Ui.TextBox();
            this.searchGameLbl = new Elegant.Ui.Label();
            this.tabControl1 = new Elegant.Ui.TabControl();
            this.tabPage1 = new Elegant.Ui.TabPage();
            this.steamSvrNavBar = new Elegant.Ui.NavigationBar();
            this.welcomeBtn = new Elegant.Ui.ToggleButton();
            this.daysToDieBtn = new Elegant.Ui.ToggleButton();
            this.arkBtn = new Elegant.Ui.ToggleButton();
            this.atlasBtn = new Elegant.Ui.ToggleButton();
            this.CodBo3Btn = new Elegant.Ui.ToggleButton();
            this.cs16Btn = new Elegant.Ui.ToggleButton();
            this.CSsourceBtn = new Elegant.Ui.ToggleButton();
            this.CSGOBtn = new Elegant.Ui.ToggleButton();
            this.garrysModBtn = new Elegant.Ui.ToggleButton();
            this.hurtWorldBtn = new Elegant.Ui.ToggleButton();
            this.KillingFloorTwoBtn = new Elegant.Ui.ToggleButton();
            this.l4dBtn = new Elegant.Ui.ToggleButton();
            this.l4d2Btn = new Elegant.Ui.ToggleButton();
            this.RustBtn = new Elegant.Ui.ToggleButton();
            this.SvenCoopBtn = new Elegant.Ui.ToggleButton();
            this.synergyBtn = new Elegant.Ui.ToggleButton();
            this.unturnedBtn = new Elegant.Ui.ToggleButton();
            this.separator3 = new Elegant.Ui.Separator();
            this.tabPage2 = new Elegant.Ui.TabPage();
            this.label1 = new Elegant.Ui.Label();
            this.textBox1 = new Elegant.Ui.TextBox();
            this.groupBox1 = new Elegant.Ui.GroupBox();
            this.panel2 = new Elegant.Ui.Panel();
            this.button1 = new Elegant.Ui.Button();
            this.button4 = new Elegant.Ui.Button();
            this.button7 = new Elegant.Ui.Button();
            this.button23 = new Elegant.Ui.Button();
            this.button25 = new Elegant.Ui.Button();
            this.button3 = new Elegant.Ui.Button();
            this.button6 = new Elegant.Ui.Button();
            this.button21 = new Elegant.Ui.Button();
            this.button24 = new Elegant.Ui.Button();
            this.button26 = new Elegant.Ui.Button();
            this.button27 = new Elegant.Ui.Button();
            this.button28 = new Elegant.Ui.Button();
            this.button29 = new Elegant.Ui.Button();
            this.button30 = new Elegant.Ui.Button();
            this.separator2 = new Elegant.Ui.Separator();
            this.panel1 = new Elegant.Ui.Panel();
            this.serverPnl = new Elegant.Ui.GroupBox();
            this.groupBox7 = new Elegant.Ui.GroupBox();
            this.srvSizeLbl = new Elegant.Ui.Label();
            this.srvSizeTxtBox = new Elegant.Ui.TextBox();
            this.extraGrpBox = new Elegant.Ui.GroupBox();
            this.extraBtn4 = new Elegant.Ui.Button();
            this.extraBtn3 = new Elegant.Ui.Button();
            this.extraBtn2 = new Elegant.Ui.Button();
            this.extraBtn1 = new Elegant.Ui.Button();
            this.instGuideBtn = new Elegant.Ui.Button();
            this.groupBox5 = new Elegant.Ui.GroupBox();
            this.installSvrBtn = new Elegant.Ui.Button();
            this.gameImgPicBox = new System.Windows.Forms.PictureBox();
            this.actionsGrpBox = new Elegant.Ui.GroupBox();
            this.act_Btn1 = new Elegant.Ui.Button();
            this.editSettingsDropBtn = new Elegant.Ui.DropDown();
            this.popupMenu4 = new Elegant.Ui.PopupMenu(this.components);
            this.easyStgBtn = new Elegant.Ui.Button();
            this.button8 = new Elegant.Ui.Button();
            this.button9 = new Elegant.Ui.Button();
            this.button10 = new Elegant.Ui.Button();
            this.button12 = new Elegant.Ui.Button();
            this.reinstBtn = new Elegant.Ui.Button();
            this.openFoldBtn = new Elegant.Ui.Button();
            this.updtSvrBtn = new Elegant.Ui.Button();
            this.delBtn = new Elegant.Ui.Button();
            this.button16 = new Elegant.Ui.Button();
            this.groupBox3 = new Elegant.Ui.GroupBox();
            this.welcomeLbl = new Elegant.Ui.Label();
            this.welcomeDesclbl = new Elegant.Ui.Label();
            this.basicInstrLbl = new Elegant.Ui.Label();
            this.basicInstrTxtBox = new Elegant.Ui.TextBox();
            this.nonSteamServerTimer = new System.Windows.Forms.Timer(this.components);
            this.contextMenuExtenderProvider1 = new Elegant.Ui.ContextMenuExtenderProvider(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.slowTimer = new System.Windows.Forms.Timer(this.components);
            this.editServerSettigsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            themeSelector = new Elegant.Ui.ThemeSelector(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noInternetTab2)).BeginInit();
            this.noInternetTab2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonGroup6)).BeginInit();
            this.ribbonGroup6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainTabPage)).BeginInit();
            this.mainTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.generalSvrOptGroup)).BeginInit();
            this.generalSvrOptGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optionsRibbon)).BeginInit();
            this.optionsRibbon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentTaskRibGrp)).BeginInit();
            this.currentTaskRibGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.helpInfTab)).BeginInit();
            this.helpInfTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.helpInfGrpBox)).BeginInit();
            this.helpInfGrpBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonGroup4)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.steamSvrNavBar.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.serverPnl.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.extraGrpBox.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gameImgPicBox)).BeginInit();
            this.actionsGrpBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu4)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // SteamServertimer
            // 
            this.SteamServertimer.Interval = 2000;
            this.SteamServertimer.Tick += new System.EventHandler(this.steamGame_Tick);
            // 
            // generalTimer
            // 
            this.generalTimer.Interval = 1500;
            this.generalTimer.Tick += new System.EventHandler(this.generalTimer_Tick);
            // 
            // ribbon1
            // 
            this.ribbon1.AllowMinimizing = false;
            this.ribbon1.ApplicationButtonImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("ribbon1.ApplicationButtonImages.Images"))))});
            this.ribbon1.ApplicationButtonPopup = this.applicationMenu1;
            this.ribbon1.ApplicationButtonText = "Data";
            this.ribbon1.ApplicationButtonVisible = false;
            this.ribbon1.ContextualTabGroups.AddRange(new Elegant.Ui.RibbonContextualTabGroup[] {
            this.noInternetTab});
            this.ribbon1.CurrentTabPage = this.mainTabPage;
            this.ribbon1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ribbon1.Id = "58c83fc8-0d5e-49f9-8731-e7ffea83c55d";
            this.ribbon1.Location = new System.Drawing.Point(0, 0);
            this.ribbon1.MinimizeButtonVisible = false;
            this.ribbon1.Name = "ribbon1";
            this.ribbon1.QuickAccessToolbarDropDownVisible = false;
            this.ribbon1.QuickAccessToolbarPlacementMode = Elegant.Ui.QuickAccessToolbarPlacementMode.Hidden;
            this.ribbon1.Size = new System.Drawing.Size(629, 148);
            this.ribbon1.TabIndex = 22;
            this.ribbon1.TabPages.AddRange(new Elegant.Ui.RibbonTabPage[] {
            this.mainTabPage,
            this.helpInfTab});
            this.ribbon1.Text = "Server Creation Tool 3.1";
            // 
            // applicationMenu1
            // 
            this.applicationMenu1.ContentMinimumHeight = 0;
            this.applicationMenu1.ExitButtonCommandName = "Exit";
            this.applicationMenu1.KeepPopupsWithOffsetPlacementWithinPlacementArea = false;
            this.applicationMenu1.OptionsButtonImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("applicationMenu1.OptionsButtonImages.Images"))))});
            this.applicationMenu1.OptionsButtonVisible = false;
            this.applicationMenu1.PlacementMode = Elegant.Ui.PopupPlacementMode.Bottom;
            this.applicationMenu1.Size = new System.Drawing.Size(100, 100);
            // 
            // noInternetTab
            // 
            this.noInternetTab.Caption = "No Internet";
            this.noInternetTab.TabPages.AddRange(new Elegant.Ui.RibbonTabPage[] {
            this.noInternetTab2});
            // 
            // noInternetTab2
            // 
            this.noInternetTab2.Controls.Add(this.ribbonGroup6);
            this.noInternetTab2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noInternetTab2.KeyTip = null;
            this.noInternetTab2.Location = new System.Drawing.Point(0, 0);
            this.noInternetTab2.Name = "noInternetTab2";
            this.noInternetTab2.Size = new System.Drawing.Size(629, 101);
            this.noInternetTab2.TabIndex = 0;
            this.noInternetTab2.Text = "Error";
            this.noInternetTab2.Visible = false;
            // 
            // ribbonGroup6
            // 
            this.ribbonGroup6.Controls.Add(this.refreshConnBtn);
            this.ribbonGroup6.Controls.Add(this.noInternetTxtBox);
            this.ribbonGroup6.Location = new System.Drawing.Point(4, 3);
            this.ribbonGroup6.Name = "ribbonGroup6";
            this.ribbonGroup6.Size = new System.Drawing.Size(245, 0);
            this.ribbonGroup6.TabIndex = 0;
            this.ribbonGroup6.Text = "Actions";
            // 
            // refreshConnBtn
            // 
            this.refreshConnBtn.Id = "ac6c45ae-cca5-4252-a79f-8c400b84bc93";
            this.refreshConnBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.refresh)});
            this.refreshConnBtn.Location = new System.Drawing.Point(4, 2);
            this.refreshConnBtn.Name = "refreshConnBtn";
            this.refreshConnBtn.Size = new System.Drawing.Size(46, 0);
            this.refreshConnBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.refresh16x16)});
            this.refreshConnBtn.TabIndex = 1;
            this.refreshConnBtn.Text = "Refresh";
            this.refreshConnBtn.Click += new System.EventHandler(this.refreshConnBtn_Click);
            // 
            // noInternetTxtBox
            // 
            this.noInternetTxtBox.Id = "ff1caed2-a8ea-464f-ae63-5c05ca6c7c9f";
            this.noInternetTxtBox.Location = new System.Drawing.Point(52, 2);
            this.noInternetTxtBox.Multiline = true;
            this.noInternetTxtBox.Name = "noInternetTxtBox";
            this.noInternetTxtBox.Size = new System.Drawing.Size(191, 72);
            this.noInternetTxtBox.TabIndex = 2;
            this.noInternetTxtBox.Text = "There is no internet connection!\r\n----\r\nPlease check your internet access!\r\n";
            this.noInternetTxtBox.TextEditorWidth = 185;
            // 
            // mainTabPage
            // 
            this.mainTabPage.Controls.Add(this.generalSvrOptGroup);
            this.mainTabPage.Controls.Add(this.optionsRibbon);
            this.mainTabPage.Controls.Add(this.currentTaskRibGrp);
            this.mainTabPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabPage.KeyTip = null;
            this.mainTabPage.Location = new System.Drawing.Point(0, 0);
            this.mainTabPage.Name = "mainTabPage";
            this.mainTabPage.Size = new System.Drawing.Size(629, 101);
            this.mainTabPage.TabIndex = 0;
            this.mainTabPage.Text = "Main Menu";
            // 
            // generalSvrOptGroup
            // 
            this.generalSvrOptGroup.Controls.Add(this.openSrvsFoldBtn);
            this.generalSvrOptGroup.Controls.Add(this.selectInstFoldBtn);
            this.generalSvrOptGroup.Location = new System.Drawing.Point(4, 3);
            this.generalSvrOptGroup.Name = "generalSvrOptGroup";
            this.generalSvrOptGroup.Size = new System.Drawing.Size(179, 94);
            this.generalSvrOptGroup.TabIndex = 2;
            this.generalSvrOptGroup.Text = "General Server Options";
            // 
            // openSrvsFoldBtn
            // 
            this.openSrvsFoldBtn.Id = "13794610-c326-4089-b9c8-ea1436877188";
            this.openSrvsFoldBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.folderOpen)});
            this.openSrvsFoldBtn.Location = new System.Drawing.Point(4, 2);
            this.openSrvsFoldBtn.Name = "openSrvsFoldBtn";
            this.openSrvsFoldBtn.Size = new System.Drawing.Size(72, 72);
            this.openSrvsFoldBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.folder)});
            this.openSrvsFoldBtn.TabIndex = 1;
            this.openSrvsFoldBtn.Text = "Open servers folder";
            this.openSrvsFoldBtn.Click += new System.EventHandler(this.openServersFoldBtn_Click);
            // 
            // selectInstFoldBtn
            // 
            this.selectInstFoldBtn.Id = "88de8fc1-cc80-4148-ba64-42748bd7b632";
            this.selectInstFoldBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._2274)});
            this.selectInstFoldBtn.Location = new System.Drawing.Point(78, 2);
            this.selectInstFoldBtn.Name = "selectInstFoldBtn";
            this.selectInstFoldBtn.Size = new System.Drawing.Size(96, 72);
            this.selectInstFoldBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._2276)});
            this.selectInstFoldBtn.TabIndex = 0;
            this.selectInstFoldBtn.Text = "Select Server installation folder";
            this.selectInstFoldBtn.Click += new System.EventHandler(this.changeSrvrInstFoldBtn_Click);
            // 
            // optionsRibbon
            // 
            this.optionsRibbon.Controls.Add(this.chck4UpdatesBtn);
            this.optionsRibbon.Controls.Add(this.styleBtn);
            this.optionsRibbon.Controls.Add(this.langBtn);
            this.optionsRibbon.Location = new System.Drawing.Point(185, 3);
            this.optionsRibbon.Name = "optionsRibbon";
            this.optionsRibbon.Size = new System.Drawing.Size(176, 94);
            this.optionsRibbon.TabIndex = 4;
            this.optionsRibbon.Text = "Other Options";
            // 
            // chck4UpdatesBtn
            // 
            this.chck4UpdatesBtn.Id = "8f34342d-d968-40a3-a3da-c2785b5f62d3";
            this.chck4UpdatesBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.captcha)});
            this.chck4UpdatesBtn.Location = new System.Drawing.Point(4, 2);
            this.chck4UpdatesBtn.Name = "chck4UpdatesBtn";
            this.chck4UpdatesBtn.Size = new System.Drawing.Size(55, 72);
            this.chck4UpdatesBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.captcha)});
            this.chck4UpdatesBtn.TabIndex = 1;
            this.chck4UpdatesBtn.Text = "Check for updates";
            this.chck4UpdatesBtn.Click += new System.EventHandler(this.chck4UpdatesBtn_Click);
            // 
            // styleBtn
            // 
            this.styleBtn.Id = "c75eb6a7-cc16-4f3a-a8ba-3aa513ef3ee0";
            this.styleBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.desktopWithcolors)});
            this.styleBtn.Location = new System.Drawing.Point(61, 2);
            this.styleBtn.Name = "styleBtn";
            this.styleBtn.Popup = this.popupMenu2;
            this.styleBtn.Size = new System.Drawing.Size(42, 72);
            this.styleBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.desktopWithcolors)});
            this.styleBtn.TabIndex = 0;
            this.styleBtn.Text = "Style";
            // 
            // popupMenu2
            // 
            this.popupMenu2.Items.AddRange(new System.Windows.Forms.Control[] {
            this.button2,
            this.button11});
            this.popupMenu2.KeepPopupsWithOffsetPlacementWithinPlacementArea = false;
            this.popupMenu2.PlacementMode = Elegant.Ui.PopupPlacementMode.Bottom;
            this.popupMenu2.Size = new System.Drawing.Size(100, 100);
            // 
            // button2
            // 
            this.button2.Id = "24ef012d-6625-424a-851f-0b8f797cc486";
            this.button2.Location = new System.Drawing.Point(2, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(142, 35);
            this.button2.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.blue_theme)});
            this.button2.TabIndex = 3;
            this.button2.Text = "Blue";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button11
            // 
            this.button11.Id = "dfc04c7f-fed5-4b09-819f-f16d51894bc4";
            this.button11.Location = new System.Drawing.Point(2, 37);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(142, 35);
            this.button11.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.black_theme)});
            this.button11.TabIndex = 4;
            this.button11.Text = "Black";
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // langBtn
            // 
            this.langBtn.Id = "6cf0f22c-72cc-42a2-8271-25fc12804d62";
            this.langBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.translate__1_)});
            this.langBtn.Location = new System.Drawing.Point(105, 2);
            this.langBtn.Name = "langBtn";
            this.langBtn.Popup = this.popupMenu1;
            this.langBtn.Size = new System.Drawing.Size(66, 72);
            this.langBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.translate)});
            this.langBtn.TabIndex = 0;
            this.langBtn.Text = "Select Language";
            // 
            // popupMenu1
            // 
            this.popupMenu1.Items.AddRange(new System.Windows.Forms.Control[] {
            this.engBtn,
            this.gerBtn});
            this.popupMenu1.KeepPopupsWithOffsetPlacementWithinPlacementArea = false;
            this.popupMenu1.PlacementMode = Elegant.Ui.PopupPlacementMode.Bottom;
            this.popupMenu1.Size = new System.Drawing.Size(100, 100);
            // 
            // engBtn
            // 
            this.engBtn.Id = "11a748a4-49f9-4409-8865-9d3043cc81dc";
            this.engBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.icons8_great_britain_24)});
            this.engBtn.Location = new System.Drawing.Point(2, 2);
            this.engBtn.Name = "engBtn";
            this.engBtn.Size = new System.Drawing.Size(134, 27);
            this.engBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.icons8_great_britain_24)});
            this.engBtn.TabIndex = 3;
            this.engBtn.Text = "English";
            this.engBtn.Click += new System.EventHandler(this.engBtn_Click);
            // 
            // gerBtn
            // 
            this.gerBtn.Id = "e3a8d5e3-fce0-4a3f-af60-1c42d04a45e5";
            this.gerBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.icons8_germany_24)});
            this.gerBtn.Location = new System.Drawing.Point(2, 29);
            this.gerBtn.Name = "gerBtn";
            this.gerBtn.Size = new System.Drawing.Size(134, 27);
            this.gerBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.icons8_germany_24)});
            this.gerBtn.TabIndex = 4;
            this.gerBtn.Text = "German";
            this.gerBtn.Click += new System.EventHandler(this.gerBtn_Click_1);
            // 
            // currentTaskRibGrp
            // 
            this.currentTaskRibGrp.Controls.Add(this.currentTaskNameLbl);
            this.currentTaskRibGrp.Controls.Add(this.progressBar1);
            this.currentTaskRibGrp.Location = new System.Drawing.Point(363, 3);
            this.currentTaskRibGrp.Name = "currentTaskRibGrp";
            this.currentTaskRibGrp.Size = new System.Drawing.Size(194, 94);
            this.currentTaskRibGrp.TabIndex = 3;
            this.currentTaskRibGrp.Text = "Current Task";
            this.currentTaskRibGrp.Visible = false;
            // 
            // currentTaskNameLbl
            // 
            this.currentTaskNameLbl.Location = new System.Drawing.Point(3, 2);
            this.currentTaskNameLbl.Name = "currentTaskNameLbl";
            this.currentTaskNameLbl.Size = new System.Drawing.Size(63, 24);
            this.currentTaskNameLbl.TabIndex = 8;
            this.currentTaskNameLbl.Text = "TASK NAME";
            // 
            // progressBar1
            // 
            this.progressBar1.DesiredWidth = 185;
            this.progressBar1.Id = "77834854-e80c-45bc-bb6f-1cafcd361cc1";
            this.progressBar1.Location = new System.Drawing.Point(4, 26);
            this.progressBar1.MarqueeAnimationSpeed = 400;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(185, 12);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 7;
            this.progressBar1.Text = "progressBar1";
            this.progressBar1.Value = 50;
            // 
            // helpInfTab
            // 
            this.helpInfTab.Controls.Add(this.helpInfGrpBox);
            this.helpInfTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpInfTab.KeyTip = null;
            this.helpInfTab.Location = new System.Drawing.Point(0, 0);
            this.helpInfTab.Name = "helpInfTab";
            this.helpInfTab.Size = new System.Drawing.Size(629, 101);
            this.helpInfTab.TabIndex = 0;
            this.helpInfTab.Text = "Help-Information";
            // 
            // helpInfGrpBox
            // 
            this.helpInfGrpBox.Controls.Add(this.variousStuffBtn);
            this.helpInfGrpBox.Controls.Add(this.faqBtn);
            this.helpInfGrpBox.Controls.Add(this.serverGroupBtn);
            this.helpInfGrpBox.Controls.Add(this.separator1);
            this.helpInfGrpBox.Controls.Add(this.plannedFeaturesBtn);
            this.helpInfGrpBox.Controls.Add(this.appLogsbtn);
            this.helpInfGrpBox.Controls.Add(this.changeLogBtn);
            this.helpInfGrpBox.Controls.Add(this.donateBtn);
            this.helpInfGrpBox.Controls.Add(this.aboutBtn);
            this.helpInfGrpBox.Location = new System.Drawing.Point(4, 3);
            this.helpInfGrpBox.Name = "helpInfGrpBox";
            this.helpInfGrpBox.Size = new System.Drawing.Size(378, 94);
            this.helpInfGrpBox.TabIndex = 0;
            this.helpInfGrpBox.Text = "Help-Information";
            // 
            // variousStuffBtn
            // 
            this.variousStuffBtn.Id = "0ef63bb6-3792-4208-aaef-0925b97c8eb5";
            this.variousStuffBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.settings__4_)});
            this.variousStuffBtn.Location = new System.Drawing.Point(142, 2);
            this.variousStuffBtn.Name = "variousStuffBtn";
            this.variousStuffBtn.Popup = this.popupMenu3;
            this.variousStuffBtn.Size = new System.Drawing.Size(77, 0);
            this.variousStuffBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.settings__4_)});
            this.variousStuffBtn.TabIndex = 7;
            this.variousStuffBtn.Text = "Various Stuff";
            // 
            // popupMenu3
            // 
            this.popupMenu3.Items.AddRange(new System.Windows.Forms.Control[] {
            this.portFwRouterBtn});
            this.popupMenu3.KeepPopupsWithOffsetPlacementWithinPlacementArea = false;
            this.popupMenu3.PlacementMode = Elegant.Ui.PopupPlacementMode.Bottom;
            this.popupMenu3.Size = new System.Drawing.Size(100, 100);
            // 
            // portFwRouterBtn
            // 
            this.portFwRouterBtn.Id = "4325dcb7-bc3a-4235-a11d-ccb2b560f3b3";
            this.portFwRouterBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.port)});
            this.portFwRouterBtn.Location = new System.Drawing.Point(2, 2);
            this.portFwRouterBtn.Name = "portFwRouterBtn";
            this.portFwRouterBtn.Size = new System.Drawing.Size(233, 23);
            this.portFwRouterBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.Icon_20__2_)});
            this.portFwRouterBtn.TabIndex = 3;
            this.portFwRouterBtn.Text = "How to port forward your router";
            this.portFwRouterBtn.Click += new System.EventHandler(this.button34_Click_1);
            // 
            // faqBtn
            // 
            this.faqBtn.Id = "28ce35f1-ceb3-4204-bc35-0b7fbd611ab5";
            this.faqBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._1502)});
            this.faqBtn.Location = new System.Drawing.Point(142, 2);
            this.faqBtn.Name = "faqBtn";
            this.faqBtn.Size = new System.Drawing.Size(30, 0);
            this.faqBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._1502)});
            this.faqBtn.TabIndex = 2;
            this.faqBtn.Text = "FAQ";
            this.faqBtn.Click += new System.EventHandler(this.button5_Click);
            // 
            // serverGroupBtn
            // 
            this.serverGroupBtn.Id = "e14cf0e8-1259-4e46-a333-1d53fd6cd24d";
            this.serverGroupBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.chat_group)});
            this.serverGroupBtn.Location = new System.Drawing.Point(142, 2);
            this.serverGroupBtn.Name = "serverGroupBtn";
            this.serverGroupBtn.Size = new System.Drawing.Size(62, 0);
            this.serverGroupBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.chat_group)});
            this.serverGroupBtn.TabIndex = 1;
            this.serverGroupBtn.Text = "Tool Group";
            this.serverGroupBtn.Click += new System.EventHandler(this.serverGroupBtn_Click_1);
            // 
            // separator1
            // 
            this.separator1.Id = "ad8d34a0-5126-4a2d-9d97-db4c8517ccc4";
            this.separator1.Location = new System.Drawing.Point(144, 7);
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(2, 60);
            this.separator1.TabIndex = 3;
            this.separator1.Text = "separator1";
            // 
            // plannedFeaturesBtn
            // 
            this.plannedFeaturesBtn.Id = "3cc6cadf-f3da-445c-8337-012a84a808e3";
            this.plannedFeaturesBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.pencil)});
            this.plannedFeaturesBtn.Location = new System.Drawing.Point(142, 74);
            this.plannedFeaturesBtn.Name = "plannedFeaturesBtn";
            this.plannedFeaturesBtn.Size = new System.Drawing.Size(92, 0);
            this.plannedFeaturesBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.pencil)});
            this.plannedFeaturesBtn.TabIndex = 4;
            this.plannedFeaturesBtn.Text = "Planned Features";
            this.plannedFeaturesBtn.Click += new System.EventHandler(this.plannedFeaturesBtn_Click);
            // 
            // appLogsbtn
            // 
            this.appLogsbtn.Id = "68d1a90c-9eea-4b6b-9da0-e41db427e89b";
            this.appLogsbtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._622)});
            this.appLogsbtn.Location = new System.Drawing.Point(142, 74);
            this.appLogsbtn.Name = "appLogsbtn";
            this.appLogsbtn.Size = new System.Drawing.Size(54, 0);
            this.appLogsbtn.TabIndex = 9;
            this.appLogsbtn.Text = "App Logs";
            this.appLogsbtn.Click += new System.EventHandler(this.button9_Click_1);
            // 
            // changeLogBtn
            // 
            this.changeLogBtn.Id = "3139bf5f-b471-47eb-ac8b-4b722eab6b98";
            this.changeLogBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.CHECK_BOARD_48_48)});
            this.changeLogBtn.Location = new System.Drawing.Point(142, 74);
            this.changeLogBtn.Name = "changeLogBtn";
            this.changeLogBtn.Size = new System.Drawing.Size(67, 0);
            this.changeLogBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.CHECK_BOARD)});
            this.changeLogBtn.TabIndex = 0;
            this.changeLogBtn.Text = "Change Log";
            this.changeLogBtn.Click += new System.EventHandler(this.changeLogBtn_Click_1);
            // 
            // donateBtn
            // 
            this.donateBtn.Id = "8308b23c-1734-40ae-aa36-345dadc5cda6";
            this.donateBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("donateBtn.LargeImages.Images"))))});
            this.donateBtn.Location = new System.Drawing.Point(142, 74);
            this.donateBtn.Name = "donateBtn";
            this.donateBtn.Size = new System.Drawing.Size(44, 0);
            this.donateBtn.TabIndex = 8;
            this.donateBtn.Text = "Donate";
            this.donateBtn.Click += new System.EventHandler(this.donateBtn_Click);
            // 
            // aboutBtn
            // 
            this.aboutBtn.Id = "58c1dfcf-a1c1-4cc3-bc6c-b5cbac8b4759";
            this.aboutBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._2191)});
            this.aboutBtn.Location = new System.Drawing.Point(142, 74);
            this.aboutBtn.Name = "aboutBtn";
            this.aboutBtn.Size = new System.Drawing.Size(37, 0);
            this.aboutBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._2191)});
            this.aboutBtn.TabIndex = 5;
            this.aboutBtn.Text = "About";
            this.aboutBtn.Click += new System.EventHandler(this.button6_Click);
            // 
            // formFrameSkinner
            // 
            this.formFrameSkinner.AllowGlass = false;
            this.formFrameSkinner.Form = this;
            // 
            // ribbonContextualTabGroup2
            // 
            this.ribbonContextualTabGroup2.Caption = null;
            this.ribbonContextualTabGroup2.Color = Elegant.Ui.RibbonContextualTabGroupColor.Cyan;
            // 
            // ribbonGroup4
            // 
            this.ribbonGroup4.Location = new System.Drawing.Point(0, 0);
            this.ribbonGroup4.Name = "ribbonGroup4";
            this.ribbonGroup4.Size = new System.Drawing.Size(535, 94);
            this.ribbonGroup4.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.steamGameSrchBox);
            this.groupBox2.Controls.Add(this.searchGameLbl);
            this.groupBox2.Controls.Add(this.tabControl1);
            this.groupBox2.Id = "80e3828c-b5ac-4319-ad5a-7ade73bcb6ed";
            this.groupBox2.Location = new System.Drawing.Point(1, 148);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(267, 257);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.Text = "Select a server";
            // 
            // steamGameSrchBox
            // 
            this.steamGameSrchBox.Id = "fda031e4-5f0a-4bc7-bd87-2f91e76efced";
            this.steamGameSrchBox.Location = new System.Drawing.Point(149, 10);
            this.steamGameSrchBox.Name = "steamGameSrchBox";
            this.steamGameSrchBox.Size = new System.Drawing.Size(110, 21);
            this.steamGameSrchBox.TabIndex = 2;
            this.steamGameSrchBox.TextEditorWidth = 104;
            this.steamGameSrchBox.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // searchGameLbl
            // 
            this.searchGameLbl.Location = new System.Drawing.Point(80, 14);
            this.searchGameLbl.Name = "searchGameLbl";
            this.searchGameLbl.Size = new System.Drawing.Size(68, 13);
            this.searchGameLbl.TabIndex = 0;
            this.searchGameLbl.Text = "Search game:";
            // 
            // tabControl1
            // 
            this.tabControl1.Location = new System.Drawing.Point(2, 32);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedTabPage = this.tabPage1;
            this.tabControl1.Size = new System.Drawing.Size(262, 224);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.TabPages.AddRange(new Elegant.Ui.TabPage[] {
            this.tabPage1,
            this.tabPage2});
            this.tabControl1.Text = "+";
            this.tabControl1.SelectedTabPageChanged += new Elegant.Ui.TabPageChangedEventHandler(this.tabControl1_SelectedTabPageChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.ActiveControl = null;
            this.tabPage1.Controls.Add(this.steamSvrNavBar);
            this.tabPage1.Controls.Add(this.separator3);
            this.tabPage1.KeyTip = null;
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(260, 203);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Steam Servers";
            // 
            // steamSvrNavBar
            // 
            this.steamSvrNavBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.steamSvrNavBar.Controls.Add(this.welcomeBtn);
            this.steamSvrNavBar.Controls.Add(this.daysToDieBtn);
            this.steamSvrNavBar.Controls.Add(this.arkBtn);
            this.steamSvrNavBar.Controls.Add(this.atlasBtn);
            this.steamSvrNavBar.Controls.Add(this.CodBo3Btn);
            this.steamSvrNavBar.Controls.Add(this.cs16Btn);
            this.steamSvrNavBar.Controls.Add(this.CSsourceBtn);
            this.steamSvrNavBar.Controls.Add(this.CSGOBtn);
            this.steamSvrNavBar.Controls.Add(this.garrysModBtn);
            this.steamSvrNavBar.Controls.Add(this.hurtWorldBtn);
            this.steamSvrNavBar.Controls.Add(this.KillingFloorTwoBtn);
            this.steamSvrNavBar.Controls.Add(this.l4dBtn);
            this.steamSvrNavBar.Controls.Add(this.l4d2Btn);
            this.steamSvrNavBar.Controls.Add(this.RustBtn);
            this.steamSvrNavBar.Controls.Add(this.SvenCoopBtn);
            this.steamSvrNavBar.Controls.Add(this.synergyBtn);
            this.steamSvrNavBar.Controls.Add(this.unturnedBtn);
            this.steamSvrNavBar.Id = "55a8b331-7488-48be-a11d-5a0c33070b68";
            this.steamSvrNavBar.Location = new System.Drawing.Point(2, 0);
            this.steamSvrNavBar.Name = "steamSvrNavBar";
            this.steamSvrNavBar.Size = new System.Drawing.Size(258, 201);
            this.steamSvrNavBar.TabIndex = 0;
            this.steamSvrNavBar.UseTabToNavigate = false;
            this.steamSvrNavBar.WrapNavigation = false;
            // 
            // welcomeBtn
            // 
            this.welcomeBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.welcomeBtn.Id = "ac9a18cf-7a87-462e-b7ef-1a0602d79d03";
            this.welcomeBtn.Location = new System.Drawing.Point(2, 2);
            this.welcomeBtn.Name = "welcomeBtn";
            this.welcomeBtn.Pressed = true;
            this.welcomeBtn.RadioGroupName = "NavigationBarToggleButtons";
            this.welcomeBtn.Size = new System.Drawing.Size(237, 29);
            this.welcomeBtn.TabIndex = 0;
            this.welcomeBtn.Tag = "noTag";
            this.welcomeBtn.Text = "-----------------Welcome-----------------";
            this.welcomeBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.welcomeBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.welcomeBtn.Click += new System.EventHandler(this.welcomeBtn_Click);
            // 
            // daysToDieBtn
            // 
            this.daysToDieBtn.CommandName = "";
            this.daysToDieBtn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.daysToDieBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.daysToDieBtn.Id = "100755d3-fc2d-40da-a4cf-55a9e0220f45";
            this.daysToDieBtn.Location = new System.Drawing.Point(2, 33);
            this.daysToDieBtn.Name = "daysToDieBtn";
            this.daysToDieBtn.RadioGroupName = "NavigationBarToggleButtons";
            this.daysToDieBtn.Size = new System.Drawing.Size(237, 25);
            this.daysToDieBtn.TabIndex = 1;
            this.daysToDieBtn.Tag = "7days";
            this.daysToDieBtn.Text = "7 Days to Die";
            this.daysToDieBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.daysToDieBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.daysToDieBtn.Click += new System.EventHandler(this.daysToDieBtn_Click_1);
            // 
            // arkBtn
            // 
            this.arkBtn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.arkBtn.Id = "985f46f7-389d-422b-8a64-270f82350580";
            this.arkBtn.Location = new System.Drawing.Point(2, 60);
            this.arkBtn.Name = "arkBtn";
            this.arkBtn.RadioGroupName = "NavigationBarToggleButtons";
            this.arkBtn.Size = new System.Drawing.Size(237, 25);
            this.arkBtn.TabIndex = 2;
            this.arkBtn.Tag = "ark";
            this.arkBtn.Text = "ARK: Survival Evolved";
            this.arkBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.arkBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.arkBtn.Click += new System.EventHandler(this.arkBtn_Click);
            // 
            // atlasBtn
            // 
            this.atlasBtn.Id = "0cdbc559-2c0a-4560-ac22-8a73bd108b54";
            this.atlasBtn.Location = new System.Drawing.Point(2, 87);
            this.atlasBtn.Name = "atlasBtn";
            this.atlasBtn.RadioGroupName = "NavigationBarToggleButtons";
            this.atlasBtn.Size = new System.Drawing.Size(237, 25);
            this.atlasBtn.TabIndex = 16;
            this.atlasBtn.Tag = ".emtpyF";
            this.atlasBtn.Text = "ATLAS";
            this.atlasBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.atlasBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.atlasBtn.Click += new System.EventHandler(this.atlasBtn_Click);
            // 
            // CodBo3Btn
            // 
            this.CodBo3Btn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.CodBo3Btn.Id = "495d81f0-1c92-46d9-b644-24a8715126fd";
            this.CodBo3Btn.Location = new System.Drawing.Point(2, 114);
            this.CodBo3Btn.Name = "CodBo3Btn";
            this.CodBo3Btn.RadioGroupName = "NavigationBarToggleButtons";
            this.CodBo3Btn.Size = new System.Drawing.Size(237, 25);
            this.CodBo3Btn.TabIndex = 3;
            this.CodBo3Btn.Tag = "bo3";
            this.CodBo3Btn.Text = "Call of Duty: Black Ops 3";
            this.CodBo3Btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CodBo3Btn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.CodBo3Btn.Click += new System.EventHandler(this.CODblackOpsThreeBtn_Click);
            // 
            // cs16Btn
            // 
            this.cs16Btn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.cs16Btn.Id = "188c972f-c7b0-4dce-be2e-9a52846c6685";
            this.cs16Btn.Location = new System.Drawing.Point(2, 141);
            this.cs16Btn.Name = "cs16Btn";
            this.cs16Btn.RadioGroupName = "NavigationBarToggleButtons";
            this.cs16Btn.Size = new System.Drawing.Size(237, 25);
            this.cs16Btn.TabIndex = 4;
            this.cs16Btn.Tag = "cs";
            this.cs16Btn.Text = "Counter-Strike: 1.6";
            this.cs16Btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cs16Btn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cs16Btn.Click += new System.EventHandler(this.csOnePointsixBtn_Click);
            // 
            // CSsourceBtn
            // 
            this.CSsourceBtn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.CSsourceBtn.Id = "b8aea746-2381-4346-8c63-3282c6e7ebd7";
            this.CSsourceBtn.Location = new System.Drawing.Point(2, 168);
            this.CSsourceBtn.Name = "CSsourceBtn";
            this.CSsourceBtn.RadioGroupName = "NavigationBarToggleButtons";
            this.CSsourceBtn.Size = new System.Drawing.Size(237, 25);
            this.CSsourceBtn.TabIndex = 5;
            this.CSsourceBtn.Tag = "css";
            this.CSsourceBtn.Text = "Counter-Strike: Source";
            this.CSsourceBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CSsourceBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.CSsourceBtn.Click += new System.EventHandler(this.CSsourceBtn_Click);
            // 
            // CSGOBtn
            // 
            this.CSGOBtn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.CSGOBtn.Id = "21ae176b-4478-487c-bd84-fa9bcbac13cf";
            this.CSGOBtn.Location = new System.Drawing.Point(2, 195);
            this.CSGOBtn.Name = "CSGOBtn";
            this.CSGOBtn.RadioGroupName = "NavigationBarToggleButtons";
            this.CSGOBtn.Size = new System.Drawing.Size(237, 25);
            this.CSGOBtn.TabIndex = 6;
            this.CSGOBtn.Tag = "csgo";
            this.CSGOBtn.Text = "Counter-Strike: Global Offensive";
            this.CSGOBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CSGOBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.CSGOBtn.Click += new System.EventHandler(this.CSGOBtn_Click);
            // 
            // garrysModBtn
            // 
            this.garrysModBtn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.garrysModBtn.Id = "d2409877-e2f4-4060-9ee7-46a2b188318e";
            this.garrysModBtn.Location = new System.Drawing.Point(2, 222);
            this.garrysModBtn.Name = "garrysModBtn";
            this.garrysModBtn.RadioGroupName = "NavigationBarToggleButtons";
            this.garrysModBtn.Size = new System.Drawing.Size(237, 25);
            this.garrysModBtn.TabIndex = 7;
            this.garrysModBtn.Tag = "gmod";
            this.garrysModBtn.Text = "Garry\'s Mod";
            this.garrysModBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.garrysModBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.garrysModBtn.Click += new System.EventHandler(this.garrysModBtn_Click);
            // 
            // hurtWorldBtn
            // 
            this.hurtWorldBtn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.hurtWorldBtn.Id = "f78ab720-2347-4165-a6da-49a5300393da";
            this.hurtWorldBtn.Location = new System.Drawing.Point(2, 249);
            this.hurtWorldBtn.Name = "hurtWorldBtn";
            this.hurtWorldBtn.RadioGroupName = "NavigationBarToggleButtons";
            this.hurtWorldBtn.Size = new System.Drawing.Size(237, 25);
            this.hurtWorldBtn.TabIndex = 8;
            this.hurtWorldBtn.Tag = "hurtworld";
            this.hurtWorldBtn.Text = "Hurtworld";
            this.hurtWorldBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.hurtWorldBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.hurtWorldBtn.Click += new System.EventHandler(this.hurtWorldBtn_Click);
            // 
            // KillingFloorTwoBtn
            // 
            this.KillingFloorTwoBtn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.KillingFloorTwoBtn.Id = "65aa8032-940f-4ae8-83a4-b4e2380febc4";
            this.KillingFloorTwoBtn.Location = new System.Drawing.Point(2, 276);
            this.KillingFloorTwoBtn.Name = "KillingFloorTwoBtn";
            this.KillingFloorTwoBtn.RadioGroupName = "NavigationBarToggleButtons";
            this.KillingFloorTwoBtn.Size = new System.Drawing.Size(237, 25);
            this.KillingFloorTwoBtn.TabIndex = 9;
            this.KillingFloorTwoBtn.Tag = "kf2";
            this.KillingFloorTwoBtn.Text = "Killing Floor 2";
            this.KillingFloorTwoBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.KillingFloorTwoBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.KillingFloorTwoBtn.Click += new System.EventHandler(this.KillingFloorTwoBtn_Click);
            // 
            // l4dBtn
            // 
            this.l4dBtn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.l4dBtn.Id = "e271bded-110c-4fa9-8910-6363fdb7099c";
            this.l4dBtn.Location = new System.Drawing.Point(2, 303);
            this.l4dBtn.Name = "l4dBtn";
            this.l4dBtn.RadioGroupName = "NavigationBarToggleButtons";
            this.l4dBtn.Size = new System.Drawing.Size(237, 25);
            this.l4dBtn.TabIndex = 10;
            this.l4dBtn.Tag = "l4d";
            this.l4dBtn.Text = "Left 4 Dead";
            this.l4dBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.l4dBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.l4dBtn.Click += new System.EventHandler(this.l4dBtn_Click);
            // 
            // l4d2Btn
            // 
            this.l4d2Btn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.l4d2Btn.Id = "3a27024b-d3c7-4aa6-bb6c-1fbf7a7602c2";
            this.l4d2Btn.Location = new System.Drawing.Point(2, 330);
            this.l4d2Btn.Name = "l4d2Btn";
            this.l4d2Btn.RadioGroupName = "NavigationBarToggleButtons";
            this.l4d2Btn.Size = new System.Drawing.Size(237, 25);
            this.l4d2Btn.TabIndex = 11;
            this.l4d2Btn.Tag = "l4d2";
            this.l4d2Btn.Text = "Left 4 Dead 2";
            this.l4d2Btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.l4d2Btn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.l4d2Btn.Click += new System.EventHandler(this.l4d2Btn_Click);
            // 
            // RustBtn
            // 
            this.RustBtn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.RustBtn.Id = "18372002-66c5-4a33-9b2b-6dca479d0681";
            this.RustBtn.Location = new System.Drawing.Point(2, 357);
            this.RustBtn.Name = "RustBtn";
            this.RustBtn.RadioGroupName = "NavigationBarToggleButtons";
            this.RustBtn.Size = new System.Drawing.Size(237, 25);
            this.RustBtn.TabIndex = 12;
            this.RustBtn.Tag = "rust";
            this.RustBtn.Text = "Rust";
            this.RustBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RustBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.RustBtn.Click += new System.EventHandler(this.RustBtn_Click);
            // 
            // SvenCoopBtn
            // 
            this.SvenCoopBtn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.SvenCoopBtn.Id = "ea65df30-78cf-4836-9eec-1a04e67ca347";
            this.SvenCoopBtn.Location = new System.Drawing.Point(2, 384);
            this.SvenCoopBtn.Name = "SvenCoopBtn";
            this.SvenCoopBtn.RadioGroupName = "NavigationBarToggleButtons";
            this.SvenCoopBtn.Size = new System.Drawing.Size(237, 25);
            this.SvenCoopBtn.TabIndex = 13;
            this.SvenCoopBtn.Tag = "sven";
            this.SvenCoopBtn.Text = "Sven Coop";
            this.SvenCoopBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SvenCoopBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.SvenCoopBtn.Click += new System.EventHandler(this.SvenCoopBtn_Click);
            // 
            // synergyBtn
            // 
            this.synergyBtn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.synergyBtn.Id = "9b42c672-1196-46c8-93ec-446b1663e144";
            this.synergyBtn.Location = new System.Drawing.Point(2, 411);
            this.synergyBtn.Name = "synergyBtn";
            this.synergyBtn.RadioGroupName = "NavigationBarToggleButtons";
            this.synergyBtn.Size = new System.Drawing.Size(237, 25);
            this.synergyBtn.TabIndex = 14;
            this.synergyBtn.Tag = "noTag";
            this.synergyBtn.Text = "Synergy";
            this.synergyBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.synergyBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.synergyBtn.Visible = false;
            // 
            // unturnedBtn
            // 
            this.unturnedBtn.Id = "62113d56-149c-476d-8b9f-627d881c9b1a";
            this.unturnedBtn.Location = new System.Drawing.Point(2, 438);
            this.unturnedBtn.Name = "unturnedBtn";
            this.unturnedBtn.RadioGroupName = "NavigationBarToggleButtons";
            this.unturnedBtn.Size = new System.Drawing.Size(237, 25);
            this.unturnedBtn.TabIndex = 15;
            this.unturnedBtn.Tag = "unturned";
            this.unturnedBtn.Text = "Unturned";
            this.unturnedBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.unturnedBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.unturnedBtn.Click += new System.EventHandler(this.unturnedBtn_Click);
            // 
            // separator3
            // 
            this.separator3.Id = "b32e81c0-788e-4a08-b9a0-e03684a34f0a";
            this.separator3.Location = new System.Drawing.Point(8, -4);
            this.separator3.Name = "separator3";
            this.separator3.Orientation = Elegant.Ui.SeparatorOrientation.Horizontal;
            this.separator3.Size = new System.Drawing.Size(233, 5);
            this.separator3.TabIndex = 14;
            // 
            // tabPage2
            // 
            this.tabPage2.ActiveControl = null;
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.KeyTip = null;
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(260, 203);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Other Servers";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(66, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "COMING SOON";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.textBox1.Id = "7e648fb2-3990-4c7e-a4e6-1c25a7ba960b";
            this.textBox1.Location = new System.Drawing.Point(4, 2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(362, 72);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            this.textBox1.TextEditorWidth = 356;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Id = "d6dad4e3-7a00-41b0-b9e7-59c6265d138d";
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.button7);
            this.panel2.Controls.Add(this.button23);
            this.panel2.Controls.Add(this.button25);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 100);
            this.panel2.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Id = "40b313b4-2106-4cb6-bbbc-ec505217300f";
            this.button1.Location = new System.Drawing.Point(205, 69);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(193, 21);
            this.button1.TabIndex = 40;
            this.button1.Text = "Rust";
            this.button1.WordWrap = true;
            // 
            // button4
            // 
            this.button4.Id = "c06ab29a-06fb-4b49-8d37-a99e1ae68a95";
            this.button4.Location = new System.Drawing.Point(205, 91);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(193, 21);
            this.button4.TabIndex = 39;
            this.button4.Text = "Synergy";
            this.button4.WordWrap = true;
            // 
            // button7
            // 
            this.button7.Id = "d85e1aef-00e1-4ea4-9536-1885611b32e7";
            this.button7.Location = new System.Drawing.Point(205, 3);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(193, 21);
            this.button7.TabIndex = 37;
            this.button7.Text = "Hurtworld";
            this.button7.WordWrap = true;
            // 
            // button23
            // 
            this.button23.Id = "863c6f6a-ff70-4335-82aa-4e1f413b947a";
            this.button23.Location = new System.Drawing.Point(205, 25);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(193, 21);
            this.button23.TabIndex = 36;
            this.button23.Text = "Garry\'s Mod";
            this.button23.WordWrap = true;
            // 
            // button25
            // 
            this.button25.Id = "7251a2ee-29b4-459d-acc2-107751cc1729";
            this.button25.Location = new System.Drawing.Point(205, 47);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(193, 21);
            this.button25.TabIndex = 35;
            this.button25.Text = "Counter-Strike 1.6";
            this.button25.WordWrap = true;
            // 
            // button3
            // 
            this.button3.Id = "ed1a9fcc-5ecf-44ee-b9ce-ecf5298e78a4";
            this.button3.Location = new System.Drawing.Point(0, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 0;
            // 
            // button6
            // 
            this.button6.Id = "6ba33833-dcc1-47b5-954a-014ce156e781";
            this.button6.Location = new System.Drawing.Point(0, 0);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 0;
            // 
            // button21
            // 
            this.button21.Id = "e2cd65fd-0db7-452a-968b-d857c1e76545";
            this.button21.Location = new System.Drawing.Point(0, 0);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(75, 23);
            this.button21.TabIndex = 0;
            // 
            // button24
            // 
            this.button24.Id = "8c38544a-6d15-4064-97e9-aeaaa3710796";
            this.button24.Location = new System.Drawing.Point(0, 0);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(75, 23);
            this.button24.TabIndex = 0;
            // 
            // button26
            // 
            this.button26.Id = "8af91525-21c4-4e2e-a081-9e47e3aefc99";
            this.button26.Location = new System.Drawing.Point(0, 0);
            this.button26.Name = "button26";
            this.button26.Size = new System.Drawing.Size(75, 23);
            this.button26.TabIndex = 0;
            // 
            // button27
            // 
            this.button27.Id = "b3858852-c9dc-454e-9619-afd67f8452c0";
            this.button27.Location = new System.Drawing.Point(0, 0);
            this.button27.Name = "button27";
            this.button27.Size = new System.Drawing.Size(75, 23);
            this.button27.TabIndex = 0;
            // 
            // button28
            // 
            this.button28.Id = "ac690cbc-e559-41c8-8854-874ee3b460a8";
            this.button28.Location = new System.Drawing.Point(0, 0);
            this.button28.Name = "button28";
            this.button28.Size = new System.Drawing.Size(75, 23);
            this.button28.TabIndex = 0;
            // 
            // button29
            // 
            this.button29.Id = "ba73769b-f61c-4f56-be0f-329b3a163818";
            this.button29.Location = new System.Drawing.Point(0, 0);
            this.button29.Name = "button29";
            this.button29.Size = new System.Drawing.Size(75, 23);
            this.button29.TabIndex = 0;
            // 
            // button30
            // 
            this.button30.Id = "2dc5ab87-2698-4c36-beae-447c8ee6d1fd";
            this.button30.Location = new System.Drawing.Point(0, 0);
            this.button30.Name = "button30";
            this.button30.Size = new System.Drawing.Size(75, 23);
            this.button30.TabIndex = 0;
            // 
            // separator2
            // 
            this.separator2.Id = "723a6092-5b0a-49ce-9637-59136bd47f44";
            this.separator2.Location = new System.Drawing.Point(0, 0);
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(100, 23);
            this.separator2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.serverPnl);
            this.panel1.Controls.Add(this.button16);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.basicInstrLbl);
            this.panel1.Controls.Add(this.basicInstrTxtBox);
            this.panel1.Location = new System.Drawing.Point(267, 148);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(362, 257);
            this.panel1.TabIndex = 26;
            this.panel1.Text = "panel1";
            // 
            // serverPnl
            // 
            this.serverPnl.Controls.Add(this.groupBox7);
            this.serverPnl.Controls.Add(this.extraGrpBox);
            this.serverPnl.Controls.Add(this.groupBox5);
            this.serverPnl.Controls.Add(this.gameImgPicBox);
            this.serverPnl.Controls.Add(this.actionsGrpBox);
            this.serverPnl.Id = "9c35aab6-3195-4bff-99b9-00c9be860aec";
            this.serverPnl.Location = new System.Drawing.Point(186, 220);
            this.serverPnl.Name = "serverPnl";
            this.serverPnl.Size = new System.Drawing.Size(360, 257);
            this.serverPnl.TabIndex = 25;
            this.serverPnl.Text = "SERVER_NAME";
            this.serverPnl.Visible = false;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.srvSizeLbl);
            this.groupBox7.Controls.Add(this.srvSizeTxtBox);
            this.groupBox7.Id = "01b13104-461f-4676-9eaf-ad88d6dac816";
            this.groupBox7.Location = new System.Drawing.Point(130, 123);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(104, 68);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.Text = "Server Info";
            // 
            // srvSizeLbl
            // 
            this.srvSizeLbl.Location = new System.Drawing.Point(40, 17);
            this.srvSizeLbl.Name = "srvSizeLbl";
            this.srvSizeLbl.Size = new System.Drawing.Size(26, 15);
            this.srvSizeLbl.TabIndex = 0;
            this.srvSizeLbl.Text = "Size:";
            // 
            // srvSizeTxtBox
            // 
            this.srvSizeTxtBox.Id = "9eff4d81-0c27-43f3-b2a8-08b20386e24f";
            this.srvSizeTxtBox.Location = new System.Drawing.Point(14, 32);
            this.srvSizeTxtBox.Name = "srvSizeTxtBox";
            this.srvSizeTxtBox.ReadOnly = true;
            this.srvSizeTxtBox.Size = new System.Drawing.Size(80, 21);
            this.srvSizeTxtBox.TabIndex = 1;
            this.srvSizeTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.srvSizeTxtBox.TextEditorWidth = 74;
            this.srvSizeTxtBox.UseVisualThemeForForeground = false;
            // 
            // extraGrpBox
            // 
            this.extraGrpBox.Controls.Add(this.extraBtn4);
            this.extraGrpBox.Controls.Add(this.extraBtn3);
            this.extraGrpBox.Controls.Add(this.extraBtn2);
            this.extraGrpBox.Controls.Add(this.extraBtn1);
            this.extraGrpBox.Controls.Add(this.instGuideBtn);
            this.extraGrpBox.Id = "756d47e8-ee3a-4ea7-8ded-1eb8fccdcf0d";
            this.extraGrpBox.Location = new System.Drawing.Point(3, 123);
            this.extraGrpBox.Name = "extraGrpBox";
            this.extraGrpBox.Size = new System.Drawing.Size(126, 134);
            this.extraGrpBox.TabIndex = 5;
            this.extraGrpBox.Text = "Extra";
            // 
            // extraBtn4
            // 
            this.extraBtn4.CommandName = "extraBtn4Cmd";
            this.extraBtn4.Id = "85d15f2b-84d5-4eb5-81b5-6b6f82f2c0e3";
            this.extraBtn4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extraBtn4.Location = new System.Drawing.Point(3, 109);
            this.extraBtn4.Name = "extraBtn4";
            this.extraBtn4.Size = new System.Drawing.Size(120, 23);
            this.extraBtn4.TabIndex = 7;
            this.extraBtn4.Text = "extraBtn4";
            // 
            // extraBtn3
            // 
            this.extraBtn3.CommandName = "extraBtn3Cmd";
            this.extraBtn3.Id = "b74b4b34-0818-46af-aa87-3d50c66be743";
            this.extraBtn3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extraBtn3.Location = new System.Drawing.Point(3, 85);
            this.extraBtn3.Name = "extraBtn3";
            this.extraBtn3.Size = new System.Drawing.Size(120, 23);
            this.extraBtn3.TabIndex = 6;
            this.extraBtn3.Text = "extraBtn3";
            // 
            // extraBtn2
            // 
            this.extraBtn2.CommandName = "extraBtn2Cmd";
            this.extraBtn2.Id = "88c218d6-99ef-4c24-9510-31b2ce5a5b9c";
            this.extraBtn2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extraBtn2.Location = new System.Drawing.Point(3, 61);
            this.extraBtn2.Name = "extraBtn2";
            this.extraBtn2.Size = new System.Drawing.Size(120, 23);
            this.extraBtn2.TabIndex = 5;
            this.extraBtn2.Text = "extraBtn2";
            // 
            // extraBtn1
            // 
            this.extraBtn1.CommandName = "extraBtn1Cmd";
            this.extraBtn1.Id = "ba58be1a-58ab-4bc9-922e-bd59999f999b";
            this.extraBtn1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extraBtn1.Location = new System.Drawing.Point(3, 37);
            this.extraBtn1.Name = "extraBtn1";
            this.extraBtn1.Size = new System.Drawing.Size(120, 23);
            this.extraBtn1.TabIndex = 4;
            this.extraBtn1.Text = "Premade server.cfg file";
            // 
            // instGuideBtn
            // 
            this.instGuideBtn.CommandName = "";
            this.instGuideBtn.Id = "ef8a40e2-27c2-485b-840e-84c59bb0043d";
            this.instGuideBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.instGuideBtn.Location = new System.Drawing.Point(3, 13);
            this.instGuideBtn.Name = "instGuideBtn";
            this.instGuideBtn.Size = new System.Drawing.Size(120, 23);
            this.instGuideBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._1091)});
            this.instGuideBtn.TabIndex = 2;
            this.instGuideBtn.Tag = "guide";
            this.instGuideBtn.Text = "   Installation Guide";
            this.instGuideBtn.Click += new System.EventHandler(this.instGuideBtn_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.installSvrBtn);
            this.groupBox5.Id = "119abb03-245e-4ca8-a205-d345049ecd96";
            this.groupBox5.Location = new System.Drawing.Point(3, 83);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(115, 40);
            this.groupBox5.TabIndex = 4;
            // 
            // installSvrBtn
            // 
            this.installSvrBtn.CommandName = "InstallServerBtn";
            this.installSvrBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.installSvrBtn.ForeColor = System.Drawing.Color.Green;
            this.installSvrBtn.Id = "30b8ad99-c041-43eb-b076-73c5fb7300cb";
            this.installSvrBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.installSvrBtn.Location = new System.Drawing.Point(3, 4);
            this.installSvrBtn.Name = "installSvrBtn";
            this.installSvrBtn.Size = new System.Drawing.Size(109, 32);
            this.installSvrBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.output_onlinepngtools3)});
            this.installSvrBtn.TabIndex = 0;
            this.installSvrBtn.Text = "     Install Server";
            this.installSvrBtn.UseVisualThemeForForeground = false;
            // 
            // gameImgPicBox
            // 
            this.gameImgPicBox.BackColor = System.Drawing.Color.Transparent;
            this.gameImgPicBox.BackgroundImage = global::Server_Creation_Tool.Properties.Resources._7DaysToDie_logo;
            this.gameImgPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.gameImgPicBox.Location = new System.Drawing.Point(9, 11);
            this.gameImgPicBox.Name = "gameImgPicBox";
            this.gameImgPicBox.Size = new System.Drawing.Size(322, 69);
            this.gameImgPicBox.TabIndex = 3;
            this.gameImgPicBox.TabStop = false;
            // 
            // actionsGrpBox
            // 
            this.actionsGrpBox.Controls.Add(this.act_Btn1);
            this.actionsGrpBox.Controls.Add(this.editSettingsDropBtn);
            this.actionsGrpBox.Controls.Add(this.reinstBtn);
            this.actionsGrpBox.Controls.Add(this.openFoldBtn);
            this.actionsGrpBox.Controls.Add(this.updtSvrBtn);
            this.actionsGrpBox.Controls.Add(this.delBtn);
            this.actionsGrpBox.Id = "fc599d8b-c74a-4e5e-95a9-3c5f5e283753";
            this.actionsGrpBox.Location = new System.Drawing.Point(235, 81);
            this.actionsGrpBox.Name = "actionsGrpBox";
            this.actionsGrpBox.Size = new System.Drawing.Size(123, 138);
            this.actionsGrpBox.TabIndex = 2;
            this.actionsGrpBox.Text = "Actions";
            // 
            // act_Btn1
            // 
            this.act_Btn1.CommandName = "deleteSrv";
            this.act_Btn1.Id = "85286d3f-17ba-4a98-9914-294e1d8ec0a0";
            this.act_Btn1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.act_Btn1.Location = new System.Drawing.Point(4, 135);
            this.act_Btn1.Name = "act_Btn1";
            this.act_Btn1.Size = new System.Drawing.Size(110, 23);
            this.act_Btn1.TabIndex = 9;
            this.act_Btn1.Text = "act_Btn1";
            this.act_Btn1.Visible = false;
            // 
            // editSettingsDropBtn
            // 
            this.editSettingsDropBtn.CommandName = "editConfSrv";
            this.editSettingsDropBtn.Id = "951da695-21f0-4580-901d-9d16659dce4d";
            this.editSettingsDropBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.editSettingsDropBtn.Location = new System.Drawing.Point(4, 111);
            this.editSettingsDropBtn.Name = "editSettingsDropBtn";
            this.editSettingsDropBtn.Popup = this.popupMenu4;
            this.editSettingsDropBtn.Size = new System.Drawing.Size(116, 23);
            this.editSettingsDropBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._1645)});
            this.editSettingsDropBtn.TabIndex = 8;
            this.editSettingsDropBtn.Text = "    Edit settings";
            this.editSettingsDropBtn.WordWrap = true;
            // 
            // popupMenu4
            // 
            this.popupMenu4.Items.AddRange(new System.Windows.Forms.Control[] {
            this.easyStgBtn,
            this.button8,
            this.button9,
            this.button10,
            this.button12});
            this.popupMenu4.KeepPopupsWithOffsetPlacementWithinPlacementArea = false;
            this.popupMenu4.PlacementMode = Elegant.Ui.PopupPlacementMode.Bottom;
            this.popupMenu4.Size = new System.Drawing.Size(100, 100);
            // 
            // easyStgBtn
            // 
            this.easyStgBtn.Id = "9224e92b-fef1-4f23-9ef6-3660a70673cb";
            this.easyStgBtn.Location = new System.Drawing.Point(2, 2);
            this.easyStgBtn.Name = "easyStgBtn";
            this.easyStgBtn.Size = new System.Drawing.Size(161, 23);
            this.easyStgBtn.TabIndex = 3;
            this.easyStgBtn.Tag = "0";
            this.easyStgBtn.Text = "Easy Settings Editor";
            this.easyStgBtn.Visible = false;
            // 
            // button8
            // 
            this.button8.Id = "1d40e929-aecd-4d50-a89c-32b6b4569793";
            this.button8.Location = new System.Drawing.Point(2, 25);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(161, 23);
            this.button8.TabIndex = 4;
            this.button8.Tag = "1";
            this.button8.Text = "button8";
            // 
            // button9
            // 
            this.button9.Id = "6b42cb43-11e5-4778-8310-6a8ef0416569";
            this.button9.Location = new System.Drawing.Point(2, 48);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(161, 23);
            this.button9.TabIndex = 5;
            this.button9.Tag = "2";
            this.button9.Text = "button9";
            // 
            // button10
            // 
            this.button10.Id = "51cecd83-5e58-4448-8dd5-62d03d2a6ea4";
            this.button10.Location = new System.Drawing.Point(2, 71);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(161, 23);
            this.button10.TabIndex = 6;
            this.button10.Tag = "3";
            this.button10.Text = "button10";
            // 
            // button12
            // 
            this.button12.Id = "1ae8a63b-0d01-4918-acf0-2cc68a05e189";
            this.button12.Location = new System.Drawing.Point(2, 94);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(161, 23);
            this.button12.TabIndex = 7;
            this.button12.Tag = "4";
            this.button12.Text = "button12";
            // 
            // reinstBtn
            // 
            this.reinstBtn.CommandName = "reinstallSrv";
            this.reinstBtn.Id = "c23115bb-f7b2-42d9-b9b0-96e83109bb92";
            this.reinstBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.reinstBtn.Location = new System.Drawing.Point(4, 39);
            this.reinstBtn.Name = "reinstBtn";
            this.reinstBtn.Size = new System.Drawing.Size(116, 23);
            this.reinstBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._2233)});
            this.reinstBtn.TabIndex = 4;
            this.reinstBtn.Text = " Reinstall";
            // 
            // openFoldBtn
            // 
            this.openFoldBtn.CommandName = "openFoldSrv";
            this.openFoldBtn.Id = "ddf33851-4e1c-4918-8731-79ee8ef6b554";
            this.openFoldBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.openFoldBtn.Location = new System.Drawing.Point(4, 63);
            this.openFoldBtn.Name = "openFoldBtn";
            this.openFoldBtn.Size = new System.Drawing.Size(116, 23);
            this.openFoldBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._468)});
            this.openFoldBtn.TabIndex = 1;
            this.openFoldBtn.Text = " Open folder";
            this.openFoldBtn.Click += new System.EventHandler(this.openFoldBtn_Click);
            // 
            // updtSvrBtn
            // 
            this.updtSvrBtn.CommandName = "updateSrv";
            this.updtSvrBtn.Id = "6b144d59-642a-434e-91d8-519024bd707d";
            this.updtSvrBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.updtSvrBtn.Location = new System.Drawing.Point(4, 15);
            this.updtSvrBtn.Name = "updtSvrBtn";
            this.updtSvrBtn.Size = new System.Drawing.Size(116, 23);
            this.updtSvrBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.refresh16x16)});
            this.updtSvrBtn.TabIndex = 0;
            this.updtSvrBtn.Text = " Update";
            // 
            // delBtn
            // 
            this.delBtn.CommandName = "deleteSrv";
            this.delBtn.Id = "2a3fd4f5-5c1b-45fd-bcdc-adaa776165f2";
            this.delBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.delBtn.Location = new System.Drawing.Point(4, 87);
            this.delBtn.Name = "delBtn";
            this.delBtn.Size = new System.Drawing.Size(116, 23);
            this.delBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._1263)});
            this.delBtn.TabIndex = 2;
            this.delBtn.Tag = "sgtDrp";
            this.delBtn.Text = " Delete";
            this.delBtn.Click += new System.EventHandler(this.delBtn_Click);
            // 
            // button16
            // 
            this.button16.Id = "c0f9687f-f565-4f4d-baa3-0fe0893fe243";
            this.button16.Location = new System.Drawing.Point(9, 77);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(109, 39);
            this.button16.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("button16.SmallImages.Images"))))});
            this.button16.TabIndex = 33;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.welcomeLbl);
            this.groupBox3.Controls.Add(this.welcomeDesclbl);
            this.groupBox3.Id = "5d5e9b40-f0a2-48ae-b5ca-66c4098d502a";
            this.groupBox3.Location = new System.Drawing.Point(3, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(355, 69);
            this.groupBox3.TabIndex = 31;
            // 
            // welcomeLbl
            // 
            this.welcomeLbl.Font = new System.Drawing.Font("Segoe UI Semibold", 12.22F, System.Drawing.FontStyle.Bold);
            this.welcomeLbl.Location = new System.Drawing.Point(39, 4);
            this.welcomeLbl.Name = "welcomeLbl";
            this.welcomeLbl.Size = new System.Drawing.Size(277, 25);
            this.welcomeLbl.TabIndex = 29;
            this.welcomeLbl.Text = "Welcome to Server Creation Tool 3.1";
            // 
            // welcomeDesclbl
            // 
            this.welcomeDesclbl.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.welcomeDesclbl.Location = new System.Drawing.Point(34, 30);
            this.welcomeDesclbl.Name = "welcomeDesclbl";
            this.welcomeDesclbl.Size = new System.Drawing.Size(281, 39);
            this.welcomeDesclbl.TabIndex = 30;
            this.welcomeDesclbl.Text = "This tool allows you to download, install and manage\r\n      steam and non-steam s" +
    "ervers quick and easy.";
            // 
            // basicInstrLbl
            // 
            this.basicInstrLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.basicInstrLbl.Location = new System.Drawing.Point(133, 103);
            this.basicInstrLbl.Name = "basicInstrLbl";
            this.basicInstrLbl.Size = new System.Drawing.Size(98, 15);
            this.basicInstrLbl.TabIndex = 28;
            this.basicInstrLbl.Text = "Basic Instructions:";
            // 
            // basicInstrTxtBox
            // 
            this.basicInstrTxtBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.basicInstrTxtBox.Id = "4c33a4bb-dc0c-4f36-bfee-73e24ac6ec87";
            this.basicInstrTxtBox.Location = new System.Drawing.Point(7, 119);
            this.basicInstrTxtBox.Multiline = true;
            this.basicInstrTxtBox.Name = "basicInstrTxtBox";
            this.basicInstrTxtBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.basicInstrTxtBox.Size = new System.Drawing.Size(348, 118);
            this.basicInstrTxtBox.TabIndex = 27;
            this.basicInstrTxtBox.Text = resources.GetString("basicInstrTxtBox.Text");
            this.basicInstrTxtBox.TextEditorWidth = 344;
            this.basicInstrTxtBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.basicInstrTxtBox_KeyDown);
            // 
            // nonSteamServerTimer
            // 
            this.nonSteamServerTimer.Interval = 2000;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 25000;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Tips-Info";
            // 
            // slowTimer
            // 
            this.slowTimer.Enabled = true;
            this.slowTimer.Interval = 12000;
            this.slowTimer.Tick += new System.EventHandler(this.logTimer_Tick);
            // 
            // editServerSettigsToolStripMenuItem
            // 
            this.editServerSettigsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("editServerSettigsToolStripMenuItem.Image")));
            this.editServerSettigsToolStripMenuItem.Name = "editServerSettigsToolStripMenuItem";
            this.editServerSettigsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.editServerSettigsToolStripMenuItem.Text = "Edit Server Settings";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 404);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.ribbon1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(637, 412);
            this.MinimumSize = new System.Drawing.Size(637, 412);
            this.Name = "MainForm";
            this.Text = "Server Creation Tool 3.1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noInternetTab2)).EndInit();
            this.noInternetTab2.ResumeLayout(false);
            this.noInternetTab2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonGroup6)).EndInit();
            this.ribbonGroup6.ResumeLayout(false);
            this.ribbonGroup6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainTabPage)).EndInit();
            this.mainTabPage.ResumeLayout(false);
            this.mainTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.generalSvrOptGroup)).EndInit();
            this.generalSvrOptGroup.ResumeLayout(false);
            this.generalSvrOptGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optionsRibbon)).EndInit();
            this.optionsRibbon.ResumeLayout(false);
            this.optionsRibbon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentTaskRibGrp)).EndInit();
            this.currentTaskRibGrp.ResumeLayout(false);
            this.currentTaskRibGrp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.helpInfTab)).EndInit();
            this.helpInfTab.ResumeLayout(false);
            this.helpInfTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.helpInfGrpBox)).EndInit();
            this.helpInfGrpBox.ResumeLayout(false);
            this.helpInfGrpBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonGroup4)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.steamSvrNavBar.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.serverPnl.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.extraGrpBox.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gameImgPicBox)).EndInit();
            this.actionsGrpBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu4)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

		}
        private System.Windows.Forms.Timer SteamServertimer;
        private System.Windows.Forms.Timer generalTimer;
        private System.Windows.Forms.ToolStripMenuItem editServerSettigsToolStripMenuItem;
        private Elegant.Ui.Ribbon ribbon1;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner;
        private Elegant.Ui.ApplicationMenu applicationMenu1;
        private Elegant.Ui.RibbonContextualTabGroup ribbonContextualTabGroup2;
        private Elegant.Ui.RibbonGroup ribbonGroup4;
        private Elegant.Ui.GroupBox groupBox2;
        private Elegant.Ui.TextBox textBox1;
        private Elegant.Ui.RibbonTabPage helpInfTab;
        private Elegant.Ui.RibbonGroup helpInfGrpBox;
        private Elegant.Ui.Button changeLogBtn;
        private Elegant.Ui.Button faqBtn;
        private Elegant.Ui.Button serverGroupBtn;
        private Elegant.Ui.Separator separator1;
        private Elegant.Ui.Button plannedFeaturesBtn;
        private Elegant.Ui.Button aboutBtn;
        private Elegant.Ui.DropDown langBtn;
        private Elegant.Ui.PopupMenu popupMenu1;
        private Elegant.Ui.Button engBtn;
        private Elegant.Ui.Button gerBtn;
        private Elegant.Ui.RibbonTabPage mainTabPage;
        private Elegant.Ui.RibbonGroup generalSvrOptGroup;
        private Elegant.Ui.GroupBox groupBox1;
        private Elegant.Ui.Panel panel2;
        private Elegant.Ui.Button button1;
        private Elegant.Ui.Button button4;
        private Elegant.Ui.Button button7;
        private Elegant.Ui.Button button23;
        private Elegant.Ui.Button button25;
        private Elegant.Ui.Button button3;
        private Elegant.Ui.Button button6;
        private Elegant.Ui.Button button21;
        private Elegant.Ui.Button button24;
        private Elegant.Ui.Button button26;
        private Elegant.Ui.Button button27;
        private Elegant.Ui.Button button28;
        private Elegant.Ui.Button button29;
        private Elegant.Ui.Button button30;
        private Elegant.Ui.Button selectInstFoldBtn;
        private Elegant.Ui.Button openSrvsFoldBtn;
        private Elegant.Ui.RibbonContextualTabGroup noInternetTab;
        private Elegant.Ui.RibbonGroup ribbonGroup6;
        private Elegant.Ui.Button refreshConnBtn;
        private Elegant.Ui.TextBox noInternetTxtBox;
        private Elegant.Ui.TabControl tabControl1;
        private Elegant.Ui.TabPage tabPage1;
        private Elegant.Ui.TabPage tabPage2;
        private Elegant.Ui.RibbonTabPage noInternetTab2;
        private Elegant.Ui.NavigationBar steamSvrNavBar;
        private Elegant.Ui.ToggleButton daysToDieBtn;
        private Elegant.Ui.ToggleButton CSsourceBtn;
        private Elegant.Ui.ToggleButton KillingFloorTwoBtn;
        private Elegant.Ui.ToggleButton SvenCoopBtn;
        private Elegant.Ui.ToggleButton arkBtn;
        private Elegant.Ui.ToggleButton CSGOBtn;
        private Elegant.Ui.ToggleButton CodBo3Btn;
        private Elegant.Ui.ToggleButton l4dBtn;
        private Elegant.Ui.ToggleButton l4d2Btn;
        private Elegant.Ui.ToggleButton hurtWorldBtn;
        private Elegant.Ui.ToggleButton garrysModBtn;
        private Elegant.Ui.ToggleButton cs16Btn;
        private Elegant.Ui.ToggleButton RustBtn;
        private Elegant.Ui.ToggleButton synergyBtn;
        private Elegant.Ui.Label label1;
        private Elegant.Ui.Label searchGameLbl;
        private Elegant.Ui.TextBox steamGameSrchBox;
        private Elegant.Ui.Separator separator2;
        private Elegant.Ui.DropDown variousStuffBtn;
        private Elegant.Ui.PopupMenu popupMenu3;
        private Elegant.Ui.Button portFwRouterBtn;
        private Elegant.Ui.Panel panel1;
        private Elegant.Ui.Separator separator3;
        private Elegant.Ui.ToggleButton welcomeBtn;
        private Elegant.Ui.Label basicInstrLbl;
        private Elegant.Ui.TextBox basicInstrTxtBox;
        private Elegant.Ui.GroupBox groupBox3;
        private Elegant.Ui.Label welcomeLbl;
        private Elegant.Ui.Label welcomeDesclbl;
        private Elegant.Ui.RibbonGroup currentTaskRibGrp;
        private Elegant.Ui.Label currentTaskNameLbl;
        private Elegant.Ui.ProgressBar progressBar1;
        private System.Windows.Forms.Timer nonSteamServerTimer;
        private Elegant.Ui.ContextMenuExtenderProvider contextMenuExtenderProvider1;
        private Elegant.Ui.RibbonGroup optionsRibbon;
        private Elegant.Ui.Button chck4UpdatesBtn;
        private Elegant.Ui.DropDown styleBtn;
        private Elegant.Ui.PopupMenu popupMenu2;
        private Elegant.Ui.Button button2;
        private Elegant.Ui.Button button11;
        private System.Windows.Forms.ToolTip toolTip1;
        private Elegant.Ui.Button donateBtn;
        private Elegant.Ui.Button button16;
        private Elegant.Ui.Button appLogsbtn;
        private System.Windows.Forms.Timer slowTimer;
        private Elegant.Ui.ToggleButton unturnedBtn;
        private Elegant.Ui.ToggleButton atlasBtn;
        private Elegant.Ui.GroupBox serverPnl;
        private Elegant.Ui.GroupBox groupBox7;
        private Elegant.Ui.Label srvSizeLbl;
        private Elegant.Ui.TextBox srvSizeTxtBox;
        private Elegant.Ui.GroupBox extraGrpBox;
        private Elegant.Ui.Button extraBtn4;
        private Elegant.Ui.Button extraBtn3;
        private Elegant.Ui.Button extraBtn2;
        private Elegant.Ui.Button extraBtn1;
        private Elegant.Ui.Button instGuideBtn;
        private Elegant.Ui.GroupBox groupBox5;
        private Elegant.Ui.Button installSvrBtn;
        private System.Windows.Forms.PictureBox gameImgPicBox;
        private Elegant.Ui.GroupBox actionsGrpBox;
        private Elegant.Ui.Button act_Btn1;
        private Elegant.Ui.DropDown editSettingsDropBtn;
        private Elegant.Ui.PopupMenu popupMenu4;
        private Elegant.Ui.Button reinstBtn;
        private Elegant.Ui.Button openFoldBtn;
        private Elegant.Ui.Button updtSvrBtn;
        private Elegant.Ui.Button delBtn;
        private Elegant.Ui.Button easyStgBtn;
        private Elegant.Ui.Button button8;
        private Elegant.Ui.Button button9;
        private Elegant.Ui.Button button10;
        private Elegant.Ui.Button button12;
    }
}
