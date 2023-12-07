using System.Reflection;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Production_Controll
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panelMouseEnter(object sender)
        {
            Panel panel = sender as Panel;
            if (panel != null)
            {
                panel.BorderStyle = BorderStyle.FixedSingle; // Change the border style to indicate hover
                panel.BackColor = System.Drawing.Color.LightBlue; // Change background color if desired
            }
        }

        private void panelMouseLeave(object sender)
        {
            Panel panel = sender as Panel;
            if (panel != null)
            {
                panel.BorderStyle = BorderStyle.None; // Revert the border style when mouse leaves
                panel.BackColor = System.Drawing.Color.White; // Revert background color if changed
            }
        }
        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            panelMouseEnter(sender);
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            panelMouseLeave(sender);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}