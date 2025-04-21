using Server_Creation_Tool;
using System;
using System.Drawing;

namespace Server_creation_tool.classes
{
    internal class SmoothBtnMaker
    {
        public customSmoothBtn lastAddedBtn = null;
        //startLoc Y coordinate only has a use when creating the first button. Then its not needed but its advised to leave it the same till you're finished adding buttons

        public customSmoothBtn createNextSmoothBtnUNUSED(customSmoothBtn templateBtn, Point startLoc, string text, Action action, Image img = null, int height = 0)//UNUSED this function creates a column of buttons of similar characteristics
        {
            customSmoothBtn btn = templateBtn.CloneCtrl();
            if (img != null) btn.Image = img;
            else btn.Image = null;
            btn.Text = text;
            btn.Name = "nextbtn";
            if (height != 0) btn.Height = height;//if its 0 it will remain with the height of the template
            if (lastAddedBtn == null)
            {
                btn.Location = new Point(startLoc.X, startLoc.Y);
            }
            else
            {
                btn.Top = lastAddedBtn.Bottom;
                btn.Name += lastAddedBtn.Location.Y;
            }
            if (action != null) btn.Click += (_, _2) => action();
            lastAddedBtn = btn;
            return btn;
        }
    }
}
