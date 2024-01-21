using OfficeOpenXml;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Production_Controll.Product;


namespace Production_Controll
{
    public static class FormExtensions
    {


        private const int panelHeight = 50;
        private const int panelMargin = 10;
        private const string productPanelName = "productPanel";
        private static ProductService productService = new ProductService();
        private static ModificationService modificationService = new ModificationService();
        public static CityService cityService = new CityService();  

        public static TabPage CreateTabPage(this Form form, City city)
        {
            TabPage tabPage = new TabPage();
            tabPage.Text = city.name;
            //capacity
            tabPage.AutoScroll = true;
            tabPage.Tag = city.id;
            tabPage.Visible = true;
            return tabPage;
        }


        public static Panel CreateProductPanel(this Form form,Product product)
        {
            Panel productPanel = new Panel();
            productPanel.Name = "productPanel";
            productPanel.Tag = product;
            string productName = product.name;
            productPanel.Size = new Size(form.ClientSize.Width - SystemInformation.VerticalScrollBarWidth, panelHeight);
            productPanel.Name = productName;

            Label nameLabel = createLabel(productName, new Point(10, 10));

            Label centerLabel = createLabel("ბოლო რედ." + product.lastModified, Point.Empty); 
            centerLabel.Name = "centerLabel";
            centerLabel.AutoSize = true;
            int centerLabelWidth = centerLabel.PreferredWidth;
            int centerLabelHeight = centerLabel.PreferredHeight;
            centerLabel.Location = new Point((productPanel.ClientSize.Width - centerLabelWidth) / 2,
                                             (productPanel.ClientSize.Height - centerLabelHeight) / 2);

            Label quantityLabel = createLabel("რაოდენობა: " + product.quantity, new Point(productPanel.ClientSize.Width - 170, 10));
            quantityLabel.Name = "quantityLabel";

            productPanel.Controls.Add(nameLabel);
            productPanel.Controls.Add(quantityLabel);
            productPanel.Controls.Add(centerLabel);

            

            return productPanel;
        }



        public static Label createLabel(string labelText, Point location)
        {
            Label label = new Label();
            label.Text = labelText;
            label.AutoSize = true;
            label.Location = location;
            return label;
        }

        public static void GenerateExcelForAll(this Form form,TabControl tabControl)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("ProductInfo");

                worksheet.Cells[1, 1].Value = "Product Name";
                worksheet.Cells[1, 2].Value = "Last Modified";
                worksheet.Cells[1, 3].Value = "Quantity";
                worksheet.Cells[1, 4].Value = "City";

                int row = 2; 

                foreach (TabPage tabPage in tabControl.TabPages)
                {
                    foreach (Control control in tabPage.Controls)
                    {
                        if (control is Panel panel && control.Tag is Product product)
                        {
                            string productName = product.name;
                            string lastModified = productService.GetLastModifiedDate(product.id).ToString();
                            int quantity = productService.GetQuantityById(product.id);
                            string city = productService.GetCityById(product.id);

                            worksheet.Cells[row, 1].Value = productName;
                            worksheet.Cells[row, 2].Value = lastModified;
                            worksheet.Cells[row, 3].Value = quantity;
                            worksheet.Cells[row, 4].Value = city;

                            row++;
                        }
                    }
                }

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fileName = Path.Combine(desktopPath, "ProductInfo.xlsx");
                System.IO.FileInfo excelFile = new System.IO.FileInfo(fileName);
                excelPackage.SaveAs(excelFile);
            }
        }

        public static void GenerateExcelForOne(this Form form, long productId)
        {

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


            List<Modification> modifications = modificationService.GetAllModificationsByProductId(productId);

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("ProductModifications");

                worksheet.Cells[1, 1].Value = "Product Name";
                worksheet.Cells[1, 2].Value = "Operation Type";
                worksheet.Cells[1, 3].Value = "Quantity Changed";
                worksheet.Cells[1, 4].Value = "Date";

                int row = 2; 

                foreach (var modification in modifications)
                {
                    string productName = productService.GetProductNameById(modification.productId);
                    string operationType = modification.operation.ToString();
                    int quantityChanged = modification.quantity;
                    string date = modification.date.ToString("yyyy-MM-dd HH:mm:ss");

                    worksheet.Cells[row, 1].Value = productName;
                    worksheet.Cells[row, 2].Value = operationType;
                    worksheet.Cells[row, 3].Value = quantityChanged;
                    worksheet.Cells[row, 4].Value = date;

                    row++;
                }

                string productLabelName = productService.GetProductNameById(productId);
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fileName = Path.Combine(desktopPath, $"{productLabelName}_Modifications.xlsx");
                System.IO.FileInfo excelFile = new System.IO.FileInfo(fileName);
                excelPackage.SaveAs(excelFile);


            }
        }

        public static void UpdateProductQuantity(this Form form, long productId, Modification.Operation operation, int quantity, Panel panel)
        {
            if (operation == Modification.Operation.Addition)
            {
                productService.AddQuantity(productId, quantity);
            }
            else if (operation == Modification.Operation.Substraction)
            {
                productService.SubtractQuantity(productId, quantity);
            }
            UpdateLabels(panel, productId);
        }

        public static void UpdateLabels(Panel panelToUpdate, long productId)
        {
            if (panelToUpdate != null)
            {
                string date = productService.GetLastModifiedDate(productId).ToString();
                int quantity = productService.GetQuantityById(productId);

                UpdateLabel(panelToUpdate, "quantityLabel", "რაოდენობა: " + quantity);
                UpdateLabel(panelToUpdate, "centerLabel", "ბოლო რედ." + date);
            }
        }

        private static void UpdateLabel(Panel panel, string labelName, string newText)
        {
            Control[] labels = panel.Controls.Find(labelName, true);
            if (labels.Length > 0 && labels[0] is Label label)
            {
                label.Text = newText;
            }
        }

    }
}
