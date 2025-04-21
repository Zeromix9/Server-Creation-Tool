using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ScrollAblePanel
{

    [Designer(typeof(ScrollAblePanelDesigner))] //Set the desiner to the custom ScrollAblePanel designer
    [Docking(DockingBehavior.Ask)]              //propts the user to dock the control
    public partial class ScrollAblePanelControl : UserControl
    {
        //VARIABLES
        private bool _IsMouseDown;          //If the mouse is down or not (Main Panel)
        private Point _LastScrollAtMove;       //The Position of the last mouse position recorded
        private bool _IsMouseVDown;         //if the mouse is down or not (Scroll Bar)
        private bool _DisableScrolling;     //if the panel is too small, turn off the scrolling          

        // Defines the property EditablePanel, where the scroll content can be edited
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public FlowLayoutPanel EditablePanel
        {
            get { return ScrollPanel; }
        }

        // ScrollAt Bar Size Control
        [Category("Appearance")]
        [Description("Gets or sets the size the scroll bar widget")]
        public int ScrollBarSize
        {
            get { return ScrollAt.Height; }
            set
            {
                ScrollAt.Height = value;
                CalculateScrollBar();
            }
        }

        public ScrollAblePanelControl()
        {
            InitializeComponent();
        }

        private Point GetMousePosition()
        {
            //Returns the positon of the mouse within the screen
            return ScrollContainter.PointToClient(Control.MousePosition);
        }

        //CACULATIONS
        private void calcScrollBarHeight()
        {
            ScrollAt.Height = this.Height - 2;
            int calculatedHeight = ScrollAt.Height - ScrollPanel.Height / 6;
            if (calculatedHeight > this.Height / 2)
            {
                ScrollAt.Height = this.Height / 2;
            }
            else if (calculatedHeight < 30)
            {
                ScrollAt.Height = 30;
            }
            else
            {
                ScrollAt.Height = calculatedHeight;
            }
        }
        //Calculates the position of the scroll bar based off the position of the main panel
        private void CalculateScrollBar()
        {
            //Get the Y position currently at the top of the panel, being looked at
            float CurrentlyLookingAt = Math.Abs(ScrollPanel.Location.Y);
            //Find out what percent it is through the document, getting rid of the height of the panel so we go from 0-100
            float Percent = (CurrentlyLookingAt / (ScrollPanel.Height - this.Height)) * 100;
            //get the maximum movement area up and down for the ScrollAt panel
            float ScrollMovementArea = ScrollContainter.Height - ScrollAt.Height;
            //Translate the percentage looked at to the percentage along the scroll bar
            ScrollAt.Location = new Point(ScrollAt.Location.X, Convert.ToInt32((ScrollMovementArea / 100) * Percent) + ScrollContainter.Location.Y);
        }
        //Calculates the position of the main panel based off the position of the scroll bar
        private void CalculateScrollPanel()
        {
            //get the maximum movement area up and down for the ScrollAt panel
            float ScrollMovementArea = ScrollContainter.Height - ScrollAt.Height;
            //Find out how along the scroll bar we currently are
            float Percent = ((ScrollAt.Location.Y - ScrollContainter.Location.Y) / ScrollMovementArea) * 100;
            //Get the maximum movement area for the scroll panel
            float ScrollArea = (ScrollPanel.Height - this.Height);
            //Translate the percentage along the scroll bar to the percentage along the scroll panel
            ScrollPanel.Location = new Point(ScrollPanel.Location.X, Convert.ToInt32((ScrollArea / 100) * Percent) * -1);
        }


        //SCROLL PANEL EVENTS
        private void ScrollAblePanelControl_MouseDown(object sender, MouseEventArgs e)
        {
            //if the mouse is not already pressed and scrolling isnt not disabled
            if ((!_IsMouseDown) && (!_DisableScrolling))
            {
                //set the mouse to down (Main panel)
                _IsMouseDown = true;
                //Record the position of the mouse down
                _LastScrollAtMove = GetMousePosition();
            }
        }

        private void ScrollAblePanelControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (_IsMouseDown)
            {
                //finished scrolling
                _IsMouseDown = false;
            }
        }

        public void ScrollAblePanelControl_MouseWheel(object sender, MouseEventArgs e)
        {
            int i;
            if (e.Delta > 0)
            { i = -20; }
            else
            { i = 20; }
            int newScrollAtLocY = ScrollAt.Location.Y + i;
            if (newScrollAtLocY > 2 && newScrollAtLocY + ScrollAt.Size.Height < ScrollContainter.Height - 4)
            {
                ScrollAt.Location = new Point(ScrollAt.Location.X, newScrollAtLocY);
            }
            else
            {
                int x = ScrollContainter.Height / 4;
                if (ScrollAt.Location.Y <= x)
                {
                    ScrollAt.Location = new Point(ScrollAt.Location.X, 3);
                }
                if (ScrollAt.Bottom >= ScrollContainter.Height - x)
                {
                    ScrollAt.Location = new Point(ScrollAt.Location.X, ScrollContainter.Height - ScrollAt.Height + 4);
                }
            }

        }
        private void ScrollPanel_MouseEnter(object sender, EventArgs e)
        {
            //If the mouse enters the panel, change the cursor so the user knows they can scroll
            Cursor = System.Windows.Forms.Cursors.Hand;
            this.Focus();
        }

        private void ScrollPanel_MouseLeave(object sender, EventArgs e)
        {
            //restore the cursor to defualt when out of the panel
            Cursor = System.Windows.Forms.Cursors.Default;
        }

        //SCROLL BAR EVENTS
        private void ScrollAt_MouseDown(object sender, MouseEventArgs e)
        {
            //if the mouse is not already pressed and scrolling isnt not disabled
            if ((!_IsMouseVDown) && (!_DisableScrolling))
            {
                //set the mouse to down (Scroll Bars)
                _IsMouseVDown = true;
                //Record the position of the mouse down
                _LastScrollAtMove = ScrollAt.Location;
                lastMouseLocY = GetMousePosition().Y;
            }
        }

        int lastMouseLocY;
        private void ScrollAt_MouseMove(object sender, MouseEventArgs e)
        {
            int currentMousePosY = GetMousePosition().Y;
            //if the mouse is down, aka we're scrolling with the bar
            if (_IsMouseVDown)
            {
                int newScrollAtLocY = ScrollAt.Location.Y + (currentMousePosY - lastMouseLocY);
                if (newScrollAtLocY > 2 && newScrollAtLocY + ScrollAt.Size.Height < ScrollContainter.Height + 5)
                {
                    ScrollAt.Location = new Point(ScrollAt.Location.X, newScrollAtLocY);
                }
            }
            lastMouseLocY = GetMousePosition().Y;
        }

        public void scrollToTop()
        {
            ScrollAt.Location = new Point(ScrollAt.Location.X, 4);
        }
        private void ScrollAt_LocationChanged(object sender, EventArgs e)
        {
            //grab the current mouse position and see if it has moved
            Point currentScrollAtLoc = ScrollAt.Location;
            if (_LastScrollAtMove != currentScrollAtLoc)
            {
                CalculateScrollPanel();
            }
            //record the current mouse as the last mouse
            _LastScrollAtMove = ScrollAt.Location;

        }
        private void ScrollAt_MouseUp(object sender, MouseEventArgs e)
        {
            if (_IsMouseVDown)
            {
                //finished scrolling
                _IsMouseVDown = false;
            }
        }

        private void ScrollAblePanelControl_SizeChanged(object sender, EventArgs e)
        {
            //Set the X Position of the Scroll Bar
            ScrollContainter.Left = this.Width - ScrollContainter.Width + 0;
            ScrollAt.Left = this.Width - ScrollContainter.Width + 0;
            //Set the Height of the scroll bar
            ScrollContainter.Height = this.Height - (ScrollContainter.Location.Y * 2);

            //If the panel created to scroll is too small, turn off scrolling and disable the scroll bars.
            if (ScrollPanel.Height <= this.Height)
            {
                _DisableScrolling = true;
                ScrollAt.Enabled = false;
                ScrollAt.Visible = false;
                ScrollContainter.Enabled = false;
            }
            else
            {
                //otherwise allow scrolling
                _DisableScrolling = false;
                ScrollAt.Enabled = true;
                ScrollAt.Visible = true;
                ScrollContainter.Enabled = true;
            }
        }
        public void resizeScrollPnl(int height)
        {
            //check if the height var is smaller than the height of the scrollPnlSrvList panel. If it is smaller then dont allow the Editable panel to get any smaller than the height of the scrollPnlSrvList 
            if (height <= this.Height)
            {
                this.EditablePanel.Size = new Size(this.Width, this.Height);
                // panel.ScrollContainter.Visible = false;
            }
            else
            {
                this.EditablePanel.Size = new Size(this.Width - 11, height + 2);
               // this.ScrollContainter.Visible = true;
            }
        }
        private void ScrollAt_MouseEnter(object sender, EventArgs e)
        {
            scrollBarHover();
        }

        private void ScrollAt_MouseLeave(object sender, EventArgs e)
        {
            scrollBarNormal();
        }
        private void ScrollPanel_SizeChanged(object sender, EventArgs e)
        {
            ScrollAblePanelControl_SizeChanged(null, null);
            calcScrollBarHeight();
        }

        private void ScrollAblePanelControl_Load(object sender, EventArgs e)
        {
            calcScrollBarHeight();
            scrollBarNormal();
        }

        private void ScrollPanel_Paint(object sender, PaintEventArgs e)
        {

        }
        private void scrollBarHover()
        {
            scrollAtGraphicTop.BackgroundImage = Server_creation_tool.Properties.Resources.scrollBarTopHover;
            scrollAtGraphicMid.BackgroundImage = Server_creation_tool.Properties.Resources.scrollBarMidHover;
            scrollAtGraphicBottom.BackgroundImage = Server_creation_tool.Properties.Resources.scrollBarBottomHover;
        }
        private void scrollBarNormal()
        {
            scrollAtGraphicTop.BackgroundImage = Server_creation_tool.Properties.Resources.scrollBarTopNormal;
            scrollAtGraphicMid.BackgroundImage = Server_creation_tool.Properties.Resources.scrollBarMidNormal;
            scrollAtGraphicBottom.BackgroundImage = Server_creation_tool.Properties.Resources.scrollBarBottomNormal;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void scrollAtGraphicMid_Click(object sender, EventArgs e)
        {

        }

        private void ScrollContainter_Paint(object sender, PaintEventArgs e)
        {

        }
    }


    //The Desinger Class
    internal class ScrollAblePanelDesigner : System.Windows.Forms.Design.ParentControlDesigner
    {
        public override void Initialize(System.ComponentModel.IComponent component)
        {
            base.Initialize(component);

            if (this.Control is ScrollAblePanelControl)
            {
                this.EnableDesignMode((
                   //get the EditablePanel attritubute from the class ScrollAblePanelControl
                   (ScrollAblePanelControl)this.Control).EditablePanel, "EditablePanel");
            }
        }
    }

}
