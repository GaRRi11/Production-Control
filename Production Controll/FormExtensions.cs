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

        public static void GenerateExcelForAll(this Form form, List<City> cities, List<Product> products)
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

                foreach (Product product in products)
                {
                    string productName = product.name;
                    string lastModified = product.lastModified.ToString();
                    int quantity = product.quantity;

                    // Access the city information directly from the Product
                    City productCity = cities.FirstOrDefault(city => city.id == product.cityId);
                    string city = productCity != null ? productCity.name : $"Unknown City ({product.cityId})";

                    worksheet.Cells[row, 1].Value = productName;
                    worksheet.Cells[row, 2].Value = lastModified;
                    worksheet.Cells[row, 3].Value = quantity;
                    worksheet.Cells[row, 4].Value = city;

                    row++;
                }

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fileName = Path.Combine(desktopPath, "ProductInfo.xlsx");
                System.IO.FileInfo excelFile = new System.IO.FileInfo(fileName);
                excelPackage.SaveAs(excelFile);
            }
        }

        public static void GenerateExcelForOne(this Form form, Product product, List<Modification> modifications)
        {

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

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
                    string productName = product.name;
                    string operationType = modification.operation.ToString();
                    int quantityChanged = modification.quantity;
                    string date = modification.date.ToString("yyyy-MM-dd HH:mm:ss");

                    worksheet.Cells[row, 1].Value = productName;
                    worksheet.Cells[row, 2].Value = operationType;
                    worksheet.Cells[row, 3].Value = quantityChanged;
                    worksheet.Cells[row, 4].Value = date;

                    row++;
                }

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
