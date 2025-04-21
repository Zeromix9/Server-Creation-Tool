
namespace Server_Creation_Tool
{
    partial class customSmoothBtn
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
            this.SuspendLayout();
            // 
            // customSmoothBtn
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.FlatAppearance.BorderSize = 0;
            this.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.UseVisualStyleBackColor = false;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.smoothBtn_MouseDown);
            this.MouseEnter += new System.EventHandler(this.smoothBtn_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.smoothBtn_MouseLeave);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.smoothBtn_MouseUp);
            this.Click += new System.EventHandler(this.customSmoothBtn_Click);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
