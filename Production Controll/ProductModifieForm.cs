using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Production_Controll
{
    public partial class ProductModifieForm : Form
    {
        private readonly CityService cityService;
        private readonly ProductService productService;
        private readonly ProductGroupService productGroupService;
        private readonly ModificationService modificationService;
        private readonly MainForm parentForm;
        private readonly Panel selectedPanel;
        private readonly Product product;

        public ProductModifieForm()
        {
            InitializeComponent();
            this.AcceptButton = savebtn;
            quantityTextBox.KeyPress += quantityTextBox_KeyPress;
        }

        public ProductModifieForm(MainForm parent, long productId, Panel selectedPanel) : this()
        {
            this.parentForm = parent;
            this.selectedPanel = selectedPanel;
            this.productService = new ProductService();
            this.cityService = new CityService();
            this.productGroupService = new ProductGroupService();
            this.modificationService = new ModificationService();
            this.product = productService.GetProductById(productId);
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            if (product == null)
            {
                MessageBox.Show("Failed to retrieve product information from the database.");
                return;
            }

            productNameLabel.Text = "Name: " + product.name;
            expirationDateLabel.Text = "Expiration Date: " + product.expirationDate.ToShortDateString();
            priceLabel.Text = "Price: " + product.price;
            City city = cityService.FindById(product.cityId);
            cityLabel.Text = "City: " + (city != null ? city.name : "Unknown");
            quantityLabel.Text = "Quantity: " + product.quantity.ToString();

            ProductGroup productGroup = productGroupService.FindById(product.productGroupId);
            groupLabel.Text = "Group: " + (productGroup != null ? productGroup.Name + " " + productGroup.PackagingType : "Unknown");
        }

        private void quantityTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void savebtn_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(quantityTextBox.Text, out int quantity) || string.IsNullOrWhiteSpace(quantityTextBox.Text) || quantity == 0)
            {
                MessageBox.Show("Please enter a valid quantity.");
                quantityTextBox.Clear();
                return;
            }

            if (!additionRadio.Checked && !substractionRadio.Checked)
            {
                MessageBox.Show("Please choose an operation.");
                return;
            }

            Modification.Operation operation = additionRadio.Checked
                ? Modification.Operation.Addition
                : Modification.Operation.Substraction;

            if (operation == Modification.Operation.Addition && quantity > cityService.FindById(product.cityId).availableSpace)
            {
                MessageBox.Show("There is not enough space in the city.");
                return;
            }

            if (operation == Modification.Operation.Substraction && !productService.CheckQuantityForSubtraction(product.id, quantity))
            {
                MessageBox.Show("Please enter a valid quantity.");
                return;
            }

            Modification modification = new Modification(product.id, operation, quantity, DateTime.Now);
            modificationService.SaveModification(modification);

            if (!productService.UpdateQuantity(product.id, operation, quantity))
            {
                MessageBox.Show("Failed to modify product quantity. Please try again.");
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
                MessageBox.Show("Failed to delete product. Please try again.");
                return;
            }

            parentForm.RefreshTabPagesAndPanelsFromDatabase();
            this.Close();
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
            using (ProductHistoryForm productHistoryForm = new ProductHistoryForm(product))
            {
                productHistoryForm.ShowDialog();
            }
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            using (ProductAddForm productAddForm = new ProductAddForm(parentForm, product))
            {
                productAddForm.ShowDialog();
            }
            UpdateLabels();
        }
    }
}
