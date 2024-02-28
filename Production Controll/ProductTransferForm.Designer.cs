namespace Production_Controll
{
    partial class ProductTransferForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductTransferForm));
            productNameLabel = new Label();
            savebtn = new Button();
            cityLabel = new Label();
            quantityLabel = new Label();
            textBox1 = new TextBox();
            label1 = new Label();
            cityComboBox = new ComboBox();
            allBtn = new Button();
            SuspendLayout();
            // 
            // productNameLabel
            // 
            productNameLabel.AutoSize = true;
            productNameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            productNameLabel.Location = new Point(190, 9);
            productNameLabel.Name = "productNameLabel";
            productNameLabel.Size = new Size(51, 20);
            productNameLabel.TabIndex = 16;
            productNameLabel.Text = "label2";
            // 
            // savebtn
            // 
            savebtn.Font = new Font("Segoe UI", 7.8F, FontStyle.Bold, GraphicsUnit.Point);
            savebtn.Location = new Point(341, 200);
            savebtn.Name = "savebtn";
            savebtn.Size = new Size(99, 45);
            savebtn.TabIndex = 17;
            savebtn.Text = "Save";
            savebtn.UseVisualStyleBackColor = true;
            savebtn.Click += savebtn_Click;
            // 
            // cityLabel
            // 
            cityLabel.AutoSize = true;
            cityLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            cityLabel.Location = new Point(35, 76);
            cityLabel.Name = "cityLabel";
            cityLabel.Size = new Size(51, 20);
            cityLabel.TabIndex = 19;
            cityLabel.Text = "label2";
            // 
            // quantityLabel
            // 
            quantityLabel.AutoSize = true;
            quantityLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            quantityLabel.Location = new Point(35, 170);
            quantityLabel.Name = "quantityLabel";
            quantityLabel.Size = new Size(74, 20);
            quantityLabel.TabIndex = 20;
            quantityLabel.Text = "Quantity:";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(164, 167);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 21;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.KeyPress += textBox1_KeyPress;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Image = (Image)resources.GetObject("label1.Image");
            label1.Location = new Point(164, 81);
            label1.Name = "label1";
            label1.Size = new Size(105, 20);
            label1.TabIndex = 22;
            label1.Text = "                        \r\n";
            // 
            // cityComboBox
            // 
            cityComboBox.FormattingEnabled = true;
            cityComboBox.Location = new Point(289, 68);
            cityComboBox.Name = "cityComboBox";
            cityComboBox.Size = new Size(151, 28);
            cityComboBox.TabIndex = 23;
            // 
            // allBtn
            // 
            allBtn.Font = new Font("Segoe UI", 7.8F, FontStyle.Bold, GraphicsUnit.Point);
            allBtn.Location = new Point(295, 167);
            allBtn.Name = "allBtn";
            allBtn.Size = new Size(60, 27);
            allBtn.TabIndex = 24;
            allBtn.Text = "All";
            allBtn.UseVisualStyleBackColor = true;
            allBtn.Click += allBtn_Click;
            // 
            // ProductTransferForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(452, 257);
            Controls.Add(allBtn);
            Controls.Add(cityComboBox);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(quantityLabel);
            Controls.Add(cityLabel);
            Controls.Add(savebtn);
            Controls.Add(productNameLabel);
            Name = "ProductTransferForm";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label productNameLabel;
        private Button savebtn;
        private Label cityLabel;
        private Label quantityLabel;
        private TextBox textBox1;
        private Label label1;
        private ComboBox cityComboBox;
        private Button allBtn;
    }
}