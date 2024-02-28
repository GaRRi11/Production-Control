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
    public partial class CityAddForm : Form
    {

        public string cityName;
        public int capacity;
        private CityService cityService;
        private MainForm parentForm;
        public CityAddForm(MainForm form)
        {
            InitializeComponent();
            parentForm = form;
            cityService = new CityService();
            this.AcceptButton = savebtn;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void savebtn_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 30)
            {
                MessageBox.Show("too long name");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("type the name of product");
                return;
            }
            if (textBox1.Text.All(c => c == '0'))
            {
                MessageBox.Show("Please type a valid number.", "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Exit the event handler
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("type the name of product");
                return;
            }
            cityName = textBox1.Text;
            if (!cityService.IsCityNameUnique(cityName))
            {
                MessageBox.Show("City name must be unique");
                return;
            }
            capacity = int.Parse(textBox2.Text);
            City city = new City(cityName, capacity);
            city = cityService.SaveCity(city);
            if (city == null)
            {
                MessageBox.Show("City save failed please try again");
                this.Close();
            }
            parentForm.CreateAndAddTabPage(city);
            this.Close();
        }
    }
}
