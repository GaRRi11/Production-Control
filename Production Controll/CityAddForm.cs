using System;
using System.Windows.Forms;

namespace Production_Controll
{
    public partial class CityAddForm : Form
    {
        private string cityName;
        private int capacity;
        private readonly CityService cityService;
        private readonly MainForm parentForm;
        private readonly bool modify;
        private readonly City city;

        public CityAddForm(MainForm form)
        {
            InitializeComponent();
            parentForm = form;
            cityService = new CityService();
            AcceptButton = savebtn;
        }

        public CityAddForm(MainForm form, City city) : this(form)
        {
            nameTextBox.Text = city.name;
            capacityTextBox.Text = city.capacity.ToString();
            modify = true;
            this.city = city;
        }

        private void capacityTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void savebtn_Click(object sender, EventArgs e)
        {
            cityName = nameTextBox.Text.Trim();
            string capacityText = capacityTextBox.Text.Trim();

            if (cityName.Length == 0)
            {
                MessageBox.Show("Please type the name of the city.");
                return;
            }

            if (capacityText.Length == 0)
            {
                MessageBox.Show("Please type the capacity of the city.");
                return;
            }

            if (!int.TryParse(capacityText, out capacity))
            {
                MessageBox.Show("Please type a valid number for the capacity.", "Invalid Capacity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (capacity <= 0)
            {
                MessageBox.Show("Capacity must be a positive integer.", "Invalid Capacity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (nameTextBox.Text.Length > 30)
            {
                MessageBox.Show("The city name is too long (max 30 characters).");
                return;
            }

            if (!modify)
            {
                if (!cityService.IsCityNameUnique(cityName))
                {
                    MessageBox.Show("City name must be unique.");
                    return;
                }

                City newCity = new City(cityName, capacity);
                newCity = cityService.SaveCity(newCity);

                if (newCity == null)
                {
                    MessageBox.Show("Failed to save the city. Please try again.");
                    return;
                }
            }
            else
            {
                int usedSpace = city.capacity - city.availableSpace;

                if (usedSpace > capacity)
                {
                    MessageBox.Show($"The city has used space of {usedSpace}. Please enter a valid capacity.");
                    return;
                }

                if (!cityService.modify(city.id, cityName, capacity))
                {
                    MessageBox.Show("Failed to modify the city. Please try again.");
                    return;
                }
            }

            parentForm.RefreshTabPagesAndPanelsFromDatabase();
            Close();
        }
    }
}
