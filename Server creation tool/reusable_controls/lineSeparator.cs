using System.Drawing;
using System.Windows.Forms;

namespace Server_creation_tool
{
    public partial class lineSeparator : UserControl
    {
        public lineSeparator()

        {
            InitializeComponent();
            this.Paint += new PaintEventHandler(LineSeparator_Paint);
            this.MaximumSize = new Size(2000, 2);
            this.MinimumSize = new Size(0, 2);
            this.Width = 350;
        }

        private void LineSeparator_Paint(object sender, PaintEventArgs e)

        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(105, 105, 105));
            g.DrawLine(pen, new Point(0, 0), new Point(this.Width, 0));
           // g.DrawLine(Pens.White, new Point(0, 1), new Point(this.Width, 1));
        }

        private void lineSeparator_Load(object sender, System.EventArgs e)
        {

        }
    }
}
