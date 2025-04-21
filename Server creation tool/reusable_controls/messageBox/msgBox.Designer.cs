namespace Server_creation_tool
{
    partial class msgBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(msgBox));
            this.bodyLbl = new System.Windows.Forms.Label();
            this.iconPicBox = new System.Windows.Forms.PictureBox();
            this.btnContainer = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.baseFormUsrCtrl1 = new Server_creation_tool.baseFormUsrCtrl();
            ((System.ComponentModel.ISupportInitialize)(this.iconPicBox)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bodyLbl
            // 
            this.bodyLbl.AutoSize = true;
            this.bodyLbl.BackColor = System.Drawing.Color.Transparent;
            this.bodyLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.bodyLbl.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.bodyLbl.Location = new System.Drawing.Point(61, 44);
            this.bodyLbl.MaximumSize = new System.Drawing.Size(365, 1000);
            this.bodyLbl.Name = "bodyLbl";
            this.bodyLbl.Size = new System.Drawing.Size(364, 30);
            this.bodyLbl.TabIndex = 1;
            this.bodyLbl.Text = "BodyBody                                                                         " +
    "                               ";
            // 
            // iconPicBox
            // 
            this.iconPicBox.BackColor = System.Drawing.Color.Transparent;
            this.iconPicBox.Location = new System.Drawing.Point(24, 42);
            this.iconPicBox.Name = "iconPicBox";
            this.iconPicBox.Size = new System.Drawing.Size(37, 34);
            this.iconPicBox.TabIndex = 2;
            this.iconPicBox.TabStop = false;
            // 
            // btnContainer
            // 
            this.btnContainer.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnContainer.Location = new System.Drawing.Point(91, 4);
            this.btnContainer.Name = "btnContainer";
            this.btnContainer.Size = new System.Drawing.Size(243, 24);
            this.btnContainer.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(47)))), ((int)(((byte)(67)))));
            this.panel2.Controls.Add(this.btnContainer);
            this.panel2.Location = new System.Drawing.Point(10, 140);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(440, 31);
            this.panel2.TabIndex = 5;
            // 
            // baseFormUsrCtrl1
            // 
            this.baseFormUsrCtrl1.BackColor = System.Drawing.Color.Transparent;
            this.baseFormUsrCtrl1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.baseFormUsrCtrl1.ControlBox = true;
            this.baseFormUsrCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.baseFormUsrCtrl1.Icon = null;
            this.baseFormUsrCtrl1.Location = new System.Drawing.Point(0, 0);
            this.baseFormUsrCtrl1.Minimize_Button = false;
            this.baseFormUsrCtrl1.Name = "baseFormUsrCtrl1";
            this.baseFormUsrCtrl1.parentForm = this;
            this.baseFormUsrCtrl1.Size = new System.Drawing.Size(460, 181);
            this.baseFormUsrCtrl1.TabIndex = 0;
            this.baseFormUsrCtrl1.Title = "Title";
            this.baseFormUsrCtrl1.Load += new System.EventHandler(this.baseFormUsrCtrl1_Load);
            // 
            // msgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(460, 181);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.iconPicBox);
            this.Controls.Add(this.bodyLbl);
            this.Controls.Add(this.baseFormUsrCtrl1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(860, 1200);
            this.MinimumSize = new System.Drawing.Size(120, 100);
            this.Name = "msgBox";
            this.Text = "Title";
            this.Load += new System.EventHandler(this.msgBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.iconPicBox)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label bodyLbl;
        private System.Windows.Forms.PictureBox iconPicBox;
        private System.Windows.Forms.Panel btnContainer;
        public System.Windows.Forms.Panel panel2;
        public baseFormUsrCtrl baseFormUsrCtrl1;
    }
}