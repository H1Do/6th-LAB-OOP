namespace _6th_LAB_OOP
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.colorBtn = new System.Windows.Forms.Button();
            this.sharesListBox = new System.Windows.Forms.ListBox();
            this.chosedShare = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.Color.White;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(582, 470);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            // 
            // colorBtn
            // 
            this.colorBtn.Location = new System.Drawing.Point(12, 476);
            this.colorBtn.Name = "colorBtn";
            this.colorBtn.Size = new System.Drawing.Size(45, 48);
            this.colorBtn.TabIndex = 1;
            this.colorBtn.UseVisualStyleBackColor = true;
            this.colorBtn.Click += new System.EventHandler(this.colorBtn_Click);
            // 
            // sharesListBox
            // 
            this.sharesListBox.FormattingEnabled = true;
            this.sharesListBox.Items.AddRange(new object[] {
            "Circle",
            "Triangle",
            "Square",
            "Line"});
            this.sharesListBox.Location = new System.Drawing.Point(63, 476);
            this.sharesListBox.Name = "sharesListBox";
            this.sharesListBox.Size = new System.Drawing.Size(217, 56);
            this.sharesListBox.TabIndex = 3;
            this.sharesListBox.SelectedValueChanged += new System.EventHandler(this.listBox1_SelectedValueChanged);
            // 
            // chosedShare
            // 
            this.chosedShare.AutoSize = true;
            this.chosedShare.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chosedShare.Location = new System.Drawing.Point(295, 482);
            this.chosedShare.Name = "chosedShare";
            this.chosedShare.Size = new System.Drawing.Size(0, 42);
            this.chosedShare.TabIndex = 4;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(583, 536);
            this.Controls.Add(this.chosedShare);
            this.Controls.Add(this.sharesListBox);
            this.Controls.Add(this.colorBtn);
            this.Controls.Add(this.pictureBox);
            this.Name = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Button btnSquare;
        private System.Windows.Forms.Button btnTriangle;
        private System.Windows.Forms.Button btnCircle;
        private System.Windows.Forms.Panel colorPreview;
        private System.Windows.Forms.Label lbPreview;
        private System.Windows.Forms.Label lbChoose;
        private System.Windows.Forms.ComboBox colorList;
        private System.Windows.Forms.Button btnChangeColor;
        private System.Windows.Forms.Button btnUngroup;
        private System.Windows.Forms.Button btnGroup;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.PictureBox sheet;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TreeView shapesTree;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button colorBtn;
        private System.Windows.Forms.ListBox sharesListBox;
        private System.Windows.Forms.Label chosedShare;
    }
}

