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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.label1 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.dataGridViewAssets = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.labelTotalCosts = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelBetaPortfolio = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.groupboxMetrics = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.labelCash = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelMarketValue = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dataGridViewAssets_TickerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewAssets_CostsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewAssets_MarketValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewAssets_ProportionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewAssets_ShowDetailColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAssets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupboxMetrics.SuspendLayout();
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
            this.dataGridViewAssets_CostsColumn,
            this.dataGridViewAssets_MarketValueColumn,
            this.dataGridViewAssets_ProportionColumn,
            this.dataGridViewAssets_ShowDetailColumn});
            this.dataGridViewAssets.Location = new System.Drawing.Point(12, 25);
            this.dataGridViewAssets.Name = "dataGridViewAssets";
            this.dataGridViewAssets.ShowEditingIcon = false;
            this.dataGridViewAssets.Size = new System.Drawing.Size(381, 696);
            this.dataGridViewAssets.TabIndex = 2;
            this.dataGridViewAssets.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewAssets_CellContentClick);
            this.dataGridViewAssets.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewAssets_CellValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(6, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Total costs:";
            // 
            // labelTotalCosts
            // 
            this.labelTotalCosts.AutoSize = true;
            this.labelTotalCosts.Location = new System.Drawing.Point(74, 20);
            this.labelTotalCosts.Name = "labelTotalCosts";
            this.labelTotalCosts.Size = new System.Drawing.Size(67, 13);
            this.labelTotalCosts.TabIndex = 4;
            this.labelTotalCosts.Text = "<total costs>";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Beta:";
            // 
            // labelBetaPortfolio
            // 
            this.labelBetaPortfolio.AutoSize = true;
            this.labelBetaPortfolio.Location = new System.Drawing.Point(74, 84);
            this.labelBetaPortfolio.Name = "labelBetaPortfolio";
            this.labelBetaPortfolio.Size = new System.Drawing.Size(80, 13);
            this.labelBetaPortfolio.TabIndex = 6;
            this.labelBetaPortfolio.Text = "<beta portfolio>";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.buttonUpdate);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.labelName);
            this.splitContainer1.Panel1.Controls.Add(this.dataGridViewAssets);
            this.splitContainer1.Panel1MinSize = 400;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupboxMetrics);
            this.splitContainer1.Panel2.Controls.Add(this.chart1);
            this.splitContainer1.Size = new System.Drawing.Size(1237, 787);
            this.splitContainer1.SplitterDistance = 400;
            this.splitContainer1.TabIndex = 7;
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(318, 3);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdate.TabIndex = 3;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // groupboxMetrics
            // 
            this.groupboxMetrics.Controls.Add(this.label6);
            this.groupboxMetrics.Controls.Add(this.labelCash);
            this.groupboxMetrics.Controls.Add(this.label3);
            this.groupboxMetrics.Controls.Add(this.labelMarketValue);
            this.groupboxMetrics.Controls.Add(this.labelBetaPortfolio);
            this.groupboxMetrics.Controls.Add(this.label4);
            this.groupboxMetrics.Controls.Add(this.label2);
            this.groupboxMetrics.Controls.Add(this.labelTotalCosts);
            this.groupboxMetrics.Location = new System.Drawing.Point(3, 9);
            this.groupboxMetrics.Name = "groupboxMetrics";
            this.groupboxMetrics.Size = new System.Drawing.Size(204, 177);
            this.groupboxMetrics.TabIndex = 7;
            this.groupboxMetrics.TabStop = false;
            this.groupboxMetrics.Text = "Metrics";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Cash:";
            // 
            // labelCash
            // 
            this.labelCash.AutoSize = true;
            this.labelCash.Location = new System.Drawing.Point(74, 62);
            this.labelCash.Name = "labelCash";
            this.labelCash.Size = new System.Drawing.Size(42, 13);
            this.labelCash.TabIndex = 10;
            this.labelCash.Text = "<cash>";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Mkt value:";
            // 
            // labelMarketValue
            // 
            this.labelMarketValue.AutoSize = true;
            this.labelMarketValue.Location = new System.Drawing.Point(74, 40);
            this.labelMarketValue.Name = "labelMarketValue";
            this.labelMarketValue.Size = new System.Drawing.Size(68, 13);
            this.labelMarketValue.TabIndex = 8;
            this.labelMarketValue.Text = "<total value>";
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(403, 252);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(300, 300);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // dataGridViewAssets_TickerColumn
            // 
            this.dataGridViewAssets_TickerColumn.FillWeight = 40F;
            this.dataGridViewAssets_TickerColumn.HeaderText = "Ticker";
            this.dataGridViewAssets_TickerColumn.MaxInputLength = 10;
            this.dataGridViewAssets_TickerColumn.Name = "dataGridViewAssets_TickerColumn";
            this.dataGridViewAssets_TickerColumn.ReadOnly = true;
            this.dataGridViewAssets_TickerColumn.Width = 120;
            // 
            // dataGridViewAssets_CostsColumn
            // 
            this.dataGridViewAssets_CostsColumn.DataPropertyName = "Costs";
            this.dataGridViewAssets_CostsColumn.FillWeight = 20F;
            this.dataGridViewAssets_CostsColumn.HeaderText = "Costs";
            this.dataGridViewAssets_CostsColumn.Name = "dataGridViewAssets_CostsColumn";
            this.dataGridViewAssets_CostsColumn.ReadOnly = true;
            this.dataGridViewAssets_CostsColumn.Width = 60;
            // 
            // dataGridViewAssets_MarketValueColumn
            // 
            this.dataGridViewAssets_MarketValueColumn.DataPropertyName = "MarketValue";
            this.dataGridViewAssets_MarketValueColumn.FillWeight = 20F;
            this.dataGridViewAssets_MarketValueColumn.HeaderText = "Mkt Value";
            this.dataGridViewAssets_MarketValueColumn.Name = "dataGridViewAssets_MarketValueColumn";
            this.dataGridViewAssets_MarketValueColumn.Width = 60;
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
            // dataGridViewAssets_ShowDetailColumn
            // 
            this.dataGridViewAssets_ShowDetailColumn.FillWeight = 10F;
            this.dataGridViewAssets_ShowDetailColumn.HeaderText = "Detail";
            this.dataGridViewAssets_ShowDetailColumn.Name = "dataGridViewAssets_ShowDetailColumn";
            this.dataGridViewAssets_ShowDetailColumn.ReadOnly = true;
            this.dataGridViewAssets_ShowDetailColumn.Text = "Detail";
            this.dataGridViewAssets_ShowDetailColumn.UseColumnTextForButtonValue = true;
            this.dataGridViewAssets_ShowDetailColumn.Width = 50;
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
            this.groupboxMetrics.ResumeLayout(false);
            this.groupboxMetrics.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.DataGridView dataGridViewAssets;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelTotalCosts;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelBetaPortfolio;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.GroupBox groupboxMetrics;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelCash;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelMarketValue;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewAssets_TickerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewAssets_CostsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewAssets_MarketValueColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewAssets_ProportionColumn;
        private System.Windows.Forms.DataGridViewButtonColumn dataGridViewAssets_ShowDetailColumn;
    }
}
