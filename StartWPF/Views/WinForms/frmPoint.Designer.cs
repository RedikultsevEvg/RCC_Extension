namespace RDUIL.WinForms
{
    partial class frmPoint
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
            this.tbCoord = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nudCoord_X = new System.Windows.Forms.NumericUpDown();
            this.nudCoord_Y = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudCoord_X)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCoord_Y)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Список";
            // 
            // tbCoord
            // 
            this.tbCoord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCoord.Enabled = false;
            this.tbCoord.Location = new System.Drawing.Point(33, 25);
            this.tbCoord.Name = "tbCoord";
            this.tbCoord.Size = new System.Drawing.Size(211, 20);
            this.tbCoord.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Координата X";
            // 
            // nudCoord_X
            // 
            this.nudCoord_X.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudCoord_X.Location = new System.Drawing.Point(33, 73);
            this.nudCoord_X.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudCoord_X.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nudCoord_X.Name = "nudCoord_X";
            this.nudCoord_X.Size = new System.Drawing.Size(89, 20);
            this.nudCoord_X.TabIndex = 3;
            this.nudCoord_X.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudCoord_X.ThousandsSeparator = true;
            // 
            // nudCoord_Y
            // 
            this.nudCoord_Y.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudCoord_Y.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudCoord_Y.Location = new System.Drawing.Point(155, 73);
            this.nudCoord_Y.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudCoord_Y.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nudCoord_Y.Name = "nudCoord_Y";
            this.nudCoord_Y.Size = new System.Drawing.Size(89, 20);
            this.nudCoord_Y.TabIndex = 5;
            this.nudCoord_Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(134, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Координата Y";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(169, 115);
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
            this.btnOK.Location = new System.Drawing.Point(78, 115);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmPoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 150);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.nudCoord_Y);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudCoord_X);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbCoord);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmPoint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактирование точки";
            ((System.ComponentModel.ISupportInitialize)(this.nudCoord_X)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCoord_Y)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbCoord;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudCoord_X;
        private System.Windows.Forms.NumericUpDown nudCoord_Y;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}