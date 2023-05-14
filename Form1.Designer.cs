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
            this.isCtrlCheckBox = new System.Windows.Forms.CheckBox();
            this.isCrossSelectCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(3, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(801, 415);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            // 
            // isCtrlCheckBox
            // 
            this.isCtrlCheckBox.AutoSize = true;
            this.isCtrlCheckBox.Location = new System.Drawing.Point(13, 421);
            this.isCtrlCheckBox.Name = "isCtrlCheckBox";
            this.isCtrlCheckBox.Size = new System.Drawing.Size(54, 17);
            this.isCtrlCheckBox.TabIndex = 1;
            this.isCtrlCheckBox.Text = "CTRL";
            this.isCtrlCheckBox.UseVisualStyleBackColor = true;
            // 
            // isCrossSelectCheckBox
            // 
            this.isCrossSelectCheckBox.AutoSize = true;
            this.isCrossSelectCheckBox.Location = new System.Drawing.Point(100, 421);
            this.isCrossSelectCheckBox.Name = "isCrossSelectCheckBox";
            this.isCrossSelectCheckBox.Size = new System.Drawing.Size(158, 17);
            this.isCrossSelectCheckBox.TabIndex = 2;
            this.isCrossSelectCheckBox.Text = "Перекрестное выделение";
            this.isCrossSelectCheckBox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.isCrossSelectCheckBox);
            this.Controls.Add(this.isCtrlCheckBox);
            this.Controls.Add(this.pictureBox);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.CheckBox isCtrlCheckBox;
        private System.Windows.Forms.CheckBox isCrossSelectCheckBox;
    }
}

