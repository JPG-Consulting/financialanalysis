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
            this.splitContainerPortfoliosAndCommands = new System.Windows.Forms.SplitContainer();
            this.buttonCreatePortfolioFromTransactions = new System.Windows.Forms.Button();
            this.flowLayoutPanelPortfolios = new System.Windows.Forms.FlowLayoutPanel();
            this.portfolioDetailUserControl1 = new FinancialAnalyst.UI.Windows.UserControls.PortfolioDetailUserControl();
            this.buttonRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerPortfoliosAndCommands)).BeginInit();
            this.splitContainerPortfoliosAndCommands.Panel1.SuspendLayout();
            this.splitContainerPortfoliosAndCommands.Panel2.SuspendLayout();
            this.splitContainerPortfoliosAndCommands.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Location = new System.Drawing.Point(122, 46);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerPortfoliosAndCommands);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.portfolioDetailUserControl1);
            this.splitContainerMain.Size = new System.Drawing.Size(975, 520);
            this.splitContainerMain.SplitterDistance = 480;
            this.splitContainerMain.TabIndex = 1;
            // 
            // splitContainerPortfoliosAndCommands
            // 
            this.splitContainerPortfoliosAndCommands.Location = new System.Drawing.Point(0, 0);
            this.splitContainerPortfoliosAndCommands.Name = "splitContainerPortfoliosAndCommands";
            this.splitContainerPortfoliosAndCommands.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerPortfoliosAndCommands.Panel1
            // 
            this.splitContainerPortfoliosAndCommands.Panel1.Controls.Add(this.buttonRefresh);
            this.splitContainerPortfoliosAndCommands.Panel1.Controls.Add(this.buttonCreatePortfolioFromTransactions);
            // 
            // splitContainerPortfoliosAndCommands.Panel2
            // 
            this.splitContainerPortfoliosAndCommands.Panel2.Controls.Add(this.flowLayoutPanelPortfolios);
            this.splitContainerPortfoliosAndCommands.Size = new System.Drawing.Size(314, 429);
            this.splitContainerPortfoliosAndCommands.SplitterDistance = 63;
            this.splitContainerPortfoliosAndCommands.TabIndex = 1;
            // 
            // buttonCreatePortfolioFromTransactions
            // 
            this.buttonCreatePortfolioFromTransactions.Location = new System.Drawing.Point(95, 19);
            this.buttonCreatePortfolioFromTransactions.Name = "buttonCreatePortfolioFromTransactions";
            this.buttonCreatePortfolioFromTransactions.Size = new System.Drawing.Size(180, 23);
            this.buttonCreatePortfolioFromTransactions.TabIndex = 0;
            this.buttonCreatePortfolioFromTransactions.Text = "Create portfolio from transactions";
            this.buttonCreatePortfolioFromTransactions.UseVisualStyleBackColor = true;
            this.buttonCreatePortfolioFromTransactions.Click += new System.EventHandler(this.buttonCreatePortfolioFromTransactions_Click);
            // 
            // flowLayoutPanelPortfolios
            // 
            this.flowLayoutPanelPortfolios.Location = new System.Drawing.Point(14, 17);
            this.flowLayoutPanelPortfolios.Name = "flowLayoutPanelPortfolios";
            this.flowLayoutPanelPortfolios.Size = new System.Drawing.Size(169, 249);
            this.flowLayoutPanelPortfolios.TabIndex = 0;
            // 
            // portfolioDetailUserControl1
            // 
            this.portfolioDetailUserControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.portfolioDetailUserControl1.Location = new System.Drawing.Point(34, 140);
            this.portfolioDetailUserControl1.Name = "portfolioDetailUserControl1";
            this.portfolioDetailUserControl1.Size = new System.Drawing.Size(172, 258);
            this.portfolioDetailUserControl1.TabIndex = 0;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(14, 19);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 1;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // PortfolioPlannerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1227, 643);
            this.Controls.Add(this.splitContainerMain);
            this.Name = "PortfolioPlannerForm";
            this.Text = "PortfolioPlanner";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PortfolioPlanner_FormClosed);
            this.Load += new System.EventHandler(this.PortfolioPlanner_Load);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerPortfoliosAndCommands.Panel1.ResumeLayout(false);
            this.splitContainerPortfoliosAndCommands.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerPortfoliosAndCommands)).EndInit();
            this.splitContainerPortfoliosAndCommands.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Button buttonCreatePortfolioFromTransactions;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelPortfolios;
        private System.Windows.Forms.SplitContainer splitContainerPortfoliosAndCommands;
        private UserControls.PortfolioDetailUserControl portfolioDetailUserControl1;
        private System.Windows.Forms.Button buttonRefresh;
    }
}