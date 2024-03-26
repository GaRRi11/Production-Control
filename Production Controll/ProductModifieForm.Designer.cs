namespace Production_Controll
{
    partial class ProductModifieForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductModifieForm));
            label1 = new Label();
            quantityTextBox = new TextBox();
            additionRadio = new RadioButton();
            substractionRadio = new RadioButton();
            savebtn = new Button();
            deletebtn = new Button();
            productNameLabel = new Label();
            transferbtn = new Button();
            editBtn = new Button();
            historyBtn = new Button();
            expirationDateLabel = new Label();
            priceLabel = new Label();
            groupLabel = new Label();
            cityLabel = new Label();
            quantityLabel = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(223, 9);
            label1.Name = "label1";
            label1.Size = new Size(78, 20);
            label1.TabIndex = 0;
            label1.Text = "Quantity: ";
            // 
            // quantityTextBox
            // 
            quantityTextBox.Location = new Point(315, 6);
            quantityTextBox.Name = "quantityTextBox";
            quantityTextBox.Size = new Size(125, 27);
            quantityTextBox.TabIndex = 1;
            // 
            // additionRadio
            // 
            additionRadio.AutoSize = true;
            additionRadio.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            additionRadio.Location = new Point(223, 54);
            additionRadio.Name = "additionRadio";
            additionRadio.Size = new Size(91, 24);
            additionRadio.TabIndex = 11;
            additionRadio.TabStop = true;
            additionRadio.Text = "Addition";
            additionRadio.UseVisualStyleBackColor = true;
            // 
            // substractionRadio
            // 
            substractionRadio.AutoSize = true;
            substractionRadio.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            substractionRadio.Location = new Point(223, 99);
            substractionRadio.Name = "substractionRadio";
            substractionRadio.Size = new Size(118, 24);
            substractionRadio.TabIndex = 12;
            substractionRadio.TabStop = true;
            substractionRadio.Text = "Substraction";
            substractionRadio.UseVisualStyleBackColor = true;
            // 
            // savebtn
            // 
            savebtn.Font = new Font("Segoe UI", 7.8F, FontStyle.Bold, GraphicsUnit.Point);
            savebtn.Location = new Point(363, 200);
            savebtn.Name = "savebtn";
            savebtn.Size = new Size(67, 45);
            savebtn.TabIndex = 13;
            savebtn.Text = "Save";
            savebtn.UseVisualStyleBackColor = true;
            savebtn.Click += savebtn_Click;
            // 
            // deletebtn
            // 
            deletebtn.Font = new Font("Segoe UI", 7.8F, FontStyle.Bold, GraphicsUnit.Point);
            deletebtn.Image = Properties.Resources.delete;
            deletebtn.Location = new Point(71, 200);
            deletebtn.Name = "deletebtn";
            deletebtn.Size = new Size(67, 45);
            deletebtn.TabIndex = 14;
            deletebtn.UseVisualStyleBackColor = true;
            deletebtn.Click += deletebtn_Click;
            // 
            // productNameLabel
            // 
            productNameLabel.AutoSize = true;
            productNameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            productNameLabel.Location = new Point(12, 9);
            productNameLabel.Name = "productNameLabel";
            productNameLabel.Size = new Size(51, 20);
            productNameLabel.TabIndex = 15;
            productNameLabel.Text = "label2";
            // 
            // transferbtn
            // 
            transferbtn.Font = new Font("Segoe UI", 7.8F, FontStyle.Bold, GraphicsUnit.Point);
            transferbtn.Image = Properties.Resources.truck__1_;
            transferbtn.ImageAlign = ContentAlignment.BottomCenter;
            transferbtn.Location = new Point(217, 200);
            transferbtn.Name = "transferbtn";
            transferbtn.Size = new Size(67, 45);
            transferbtn.TabIndex = 16;
            transferbtn.UseVisualStyleBackColor = true;
            transferbtn.Click += transferbtn_Click;
            // 
            // editBtn
            // 
            editBtn.Font = new Font("Segoe UI", 7.8F, FontStyle.Bold, GraphicsUnit.Point);
            editBtn.Image = (Image)resources.GetObject("editBtn.Image");
            editBtn.ImageAlign = ContentAlignment.BottomCenter;
            editBtn.Location = new Point(144, 200);
            editBtn.Name = "editBtn";
            editBtn.Size = new Size(67, 45);
            editBtn.TabIndex = 17;
            editBtn.UseVisualStyleBackColor = true;
            editBtn.Click += editBtn_Click;
            // 
            // historyBtn
            // 
            historyBtn.Font = new Font("Segoe UI", 7.8F, FontStyle.Bold, GraphicsUnit.Point);
            historyBtn.Image = (Image)resources.GetObject("historyBtn.Image");
            historyBtn.Location = new Point(290, 200);
            historyBtn.Name = "historyBtn";
            historyBtn.Size = new Size(67, 45);
            historyBtn.TabIndex = 18;
            historyBtn.UseVisualStyleBackColor = true;
            historyBtn.Click += historyBtn_Click;
            // 
            // expirationDateLabel
            // 
            expirationDateLabel.AutoSize = true;
            expirationDateLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            expirationDateLabel.Location = new Point(12, 99);
            expirationDateLabel.Name = "expirationDateLabel";
            expirationDateLabel.Size = new Size(51, 20);
            expirationDateLabel.TabIndex = 19;
            expirationDateLabel.Text = "label2";
            // 
            // priceLabel
            // 
            priceLabel.AutoSize = true;
            priceLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            priceLabel.Location = new Point(12, 136);
            priceLabel.Name = "priceLabel";
            priceLabel.Size = new Size(51, 20);
            priceLabel.TabIndex = 20;
            priceLabel.Text = "label2";
            // 
            // groupLabel
            // 
            groupLabel.AutoSize = true;
            groupLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupLabel.Location = new Point(12, 58);
            groupLabel.Name = "groupLabel";
            groupLabel.Size = new Size(51, 20);
            groupLabel.TabIndex = 21;
            groupLabel.Text = "label2";
            // 
            // cityLabel
            // 
            cityLabel.AutoSize = true;
            cityLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            cityLabel.Location = new Point(12, 38);
            cityLabel.Name = "cityLabel";
            cityLabel.Size = new Size(51, 20);
            cityLabel.TabIndex = 22;
            cityLabel.Text = "label2";
            // 
            // quantityLabel
            // 
            quantityLabel.AutoSize = true;
            quantityLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            quantityLabel.Location = new Point(12, 167);
            quantityLabel.Name = "quantityLabel";
            quantityLabel.Size = new Size(51, 20);
            quantityLabel.TabIndex = 23;
            quantityLabel.Text = "label2";
            // 
            // ProductModifieForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(452, 257);
            Controls.Add(quantityLabel);
            Controls.Add(cityLabel);
            Controls.Add(groupLabel);
            Controls.Add(priceLabel);
            Controls.Add(expirationDateLabel);
            Controls.Add(historyBtn);
            Controls.Add(editBtn);
            Controls.Add(transferbtn);
            Controls.Add(productNameLabel);
            Controls.Add(deletebtn);
            Controls.Add(savebtn);
            Controls.Add(substractionRadio);
            Controls.Add(additionRadio);
            Controls.Add(quantityTextBox);
            Controls.Add(label1);
            Name = "ProductModifieForm";
            Text = "Form3";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox quantityTextBox;
        private RadioButton additionRadio;
        private RadioButton substractionRadio;
        private Button savebtn;
        private Button deletebtn;
        private Label productNameLabel;
        private Button transferbtn;
        private Button editBtn;
        private Button historyBtn;
        private Label expirationDateLabel;
        private Label priceLabel;
        private Label groupLabel;
        private Label cityLabel;
        private Label quantityLabel;
    }
}