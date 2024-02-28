namespace Production_Controll
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tabControl1 = new TabControl();
            productionAddBtn = new Button();
            excelBtn = new Button();
            cityAddbtn = new Button();
            refreshBtn = new Button();
            emptyBtn = new Button();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            tabControl1.Location = new Point(-2, 35);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(802, 418);
            tabControl1.TabIndex = 0;
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
            productionAddBtn.Click += ProductionAddBtn_Click;
            // 
            // excelBtn
            // 
            excelBtn.Image = (Image)resources.GetObject("excelBtn.Image");
            excelBtn.Location = new Point(58, 2);
            excelBtn.Name = "excelBtn";
            excelBtn.Size = new Size(50, 31);
            excelBtn.TabIndex = 9;
            excelBtn.UseVisualStyleBackColor = true;
            excelBtn.Click += ExcelBtn_Click;
            // 
            // cityAddbtn
            // 
            cityAddbtn.Font = new Font("Segoe UI Black", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            cityAddbtn.Location = new Point(114, 2);
            cityAddbtn.Name = "cityAddbtn";
            cityAddbtn.Size = new Size(50, 31);
            cityAddbtn.TabIndex = 11;
            cityAddbtn.Text = "+";
            cityAddbtn.UseVisualStyleBackColor = true;
            cityAddbtn.Click += CityAddbtn_Click;
            // 
            // refreshBtn
            // 
            refreshBtn.Font = new Font("Segoe UI Black", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            refreshBtn.Image = (Image)resources.GetObject("refreshBtn.Image");
            refreshBtn.Location = new Point(170, 2);
            refreshBtn.Name = "refreshBtn";
            refreshBtn.Size = new Size(50, 31);
            refreshBtn.TabIndex = 12;
            refreshBtn.UseVisualStyleBackColor = true;
            refreshBtn.Click += refreshBtn_Click;
            // 
            // emptyBtn
            // 
            emptyBtn.Font = new Font("Segoe UI Black", 7F, FontStyle.Bold, GraphicsUnit.Point);
            emptyBtn.Image = Properties.Resources.clean__1_;
            emptyBtn.Location = new Point(226, 2);
            emptyBtn.Name = "emptyBtn";
            emptyBtn.Size = new Size(50, 31);
            emptyBtn.TabIndex = 13;
            emptyBtn.UseVisualStyleBackColor = true;
            emptyBtn.Click += emptyBtn_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(796, 447);
            Controls.Add(emptyBtn);
            Controls.Add(refreshBtn);
            Controls.Add(cityAddbtn);
            Controls.Add(excelBtn);
            Controls.Add(productionAddBtn);
            Controls.Add(tabControl1);
            Name = "MainForm";
            Text = "Form1";
            Load += MainForm_Load;
            ResumeLayout(false);
        }

        #endregion

        public TabControl tabControl1;
        private Button excelBtn;
        private Button productionAddBtn;
        private Button cityAddbtn;
        private Button refreshBtn;
        private Button emptyBtn;
    }
}