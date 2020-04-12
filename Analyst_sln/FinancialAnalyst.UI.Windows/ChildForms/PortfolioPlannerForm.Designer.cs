namespace FinancialAnalyst.UI.Windows.ChildForms
{
    partial class PortfolioPlannerForm
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
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.dataGridViewPortfolios = new System.Windows.Forms.DataGridView();
            this.dataGridViewPortfolios_NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewPortfolios_InitialBalanceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewPortfolios_TotalCostsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewPortfolios_MarketValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewPortfolios_TotalCashColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonLoadTransactions = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.portfolioSummaryUserControl1 = new FinancialAnalyst.UI.Windows.UserControls.PortfolioSummaryUserControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPortfolios)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.dataGridViewPortfolios);
            this.splitContainerMain.Panel1.Controls.Add(this.buttonLoadTransactions);
            this.splitContainerMain.Panel1.Controls.Add(this.buttonRefresh);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.portfolioSummaryUserControl1);
            this.splitContainerMain.Size = new System.Drawing.Size(1536, 750);
            this.splitContainerMain.SplitterDistance = 173;
            this.splitContainerMain.TabIndex = 1;
            // 
            // dataGridViewPortfolios
            // 
            this.dataGridViewPortfolios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPortfolios.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewPortfolios_NameColumn,
            this.dataGridViewPortfolios_InitialBalanceColumn,
            this.dataGridViewPortfolios_TotalCostsColumn,
            this.dataGridViewPortfolios_MarketValueColumn,
            this.dataGridViewPortfolios_TotalCashColumn});
            this.dataGridViewPortfolios.Location = new System.Drawing.Point(3, 32);
            this.dataGridViewPortfolios.Name = "dataGridViewPortfolios";
            this.dataGridViewPortfolios.Size = new System.Drawing.Size(558, 120);
            this.dataGridViewPortfolios.TabIndex = 2;
            this.dataGridViewPortfolios.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dataGridViewPortfolios_RowStateChanged);
            // 
            // dataGridViewPortfolios_NameColumn
            // 
            this.dataGridViewPortfolios_NameColumn.DataPropertyName = "Name";
            this.dataGridViewPortfolios_NameColumn.FillWeight = 40F;
            this.dataGridViewPortfolios_NameColumn.HeaderText = "Name";
            this.dataGridViewPortfolios_NameColumn.Name = "dataGridViewPortfolios_NameColumn";
            this.dataGridViewPortfolios_NameColumn.ReadOnly = true;
            // 
            // dataGridViewPortfolios_InitialBalanceColumn
            // 
            this.dataGridViewPortfolios_InitialBalanceColumn.DataPropertyName = "InitialBalance";
            this.dataGridViewPortfolios_InitialBalanceColumn.FillWeight = 15F;
            this.dataGridViewPortfolios_InitialBalanceColumn.HeaderText = "Initial balance";
            this.dataGridViewPortfolios_InitialBalanceColumn.Name = "dataGridViewPortfolios_InitialBalanceColumn";
            this.dataGridViewPortfolios_InitialBalanceColumn.ReadOnly = true;
            // 
            // dataGridViewPortfolios_TotalCostsColumn
            // 
            this.dataGridViewPortfolios_TotalCostsColumn.DataPropertyName = "TotalCosts";
            this.dataGridViewPortfolios_TotalCostsColumn.FillWeight = 15F;
            this.dataGridViewPortfolios_TotalCostsColumn.HeaderText = "Costs";
            this.dataGridViewPortfolios_TotalCostsColumn.Name = "dataGridViewPortfolios_TotalCostsColumn";
            // 
            // dataGridViewPortfolios_MarketValueColumn
            // 
            this.dataGridViewPortfolios_MarketValueColumn.DataPropertyName = "MarketValue";
            this.dataGridViewPortfolios_MarketValueColumn.FillWeight = 15F;
            this.dataGridViewPortfolios_MarketValueColumn.HeaderText = "Mkt Value";
            this.dataGridViewPortfolios_MarketValueColumn.Name = "dataGridViewPortfolios_MarketValueColumn";
            // 
            // dataGridViewPortfolios_TotalCashColumn
            // 
            this.dataGridViewPortfolios_TotalCashColumn.DataPropertyName = "Cash";
            this.dataGridViewPortfolios_TotalCashColumn.FillWeight = 15F;
            this.dataGridViewPortfolios_TotalCashColumn.HeaderText = "Cash";
            this.dataGridViewPortfolios_TotalCashColumn.Name = "dataGridViewPortfolios_TotalCashColumn";
            this.dataGridViewPortfolios_TotalCashColumn.ReadOnly = true;
            // 
            // buttonLoadTransactions
            // 
            this.buttonLoadTransactions.Location = new System.Drawing.Point(84, 3);
            this.buttonLoadTransactions.Name = "buttonLoadTransactions";
            this.buttonLoadTransactions.Size = new System.Drawing.Size(159, 23);
            this.buttonLoadTransactions.TabIndex = 0;
            this.buttonLoadTransactions.Text = "Load transactions";
            this.buttonLoadTransactions.UseVisualStyleBackColor = true;
            this.buttonLoadTransactions.Click += new System.EventHandler(this.buttonLoadTransactions_Click);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(3, 3);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 1;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // portfolioSummaryUserControl1
            // 
            this.portfolioSummaryUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.portfolioSummaryUserControl1.Location = new System.Drawing.Point(0, 0);
            this.portfolioSummaryUserControl1.Name = "portfolioSummaryUserControl1";
            this.portfolioSummaryUserControl1.Size = new System.Drawing.Size(1536, 573);
            this.portfolioSummaryUserControl1.TabIndex = 1;
            // 
            // PortfolioPlannerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1536, 750);
            this.Controls.Add(this.splitContainerMain);
            this.Name = "PortfolioPlannerForm";
            this.Text = "PortfolioPlanner";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PortfolioPlanner_FormClosed);
            this.Load += new System.EventHandler(this.PortfolioPlanner_Load);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPortfolios)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Button buttonLoadTransactions;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.DataGridView dataGridViewPortfolios;
        private UserControls.PortfolioSummaryUserControl portfolioSummaryUserControl1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewPortfolios_NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewPortfolios_InitialBalanceColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewPortfolios_TotalCostsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewPortfolios_MarketValueColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewPortfolios_TotalCashColumn;
    }
}