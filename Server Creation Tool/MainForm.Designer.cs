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
            this.langRibbon = new Elegant.Ui.RibbonTabPage();
            this.ribbonGroup1 = new Elegant.Ui.RibbonGroup();
            this.dropDown3 = new Elegant.Ui.DropDown();
            this.popupMenu3 = new Elegant.Ui.PopupMenu(this.components);
            this.button34 = new Elegant.Ui.Button();
            this.button5 = new Elegant.Ui.Button();
            this.serverGroupBtn = new Elegant.Ui.Button();
            this.separator1 = new Elegant.Ui.Separator();
            this.button32 = new Elegant.Ui.Button();
            this.plannedFeaturesBtn = new Elegant.Ui.Button();
            this.button9 = new Elegant.Ui.Button();
            this.changeLogBtn = new Elegant.Ui.Button();
            this.button8 = new Elegant.Ui.Button();
            this.aboutBtn = new Elegant.Ui.Button();
            this.mainTabPage = new Elegant.Ui.RibbonTabPage();
            this.generalSvrOptGroup = new Elegant.Ui.RibbonGroup();
            this.openServersFoldBtn = new Elegant.Ui.Button();
            this.changeSrvrInstFoldBtn = new Elegant.Ui.Button();
            this.optionsRibbon = new Elegant.Ui.RibbonGroup();
            this.check4UpdatesBtn = new Elegant.Ui.Button();
            this.dropDown2 = new Elegant.Ui.DropDown();
            this.popupMenu2 = new Elegant.Ui.PopupMenu(this.components);
            this.button2 = new Elegant.Ui.Button();
            this.button11 = new Elegant.Ui.Button();
            this.dropDown1 = new Elegant.Ui.DropDown();
            this.popupMenu1 = new Elegant.Ui.PopupMenu(this.components);
            this.engBtn = new Elegant.Ui.Button();
            this.gerBtn = new Elegant.Ui.Button();
            this.currentOperRibGrp = new Elegant.Ui.RibbonGroup();
            this.currentOperNameLbl = new Elegant.Ui.Label();
            this.progressBar1 = new Elegant.Ui.ProgressBar();
            this.formFrameSkinner = new Elegant.Ui.FormFrameSkinner();
            this.ribbonContextualTabGroup2 = new Elegant.Ui.RibbonContextualTabGroup(this.components);
            this.ribbonGroup4 = new Elegant.Ui.RibbonGroup();
            this.groupBox2 = new Elegant.Ui.GroupBox();
            this.steamGameSrchBox = new Elegant.Ui.TextBox();
            this.searchSteamGameLbl = new Elegant.Ui.Label();
            this.tabControl1 = new Elegant.Ui.TabControl();
            this.tabPage1 = new Elegant.Ui.TabPage();
            this.steamSvrNavBar = new Elegant.Ui.NavigationBar();
            this.welcomeBtn = new Elegant.Ui.ToggleButton();
            this.daysToDieBtn = new Elegant.Ui.ToggleButton();
            this.arkBtn = new Elegant.Ui.ToggleButton();
            this.CODblackOpsThreeBtn = new Elegant.Ui.ToggleButton();
            this.csOnePointsixBtn = new Elegant.Ui.ToggleButton();
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
            this.updtTimer = new System.Windows.Forms.Timer(this.components);
            this.separator2 = new Elegant.Ui.Separator();
            this.srvPanelGrpBox = new Elegant.Ui.GroupBox();
            this.groupBox7 = new Elegant.Ui.GroupBox();
            this.label2 = new Elegant.Ui.Label();
            this.srvTxtBox = new Elegant.Ui.TextBox();
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
            this.button10 = new Elegant.Ui.Button();
            this.button12 = new Elegant.Ui.Button();
            this.button13 = new Elegant.Ui.Button();
            this.button14 = new Elegant.Ui.Button();
            this.reinstBtn = new Elegant.Ui.Button();
            this.openFoldBtn = new Elegant.Ui.Button();
            this.updtSvrBtn = new Elegant.Ui.Button();
            this.delBtn = new Elegant.Ui.Button();
            this.panel1 = new Elegant.Ui.Panel();
            this.button16 = new Elegant.Ui.Button();
            this.groupBox3 = new Elegant.Ui.GroupBox();
            this.label4 = new Elegant.Ui.Label();
            this.label5 = new Elegant.Ui.Label();
            this.label3 = new Elegant.Ui.Label();
            this.basicInstrTxtBox = new Elegant.Ui.TextBox();
            this.nonSteamServerTimer = new System.Windows.Forms.Timer(this.components);
            this.contextMenuExtenderProvider1 = new Elegant.Ui.ContextMenuExtenderProvider(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.editServerSettigsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logTimer = new System.Windows.Forms.Timer(this.components);
            themeSelector = new Elegant.Ui.ThemeSelector(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noInternetTab2)).BeginInit();
            this.noInternetTab2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonGroup6)).BeginInit();
            this.ribbonGroup6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.langRibbon)).BeginInit();
            this.langRibbon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonGroup1)).BeginInit();
            this.ribbonGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainTabPage)).BeginInit();
            this.mainTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.generalSvrOptGroup)).BeginInit();
            this.generalSvrOptGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optionsRibbon)).BeginInit();
            this.optionsRibbon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentOperRibGrp)).BeginInit();
            this.currentOperRibGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonGroup4)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.steamSvrNavBar.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.srvPanelGrpBox.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.extraGrpBox.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gameImgPicBox)).BeginInit();
            this.actionsGrpBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu4)).BeginInit();
            this.panel1.SuspendLayout();
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
            this.generalTimer.Enabled = true;
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
            this.ribbon1.CurrentTabPage = this.langRibbon;
            this.ribbon1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ribbon1.Id = "f194746d-ef81-4736-9469-969158232f18";
            this.ribbon1.Location = new System.Drawing.Point(0, 0);
            this.ribbon1.MinimizeButtonVisible = false;
            this.ribbon1.Name = "ribbon1";
            this.ribbon1.QuickAccessToolbarDropDownVisible = false;
            this.ribbon1.QuickAccessToolbarPlacementMode = Elegant.Ui.QuickAccessToolbarPlacementMode.Hidden;
            this.ribbon1.Size = new System.Drawing.Size(629, 148);
            this.ribbon1.TabIndex = 22;
            this.ribbon1.TabPages.AddRange(new Elegant.Ui.RibbonTabPage[] {
            this.mainTabPage,
            this.langRibbon});
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
            this.ribbonGroup6.Size = new System.Drawing.Size(245, 94);
            this.ribbonGroup6.TabIndex = 0;
            this.ribbonGroup6.Text = "Actions";
            // 
            // refreshConnBtn
            // 
            this.refreshConnBtn.Id = "33ea7943-be5d-4a4c-ab7a-427696de20fd";
            this.refreshConnBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.refresh)});
            this.refreshConnBtn.Location = new System.Drawing.Point(26, 2);
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
            this.noInternetTxtBox.Id = "821bb26a-0060-45cd-9bca-360cf23d825b";
            this.noInternetTxtBox.Location = new System.Drawing.Point(26, 2);
            this.noInternetTxtBox.Multiline = true;
            this.noInternetTxtBox.Name = "noInternetTxtBox";
            this.noInternetTxtBox.Size = new System.Drawing.Size(191, 72);
            this.noInternetTxtBox.TabIndex = 2;
            this.noInternetTxtBox.Text = "There is no internet connection!\r\n----\r\nPlease check your internet access!\r\n";
            this.noInternetTxtBox.TextEditorWidth = 185;
            // 
            // langRibbon
            // 
            this.langRibbon.Controls.Add(this.ribbonGroup1);
            this.langRibbon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.langRibbon.KeyTip = null;
            this.langRibbon.Location = new System.Drawing.Point(0, 0);
            this.langRibbon.Name = "langRibbon";
            this.langRibbon.Size = new System.Drawing.Size(629, 101);
            this.langRibbon.TabIndex = 0;
            this.langRibbon.Text = "Help-Information";
            // 
            // ribbonGroup1
            // 
            this.ribbonGroup1.Controls.Add(this.dropDown3);
            this.ribbonGroup1.Controls.Add(this.button5);
            this.ribbonGroup1.Controls.Add(this.serverGroupBtn);
            this.ribbonGroup1.Controls.Add(this.separator1);
            this.ribbonGroup1.Controls.Add(this.button32);
            this.ribbonGroup1.Controls.Add(this.plannedFeaturesBtn);
            this.ribbonGroup1.Controls.Add(this.button9);
            this.ribbonGroup1.Controls.Add(this.changeLogBtn);
            this.ribbonGroup1.Controls.Add(this.button8);
            this.ribbonGroup1.Controls.Add(this.aboutBtn);
            this.ribbonGroup1.Location = new System.Drawing.Point(4, 3);
            this.ribbonGroup1.Name = "ribbonGroup1";
            this.ribbonGroup1.Size = new System.Drawing.Size(466, 94);
            this.ribbonGroup1.TabIndex = 0;
            this.ribbonGroup1.Text = "Help-Information";
            // 
            // dropDown3
            // 
            this.dropDown3.Id = "de417b21-e7f6-4566-b4b6-b772a870c4d5";
            this.dropDown3.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.settings__4_)});
            this.dropDown3.Location = new System.Drawing.Point(4, 2);
            this.dropDown3.Name = "dropDown3";
            this.dropDown3.Popup = this.popupMenu3;
            this.dropDown3.Size = new System.Drawing.Size(88, 72);
            this.dropDown3.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.settings__4_)});
            this.dropDown3.TabIndex = 7;
            this.dropDown3.Text = "Help for setting up servers";
            // 
            // popupMenu3
            // 
            this.popupMenu3.Items.AddRange(new System.Windows.Forms.Control[] {
            this.button34});
            this.popupMenu3.KeepPopupsWithOffsetPlacementWithinPlacementArea = false;
            this.popupMenu3.PlacementMode = Elegant.Ui.PopupPlacementMode.Bottom;
            this.popupMenu3.Size = new System.Drawing.Size(100, 100);
            // 
            // button34
            // 
            this.button34.Id = "a065ba1c-06b5-4e49-860d-8c907eeeff7e";
            this.button34.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.port)});
            this.button34.Location = new System.Drawing.Point(2, 2);
            this.button34.Name = "button34";
            this.button34.Size = new System.Drawing.Size(233, 23);
            this.button34.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.Icon_20__2_)});
            this.button34.TabIndex = 3;
            this.button34.Text = "How to port forward your router";
            this.button34.Click += new System.EventHandler(this.button34_Click_1);
            // 
            // button5
            // 
            this.button5.Id = "05812acb-de7a-4cb6-b47d-db8fbe9bf039";
            this.button5.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._1502)});
            this.button5.Location = new System.Drawing.Point(94, 2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(42, 72);
            this.button5.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._1502)});
            this.button5.TabIndex = 2;
            this.button5.Text = "FAQ";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // serverGroupBtn
            // 
            this.serverGroupBtn.Id = "0e1fd651-cc6b-4b9d-a6a0-0d2ac588bd5a";
            this.serverGroupBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.chat_group)});
            this.serverGroupBtn.Location = new System.Drawing.Point(138, 2);
            this.serverGroupBtn.Name = "serverGroupBtn";
            this.serverGroupBtn.Size = new System.Drawing.Size(42, 72);
            this.serverGroupBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.chat_group)});
            this.serverGroupBtn.TabIndex = 1;
            this.serverGroupBtn.Text = "Server Group";
            this.serverGroupBtn.Click += new System.EventHandler(this.serverGroupBtn_Click_1);
            // 
            // separator1
            // 
            this.separator1.Id = "34dff717-ee67-4b91-ad36-90995729da61";
            this.separator1.Location = new System.Drawing.Point(184, 7);
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(2, 60);
            this.separator1.TabIndex = 3;
            this.separator1.Text = "separator1";
            // 
            // button32
            // 
            this.button32.Id = "06f16e20-038b-4209-9877-4de187dff9f1";
            this.button32.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.world_wide_web)});
            this.button32.Location = new System.Drawing.Point(190, 2);
            this.button32.Name = "button32";
            this.button32.Size = new System.Drawing.Size(42, 72);
            this.button32.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.world_wide_web)});
            this.button32.TabIndex = 6;
            this.button32.Text = "Tool\'s page";
            this.button32.Click += new System.EventHandler(this.button32_Click);
            // 
            // plannedFeaturesBtn
            // 
            this.plannedFeaturesBtn.Id = "89b60cbb-01d2-4be0-9174-6376d7c02bbc";
            this.plannedFeaturesBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.pencil)});
            this.plannedFeaturesBtn.Location = new System.Drawing.Point(234, 2);
            this.plannedFeaturesBtn.Name = "plannedFeaturesBtn";
            this.plannedFeaturesBtn.Size = new System.Drawing.Size(48, 72);
            this.plannedFeaturesBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.pencil)});
            this.plannedFeaturesBtn.TabIndex = 4;
            this.plannedFeaturesBtn.Text = "Planned Features";
            this.plannedFeaturesBtn.Click += new System.EventHandler(this.plannedFeaturesBtn_Click);
            // 
            // button9
            // 
            this.button9.Id = "297e6b1f-4afe-4502-9e13-4c7ba04cdbdb";
            this.button9.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._622)});
            this.button9.Location = new System.Drawing.Point(284, 2);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(42, 72);
            this.button9.TabIndex = 9;
            this.button9.Text = "App Logs";
            this.button9.Click += new System.EventHandler(this.button9_Click_1);
            // 
            // changeLogBtn
            // 
            this.changeLogBtn.Id = "2e46f931-fe47-4631-b619-c898a8e94743";
            this.changeLogBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.CHECK_BOARD_48_48)});
            this.changeLogBtn.Location = new System.Drawing.Point(328, 2);
            this.changeLogBtn.Name = "changeLogBtn";
            this.changeLogBtn.Size = new System.Drawing.Size(45, 72);
            this.changeLogBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.CHECK_BOARD)});
            this.changeLogBtn.TabIndex = 0;
            this.changeLogBtn.Text = "Change Log";
            this.changeLogBtn.Click += new System.EventHandler(this.changeLogBtn_Click_1);
            // 
            // button8
            // 
            this.button8.Id = "8fd67815-e9e1-4581-ab7f-c6fde8f5eb53";
            this.button8.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("button8.LargeImages.Images"))))});
            this.button8.Location = new System.Drawing.Point(375, 2);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(42, 72);
            this.button8.TabIndex = 8;
            this.button8.Text = "Donate";
            // 
            // aboutBtn
            // 
            this.aboutBtn.Id = "d9df181f-cb0b-4e2d-be98-7409ed08db1c";
            this.aboutBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._2191)});
            this.aboutBtn.Location = new System.Drawing.Point(419, 2);
            this.aboutBtn.Name = "aboutBtn";
            this.aboutBtn.Size = new System.Drawing.Size(42, 72);
            this.aboutBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._2191)});
            this.aboutBtn.TabIndex = 5;
            this.aboutBtn.Text = "About";
            this.aboutBtn.Click += new System.EventHandler(this.button6_Click);
            // 
            // mainTabPage
            // 
            this.mainTabPage.Controls.Add(this.generalSvrOptGroup);
            this.mainTabPage.Controls.Add(this.optionsRibbon);
            this.mainTabPage.Controls.Add(this.currentOperRibGrp);
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
            this.generalSvrOptGroup.Controls.Add(this.openServersFoldBtn);
            this.generalSvrOptGroup.Controls.Add(this.changeSrvrInstFoldBtn);
            this.generalSvrOptGroup.Location = new System.Drawing.Point(4, 3);
            this.generalSvrOptGroup.Name = "generalSvrOptGroup";
            this.generalSvrOptGroup.Size = new System.Drawing.Size(179, 94);
            this.generalSvrOptGroup.TabIndex = 2;
            this.generalSvrOptGroup.Text = "General Server Options";
            // 
            // openServersFoldBtn
            // 
            this.openServersFoldBtn.Id = "42ee3df9-1c92-45d3-ad19-acacb20739c6";
            this.openServersFoldBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.folderOpen)});
            this.openServersFoldBtn.Location = new System.Drawing.Point(12, 2);
            this.openServersFoldBtn.Name = "openServersFoldBtn";
            this.openServersFoldBtn.Size = new System.Drawing.Size(101, 0);
            this.openServersFoldBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.folder)});
            this.openServersFoldBtn.TabIndex = 1;
            this.openServersFoldBtn.Text = "Open servers folder";
            this.openServersFoldBtn.Click += new System.EventHandler(this.openServersFoldBtn_Click);
            // 
            // changeSrvrInstFoldBtn
            // 
            this.changeSrvrInstFoldBtn.Id = "06bbf161-17aa-4ac7-ab21-097a5d98bff0";
            this.changeSrvrInstFoldBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._2274)});
            this.changeSrvrInstFoldBtn.Location = new System.Drawing.Point(12, 2);
            this.changeSrvrInstFoldBtn.Name = "changeSrvrInstFoldBtn";
            this.changeSrvrInstFoldBtn.Size = new System.Drawing.Size(154, 0);
            this.changeSrvrInstFoldBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._2276)});
            this.changeSrvrInstFoldBtn.TabIndex = 0;
            this.changeSrvrInstFoldBtn.Text = "Select Server installation folder";
            this.changeSrvrInstFoldBtn.Click += new System.EventHandler(this.changeSrvrInstFoldBtn_Click);
            // 
            // optionsRibbon
            // 
            this.optionsRibbon.Controls.Add(this.check4UpdatesBtn);
            this.optionsRibbon.Controls.Add(this.dropDown2);
            this.optionsRibbon.Controls.Add(this.dropDown1);
            this.optionsRibbon.Location = new System.Drawing.Point(185, 3);
            this.optionsRibbon.Name = "optionsRibbon";
            this.optionsRibbon.Size = new System.Drawing.Size(176, 94);
            this.optionsRibbon.TabIndex = 4;
            this.optionsRibbon.Text = "Other Options";
            // 
            // check4UpdatesBtn
            // 
            this.check4UpdatesBtn.Id = "3eaae1a8-f5e6-462e-a007-5772cd760ed9";
            this.check4UpdatesBtn.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.captcha)});
            this.check4UpdatesBtn.Location = new System.Drawing.Point(38, 2);
            this.check4UpdatesBtn.Name = "check4UpdatesBtn";
            this.check4UpdatesBtn.Size = new System.Drawing.Size(96, 0);
            this.check4UpdatesBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.captcha)});
            this.check4UpdatesBtn.TabIndex = 1;
            this.check4UpdatesBtn.Text = "Check for updates";
            // 
            // dropDown2
            // 
            this.dropDown2.Id = "279522bb-2088-4e64-8899-c729b9f02c3b";
            this.dropDown2.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.desktopWithcolors)});
            this.dropDown2.Location = new System.Drawing.Point(38, 2);
            this.dropDown2.Name = "dropDown2";
            this.dropDown2.Popup = this.popupMenu2;
            this.dropDown2.Size = new System.Drawing.Size(40, 0);
            this.dropDown2.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.desktopWithcolors)});
            this.dropDown2.TabIndex = 0;
            this.dropDown2.Text = "Style";
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
            this.button2.Id = "a200069f-38ab-4cd6-99d0-40674928bdae";
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
            this.button11.Id = "2ee69117-a6bd-4484-935f-524082ef6257";
            this.button11.Location = new System.Drawing.Point(2, 37);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(142, 35);
            this.button11.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.black_theme)});
            this.button11.TabIndex = 4;
            this.button11.Text = "Black";
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // dropDown1
            // 
            this.dropDown1.Id = "fc44b2cf-d579-4a30-a6b5-4beee1883528";
            this.dropDown1.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.translate__1_)});
            this.dropDown1.Location = new System.Drawing.Point(38, 2);
            this.dropDown1.Name = "dropDown1";
            this.dropDown1.Popup = this.popupMenu1;
            this.dropDown1.Size = new System.Drawing.Size(98, 0);
            this.dropDown1.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.translate)});
            this.dropDown1.TabIndex = 0;
            this.dropDown1.Text = "Select Language";
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
            this.engBtn.Id = "d1995150-54c8-4c52-b321-35bc99b3ea75";
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
            this.gerBtn.Id = "41a1c2bb-4473-4ba3-865c-63ee8869bf58";
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
            // currentOperRibGrp
            // 
            this.currentOperRibGrp.Controls.Add(this.currentOperNameLbl);
            this.currentOperRibGrp.Controls.Add(this.progressBar1);
            this.currentOperRibGrp.Location = new System.Drawing.Point(363, 3);
            this.currentOperRibGrp.Name = "currentOperRibGrp";
            this.currentOperRibGrp.Size = new System.Drawing.Size(184, 94);
            this.currentOperRibGrp.TabIndex = 3;
            this.currentOperRibGrp.Text = "Current Task";
            this.currentOperRibGrp.Visible = false;
            // 
            // currentOperNameLbl
            // 
            this.currentOperNameLbl.Location = new System.Drawing.Point(3, 2);
            this.currentOperNameLbl.Name = "currentOperNameLbl";
            this.currentOperNameLbl.Size = new System.Drawing.Size(97, 13);
            this.currentOperNameLbl.TabIndex = 8;
            this.currentOperNameLbl.Text = "OPERATION NAME";
            // 
            // progressBar1
            // 
            this.progressBar1.DesiredWidth = 175;
            this.progressBar1.Id = "3f4e45a2-d670-4e07-9173-b946a77deeb4";
            this.progressBar1.Location = new System.Drawing.Point(4, 15);
            this.progressBar1.MarqueeAnimationSpeed = 400;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(175, 12);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 7;
            this.progressBar1.Text = "progressBar1";
            this.progressBar1.Value = 50;
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
            this.groupBox2.Controls.Add(this.searchSteamGameLbl);
            this.groupBox2.Controls.Add(this.tabControl1);
            this.groupBox2.Id = "378a1e9e-9e31-4eb5-9cc5-993b67792cbc";
            this.groupBox2.Location = new System.Drawing.Point(1, 148);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(267, 257);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.Text = "Select a server";
            // 
            // steamGameSrchBox
            // 
            this.steamGameSrchBox.Id = "562008cb-6a95-4e59-8bee-cf4de4812db3";
            this.steamGameSrchBox.Location = new System.Drawing.Point(149, 10);
            this.steamGameSrchBox.Name = "steamGameSrchBox";
            this.steamGameSrchBox.Size = new System.Drawing.Size(110, 21);
            this.steamGameSrchBox.TabIndex = 2;
            this.steamGameSrchBox.TextEditorWidth = 104;
            this.steamGameSrchBox.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // searchSteamGameLbl
            // 
            this.searchSteamGameLbl.Location = new System.Drawing.Point(80, 14);
            this.searchSteamGameLbl.Name = "searchSteamGameLbl";
            this.searchSteamGameLbl.Size = new System.Drawing.Size(68, 13);
            this.searchSteamGameLbl.TabIndex = 0;
            this.searchSteamGameLbl.Text = "Search game:";
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
            this.steamSvrNavBar.Controls.Add(this.welcomeBtn);
            this.steamSvrNavBar.Controls.Add(this.daysToDieBtn);
            this.steamSvrNavBar.Controls.Add(this.arkBtn);
            this.steamSvrNavBar.Controls.Add(this.CODblackOpsThreeBtn);
            this.steamSvrNavBar.Controls.Add(this.csOnePointsixBtn);
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
            this.steamSvrNavBar.Id = "c1c935da-2591-4245-ac58-354bafcd0120";
            this.steamSvrNavBar.Location = new System.Drawing.Point(2, 0);
            this.steamSvrNavBar.Name = "steamSvrNavBar";
            this.steamSvrNavBar.Size = new System.Drawing.Size(258, 201);
            this.steamSvrNavBar.TabIndex = 0;
            this.steamSvrNavBar.Text = "navigationBar1";
            this.steamSvrNavBar.UseTabToNavigate = false;
            this.steamSvrNavBar.WrapNavigation = false;
            // 
            // welcomeBtn
            // 
            this.welcomeBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.welcomeBtn.Id = "374317c1-3547-4c4c-b593-3518c5db23a2";
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
            this.daysToDieBtn.Id = "494c093a-302d-422f-abf9-68e081bf4974";
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
            this.arkBtn.Id = "c188fb2e-d0b2-45f2-8eb3-d9cc745369a3";
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
            // CODblackOpsThreeBtn
            // 
            this.CODblackOpsThreeBtn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.CODblackOpsThreeBtn.Id = "5683f765-34dc-42de-98a9-8571199ac3d2";
            this.CODblackOpsThreeBtn.Location = new System.Drawing.Point(2, 87);
            this.CODblackOpsThreeBtn.Name = "CODblackOpsThreeBtn";
            this.CODblackOpsThreeBtn.RadioGroupName = "NavigationBarToggleButtons";
            this.CODblackOpsThreeBtn.Size = new System.Drawing.Size(237, 25);
            this.CODblackOpsThreeBtn.TabIndex = 3;
            this.CODblackOpsThreeBtn.Tag = "bo3";
            this.CODblackOpsThreeBtn.Text = "Call of Duty: Black Ops 3";
            this.CODblackOpsThreeBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CODblackOpsThreeBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.CODblackOpsThreeBtn.Click += new System.EventHandler(this.CODblackOpsThreeBtn_Click);
            // 
            // csOnePointsixBtn
            // 
            this.csOnePointsixBtn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.csOnePointsixBtn.Id = "bdc74a35-5973-4afc-a2bb-6e8d7d0d8e72";
            this.csOnePointsixBtn.Location = new System.Drawing.Point(2, 114);
            this.csOnePointsixBtn.Name = "csOnePointsixBtn";
            this.csOnePointsixBtn.RadioGroupName = "NavigationBarToggleButtons";
            this.csOnePointsixBtn.Size = new System.Drawing.Size(237, 25);
            this.csOnePointsixBtn.TabIndex = 4;
            this.csOnePointsixBtn.Tag = "cs";
            this.csOnePointsixBtn.Text = "Counter-Strike: 1.6";
            this.csOnePointsixBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.csOnePointsixBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.csOnePointsixBtn.Click += new System.EventHandler(this.csOnePointsixBtn_Click);
            // 
            // CSsourceBtn
            // 
            this.CSsourceBtn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.CSsourceBtn.Id = "42ac0f8f-409a-45c2-86d7-1fed1e6fb269";
            this.CSsourceBtn.Location = new System.Drawing.Point(2, 141);
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
            this.CSGOBtn.Id = "d4efdfda-e7b8-48c7-a131-bdcc1eb8f72e";
            this.CSGOBtn.Location = new System.Drawing.Point(2, 168);
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
            this.garrysModBtn.Id = "da71cf5b-ca05-4c82-a6ff-bd382dab2329";
            this.garrysModBtn.Location = new System.Drawing.Point(2, 195);
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
            this.hurtWorldBtn.Id = "64445731-54ca-4884-a491-df6633d41c60";
            this.hurtWorldBtn.Location = new System.Drawing.Point(2, 222);
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
            this.KillingFloorTwoBtn.Id = "bc2dae92-901b-4b4e-b227-a7831f81de2a";
            this.KillingFloorTwoBtn.Location = new System.Drawing.Point(2, 249);
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
            this.l4dBtn.Id = "eac33f6a-5296-4791-99cd-fa30d87b5b58";
            this.l4dBtn.Location = new System.Drawing.Point(2, 276);
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
            this.l4d2Btn.Id = "52f84a99-4470-4d42-b0bc-f13c994f27ba";
            this.l4d2Btn.Location = new System.Drawing.Point(2, 303);
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
            this.RustBtn.Id = "83056a08-f2d6-4634-a7e7-0e23dfed19c6";
            this.RustBtn.Location = new System.Drawing.Point(2, 330);
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
            this.SvenCoopBtn.Id = "bf503d6c-d483-4aaf-9d0b-6f60c853a647";
            this.SvenCoopBtn.Location = new System.Drawing.Point(2, 357);
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
            this.synergyBtn.Id = "b6536169-5e9b-47a7-8971-8ada42204ffd";
            this.synergyBtn.Location = new System.Drawing.Point(2, 384);
            this.synergyBtn.Name = "synergyBtn";
            this.synergyBtn.RadioGroupName = "NavigationBarToggleButtons";
            this.synergyBtn.Size = new System.Drawing.Size(237, 25);
            this.synergyBtn.TabIndex = 14;
            this.synergyBtn.Tag = "synergy";
            this.synergyBtn.Text = "Synergy";
            this.synergyBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.synergyBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.synergyBtn.Visible = false;
            // 
            // separator3
            // 
            this.separator3.Id = "30c08f2c-1a8a-48af-b8b3-a199c45c2ca1";
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
            this.tabPage2.Size = new System.Drawing.Size(257, 193);
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
            this.textBox1.Id = "a6d447f7-8c6d-4bca-aafa-3754734ae39d";
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
            this.groupBox1.Id = "0f4430c6-15d1-4ded-aa44-ada151a6f6f3";
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
            this.button1.Id = "3f30c86a-9b5c-4ad1-afe0-b7372a8fb5e2";
            this.button1.Location = new System.Drawing.Point(205, 69);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(193, 21);
            this.button1.TabIndex = 40;
            this.button1.Text = "Rust";
            this.button1.WordWrap = true;
            // 
            // button4
            // 
            this.button4.Id = "f3c6c028-2b78-45ea-a5cf-e81a95ca4157";
            this.button4.Location = new System.Drawing.Point(205, 91);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(193, 21);
            this.button4.TabIndex = 39;
            this.button4.Text = "Synergy";
            this.button4.WordWrap = true;
            // 
            // button7
            // 
            this.button7.Id = "83cd313f-936e-41a2-b658-de2df5579729";
            this.button7.Location = new System.Drawing.Point(205, 3);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(193, 21);
            this.button7.TabIndex = 37;
            this.button7.Text = "Hurtworld";
            this.button7.WordWrap = true;
            // 
            // button23
            // 
            this.button23.Id = "41428d64-3bb0-4046-8fd2-f215769fa61c";
            this.button23.Location = new System.Drawing.Point(205, 25);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(193, 21);
            this.button23.TabIndex = 36;
            this.button23.Text = "Garry\'s Mod";
            this.button23.WordWrap = true;
            // 
            // button25
            // 
            this.button25.Id = "5184cb6c-6560-453e-af82-d16330df2019";
            this.button25.Location = new System.Drawing.Point(205, 47);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(193, 21);
            this.button25.TabIndex = 35;
            this.button25.Text = "Counter-Strike 1.6";
            this.button25.WordWrap = true;
            // 
            // button3
            // 
            this.button3.Id = "5676bd23-c17a-435f-a4a5-860ec45a5914";
            this.button3.Location = new System.Drawing.Point(0, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 0;
            // 
            // button6
            // 
            this.button6.Id = "fe3d17eb-8c9e-4618-bacf-d2a38230d862";
            this.button6.Location = new System.Drawing.Point(0, 0);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 0;
            // 
            // button21
            // 
            this.button21.Id = "0b92fe5f-2c23-4274-9b43-42f86e389277";
            this.button21.Location = new System.Drawing.Point(0, 0);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(75, 23);
            this.button21.TabIndex = 0;
            // 
            // button24
            // 
            this.button24.Id = "89c1d9c8-d078-40b6-a4ce-d358bc96c4e6";
            this.button24.Location = new System.Drawing.Point(0, 0);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(75, 23);
            this.button24.TabIndex = 0;
            // 
            // button26
            // 
            this.button26.Id = "2ee3d875-7a5d-410a-ba8e-74a813b014e9";
            this.button26.Location = new System.Drawing.Point(0, 0);
            this.button26.Name = "button26";
            this.button26.Size = new System.Drawing.Size(75, 23);
            this.button26.TabIndex = 0;
            // 
            // button27
            // 
            this.button27.Id = "788071e9-8f4a-47c5-b0e8-7d0513eef2aa";
            this.button27.Location = new System.Drawing.Point(0, 0);
            this.button27.Name = "button27";
            this.button27.Size = new System.Drawing.Size(75, 23);
            this.button27.TabIndex = 0;
            // 
            // button28
            // 
            this.button28.Id = "b1100076-1cb0-4f8d-9914-d76c03f015d4";
            this.button28.Location = new System.Drawing.Point(0, 0);
            this.button28.Name = "button28";
            this.button28.Size = new System.Drawing.Size(75, 23);
            this.button28.TabIndex = 0;
            // 
            // button29
            // 
            this.button29.Id = "540f4055-94eb-4f71-8294-7b59a213843e";
            this.button29.Location = new System.Drawing.Point(0, 0);
            this.button29.Name = "button29";
            this.button29.Size = new System.Drawing.Size(75, 23);
            this.button29.TabIndex = 0;
            // 
            // button30
            // 
            this.button30.Id = "213fe207-3f50-4ce7-ac69-859875f4dd05";
            this.button30.Location = new System.Drawing.Point(0, 0);
            this.button30.Name = "button30";
            this.button30.Size = new System.Drawing.Size(75, 23);
            this.button30.TabIndex = 0;
            // 
            // updtTimer
            // 
            this.updtTimer.Enabled = true;
            this.updtTimer.Interval = 1300;
            this.updtTimer.Tick += new System.EventHandler(this.updtTimer_Tick);
            // 
            // separator2
            // 
            this.separator2.Id = "fb7ba343-ae55-4d35-96aa-80c376b13bf0";
            this.separator2.Location = new System.Drawing.Point(0, 0);
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(100, 23);
            this.separator2.TabIndex = 0;
            // 
            // srvPanelGrpBox
            // 
            this.srvPanelGrpBox.Controls.Add(this.groupBox7);
            this.srvPanelGrpBox.Controls.Add(this.extraGrpBox);
            this.srvPanelGrpBox.Controls.Add(this.groupBox5);
            this.srvPanelGrpBox.Controls.Add(this.gameImgPicBox);
            this.srvPanelGrpBox.Controls.Add(this.actionsGrpBox);
            this.srvPanelGrpBox.Id = "495c98b0-9f89-42c4-adb6-15450ca4f11f";
            this.srvPanelGrpBox.Location = new System.Drawing.Point(1, 0);
            this.srvPanelGrpBox.Name = "srvPanelGrpBox";
            this.srvPanelGrpBox.Size = new System.Drawing.Size(360, 257);
            this.srvPanelGrpBox.TabIndex = 25;
            this.srvPanelGrpBox.Text = "SELECT A GAME ";
            this.srvPanelGrpBox.Visible = false;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label2);
            this.groupBox7.Controls.Add(this.srvTxtBox);
            this.groupBox7.Id = "8c7f9d00-ee88-477f-92c8-e941e83f2e1a";
            this.groupBox7.Location = new System.Drawing.Point(130, 123);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(107, 68);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.Text = "Server Info";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(40, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Size:";
            // 
            // srvTxtBox
            // 
            this.srvTxtBox.Id = "f38f7882-e9bd-4ba0-8ba4-b803e38b2d73";
            this.srvTxtBox.Location = new System.Drawing.Point(14, 32);
            this.srvTxtBox.Name = "srvTxtBox";
            this.srvTxtBox.ReadOnly = true;
            this.srvTxtBox.Size = new System.Drawing.Size(80, 21);
            this.srvTxtBox.TabIndex = 1;
            this.srvTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.srvTxtBox.TextEditorWidth = 74;
            this.srvTxtBox.UseVisualThemeForForeground = false;
            // 
            // extraGrpBox
            // 
            this.extraGrpBox.Controls.Add(this.extraBtn4);
            this.extraGrpBox.Controls.Add(this.extraBtn3);
            this.extraGrpBox.Controls.Add(this.extraBtn2);
            this.extraGrpBox.Controls.Add(this.extraBtn1);
            this.extraGrpBox.Controls.Add(this.instGuideBtn);
            this.extraGrpBox.Id = "5985e57f-53ed-4dc0-b11e-99e130e0cf2d";
            this.extraGrpBox.Location = new System.Drawing.Point(3, 123);
            this.extraGrpBox.Name = "extraGrpBox";
            this.extraGrpBox.Size = new System.Drawing.Size(126, 134);
            this.extraGrpBox.TabIndex = 5;
            this.extraGrpBox.Text = "Extra";
            // 
            // extraBtn4
            // 
            this.extraBtn4.CommandName = "extraBtn4Cmd";
            this.extraBtn4.Id = "3ca315cb-92ef-4582-8a7b-35c6dc870a83";
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
            this.extraBtn3.Id = "dc9b6e34-c85b-4ca1-bc85-232ab5642c42";
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
            this.extraBtn2.Id = "9f447427-ab32-4fe4-a753-7d1639316947";
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
            this.extraBtn1.Id = "1608facb-1659-4f3d-8500-cf891f38bd91";
            this.extraBtn1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extraBtn1.Location = new System.Drawing.Point(3, 37);
            this.extraBtn1.Name = "extraBtn1";
            this.extraBtn1.Size = new System.Drawing.Size(120, 23);
            this.extraBtn1.TabIndex = 4;
            this.extraBtn1.Text = "extraBtn1";
            // 
            // instGuideBtn
            // 
            this.instGuideBtn.CommandName = "instGuideSrv";
            this.instGuideBtn.Id = "19f15f10-a024-445e-8977-169184e2552c";
            this.instGuideBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.instGuideBtn.Location = new System.Drawing.Point(3, 13);
            this.instGuideBtn.Name = "instGuideBtn";
            this.instGuideBtn.Size = new System.Drawing.Size(120, 23);
            this.instGuideBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._1091)});
            this.instGuideBtn.TabIndex = 2;
            this.instGuideBtn.Text = "   Installation Guide";
            this.instGuideBtn.Click += new System.EventHandler(this.instGuideBtn_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.installSvrBtn);
            this.groupBox5.Id = "1b4fcd40-0061-4997-b0f0-5d337b6c91d2";
            this.groupBox5.Location = new System.Drawing.Point(3, 83);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(115, 40);
            this.groupBox5.TabIndex = 4;
            // 
            // installSvrBtn
            // 
            this.installSvrBtn.CommandName = "InstallServerBtn";
            this.installSvrBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.installSvrBtn.ForeColor = System.Drawing.Color.Green;
            this.installSvrBtn.Id = "5062984b-2591-46f3-bfda-68a0e57989a8";
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
            this.gameImgPicBox.Location = new System.Drawing.Point(17, 12);
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
            this.actionsGrpBox.Id = "319350b1-cc98-4d8f-8f7b-3c1069d1824a";
            this.actionsGrpBox.Location = new System.Drawing.Point(238, 81);
            this.actionsGrpBox.Name = "actionsGrpBox";
            this.actionsGrpBox.Size = new System.Drawing.Size(119, 138);
            this.actionsGrpBox.TabIndex = 2;
            this.actionsGrpBox.Text = "Actions";
            // 
            // act_Btn1
            // 
            this.act_Btn1.CommandName = "deleteSrv";
            this.act_Btn1.Id = "a9d7e1fe-3a41-4506-b784-10caa0b6eced";
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
            this.editSettingsDropBtn.Id = "972bdc11-b09c-4478-9123-1a8e507fcffa";
            this.editSettingsDropBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.editSettingsDropBtn.Location = new System.Drawing.Point(4, 111);
            this.editSettingsDropBtn.Name = "editSettingsDropBtn";
            this.editSettingsDropBtn.Popup = this.popupMenu4;
            this.editSettingsDropBtn.Size = new System.Drawing.Size(112, 23);
            this.editSettingsDropBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._1645)});
            this.editSettingsDropBtn.TabIndex = 8;
            this.editSettingsDropBtn.Text = "    Edit settings";
            this.editSettingsDropBtn.WordWrap = true;
            this.editSettingsDropBtn.Click += new System.EventHandler(this.editSettingsDropBtn_Click);
            // 
            // popupMenu4
            // 
            this.popupMenu4.Items.AddRange(new System.Windows.Forms.Control[] {
            this.easyStgBtn,
            this.button10,
            this.button12,
            this.button13,
            this.button14});
            this.popupMenu4.KeepPopupsWithOffsetPlacementWithinPlacementArea = false;
            this.popupMenu4.PlacementMode = Elegant.Ui.PopupPlacementMode.Bottom;
            this.popupMenu4.Size = new System.Drawing.Size(100, 100);
            // 
            // easyStgBtn
            // 
            this.easyStgBtn.CommandName = "easyStgBtnCmd";
            this.easyStgBtn.Id = "113e9c01-0247-46e0-af12-86910ed83a52";
            this.easyStgBtn.Location = new System.Drawing.Point(2, 2);
            this.easyStgBtn.Name = "easyStgBtn";
            this.easyStgBtn.Size = new System.Drawing.Size(160, 23);
            this.easyStgBtn.TabIndex = 3;
            this.easyStgBtn.Tag = "stgDrp_easyStgEditBtn";
            this.easyStgBtn.Text = "Easy settings editor";
            this.easyStgBtn.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.CommandName = "editConfSrv";
            this.button10.Id = "46ae873a-fca9-4baf-bdc5-1c60db31f3ad";
            this.button10.Location = new System.Drawing.Point(2, 25);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(160, 23);
            this.button10.TabIndex = 4;
            this.button10.Tag = "stgDrp";
            this.button10.Text = "button10";
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button12
            // 
            this.button12.CommandName = "editConfSrv";
            this.button12.Id = "7b8e9bd9-eeb6-4eb5-8d34-aacac73367db";
            this.button12.Location = new System.Drawing.Point(2, 48);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(160, 23);
            this.button12.TabIndex = 6;
            this.button12.Tag = "stgDrp";
            this.button12.Text = "button12";
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.CommandName = "editConfSrv";
            this.button13.Id = "8963af42-003d-4d9f-9d30-a2d76b0160cd";
            this.button13.Location = new System.Drawing.Point(2, 71);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(160, 23);
            this.button13.TabIndex = 7;
            this.button13.Tag = "stgDrp";
            this.button13.Text = "button13";
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button14
            // 
            this.button14.Id = "15f7006c-dfb3-4774-aa87-3d4737721a08";
            this.button14.Location = new System.Drawing.Point(2, 94);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(160, 23);
            this.button14.TabIndex = 9;
            this.button14.Text = "button14";
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // reinstBtn
            // 
            this.reinstBtn.CommandName = "reinstallSrv";
            this.reinstBtn.Id = "01dbccaa-503a-4133-9a0f-9d44c8d4f052";
            this.reinstBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.reinstBtn.Location = new System.Drawing.Point(4, 39);
            this.reinstBtn.Name = "reinstBtn";
            this.reinstBtn.Size = new System.Drawing.Size(112, 23);
            this.reinstBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._2233)});
            this.reinstBtn.TabIndex = 4;
            this.reinstBtn.Text = " Reinstall";
            // 
            // openFoldBtn
            // 
            this.openFoldBtn.CommandName = "openFoldSrv";
            this.openFoldBtn.Id = "b9182e27-e47b-4e8f-ade5-91cb9ceed3cb";
            this.openFoldBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.openFoldBtn.Location = new System.Drawing.Point(4, 63);
            this.openFoldBtn.Name = "openFoldBtn";
            this.openFoldBtn.Size = new System.Drawing.Size(112, 23);
            this.openFoldBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._468)});
            this.openFoldBtn.TabIndex = 1;
            this.openFoldBtn.Text = " Open folder";
            this.openFoldBtn.Click += new System.EventHandler(this.openFoldBtn_Click);
            // 
            // updtSvrBtn
            // 
            this.updtSvrBtn.CommandName = "updateSrv";
            this.updtSvrBtn.Id = "78fbd6dc-8cbf-4b00-bf0c-2948e0e71fe8";
            this.updtSvrBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.updtSvrBtn.Location = new System.Drawing.Point(4, 15);
            this.updtSvrBtn.Name = "updtSvrBtn";
            this.updtSvrBtn.Size = new System.Drawing.Size(112, 23);
            this.updtSvrBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources.refresh16x16)});
            this.updtSvrBtn.TabIndex = 0;
            this.updtSvrBtn.Text = " Update";
            // 
            // delBtn
            // 
            this.delBtn.CommandName = "deleteSrv";
            this.delBtn.Id = "dfb0f3ea-d8de-4953-9596-f2e086b8574c";
            this.delBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.delBtn.Location = new System.Drawing.Point(4, 87);
            this.delBtn.Name = "delBtn";
            this.delBtn.Size = new System.Drawing.Size(112, 23);
            this.delBtn.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Server_Creation_Tool.Properties.Resources._1263)});
            this.delBtn.TabIndex = 2;
            this.delBtn.Tag = "sgtDrp";
            this.delBtn.Text = " Delete";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.srvPanelGrpBox);
            this.panel1.Controls.Add(this.button16);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.basicInstrTxtBox);
            this.panel1.Location = new System.Drawing.Point(267, 148);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(362, 257);
            this.panel1.TabIndex = 26;
            this.panel1.Text = "panel1";
            // 
            // button16
            // 
            this.button16.Id = "37e59111-0b26-4a97-aa3a-8e66f64b86fd";
            this.button16.Location = new System.Drawing.Point(9, 77);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(109, 39);
            this.button16.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("button16.SmallImages.Images"))))});
            this.button16.TabIndex = 33;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Id = "090d8a8c-eb49-4cc2-895b-6ffc57ab53d0";
            this.groupBox3.Location = new System.Drawing.Point(3, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(355, 69);
            this.groupBox3.TabIndex = 31;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label4.Location = new System.Drawing.Point(17, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(317, 25);
            this.label4.TabIndex = 29;
            this.label4.Text = "Welcome to Server Creation Tool 3.1";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label5.Location = new System.Drawing.Point(34, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(281, 39);
            this.label5.TabIndex = 30;
            this.label5.Text = "This tool allows you to download, install and manage\r\n      steam and non-steam s" +
    "ervers quick and easy.";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label3.Location = new System.Drawing.Point(135, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 15);
            this.label3.TabIndex = 28;
            this.label3.Text = "Basic Instructions:";
            // 
            // basicInstrTxtBox
            // 
            this.basicInstrTxtBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.basicInstrTxtBox.Id = "8f75761a-4259-4537-aecd-69480e8ac8ab";
            this.basicInstrTxtBox.Location = new System.Drawing.Point(7, 119);
            this.basicInstrTxtBox.Multiline = true;
            this.basicInstrTxtBox.Name = "basicInstrTxtBox";
            this.basicInstrTxtBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.basicInstrTxtBox.Size = new System.Drawing.Size(348, 118);
            this.basicInstrTxtBox.TabIndex = 27;
            this.basicInstrTxtBox.Text = resources.GetString("basicInstrTxtBox.Text");
            this.basicInstrTxtBox.TextEditorWidth = 344;
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
            // editServerSettigsToolStripMenuItem
            // 
            this.editServerSettigsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("editServerSettigsToolStripMenuItem.Image")));
            this.editServerSettigsToolStripMenuItem.Name = "editServerSettigsToolStripMenuItem";
            this.editServerSettigsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.editServerSettigsToolStripMenuItem.Text = "Edit Server Settings";
            // 
            // logTimer
            // 
            this.logTimer.Enabled = true;
            this.logTimer.Interval = 10000;
            this.logTimer.Tick += new System.EventHandler(this.logTimer_Tick);
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
            ((System.ComponentModel.ISupportInitialize)(this.langRibbon)).EndInit();
            this.langRibbon.ResumeLayout(false);
            this.langRibbon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonGroup1)).EndInit();
            this.ribbonGroup1.ResumeLayout(false);
            this.ribbonGroup1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu3)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.currentOperRibGrp)).EndInit();
            this.currentOperRibGrp.ResumeLayout(false);
            this.currentOperRibGrp.PerformLayout();
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
            this.srvPanelGrpBox.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.extraGrpBox.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gameImgPicBox)).EndInit();
            this.actionsGrpBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu4)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private Elegant.Ui.RibbonTabPage langRibbon;
        private Elegant.Ui.RibbonGroup ribbonGroup1;
        private Elegant.Ui.Button changeLogBtn;
        private Elegant.Ui.Button button5;
        private Elegant.Ui.Button serverGroupBtn;
        private Elegant.Ui.Separator separator1;
        private Elegant.Ui.Button plannedFeaturesBtn;
        private Elegant.Ui.Button aboutBtn;
        private Elegant.Ui.DropDown dropDown1;
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
        private Elegant.Ui.Button changeSrvrInstFoldBtn;
        private Elegant.Ui.Button openServersFoldBtn;
        private Elegant.Ui.RibbonContextualTabGroup noInternetTab;
        private Elegant.Ui.RibbonGroup ribbonGroup6;
        private Elegant.Ui.Button refreshConnBtn;
        private Elegant.Ui.TextBox noInternetTxtBox;
        private Elegant.Ui.TabControl tabControl1;
        private Elegant.Ui.TabPage tabPage1;
        private Elegant.Ui.TabPage tabPage2;
        private Elegant.Ui.Button button32;
        private Elegant.Ui.RibbonTabPage noInternetTab2;
        private System.Windows.Forms.Timer updtTimer;
        private Elegant.Ui.NavigationBar steamSvrNavBar;
        private Elegant.Ui.ToggleButton daysToDieBtn;
        private Elegant.Ui.ToggleButton CSsourceBtn;
        private Elegant.Ui.ToggleButton KillingFloorTwoBtn;
        private Elegant.Ui.ToggleButton SvenCoopBtn;
        private Elegant.Ui.ToggleButton arkBtn;
        private Elegant.Ui.ToggleButton CSGOBtn;
        private Elegant.Ui.ToggleButton CODblackOpsThreeBtn;
        private Elegant.Ui.ToggleButton l4dBtn;
        private Elegant.Ui.ToggleButton l4d2Btn;
        private Elegant.Ui.ToggleButton hurtWorldBtn;
        private Elegant.Ui.ToggleButton garrysModBtn;
        private Elegant.Ui.ToggleButton csOnePointsixBtn;
        private Elegant.Ui.ToggleButton RustBtn;
        private Elegant.Ui.ToggleButton synergyBtn;
        private Elegant.Ui.Label label1;
        private Elegant.Ui.Label searchSteamGameLbl;
        private Elegant.Ui.TextBox steamGameSrchBox;
        private Elegant.Ui.Separator separator2;
        private Elegant.Ui.GroupBox srvPanelGrpBox;
        private Elegant.Ui.GroupBox actionsGrpBox;
        private System.Windows.Forms.PictureBox gameImgPicBox;
        private Elegant.Ui.Button openFoldBtn;
        private Elegant.Ui.Button updtSvrBtn;
        private Elegant.Ui.GroupBox extraGrpBox;
        private Elegant.Ui.GroupBox groupBox5;
        private Elegant.Ui.Button delBtn;
        private Elegant.Ui.TextBox srvTxtBox;
        private Elegant.Ui.Label label2;
        private Elegant.Ui.Button instGuideBtn;
        private Elegant.Ui.GroupBox groupBox7;
        private Elegant.Ui.Button reinstBtn;
        private Elegant.Ui.DropDown dropDown3;
        private Elegant.Ui.PopupMenu popupMenu3;
        private Elegant.Ui.Button button34;
        private Elegant.Ui.Panel panel1;
        private Elegant.Ui.Button installSvrBtn;
        private Elegant.Ui.Separator separator3;
        private Elegant.Ui.ToggleButton welcomeBtn;
        private Elegant.Ui.Label label3;
        private Elegant.Ui.TextBox basicInstrTxtBox;
        private Elegant.Ui.GroupBox groupBox3;
        private Elegant.Ui.Label label4;
        private Elegant.Ui.Label label5;
        private Elegant.Ui.RibbonGroup currentOperRibGrp;
        private Elegant.Ui.Label currentOperNameLbl;
        private Elegant.Ui.ProgressBar progressBar1;
        private System.Windows.Forms.Timer nonSteamServerTimer;
        private Elegant.Ui.DropDown editSettingsDropBtn;
        private Elegant.Ui.ContextMenuExtenderProvider contextMenuExtenderProvider1;
        private Elegant.Ui.PopupMenu popupMenu4;
        private Elegant.Ui.Button easyStgBtn;
        private Elegant.Ui.Button button10;
        private Elegant.Ui.Button button12;
        private Elegant.Ui.Button button13;
        private Elegant.Ui.RibbonGroup optionsRibbon;
        private Elegant.Ui.Button check4UpdatesBtn;
        private Elegant.Ui.DropDown dropDown2;
        private Elegant.Ui.PopupMenu popupMenu2;
        private Elegant.Ui.Button button2;
        private Elegant.Ui.Button button11;
        private Elegant.Ui.Button extraBtn2;
        private Elegant.Ui.Button extraBtn1;
        private Elegant.Ui.Button act_Btn1;
        private System.Windows.Forms.ToolTip toolTip1;
        private Elegant.Ui.Button button14;
        private Elegant.Ui.Button button8;
        private Elegant.Ui.Button button16;
        private Elegant.Ui.Button extraBtn4;
        private Elegant.Ui.Button extraBtn3;
        private Elegant.Ui.Button button9;
        private System.Windows.Forms.Timer logTimer;
    }
}
