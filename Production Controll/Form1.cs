using OfficeOpenXml;
using System;
using System.Drawing;
using System.Windows.Forms;
using OfficeOpenXml;
using MySql.Data.MySqlClient;

namespace Production_Controll
{
    public partial class Form1 : Form
    {
        private int panelCountTbilisi = 0;
        private int panelCountQutaisi = 0;
        private const int panelHeight = 50;
        private const int panelMargin = 10;
        private const string ProductPanelName = "productPanel";

        private ProductService ProductService;
        private DatabaseManager DatabaseManager;

        public Form1()
        {
            InitializeComponent();
            this.FormClosing += Form1_FormClosing; // Subscribe to the FormClosing event
            this.ProductService = new ProductService();
            this.DatabaseManager = new DatabaseManager();
            DatabaseManager.InitializeDB();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DatabaseManager.DropTables();
        }

        private void panelMouseEnterAndLeave(object sender, bool enter)
        {
            if (sender is Panel panel)
            {
                panel.BackColor = enter ? Color.LightBlue : Color.White;
                panel.BorderStyle = enter ? BorderStyle.Fixed3D : BorderStyle.None;
            }
        }

        private void panel_MouseEnter(object sender, EventArgs e)
        {
            panelMouseEnterAndLeave(sender, true);
        }

        private void panel_MouseLeave(object sender, EventArgs e)
        {
            panelMouseEnterAndLeave(sender, false);
        }

        private void AddProductPanelToSelectedTab(Panel productPanel)
        {
            string tabName = this.tabControl1.SelectedTab.Name;
            productPanel.Location = new Point(0, (tabName == "tabPage1" ? panelCountTbilisi++ : panelCountQutaisi++) * (panelHeight + panelMargin));
            productPanel.Name = ProductPanelName;

            this.tabControl1.SelectedTab.AutoScroll = true;
            this.tabControl1.SelectedTab.Controls.Add(productPanel);
        }

        private void AddProductPanel(string productName)
        {
            Product.City city = (this.tabControl1.SelectedTab.Name == "tabPage1") ? Product.City.TBILISI : Product.City.KUTAISI;
            Product product = new Product(productName, city);
            ProductService.CreateProduct(product);

            Panel productPanel = this.CreateProductPanel(product);
            productPanel.Name = ProductPanelName;

            productPanel.Click += panel_Click;
            productPanel.MouseEnter += panel_MouseEnter;
            productPanel.MouseLeave += panel_MouseLeave;

            AddProductPanelToSelectedTab(productPanel);
        }

        private void panel_Click(object sender, EventArgs e)
        {
            if (sender is Panel panel && panel.Tag is Product product)
            {
                long productId = product.id;

                using (Form3 form3 = new Form3(this, productId, panel))
                {
                    form3.ShowDialog();
                }
            }
        }

        private void productionAddBtn_Click(object sender, EventArgs e)
        {
            using (Form2 form2 = new Form2())
            {
                form2.ShowDialog();
                string productName = form2.productName;

                if (!string.IsNullOrEmpty(productName))
                {
                    AddProductPanel(productName);
                }
            }
        }



        public void DeletePanel()
        {
            string tabName = this.tabControl1.SelectedTab.Name;

            foreach (Control control in tabControl1.SelectedTab.Controls)
            {
                if (control is Panel panel && control.Name == ProductPanelName)
                {
                    if (tabName == "tabPage1")
                    {
                        panelCountTbilisi--;
                    }
                    else
                    {
                        panelCountQutaisi--;
                    }
                    tabControl1.SelectedTab.Controls.Remove(control);
                    break;
                }
            }
        }

        
        private void excelBtn_Click(object sender, EventArgs e)
        {

            this.GenerateExcelForAll(tabControl1);

        }
    }
}