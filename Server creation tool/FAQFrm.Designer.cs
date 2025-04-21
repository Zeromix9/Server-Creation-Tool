namespace Server_creation_tool
{
    partial class FAQFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FAQFrm));
            this.baseFormUsrCtrl1 = new Server_creation_tool.baseFormUsrCtrl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.okBtn = new Server_Creation_Tool.customSmoothBtn();
            this.FAQ1Lbl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.FAQ2Lbl = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.FAQ3Lbl = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.FAQ4Lbl = new System.Windows.Forms.Label();
            this.joinServerGrpLbl = new System.Windows.Forms.Label();
            this.discordSrvLinkLbl = new System.Windows.Forms.LinkLabel();
            this.steamGrpLinkLbl = new System.Windows.Forms.LinkLabel();
            this.FAQ1LinkLbl = new System.Windows.Forms.LinkLabel();
            this.FAQ1BODYLbl = new System.Windows.Forms.Label();
            this.FAQ2BODYLbl = new System.Windows.Forms.Label();
            this.FAQ3BODYLbl = new System.Windows.Forms.Label();
            this.FAQ4BODYLbl = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseFormUsrCtrl1
            // 
            this.baseFormUsrCtrl1.BackColor = System.Drawing.Color.Transparent;
            this.baseFormUsrCtrl1.ControlBox = true;
            this.baseFormUsrCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.baseFormUsrCtrl1.Icon = global::Server_creation_tool.Properties.Resources._150216;
            this.baseFormUsrCtrl1.Location = new System.Drawing.Point(0, 0);
            this.baseFormUsrCtrl1.Minimize_Button = false;
            this.baseFormUsrCtrl1.Name = "baseFormUsrCtrl1";
            this.baseFormUsrCtrl1.parentForm = this;
            this.baseFormUsrCtrl1.Size = new System.Drawing.Size(632, 417);
            this.baseFormUsrCtrl1.TabIndex = 0;
            this.baseFormUsrCtrl1.Title = "FAQ - Frequently Asked Questions";
            this.baseFormUsrCtrl1.Load += new System.EventHandler(this.baseFormUsrCtrl1_Load);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(47)))), ((int)(((byte)(67)))));
            this.panel2.Controls.Add(this.okBtn);
            this.panel2.Location = new System.Drawing.Point(10, 374);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(612, 33);
            this.panel2.TabIndex = 6;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.auto_color = false;
            this.okBtn.auto_color_dark_percent_press = 0.15F;
            this.okBtn.auto_color_light_percent_hover = 0.35F;
            this.okBtn.BackColor = System.Drawing.Color.Transparent;
            this.okBtn.BackgroundBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(71)))), ((int)(((byte)(104)))));
            this.okBtn.ClickTransitionSpeed = 60;
            this.okBtn.ColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(84)))), ((int)(((byte)(119)))));
            this.okBtn.ColorNormal = System.Drawing.Color.Transparent;
            this.okBtn.ColorPressed = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(109)))), ((int)(((byte)(155)))));
            this.okBtn.EnableSmoothTransitions = true;
            this.okBtn.FlatAppearance.BorderSize = 0;
            this.okBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.okBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.okBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.okBtn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.okBtn.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.okBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.okBtn.Location = new System.Drawing.Point(235, 4);
            this.okBtn.Margin = new System.Windows.Forms.Padding(0);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(136, 24);
            this.okBtn.TabIndex = 48;
            this.okBtn.Text = "OK";
            this.okBtn.Toggled = false;
            this.okBtn.TransitionSpeed = 130;
            this.okBtn.UseVisualStyleBackColor = false;
            this.okBtn.Click += new System.EventHandler(this.OpenFoldBtn_Click);
            // 
            // FAQ1Lbl
            // 
            this.FAQ1Lbl.AutoSize = true;
            this.FAQ1Lbl.BackColor = System.Drawing.Color.Transparent;
            this.FAQ1Lbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.FAQ1Lbl.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.FAQ1Lbl.Location = new System.Drawing.Point(35, 33);
            this.FAQ1Lbl.Name = "FAQ1Lbl";
            this.FAQ1Lbl.Size = new System.Drawing.Size(369, 17);
            this.FAQ1Lbl.TabIndex = 9;
            this.FAQ1Lbl.Text = "I can not see my server / No one can connect to my server";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label2.Location = new System.Drawing.Point(17, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "➤";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label3.Location = new System.Drawing.Point(17, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 17);
            this.label3.TabIndex = 12;
            this.label3.Text = "➤";
            // 
            // FAQ2Lbl
            // 
            this.FAQ2Lbl.AutoSize = true;
            this.FAQ2Lbl.BackColor = System.Drawing.Color.Transparent;
            this.FAQ2Lbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.FAQ2Lbl.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.FAQ2Lbl.Location = new System.Drawing.Point(35, 119);
            this.FAQ2Lbl.Name = "FAQ2Lbl";
            this.FAQ2Lbl.Size = new System.Drawing.Size(321, 17);
            this.FAQ2Lbl.TabIndex = 11;
            this.FAQ2Lbl.Text = "Is there another way instead of opening the ports?";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label5.Location = new System.Drawing.Point(17, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 17);
            this.label5.TabIndex = 14;
            this.label5.Text = "➤";
            // 
            // FAQ3Lbl
            // 
            this.FAQ3Lbl.AutoSize = true;
            this.FAQ3Lbl.BackColor = System.Drawing.Color.Transparent;
            this.FAQ3Lbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.FAQ3Lbl.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.FAQ3Lbl.Location = new System.Drawing.Point(35, 204);
            this.FAQ3Lbl.Name = "FAQ3Lbl";
            this.FAQ3Lbl.Size = new System.Drawing.Size(358, 17);
            this.FAQ3Lbl.TabIndex = 13;
            this.FAQ3Lbl.Text = "The SteamCMD download failed! What should I do now?";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label7.Location = new System.Drawing.Point(17, 254);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 17);
            this.label7.TabIndex = 16;
            this.label7.Text = "➤";
            // 
            // FAQ4Lbl
            // 
            this.FAQ4Lbl.AutoSize = true;
            this.FAQ4Lbl.BackColor = System.Drawing.Color.Transparent;
            this.FAQ4Lbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.FAQ4Lbl.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.FAQ4Lbl.Location = new System.Drawing.Point(35, 254);
            this.FAQ4Lbl.Name = "FAQ4Lbl";
            this.FAQ4Lbl.Size = new System.Drawing.Size(182, 17);
            this.FAQ4Lbl.TabIndex = 15;
            this.FAQ4Lbl.Text = "How do I update my server?";
            // 
            // joinServerGrpLbl
            // 
            this.joinServerGrpLbl.AutoSize = true;
            this.joinServerGrpLbl.BackColor = System.Drawing.Color.Transparent;
            this.joinServerGrpLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.joinServerGrpLbl.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.joinServerGrpLbl.Location = new System.Drawing.Point(34, 321);
            this.joinServerGrpLbl.Name = "joinServerGrpLbl";
            this.joinServerGrpLbl.Size = new System.Drawing.Size(562, 17);
            this.joinServerGrpLbl.TabIndex = 17;
            this.joinServerGrpLbl.Text = "If you have any more questions or need any help, join our discord server or steam" +
    " group.";
            // 
            // discordSrvLinkLbl
            // 
            this.discordSrvLinkLbl.AutoSize = true;
            this.discordSrvLinkLbl.BackColor = System.Drawing.Color.Transparent;
            this.discordSrvLinkLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.discordSrvLinkLbl.LinkColor = System.Drawing.Color.Magenta;
            this.discordSrvLinkLbl.Location = new System.Drawing.Point(195, 342);
            this.discordSrvLinkLbl.Name = "discordSrvLinkLbl";
            this.discordSrvLinkLbl.Size = new System.Drawing.Size(107, 18);
            this.discordSrvLinkLbl.TabIndex = 18;
            this.discordSrvLinkLbl.TabStop = true;
            this.discordSrvLinkLbl.Text = "Discord Server";
            this.discordSrvLinkLbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // steamGrpLinkLbl
            // 
            this.steamGrpLinkLbl.AutoSize = true;
            this.steamGrpLinkLbl.BackColor = System.Drawing.Color.Transparent;
            this.steamGrpLinkLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.steamGrpLinkLbl.LinkColor = System.Drawing.Color.Magenta;
            this.steamGrpLinkLbl.Location = new System.Drawing.Point(318, 342);
            this.steamGrpLinkLbl.Name = "steamGrpLinkLbl";
            this.steamGrpLinkLbl.Size = new System.Drawing.Size(97, 18);
            this.steamGrpLinkLbl.TabIndex = 19;
            this.steamGrpLinkLbl.TabStop = true;
            this.steamGrpLinkLbl.Text = "Steam Group";
            this.steamGrpLinkLbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // FAQ1LinkLbl
            // 
            this.FAQ1LinkLbl.AutoSize = true;
            this.FAQ1LinkLbl.BackColor = System.Drawing.Color.Transparent;
            this.FAQ1LinkLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.FAQ1LinkLbl.LinkColor = System.Drawing.Color.Magenta;
            this.FAQ1LinkLbl.Location = new System.Drawing.Point(248, 85);
            this.FAQ1LinkLbl.Name = "FAQ1LinkLbl";
            this.FAQ1LinkLbl.Size = new System.Drawing.Size(64, 17);
            this.FAQ1LinkLbl.TabIndex = 20;
            this.FAQ1LinkLbl.TabStop = true;
            this.FAQ1LinkLbl.Text = "Click here";
            this.FAQ1LinkLbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // FAQ1BODYLbl
            // 
            this.FAQ1BODYLbl.AutoSize = true;
            this.FAQ1BODYLbl.BackColor = System.Drawing.Color.Transparent;
            this.FAQ1BODYLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.FAQ1BODYLbl.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.FAQ1BODYLbl.Location = new System.Drawing.Point(34, 51);
            this.FAQ1BODYLbl.Name = "FAQ1BODYLbl";
            this.FAQ1BODYLbl.Size = new System.Drawing.Size(560, 51);
            this.FAQ1BODYLbl.TabIndex = 21;
            this.FAQ1BODYLbl.Text = resources.GetString("FAQ1BODYLbl.Text");
            // 
            // FAQ2BODYLbl
            // 
            this.FAQ2BODYLbl.AutoSize = true;
            this.FAQ2BODYLbl.BackColor = System.Drawing.Color.Transparent;
            this.FAQ2BODYLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.FAQ2BODYLbl.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.FAQ2BODYLbl.Location = new System.Drawing.Point(35, 136);
            this.FAQ2BODYLbl.Name = "FAQ2BODYLbl";
            this.FAQ2BODYLbl.Size = new System.Drawing.Size(537, 51);
            this.FAQ2BODYLbl.TabIndex = 22;
            this.FAQ2BODYLbl.Text = resources.GetString("FAQ2BODYLbl.Text");
            // 
            // FAQ3BODYLbl
            // 
            this.FAQ3BODYLbl.AutoSize = true;
            this.FAQ3BODYLbl.BackColor = System.Drawing.Color.Transparent;
            this.FAQ3BODYLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.FAQ3BODYLbl.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.FAQ3BODYLbl.Location = new System.Drawing.Point(35, 221);
            this.FAQ3BODYLbl.Name = "FAQ3BODYLbl";
            this.FAQ3BODYLbl.Size = new System.Drawing.Size(465, 17);
            this.FAQ3BODYLbl.TabIndex = 23;
            this.FAQ3BODYLbl.Text = "Mostly you just need to retry the download, then the problem should be fixed.";
            // 
            // FAQ4BODYLbl
            // 
            this.FAQ4BODYLbl.AutoSize = true;
            this.FAQ4BODYLbl.BackColor = System.Drawing.Color.Transparent;
            this.FAQ4BODYLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.FAQ4BODYLbl.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.FAQ4BODYLbl.Location = new System.Drawing.Point(35, 271);
            this.FAQ4BODYLbl.Name = "FAQ4BODYLbl";
            this.FAQ4BODYLbl.Size = new System.Drawing.Size(456, 17);
            this.FAQ4BODYLbl.TabIndex = 24;
            this.FAQ4BODYLbl.Text = "Just click on the \"Update\\Repair\" button, the server will automatically update.";
            // 
            // FAQFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(632, 417);
            this.Controls.Add(this.FAQ4BODYLbl);
            this.Controls.Add(this.FAQ1LinkLbl);
            this.Controls.Add(this.FAQ3BODYLbl);
            this.Controls.Add(this.FAQ2BODYLbl);
            this.Controls.Add(this.FAQ1BODYLbl);
            this.Controls.Add(this.steamGrpLinkLbl);
            this.Controls.Add(this.discordSrvLinkLbl);
            this.Controls.Add(this.joinServerGrpLbl);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.FAQ4Lbl);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.FAQ3Lbl);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.FAQ2Lbl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FAQ1Lbl);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.baseFormUsrCtrl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FAQFrm";
            this.Text = "FAQ - Frequently Asked Questions";
            this.Load += new System.EventHandler(this.FAQFrm_Load);
            this.TextChanged += new System.EventHandler(this.FAQFrm_TextChanged);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Server_creation_tool.baseFormUsrCtrl baseFormUsrCtrl1;
        private System.Windows.Forms.Panel panel2;
        private Server_Creation_Tool.customSmoothBtn okBtn;
        private System.Windows.Forms.Label FAQ1Lbl;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label FAQ4Lbl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label FAQ3Lbl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label FAQ2Lbl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel steamGrpLinkLbl;
        private System.Windows.Forms.LinkLabel discordSrvLinkLbl;
        private System.Windows.Forms.Label joinServerGrpLbl;
        private System.Windows.Forms.LinkLabel FAQ1LinkLbl;
        private System.Windows.Forms.Label FAQ4BODYLbl;
        private System.Windows.Forms.Label FAQ3BODYLbl;
        private System.Windows.Forms.Label FAQ2BODYLbl;
        private System.Windows.Forms.Label FAQ1BODYLbl;
    }
}