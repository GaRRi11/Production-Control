using OfficeOpenXml;
using System;
using System.Windows.Forms;

namespace Production_Controll
{
    public partial class ProductModifieForm : Form
    {
        private long productId;
        private City city;
        private Product product;
        private ProductService productService;
        private ModificationService modificationService;
        private CityService cityService;

        private MainForm parentForm;
        private Panel selectedPanel;

        public ProductModifieForm()
        {
            InitializeComponent();
            this.AcceptButton = savebtn;
            textBox1.KeyPress += textBox1_KeyPress; 
        }

        public ProductModifieForm(MainForm parent, long productId, Panel selectedPanel)
            : this()
        {
            this.productId = productId;
            this.parentForm = parent;
            this.selectedPanel = selectedPanel;
            this.productService = new ProductService();
            this.cityService = new CityService();
            this.modificationService = new ModificationService();
            this.product = productService.GetProductById(productId);
            this.city = cityService.FindById(product.cityId);
            this.productNameLabel.Text = product.name;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void savebtn_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out int number) || string.IsNullOrWhiteSpace(textBox1.Text) || number == 0)
            {
                MessageBox.Show("Type valid number");
                textBox1.Clear();
                return;
            }

            if (!additionRadio.Checked && !substractionRadio.Checked)
            {
                MessageBox.Show("Choose operation");
                return;
            }

            int quantity = int.Parse(textBox1.Text);

            Modification.Operation operation = additionRadio.Checked
                ? Modification.Operation.Addition
                : Modification.Operation.Substraction;

            Modification modification = new Modification(productId, operation, quantity, DateTime.Now);

            if (operation == Modification.Operation.Addition) {

                if (quantity > city.availableSpace)
                {
                    MessageBox.Show("no enought space in that city");
                    return;
                }

            }

                if (operation == Modification.Operation.Substraction)
            {
                if (!productService.CheckQuantityForSubtraction(product.id, quantity))
                {
                    MessageBox.Show("type valid number");
                    return;
                }
            }


            if (!productService.UpdateQuantity(modification))
            {
                MessageBox.Show("Product quantity modifie failed please try again");
                return;
            }
            if(this.UpdateLabels(selectedPanel, product.id))
            {
                MessageBox.Show("Label text update failed please restart the app");
                return;
            }
            if(!this.UpdateTabPageText(GetTabPageByCity(city), city.id))
            {
                MessageBox.Show("TabPage text update failed please restart the app");
                return;
            }
            this.Close();
        }

        private void deletebtn_Click(object sender, EventArgs e)
        {
            if (!productService.DeleteProduct(productId))
            {
                MessageBox.Show("product delete failed please try again");
                return;
            }
            if (!parentForm.DeletePanel(selectedPanel, city.id))
            {
                MessageBox.Show("Panel deletion failed please restart the app");
                return;
            }
            if (!this.UpdateTabPageText(GetTabPageByCity(city), city.id))
            {
                MessageBox.Show("TabPage text update failed please restart the app");
                return;
            }
            this.Close();
        }

        public TabPage GetTabPageByCity(City city)
        {
            var association = this.GetTabPageCityAssociationByCity(city.id);
            TabPage tabPage = new TabPage();
            if (association != null)
            {
                tabPage = association.TabPage;
                return tabPage;
            }
            return null;
        }

        private void excelBtn_Click(object sender, EventArgs e)
        {
            List<Modification> modifications = modificationService.GetAllModificationsByProductId(product.id);
            if(modifications.Count > 0)
            {
                this.GenerateExcelForOne(product, modifications);
                return;
            }
            MessageBox.Show("excel file generation failed please try again");
            return;
        }

    }
}