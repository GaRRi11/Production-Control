﻿namespace Production_Controll
{
    partial class CityAddForm
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
            label1 = new Label();
            label2 = new Label();
            nameTextBox = new TextBox();
            capacityTextBox = new TextBox();
            savebtn = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(34, 64);
            label1.Name = "label1";
            label1.Size = new Size(51, 20);
            label1.TabIndex = 1;
            label1.Text = "Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(34, 123);
            label2.Name = "label2";
            label2.Size = new Size(68, 20);
            label2.TabIndex = 2;
            label2.Text = "Capacity";
            // 
            // nameTextBox
            // 
            nameTextBox.Location = new Point(232, 57);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(125, 27);
            nameTextBox.TabIndex = 3;
            // 
            // capacityTextBox
            // 
            capacityTextBox.Location = new Point(232, 116);
            capacityTextBox.Name = "capacityTextBox";
            capacityTextBox.Size = new Size(125, 27);
            capacityTextBox.TabIndex = 4;
            capacityTextBox.KeyPress += capacityTextBox_KeyPress;
            // 
            // savebtn
            // 
            savebtn.Font = new Font("Segoe UI", 7.8F, FontStyle.Bold, GraphicsUnit.Point);
            savebtn.Location = new Point(341, 200);
            savebtn.Name = "savebtn";
            savebtn.Size = new Size(99, 45);
            savebtn.TabIndex = 14;
            savebtn.Text = "Save";
            savebtn.UseVisualStyleBackColor = true;
            savebtn.Click += savebtn_Click;
            // 
            // CityAddForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(452, 257);
            Controls.Add(savebtn);
            Controls.Add(capacityTextBox);
            Controls.Add(nameTextBox);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "CityAddForm";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox nameTextBox;
        private TextBox capacityTextBox;
        private Button savebtn;
    }
}