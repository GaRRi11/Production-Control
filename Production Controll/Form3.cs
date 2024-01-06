using OfficeOpenXml;
using System;
using System.Windows.Forms;

namespace Production_Controll
{
    public partial class Form3 : Form
    {
        private long productId;
        private ProductService productService;
        private ModificationService modificationService;

        private Form1 parentForm;
        private Panel selectedPanel;

        public Form3()
        {
            InitializeComponent();
            this.AcceptButton = savebtn;
            textBox1.KeyPress += textBox1_KeyPress; // Wire up the KeyPress event
        }

        public Form3(Form1 parent, long productId, Panel selectedPanel)
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
            // Allowing only numbers, Backspace, and Control keys
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

            this.UpdateProductQuantity(productId, operation, quantity);
            this.UpdateLabels(selectedPanel,productId);

            this.Close();
        }

        private void deletebtn_Click(object sender, EventArgs e)
        {
            parentForm.DeletePanel();
            productService.DeleteProduct(productId);
            this.Close();
        }

        private void excelBtn_Click(object sender, EventArgs e)
        {
            this.GenerateExcelForOne(productId);
        }

    }
}