namespace Production_Controll
{
    partial class ProductHistoryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductHistoryForm));
            dataGridViewProductModifications = new DataGridView();
            excelBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewProductModifications).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewProductModifications
            // 
            dataGridViewProductModifications.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewProductModifications.Location = new Point(-1, 1);
            dataGridViewProductModifications.Name = "dataGridViewProductModifications";
            dataGridViewProductModifications.RowHeadersWidth = 51;
            dataGridViewProductModifications.RowTemplate.Height = 29;
            dataGridViewProductModifications.Size = new Size(747, 403);
            dataGridViewProductModifications.TabIndex = 0;
            // 
            // excelBtn
            // 
            excelBtn.Image = (Image)resources.GetObject("excelBtn.Image");
            excelBtn.Location = new Point(738, 410);
            excelBtn.Name = "excelBtn";
            excelBtn.Size = new Size(50, 31);
            excelBtn.TabIndex = 11;
            excelBtn.UseVisualStyleBackColor = true;
            excelBtn.Click += excelBtn_Click;
            // 
            // ProductHistoryForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(excelBtn);
            Controls.Add(dataGridViewProductModifications);
            Name = "ProductHistoryForm";
            Text = "Form1";
            Load += ProductHistoryForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewProductModifications).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridViewProductModifications;
        private Button excelBtn;
    }
}