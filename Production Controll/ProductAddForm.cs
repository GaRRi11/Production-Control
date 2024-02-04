using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Production_Controll.Product;

namespace Production_Controll
{
    public partial class ProductAddForm : Form
    {
        public string productName { get; set; }
        private MainForm parentForm;
        private ProductService productService;
        private CityService cityService;
        private City city;
        public ProductAddForm(MainForm parentForm,long cityId)
        {
            InitializeComponent();
            this.AcceptButton = button1;
            this.parentForm = parentForm;
            this.productService = new ProductService();
            this.cityService = new CityService();
            this.city = cityService.FindById(cityId);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Size = new Size(407, 247);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 30)
            {
                MessageBox.Show("too long name");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("type the name of product");
                return;
            }
            productName = textBox1.Text;

            if (!string.IsNullOrEmpty(productName))
            {
                if (productService.DoesProductExistInCity(productName, city.id))
                {
                    MessageBox.Show($"{productName} already exists in that city");
                    return;
                }
                var association = this.GetTabPageCityAssociationByCity(city.id);
                TabPage tabPage = new TabPage();
                if (association != null)
                {
                    tabPage = association.TabPage;
                }

                Product product = new Product(productName, city.id);
                product = productService.SaveProduct(product);
                if(product == null) {
                    MessageBox.Show("Product save failed please try again");
                    this.Close();
                }
                parentForm.AddProductPanel(product, tabPage);
            }
            this.Close();
        }
    }
}
