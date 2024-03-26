using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Production_Controll
{
    public partial class ProductAddForm : Form
    {
        private readonly MainForm parentForm;
        private readonly ProductService productService;
        private readonly ProductGroupService productGroupService;
        private readonly ModificationService modificationService;
        private Product product;
        private bool modify;
        private long cityId;

        public ProductAddForm(MainForm parentForm, long cityId)
        {
            InitializeComponent();
            this.cityId = cityId;
            this.parentForm = parentForm;
            this.productService = new ProductService();
            this.productGroupService = new ProductGroupService();
            this.modificationService = new ModificationService();
            this.AcceptButton = saveBtn;
        }

        public ProductAddForm(MainForm parentForm, Product product) : this(parentForm, product.cityId)
        {
            this.product = product;
            this.modify = true;
        }

        private void SetProductFields(Product product)
        {
            textBox2.Text = product.name;
            priceTextBox.Text = product.price.ToString();
            dateTimePicker.Value = product.expirationDate;

            for (int i = 0; i < groupComboBox.Items.Count; i++)
            {
                string item = groupComboBox.Items[i].ToString();
                if (item.StartsWith($"{product.productGroupId} -"))
                {
                    groupComboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Size = new Size(397, 337);
            dateTimePicker.Value = DateTime.Now.AddDays(1);

            List<ProductGroup> productGroups = productGroupService.GetAllProductGroups();

            if (productGroups != null)
            {
                foreach (var group in productGroups)
                {
                    groupComboBox.Items.Add($"{group.Id} - {group.Name} - {group.PackagingType} - {group.Liter}");
                }
            }

            if (modify && product != null)
            {
                SetProductFields(product);
            }
        }

        private void ModifyProduct()
        {
            string newName = textBox2.Text;
            decimal newPrice = decimal.Parse(priceTextBox.Text);
            DateTime newExpirationDate = dateTimePicker.Value;
            long productId = product.id;

            bool success = productService.UpdateProduct(productId, newName, newPrice, newExpirationDate);

            if (success)
            {
                parentForm.RefreshTabPagesAndPanelsFromDatabase();
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to update product. Please try again.");
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 30)
            {
                MessageBox.Show("Product name is too long.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please enter the product name.");
                return;
            }

            if (groupComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a product group.");
                return;
            }

            if (!decimal.TryParse(priceTextBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price.");
                return;
            }

            if (dateTimePicker.Value <= DateTime.Now)
            {
                MessageBox.Show("Please enter a valid expiration date in the future.");
                return;
            }

            DateTime expirationDate = dateTimePicker.Value;
            long productGroupId = Convert.ToInt64(groupComboBox.SelectedItem.ToString().Split('-')[0].Trim());

            Product newProduct = new Product(textBox2.Text, productGroupId, price, expirationDate, cityId);

            if (!modify)
            {
                newProduct = productService.SaveProduct(newProduct);

                if (newProduct == null)
                {
                    MessageBox.Show("Failed to save product. Please try again.");
                    return;
                }

                Modification modification = new Modification(newProduct.id, Modification.Operation.CREATE, 0, DateTime.Now);
                modificationService.SaveModification(modification);
            }
            else
            {
                ModifyProduct();
            }

            parentForm.RefreshTabPagesAndPanelsFromDatabase();
            this.Close();
        }
    }
}
