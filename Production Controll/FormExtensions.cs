using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_Controll
{
    public static class FormExtensions
    {


        private const int panelHeight = 50;
        private const int panelMargin = 10;
        public static Panel CreateProductPanel(this Form form,Product product)
        {
            Panel productPanel = new Panel();
            productPanel.Name = "productPanel";
            productPanel.Tag = product;
            string productName = product.name;
            productPanel.Size = new Size(form.ClientSize.Width - SystemInformation.VerticalScrollBarWidth, panelHeight);
            productPanel.Name = productName;

            Label nameLabel = createLabel(productName, new Point(10, 10));
            Label centerLabel = createLabel("ბოლო რედ." + product.LastModified, Point.Empty); // Initialize centerLabel
            centerLabel.Name = "centerLabel";
            // Calculate the size of centerLabel
            centerLabel.AutoSize = true;
            int centerLabelWidth = centerLabel.PreferredWidth;
            int centerLabelHeight = centerLabel.PreferredHeight;

            // Set the location of centerLabel using the calculated size
            centerLabel.Location = new Point((productPanel.ClientSize.Width - centerLabelWidth) / 2,
                                             (productPanel.ClientSize.Height - centerLabelHeight) / 2);

            Label quantityLabel = createLabel("რაოდენობა: " + product.quantity, new Point(productPanel.ClientSize.Width - 170, 10));
            quantityLabel.Name = "quantityLabel";

            productPanel.Controls.Add(nameLabel);
            productPanel.Controls.Add(quantityLabel);
            productPanel.Controls.Add(centerLabel);

            

            return productPanel;
        }



        public static Label createLabel(string labelText, Point location)
        {
            Label label = new Label();
            label.Text = labelText;
            label.AutoSize = true;
            label.Location = location;
            return label;
        }

    }
}
