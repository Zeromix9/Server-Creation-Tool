using Server_Creation_Tool;
using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Server_creation_tool
{

    public partial class msgBox : Form
    {
        public msgBox()
        {
            InitializeComponent();
        }
        private string title;
        private string body;
        private Image icon = null;
        private Color backcolor = Color.Transparent;
        private Color forecolor = SystemColors.ControlLight;
        private MessageBoxButtons buttons = MessageBoxButtons.OK;
        private bool showInTaskbar = false;
        private bool dontShowOption = false;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Body
        {
            get { return body; }
            set { body = value; }
        }
        public Image MsgBoxIcon
        {
            get { return icon; }
            set { icon = value; }
        }
        public Color Backcolor
        {
            get { return backcolor; }
            set { backcolor = value; }
        }
        public Color Forecolor
        {
            get { return forecolor; }
            set { forecolor = value; }
        }
        public MessageBoxButtons Buttons
        {
            get { return buttons; }
            set { buttons = value; }
        }
        public bool Show_in_task_bar
        {
            get { return showInTaskbar; }
            set { showInTaskbar = value; }
        }
        public bool dont_show_again_checkbox
        {
            get { return dontShowOption; }
            set { dontShowOption = value; }
        }
        private void baseFormUsrCtrl1_Load(object sender, EventArgs e)
        {
            baseFormUsrCtrl1.Title = title;

        }

        private void msgBox_Load(object sender, EventArgs e)
        {
            //I removed the background images and put a color that looks close to that of the images because the images make the form look laggy when it shows up
            baseFormUsrCtrl1.panel2.BackgroundImage = null;
            baseFormUsrCtrl1.panel3.BackgroundImage = null;
            baseFormUsrCtrl1.panel4.BackgroundImage = null;
            Color c = Color.FromArgb(9, 19, 28);
            baseFormUsrCtrl1.panel2.BackColor = c;
            baseFormUsrCtrl1.panel3.BackColor = c;
            baseFormUsrCtrl1.panel4.BackColor = c;
            //--
            this.Text = title;
            baseFormUsrCtrl1.Title = title;
            bodyLbl.BackColor = backcolor;
            bodyLbl.ForeColor = forecolor;
            bodyLbl.Text = body;
            if (icon != null)
            { iconPicBox.Image = icon; }
            else { bodyLbl.Location = new Point(bodyLbl.Location.X - 32, bodyLbl.Location.Y); iconPicBox.Visible = false; }
            setWindowSize();
            iconPicBox.Location = new Point(iconPicBox.Location.X, (this.Height / 2) - 21);         
            btnContainer.Location = new Point((panel2.Width / 2) - (btnContainer.Width / 2) + 1 , btnContainer.Location.Y);
            if (dontShowOption)
            {
                if (btnContainer.Left <= chkBox.Right + 5)
                {
                    btnContainer.Left = chkBox.Right + 6;
                }
            }
            //   this.ShowInTaskbar = showInTaskbar;
        }

        public System.Windows.Forms.CheckBox chkBox;
        //use this to quickly create basic message boxes
        public DialogResult Show(Form parentform, string body1, string title1, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon1 = MessageBoxIcon.None, Image img = null)
        {
            // msgBox MsgBox = new msgBox();
            this.title = title1;
            this.body = body1;
            this.baseFormUsrCtrl1.Title = title1;
            //if there is an image, the MessageBoxIcon will be ignored
            SystemSound sound = null;
            if (img != null) this.icon = img;
            else
            {
                switch (icon1)
                {
                    case MessageBoxIcon.Information://value 64. Includes Asterisk
                        this.icon = SystemIcons.Information.ToBitmap();
                        sound = SystemSounds.Asterisk;
                        break;
                    case MessageBoxIcon.Warning: //value 48. Includes Exclamation
                        this.icon = SystemIcons.Warning.ToBitmap();
                        sound = SystemSounds.Exclamation;
                        break;
                    case MessageBoxIcon.Question: //value 32
                        this.icon = SystemIcons.Question.ToBitmap();
                        sound = SystemSounds.Asterisk; //SystemSounds.Question does not produce any sound so we will use the Asterisk
                        break;
                    case MessageBoxIcon.Error: //value 16. Includes Stop and Hand
                        this.icon = SystemIcons.Error.ToBitmap();
                        sound = SystemSounds.Hand;
                        break;
                    default:
                        this.icon = null;
                        break;
                }
            }

            addDefaultBtnLocal(this, buttons);
            if (dontShowOption)
            {
                chkBox = new CheckBox();
                chkBox.AutoSize = true;
                chkBox.Location = new System.Drawing.Point(7, 8);
                chkBox.Text = "Don't show again."; //DO NOT FORGET TO FIX THIS / translation = Nicht erneut anzeigen 
                chkBox.ForeColor = SystemColors.Control;
                chkBox.UseVisualStyleBackColor = true;
                panel2.Controls.Add(chkBox);
            }
            this.StartPosition = FormStartPosition.CenterParent;
            // MsgBox.Location = new Point(parentform.Location.X + (parentform.Width /2)+(MsgBox.Width /2), parentform.Location.Y + (parentform.Height /2)+ (MsgBox.Height / 2)); 
            this.Focus();
            if (sound != null) sound.Play();
            return this.ShowDialog();
        }
        private void setWindowSize()
        {
            Size originalSize = this.Size;
            int width = bodyLbl.Size.Width;
            int extraWidth = 0;
            if (iconPicBox.Visible == true)
            { extraWidth += 90; }
            else { extraWidth += 60; }
            if (dontShowOption)
            {
                if (btnContainer.Controls.Count == 2)
                {
                   // extraWidth += 40;
                }
                else if (btnContainer.Controls.Count == 3)
                {
                    extraWidth += 50;
                }

            }
            this.Width = width + extraWidth;
            this.MinimumSize = new Size(btnContainer.Width + 30, this.MinimumSize.Height);
            this.Height = bodyLbl.Size.Height + 100;
            //  this.Location = new Point(this.Location.X + originalSize.Width - this.Width,this.Location.Y + originalSize.Height - this.Height);
        }
        private void addDefaultBtnLocal(msgBox MsgBox, MessageBoxButtons buttons)
        {
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    addBtnLocal(MsgBox, "OK", SystemColors.ControlLight, () => MsgBox.DialogResult = DialogResult.OK);
                    break;
                case MessageBoxButtons.OKCancel:
                    addBtnLocal(MsgBox, "OK", SystemColors.ControlLight, () => MsgBox.DialogResult = DialogResult.OK);
                    addBtnLocal(MsgBox, "Cancel", SystemColors.ControlLight, () => MsgBox.DialogResult = DialogResult.Cancel);
                    break;
                case MessageBoxButtons.YesNo:
                    addBtnLocal(MsgBox, "Yes", SystemColors.ControlLight, () => MsgBox.DialogResult = DialogResult.Yes);
                    addBtnLocal(MsgBox, "No", SystemColors.ControlLight, () => MsgBox.DialogResult = DialogResult.No);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    addBtnLocal(MsgBox, "Yes", SystemColors.ControlLight, () => MsgBox.DialogResult = DialogResult.Yes);
                    addBtnLocal(MsgBox, "No", SystemColors.ControlLight, () => MsgBox.DialogResult = DialogResult.No);
                    addBtnLocal(MsgBox, "Cancel", SystemColors.ControlLight, () => MsgBox.DialogResult = DialogResult.Cancel);
                    break;
                case MessageBoxButtons.RetryCancel:
                    addBtnLocal(MsgBox, "Retry", SystemColors.ControlLight, () => MsgBox.DialogResult = DialogResult.Retry);
                    addBtnLocal(MsgBox, "Cancel", SystemColors.ControlLight, () => MsgBox.DialogResult = DialogResult.Cancel);
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    addBtnLocal(MsgBox, "Abort", SystemColors.ControlLight, () => MsgBox.DialogResult = DialogResult.Abort);
                    addBtnLocal(MsgBox, "Retry", SystemColors.ControlLight, () => MsgBox.DialogResult = DialogResult.Retry);
                    addBtnLocal(MsgBox, "Ignore", SystemColors.ControlLight, () => MsgBox.DialogResult = DialogResult.Ignore);
                    break;
            }
        }

        int nextBtnPos = 1;
        int pnlWidth = -1;
        //used locally for adding default buttons when the programmer uses the 'show' function
        private void addBtnLocal(msgBox MsgBox, string text, Color forecolor, Action clickAction = null, bool autosize = false)
        {
            customSmoothBtn Btn = new customSmoothBtn();
            Action act = () => { clickAction(); MsgBox.Close(); };
            Btn.Text = text;
            Btn.ColorHover = Color.FromArgb(59, 84, 119);
            Btn.ColorNormal = panel2.BackColor;
            Btn.ColorPressed = Color.FromArgb(76, 109, 155);
            Btn.BackColor = Color.Transparent;
            Btn.ForeColor = forecolor;
            Btn.AutoSize = autosize;
            Btn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Btn.Size = new Size(100, 22);
            Btn.Location = new Point(nextBtnPos, 1);
            Btn.Click += (_, _2) => act();
            nextBtnPos += Btn.Width + 5;
            pnlWidth += Btn.Width + 5;
            MsgBox.btnContainer.Width = pnlWidth;
            MsgBox.btnContainer.Controls.Add(Btn);
            MsgBox.btnContainer.BringToFront();
            Btn.BringToFront();
        }
        //for use when creating advanced messageboxes. The programmer can call it and add custom buttons
        public void addBtn(string text, Color forecolor, Action clickAction = null, bool autosize = false)
        {
            addBtnLocal(this, text, forecolor, clickAction, autosize);
        }
        public void addDefaultBtn(MessageBoxButtons buttons)
        {
            addDefaultBtnLocal(this, buttons);
        }

     
    }
}
