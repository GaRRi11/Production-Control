namespace Production_Controll
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            label1 = new Label();
            textBox1 = new TextBox();
            excelBtn = new Button();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            Save = new Button();
            button2 = new Button();
            productName = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(37, 81);
            label1.Name = "label1";
            label1.Size = new Size(78, 20);
            label1.TabIndex = 0;
            label1.Text = "Quantity: ";
            label1.Click += label1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(269, 81);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 1;
            // 
            // excelBtn
            // 
            excelBtn.Image = (Image)resources.GetObject("excelBtn.Image");
            excelBtn.Location = new Point(12, 214);
            excelBtn.Name = "excelBtn";
            excelBtn.Size = new Size(50, 31);
            excelBtn.TabIndex = 10;
            excelBtn.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            radioButton1.Location = new Point(37, 121);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(91, 24);
            radioButton1.TabIndex = 11;
            radioButton1.TabStop = true;
            radioButton1.Text = "Addition";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            radioButton2.Location = new Point(37, 163);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(118, 24);
            radioButton2.TabIndex = 12;
            radioButton2.TabStop = true;
            radioButton2.Text = "Substraction";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // Save
            // 
            Save.Font = new Font("Segoe UI", 7.8F, FontStyle.Bold, GraphicsUnit.Point);
            Save.Location = new Point(341, 200);
            Save.Name = "Save";
            Save.Size = new Size(99, 45);
            Save.TabIndex = 13;
            Save.Text = "Save";
            Save.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 7.8F, FontStyle.Bold, GraphicsUnit.Point);
            button2.Location = new Point(218, 200);
            button2.Name = "button2";
            button2.Size = new Size(99, 45);
            button2.TabIndex = 14;
            button2.Text = "Delete Product";
            button2.UseVisualStyleBackColor = true;
            // 
            // productName
            // 
            productName.AutoSize = true;
            productName.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            productName.Location = new Point(47, 30);
            productName.Name = "productName";
            productName.Size = new Size(51, 20);
            productName.TabIndex = 15;
            productName.Text = "label2";
            productName.Click += productName_Click;
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(452, 257);
            Controls.Add(productName);
            Controls.Add(button2);
            Controls.Add(Save);
            Controls.Add(radioButton2);
            Controls.Add(radioButton1);
            Controls.Add(excelBtn);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Name = "Form3";
            Text = "Form3";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private Button excelBtn;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private Button Save;
        private Button button2;
        private Label productName;
    }
}