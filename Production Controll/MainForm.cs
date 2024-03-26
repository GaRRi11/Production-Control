using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OfficeOpenXml;
using MySql.Data.MySqlClient;
using static Production_Controll.Product;

namespace Production_Controll
{
    public partial class MainForm : Form
    {
        private const int PanelHeight = 50;
        private const int PanelMargin = 10;

        private readonly ProductService productService;
        private readonly DatabaseManager databaseManager;
        private readonly CityService cityService;
        private readonly ProductGroupService productGroupService;

        public MainForm()
        {
            InitializeComponent();
            databaseManager = new DatabaseManager();
            productService = new ProductService();
            cityService = new CityService();
            productGroupService = new ProductGroupService();
            databaseManager.InitializeDB();
            this.LoadProductGroups(groupComboBox);
            this.LoadExpirationDates(dateComboBox);
            RefreshTabPagesAndPanelsFromDatabase();
        }

        private void CityAddbtn_Click(object sender, EventArgs e)
        {
            using (CityAddForm cityAddForm = new CityAddForm(this))
            {
                cityAddForm.ShowDialog();
            }
        }

        private void ProductionAddBtn_Click(object sender, EventArgs e)
        {
            long cityId = GetSelectedCityId();
            City city = cityService.FindById(cityId);

            if (city == null)
            {
                MessageBox.Show("Product add form open failed,cannot find city, please try again");
                return;
            }

            if (city.availableSpace == 0)
            {
                MessageBox.Show("City is full");
                return;
            }

            using (ProductAddForm form2 = new ProductAddForm(this, cityId))
            {
                form2.ShowDialog();
            }
        }

        private void ExcelBtn_Click(object sender, EventArgs e)
        {
            List<City> allCities = cityService.GetAllCities();
            List<Product> allProducts = productService.GetAllProducts();

            if (allCities.Count > 0 && allProducts.Count > 0)
            {
                this.GenerateExcelForAll(allCities, allProducts);
                return;
            }
            MessageBox.Show("No cities and products found");
            return;
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            this.RefreshTabPagesAndPanelsFromDatabase();
        }

        private void emptyBtn_Click(object sender, EventArgs e)
        {
            long cityId = GetSelectedCityId();
            City city = cityService.FindById(cityId);

            if (city != null)
            {
                DialogResult result = MessageBox.Show($"Do you want to empty the city '{city.name}'?", "Confirm Emptying City", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool deletionSuccess = productService.DeleteAllProductsInCity(cityId);

                    if (deletionSuccess)
                    {
                        MessageBox.Show("All products in the city have been deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshTabPagesAndPanelsFromDatabase();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete products from the city. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Failed to retrieve city information. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            City city = cityService.FindById(GetSelectedCityId());

            if (city == null)
            {
                MessageBox.Show("no city added");
                return;
            }

            using (CityAddForm cityAddForm = new CityAddForm(this, city))
            {
                cityAddForm.ShowDialog();
            }
        }

        private void groupBtn_Click(object sender, EventArgs e)
        {
            using (ProductGroupForm productGroupForm = new ProductGroupForm(this))
            {
                productGroupForm.ShowDialog();
            }
        }

        private void Panel_Click(object sender, EventArgs e)
        {
            if (sender is Panel panel)
            {
                var association = this.GetPanelAssociation(panel);

                if (association != null)
                {
                    long productId = association.productId;

                    using (ProductModifieForm form3 = new ProductModifieForm(this, productId, panel))
                    {
                        form3.ShowDialog();
                    }
                }
            }
        }

        private void dateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dateComboBox.SelectedItem != null)
            {
                string selectedOption = dateComboBox.SelectedItem.ToString();

                if (selectedOption == "None")
                {
                    RefreshTabPagesAndPanelsFromDatabase();
                }
                else
                {
                    DateTime expirationDate = DateTime.Now.AddMonths(int.Parse(selectedOption.Split(' ')[0]));

                    FilterProductsByExpirationDate(expirationDate);
                }
            }
        }

        private void FilterProductsByExpirationDate(DateTime expirationDate)
        {
            List<Product> allProducts = productService.GetAllProducts();


            if (allProducts != null)
            {
                ClearPanels();
                List<Product> filteredProducts = allProducts.Where(p => p.expirationDate <= expirationDate).ToList();
                foreach (var product in filteredProducts)
                {
                    AddProductPanel(product);
                }
            }
        }

        public void LoadProductGroups()
        {
            this.LoadProductGroups(groupComboBox);
        }

        public void LoadCitiesAndProducts()
        {
            LoadCityTabPages();
            LoadProductPanels();
        }

        private void LoadCityTabPages()
        {
            List<City> cities = cityService.GetAllCities();
            if (cities != null)
            {
                foreach (var city in cities)
                {
                    TabPage tabPage = GetTabPageByCityId(city.id);
                    if (tabPage == null)
                    {
                        tabPage = CreateAndAddTabPage(city);
                    }
                }
            }
            else
            {
                MessageBox.Show("Failed to load cities. Please restart the app.");
            }
        }

        private void LoadProductPanels()
        {
            List<Product> products = productService.GetAllProducts();
            if (products != null)
            {
                foreach (var product in products)
                {
                    AddProductPanel(product);
                }
            }
            else
            {
                MessageBox.Show("Failed to load products for the city. Please restart the app.");
                return;
            }
        }


        public void RefreshTabPagesAndPanelsFromDatabase()
        {
            ClearPanels();

            foreach (TabPage tabPage in tabControl1.TabPages.OfType<TabPage>().ToList())
            {
                foreach (Panel panel in tabPage.Controls.OfType<Panel>().ToList())
                {
                    tabPage.Controls.Remove(panel); 
                    this.ClearPanelAssociations();

                }

                this.ClearTabPageAssociations();
                tabControl1.TabPages.Remove(tabPage);
            }

            LoadCitiesAndProducts();
        }

        private TabPage GetTabPageByCityId(long cityId)
        {
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                var association = this.GetTabPageCityAssociationByTabPage(tabPage);
                if (association != null && association.cityId == cityId)
                {
                    return tabPage;
                }
            }
            return null;
        }

        private void groupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearPanels();

            if (groupComboBox.SelectedItem != null)
            {
                if (groupComboBox.SelectedItem.ToString() == "None")
                {
                    RefreshTabPagesAndPanelsFromDatabase();
                }
                else
                {
                    string selectedItem = groupComboBox.SelectedItem.ToString();
                    long groupId = Convert.ToInt64(selectedItem.Split('-')[0].Trim());

                    LoadProductsByGroupId(groupId);
                }
            }
        }


