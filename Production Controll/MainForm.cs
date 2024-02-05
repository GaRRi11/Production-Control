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

        private void LoadCitiesAndProducts()
        {
            List<City> cities = cityService.GetAllCities();
            if(cities.Count > 0)
            {
                foreach (var city in cities)
                {
                    TabPage tabPage = CreateAndAddTabPage(city);

                    List<Product> products = productService.GetAllProductsByCityId(city.id);
                    if (products.Count > 0)
                    {

                        foreach (var product in products)
                        {
                            AddProductPanel(product, tabPage);
                            return;
                        }

                    }
                    else
                    {
                        MessageBox.Show("cities and product load failed please restart the app");
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("cities and product load failed please restart the app");
                return;
            }

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
            Panel productPanel = new Panel();

            if (product == null || productPanel == null || targetTabPage == null || panelCounts.ContainsKey(targetTabPage))
            {
                MessageBox.Show("Product panel addition failed please restart the app");
                return;
            }
            productPanel = this.CreateProductPanel(product);
            productPanel.Click += Panel_Click;
            productPanel.MouseEnter += Panel_MouseEnter;
            productPanel.MouseLeave += Panel_MouseLeave;
            panelCounts[targetTabPage] = 0;
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

            if (cityId == -1) {
                MessageBox.Show("Production form open failed please try again");
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
            if(allCities.Count > 0  && allProducts.Count > 0)
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
    }
}
