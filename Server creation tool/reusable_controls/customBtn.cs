﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Server_Creation_Tool
{
    public partial class customBtn : PictureBox
    {
        public customBtn()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
        private Image NormalImage;
        private Image hoverImage;
        private Image DownImage;
        private bool toggled = false;
        public Image ImageNormal
        {
            get { return NormalImage; }
            set { NormalImage = value; }
        }
        public Image ImageHover
        {
            get { return hoverImage; }
            set { hoverImage = value; }
        }
        public Image imageDown
        {
            get { return DownImage; }
            set { DownImage = value; }
        }

        private void customBtn_MouseLeave(object sender, EventArgs e)
        {
            this.BackgroundImage = NormalImage;
        }

        private void customBtn_MouseEnter(object sender, EventArgs e)
        {
            this.BackgroundImage = hoverImage;
        }

        private void customBtn_MouseDown(object sender, MouseEventArgs e)
        {
            this.BackgroundImage = DownImage;
            this.Focus();
        }

        private void customBtn_MouseUp(object sender, MouseEventArgs e)
        {
            //use try because it crashes when the application exits after clicking the close button
            try
            {
                if (ClientRectangle.Contains(PointToClient(Control.MousePosition)))
                { this.BackgroundImage = hoverImage; }
                else
                { this.BackgroundImage = NormalImage; }
            }
            catch { }
        }
        public bool Toggled
        {
            get { return toggled; }
            set { toggled = value; }
        }


        private void customBtn_Click(object sender, EventArgs e)
        {
            if (Toggled) Toggled = false;
            else Toggled = true;
        }
    }
}
