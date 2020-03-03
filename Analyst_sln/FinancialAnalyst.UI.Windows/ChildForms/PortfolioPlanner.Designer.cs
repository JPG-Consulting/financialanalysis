namespace FinancialAnalyst.UI.Windows.ChildForms
{
    partial class PortfolioPlanner
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
            this.buttonCreatePortfolio = new System.Windows.Forms.Button();
            this.flowLayoutPanelPortfolios = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
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
            this.splitContainerMain.Panel1.Controls.Add(this.buttonCreatePortfolio);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.flowLayoutPanelPortfolios);
            this.splitContainerMain.Size = new System.Drawing.Size(800, 450);
            this.splitContainerMain.SplitterDistance = 68;
            this.splitContainerMain.TabIndex = 1;
            // 
            // buttonCreatePortfolio
            // 
            this.buttonCreatePortfolio.Location = new System.Drawing.Point(12, 12);
            this.buttonCreatePortfolio.Name = "buttonCreatePortfolio";
            this.buttonCreatePortfolio.Size = new System.Drawing.Size(75, 23);
            this.buttonCreatePortfolio.TabIndex = 0;
            this.buttonCreatePortfolio.Text = "Create portfolio";
            this.buttonCreatePortfolio.UseVisualStyleBackColor = true;
            this.buttonCreatePortfolio.Click += new System.EventHandler(this.buttonCreatePortfolio_Click);
            // 
            // flowLayoutPanelPortfolios
            // 
            this.flowLayoutPanelPortfolios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelPortfolios.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelPortfolios.Name = "flowLayoutPanelPortfolios";
            this.flowLayoutPanelPortfolios.Size = new System.Drawing.Size(800, 378);
            this.flowLayoutPanelPortfolios.TabIndex = 0;
            // 
            // PortfolioPlanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainerMain);
            this.Name = "PortfolioPlanner";
            this.Text = "PortfolioPlanner";
            this.Load += new System.EventHandler(this.PortfolioPlanner_Load);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Button buttonCreatePortfolio;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelPortfolios;
    }
}