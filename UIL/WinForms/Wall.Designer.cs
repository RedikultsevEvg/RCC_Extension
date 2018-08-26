namespace RDUIL.WinForms
{
    partial class frmWall
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWall));
            this.tbMain = new System.Windows.Forms.TabControl();
            this.tbpMain = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.cbWallTypes = new System.Windows.Forms.ComboBox();
            this.cbLevels = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbpFormWork = new System.Windows.Forms.TabPage();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.cbRewriteHeight = new System.Windows.Forms.CheckBox();
            this.nudConcreteEndOffset = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.nudConcreteStartOffset = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.btnEndPoint = new System.Windows.Forms.Button();
            this.tbEndCoord = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnStartPoint = new System.Windows.Forms.Button();
            this.tbStartCoord = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbpReinforcement = new System.Windows.Forms.TabPage();
            this.tbHorSpacing = new System.Windows.Forms.TextBox();
            this.tbVertSpacing = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.nudReinforcementEndOffset = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.nudReinforcementStartOffset = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnVertSpacingSetting = new System.Windows.Forms.Button();
            this.cbOverrideVertSpacing = new System.Windows.Forms.CheckBox();
            this.cbOverrideHorSpacing = new System.Windows.Forms.CheckBox();
            this.btnHorSpacingSetting = new System.Windows.Forms.Button();
            this.tbMain.SuspendLayout();
            this.tbpMain.SuspendLayout();
            this.tbpFormWork.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudConcreteEndOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudConcreteStartOffset)).BeginInit();
            this.tbpReinforcement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudReinforcementEndOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudReinforcementStartOffset)).BeginInit();
            this.SuspendLayout();
            // 
            // tbMain
            // 
            this.tbMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMain.Controls.Add(this.tbpMain);
            this.tbMain.Controls.Add(this.tbpFormWork);
            this.tbMain.Controls.Add(this.tbpReinforcement);
            this.tbMain.Location = new System.Drawing.Point(12, 12);
            this.tbMain.Name = "tbMain";
            this.tbMain.SelectedIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(519, 270);
            this.tbMain.TabIndex = 0;
            // 
            // tbpMain
            // 
            this.tbpMain.Controls.Add(this.label12);
            this.tbpMain.Controls.Add(this.cbWallTypes);
            this.tbpMain.Controls.Add(this.cbLevels);
            this.tbpMain.Controls.Add(this.label3);
            this.tbpMain.Controls.Add(this.label2);
            this.tbpMain.Controls.Add(this.tbName);
            this.tbpMain.Controls.Add(this.label1);
            this.tbpMain.Location = new System.Drawing.Point(4, 22);
            this.tbpMain.Name = "tbpMain";
            this.tbpMain.Padding = new System.Windows.Forms.Padding(3);
            this.tbpMain.Size = new System.Drawing.Size(511, 232);
            this.tbpMain.TabIndex = 0;
            this.tbpMain.Text = "Основное";
            this.tbpMain.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(240, 34);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(19, 13);
            this.label12.TabIndex = 7;
            this.label12.Text = "----";
            // 
            // cbWallTypes
            // 
            this.cbWallTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbWallTypes.FormattingEnabled = true;
            this.cbWallTypes.Location = new System.Drawing.Point(31, 31);
            this.cbWallTypes.Name = "cbWallTypes";
            this.cbWallTypes.Size = new System.Drawing.Size(191, 21);
            this.cbWallTypes.TabIndex = 6;
            // 
            // cbLevels
            // 
            this.cbLevels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLevels.Enabled = false;
            this.cbLevels.FormattingEnabled = true;
            this.cbLevels.Location = new System.Drawing.Point(31, 81);
            this.cbLevels.Name = "cbLevels";
            this.cbLevels.Size = new System.Drawing.Size(474, 21);
            this.cbLevels.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Тип стены";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Уровень";
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName.Location = new System.Drawing.Point(280, 32);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(225, 20);
            this.tbName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(268, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Наименование";
            // 
            // tbpFormWork
            // 
            this.tbpFormWork.Controls.Add(this.nudHeight);
            this.tbpFormWork.Controls.Add(this.cbRewriteHeight);
            this.tbpFormWork.Controls.Add(this.nudConcreteEndOffset);
            this.tbpFormWork.Controls.Add(this.label7);
            this.tbpFormWork.Controls.Add(this.nudConcreteStartOffset);
            this.tbpFormWork.Controls.Add(this.label6);
            this.tbpFormWork.Controls.Add(this.btnEndPoint);
            this.tbpFormWork.Controls.Add(this.tbEndCoord);
            this.tbpFormWork.Controls.Add(this.label5);
            this.tbpFormWork.Controls.Add(this.btnStartPoint);
            this.tbpFormWork.Controls.Add(this.tbStartCoord);
            this.tbpFormWork.Controls.Add(this.label4);
            this.tbpFormWork.Location = new System.Drawing.Point(4, 22);
            this.tbpFormWork.Name = "tbpFormWork";
            this.tbpFormWork.Padding = new System.Windows.Forms.Padding(3);
            this.tbpFormWork.Size = new System.Drawing.Size(511, 232);
            this.tbpFormWork.TabIndex = 1;
            this.tbpFormWork.Text = "Опалубка";
            this.tbpFormWork.UseVisualStyleBackColor = true;
            // 
            // nudHeight
            // 
            this.nudHeight.Enabled = false;
            this.nudHeight.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudHeight.Location = new System.Drawing.Point(374, 32);
            this.nudHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudHeight.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.Size = new System.Drawing.Size(100, 20);
            this.nudHeight.TabIndex = 11;
            this.nudHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudHeight.ThousandsSeparator = true;
            this.nudHeight.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // cbRewriteHeight
            // 
            this.cbRewriteHeight.AutoSize = true;
            this.cbRewriteHeight.Location = new System.Drawing.Point(351, 16);
            this.cbRewriteHeight.Name = "cbRewriteHeight";
            this.cbRewriteHeight.Size = new System.Drawing.Size(150, 17);
            this.cbRewriteHeight.TabIndex = 10;
            this.cbRewriteHeight.Text = "Переопределить высоту";
            this.cbRewriteHeight.UseVisualStyleBackColor = true;
            this.cbRewriteHeight.CheckedChanged += new System.EventHandler(this.cbRewriteHeight_CheckedChanged);
            // 
            // nudConcreteEndOffset
            // 
            this.nudConcreteEndOffset.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudConcreteEndOffset.Location = new System.Drawing.Point(192, 147);
            this.nudConcreteEndOffset.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudConcreteEndOffset.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudConcreteEndOffset.Name = "nudConcreteEndOffset";
            this.nudConcreteEndOffset.Size = new System.Drawing.Size(100, 20);
            this.nudConcreteEndOffset.TabIndex = 9;
            this.nudConcreteEndOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudConcreteEndOffset.ThousandsSeparator = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(173, 131);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Офсет в конце";
            // 
            // nudConcreteStartOffset
            // 
            this.nudConcreteStartOffset.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudConcreteStartOffset.Location = new System.Drawing.Point(192, 32);
            this.nudConcreteStartOffset.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudConcreteStartOffset.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudConcreteStartOffset.Name = "nudConcreteStartOffset";
            this.nudConcreteStartOffset.Size = new System.Drawing.Size(100, 20);
            this.nudConcreteStartOffset.TabIndex = 7;
            this.nudConcreteStartOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudConcreteStartOffset.ThousandsSeparator = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(173, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Офсет в начале";
            // 
            // btnEndPoint
            // 
            this.btnEndPoint.Image = ((System.Drawing.Image)(resources.GetObject("btnEndPoint.Image")));
            this.btnEndPoint.Location = new System.Drawing.Point(36, 173);
            this.btnEndPoint.Name = "btnEndPoint";
            this.btnEndPoint.Size = new System.Drawing.Size(100, 39);
            this.btnEndPoint.TabIndex = 5;
            this.btnEndPoint.UseVisualStyleBackColor = true;
            this.btnEndPoint.Click += new System.EventHandler(this.btnEndPoint_Click);
            // 
            // tbEndCoord
            // 
            this.tbEndCoord.Enabled = false;
            this.tbEndCoord.Location = new System.Drawing.Point(36, 147);
            this.tbEndCoord.Name = "tbEndCoord";
            this.tbEndCoord.Size = new System.Drawing.Size(100, 20);
            this.tbEndCoord.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Координаты конца";
            // 
            // btnStartPoint
            // 
            this.btnStartPoint.Image = ((System.Drawing.Image)(resources.GetObject("btnStartPoint.Image")));
            this.btnStartPoint.Location = new System.Drawing.Point(31, 58);
            this.btnStartPoint.Name = "btnStartPoint";
            this.btnStartPoint.Size = new System.Drawing.Size(100, 39);
            this.btnStartPoint.TabIndex = 2;
            this.btnStartPoint.UseVisualStyleBackColor = true;
            this.btnStartPoint.Click += new System.EventHandler(this.btnStartPoint_Click);
            // 
            // tbStartCoord
            // 
            this.tbStartCoord.Enabled = false;
            this.tbStartCoord.Location = new System.Drawing.Point(31, 32);
            this.tbStartCoord.Name = "tbStartCoord";
            this.tbStartCoord.Size = new System.Drawing.Size(100, 20);
            this.tbStartCoord.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Координаты начала";
            // 
            // tbpReinforcement
            // 
            this.tbpReinforcement.Controls.Add(this.cbOverrideHorSpacing);
            this.tbpReinforcement.Controls.Add(this.btnHorSpacingSetting);
            this.tbpReinforcement.Controls.Add(this.cbOverrideVertSpacing);
            this.tbpReinforcement.Controls.Add(this.btnVertSpacingSetting);
            this.tbpReinforcement.Controls.Add(this.tbHorSpacing);
            this.tbpReinforcement.Controls.Add(this.tbVertSpacing);
            this.tbpReinforcement.Controls.Add(this.label11);
            this.tbpReinforcement.Controls.Add(this.label10);
            this.tbpReinforcement.Controls.Add(this.nudReinforcementEndOffset);
            this.tbpReinforcement.Controls.Add(this.label8);
            this.tbpReinforcement.Controls.Add(this.nudReinforcementStartOffset);
            this.tbpReinforcement.Controls.Add(this.label9);
            this.tbpReinforcement.Location = new System.Drawing.Point(4, 22);
            this.tbpReinforcement.Name = "tbpReinforcement";
            this.tbpReinforcement.Padding = new System.Windows.Forms.Padding(3);
            this.tbpReinforcement.Size = new System.Drawing.Size(511, 244);
            this.tbpReinforcement.TabIndex = 2;
            this.tbpReinforcement.Text = "Армирование";
            this.tbpReinforcement.UseVisualStyleBackColor = true;
            // 
            // tbHorSpacing
            // 
            this.tbHorSpacing.Enabled = false;
            this.tbHorSpacing.Location = new System.Drawing.Point(28, 148);
            this.tbHorSpacing.Name = "tbHorSpacing";
            this.tbHorSpacing.Size = new System.Drawing.Size(172, 20);
            this.tbHorSpacing.TabIndex = 19;
            // 
            // tbVertSpacing
            // 
            this.tbVertSpacing.Enabled = false;
            this.tbVertSpacing.Location = new System.Drawing.Point(28, 33);
            this.tbVertSpacing.Name = "tbVertSpacing";
            this.tbVertSpacing.Size = new System.Drawing.Size(172, 20);
            this.tbVertSpacing.TabIndex = 18;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 132);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(147, 13);
            this.label11.TabIndex = 15;
            this.label11.Text = "Горизонтальная раскладка";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(136, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "Вертикальная раскладка";
            // 
            // nudReinforcementEndOffset
            // 
            this.nudReinforcementEndOffset.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudReinforcementEndOffset.Location = new System.Drawing.Point(282, 149);
            this.nudReinforcementEndOffset.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudReinforcementEndOffset.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudReinforcementEndOffset.Name = "nudReinforcementEndOffset";
            this.nudReinforcementEndOffset.Size = new System.Drawing.Size(100, 20);
            this.nudReinforcementEndOffset.TabIndex = 13;
            this.nudReinforcementEndOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudReinforcementEndOffset.ThousandsSeparator = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(263, 133);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(82, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Офсет в конце";
            // 
            // nudReinforcementStartOffset
            // 
            this.nudReinforcementStartOffset.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudReinforcementStartOffset.Location = new System.Drawing.Point(282, 34);
            this.nudReinforcementStartOffset.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudReinforcementStartOffset.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudReinforcementStartOffset.Name = "nudReinforcementStartOffset";
            this.nudReinforcementStartOffset.Size = new System.Drawing.Size(100, 20);
            this.nudReinforcementStartOffset.TabIndex = 11;
            this.nudReinforcementStartOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudReinforcementStartOffset.ThousandsSeparator = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(263, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Офсет в начале";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(452, 291);
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
            this.btnOK.Location = new System.Drawing.Point(353, 291);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnVertSpacingSetting
            // 
            this.btnVertSpacingSetting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVertSpacingSetting.Enabled = false;
            this.btnVertSpacingSetting.Image = ((System.Drawing.Image)(resources.GetObject("btnVertSpacingSetting.Image")));
            this.btnVertSpacingSetting.Location = new System.Drawing.Point(64, 82);
            this.btnVertSpacingSetting.Name = "btnVertSpacingSetting";
            this.btnVertSpacingSetting.Size = new System.Drawing.Size(136, 42);
            this.btnVertSpacingSetting.TabIndex = 20;
            this.btnVertSpacingSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnVertSpacingSetting.UseVisualStyleBackColor = true;
            this.btnVertSpacingSetting.Click += new System.EventHandler(this.btnVertSpacingSetting_Click);
            // 
            // cbOverrideVertSpacing
            // 
            this.cbOverrideVertSpacing.AutoSize = true;
            this.cbOverrideVertSpacing.Location = new System.Drawing.Point(64, 59);
            this.cbOverrideVertSpacing.Name = "cbOverrideVertSpacing";
            this.cbOverrideVertSpacing.Size = new System.Drawing.Size(111, 17);
            this.cbOverrideVertSpacing.TabIndex = 21;
            this.cbOverrideVertSpacing.Text = "Переопределить";
            this.cbOverrideVertSpacing.UseVisualStyleBackColor = true;
            this.cbOverrideVertSpacing.CheckedChanged += new System.EventHandler(this.cbOverrideVertSpacing_CheckedChanged);
            // 
            // cbOverrideHorSpacing
            // 
            this.cbOverrideHorSpacing.AutoSize = true;
            this.cbOverrideHorSpacing.Location = new System.Drawing.Point(64, 173);
            this.cbOverrideHorSpacing.Name = "cbOverrideHorSpacing";
            this.cbOverrideHorSpacing.Size = new System.Drawing.Size(111, 17);
            this.cbOverrideHorSpacing.TabIndex = 23;
            this.cbOverrideHorSpacing.Text = "Переопределить";
            this.cbOverrideHorSpacing.UseVisualStyleBackColor = true;
            this.cbOverrideHorSpacing.CheckedChanged += new System.EventHandler(this.cbOverrideHorSpacing_CheckedChanged);
            // 
            // btnHorSpacingSetting
            // 
            this.btnHorSpacingSetting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHorSpacingSetting.Enabled = false;
            this.btnHorSpacingSetting.Image = ((System.Drawing.Image)(resources.GetObject("btnHorSpacingSetting.Image")));
            this.btnHorSpacingSetting.Location = new System.Drawing.Point(64, 196);
            this.btnHorSpacingSetting.Name = "btnHorSpacingSetting";
            this.btnHorSpacingSetting.Size = new System.Drawing.Size(136, 42);
            this.btnHorSpacingSetting.TabIndex = 22;
            this.btnHorSpacingSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHorSpacingSetting.UseVisualStyleBackColor = true;
            this.btnHorSpacingSetting.Click += new System.EventHandler(this.btnHorSpacingSetting_Click);
            // 
            // frmWall
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 326);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tbMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmWall";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Стена";
            this.tbMain.ResumeLayout(false);
            this.tbpMain.ResumeLayout(false);
            this.tbpMain.PerformLayout();
            this.tbpFormWork.ResumeLayout(false);
            this.tbpFormWork.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudConcreteEndOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudConcreteStartOffset)).EndInit();
            this.tbpReinforcement.ResumeLayout(false);
            this.tbpReinforcement.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudReinforcementEndOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudReinforcementStartOffset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbMain;
        private System.Windows.Forms.TabPage tbpMain;
        private System.Windows.Forms.TabPage tbpFormWork;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TabPage tbpReinforcement;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbWallTypes;
        private System.Windows.Forms.ComboBox cbLevels;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudConcreteEndOffset;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudConcreteStartOffset;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnEndPoint;
        private System.Windows.Forms.TextBox tbEndCoord;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnStartPoint;
        private System.Windows.Forms.TextBox tbStartCoord;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private System.Windows.Forms.CheckBox cbRewriteHeight;
        private System.Windows.Forms.TextBox tbHorSpacing;
        private System.Windows.Forms.TextBox tbVertSpacing;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nudReinforcementEndOffset;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudReinforcementStartOffset;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox cbOverrideHorSpacing;
        private System.Windows.Forms.Button btnHorSpacingSetting;
        private System.Windows.Forms.CheckBox cbOverrideVertSpacing;
        private System.Windows.Forms.Button btnVertSpacingSetting;
    }
}