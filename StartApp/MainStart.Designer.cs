namespace StartApp
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnWalls = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbSaveToFile = new System.Windows.Forms.ToolStripButton();
            this.tsbLoadFromFile = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnWalls
            // 
            this.btnWalls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWalls.Location = new System.Drawing.Point(0, 28);
            this.btnWalls.Name = "btnWalls";
            this.btnWalls.Size = new System.Drawing.Size(776, 34);
            this.btnWalls.TabIndex = 0;
            this.btnWalls.Text = "Рассчитать стены";
            this.btnWalls.UseVisualStyleBackColor = true;
            this.btnWalls.Click += new System.EventHandler(this.button1_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbLoadFromFile,
            this.tsbSaveToFile});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbSaveToFile
            // 
            this.tsbSaveToFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSaveToFile.Image = ((System.Drawing.Image)(resources.GetObject("tsbSaveToFile.Image")));
            this.tsbSaveToFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSaveToFile.Name = "tsbSaveToFile";
            this.tsbSaveToFile.Size = new System.Drawing.Size(23, 22);
            this.tsbSaveToFile.Text = "Сохранить";
            this.tsbSaveToFile.Click += new System.EventHandler(this.tsbSaveToFile_Click);
            // 
            // tsbLoadFromFile
            // 
            this.tsbLoadFromFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbLoadFromFile.Image = ((System.Drawing.Image)(resources.GetObject("tsbLoadFromFile.Image")));
            this.tsbLoadFromFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLoadFromFile.Name = "tsbLoadFromFile";
            this.tsbLoadFromFile.Size = new System.Drawing.Size(23, 22);
            this.tsbLoadFromFile.Text = "Открыть";
            this.tsbLoadFromFile.Click += new System.EventHandler(this.tsbLoadFromFile_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnWalls);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Калькулятор";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnWalls;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbSaveToFile;
        private System.Windows.Forms.ToolStripButton tsbLoadFromFile;
    }
}

