
namespace Server_Creation_Tool
{
    partial class aboutFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(aboutFrm));
            this.ytChannelLink = new System.Windows.Forms.LinkLabel();
            this.devsLbl = new System.Windows.Forms.Label();
            this.toolWasCreated = new System.Windows.Forms.Label();
            this.steamProfLink = new System.Windows.Forms.LinkLabel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.baseFormUsrCtrl1 = new Server_creation_tool.baseFormUsrCtrl();
            this.changeLogLbl = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.devNamesLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ytChannelLink
            // 
            this.ytChannelLink.AutoSize = true;
            this.ytChannelLink.BackColor = System.Drawing.Color.Transparent;
            this.ytChannelLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.ytChannelLink.Location = new System.Drawing.Point(128, 56);
            this.ytChannelLink.Name = "ytChannelLink";
            this.ytChannelLink.Size = new System.Drawing.Size(133, 20);
            this.ytChannelLink.TabIndex = 16;
            this.ytChannelLink.TabStop = true;
            this.ytChannelLink.Text = "Youtube Channel";
            this.ytChannelLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // devsLbl
            // 
            this.devsLbl.AutoSize = true;
            this.devsLbl.BackColor = System.Drawing.Color.Transparent;
            this.devsLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.devsLbl.Location = new System.Drawing.Point(275, 37);
            this.devsLbl.Name = "devsLbl";
            this.devsLbl.Size = new System.Drawing.Size(93, 20);
            this.devsLbl.TabIndex = 15;
            this.devsLbl.Text = "Developers:";
            // 
            // toolWasCreated
            // 
            this.toolWasCreated.AutoSize = true;
            this.toolWasCreated.BackColor = System.Drawing.Color.Transparent;
            this.toolWasCreated.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.toolWasCreated.ForeColor = System.Drawing.Color.Black;
            this.toolWasCreated.Location = new System.Drawing.Point(19, 36);
            this.toolWasCreated.Name = "toolWasCreated";
            this.toolWasCreated.Size = new System.Drawing.Size(242, 20);
            this.toolWasCreated.TabIndex = 14;
            this.toolWasCreated.Text = "This Tool was created by Zeromix";
            // 
            // steamProfLink
            // 
            this.steamProfLink.AutoSize = true;
            this.steamProfLink.BackColor = System.Drawing.Color.Transparent;
            this.steamProfLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.steamProfLink.Location = new System.Drawing.Point(19, 56);
            this.steamProfLink.Name = "steamProfLink";
            this.steamProfLink.Size = new System.Drawing.Size(104, 20);
            this.steamProfLink.TabIndex = 13;
            this.steamProfLink.TabStop = true;
            this.steamProfLink.Text = "Steam Profile";
            this.steamProfLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.linkLabel3.Location = new System.Drawing.Point(341, 57);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(105, 20);
            this.linkLabel3.TabIndex = 17;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "cookieoreo18";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // baseFormUsrCtrl1
            // 
            this.baseFormUsrCtrl1.BackColor = System.Drawing.Color.Transparent;
            this.baseFormUsrCtrl1.ControlBox = true;
            this.baseFormUsrCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.baseFormUsrCtrl1.Icon = global::Server_creation_tool.Properties.Resources._219121;
            this.baseFormUsrCtrl1.Location = new System.Drawing.Point(0, 0);
            this.baseFormUsrCtrl1.Minimize_Button = false;
            this.baseFormUsrCtrl1.Name = "baseFormUsrCtrl1";
            this.baseFormUsrCtrl1.parentForm = this;
            this.baseFormUsrCtrl1.Size = new System.Drawing.Size(530, 504);
            this.baseFormUsrCtrl1.TabIndex = 0;
            this.baseFormUsrCtrl1.Title = "About";
            this.baseFormUsrCtrl1.Load += new System.EventHandler(this.baseFormUsrCtrl1_Load);
            // 
            // changeLogLbl
            // 
            this.changeLogLbl.AutoSize = true;
            this.changeLogLbl.BackColor = System.Drawing.Color.Transparent;
            this.changeLogLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.changeLogLbl.Location = new System.Drawing.Point(14, 93);
            this.changeLogLbl.Name = "changeLogLbl";
            this.changeLogLbl.Size = new System.Drawing.Size(96, 20);
            this.changeLogLbl.TabIndex = 18;
            this.changeLogLbl.Text = "ChangeLog:";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.richTextBox1.Location = new System.Drawing.Point(16, 115);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(500, 373);
            this.richTextBox1.TabIndex = 19;
            this.richTextBox1.Text = "NOTE: To edit the changelog go to the resources and edit the \"changelog\" file. Th" +
    "e file must be in .rtf format.";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Location = new System.Drawing.Point(266, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(2, 50);
            this.panel1.TabIndex = 21;
            // 
            // devNamesLbl
            // 
            this.devNamesLbl.AutoSize = true;
            this.devNamesLbl.BackColor = System.Drawing.Color.Transparent;
            this.devNamesLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.devNamesLbl.Location = new System.Drawing.Point(275, 58);
            this.devNamesLbl.Name = "devNamesLbl";
            this.devNamesLbl.Size = new System.Drawing.Size(239, 20);
            this.devNamesLbl.TabIndex = 22;
            this.devNamesLbl.Text = "BasisBit,                          , Zeromix";
            // 
            // aboutFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(530, 504);
            this.Controls.Add(this.linkLabel3);
            this.Controls.Add(this.devNamesLbl);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.changeLogLbl);
            this.Controls.Add(this.ytChannelLink);
            this.Controls.Add(this.devsLbl);
            this.Controls.Add(this.toolWasCreated);
            this.Controls.Add(this.steamProfLink);
            this.Controls.Add(this.baseFormUsrCtrl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "aboutFrm";
            this.Text = "About";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.aboutFrm_FormClosing);
            this.Load += new System.EventHandler(this.aboutFrm_Load);
            this.TextChanged += new System.EventHandler(this.aboutFrm_TextChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Server_creation_tool.baseFormUsrCtrl baseFormUsrCtrl1;
        private System.Windows.Forms.LinkLabel ytChannelLink;
        private System.Windows.Forms.Label devsLbl;
        private System.Windows.Forms.Label toolWasCreated;
        private System.Windows.Forms.LinkLabel steamProfLink;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.Label changeLogLbl;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label devNamesLbl;
    }
}