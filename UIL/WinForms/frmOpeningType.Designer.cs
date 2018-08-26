namespace RDUIL.WinForms
{
    partial class frmOpeningType
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tbPurpose = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nudBottom = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lbQuantVertRight = new System.Windows.Forms.Label();
            this.nudQuantVertRight = new System.Windows.Forms.NumericUpDown();
            this.lbQuantVertLeft = new System.Windows.Forms.Label();
            this.nudQuantVertLeft = new System.Windows.Forms.NumericUpDown();
            this.cbMoveVert = new System.Windows.Forms.CheckBox();
            this.cbAddEdgeBottom = new System.Windows.Forms.CheckBox();
            this.cbAddEdgeTop = new System.Windows.Forms.CheckBox();
            this.cbAddEdgeRight = new System.Windows.Forms.CheckBox();
            this.cbAddEdgeLeft = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.nudQuantInclined = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.cbIncBottomRight = new System.Windows.Forms.CheckBox();
            this.cbIncBottomLeft = new System.Windows.Forms.CheckBox();
            this.cbIncTopRight = new System.Windows.Forms.CheckBox();
            this.cbIncTopLeft = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantVertRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantVertLeft)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantInclined)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(370, 196);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tbPurpose);
            this.tabPage1.Controls.Add(this.tbName);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.nudBottom);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.nudWidth);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.nudHeight);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(362, 170);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Основное";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tbPurpose
            // 
            this.tbPurpose.Location = new System.Drawing.Point(177, 34);
            this.tbPurpose.Name = "tbPurpose";
            this.tbPurpose.Size = new System.Drawing.Size(80, 20);
            this.tbPurpose.TabIndex = 11;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(39, 34);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(80, 20);
            this.tbName.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(161, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Назначение";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Марка";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(161, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Привязка снизу, мм";
            // 
            // nudBottom
            // 
            this.nudBottom.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudBottom.Location = new System.Drawing.Point(177, 77);
            this.nudBottom.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudBottom.Name = "nudBottom";
            this.nudBottom.Size = new System.Drawing.Size(80, 20);
            this.nudBottom.TabIndex = 6;
            this.nudBottom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ширина, мм";
            // 
            // nudWidth
            // 
            this.nudWidth.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudWidth.Location = new System.Drawing.Point(39, 125);
            this.nudWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.Size = new System.Drawing.Size(80, 20);
            this.nudWidth.TabIndex = 2;
            this.nudWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudWidth.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Высота, мм";
            // 
            // nudHeight
            // 
            this.nudHeight.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudHeight.Location = new System.Drawing.Point(39, 77);
            this.nudHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.Size = new System.Drawing.Size(80, 20);
            this.nudHeight.TabIndex = 0;
            this.nudHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudHeight.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lbQuantVertRight);
            this.tabPage2.Controls.Add(this.nudQuantVertRight);
            this.tabPage2.Controls.Add(this.lbQuantVertLeft);
            this.tabPage2.Controls.Add(this.nudQuantVertLeft);
            this.tabPage2.Controls.Add(this.cbMoveVert);
            this.tabPage2.Controls.Add(this.cbAddEdgeBottom);
            this.tabPage2.Controls.Add(this.cbAddEdgeTop);
            this.tabPage2.Controls.Add(this.cbAddEdgeRight);
            this.tabPage2.Controls.Add(this.cbAddEdgeLeft);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(362, 170);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Обрамления";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lbQuantVertRight
            // 
            this.lbQuantVertRight.AutoSize = true;
            this.lbQuantVertRight.Location = new System.Drawing.Point(210, 116);
            this.lbQuantVertRight.Name = "lbQuantVertRight";
            this.lbQuantVertRight.Size = new System.Drawing.Size(139, 13);
            this.lbQuantVertRight.TabIndex = 8;
            this.lbQuantVertRight.Text = "Участить стержни справа";
            // 
            // nudQuantVertRight
            // 
            this.nudQuantVertRight.Location = new System.Drawing.Point(225, 144);
            this.nudQuantVertRight.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudQuantVertRight.Name = "nudQuantVertRight";
            this.nudQuantVertRight.Size = new System.Drawing.Size(80, 20);
            this.nudQuantVertRight.TabIndex = 7;
            this.nudQuantVertRight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudQuantVertRight.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // lbQuantVertLeft
            // 
            this.lbQuantVertLeft.AutoSize = true;
            this.lbQuantVertLeft.Location = new System.Drawing.Point(56, 116);
            this.lbQuantVertLeft.Name = "lbQuantVertLeft";
            this.lbQuantVertLeft.Size = new System.Drawing.Size(133, 13);
            this.lbQuantVertLeft.TabIndex = 6;
            this.lbQuantVertLeft.Text = "Участить стержни слева";
            // 
            // nudQuantVertLeft
            // 
            this.nudQuantVertLeft.Location = new System.Drawing.Point(71, 144);
            this.nudQuantVertLeft.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudQuantVertLeft.Name = "nudQuantVertLeft";
            this.nudQuantVertLeft.Size = new System.Drawing.Size(80, 20);
            this.nudQuantVertLeft.TabIndex = 5;
            this.nudQuantVertLeft.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudQuantVertLeft.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // cbMoveVert
            // 
            this.cbMoveVert.AutoSize = true;
            this.cbMoveVert.Location = new System.Drawing.Point(23, 96);
            this.cbMoveVert.Name = "cbMoveVert";
            this.cbMoveVert.Size = new System.Drawing.Size(201, 17);
            this.cbMoveVert.TabIndex = 4;
            this.cbMoveVert.Text = "Развигать вертикальные стержни";
            this.cbMoveVert.UseVisualStyleBackColor = true;
            this.cbMoveVert.CheckedChanged += new System.EventHandler(this.cbMoveVert_CheckedChanged);
            // 
            // cbAddEdgeBottom
            // 
            this.cbAddEdgeBottom.AutoSize = true;
            this.cbAddEdgeBottom.Location = new System.Drawing.Point(213, 55);
            this.cbAddEdgeBottom.Name = "cbAddEdgeBottom";
            this.cbAddEdgeBottom.Size = new System.Drawing.Size(56, 17);
            this.cbAddEdgeBottom.TabIndex = 3;
            this.cbAddEdgeBottom.Text = "Снизу";
            this.cbAddEdgeBottom.UseVisualStyleBackColor = true;
            // 
            // cbAddEdgeTop
            // 
            this.cbAddEdgeTop.AutoSize = true;
            this.cbAddEdgeTop.Location = new System.Drawing.Point(213, 21);
            this.cbAddEdgeTop.Name = "cbAddEdgeTop";
            this.cbAddEdgeTop.Size = new System.Drawing.Size(61, 17);
            this.cbAddEdgeTop.TabIndex = 2;
            this.cbAddEdgeTop.Text = "Сверху";
            this.cbAddEdgeTop.UseVisualStyleBackColor = true;
            // 
            // cbAddEdgeRight
            // 
            this.cbAddEdgeRight.AutoSize = true;
            this.cbAddEdgeRight.Location = new System.Drawing.Point(23, 55);
            this.cbAddEdgeRight.Name = "cbAddEdgeRight";
            this.cbAddEdgeRight.Size = new System.Drawing.Size(63, 17);
            this.cbAddEdgeRight.TabIndex = 1;
            this.cbAddEdgeRight.Text = "Справа";
            this.cbAddEdgeRight.UseVisualStyleBackColor = true;
            // 
            // cbAddEdgeLeft
            // 
            this.cbAddEdgeLeft.AutoSize = true;
            this.cbAddEdgeLeft.Location = new System.Drawing.Point(23, 21);
            this.cbAddEdgeLeft.Name = "cbAddEdgeLeft";
            this.cbAddEdgeLeft.Size = new System.Drawing.Size(57, 17);
            this.cbAddEdgeLeft.TabIndex = 0;
            this.cbAddEdgeLeft.Text = "Слева";
            this.cbAddEdgeLeft.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.nudQuantInclined);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.cbIncBottomRight);
            this.tabPage3.Controls.Add(this.cbIncBottomLeft);
            this.tabPage3.Controls.Add(this.cbIncTopRight);
            this.tabPage3.Controls.Add(this.cbIncTopLeft);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(362, 170);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Диагонали";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // nudQuantInclined
            // 
            this.nudQuantInclined.Location = new System.Drawing.Point(105, 16);
            this.nudQuantInclined.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudQuantInclined.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudQuantInclined.Name = "nudQuantInclined";
            this.nudQuantInclined.Size = new System.Drawing.Size(100, 20);
            this.nudQuantInclined.TabIndex = 7;
            this.nudQuantInclined.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudQuantInclined.ThousandsSeparator = true;
            this.nudQuantInclined.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Количество";
            // 
            // cbIncBottomRight
            // 
            this.cbIncBottomRight.AutoSize = true;
            this.cbIncBottomRight.Location = new System.Drawing.Point(220, 128);
            this.cbIncBottomRight.Name = "cbIncBottomRight";
            this.cbIncBottomRight.Size = new System.Drawing.Size(89, 17);
            this.cbIncBottomRight.TabIndex = 3;
            this.cbIncBottomRight.Text = "Снизу слева";
            this.cbIncBottomRight.UseVisualStyleBackColor = true;
            // 
            // cbIncBottomLeft
            // 
            this.cbIncBottomLeft.AutoSize = true;
            this.cbIncBottomLeft.Location = new System.Drawing.Point(24, 128);
            this.cbIncBottomLeft.Name = "cbIncBottomLeft";
            this.cbIncBottomLeft.Size = new System.Drawing.Size(89, 17);
            this.cbIncBottomLeft.TabIndex = 2;
            this.cbIncBottomLeft.Text = "Снизу слева";
            this.cbIncBottomLeft.UseVisualStyleBackColor = true;
            // 
            // cbIncTopRight
            // 
            this.cbIncTopRight.AutoSize = true;
            this.cbIncTopRight.Location = new System.Drawing.Point(220, 52);
            this.cbIncTopRight.Name = "cbIncTopRight";
            this.cbIncTopRight.Size = new System.Drawing.Size(100, 17);
            this.cbIncTopRight.TabIndex = 1;
            this.cbIncTopRight.Text = "Сверху справа";
            this.cbIncTopRight.UseVisualStyleBackColor = true;
            // 
            // cbIncTopLeft
            // 
            this.cbIncTopLeft.AutoSize = true;
            this.cbIncTopLeft.Location = new System.Drawing.Point(24, 52);
            this.cbIncTopLeft.Name = "cbIncTopLeft";
            this.cbIncTopLeft.Size = new System.Drawing.Size(94, 17);
            this.cbIncTopLeft.TabIndex = 0;
            this.cbIncTopLeft.Text = "Сверху слева";
            this.cbIncTopLeft.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(303, 214);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(212, 214);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmOpeningType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 246);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmOpeningType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Тип отверстия";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantVertRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantVertLeft)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantInclined)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudBottom;
        private System.Windows.Forms.Label lbQuantVertLeft;
        private System.Windows.Forms.NumericUpDown nudQuantVertLeft;
        private System.Windows.Forms.CheckBox cbMoveVert;
        private System.Windows.Forms.CheckBox cbAddEdgeBottom;
        private System.Windows.Forms.CheckBox cbAddEdgeTop;
        private System.Windows.Forms.CheckBox cbAddEdgeRight;
        private System.Windows.Forms.CheckBox cbAddEdgeLeft;
        private System.Windows.Forms.Label lbQuantVertRight;
        private System.Windows.Forms.NumericUpDown nudQuantVertRight;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox tbPurpose;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.NumericUpDown nudQuantInclined;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbIncBottomRight;
        private System.Windows.Forms.CheckBox cbIncBottomLeft;
        private System.Windows.Forms.CheckBox cbIncTopRight;
        private System.Windows.Forms.CheckBox cbIncTopLeft;
    }
}