using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Production_Controll
{
    public partial class Form2 : Form
    {
        public string productName { get; set; }
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Size = new Size(407, 247);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > 30)
            {
                MessageBox.Show("too long name");
                return;
            }
            productName = textBox1.Text;
            this.Close();
        }
    }
}
