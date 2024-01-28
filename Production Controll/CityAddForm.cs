﻿using System;
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
        public CityService cityService = new CityService();
        private MainForm parentForm;
        public CityAddForm(MainForm form)
        {
            InitializeComponent();
            parentForm = form;
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
            City city = parentForm.SaveCity(cityName, capacity);
            parentForm.CreateAndAddTabPage(city);
            this.Close();
        }
    }
}
