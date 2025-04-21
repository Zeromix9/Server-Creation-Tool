using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;

namespace ScrollAblePanel
{
    //Properties to use by the Visual Studio Sesigner
    [Designer(typeof(ScrollAblePanelDesigner))] //Set the desiner to the custom ScrollAblePanel designer
    [Docking(DockingBehavior.Ask)]              //propts the user to dock the control
    public partial class ScrollAblePanelControl : UserControl
    {
        //VARIABLES
        private bool _IsMouseDown;          //If the mouse is down or not (Main Panel)
        private Point _LastMouseMove;       //The Position of the last mouse position recorded
        private bool _IsMouseVDown;         //if the mouse is down or not (Scroll Bar)
        private bool _DisableScrolling;     //if the panel is too small, turn off the scrolling          

        // Defines the property EditablePanel, where the scroll content can be edited
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Panel EditablePanel
        {
            get { return ScrollPanel; }
        }

        // ScrollAt Bar Size Control
        [Category("Appearance")]
        [Description("Gets or sets the size the scroll bar widget")]
        public int ScrollBarSize
        {
            get { return ScrollAt.Height; }
            set { 
                ScrollAt.Height = value;
                CalculateScrollBar();
            }
        }

        //CONSTRUCTOR
        public ScrollAblePanelControl()
        {
            InitializeComponent();



        }

        //METHODS
        private Point GetMousePosition()
        {
            //Returns the positon of the mouse within the screen
            return (this.PointToScreen(System.Windows.Forms.Control.MousePosition));
        }

        //CACULATIONS
        //Calculates the position of the scroll bar based off the position of the main panel
        private  void CalculateScrollBar()
        {
            //Get the Y position currently at the top of the panel, being looked at
            float CurrentlyLookingAt = Math.Abs(ScrollPanel.Location.Y);
            //Find out what percent it is through the document, getting rid of the height of the panel so we go from 0-100
            float Percent = (CurrentlyLookingAt / (ScrollPanel.Height - this.Height)) * 100;
            //get the maximum movement area up and down for the ScrollAt panel
            float ScrollMovementArea = ScrollContainter.Height - ScrollAt.Height;
            //Translate the percentage looked at to the percentage along the scroll bar
            ScrollAt.Location = new Point(ScrollAt.Location.X, Convert.ToInt32((ScrollMovementArea/100) * Percent) + ScrollContainter.Location.Y);
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
                _LastMouseMove = GetMousePosition();
            }
        }

        private void ScrollAblePanelControl_MouseMove(object sender, MouseEventArgs e)
        {
            //if the mouse is down, aka we're scrolling
            if (_IsMouseDown)
            {
                //grab the current mouse position and see if it has moved
                Point currentlMouse = GetMousePosition();
                if (_LastMouseMove != currentlMouse)
                {
                    //check if it would be going over the top of the panel
                    if (ScrollPanel.Location.Y + (currentlMouse.Y - _LastMouseMove.Y) > 0)
                    {
                        //if it is, set it to the top
                        ScrollPanel.Top = 0;
                    }
                    else
                    {
                        //check if it would be going past the bottom of the panel
                        if (ScrollPanel.Location.Y + (currentlMouse.Y - _LastMouseMove.Y) < (ScrollPanel.Height - this.Height) * -1)
                        {
                            //if it is, set it to the bottom
                            ScrollPanel.Location = new Point(ScrollPanel.Location.X, (ScrollPanel.Height - this.Height) * -1);
                        }
                        else
                        {
                            //other wise move it based off the change in mouse positon
                            ScrollPanel.Location = new Point(ScrollPanel.Location.X, ScrollPanel.Location.Y + (currentlMouse.Y - _LastMouseMove.Y));
                        }
                    }
                    //re-calculate the scroll bar based off our new main position
                    CalculateScrollBar();
                }
                //record the current mouse as the last mouse
                _LastMouseMove = GetMousePosition();
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

        private void ScrollPanel_MouseEnter(object sender, EventArgs e)
        {
            //If the mouse enters the panel, change the cursor so the user knows they can scroll
            Cursor = System.Windows.Forms.Cursors.Hand;
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
                _LastMouseMove = GetMousePosition();
            }
        }

        private void ScrollAt_MouseMove(object sender, MouseEventArgs e)
        {
            //if the mouse is down, aka we're scrolling with the bar
            if (_IsMouseVDown)
            {
                //grab the current mouse position and see if it has moved
                Point currentlMouse = GetMousePosition();
                if (_LastMouseMove != currentlMouse)
                {
                    //check if it would be going over the top of the scroll bar
                    if (ScrollAt.Location.Y + (currentlMouse.Y - _LastMouseMove.Y) < ScrollContainter.Location.Y)
                    {
                        //if it is, set it to the top
                        ScrollAt.Location = new Point(ScrollAt.Location.X, ScrollContainter.Location.Y);
                    }
                    else
                    {
                        //check if it would be going past the bottom of the scroll bar
                        if (ScrollAt.Location.Y + (currentlMouse.Y - _LastMouseMove.Y) > ScrollContainter.Height + ScrollContainter.Location.Y - ScrollAt.Height)
                        {
                            //if it is, set it to the bottom
                            ScrollAt.Location = new Point(ScrollAt.Location.X, ScrollContainter.Height + ScrollContainter.Location.Y - ScrollAt.Height);
                        }
                        else
                        {
                            //other wise move it based off the change in mouse positon
                            ScrollAt.Location = new Point(ScrollAt.Location.X, ScrollAt.Location.Y + (currentlMouse.Y - _LastMouseMove.Y));
                        }
                    }
                    //other wise move it based off the change in mouse positon
                    CalculateScrollPanel();
                }
                //record the current mouse as the last mouse
                _LastMouseMove = GetMousePosition();
            }
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
            ScrollContainter.Left = this.Width - ScrollContainter.Width - 4;
            ScrollAt.Left = this.Width - ScrollContainter.Width - 3;
            //Set the Height of the scroll bar
            ScrollContainter.Height = this.Height - (ScrollContainter.Location.Y * 2);

            //If the panel created to scroll is too small, turn off scrolling and disable the scroll bars.
            if (ScrollPanel.Height < this.Height)
            {
                _DisableScrolling = true;
                ScrollAt.Enabled = false;
                ScrollContainter.Enabled = false;
            }
            else
            {
                //otherwise allow scrolling
                _DisableScrolling = false;
                ScrollAt.Enabled = true;
                ScrollContainter.Enabled = true;
            }
        }

        private void ScrollAt_Paint(object sender, PaintEventArgs e)
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
