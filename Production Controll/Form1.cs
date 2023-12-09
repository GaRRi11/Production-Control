using System.Reflection;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace Production_Controll
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private int panelCount1 = 0;
        private int panelCount2 = 0;
        private const int panelHeight = 50; 
        private const int panelMargin = 10; 


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panelMouseEnter(object sender)
        {
            Panel panel = sender as Panel;
            if (panel != null)
            {
                panel.BackColor = Color.LightBlue; 
                panel.BorderStyle = BorderStyle.Fixed3D; 
            }
        }

        private void panelMouseLeave(object sender)
        {
            Panel panel = sender as Panel;
            if (panel != null)
            {
                panel.BackColor = Color.White; 
                panel.BorderStyle = BorderStyle.None;
            }
        }
        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            panelMouseEnter(sender);
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            panelMouseLeave(sender);
        }

        
        private void AddProductPanel(string productName)
        {
            string tabName = this.tabControl1.SelectedTab.Name;
            Panel productPanel = new Panel();
            productPanel.Size = new Size(this.ClientSize.Width - SystemInformation.VerticalScrollBarWidth, panelHeight);
            productPanel.Name = productName;
            productPanel.Click += panel_Click;

            if (tabName == "tabPage1")
            {
                productPanel.Location = new Point(0, panelCount1 * (panelHeight + panelMargin));
                panelCount1++;
            }
            else
            {
                productPanel.Location = new Point(0, panelCount2 * (panelHeight + panelMargin));
                panelCount2++;
            }

            productPanel.MouseEnter += panel1_MouseEnter;
            productPanel.MouseLeave += panel1_MouseLeave;

            Label nameLabel = new Label();
            nameLabel.Text = productName;
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(10, 10); 

            Label centerLabel = new Label();
            centerLabel.Text = "ბოლო რედ.";
            centerLabel.AutoSize = true;

            centerLabel.Location = new Point((productPanel.ClientSize.Width - centerLabel.Width) / 2, (productPanel.ClientSize.Height - centerLabel.Height) / 2);


            Label quantityLabel = new Label();
            quantityLabel.Text = "რაოდენობა";
            quantityLabel.AutoSize = true;
            quantityLabel.Location = new Point(productPanel.ClientSize.Width - 100, 10); 

            productPanel.Controls.Add(nameLabel);
            productPanel.Controls.Add(quantityLabel);
            productPanel.Controls.Add(centerLabel);

            

            this.tabControl1.SelectedTab.AutoScroll = true; 

            this.tabControl1.SelectedTab.Controls.Add(productPanel);



        }

        private void panel_Click(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel != null)
            {
                string productName = panel.Name;

                using (Form3 form3 = new Form3(productName))
                {
                    form3.ShowDialog();
                }
            }
        }

        private void productionAddBtn_Click(object sender, EventArgs e)
        {
            string productName;
            using (Form2 form2 = new Form2())
            {
                form2.ShowDialog();
                productName = form2.productName;
                MessageBox.Show("Product added: " + productName);
            }
            AddProductPanel(productName);

        }
    }
}