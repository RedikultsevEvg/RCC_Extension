namespace RDUIL.WinForms
{
    partial class frmDetailList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDetailList));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.tsbEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbClone = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.tsbWalls = new System.Windows.Forms.ToolStripButton();
            this.tsbWallTypes = new System.Windows.Forms.ToolStripButton();
            this.tsbOpeningTypes = new System.Windows.Forms.ToolStripButton();
            this.tsbOpenings = new System.Windows.Forms.ToolStripButton();
            this.tsbReport = new System.Windows.Forms.ToolStripButton();
            this.tsbSteelColumnBase = new System.Windows.Forms.ToolStripButton();
            this.tsbColumnForces = new System.Windows.Forms.ToolStripButton();
            this.tsbSteelColumnBaseParts = new System.Windows.Forms.ToolStripButton();
            this.lvDetails = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNew,
            this.tsbEdit,
            this.tsbClone,
            this.tsbDelete,
            this.tsbWalls,
            this.tsbWallTypes,
            this.tsbOpeningTypes,
            this.tsbOpenings,
            this.tsbReport,
            this.tsbSteelColumnBase,
            this.tsbColumnForces,
            this.tsbSteelColumnBaseParts});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(834, 37);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbNew
            // 
            this.tsbNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNew.Image = ((System.Drawing.Image)(resources.GetObject("tsbNew.Image")));
            this.tsbNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNew.Name = "tsbNew";
            this.tsbNew.Size = new System.Drawing.Size(34, 34);
            this.tsbNew.Text = "Добавить";
            this.tsbNew.Click += new System.EventHandler(this.tsbNew_Click);
            // 
            // tsbEdit
            // 
            this.tsbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbEdit.Image = ((System.Drawing.Image)(resources.GetObject("tsbEdit.Image")));
            this.tsbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEdit.Name = "tsbEdit";
            this.tsbEdit.Size = new System.Drawing.Size(34, 34);
            this.tsbEdit.Text = "Изменить";
            this.tsbEdit.Click += new System.EventHandler(this.tsbEdit_Click);
            // 
            // tsbClone
            // 
            this.tsbClone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClone.Image = ((System.Drawing.Image)(resources.GetObject("tsbClone.Image")));
            this.tsbClone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClone.Name = "tsbClone";
            this.tsbClone.Size = new System.Drawing.Size(34, 34);
            this.tsbClone.Text = "Клонировать";
            this.tsbClone.Visible = false;
            // 
            // tsbDelete
            // 
            this.tsbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsbDelete.Image")));
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(34, 34);
            this.tsbDelete.Text = "Удалить";
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // tsbWalls
            // 
            this.tsbWalls.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbWalls.Image = ((System.Drawing.Image)(resources.GetObject("tsbWalls.Image")));
            this.tsbWalls.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbWalls.Name = "tsbWalls";
            this.tsbWalls.Size = new System.Drawing.Size(34, 34);
            this.tsbWalls.Text = "Стены";
            this.tsbWalls.Click += new System.EventHandler(this.tsbWalls_Click);
            // 
            // tsbWallTypes
            // 
            this.tsbWallTypes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbWallTypes.Image = ((System.Drawing.Image)(resources.GetObject("tsbWallTypes.Image")));
            this.tsbWallTypes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbWallTypes.Name = "tsbWallTypes";
            this.tsbWallTypes.Size = new System.Drawing.Size(34, 34);
            this.tsbWallTypes.Text = "Тип стены";
            this.tsbWallTypes.Click += new System.EventHandler(this.tsbWallType_Click);
            // 
            // tsbOpeningTypes
            // 
            this.tsbOpeningTypes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpeningTypes.Image = ((System.Drawing.Image)(resources.GetObject("tsbOpeningTypes.Image")));
            this.tsbOpeningTypes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpeningTypes.Name = "tsbOpeningTypes";
            this.tsbOpeningTypes.Size = new System.Drawing.Size(34, 34);
            this.tsbOpeningTypes.Text = "Типы отверстий";
            this.tsbOpeningTypes.Click += new System.EventHandler(this.tsbOpeningTypes_Click);
            // 
            // tsbOpenings
            // 
            this.tsbOpenings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpenings.Image = ((System.Drawing.Image)(resources.GetObject("tsbOpenings.Image")));
            this.tsbOpenings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpenings.Name = "tsbOpenings";
            this.tsbOpenings.Size = new System.Drawing.Size(34, 34);
            this.tsbOpenings.Text = "Отверстия";
            this.tsbOpenings.Click += new System.EventHandler(this.tsbOpenings_Click);
            // 
            // tsbReport
            // 
            this.tsbReport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbReport.Image = ((System.Drawing.Image)(resources.GetObject("tsbReport.Image")));
            this.tsbReport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReport.Name = "tsbReport";
            this.tsbReport.Size = new System.Drawing.Size(34, 34);
            this.tsbReport.Text = "Сумма по выбранным";
            this.tsbReport.Click += new System.EventHandler(this.tsbReport_Click);
            // 
            // tsbSteelColumnBase
            // 
            this.tsbSteelColumnBase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSteelColumnBase.Image = ((System.Drawing.Image)(resources.GetObject("tsbSteelColumnBase.Image")));
            this.tsbSteelColumnBase.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSteelColumnBase.Name = "tsbSteelColumnBase";
            this.tsbSteelColumnBase.Size = new System.Drawing.Size(34, 34);
            this.tsbSteelColumnBase.Text = "Базы стальных колонн";
            this.tsbSteelColumnBase.Click += new System.EventHandler(this.tsbSteelColumnBase_Click);
            // 
            // tsbColumnForces
            // 
            this.tsbColumnForces.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbColumnForces.Image = ((System.Drawing.Image)(resources.GetObject("tsbColumnForces.Image")));
            this.tsbColumnForces.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbColumnForces.Name = "tsbColumnForces";
            this.tsbColumnForces.Size = new System.Drawing.Size(34, 34);
            this.tsbColumnForces.Text = "Нагружения";
            this.tsbColumnForces.Click += new System.EventHandler(this.tsbColumnForces_Click);
            // 
            // tsbSteelColumnBaseParts
            // 
            this.tsbSteelColumnBaseParts.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSteelColumnBaseParts.Image = ((System.Drawing.Image)(resources.GetObject("tsbSteelColumnBaseParts.Image")));
            this.tsbSteelColumnBaseParts.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSteelColumnBaseParts.Name = "tsbSteelColumnBaseParts";
            this.tsbSteelColumnBaseParts.Size = new System.Drawing.Size(34, 34);
            this.tsbSteelColumnBaseParts.Text = "Участки";
            this.tsbSteelColumnBaseParts.Click += new System.EventHandler(this.tsbSteelBaseParts_Click);
            // 
            // lvDetails
            // 
            this.lvDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDetails.Location = new System.Drawing.Point(12, 42);
            this.lvDetails.Name = "lvDetails";
            this.lvDetails.Size = new System.Drawing.Size(810, 382);
            this.lvDetails.TabIndex = 1;
            this.lvDetails.UseCompatibleStateImageBehavior = false;
            this.lvDetails.View = System.Windows.Forms.View.Details;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(747, 430);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmDetailList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 462);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvDetails);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(850, 500);
            this.Name = "frmDetailList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Стены";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNew;
        private System.Windows.Forms.ToolStripButton tsbEdit;
        private System.Windows.Forms.ToolStripButton tsbClone;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.ListView lvDetails;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolStripButton tsbWalls;
        private System.Windows.Forms.ToolStripButton tsbWallTypes;
        private System.Windows.Forms.ToolStripButton tsbOpeningTypes;
        private System.Windows.Forms.ToolStripButton tsbOpenings;
        private System.Windows.Forms.ToolStripButton tsbReport;
        private System.Windows.Forms.ToolStripButton tsbSteelColumnBase;
        private System.Windows.Forms.ToolStripButton tsbColumnForces;
        private System.Windows.Forms.ToolStripButton tsbSteelColumnBaseParts;
    }
}