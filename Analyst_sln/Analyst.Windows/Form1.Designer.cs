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
            this.btnFilterSubmissions = new System.Windows.Forms.Button();
            this.btnLoadDataset = new System.Windows.Forms.Button();
            this.dgvDatasets = new System.Windows.Forms.DataGridView();
            this.dgvDatasetInProcess = new System.Windows.Forms.DataGridView();
            this.lblTimer = new System.Windows.Forms.Label();
            this.lblDatasetInProcess = new System.Windows.Forms.Label();
            this.cboTables = new System.Windows.Forms.ComboBox();
            this.lblTable = new System.Windows.Forms.Label();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.lblKeyToFilter = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatasets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatasetInProcess)).BeginInit();
            this.SuspendLayout();
            // 
            // btnFilterSubmissions
            // 
            this.btnFilterSubmissions.Location = new System.Drawing.Point(1004, 66);
            this.btnFilterSubmissions.Name = "btnFilterSubmissions";
            this.btnFilterSubmissions.Size = new System.Drawing.Size(278, 38);
            this.btnFilterSubmissions.TabIndex = 0;
            this.btnFilterSubmissions.Text = "Filter submissions";
            this.btnFilterSubmissions.UseVisualStyleBackColor = true;
            this.btnFilterSubmissions.Click += new System.EventHandler(this.btnFilterSubmissions_Click);
            // 
            // btnLoadDataset
            // 
            this.btnLoadDataset.Location = new System.Drawing.Point(13, 16);
            this.btnLoadDataset.Name = "btnLoadDataset";
            this.btnLoadDataset.Size = new System.Drawing.Size(192, 30);
            this.btnLoadDataset.TabIndex = 1;
            this.btnLoadDataset.Text = "Load Dataset";
            this.btnLoadDataset.UseVisualStyleBackColor = true;
            this.btnLoadDataset.Click += new System.EventHandler(this.btnLoadDataset_Click);
            // 
            // dgvDatasets
            // 
            this.dgvDatasets.AllowUserToAddRows = false;
            this.dgvDatasets.AllowUserToDeleteRows = false;
            this.dgvDatasets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDatasets.Location = new System.Drawing.Point(13, 187);
            this.dgvDatasets.Name = "dgvDatasets";
            this.dgvDatasets.RowTemplate.Height = 28;
            this.dgvDatasets.Size = new System.Drawing.Size(1717, 179);
            this.dgvDatasets.TabIndex = 3;
            // 
            // dgvDatasetInProcess
            // 
            this.dgvDatasetInProcess.AllowUserToAddRows = false;
            this.dgvDatasetInProcess.AllowUserToDeleteRows = false;
            this.dgvDatasetInProcess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDatasetInProcess.Location = new System.Drawing.Point(13, 421);
            this.dgvDatasetInProcess.Name = "dgvDatasetInProcess";
            this.dgvDatasetInProcess.Size = new System.Drawing.Size(1717, 84);
            this.dgvDatasetInProcess.TabIndex = 5;
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Location = new System.Drawing.Point(12, 103);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(63, 20);
            this.lblTimer.TabIndex = 6;
            this.lblTimer.Text = "lblTimer";
            // 
            // lblDatasetInProcess
            // 
            this.lblDatasetInProcess.AutoSize = true;
            this.lblDatasetInProcess.Location = new System.Drawing.Point(12, 66);
            this.lblDatasetInProcess.Name = "lblDatasetInProcess";
            this.lblDatasetInProcess.Size = new System.Drawing.Size(152, 20);
            this.lblDatasetInProcess.TabIndex = 7;
            this.lblDatasetInProcess.Text = "lblDatasetInProcess";
            // 
            // cboTables
            // 
            this.cboTables.FormattingEnabled = true;
            this.cboTables.Location = new System.Drawing.Point(576, 12);
            this.cboTables.Name = "cboTables";
            this.cboTables.Size = new System.Drawing.Size(121, 28);
            this.cboTables.TabIndex = 9;
            // 
            // lblTable
            // 
            this.lblTable.AutoSize = true;
            this.lblTable.Location = new System.Drawing.Point(507, 15);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(48, 20);
            this.lblTable.TabIndex = 10;
            this.lblTable.Text = "Table";
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(1117, 25);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(165, 26);
            this.txtKey.TabIndex = 11;
            this.txtKey.Text = "0001564590-17-001812";
            // 
            // lblKeyToFilter
            // 
            this.lblKeyToFilter.AutoSize = true;
            this.lblKeyToFilter.Location = new System.Drawing.Point(1004, 25);
            this.lblKeyToFilter.Name = "lblKeyToFilter";
            this.lblKeyToFilter.Size = new System.Drawing.Size(87, 20);
            this.lblKeyToFilter.TabIndex = 12;
            this.lblKeyToFilter.Text = "Key to filter";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1749, 544);
            this.Controls.Add(this.lblKeyToFilter);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.lblTable);
            this.Controls.Add(this.cboTables);
            this.Controls.Add(this.lblDatasetInProcess);
            this.Controls.Add(this.lblTimer);
            this.Controls.Add(this.dgvDatasetInProcess);
            this.Controls.Add(this.dgvDatasets);
            this.Controls.Add(this.btnLoadDataset);
            this.Controls.Add(this.btnFilterSubmissions);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatasets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatasetInProcess)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFilterSubmissions;
        private System.Windows.Forms.Button btnLoadDataset;
        private System.Windows.Forms.DataGridView dgvDatasets;
        private System.Windows.Forms.DataGridView dgvDatasetInProcess;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.Label lblDatasetInProcess;
        private System.Windows.Forms.ComboBox cboTables;
        private System.Windows.Forms.Label lblTable;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Label lblKeyToFilter;
    }
}

