namespace Production_Controll
{
    partial class ProductGroupForm
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
            listBox1 = new ListBox();
            productionAddBtn = new Button();
            deletebtn = new Button();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 20;
            listBox1.Location = new Point(12, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(293, 244);
            listBox1.TabIndex = 0;
            // 
            // productionAddBtn
            // 
            productionAddBtn.Font = new Font("Segoe UI Black", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            productionAddBtn.Location = new Point(12, 269);
            productionAddBtn.Name = "productionAddBtn";
            productionAddBtn.Size = new Size(50, 50);
            productionAddBtn.TabIndex = 11;
            productionAddBtn.Text = "+";
            productionAddBtn.UseVisualStyleBackColor = true;
            productionAddBtn.Click += productionAddBtn_Click;
            // 
            // deletebtn
            // 
            deletebtn.Font = new Font("Segoe UI", 7.8F, FontStyle.Bold, GraphicsUnit.Point);
            deletebtn.Image = Properties.Resources.delete;
            deletebtn.Location = new Point(255, 269);
            deletebtn.Name = "deletebtn";
            deletebtn.Size = new Size(50, 50);
            deletebtn.TabIndex = 15;
            deletebtn.UseVisualStyleBackColor = true;
            deletebtn.Click += deletebtn_Click;
            // 
            // ProductGroupForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(317, 331);
            Controls.Add(deletebtn);
            Controls.Add(productionAddBtn);
            Controls.Add(listBox1);
            Name = "ProductGroupForm";
            Text = "ProductGroupForm";
            ResumeLayout(false);
        }

        #endregion

        private ListBox listBox1;
        private Button productionAddBtn;
        private Button deletebtn;
    }
}