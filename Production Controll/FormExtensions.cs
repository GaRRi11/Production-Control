using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Production_Controll
{
    public static class FormExtensions
    {


        private const int panelHeight = 50;
        private const int panelMargin = 10;
        private const string productPanelName = "productPanel";
        private static ProductService productService = new ProductService();
        private static ModificationService modificationService = new ModificationService();

        public static Panel CreateProductPanel(this Form form,Product product)
        {
            Panel productPanel = new Panel();
            productPanel.Name = "productPanel";
            productPanel.Tag = product;
            string productName = product.name;
            productPanel.Size = new Size(form.ClientSize.Width - SystemInformation.VerticalScrollBarWidth, panelHeight);
            productPanel.Name = productName;

            Label nameLabel = createLabel(productName, new Point(10, 10));
            Label centerLabel = createLabel("ბოლო რედ." + product.lastModified, Point.Empty); // Initialize centerLabel
            centerLabel.Name = "centerLabel";
            // Calculate the size of centerLabel
            centerLabel.AutoSize = true;
            int centerLabelWidth = centerLabel.PreferredWidth;
            int centerLabelHeight = centerLabel.PreferredHeight;

            // Set the location of centerLabel using the calculated size
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

                // Headers
                worksheet.Cells[1, 1].Value = "Product Name";
                worksheet.Cells[1, 2].Value = "Last Modified";
                worksheet.Cells[1, 3].Value = "Quantity";
                worksheet.Cells[1, 4].Value = "City";

                int row = 2; // Start from the second row for data

                foreach (TabPage tabPage in tabControl.TabPages)
                {
                    foreach (Control control in tabPage.Controls)
                    {
                        if (control is Panel panel && control.Name == productPanelName && control.Tag is Product product)
                        {
                            // Extracting product information
                            string productName = product.name;
                            string lastModified = productService.GetLastModifiedDate(product.id).ToString();
                            int quantity = productService.GetQuantityById(product.id);
                            string city = productService.GetCityById(product.id);

                            // Writing product information to Excel
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
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set the license context

            // Get all modifications for the current product
            List<Modification> modifications = modificationService.GetAllModificationsById(productId);

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("ProductModifications");

                // Headers
                worksheet.Cells[1, 1].Value = "Product Name";
                worksheet.Cells[1, 2].Value = "Operation Type";
                worksheet.Cells[1, 3].Value = "Quantity Changed";
                worksheet.Cells[1, 4].Value = "Date";

                int row = 2; // Start from the second row for data

                foreach (var modification in modifications)
                {
                    // Extracting modification information
                    string productName = productService.GetProductNameById(modification.productId);
                    string operationType = modification.operation.ToString();
                    int quantityChanged = modification.quantity;
                    string date = modification.date.ToString("yyyy-MM-dd HH:mm:ss");

                    // Writing modification information to Excel
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

        public static void UpdateProductQuantity(this Form form, long productId, Modification.Operation operation, int quantity)
        {
            if (operation == Modification.Operation.Addition)
            {
                productService.AddQuantity(productId, quantity);
            }
            else if (operation == Modification.Operation.Substraction)
            {
                productService.SubtractQuantity(productId, quantity);
            }
        }

        public static void UpdateLabels(this Form form, Panel panelToUpdate, long productId)
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
