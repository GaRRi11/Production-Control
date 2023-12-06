using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Production_Controll
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private ProductManager productManager = new ProductManager();
        private ProductDTO productDTO = new ProductDTO();

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("koka kola");
            comboBox1.Items.Add("fanta");


            comboBox3.Items.Add("0.5");
            comboBox3.Items.Add("1");


            comboBox4.Items.Add("Tbilisi");
            comboBox4.Items.Add("Natakhtari");

            comboBox1.SelectionChangeCommitted += comboBox1_SelectedIndexChanged;



            dataGridView1.DataSource = productManager.getTbilisiProducts();
            ConfigureDataGridView(dataGridView1); // Configure dataGridView1

            dataGridView2.DataSource = productManager.getNatakhtariProducts();
            ConfigureDataGridView(dataGridView2); // Configure dataGridView2

            // Add dataGridView2 to tabPage4
            dataGridView1.Dock = DockStyle.Fill;
            tabPage3.Controls.Add(dataGridView1);

            dataGridView2.Dock = DockStyle.Fill;
            tabPage4.Controls.Add(dataGridView2);

        }

        private void updateDataSource()
        {
            dataGridView1.DataSource = null; // Reset the DataSource
            dataGridView1.DataSource = productManager.getTbilisiProducts(); // Set the updated data source

            dataGridView2.DataSource = null; // Reset the DataSource
            dataGridView2.DataSource = productManager.getNatakhtariProducts(); // Set the updated data source
        
            ConfigureDataGridView(dataGridView1);

            ConfigureDataGridView(dataGridView2);
        }
        private void ConfigureDataGridView(DataGridView dataGridView)
        {
            dataGridView.Columns["name"].HeaderText = "Product"; // Adjust column headers
            dataGridView.Columns["liters"].HeaderText = "Liters";
            dataGridView.Columns["version"].HeaderText = "Version";
            dataGridView.Columns["quantity"].HeaderText = "Quantity";

            // Optionally, set DataGridView properties or appearance
            // Example:
            dataGridView.AutoResizeColumns();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear(); // Clear the items in comboBox2
            if (comboBox1.SelectedItem != null)
            {


                // Add items to comboBox2 based on the selection in comboBox1
                if (comboBox1.SelectedItem.ToString() == "koka kola")
                {
                    comboBox2.Items.Add("zero");
                    comboBox2.Items.Add("lime");
                }
                else if (comboBox1.SelectedItem.ToString() == "fanta")
                {
                    comboBox2.Items.Add("tropiki");
                    comboBox2.Items.Add("chveulebrivi");
                }
            }
        }



        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = comboBox1.SelectedItem?.ToString();
            string version = comboBox2.SelectedItem?.ToString();
            string city = comboBox4.SelectedItem?.ToString();
            int quantity;

            if (!int.TryParse(textBox1.Text, out quantity) || string.IsNullOrWhiteSpace(textBox1.Text))
            {
                // Handle invalid input for quantity
                // For example, show an error message or set a default value
                MessageBox.Show("Please enter a valid value for Quantity.");
                return;
            }

            double liters;
            if (comboBox3.SelectedItem != null && double.TryParse(comboBox3.SelectedItem.ToString(), out liters))
            {
                // Assuming you have productManager and productDTO instances
                productManager.add(productDTO.createProduct(name, version, liters, city, quantity));
                updateDataSource();
            }
            else
            {
                // Handle cases where ComboBox3 value is not selected or is invalid
                MessageBox.Show("Please select a valid value for Liters.");
            }
        }

    }
}