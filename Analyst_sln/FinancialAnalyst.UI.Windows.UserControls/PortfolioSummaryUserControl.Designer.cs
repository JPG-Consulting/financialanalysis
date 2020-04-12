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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.label1 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.dataGridViewAssets = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.labelTotalPercentage = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelBetaPortfolio = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dataGridViewAssets_TickerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewAssets_ProportionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewAssets_AmountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewAssets_ShowDetailColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dataGridViewAssets_ExchangeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAssets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(53, 9);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(45, 13);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "<name>";
            // 
            // dataGridViewAssets
            // 
            this.dataGridViewAssets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewAssets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAssets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewAssets_TickerColumn,
            this.dataGridViewAssets_ProportionColumn,
            this.dataGridViewAssets_AmountColumn,
            this.dataGridViewAssets_ShowDetailColumn,
            this.dataGridViewAssets_ExchangeColumn});
            this.dataGridViewAssets.Location = new System.Drawing.Point(12, 25);
            this.dataGridViewAssets.Name = "dataGridViewAssets";
            this.dataGridViewAssets.ShowEditingIcon = false;
            this.dataGridViewAssets.Size = new System.Drawing.Size(330, 700);
            this.dataGridViewAssets.TabIndex = 2;
            this.dataGridViewAssets.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewAssets_CellContentClick);
            this.dataGridViewAssets.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewAssets_CellValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 744);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Total:";
            // 
            // labelTotalPercentage
            // 
            this.labelTotalPercentage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelTotalPercentage.AutoSize = true;
            this.labelTotalPercentage.Location = new System.Drawing.Point(53, 744);
            this.labelTotalPercentage.Name = "labelTotalPercentage";
            this.labelTotalPercentage.Size = new System.Drawing.Size(96, 13);
            this.labelTotalPercentage.TabIndex = 4;
            this.labelTotalPercentage.Text = "<total percentage>";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 769);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Beta:";
            // 
            // labelBetaPortfolio
            // 
            this.labelBetaPortfolio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelBetaPortfolio.AutoSize = true;
            this.labelBetaPortfolio.Location = new System.Drawing.Point(53, 769);
            this.labelBetaPortfolio.Name = "labelBetaPortfolio";
            this.labelBetaPortfolio.Size = new System.Drawing.Size(80, 13);
            this.labelBetaPortfolio.TabIndex = 6;
            this.labelBetaPortfolio.Text = "<beta portfolio>";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.labelBetaPortfolio);
            this.splitContainer1.Panel1.Controls.Add(this.labelName);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.dataGridViewAssets);
            this.splitContainer1.Panel1.Controls.Add(this.labelTotalPercentage);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.chart1);
            this.splitContainer1.Size = new System.Drawing.Size(1237, 787);
            this.splitContainer1.SplitterDistance = 345;
            this.splitContainer1.TabIndex = 7;
            // 
            // chart1
            // 
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(73, 128);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(300, 300);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // dataGridViewAssets_TickerColumn
            // 
            this.dataGridViewAssets_TickerColumn.DataPropertyName = "Ticker";
            this.dataGridViewAssets_TickerColumn.FillWeight = 30F;
            this.dataGridViewAssets_TickerColumn.HeaderText = "Ticker";
            this.dataGridViewAssets_TickerColumn.MaxInputLength = 10;
            this.dataGridViewAssets_TickerColumn.Name = "dataGridViewAssets_TickerColumn";
            this.dataGridViewAssets_TickerColumn.ReadOnly = true;
            this.dataGridViewAssets_TickerColumn.Width = 120;
            // 
            // dataGridViewAssets_ProportionColumn
            // 
            this.dataGridViewAssets_ProportionColumn.DataPropertyName = "Percentage";
            this.dataGridViewAssets_ProportionColumn.FillWeight = 10F;
            this.dataGridViewAssets_ProportionColumn.HeaderText = "%";
            this.dataGridViewAssets_ProportionColumn.MaxInputLength = 5;
            this.dataGridViewAssets_ProportionColumn.Name = "dataGridViewAssets_ProportionColumn";
            this.dataGridViewAssets_ProportionColumn.Width = 50;
            // 
            // dataGridViewAssets_AmountColumn
            // 
            this.dataGridViewAssets_AmountColumn.DataPropertyName = "Amount";
            this.dataGridViewAssets_AmountColumn.FillWeight = 20F;
            this.dataGridViewAssets_AmountColumn.HeaderText = "Amount";
            this.dataGridViewAssets_AmountColumn.Name = "dataGridViewAssets_AmountColumn";
            this.dataGridViewAssets_AmountColumn.Width = 60;
            // 
            // dataGridViewAssets_ShowDetailColumn
            // 
            this.dataGridViewAssets_ShowDetailColumn.FillWeight = 20F;
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
            this.dataGridViewAssets_ExchangeColumn.FillWeight = 20F;
            this.dataGridViewAssets_ExchangeColumn.HeaderText = "Exchange";
            this.dataGridViewAssets_ExchangeColumn.Name = "dataGridViewAssets_ExchangeColumn";
            this.dataGridViewAssets_ExchangeColumn.Visible = false;
            // 
            // PortfolioSummaryUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "PortfolioSummaryUserControl";
            this.Size = new System.Drawing.Size(1237, 787);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAssets)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.DataGridView dataGridViewAssets;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelTotalPercentage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelBetaPortfolio;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewAssets_TickerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewAssets_ProportionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewAssets_AmountColumn;
        private System.Windows.Forms.DataGridViewButtonColumn dataGridViewAssets_ShowDetailColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewAssets_ExchangeColumn;
    }
}
