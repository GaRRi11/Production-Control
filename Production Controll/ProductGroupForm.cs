using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Production_Controll
{
    public partial class ProductGroupForm : Form
    {
        private readonly ProductGroupService productGroupService;
        private readonly ProductService productService;
        private readonly MainForm mainForm;

        public ProductGroupForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            productGroupService = new ProductGroupService();
            productService = new ProductService();
            FillProductGroupsListBox();
        }

        public void FillProductGroupsListBox()
        {
            listBox1.Items.Clear();
            var productGroups = productGroupService.GetAllProductGroups();
            if (productGroups != null)
            {
                foreach (var group in productGroups)
                {
                    string productGroupInfo = $"{group.Id} - {group.Name} - {group.PackagingType} - {group.Liter} liters - {group.PackagingType}";
                    listBox1.Items.Add(productGroupInfo);
                }
                mainForm.LoadProductGroups();
            }
        }

        private void productionAddBtn_Click(object sender, EventArgs e)
        {
            using (var productGroupAddForm = new ProductGroupAddForm(this))
            {
                productGroupAddForm.ShowDialog();
            }
        }

        private void deletebtn_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a product group to delete.");
                return;
            }

            string selectedItem = listBox1.SelectedItem.ToString();
            long groupId = GetProductGroupIdFromSelectedItem(selectedItem);

            if (productService.FindByProductGroupId(groupId).Count > 0)
            {
                MessageBox.Show("Product Group is used");
                return;
            }

            bool success = productGroupService.DeleteProductGroupById(groupId);
            if (success)
            {
                MessageBox.Show("Product group deleted successfully.");
                FillProductGroupsListBox(); 
            }
            else
            {
                MessageBox.Show("Failed to delete product group.");
            }
        }

        private long GetProductGroupIdFromSelectedItem(string selectedItem)
        {
            string[] parts = selectedItem.Split('-');
            return Convert.ToInt64(parts[0].Trim());
        }
    }
}
