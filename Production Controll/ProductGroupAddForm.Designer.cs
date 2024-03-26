namespace Production_Controll
{
    partial class ProductGroupAddForm
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
            LiterTextBox = new TextBox();
            label3 = new Label();
            PackagingTypeTextBox = new TextBox();
            savebtn = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(40, 59);
            label1.Name = "label1";
            label1.Size = new Size(51, 20);
            label1.TabIndex = 2;
            label1.Text = "Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(40, 169);
            label2.Name = "label2";
            label2.Size = new Size(41, 20);
            label2.TabIndex = 3;
            label2.Text = "Liter";
            // 
            // nameTextBox
            // 
            nameTextBox.Location = new Point(239, 56);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(125, 27);
            nameTextBox.TabIndex = 4;
            nameTextBox.KeyPress += nameTextBox_KeyPress;
            // 
            // LiterTextBox
            // 
            LiterTextBox.Location = new Point(239, 162);
            LiterTextBox.Name = "LiterTextBox";
            LiterTextBox.Size = new Size(125, 27);
            LiterTextBox.TabIndex = 5;
            LiterTextBox.KeyPress += literTextBox_KeyPress;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(40, 107);
            label3.Name = "label3";
            label3.Size = new Size(117, 20);
            label3.TabIndex = 6;
            label3.Text = "Packaging Type";
            // 
            // PackagingTypeTextBox
            // 
            PackagingTypeTextBox.Location = new Point(239, 104);
            PackagingTypeTextBox.Name = "PackagingTypeTextBox";
            PackagingTypeTextBox.Size = new Size(125, 27);
            PackagingTypeTextBox.TabIndex = 7;
            PackagingTypeTextBox.KeyPress += packagingTypeTextBox_KeyPress;
            // 
            // savebtn
            // 
            savebtn.Font = new Font("Segoe UI", 7.8F, FontStyle.Bold, GraphicsUnit.Point);
            savebtn.Location = new Point(341, 200);
            savebtn.Name = "savebtn";
            savebtn.Size = new Size(99, 45);
            savebtn.TabIndex = 15;
            savebtn.Text = "Save";
            savebtn.UseVisualStyleBackColor = true;
            savebtn.Click += savebtn_Click;
            // 
            // ProductGroupAddForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(452, 257);
            Controls.Add(savebtn);
            Controls.Add(PackagingTypeTextBox);
            Controls.Add(label3);
            Controls.Add(LiterTextBox);
            Controls.Add(nameTextBox);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "ProductGroupAddForm";
            Text = "ProductionGroupAddForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox nameTextBox;
        private TextBox LiterTextBox;
        private Label label3;
        private TextBox PackagingTypeTextBox;
        private Button savebtn;
    }
}