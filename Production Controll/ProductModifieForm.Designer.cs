﻿namespace Production_Controll
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
            textBox1 = new TextBox();
            additionRadio = new RadioButton();
            substractionRadio = new RadioButton();
            savebtn = new Button();
            deletebtn = new Button();
            productNameLabel = new Label();
            transferbtn = new Button();
            editBtn = new Button();
            historyBtn = new Button();
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
            // 
            // textBox1
            // 
            textBox1.Location = new Point(269, 81);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 1;
            // 
            // additionRadio
            // 
            additionRadio.AutoSize = true;
            additionRadio.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            additionRadio.Location = new Point(37, 121);
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
            substractionRadio.Location = new Point(37, 163);
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
            productNameLabel.Location = new Point(47, 30);
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
            // ProductModifieForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(452, 257);
            Controls.Add(historyBtn);
            Controls.Add(editBtn);
            Controls.Add(transferbtn);
            Controls.Add(productNameLabel);
            Controls.Add(deletebtn);
            Controls.Add(savebtn);
            Controls.Add(substractionRadio);
            Controls.Add(additionRadio);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Name = "ProductModifieForm";
            Text = "Form3";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private RadioButton additionRadio;
        private RadioButton substractionRadio;
        private Button savebtn;
        private Button deletebtn;
        private Label productNameLabel;
        private Button transferbtn;
        private Button editBtn;
        private Button historyBtn;
    }
}