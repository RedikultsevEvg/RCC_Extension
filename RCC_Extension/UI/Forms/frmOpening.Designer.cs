namespace RCC_Extension.UI.Forms
{
    partial class frmOpening
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
            this.cbOverrideBottom = new System.Windows.Forms.CheckBox();
            this.nudBottom = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudLeft = new System.Windows.Forms.NumericUpDown();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbWall = new System.Windows.Forms.TextBox();
            this.cbOpeningTypes = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cbOverrideBottom
            // 
            this.cbOverrideBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbOverrideBottom.AutoSize = true;
            this.cbOverrideBottom.Checked = true;
            this.cbOverrideBottom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOverrideBottom.Location = new System.Drawing.Point(159, 185);
            this.cbOverrideBottom.Name = "cbOverrideBottom";
            this.cbOverrideBottom.Size = new System.Drawing.Size(204, 17);
            this.cbOverrideBottom.TabIndex = 0;
            this.cbOverrideBottom.Text = "Перезаписать привязку по высоте";
            this.cbOverrideBottom.UseVisualStyleBackColor = true;
            this.cbOverrideBottom.CheckedChanged += new System.EventHandler(this.cbOverrideBottom_CheckedChanged);
            // 
            // nudBottom
            // 
            this.nudBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudBottom.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudBottom.Location = new System.Drawing.Point(171, 208);
            this.nudBottom.Name = "nudBottom";
            this.nudBottom.Size = new System.Drawing.Size(120, 20);
            this.nudBottom.TabIndex = 3;
            this.nudBottom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudBottom.ThousandsSeparator = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(156, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Привязка от начала стены";
            // 
            // nudLeft
            // 
            this.nudLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudLeft.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudLeft.Location = new System.Drawing.Point(171, 150);
            this.nudLeft.Name = "nudLeft";
            this.nudLeft.Size = new System.Drawing.Size(120, 20);
            this.nudLeft.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(284, 251);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(194, 251);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(138, 262);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(155, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Марка";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(155, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Стена";
            // 
            // tbWall
            // 
            this.tbWall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWall.Enabled = false;
            this.tbWall.Location = new System.Drawing.Point(171, 77);
            this.tbWall.Name = "tbWall";
            this.tbWall.Size = new System.Drawing.Size(188, 20);
            this.tbWall.TabIndex = 12;
            // 
            // cbOpeningTypes
            // 
            this.cbOpeningTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbOpeningTypes.FormattingEnabled = true;
            this.cbOpeningTypes.Location = new System.Drawing.Point(171, 28);
            this.cbOpeningTypes.Name = "cbOpeningTypes";
            this.cbOpeningTypes.Size = new System.Drawing.Size(188, 21);
            this.cbOpeningTypes.TabIndex = 13;
            // 
            // frmOpening
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 286);
            this.Controls.Add(this.cbOpeningTypes);
            this.Controls.Add(this.tbWall);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.nudLeft);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudBottom);
            this.Controls.Add(this.cbOverrideBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmOpening";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Проем";
            ((System.ComponentModel.ISupportInitialize)(this.nudBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbOverrideBottom;
        private System.Windows.Forms.NumericUpDown nudBottom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudLeft;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbWall;
        private System.Windows.Forms.ComboBox cbOpeningTypes;
    }
}