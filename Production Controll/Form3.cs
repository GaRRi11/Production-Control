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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        public Form3(string productName)
        {
            InitializeComponent();
            this.productName.Text = productName;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void productName_Click(object sender, EventArgs e)
        {

        }
    }
}
