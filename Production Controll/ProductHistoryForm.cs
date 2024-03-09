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
    public partial class ProductHistoryForm : Form
    {

        private ModificationService modificationService;
        private Product product;
        public ProductHistoryForm(Product product)
        {
            InitializeComponent();
            modificationService = new ModificationService();
            this.product = product;
        }


        private void ProductHistoryForm_Load(object sender, EventArgs e)
        {
            // Call the method to load product modifications when the form is loaded
            LoadProductModifications();
        }

        private void LoadProductModifications()
        {
            // Call the method to retrieve all product modifications
            var modifications = modificationService.GetAllModificationsByProductId(product.id);

            // Check if modifications were retrieved successfully
            if (modifications != null)
            {
                // Create columns for the DataGridView
                dataGridViewProductModifications.Columns.Add("OperationType", "Operation Type");
                dataGridViewProductModifications.Columns.Add("QuantityChanged", "Quantity Changed");
                dataGridViewProductModifications.Columns.Add("Date", "Date");

                // Add rows to the DataGridView
                foreach (var modification in modifications)
                {
                    // Create a row array with the values of the modification
                    var row = new List<object>
                    {
                        modification.operation.ToString(),
                        modification.quantity,
                        modification.date
                    };

                    // If operation type is transfer, add source and target city columns
                    if (modification.operation == Modification.Operation.TRANSFER)
                    {
                        row.Add(modification.SourceCityId);
                        row.Add(modification.TargetCityId);
                    }

                    // Add the row to the DataGridView
                    dataGridViewProductModifications.Rows.Add(row.ToArray());
                }
            }
            else
            {
                MessageBox.Show("Failed to load product modifications. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void excelBtn_Click(object sender, EventArgs e)
        {
            List<Modification> modifications = modificationService.GetAllModificationsByProductId(product.id);
            if (modifications.Count > 0)
            {
                this.GenerateExcelForOne(product, modifications);
                return;
            }
            MessageBox.Show("excel file generation failed please try again");
            return;
        }

    }
}
