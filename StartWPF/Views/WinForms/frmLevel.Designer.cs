namespace RDUIL.WinForms
{
    partial class frmLevel
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.nudFlooLevel = new System.Windows.Forms.NumericUpDown();
            this.nudTopOffset = new System.Windows.Forms.NumericUpDown();
            this.nudQuant = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFlooLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuant)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(240, 176);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(145, 176);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Имя";
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName.Location = new System.Drawing.Point(26, 34);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(289, 20);
            this.tbName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Уровень пола, м";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(177, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Высота, мм";
            // 
            // nudHeight
            // 
            this.nudHeight.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudHeight.Location = new System.Drawing.Point(191, 84);
            this.nudHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.Size = new System.Drawing.Size(120, 20);
            this.nudHeight.TabIndex = 7;
            this.nudHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nudFlooLevel
            // 
            this.nudFlooLevel.DecimalPlaces = 3;
            this.nudFlooLevel.Increment = new decimal(new int[] {
            10,
            0,
            0,
            196608});
            this.nudFlooLevel.Location = new System.Drawing.Point(26, 84);
            this.nudFlooLevel.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudFlooLevel.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.nudFlooLevel.Name = "nudFlooLevel";
            this.nudFlooLevel.Size = new System.Drawing.Size(120, 20);
            this.nudFlooLevel.TabIndex = 8;
            this.nudFlooLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudFlooLevel.ThousandsSeparator = true;
            // 
            // nudTopOffset
            // 
            this.nudTopOffset.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudTopOffset.Location = new System.Drawing.Point(26, 139);
            this.nudTopOffset.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudTopOffset.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.nudTopOffset.Name = "nudTopOffset";
            this.nudTopOffset.Size = new System.Drawing.Size(120, 20);
            this.nudTopOffset.TabIndex = 12;
            this.nudTopOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nudQuant
            // 
            this.nudQuant.Location = new System.Drawing.Point(191, 139);
            this.nudQuant.Name = "nudQuant";
            this.nudQuant.Size = new System.Drawing.Size(120, 20);
            this.nudQuant.TabIndex = 11;
            this.nudQuant.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(177, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Количество, шт.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Офсет сверху, мм";
            // 
            // frmLevel
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(327, 211);
            this.Controls.Add(this.nudTopOffset);
            this.Controls.Add(this.nudQuant);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nudFlooLevel);
            this.Controls.Add(this.nudHeight);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmLevel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Уровень";
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFlooLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuant)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private System.Windows.Forms.NumericUpDown nudFlooLevel;
        private System.Windows.Forms.NumericUpDown nudTopOffset;
        private System.Windows.Forms.NumericUpDown nudQuant;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}