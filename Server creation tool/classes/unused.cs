using Server_Creation_Tool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_creation_tool.classes
{
    internal class unused
    {
        private void addExtraBtn(string text, Action action, Image img = null, bool doubleTextRows = false, bool isLastBtn = false)//doubleTextRows does not do anything when the buttons are added in the contex menu strip, since it can dynamically adjust its length based on the longest button text
        {
            if (columnMakerExtr == null) columnMakerExtr = new SmoothBtnColumn();
            int btnHeight;
            if (doubleTextRows) btnHeight = 35;
            else btnHeight = instGuideBtn.Height;
            if (columnMakerExtr.lastAddedBtn != null)
            {
                if (columnMakerExtr.lastAddedBtn.Bottom + btnHeight >= extrPnl.Height - instGuideBtn.Height && (!isLastBtn || moreContexMenuExtr != null))//check if the next to be added button will come close to getting out of bounds. If yes add a "more" dropdown button and add the rest of the buttons in there
                {
                    if (!moreBtnAddedExtr)
                    {
                        moreContexMenuExtr = new contexMenuStripFrm();
                        moreContexMenuExtr.hideInsteadOfClose = true;//hide it instead of closing it because after closing it we would have to add all the buttons again and in this case we can't
                        customSmoothBtn moreBtn = columnMakerExtr.createNextSmoothBtn(instGuideBtn, new Point(instGuideBtn.Location.X, instGuideBtn.Bottom), "                   More                ▼", null);
                        moreBtn.Click += (_, _2) => funcs.showMenuStripAtBtn(moreContexMenuExtr, moreBtn, this);
                        extrPnl.Controls.Add(moreBtn);
                        moreBtnAddedExtr = true;
                    }
                    moreContexMenuExtr.addBtn(text, action, !isLastBtn, img);
                    return;
                }
            }

            int startLocY;
            if (instGuideBtn.Visible == true) startLocY = instGuideBtn.Bottom;
            else startLocY = instGuideBtn.Location.Y;
            extrPnl.Controls.Add(columnMakerExtr.createNextSmoothBtn(instGuideBtn, new Point(instGuideBtn.Location.X, startLocY), text, action, img, btnHeight));
        }
        private void addActionBtn(string text, Action action, Image img = null, bool doubleTextRows = false, bool isLastBtn = false)//make the actions and extra functions INTO 1!!!!!
        {
            if (columnMakerAct == null) columnMakerAct = new SmoothBtnColumn();
            int btnHeight;
            if (doubleTextRows) btnHeight = 35;
            else btnHeight = editStgBtn.Height;
            if (columnMakerAct.lastAddedBtn != null)
            {
                if (columnMakerAct.lastAddedBtn.Bottom + btnHeight >= actPnl.Height - editStgBtn.Height && (!isLastBtn || moreContexMenuAct != null))//check if the next to be added button will come close to getting out of bounds. If yes add a "more" dropdown button and add the rest of the buttons in there
                {
                    if (!moreBtnAddedAct)
                    {
                        moreContexMenuAct = new contexMenuStripFrm();
                        moreContexMenuAct.hideInsteadOfClose = true;//hide it instead of closing it because after closing it we would have to add all the buttons again and in this case we can't
                        customSmoothBtn moreBtn = columnMakerAct.createNextSmoothBtn(editStgBtn, new Point(editStgBtn.Location.X, editStgBtn.Bottom), "                   More                ▼", null);
                        moreBtn.Click += (_, _2) => funcs.showMenuStripAtBtn(moreContexMenuAct, moreBtn, this);
                        actPnl.Controls.Add(moreBtn);
                        moreBtnAddedAct = true;
                    }
                    moreContexMenuAct.addBtn(text, action, !isLastBtn, img);
                    return;
                }
            }

            int startLocY = editStgBtn.Bottom;
            actPnl.Controls.Add(columnMakerAct.createNextSmoothBtn(editStgBtn, new Point(editStgBtn.Location.X, startLocY), text, action, img, btnHeight));
        }
    }
}
