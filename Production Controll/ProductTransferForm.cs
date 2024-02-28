using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Production_Controll
{
    public partial class ProductTransferForm : Form
    {
        private MainForm parentForm;
        private Panel selectedPanel;
        private Product product;
        private City city;
        private City targetCity;
        private CityService cityService;
        private ProductService productService;  
        private ModificationService modificationService;
        private List<City> cityList;
        private int maxQuantity;

        public ProductTransferForm(MainForm parentForm, Panel selectedPanel, Product product)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            this.selectedPanel = selectedPanel;
            this.cityService = new CityService();
            this.productService = new ProductService();
            this.modificationService = new ModificationService();
            this.product = product;
            this.maxQuantity = product.quantity;
            this.city = cityService.FindById(product.cityId);
            this.cityList = cityService.GetAllCities();
            productNameLabel.Text = product.name;
            cityLabel.Text = city.name;
            cityComboBox.DisplayMember = "name"; // Assuming the property name of the city is "Name"
            cityComboBox.DataSource = cityList;
            cityComboBox.SelectedIndexChanged += cityComboBox_SelectedIndexChanged;
        }

        private void cityComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update the city variable with the selected city from the combo box
            targetCity = (City)cityComboBox.SelectedItem;
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Parse the text to an integer
            if (int.TryParse(textBox1.Text, out int enteredQuantity))
            {
                // If the entered quantity is greater than the maximum quantity, set it to the maximum quantity
                if (enteredQuantity > maxQuantity)
                {
                    textBox1.Text = maxQuantity.ToString();
                }
            }
            else
            {
                // If parsing fails, reset the text to empty
                textBox1.Text = "";
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void allBtn_Click(object sender, EventArgs e)
        {
            textBox1.Text = product.quantity.ToString();
        }

        private void savebtn_Click(object sender, EventArgs e)
        {
            // Check if the TextBox is empty
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please type a number.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Exit the event handler
            }

            // Check if the TextBox contains only zeros
            if (textBox1.Text.All(c => c == '0'))
            {
                MessageBox.Show("Please type a valid number.", "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Exit the event handler
            }

            // Check if the length of the entered text is greater than 10
            if (textBox1.Text.Length > 10)
            {
                MessageBox.Show("Please type a valid number.", "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Exit the event handler
            }

            // Check if the entered value is a valid number
            if (!int.TryParse(textBox1.Text, out int quantity))
            {
                MessageBox.Show("Please type a valid number.", "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Exit the event handler
            }

            DialogResult result = MessageBox.Show("Are you sure to transfer " + quantity + " " + product.name + " from " + city.name + " to " + targetCity.name + "?",
                                           "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Check if the user clicked "Yes"
            if (result == DialogResult.Yes)
            {
                if(!productService.TransferProductToCity(product.id, quantity, targetCity.id))
                {
                    MessageBox.Show("raviiabaa");
                }

                parentForm.LoadCitiesAndProducts();
                parentForm.UpdateAllPanelLabelsAndTabPages();
                //4.modifikaciis shenaxva
                Modification modification = new Modification(product.id,Modification.Operation.TRANSFER,product.cityId,targetCity.id, quantity, DateTime.Now);
                modificationService.SaveModification(modification);
                this.Close();
            }

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
    }
}
