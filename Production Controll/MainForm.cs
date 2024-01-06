using OfficeOpenXml;
using System;
using System.Drawing;
using System.Windows.Forms;
using OfficeOpenXml;
using MySql.Data.MySqlClient;

namespace Production_Controll
{
    public partial class MainForm : Form
    {
        private int panelCountTbilisi = 0;
        private int panelCountQutaisi = 0;
        private const int panelHeight = 50;
        private const int panelMargin = 10;

        private ProductService ProductService;
        private DatabaseManager DatabaseManager;

        public MainForm()
        {
            InitializeComponent();
            this.FormClosing += Form1_FormClosing;
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

            this.tabControl1.SelectedTab.AutoScroll = true;
            this.tabControl1.SelectedTab.Controls.Add(productPanel);
        }

        private void AddProductPanel(string productName)
        {
            Product.City city = (this.tabControl1.SelectedTab.Name == "tabPage1") ? Product.City.TBILISI : Product.City.KUTAISI;
            Product product = new Product(productName, city);
            ProductService.SaveProduct(product);

            Panel productPanel = this.CreateProductPanel(product);

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

                using (ProductModifieForm form3 = new ProductModifieForm(this, productId, panel))
                {
                    form3.ShowDialog();
                }
            }
        }

        private void productionAddBtn_Click(object sender, EventArgs e)
        {
            using (ProductAddForm form2 = new ProductAddForm())
            {
                form2.ShowDialog();
                string productName = form2.productName;

                if (!string.IsNullOrEmpty(productName))
                {
                    AddProductPanel(productName);
                }
            }
        }

        public void DeletePanel(Panel panel)
        {
            string tabName = tabControl1.SelectedTab.Name;

            if (tabName == "tabPage1")
            {
                panelCountTbilisi--;
            }
            else
            {
                panelCountQutaisi--;
            }

            int removedIndex = tabControl1.SelectedTab.Controls.GetChildIndex(panel);
            tabControl1.SelectedTab.Controls.Remove(panel);

            for (int i = removedIndex; i < tabControl1.SelectedTab.Controls.Count; i++)
            {
                Control control = tabControl1.SelectedTab.Controls[i];
                if (control is Panel)
                {
                    int yPosition = control.Location.Y - (panelHeight + panelMargin);
                    control.Location = new Point(control.Location.X, yPosition);
                }
            }
        }


        private void excelBtn_Click(object sender, EventArgs e)
        {

            this.GenerateExcelForAll(tabControl1);

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}