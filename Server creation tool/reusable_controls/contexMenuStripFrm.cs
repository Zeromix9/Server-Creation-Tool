using Server_creation_tool.classes;
using Server_Creation_Tool;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Transitions;

namespace Server_creation_tool
{
    public partial class contexMenuStripFrm : Form
    {
        //enable shadow behind the menu strip
        private const int CS_DROPSHADOW = 0x20000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        public contexMenuStripFrm()
        {
            InitializeComponent();
        }
        funcsClass funcs = new funcsClass();
        private bool smoothTransitions = true;
        private bool mouseLeaveClose = false;
        private bool itsSubMenu = false;
        private bool hideNoClose = false;
        public contexMenuStripFrm parent = null;

        public bool smooth_transitions
        {
            get { return smoothTransitions; }
            set { smoothTransitions = value; }
        }
        public bool Close_on_mouse_leave
        {
            get { return mouseLeaveClose; }
            set { mouseLeaveClose = value; }
        }
        public bool Its_submenu
        {
            get { return itsSubMenu; }
            set { itsSubMenu = value; }
        }
        public bool hideInsteadOfClose
        {
            get { return hideNoClose; }
            set { hideNoClose = value; }
        }
        int maxWidth = 0;
        private void contexMenuStripFrm_Load(object sender, EventArgs e)
        {
            //reduce height by 9 beacuse for some reason even if i set the height to 0 in designer view the form during runtime has a minimum height and wont go to 0.
            this.Height -= 9;
            this.Icon = Properties.Resources.favicon__88_;
            //size the length of the menu strip accordingly to the length of the text of the buttons
            foreach (Control ctrl in this.Controls)
            {
                if (!(ctrl is PictureBox)) ctrl.Width = maxWidth + 10;
            }
            this.Width = maxWidth + 18;
            this.Focus();
        }
        int locX = 4;
        int locY = 4;
        
