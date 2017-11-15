namespace Analyst.Windows
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnMockFiles = new System.Windows.Forms.Button();
            this.btnLoadDataset = new System.Windows.Forms.Button();
            this.txtDatasetId = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnMockFiles
            // 
            this.btnMockFiles.Enabled = false;
            this.btnMockFiles.Location = new System.Drawing.Point(80, 37);
            this.btnMockFiles.Name = "btnMockFiles";
            this.btnMockFiles.Size = new System.Drawing.Size(167, 47);
            this.btnMockFiles.TabIndex = 0;
            this.btnMockFiles.Text = "Create mock files";
            this.btnMockFiles.UseVisualStyleBackColor = true;
            this.btnMockFiles.Click += new System.EventHandler(this.btnMockFiles_Click);
            // 
            // btnLoadDataset
            // 
            this.btnLoadDataset.Location = new System.Drawing.Point(80, 164);
            this.btnLoadDataset.Name = "btnLoadDataset";
            this.btnLoadDataset.Size = new System.Drawing.Size(167, 54);
            this.btnLoadDataset.TabIndex = 1;
            this.btnLoadDataset.Text = "Load Dataset";
            this.btnLoadDataset.UseVisualStyleBackColor = true;
            this.btnLoadDataset.Click += new System.EventHandler(this.btnLoadDataset_Click);
            // 
            // txtDatasetId
            // 
            this.txtDatasetId.Location = new System.Drawing.Point(80, 112);
            this.txtDatasetId.Name = "txtDatasetId";
            this.txtDatasetId.Size = new System.Drawing.Size(100, 26);
            this.txtDatasetId.TabIndex = 2;
            this.txtDatasetId.Text = "201604";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1039, 544);
            this.Controls.Add(this.txtDatasetId);
            this.Controls.Add(this.btnLoadDataset);
            this.Controls.Add(this.btnMockFiles);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMockFiles;
        private System.Windows.Forms.Button btnLoadDataset;
        private System.Windows.Forms.TextBox txtDatasetId;
    }
}

