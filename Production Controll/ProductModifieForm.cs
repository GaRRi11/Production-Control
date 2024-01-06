using OfficeOpenXml;
using System;
using System.Windows.Forms;

namespace Production_Controll
{
    public partial class ProductModifieForm : Form
    {
        private long productId;
        private ProductService productService;
        private ModificationService modificationService;

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
            this.modificationService = new ModificationService();
            this.productNameLabel.Text = productService.GetProductNameById(productId);
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
            if (!int.TryParse(textBox1.Text, out int number) || string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Type number");
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

            if (operation == Modification.Operation.Substraction)
            {
                if (!productService.CheckQuantityForSubtraction(productId, quantity))
                {
                    MessageBox.Show("type valid number");
                    return;
                }
            }

            this.UpdateProductQuantity(productId, operation, quantity,selectedPanel);

            this.Close();
        }

        private void deletebtn_Click(object sender, EventArgs e)
        {
            parentForm.DeletePanel(selectedPanel);
            productService.DeleteProduct(productId);
            this.Close();
        }

        private void excelBtn_Click(object sender, EventArgs e)
        {
            this.GenerateExcelForOne(productId);
        }

    }
}