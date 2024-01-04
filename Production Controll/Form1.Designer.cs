namespace Production_Controll
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            productionAddBtn = new Button();
            excelBtn = new Button();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            tabControl1.Location = new Point(-2, 35);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(802, 418);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(794, 385);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "თბილისი";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(794, 385);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "ქუთაისი";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // productionAddBtn
            // 
            productionAddBtn.Font = new Font("Segoe UI Black", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            productionAddBtn.Location = new Point(2, 2);
            productionAddBtn.Name = "productionAddBtn";
            productionAddBtn.Size = new Size(50, 31);
            productionAddBtn.TabIndex = 10;
            productionAddBtn.Text = "+";
            productionAddBtn.UseVisualStyleBackColor = true;
            productionAddBtn.Click += productionAddBtn_Click;
            // 
            // excelBtn
            // 
            excelBtn.Image = (Image)resources.GetObject("excelBtn.Image");
            excelBtn.Location = new Point(58, 2);
            excelBtn.Name = "excelBtn";
            excelBtn.Size = new Size(50, 31);
            excelBtn.TabIndex = 9;
            excelBtn.UseVisualStyleBackColor = true;
            excelBtn.Click += excelBtn_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(796, 447);
            Controls.Add(excelBtn);
            Controls.Add(productionAddBtn);
            Controls.Add(tabControl1);
            Name = "Form1";
            Text = "Form1";
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        public TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button excelBtn;
        private Button productionAddBtn;
    }
}