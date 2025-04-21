namespace Server_creation_tool
{
    partial class baseFormUsrCtrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(baseFormUsrCtrl));
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.minimizeFormBtn = new Server_Creation_Tool.customBtn();
            this.closeFormBtn = new Server_Creation_Tool.customBtn();
            this.formIconPicbox = new System.Windows.Forms.PictureBox();
            this.formTitleLbl = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeFormBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeFormBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.formIconPicbox)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel3.BackgroundImage")));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(522, 28);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(10, 368);
            this.panel3.TabIndex = 12;
            // 
            // panel4
            // 
            this.panel4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel4.BackgroundImage")));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 28);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(10, 368);
            this.panel4.TabIndex = 13;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 396);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(532, 10);
            this.panel2.TabIndex = 11;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(40)))), ((int)(((byte)(56)))));
            this.panel1.BackgroundImage = global::Server_creation_tool.Properties.Resources._7cs9Ed_gradient1RESIZED1_TOP_PANEL2;
            this.panel1.Controls.Add(this.minimizeFormBtn);
            this.panel1.Controls.Add(this.closeFormBtn);
            this.panel1.Controls.Add(this.formIconPicbox);
            this.panel1.Controls.Add(this.formTitleLbl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(532, 28);
            this.panel1.TabIndex = 10;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mainFormDrag);
            // 
            // minimizeFormBtn
            // 
            this.minimizeFormBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.minimizeFormBtn.BackColor = System.Drawing.Color.Transparent;
            this.minimizeFormBtn.BackgroundImage = global::Server_creation_tool.Properties.Resources.minimizeNormal;
            this.minimizeFormBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.minimizeFormBtn.imageDown = global::Server_creation_tool.Properties.Resources.minimizeDown;
            this.minimizeFormBtn.ImageHover = global::Server_creation_tool.Properties.Resources.minimizeHover;
            this.minimizeFormBtn.ImageNormal = global::Server_creation_tool.Properties.Resources.minimizeNormal;
            this.minimizeFormBtn.Location = new System.Drawing.Point(486, 7);
            this.minimizeFormBtn.Name = "minimizeFormBtn";
            this.minimizeFormBtn.Size = new System.Drawing.Size(17, 14);
            this.minimizeFormBtn.TabIndex = 54;
            this.minimizeFormBtn.TabStop = false;
            this.minimizeFormBtn.Click += new System.EventHandler(this.minimizeFormBtn_Click_1);
            // 
            // closeFormBtn
            // 
            this.closeFormBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeFormBtn.BackColor = System.Drawing.Color.Transparent;
            this.closeFormBtn.BackgroundImage = global::Server_creation_tool.Properties.Resources.btn3;
            this.closeFormBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.closeFormBtn.imageDown = global::Server_creation_tool.Properties.Resources.btn;
            this.closeFormBtn.ImageHover = global::Server_creation_tool.Properties.Resources.btn3Hover1;
            this.closeFormBtn.ImageNormal = global::Server_creation_tool.Properties.Resources.btn3;
            this.closeFormBtn.Location = new System.Drawing.Point(506, 7);
            this.closeFormBtn.Name = "closeFormBtn";
            this.closeFormBtn.Size = new System.Drawing.Size(18, 14);
            this.closeFormBtn.TabIndex = 53;
            this.closeFormBtn.TabStop = false;
            this.closeFormBtn.Click += new System.EventHandler(this.closeFormBtn_Click_1);
            // 
            // formIconPicbox
            // 
            this.formIconPicbox.BackColor = System.Drawing.Color.Transparent;
            this.formIconPicbox.BackgroundImage = global::Server_creation_tool.Properties.Resources.icons8_server_48__1_;
            this.formIconPicbox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.formIconPicbox.Location = new System.Drawing.Point(8, 5);
            this.formIconPicbox.Name = "formIconPicbox";
            this.formIconPicbox.Size = new System.Drawing.Size(19, 18);
            this.formIconPicbox.TabIndex = 6;
            this.formIconPicbox.TabStop = false;
            // 
            // formTitleLbl
            // 
            this.formTitleLbl.AutoSize = true;
            this.formTitleLbl.BackColor = System.Drawing.Color.Transparent;
            this.formTitleLbl.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.formTitleLbl.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.formTitleLbl.Location = new System.Drawing.Point(29, 5);
            this.formTitleLbl.Name = "formTitleLbl";
            this.formTitleLbl.Size = new System.Drawing.Size(93, 17);
            this.formTitleLbl.TabIndex = 6;
            this.formTitleLbl.Text = "baseFormTitle";
            this.formTitleLbl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mainFormDrag);
            // 
            // baseFormUsrCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "baseFormUsrCtrl";
            this.Size = new System.Drawing.Size(532, 406);
            this.Load += new System.EventHandler(this.baseFormUsrCtrl_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeFormBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeFormBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.formIconPicbox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox formIconPicbox;
        private System.Windows.Forms.Label formTitleLbl;
        private Server_Creation_Tool.customBtn minimizeFormBtn;
        private Server_Creation_Tool.customBtn closeFormBtn;
        public System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.Panel panel4;
        public System.Windows.Forms.Panel panel2;
    }
}
