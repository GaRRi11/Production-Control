using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Production_Controll
{
    public partial class ProductGroupAddForm : Form
    {
        private ProductGroupForm form;
        private ProductGroupService productGroupService;

        public ProductGroupAddForm(ProductGroupForm form)
        {
            InitializeComponent();
            this.form = form;
            this.productGroupService = new ProductGroupService();
        }

        private void nameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowOnlyAlphabetsAndSpaces(e);
        }

        private void packagingTypeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowOnlyAlphabetsAndSpaces(e);
        }

        private void literTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowOnlyPositiveDecimal(sender, e);
        }

        private void AllowOnlyAlphabetsAndSpaces(KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        private void AllowOnlyPositiveDecimal(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }

            if (e.KeyChar == '-' && (sender as TextBox).Text.Length == 0)
            {
                return;
            }

            TextBox textBox = sender as TextBox;
            if ((e.KeyChar == '.' && textBox != null && textBox.Text.Contains('.')) || (e.KeyChar == '-' && textBox != null && textBox.SelectionStart != 0))
            {
                e.Handled = true;
            }
        }


        private void savebtn_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            string name = nameTextBox.Text.Trim();
            string packagingType = PackagingTypeTextBox.Text.Trim();
            decimal liter = decimal.Parse(LiterTextBox.Text.Trim());

            ProductGroup productGroup = new ProductGroup
            {
                Name = name,
                PackagingType = packagingType,
                Liter = liter
            };

            productGroup = productGroupService.SaveProductGroup(productGroup);

            if (productGroup == null)
            {
                MessageBox.Show("Failed to save product group.");
                return;
            }

            form.FillProductGroupsListBox();
            this.Close();
        }

        private bool ValidateInputs()
        {
            string name = nameTextBox.Text.Trim();
            string packagingType = PackagingTypeTextBox.Text.Trim();
            string literText = LiterTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(packagingType) || string.IsNullOrWhiteSpace(literText))
            {
                MessageBox.Show("Please fill in all fields.");
                return false;
            }

            if (!Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Name should contain only alphabets.");
                return false;
            }

            if (!Regex.IsMatch(packagingType, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Packaging type should contain only alphabets.");
                return false;
            }

            if (!decimal.TryParse(literText, out decimal liter) || liter <= 0)
            {
                MessageBox.Show("Liter should be a positive decimal.");
                return false;
            }

            return true;
        }
    }
}
