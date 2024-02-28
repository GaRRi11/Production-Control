using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OfficeOpenXml;
using MySql.Data.MySqlClient;

namespace Production_Controll
{
    public partial class MainForm : Form
    {
        private const int PanelHeight = 50;
        private const int PanelMargin = 10;

        private readonly ProductService productService;
        private readonly DatabaseManager databaseManager;
        private readonly CityService cityService = new CityService();
        private readonly Dictionary<TabPage, int> panelCounts = new Dictionary<TabPage, int>();



        public MainForm()
        {
            InitializeComponent();
            productService = new ProductService();
            databaseManager = new DatabaseManager();
            databaseManager.InitializeDB();
            LoadCitiesAndProducts();
        }

        public void LoadCitiesAndProducts()
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

                    if (!panelCounts.ContainsKey(tabPage))
                    {
                        panelCounts[tabPage] = 0;
                    }

                    List<Product> products = productService.GetAllProductsByCityId(city.id);
                    if (products != null)
                    {
                        foreach (var product in products)
                        {
                            if (!IsProductPanelAdded(product.id))
                            {
                                AddProductPanel(product, tabPage);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cities and product load failed. Please restart the app.");
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Cities and product load failed. Please restart the app.");
                return;
            }
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

        private bool IsProductPanelAdded(long productId)
        {
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                foreach (Panel panel in tabPage.Controls.OfType<Panel>())
                {
                    var association = this.GetPanelAssociation(panel);
                    if (association != null && association.productId == productId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        private void CityAddbtn_Click(object sender, EventArgs e)
        {
            using (CityAddForm cityAddForm = new CityAddForm(this))
            {
                cityAddForm.ShowDialog();
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



        public void AddProductPanel(Product product, TabPage targetTabPage)
        {

            if (product == null || targetTabPage == null)
            {
                MessageBox.Show("Product panel addition failed please restart the app");
                return;
            }
            Panel productPanel;
            productPanel = this.CreateProductPanel(product);
            productPanel.Click += Panel_Click;
            productPanel.MouseEnter += Panel_MouseEnter;
            productPanel.MouseLeave += Panel_MouseLeave;
            if (!panelCounts.ContainsKey(targetTabPage))
            {
                panelCounts[targetTabPage] = 0;
            }
            int yPosition = panelCounts[targetTabPage] * (PanelHeight + PanelMargin);
            productPanel.Location = new Point(0, yPosition);
            targetTabPage.Controls.Add(productPanel);
            panelCounts[targetTabPage]++;
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


        private void ProductionAddBtn_Click(object sender, EventArgs e)
        {
            long cityId = GetSelectedCityId();
            City city = cityService.FindById(cityId);

            if (cityId == -1)
            {
                MessageBox.Show("Product add form open failed please try again");
                return;
            }

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

        public bool DeletePanel(Panel panel, long cityId)
        {
            TabPageCityAssociation association = this.GetTabPageCityAssociationByCity(cityId);

            if (association != null)
            {
                TabPage tabPage = association.TabPage;

                if (tabPage != null && panelCounts.TryGetValue(tabPage, out int panelCount))
                {
                    int removedIndex = tabPage.Controls.GetChildIndex(panel);
                    tabPage.Controls.Remove(panel);

                    for (int i = removedIndex; i < tabPage.Controls.Count; i++)
                    {
                        Control control = tabPage.Controls[i];
                        if (control is Panel)
                        {
                            int newYPosition = control.Location.Y - (PanelHeight + PanelMargin);
                            control.Location = new Point(control.Location.X, newYPosition);
                        }
                    }

                    panelCount = Math.Max(0, panelCount - 1);
                    panelCounts[tabPage] = panelCount;
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
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
            MessageBox.Show("excel file generation failed please try again");
            return;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            this.UpdateAllPanelLabelsAndTabPages();
        }

        private void emptyBtn_Click(object sender, EventArgs e)
        {
            // Get the selected city ID
            long cityId = GetSelectedCityId();

            // Find the city associated with the selected tab
            City city = cityService.FindById(cityId);

            // If the city is found, prompt the user to confirm deletion
            if (city != null)
            {
                DialogResult result = MessageBox.Show($"Do you want to empty the city '{city.name}'?", "Confirm Emptying City", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // If the user confirms, delete all products from the city
                if (result == DialogResult.Yes)
                {
                    // Delete all products from the city
                    bool deletionSuccess = productService.DeleteAllProductsInCity(cityId);

                    if (deletionSuccess)
                    {
                        // If deletion is successful, update the UI
                        MessageBox.Show("All products in the city have been deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCitiesAndProducts(); // Update the UI to reflect the changes
                        this.UpdateAllPanelLabelsAndTabPages();
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

    }
}
