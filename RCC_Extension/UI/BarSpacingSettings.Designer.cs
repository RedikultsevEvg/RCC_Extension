namespace RCC_Extension.UI
{
    partial class frmBarSpacingSettings
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
            this.label1 = new System.Windows.Forms.Label();
            this.nudMainSpacing = new System.Windows.Forms.NumericUpDown();
            this.lbLeftQuant = new System.Windows.Forms.Label();
            this.cbAddLeftBars = new System.Windows.Forms.CheckBox();
            this.nudLeftQuant = new System.Windows.Forms.NumericUpDown();
            this.nudLeftSpacing = new System.Windows.Forms.NumericUpDown();
            this.lbLeftSpacing = new System.Windows.Forms.Label();
            this.nudRightSpacing = new System.Windows.Forms.NumericUpDown();
            this.lbRightSpacing = new System.Windows.Forms.Label();
            this.nudRightQuant = new System.Windows.Forms.NumericUpDown();
            this.cbAddRightBars = new System.Windows.Forms.CheckBox();
            this.lbRightQuant = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbAdd = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudMainSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLeftQuant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLeftSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRightSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRightQuant)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Шаг основных стержней, мм";
            // 
            // nudMainSpacing
            // 
            this.nudMainSpacing.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudMainSpacing.Location = new System.Drawing.Point(181, 12);
            this.nudMainSpacing.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudMainSpacing.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudMainSpacing.Name = "nudMainSpacing";
            this.nudMainSpacing.Size = new System.Drawing.Size(120, 20);
            this.nudMainSpacing.TabIndex = 1;
            this.nudMainSpacing.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudMainSpacing.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // lbLeftQuant
            // 
            this.lbLeftQuant.AutoSize = true;
            this.lbLeftQuant.Location = new System.Drawing.Point(47, 90);
            this.lbLeftQuant.Name = "lbLeftQuant";
            this.lbLeftQuant.Size = new System.Drawing.Size(88, 13);
            this.lbLeftQuant.TabIndex = 2;
            this.lbLeftQuant.Text = "Количество, шт.";
            // 
            // cbAddLeftBars
            // 
            this.cbAddLeftBars.AutoSize = true;
            this.cbAddLeftBars.Checked = true;
            this.cbAddLeftBars.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAddLeftBars.Location = new System.Drawing.Point(15, 60);
            this.cbAddLeftBars.Name = "cbAddLeftBars";
            this.cbAddLeftBars.Size = new System.Drawing.Size(189, 17);
            this.cbAddLeftBars.TabIndex = 3;
            this.cbAddLeftBars.Text = "Добавить стержни слева/снизу";
            this.cbAddLeftBars.UseVisualStyleBackColor = true;
            this.cbAddLeftBars.CheckedChanged += new System.EventHandler(this.cbAddLeftBars_CheckedChanged);
            // 
            // nudLeftQuant
            // 
            this.nudLeftQuant.Location = new System.Drawing.Point(68, 106);
            this.nudLeftQuant.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudLeftQuant.Name = "nudLeftQuant";
            this.nudLeftQuant.Size = new System.Drawing.Size(80, 20);
            this.nudLeftQuant.TabIndex = 4;
            this.nudLeftQuant.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nudLeftSpacing
            // 
            this.nudLeftSpacing.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudLeftSpacing.Location = new System.Drawing.Point(68, 156);
            this.nudLeftSpacing.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudLeftSpacing.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudLeftSpacing.Name = "nudLeftSpacing";
            this.nudLeftSpacing.Size = new System.Drawing.Size(80, 20);
            this.nudLeftSpacing.TabIndex = 6;
            this.nudLeftSpacing.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudLeftSpacing.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // lbLeftSpacing
            // 
            this.lbLeftSpacing.AutoSize = true;
            this.lbLeftSpacing.Location = new System.Drawing.Point(47, 140);
            this.lbLeftSpacing.Name = "lbLeftSpacing";
            this.lbLeftSpacing.Size = new System.Drawing.Size(49, 13);
            this.lbLeftSpacing.TabIndex = 5;
            this.lbLeftSpacing.Text = "Шаг, мм";
            // 
            // nudRightSpacing
            // 
            this.nudRightSpacing.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudRightSpacing.Location = new System.Drawing.Point(303, 156);
            this.nudRightSpacing.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudRightSpacing.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudRightSpacing.Name = "nudRightSpacing";
            this.nudRightSpacing.Size = new System.Drawing.Size(80, 20);
            this.nudRightSpacing.TabIndex = 11;
            this.nudRightSpacing.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudRightSpacing.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // lbRightSpacing
            // 
            this.lbRightSpacing.AutoSize = true;
            this.lbRightSpacing.Location = new System.Drawing.Point(282, 140);
            this.lbRightSpacing.Name = "lbRightSpacing";
            this.lbRightSpacing.Size = new System.Drawing.Size(49, 13);
            this.lbRightSpacing.TabIndex = 10;
            this.lbRightSpacing.Text = "Шаг, мм";
            // 
            // nudRightQuant
            // 
            this.nudRightQuant.Location = new System.Drawing.Point(303, 106);
            this.nudRightQuant.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudRightQuant.Name = "nudRightQuant";
            this.nudRightQuant.Size = new System.Drawing.Size(80, 20);
            this.nudRightQuant.TabIndex = 9;
            this.nudRightQuant.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbAddRightBars
            // 
            this.cbAddRightBars.AutoSize = true;
            this.cbAddRightBars.Checked = true;
            this.cbAddRightBars.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAddRightBars.Location = new System.Drawing.Point(250, 60);
            this.cbAddRightBars.Name = "cbAddRightBars";
            this.cbAddRightBars.Size = new System.Drawing.Size(200, 17);
            this.cbAddRightBars.TabIndex = 8;
            this.cbAddRightBars.Text = "Добавить стержни справа/сверху";
            this.cbAddRightBars.UseVisualStyleBackColor = true;
            this.cbAddRightBars.CheckedChanged += new System.EventHandler(this.cbAddRightBars_CheckedChanged);
            // 
            // lbRightQuant
            // 
            this.lbRightQuant.AutoSize = true;
            this.lbRightQuant.Location = new System.Drawing.Point(282, 90);
            this.lbRightQuant.Name = "lbRightQuant";
            this.lbRightQuant.Size = new System.Drawing.Size(88, 13);
            this.lbRightQuant.TabIndex = 7;
            this.lbRightQuant.Text = "Количество, шт.";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(300, 230);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(391, 230);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cbAdd
            // 
            this.cbAdd.AutoSize = true;
            this.cbAdd.Checked = true;
            this.cbAdd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAdd.Location = new System.Drawing.Point(15, 192);
            this.cbAdd.Name = "cbAdd";
            this.cbAdd.Size = new System.Drawing.Size(208, 17);
            this.cbAdd.TabIndex = 14;
            this.cbAdd.Text = "Добавить обрамление слева/снизу";
            this.cbAdd.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(250, 192);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(219, 17);
            this.checkBox2.TabIndex = 15;
            this.checkBox2.Text = "Добавить обрамление справа/сверху";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // frmBarSpacingSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 265);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.cbAdd);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.nudRightSpacing);
            this.Controls.Add(this.lbRightSpacing);
            this.Controls.Add(this.nudRightQuant);
            this.Controls.Add(this.cbAddRightBars);
            this.Controls.Add(this.lbRightQuant);
            this.Controls.Add(this.nudLeftSpacing);
            this.Controls.Add(this.lbLeftSpacing);
            this.Controls.Add(this.nudLeftQuant);
            this.Controls.Add(this.cbAddLeftBars);
            this.Controls.Add(this.lbLeftQuant);
            this.Controls.Add(this.nudMainSpacing);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmBarSpacingSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки раскладки стержней";
            ((System.ComponentModel.ISupportInitialize)(this.nudMainSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLeftQuant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLeftSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRightSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRightQuant)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudMainSpacing;
        private System.Windows.Forms.Label lbLeftQuant;
        private System.Windows.Forms.CheckBox cbAddLeftBars;
        private System.Windows.Forms.NumericUpDown nudLeftQuant;
        private System.Windows.Forms.NumericUpDown nudLeftSpacing;
        private System.Windows.Forms.Label lbLeftSpacing;
        private System.Windows.Forms.NumericUpDown nudRightSpacing;
        private System.Windows.Forms.Label lbRightSpacing;
        private System.Windows.Forms.NumericUpDown nudRightQuant;
        private System.Windows.Forms.CheckBox cbAddRightBars;
        private System.Windows.Forms.Label lbRightQuant;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox cbAdd;
        private System.Windows.Forms.CheckBox checkBox2;
    }
}