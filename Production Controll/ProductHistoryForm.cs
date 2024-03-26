using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Production_Controll
{
    public partial class ProductHistoryForm : Form
    {
        private readonly ModificationService modificationService;
        private readonly CityService cityService;
        private readonly Product product;

        public ProductHistoryForm(Product product)
        {
            InitializeComponent();
            modificationService = new ModificationService();
            cityService = new CityService();
            this.product = product;
        }

        private void ProductHistoryForm_Load(object sender, EventArgs e)
        {
            LoadProductModifications();
        }

        private void LoadProductModifications()
        {
            var modifications = modificationService.GetAllModificationsByProductId(product.id);

            if (modifications != null)
            {
                SetupDataGridViewColumns();

                foreach (var modification in modifications)
                {
                    PopulateDataGridViewRow(modification);
                }
            }
            else
            {
                MessageBox.Show("Failed to load product modifications. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridViewColumns()
        {
            dataGridViewProductModifications.Columns.Clear();

            foreach (var property in typeof(Modification).GetProperties())
            {
                if (property.Name != "productId" && property.Name != "SourceCityId" && property.Name != "TargetCityId")
                {
                    dataGridViewProductModifications.Columns.Add(property.Name, property.Name);
                }
                else if (property.Name == "SourceCityId")
                {
                    dataGridViewProductModifications.Columns.Add("SourceCityName", "Source City");
                }
                else if (property.Name == "TargetCityId")
                {
                    dataGridViewProductModifications.Columns.Add("TargetCityName", "Target City");
                }
            }
        }

        private void PopulateDataGridViewRow(Modification modification)
        {
            var values = new List<object>();

            foreach (var property in typeof(Modification).GetProperties())
            {
                if (property.Name != "productId")
                {
                    if (property.Name == "SourceCityId")
                    {
                        values.Add(cityService.GetCityNameById(modification.SourceCityId));
                    }
                    else if (property.Name == "TargetCityId")
                    {
                        values.Add(cityService.GetCityNameById(modification.TargetCityId));
                    }
                    else
                    {
                        values.Add(property.GetValue(modification));
                    }
                }
            }

            dataGridViewProductModifications.Rows.Add(values.ToArray());
        }

        private void excelBtn_Click(object sender, EventArgs e)
        {
            List<Modification> modifications = modificationService.GetAllModificationsByProductId(product.id);
            if (modifications != null && modifications.Count > 0)
            {
                this.GenerateExcelForOne(product, modifications);
            }
            else
            {
                MessageBox.Show("Excel file generation failed. Please try again.");
            }
        }
    }
}
