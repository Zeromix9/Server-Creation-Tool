using System.Web.UI.Design;

namespace Server_creation_tool.classes
{
    partial class RichLabel: transparentRichTextBox
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
          //  this = new Server_creation_tool.classes.transparentRichTextBox();
            this.SuspendLayout();
            // 
            // rtb
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "rtb";
            this.ReadOnly = true;
            this.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.Size = new System.Drawing.Size(46, 30);
            this.TabIndex = 0;
            this.Text = "";
            this.WordWrap = false;
            this.TextChanged += new System.EventHandler(this.rtb_TextChanged);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