        contexMenuStripFrm _subMenu = null;
        int newControlCount = 0;
        private Color defaultForeColor = Color.FromArgb(175, 188, 192);
        private Color defaultNormalColor = Color.Transparent;
        private Color defaultHoverColor = Color.FromArgb(90, 96, 105);
        private Color defaultPressedColor = Color.FromArgb(90, 96, 105);
        public customSmoothBtn addBtn(string text, Action clickEvent, bool addSeparator = true, Image img = null, Color colorNormal = default, Color colorHover = default, Color fontColor = default, ContentAlignment textAlign = ContentAlignment.MiddleLeft, Func<contexMenuStripFrm, contexMenuStripFrm> subMenuCreator = null) //DO NOT USE THE LAST ARGUMENT, LEAVE ALWAYS EMPTY
        {
            customSmoothBtn  btn2 = new customSmoothBtn();
            btn2.BackgroundBackColor = this.BackColor;
            if (colorNormal == default)
            {
                colorNormal = defaultNormalColor;
                colorHover = defaultHoverColor;
            }
            btn2.BackColor = colorNormal;
            btn2.ColorHover = colorHover;
            btn2.ColorNormal = colorNormal;
            btn2.ColorPressed = colorHover;//
            int extrWidth = 0;
            if (img != null)
            {
                btn2.Image = img;
                extrWidth = 8;
            }
            btn2.EnableSmoothTransitions = smoothTransitions;
            btn2.TransitionSpeed = 90;
            btn2.ForeColor = defaultForeColor;
            if (fontColor == default)
            {
                fontColor = defaultForeColor;
            }
            btn2.ForeColor = fontColor;
            btn2.Size = new Size(159, 23);
            btn2.Font = new Font("Segoe UI", float.Parse("8,6"), FontStyle.Bold);
            btn2.TextAlign = textAlign;
            btn2.ImageAlign = ContentAlignment.MiddleLeft;
            btn2.Text = new string(' ', extrWidth) + text;
            btn2.Location = new Point(locX, locY);
            locY += btn2.Height + 1;
            newControlCount++;
            btn2.Name = "btn2" + newControlCount;
            this.Controls.Add(btn2);
            btn2.BringToFront();
            Size len = TextRenderer.MeasureText(btn2.Text, btn2.Font);
            if (addSeparator)
            {
                lineSeparator separator = new lineSeparator();
                separator.Size = btn2.Size;
                separator.Location = new Point(locX, locY - 1);
                this.Controls.Add(separator);
            }
            if (len.Width > maxWidth)
            { maxWidth = len.Width; }
            this.Height = btn2.Location.Y + 36;
            if (subMenuCreator == null)
            {
                btn2.MouseClick += (_, _2) => { clickEvent(); };
                btn2.Click += (_, _2) => closeAll();
            }
            else
            {
                //  _subMenu = subMenu;
                PictureBox arrow = new PictureBox();
                arrow.BackgroundImageLayout = ImageLayout.Stretch;
                arrow.BackColor = Color.Transparent;
                arrow.BackgroundImage = Properties.Resources.right_arrow;
                arrow.Name = "arrow" + newControlCount;
                btn2.Tag = arrow.Name;
                arrow.Tag = btn2.Name;
                this.Controls.Add(arrow);
                arrow.Height = 10;
                arrow.Width = 12;
                arrow.Location = new Point(btn2.Right + 8, btn2.Top + (btn2.Height / 2) / 2 + 2);
                arrow.BringToFront();
                arrow.Invalidate();
                btn2.MouseClick += (sender, e) => submenubtn2Arr_click(sender, e, subMenuCreator, colorNormal, colorHover);
                btn2.MouseEnter += (sender, e) => submenubtn2_enter(sender, e, colorNormal, colorHover);
                arrow.MouseClick += (sender, e) => submenubtn2Arr_click(sender, e, subMenuCreator, colorNormal, colorHover);
                arrow.MouseEnter += (sender, e) => submenubtn2Arr_click(sender, e, subMenuCreator, colorNormal, colorHover);
                //   arrow.MouseEnter += (_, _2) => { btn2.BackColor = colorHover; btn2.ColorNormal = colorHover; };
                if (mouseLeaveClose)
                {
                    //NOT WORKING PROPERLY
                    btn2.MouseLeave += (_, _2) => arrow.BackColor = Color.Transparent; this.close();
                    arrow.MouseLeave += (_, _2) => arrow.BackColor = Color.Transparent; this.close();// funcs.showMenuStripAtBtn(subMenu, btn2, this);
                }
                else
                {
                    btn2.MouseLeave += (sender, e) => submenubtnArrLeave(sender, e, colorNormal, colorHover);
                    //btn2.MouseLeave += (sender, e) => btnArrLeave(sender, e, colorNormal, colorHover);
                }
            }
            return btn2;
        }
        public void addSubMenuBtn(string text, Func<contexMenuStripFrm, contexMenuStripFrm> subMenuCreator = null, bool addSeparator = true, Image img = null, Color colorNormal = default, Color colorHover = default, Color fontColor = default, ContentAlignment textAlign = ContentAlignment.MiddleLeft)
        {
            addBtn(text, null, addSeparator, img, colorNormal, colorHover, fontColor, textAlign, subMenuCreator);
        }

