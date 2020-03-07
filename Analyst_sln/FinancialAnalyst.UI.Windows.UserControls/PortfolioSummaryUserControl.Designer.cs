namespace FinancialAnalyst.UI.Windows.UserControls
{
    partial class PortfolioSummaryUserControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.dataGridViewAssets = new System.Windows.Forms.DataGridView();
            this.dataGridViewAssets_TickerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewAssets_ProportionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewAssets_AmountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewAssets_ShowDetailColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dataGridViewAssets_ExchangeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.labelTotalPercentage = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelBetaPortfolio = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAssets)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(48, 4);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(45, 13);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "<name>";
            // 
            // dataGridViewAssets
            // 
            this.dataGridViewAssets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAssets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewAssets_TickerColumn,
            this.dataGridViewAssets_ProportionColumn,
            this.dataGridViewAssets_AmountColumn,
            this.dataGridViewAssets_ShowDetailColumn,
            this.dataGridViewAssets_ExchangeColumn});
            this.dataGridViewAssets.Location = new System.Drawing.Point(7, 29);
            this.dataGridViewAssets.Name = "dataGridViewAssets";
            this.dataGridViewAssets.ShowEditingIcon = false;
            this.dataGridViewAssets.Size = new System.Drawing.Size(248, 250);
            this.dataGridViewAssets.TabIndex = 2;
            this.dataGridViewAssets.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewAssets_CellContentClick);
            this.dataGridViewAssets.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewAssets_CellValueChanged);
            // 
            // dataGridViewAssets_TickerColumn
            // 
            this.dataGridViewAssets_TickerColumn.DataPropertyName = "Ticker";
            this.dataGridViewAssets_TickerColumn.FillWeight = 30F;
            this.dataGridViewAssets_TickerColumn.HeaderText = "Ticker";
            this.dataGridViewAssets_TickerColumn.MaxInputLength = 10;
            this.dataGridViewAssets_TickerColumn.Name = "dataGridViewAssets_TickerColumn";
            this.dataGridViewAssets_TickerColumn.ReadOnly = true;
            this.dataGridViewAssets_TickerColumn.Width = 50;
            // 
            // dataGridViewAssets_ProportionColumn
            // 
            this.dataGridViewAssets_ProportionColumn.DataPropertyName = "Percentage";
            this.dataGridViewAssets_ProportionColumn.FillWeight = 20F;
            this.dataGridViewAssets_ProportionColumn.HeaderText = "%";
            this.dataGridViewAssets_ProportionColumn.MaxInputLength = 5;
            this.dataGridViewAssets_ProportionColumn.Name = "dataGridViewAssets_ProportionColumn";
            this.dataGridViewAssets_ProportionColumn.Width = 50;
            // 
            // dataGridViewAssets_AmountColumn
            // 
            this.dataGridViewAssets_AmountColumn.DataPropertyName = "Amount";
            this.dataGridViewAssets_AmountColumn.FillWeight = 25F;
            this.dataGridViewAssets_AmountColumn.HeaderText = "Amount";
            this.dataGridViewAssets_AmountColumn.Name = "dataGridViewAssets_AmountColumn";
            this.dataGridViewAssets_AmountColumn.Width = 50;
            // 
            // dataGridViewAssets_ShowDetailColumn
            // 
            this.dataGridViewAssets_ShowDetailColumn.FillWeight = 25F;
            this.dataGridViewAssets_ShowDetailColumn.HeaderText = "Detail";
            this.dataGridViewAssets_ShowDetailColumn.Name = "dataGridViewAssets_ShowDetailColumn";
            this.dataGridViewAssets_ShowDetailColumn.ReadOnly = true;
            this.dataGridViewAssets_ShowDetailColumn.Text = "Detail";
            this.dataGridViewAssets_ShowDetailColumn.UseColumnTextForButtonValue = true;
            this.dataGridViewAssets_ShowDetailColumn.Width = 50;
            // 
            // dataGridViewAssets_ExchangeColumn
            // 
            this.dataGridViewAssets_ExchangeColumn.DataPropertyName = "Exchange";
            this.dataGridViewAssets_ExchangeColumn.HeaderText = "Exchange";
            this.dataGridViewAssets_ExchangeColumn.Name = "dataGridViewAssets_ExchangeColumn";
            this.dataGridViewAssets_ExchangeColumn.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 282);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Total:";
            // 
            // labelTotalPercentage
            // 
            this.labelTotalPercentage.AutoSize = true;
            this.labelTotalPercentage.Location = new System.Drawing.Point(46, 282);
            this.labelTotalPercentage.Name = "labelTotalPercentage";
            this.labelTotalPercentage.Size = new System.Drawing.Size(96, 13);
            this.labelTotalPercentage.TabIndex = 4;
            this.labelTotalPercentage.Text = "<total percentage>";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 299);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Beta:";
            // 
            // labelBetaPortfolio
            // 
            this.labelBetaPortfolio.AutoSize = true;
            this.labelBetaPortfolio.Location = new System.Drawing.Point(48, 299);
            this.labelBetaPortfolio.Name = "labelBetaPortfolio";
            this.labelBetaPortfolio.Size = new System.Drawing.Size(80, 13);
            this.labelBetaPortfolio.TabIndex = 6;
            this.labelBetaPortfolio.Text = "<beta portfolio>";
            // 
            // PortfolioSummaryUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelBetaPortfolio);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelTotalPercentage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridViewAssets);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.label1);
            this.Name = "PortfolioSummaryUserControl";
            this.Size = new System.Drawing.Size(265, 322);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAssets)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.DataGridView dataGridViewAssets;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelTotalPercentage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelBetaPortfolio;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewAssets_TickerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewAssets_ProportionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewAssets_AmountColumn;
        private System.Windows.Forms.DataGridViewButtonColumn dataGridViewAssets_ShowDetailColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewAssets_ExchangeColumn;
    }
}
