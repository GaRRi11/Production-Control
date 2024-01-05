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

        public void UpdateProductQuantity(long productId, Modification.Operation operation, int quantity)
        {
            if (operation == Modification.Operation.Addition)
            {
                ProductService.AddQuantity(productId, quantity);
            }
            else if (operation == Modification.Operation.Substraction)
            {
                ProductService.SubtractQuantity(productId, quantity);
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


        //private void GenerateExcel()
        //{
        //    using (ExcelPackage excelPackage = new ExcelPackage())
        //    {
        //        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("ProductInfo");

        //        // Headers
        //        worksheet.Cells[1, 1].Value = "Product Name";
        //        worksheet.Cells[1, 2].Value = "Last Modified";
        //        worksheet.Cells[1, 3].Value = "Quantity";

        //        int row = 2; // Start from the second row for data

        //        foreach (Control control in tabControl1.SelectedTab.Controls)
        //        {
        //            if (control is Panel panel && control.Name == ProductPanelName && control.Tag is Product product)
        //            {
        //                // Extracting product information
        //                string productName = product.name;
        //                string lastModified = ProductService.getLastModifiedDate(product.id).ToString();
        //                int quantity = ProductService.getQuantityById(product.id);

        //                // Writing product information to Excel
        //                worksheet.Cells[row, 1].Value = productName;
        //                worksheet.Cells[row, 2].Value = lastModified;
        //                worksheet.Cells[row, 3].Value = quantity;

        //                row++;
        //            }
        //        }

        //        // Save the Excel file
        //        string fileName = "ProductInfo.xlsx";
        //        FileInfo excelFile = new FileInfo(fileName);
        //        excelPackage.SaveAs(excelFile);

        //        // Open the Excel file
        //        if (excelFile.Exists)
        //        {
        //            System.Diagnostics.Process.Start(fileName);
        //        }
        //    }
        //}

        private void excelBtn_Click(object sender, EventArgs e)
        {
            // ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set the license context

            //GenerateExcel();

            //using (var context = new MyDbContext())
            //{
            //    var product = new Product("Example Product", Product.City.TBILISI);
            //    product.AddQuantity(10); // Adding quantity
            //    context.Products.Add(product);
            //    context.SaveChanges(); // Save changes to the database
            //}
            try
            {
                string connstring = "server=localhost;uid=root;pwd=garomysql1852;database=studentdb";
                MySqlConnection conn = new MySqlConnection();
                conn.ConnectionString = connstring;
                conn.Open();
                string sql = "select * from student";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    MessageBox.Show("name " + dr["NAME"]);
                }
            }catch (Exception ex)
            {

            }




        }
    }
}