        private void LoadProductsByGroupId(long groupId)
        {
            List<Product> products = productService.getProductsByGroupId(groupId);

            if (products != null)
            {
                foreach (var product in products)
                {
                    AddProductPanel(product);
                }
            }
        }

        private void ClearPanels()
        {
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                tabPage.Controls.Clear();
            }
        }

        public TabPage CreateAndAddTabPage(City city)
        {
            TabPage tabPage = this.CreateTabPage(city);
            tabControl1.Controls.Add(tabPage);
            return tabPage;
        }

        private void PanelMouseEnterAndLeave(object sender, bool enter)
        {
            if (sender is Panel panel)
            {
                panel.BackColor = enter ? Color.LightBlue : Color.White;
                panel.BorderStyle = enter ? BorderStyle.Fixed3D : BorderStyle.None;
            }
        }

        private void Panel_MouseEnter(object sender, EventArgs e)
        {
            PanelMouseEnterAndLeave(sender, true);
        }

        private void Panel_MouseLeave(object sender, EventArgs e)
        {
            PanelMouseEnterAndLeave(sender, false);
        }
        public void AddProductPanel(Product product)
        {
            if (product == null)
            {
                MessageBox.Show("Product panel addition failed. Please restart the app.");
                return;
            }

            long cityId = product.cityId;
            TabPage tabPage = GetTabPageByCityId(cityId);

            if (tabPage == null)
            {
                City city = cityService.FindById(cityId);
                tabPage = CreateAndAddTabPage(city);
            }

            Panel productPanel = this.CreateProductPanel(product);
            productPanel.Click += Panel_Click;
            productPanel.MouseEnter += Panel_MouseEnter;
            productPanel.MouseLeave += Panel_MouseLeave;
            int yPosition = 0;

            foreach (Control control in tabPage.Controls)
            {
                if (control is Panel panel)
                {
                    yPosition += panel.Height + PanelMargin;
                }
            }

            productPanel.Location = new Point(0, yPosition);
            tabPage.Controls.Add(productPanel);
        }

        private long GetSelectedCityId()
        {
            if (tabControl1.SelectedTab != null)
            {
                var association = this.GetTabPageCityAssociationByTabPage(tabControl1.SelectedTab);

                if (association != null)
                {
                    long cityId = association.cityId;
                    return cityId;
                }
            }

            return -1;
        }

    }
}
