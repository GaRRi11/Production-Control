using OfficeOpenXml;
using System;
using System.Windows.Forms;

namespace Production_Controll
{
    public partial class ProductModifieForm : Form
    {
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

            Modification modification = new Modification(product.id, operation, quantity, DateTime.Now);

            if (operation == Modification.Operation.Addition)
            {

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

            modificationService.SaveModification(modification);

            if (!productService.UpdateQuantity(product.id, modification.operation, quantity))
            {
                MessageBox.Show("Product quantity modifie failed please try again");
                return;
            }
            parentForm.RefreshTabPagesAndPanelsFromDatabase();
            this.Close();
        }

        private void deletebtn_Click(object sender, EventArgs e)

        {
            Modification modification = new Modification(product.id, Modification.Operation.DELETE, product.quantity, DateTime.Now);
            modificationService.SaveModification(modification);

            if (!productService.DeleteProduct(product.id))
            {
                MessageBox.Show("product delete failed please try again");
                return;
            }
            parentForm.RefreshTabPagesAndPanelsFromDatabase();
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

        private void transferbtn_Click(object sender, EventArgs e)
        {
            using (ProductTransferForm productTransferForm = new ProductTransferForm(parentForm, selectedPanel, product))
            {
                productTransferForm.ShowDialog();
            }
        }

        private void historyBtn_Click(object sender, EventArgs e)
        {
            using(ProductHistoryForm productHistoryForm = new ProductHistoryForm(product))
            {
                productHistoryForm.ShowDialog();
            }
        }
    }
}