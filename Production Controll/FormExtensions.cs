using Microsoft.VisualBasic.Logging;
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

        public static Dictionary<Panel, PanelProductAssociation> panelAssociations = new Dictionary<Panel, PanelProductAssociation>();
        public static Dictionary<TabPage, TabPageCityAssociation> tabPageCityAssociations = new Dictionary<TabPage, TabPageCityAssociation>();
        private static ProductService productService = new ProductService();
        private static CityService cityService = new CityService();
        private static ProductGroupService productGroupService = new ProductGroupService();



        public static void ClearPanelAssociations(this Form form)
        {
            // Clear the dictionary of panel associations
            panelAssociations.Clear();
        }

        public static void ClearTabPageAssociations(this Form form)
        {
            // Clear the dictionary of tab page associations
            tabPageCityAssociations.Clear();
        }

        public static void AddTabPageCityAssociation(this Form form, TabPage tabPage, long cityId)
        {
            var association = new TabPageCityAssociation(tabPage, cityId);
            tabPageCityAssociations[tabPage] = association;
        }

        public static TabPageCityAssociation GetTabPageCityAssociationByTabPage(this Form form, TabPage tabPage) 
        { 
            if (tabPageCityAssociations.TryGetValue(tabPage, out var association))
            {
                return association;
            }
            return null;
        }

        public static TabPageCityAssociation GetTabPageCityAssociationByCity(this Form form, long cityId)
        {
            return tabPageCityAssociations.Values.FirstOrDefault(association => association.cityId == cityId);
        }

        public static void AddPanelAssociation(this Form form, Panel panel, long productId)
        {
            var association = new PanelProductAssociation(panel, productId);
            panelAssociations[panel] = association;
        }

        public static PanelProductAssociation GetPanelAssociation(this Form form, Panel panel)
        {
            if (panelAssociations.TryGetValue(panel, out var association))
            {
                return association;
            }
            return null;
        }
        public static Panel FindPanelByProductId(this Form form, long productId)
        {
            foreach (var association in panelAssociations.Values)
            {
                if (association.productId == productId)
                {
                    return association.Panel;
                }
            }
            return null;
        }

        public static void LoadExpirationDates(this Form form, ComboBox dateComboBox)
        {
            // Clear existing items in the dateComboBox
            dateComboBox.Items.Clear();

            // Add expiration date options
            dateComboBox.Items.Add("None");
            dateComboBox.Items.Add("1 month");
            dateComboBox.Items.Add("2 months");
            dateComboBox.Items.Add("3 months");

            // Set "1 month" as the default selection
            dateComboBox.SelectedIndex = 0;
        }

        public static void LoadProductGroups(this Form form, ComboBox groupComboBox)
        {
            // Clear existing items in the groupComboBox
            groupComboBox.Items.Clear();

            // Add "None" option
            groupComboBox.Items.Add("None");

            // Get all product groups
            List<ProductGroup> productGroups = productGroupService.GetAllProductGroups();

            if (productGroups != null)
            {
                // Add each product group to the groupComboBox
                foreach (var group in productGroups)
                {
                    groupComboBox.Items.Add($"{group.Id} - {group.Name} - {group.PackagingType} - {group.Liter}");
                }
            }

            // Set "None" as the default selection
            groupComboBox.SelectedIndex = 0;
        }



        public static TabPage CreateTabPage(this Form form, City city)
        {
            TabPage tabPage = new TabPage();
            int used = city.capacity - city.availableSpace;
            tabPage.Text = $"{city.name} ({used}/{city.capacity})"; 
            tabPage.AutoScroll = true;
            tabPage.Visible = true;
            form.AddTabPageCityAssociation(tabPage, city.id);
            return tabPage;
        }

        public static Panel CreateProductPanel(this Form form,Product product)
        {
            Panel productPanel = new Panel();
            productPanel.Name = "productPanel";
            string productName = product.name;
            productPanel.Size = new Size(form.ClientSize.Width - SystemInformation.VerticalScrollBarWidth, 50);
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

            form.AddPanelAssociation(productPanel, product.id);


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

        public static void GenerateExcelForAll(this Form form, List<City> allCities, List<Product> allProducts)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Create Excel package
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                // Add a new worksheet to the Excel package
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Products");

                // Add column headers
                worksheet.Cells[1, 1].Value = "Product ID";
                worksheet.Cells[1, 2].Value = "Product Name";
                worksheet.Cells[1, 3].Value = "City Name";
                worksheet.Cells[1, 4].Value = "Product Group";
                worksheet.Cells[1, 5].Value = "Expiration Date";
                worksheet.Cells[1, 6].Value = "Price";
                worksheet.Cells[1, 7].Value = "Quantity";
                worksheet.Cells[1, 8].Value = "Last Modified";

                // Populate data rows
                int row = 2; // Start from the second row
                foreach (var product in allProducts)
                {
                    // Find city name by city ID
                    string cityName = allCities.FirstOrDefault(c => c.id == product.cityId)?.name;

                    // Find product group name by product group ID
                    string productGroupName = productGroupService.GetNameById(product.productGroupId);

                    // Write data to Excel worksheet
                    worksheet.Cells[row, 1].Value = product.id;
                    worksheet.Cells[row, 2].Value = product.name;
                    worksheet.Cells[row, 3].Value = cityName;
                    worksheet.Cells[row, 4].Value = productGroupName;
                    worksheet.Cells[row, 5].Value = product.expirationDate.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 6].Value = product.price;
                    worksheet.Cells[row, 7].Value = product.quantity;
                    worksheet.Cells[row, 8].Value = product.lastModified;

                    row++; // Move to the next row
                }

                // Auto-fit columns for better readability
                worksheet.Cells.AutoFitColumns();

                // Save Excel file
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    saveFileDialog.FilterIndex = 1;
                    saveFileDialog.RestoreDirectory = true;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        FileInfo fi = new FileInfo(saveFileDialog.FileName);
                        excelPackage.SaveAs(fi);
                        MessageBox.Show("Excel file generated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }


        public static void GenerateExcelForOne(this Form form, Product product, List<Modification> modifications)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("ProductModifications");

                // Add column headers
                worksheet.Cells[1, 1].Value = "Operation Type";
                worksheet.Cells[1, 2].Value = "Quantity Changed";
                worksheet.Cells[1, 3].Value = "Date";
                worksheet.Cells[1, 4].Value = "Source City";
                worksheet.Cells[1, 5].Value = "Target City";

                int row = 2;

                foreach (var modification in modifications)
                {
                    // Get city names for source and target cities
                    string sourceCityName = cityService.GetCityNameById(modification.SourceCityId);
                    string targetCityName = cityService.GetCityNameById(modification.TargetCityId);

                    // Populate the Excel worksheet with modification details
                    worksheet.Cells[row, 1].Value = modification.operation.ToString();
                    worksheet.Cells[row, 2].Value = modification.quantity;
                    worksheet.Cells[row, 3].Value = modification.date.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Cells[row, 4].Value = sourceCityName;
                    worksheet.Cells[row, 5].Value = targetCityName;

                    row++;
                }

                // Save the Excel file
                string productLabelName = product.name;
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fileName = Path.Combine(desktopPath, $"{productLabelName}_Modifications.xlsx");
                System.IO.FileInfo excelFile = new System.IO.FileInfo(fileName);
                excelPackage.SaveAs(excelFile);
            }
        }

        public static void DeleteAllTabPagesAndPanels(this Form form)
        {
            // Remove all TabPages and associated associations
            foreach (TabPage tabPage in form.Controls.OfType<TabPage>().ToList())
            {
                form.DeleteTabPageAndAssociations(tabPage);
            }

            // Clear the dictionary of panel associations
            panelAssociations.Clear();
        }

        private static void DeleteTabPageAndAssociations(this Form form, TabPage tabPage)
        {
            // Remove all panels in the TabPage and their associations
            foreach (Panel panel in tabPage.Controls.OfType<Panel>().ToList())
            {
                form.DeletePanelAndAssociation(panel);
            }

            // Remove the association for the TabPage
            tabPageCityAssociations.Remove(tabPage);

            // Remove the TabPage from the form
            form.Controls.Remove(tabPage);
        }

        private static void DeletePanelAndAssociation(this Form form, Panel panel)
        {
            // Remove the association for the Panel
            panelAssociations.Remove(panel);

            // Remove the Panel from its parent control
            panel.Parent?.Controls.Remove(panel);
        }


    }
}
