namespace RCC_Extension.UI
{
    partial class frmWallType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWallType));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nudBottomOffset = new System.Windows.Forms.NumericUpDown();
            this.nudTopOffset = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.nudBarTopOffset = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.cbRoundVertToBaseLength = new System.Windows.Forms.CheckBox();
            this.nudVertBaseLength = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.cbAddHorLapping = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.nudHorLappingLength = new System.Windows.Forms.NumericUpDown();
            this.lbHorBaseLength = new System.Windows.Forms.Label();
            this.nudHorBaseLength = new System.Windows.Forms.NumericUpDown();
            this.nudThickness = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tbVertSpacing = new System.Windows.Forms.TextBox();
            this.tbHorSpacing = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudBottomOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBarTopOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVertBaseLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHorLappingLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHorBaseLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThickness)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(475, 359);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(568, 359);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Марка";
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName.Location = new System.Drawing.Point(30, 38);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(613, 20);
            this.tbName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Бетон";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(207, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Вертикальные стержни";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(167, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Смещение от уровня снизу, мм";
            // 
            // nudBottomOffset
            // 
            this.nudBottomOffset.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudBottomOffset.Location = new System.Drawing.Point(46, 191);
            this.nudBottomOffset.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudBottomOffset.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudBottomOffset.Name = "nudBottomOffset";
            this.nudBottomOffset.Size = new System.Drawing.Size(120, 20);
            this.nudBottomOffset.TabIndex = 7;
            this.nudBottomOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nudTopOffset
            // 
            this.nudTopOffset.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudTopOffset.Location = new System.Drawing.Point(46, 255);
            this.nudTopOffset.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudTopOffset.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudTopOffset.Name = "nudTopOffset";
            this.nudTopOffset.Size = new System.Drawing.Size(120, 20);
            this.nudTopOffset.TabIndex = 9;
            this.nudTopOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudTopOffset.ThousandsSeparator = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 229);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(172, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Смещение от уровня сверху, мм";
            // 
            // nudBarTopOffset
            // 
            this.nudBarTopOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudBarTopOffset.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudBarTopOffset.Location = new System.Drawing.Point(247, 127);
            this.nudBarTopOffset.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudBarTopOffset.Name = "nudBarTopOffset";
            this.nudBarTopOffset.Size = new System.Drawing.Size(120, 20);
            this.nudBarTopOffset.TabIndex = 11;
            this.nudBarTopOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudBarTopOffset.ThousandsSeparator = true;
            this.nudBarTopOffset.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(228, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(172, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Смещение от уровня сверху, мм";
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.Location = new System.Drawing.Point(231, 292);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(136, 42);
            this.button3.TabIndex = 15;
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // cbRoundVertToBaseLength
            // 
            this.cbRoundVertToBaseLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRoundVertToBaseLength.AutoSize = true;
            this.cbRoundVertToBaseLength.Checked = true;
            this.cbRoundVertToBaseLength.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRoundVertToBaseLength.Location = new System.Drawing.Point(231, 165);
            this.cbRoundVertToBaseLength.Name = "cbRoundVertToBaseLength";
            this.cbRoundVertToBaseLength.Size = new System.Drawing.Size(142, 17);
            this.cbRoundVertToBaseLength.TabIndex = 16;
            this.cbRoundVertToBaseLength.Text = "Длина кратно базовой";
            this.cbRoundVertToBaseLength.UseVisualStyleBackColor = true;
            this.cbRoundVertToBaseLength.CheckedChanged += new System.EventHandler(this.cbAddVertLapping_CheckedChanged);
            // 
            // nudVertBaseLength
            // 
            this.nudVertBaseLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudVertBaseLength.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudVertBaseLength.Location = new System.Drawing.Point(247, 191);
            this.nudVertBaseLength.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudVertBaseLength.Name = "nudVertBaseLength";
            this.nudVertBaseLength.Size = new System.Drawing.Size(120, 20);
            this.nudVertBaseLength.TabIndex = 17;
            this.nudVertBaseLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudVertBaseLength.ThousandsSeparator = true;
            this.nudVertBaseLength.Value = new decimal(new int[] {
            11700,
            0,
            0,
            0});
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(231, 340);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(136, 42);
            this.button1.TabIndex = 18;
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbAddHorLapping
            // 
            this.cbAddHorLapping.AutoSize = true;
            this.cbAddHorLapping.Location = new System.Drawing.Point(454, 97);
            this.cbAddHorLapping.Name = "cbAddHorLapping";
            this.cbAddHorLapping.Size = new System.Drawing.Size(139, 17);
            this.cbAddHorLapping.TabIndex = 19;
            this.cbAddHorLapping.Text = "Учесть перехлест, мм";
            this.cbAddHorLapping.UseVisualStyleBackColor = true;
            this.cbAddHorLapping.CheckedChanged += new System.EventHandler(this.cbAddHorLapping_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(437, 77);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(138, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "Горизонтальные стержни";
            // 
            // nudHorLappingLength
            // 
            this.nudHorLappingLength.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudHorLappingLength.Location = new System.Drawing.Point(473, 127);
            this.nudHorLappingLength.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudHorLappingLength.Name = "nudHorLappingLength";
            this.nudHorLappingLength.Size = new System.Drawing.Size(120, 20);
            this.nudHorLappingLength.TabIndex = 21;
            this.nudHorLappingLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudHorLappingLength.ThousandsSeparator = true;
            this.nudHorLappingLength.Value = new decimal(new int[] {
            400,
            0,
            0,
            0});
            // 
            // lbHorBaseLength
            // 
            this.lbHorBaseLength.AutoSize = true;
            this.lbHorBaseLength.Location = new System.Drawing.Point(451, 169);
            this.lbHorBaseLength.Name = "lbHorBaseLength";
            this.lbHorBaseLength.Size = new System.Drawing.Size(83, 13);
            this.lbHorBaseLength.TabIndex = 22;
            this.lbHorBaseLength.Text = "Базовая длина";
            // 
            // nudHorBaseLength
            // 
            this.nudHorBaseLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudHorBaseLength.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudHorBaseLength.Location = new System.Drawing.Point(473, 191);
            this.nudHorBaseLength.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudHorBaseLength.Name = "nudHorBaseLength";
            this.nudHorBaseLength.Size = new System.Drawing.Size(120, 20);
            this.nudHorBaseLength.TabIndex = 23;
            this.nudHorBaseLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudHorBaseLength.Value = new decimal(new int[] {
            11700,
            0,
            0,
            0});
            // 
            // nudThickness
            // 
            this.nudThickness.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudThickness.Location = new System.Drawing.Point(46, 127);
            this.nudThickness.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudThickness.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudThickness.Name = "nudThickness";
            this.nudThickness.Size = new System.Drawing.Size(120, 20);
            this.nudThickness.TabIndex = 25;
            this.nudThickness.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudThickness.ThousandsSeparator = true;
            this.nudThickness.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(27, 101);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Толщина, мм";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(27, 292);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(136, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Раскладка вертикальная";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(30, 340);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(147, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "Раскладка горизонтальная";
            // 
            // tbVertSpacing
            // 
            this.tbVertSpacing.Enabled = false;
            this.tbVertSpacing.Location = new System.Drawing.Point(46, 314);
            this.tbVertSpacing.Name = "tbVertSpacing";
            this.tbVertSpacing.Size = new System.Drawing.Size(117, 20);
            this.tbVertSpacing.TabIndex = 28;
            // 
            // tbHorSpacing
            // 
            this.tbHorSpacing.Enabled = false;
            this.tbHorSpacing.Location = new System.Drawing.Point(46, 361);
            this.tbHorSpacing.Name = "tbHorSpacing";
            this.tbHorSpacing.Size = new System.Drawing.Size(117, 20);
            this.tbHorSpacing.TabIndex = 29;
            // 
            // frmWallType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 394);
            this.Controls.Add(this.tbHorSpacing);
            this.Controls.Add(this.tbVertSpacing);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.nudThickness);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.nudHorBaseLength);
            this.Controls.Add(this.lbHorBaseLength);
            this.Controls.Add(this.nudHorLappingLength);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cbAddHorLapping);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.nudVertBaseLength);
            this.Controls.Add(this.cbRoundVertToBaseLength);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.nudBarTopOffset);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nudTopOffset);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nudBottomOffset);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmWallType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактировать тип стены";
            ((System.ComponentModel.ISupportInitialize)(this.nudBottomOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBarTopOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVertBaseLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHorLappingLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHorBaseLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThickness)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudBottomOffset;
        private System.Windows.Forms.NumericUpDown nudTopOffset;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudBarTopOffset;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox cbRoundVertToBaseLength;
        private System.Windows.Forms.NumericUpDown nudVertBaseLength;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cbAddHorLapping;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudHorLappingLength;
        private System.Windows.Forms.Label lbHorBaseLength;
        private System.Windows.Forms.NumericUpDown nudHorBaseLength;
        private System.Windows.Forms.NumericUpDown nudThickness;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbVertSpacing;
        private System.Windows.Forms.TextBox tbHorSpacing;
    }
}