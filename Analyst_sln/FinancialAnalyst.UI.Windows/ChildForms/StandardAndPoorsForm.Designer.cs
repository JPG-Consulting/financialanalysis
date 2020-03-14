namespace FinancialAnalyst.UI.Windows.ChildForms
{
    partial class StandardAndPoorsForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartSP = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartSP)).BeginInit();
            this.SuspendLayout();
            // 
            // chartSP
            // 
            chartArea1.Name = "ChartArea1";
            this.chartSP.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartSP.Legends.Add(legend1);
            this.chartSP.Location = new System.Drawing.Point(36, 49);
            this.chartSP.Name = "chartSP";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartSP.Series.Add(series1);
            this.chartSP.Size = new System.Drawing.Size(641, 366);
            this.chartSP.TabIndex = 0;
            this.chartSP.Text = "chart1";
            // 
            // StandardAndPoors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 550);
            this.Controls.Add(this.chartSP);
            this.Name = "StandardAndPoors";
            this.Text = "StandardAndPoors";
            this.Load += new System.EventHandler(this.StandardAndPoors_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartSP)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartSP;
    }
}