        public void addLabel(string text, Color fontColor = default, Label customLbl = null)
        {
            Label lbl;
            if (customLbl == null)
            {
                lbl = new Label();
                if (fontColor == default)
                {
                    fontColor = defaultForeColor;
                }
                lbl.ForeColor = fontColor;
               // lbl.BackColor = Color.Transparent;
                lbl.Font = new Font("Segoe UI", float.Parse("8,6"), FontStyle.Bold);
            }
            else lbl = customLbl;
            lbl.Text = text;
            int extraSpace = 0;
            if (locY != 4)
            {
                extraSpace = 4;
            }
            Size len = TextRenderer.MeasureText(lbl.Text, lbl.Font);
            lbl.Location = new Point(locX, locY + extraSpace);
            locY += len.Height+ extraSpace;
            this.Controls.Add(lbl);
            lbl.BringToFront();
            if (len.Width > maxWidth)
            { maxWidth = len.Width; }
            this.Height = lbl.Location.Y + lbl.Height;
        }
        private void submenubtn2Arr_click(object sender, EventArgs e, Func<contexMenuStripFrm, contexMenuStripFrm> subMenuCreator, Color colorNormal, Color colorHover)
        {

            increaseclicksOnMenuCount();
            // clicksOnMenuCount++;
            foreach (Control btnC in this.Controls)
            {
                if (btnC is customSmoothBtn)
                {
                    (btnC as customSmoothBtn).BackColor = colorNormal;
                    (btnC as customSmoothBtn).ColorNormal = colorNormal;
                    try
                    {
                        (this.Controls[btnC.Tag.ToString()] as PictureBox).BackColor = colorNormal;
                    }
                    catch { }
                }
            }
            customSmoothBtn btn;
            PictureBox arr;
            if (sender is PictureBox)
            {
                btn = this.Controls[(sender as PictureBox).Tag.ToString()] as customSmoothBtn;
                arr = (sender as PictureBox);
            }
            else //if customsmoothBtn
            {
                btn = (sender as customSmoothBtn);
                arr = this.Controls[(sender as customSmoothBtn).Tag.ToString()] as PictureBox;
            }
            arr.BackColor = colorHover;
            btn.BackColor = colorHover;
            btn.ColorNormal = colorHover;
            try
            {
                if (_subMenu.Visible && sender is PictureBox) return;
            }
            catch { }
            try
            {
                _subMenu.close();
                _subMenu.Dispose();
                _subMenu = null;
            }
            catch { }
            _subMenu = subMenuCreator(this);
            funcs.showMenuStripAtBtn(_subMenu, btn, this, true, false, true);
        }
        private void submenubtn2_enter(object sender, EventArgs e, Color colorNormal, Color colorHover)
        {
            PictureBox arr = this.Controls[(sender as customSmoothBtn).Tag.ToString()] as PictureBox;
            arr.BackColor = colorHover;
            foreach (Control btnC in this.Controls)
            {
                if (btnC is customSmoothBtn)
                {
                    try
                    {
                        if (_subMenu.Visible) return;
                    }
                    catch { }
                    (btnC as customSmoothBtn).BackColor = colorNormal;
                    (btnC as customSmoothBtn).ColorNormal = colorNormal;
                }
            }
        }
        private void submenubtnArrLeave(object sender, EventArgs e, Color colorNormal, Color colorHover)
        {
            try//try because it might be null
            {
                if (!_subMenu.IsDisposed & _subMenu.Visible)
                { return; }
            }
            catch { }
            customSmoothBtn btn = (sender as customSmoothBtn);
            PictureBox arr = this.Controls[(sender as customSmoothBtn).Tag.ToString()] as PictureBox;
            arr.BackColor = colorNormal;
            btn.BackColor = colorNormal;
            btn.ColorNormal = colorNormal;
        }


        private void trans(PictureBox box, Color targetColor, int transSpeed)//UNUSED
        {
            Transition.run(box, "BackColor", targetColor, new TransitionType_Linear(transSpeed));
        }
        int clicksOnMenuCount = 0;
        int prevClicksOnMenuCount = 0;
        //close menu strip when user clicks somewhere else other than the menu strip
        private void contexMenuStripFrm_Deactivate(object sender, EventArgs e)
        {
            if (clicksOnMenuCount != prevClicksOnMenuCount)
            {
                prevClicksOnMenuCount = clicksOnMenuCount;
                Debug.WriteLine("sus");
                return;
            }
            close();

        }
        public void closeAll()
        {
            try
            { parent.closeAll(); }
            catch { }
            close();
        }
        public void closeAllIfClickedOutside()
        {

        }
        public void increaseclicksOnMenuCount()
        {
            try
            { parent.increaseclicksOnMenuCount(); }
            catch { }
            clicksOnMenuCount++;
        }
        private void close()
        {
            if (hideNoClose)
            {
                this.Hide();
                return;
            }
            //delay the closing of the form(aka contex menu strip) in order to make buttons and other controls work when you click them while the contex menu strip is open 
            this.Hide();
            funcs.StartThread(() =>
            {
                Thread.Sleep(100);
                try
                {
                    funcs.InvokeIfRequired(this, () =>
                    { this.Close(); this.Dispose(); });
                }
                catch { }
            });
        }

    }
}
