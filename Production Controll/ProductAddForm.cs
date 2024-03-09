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
        private ModificationService modificationService;
        private City city;
        public ProductAddForm(MainForm parentForm,long cityId)
        {
            InitializeComponent();
            this.AcceptButton = button1;
            this.parentForm = parentForm;
            this.productService = new ProductService();
            this.cityService = new CityService();
            this.modificationService = new ModificationService();
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
            if(city == null)
            {
                MessageBox.Show("Product save failed. cannot find city. please try again");
                return;
            }

            
                if (productService.DoesProductExistInCity(productName, city.id))
                {
                    MessageBox.Show($"{productName} already exists in that city");
                    return;
                }
                Product product = new Product(productName, city.id);
                product = productService.SaveProduct(product);
                Modification modification = new Modification(product.id, Modification.Operation.CREATE, 0, DateTime.Now);
                modificationService.SaveModification(modification);

            if (product == null)
                {
                    MessageBox.Show("Product save failed please try again");
                    return;
                }
                var association = this.GetTabPageCityAssociationByCity(city.id);
                TabPage tabPage = new TabPage();
                if (association == null)
                {
                    MessageBox.Show("Product panel add failed please restart the app");
                    return;
                }
                tabPage = association.TabPage;
                parentForm.RefreshTabPagesAndPanelsFromDatabase();
            
            this.Close();
        }
    }
}
