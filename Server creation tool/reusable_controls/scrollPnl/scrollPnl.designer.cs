namespace ScrollAblePanel
{
    partial class ScrollAblePanelControl
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
            this.ScrollContainter = new System.Windows.Forms.Panel();
            this.ScrollAt = new System.Windows.Forms.Panel();
            this.scrollAtGraphicBottom = new System.Windows.Forms.PictureBox();
            this.scrollAtGraphicMid = new System.Windows.Forms.PictureBox();
            this.scrollAtGraphicTop = new System.Windows.Forms.PictureBox();
            this.ScrollPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ScrollAt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scrollAtGraphicBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scrollAtGraphicMid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scrollAtGraphicTop)).BeginInit();
            this.SuspendLayout();
            // 
            // ScrollContainter
            // 
            this.ScrollContainter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScrollContainter.BackColor = System.Drawing.Color.Transparent;
            this.ScrollContainter.Location = new System.Drawing.Point(272, 3);
            this.ScrollContainter.Name = "ScrollContainter";
            this.ScrollContainter.Size = new System.Drawing.Size(10, 224);
            this.ScrollContainter.TabIndex = 0;
            this.ScrollContainter.Paint += new System.Windows.Forms.PaintEventHandler(this.ScrollContainter_Paint);
            // 
            // ScrollAt
            // 
            this.ScrollAt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScrollAt.BackColor = System.Drawing.Color.Transparent;
            this.ScrollAt.Controls.Add(this.scrollAtGraphicBottom);
            this.ScrollAt.Controls.Add(this.scrollAtGraphicMid);
            this.ScrollAt.Controls.Add(this.scrollAtGraphicTop);
            this.ScrollAt.Location = new System.Drawing.Point(273, 5);
            this.ScrollAt.Name = "ScrollAt";
            this.ScrollAt.Size = new System.Drawing.Size(7, 38);
            this.ScrollAt.TabIndex = 1;
            this.ScrollAt.LocationChanged += new System.EventHandler(this.ScrollAt_LocationChanged);
            this.ScrollAt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScrollAt_MouseDown);
            this.ScrollAt.MouseEnter += new System.EventHandler(this.ScrollAt_MouseEnter);
            this.ScrollAt.MouseLeave += new System.EventHandler(this.ScrollAt_MouseLeave);
            this.ScrollAt.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ScrollAt_MouseMove);
            this.ScrollAt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ScrollAt_MouseUp);
            // 
            // scrollAtGraphicBottom
            // 
            this.scrollAtGraphicBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scrollAtGraphicBottom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.scrollAtGraphicBottom.Location = new System.Drawing.Point(0, 32);
            this.scrollAtGraphicBottom.Name = "scrollAtGraphicBottom";
            this.scrollAtGraphicBottom.Size = new System.Drawing.Size(7, 6);
            this.scrollAtGraphicBottom.TabIndex = 2;
            this.scrollAtGraphicBottom.TabStop = false;
            this.scrollAtGraphicBottom.Click += new System.EventHandler(this.pictureBox3_Click);
            this.scrollAtGraphicBottom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScrollAt_MouseDown);
            this.scrollAtGraphicBottom.MouseEnter += new System.EventHandler(this.ScrollAt_MouseEnter);
            this.scrollAtGraphicBottom.MouseLeave += new System.EventHandler(this.ScrollAt_MouseLeave);
            this.scrollAtGraphicBottom.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ScrollAt_MouseMove);
            this.scrollAtGraphicBottom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ScrollAt_MouseUp);
            // 
            // scrollAtGraphicMid
            // 
            this.scrollAtGraphicMid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scrollAtGraphicMid.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.scrollAtGraphicMid.Location = new System.Drawing.Point(0, 4);
            this.scrollAtGraphicMid.Name = "scrollAtGraphicMid";
            this.scrollAtGraphicMid.Size = new System.Drawing.Size(7, 29);
            this.scrollAtGraphicMid.TabIndex = 1;
            this.scrollAtGraphicMid.TabStop = false;
            this.scrollAtGraphicMid.Click += new System.EventHandler(this.scrollAtGraphicMid_Click);
            this.scrollAtGraphicMid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScrollAt_MouseDown);
            this.scrollAtGraphicMid.MouseEnter += new System.EventHandler(this.ScrollAt_MouseEnter);
            this.scrollAtGraphicMid.MouseLeave += new System.EventHandler(this.ScrollAt_MouseLeave);
            this.scrollAtGraphicMid.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ScrollAt_MouseMove);
            this.scrollAtGraphicMid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ScrollAt_MouseUp);
            // 
            // scrollAtGraphicTop
            // 
            this.scrollAtGraphicTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scrollAtGraphicTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.scrollAtGraphicTop.Location = new System.Drawing.Point(0, 0);
            this.scrollAtGraphicTop.Name = "scrollAtGraphicTop";
            this.scrollAtGraphicTop.Size = new System.Drawing.Size(7, 6);
            this.scrollAtGraphicTop.TabIndex = 0;
            this.scrollAtGraphicTop.TabStop = false;
            this.scrollAtGraphicTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScrollAt_MouseDown);
            this.scrollAtGraphicTop.MouseEnter += new System.EventHandler(this.ScrollAt_MouseEnter);
            this.scrollAtGraphicTop.MouseLeave += new System.EventHandler(this.ScrollAt_MouseLeave);
            this.scrollAtGraphicTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ScrollAt_MouseMove);
            this.scrollAtGraphicTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ScrollAt_MouseUp);
            // 
            // ScrollPanel
            // 
            this.ScrollPanel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ScrollPanel.Location = new System.Drawing.Point(0, 0);
            this.ScrollPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ScrollPanel.Name = "ScrollPanel";
            this.ScrollPanel.Size = new System.Drawing.Size(269, 399);
            this.ScrollPanel.TabIndex = 2;
            this.ScrollPanel.SizeChanged += new System.EventHandler(this.ScrollPanel_SizeChanged);
            this.ScrollPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.ScrollPanel_Paint);
            // 
            // ScrollAblePanelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ScrollAt);
            this.Controls.Add(this.ScrollPanel);
            this.Controls.Add(this.ScrollContainter);
            this.DoubleBuffered = true;
            this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ScrollAblePanelControl";
            this.Size = new System.Drawing.Size(287, 231);
            this.Load += new System.EventHandler(this.ScrollAblePanelControl_Load);
            this.SizeChanged += new System.EventHandler(this.ScrollAblePanelControl_SizeChanged);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScrollAt_MouseDown);
            this.MouseEnter += new System.EventHandler(this.ScrollAt_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.ScrollAt_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ScrollAt_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ScrollAt_MouseUp);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ScrollAblePanelControl_MouseWheel);
            this.ScrollAt.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scrollAtGraphicBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scrollAtGraphicMid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scrollAtGraphicTop)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel ScrollAt;
        private System.Windows.Forms.FlowLayoutPanel ScrollPanel;
        private System.Windows.Forms.PictureBox scrollAtGraphicMid;
        private System.Windows.Forms.PictureBox scrollAtGraphicTop;
        private System.Windows.Forms.PictureBox scrollAtGraphicBottom;
        public System.Windows.Forms.Panel ScrollContainter;
    }
}
