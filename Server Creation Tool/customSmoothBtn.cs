using System;
using System.Drawing;
using System.Windows.Forms;
using Transitions;

namespace Server_Creation_Tool
{
    public partial class customSmoothBtn : Button
    {
        public customSmoothBtn()
        {

            InitializeComponent();
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
        private Color _normalColor = Color.Transparent;
        private Color _hoverColor;
        private Color _PressedColor;
        private Color _BackgroundBackColor;
        private int _transSpeed = 130;
        private int _clickTransSpeed = 80;
        public bool _smoothTrans = true;

        public Color ColorNormal
        {
            get { return _normalColor; }
            set { _normalColor = value; }
        }
        public Color ColorHover
        {
            get { return _hoverColor; }
            set { _hoverColor = value; }
        }
        public Color ColorPressed
        {
            get { return _PressedColor; }
            set { _PressedColor = value; }
        }
        public int TransitionSpeed
        {
            get { return _transSpeed; }
            set { _transSpeed = value; }
        }
        public int ClickTransitionSpeed
        {
            get { return _clickTransSpeed; }
            set { _clickTransSpeed = value; }
        }
        public Color BackgroundBackColor
        {
            get { return _BackgroundBackColor; }
            set { _BackgroundBackColor = value; }
        }
        public bool EnableSmoothTransitions
        {
            get { return _smoothTrans; }
            set { _smoothTrans = value; }
        }


        //THIS IS CURRENTLY UNSUED
        private Color increaseBrightnesst(Color color, double multiplier)//you can also darken it by putting a number below 1
        {

            Color c1 = color;
             Color c2 = Color.FromArgb(c1.A,
                (int)(c1.R * multiplier), (int)(c1.G * multiplier), (int)(c1.B * multiplier));
            return c2;
        }

        private void smoothBtn_MouseEnter(object sender, EventArgs e)
        {

            if (!_smoothTrans)
            { this.FlatAppearance.MouseOverBackColor = _hoverColor; return; }
            if (_normalColor == Color.Transparent)
            { this.FlatAppearance.MouseOverBackColor = _BackgroundBackColor; }
            else
            { this.FlatAppearance.MouseOverBackColor = _normalColor; }
            Transition.run(this.FlatAppearance, "MouseOverBackColor", _hoverColor, new TransitionType_Linear(_transSpeed));

        }

        private void smoothBtn_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_smoothTrans)
            { this.FlatAppearance.MouseDownBackColor = _PressedColor; return; }
            this.FlatAppearance.MouseDownBackColor = this.FlatAppearance.MouseOverBackColor;
            Transition.run(this.FlatAppearance, "MouseDownBackColor", _PressedColor, new TransitionType_Linear(_clickTransSpeed));
        }

        private void smoothBtn_MouseUp(object sender, MouseEventArgs e)
        {
            this.FlatAppearance.MouseOverBackColor = this.FlatAppearance.MouseDownBackColor;
            Transition.run(this.FlatAppearance, "MouseOverBackColor", _hoverColor, new TransitionType_Linear(_clickTransSpeed));
        }

        private void smoothBtn_MouseLeave(object sender, EventArgs e)
        {
            Color _c;
            if (!_smoothTrans)
            { this.BackColor = _normalColor; return; }
            if (_normalColor == Color.Transparent)
            {
                this.BackColor = _hoverColor;
                _c = _BackgroundBackColor;
            }
            else
            { this.BackColor = _hoverColor; _c = _normalColor; }
            Transition t2 = new Transition(new TransitionType_Linear(_transSpeed));
            t2.add(this, "BackColor", _c);
            t2.run();
            t2.TransitionCompletedEvent += new EventHandler<Transition.Args>(transComplete);
        }

        private void transComplete(object sender, Transition.Args e)
        {
            this.BackColor = _normalColor;
        }


    }
}
