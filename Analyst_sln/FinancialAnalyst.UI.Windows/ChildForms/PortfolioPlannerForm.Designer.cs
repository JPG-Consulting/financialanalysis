﻿namespace FinancialAnalyst.UI.Windows.ChildForms
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
            this.buttonCreatePortfolio = new System.Windows.Forms.Button();
            this.flowLayoutPanelPortfolios = new System.Windows.Forms.FlowLayoutPanel();
            this.portfolioDetailUserControl1 = new FinancialAnalyst.UI.Windows.UserControls.PortfolioDetailUserControl();
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
            this.splitContainerMain.SplitterDistance = 481;
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
            this.splitContainerPortfoliosAndCommands.Panel1.Controls.Add(this.buttonCreatePortfolio);
            // 
            // splitContainerPortfoliosAndCommands.Panel2
            // 
            this.splitContainerPortfoliosAndCommands.Panel2.Controls.Add(this.flowLayoutPanelPortfolios);
            this.splitContainerPortfoliosAndCommands.Size = new System.Drawing.Size(314, 429);
            this.splitContainerPortfoliosAndCommands.SplitterDistance = 63;
            this.splitContainerPortfoliosAndCommands.TabIndex = 1;
            // 
            // buttonCreatePortfolio
            // 
            this.buttonCreatePortfolio.Location = new System.Drawing.Point(3, 19);
            this.buttonCreatePortfolio.Name = "buttonCreatePortfolio";
            this.buttonCreatePortfolio.Size = new System.Drawing.Size(75, 23);
            this.buttonCreatePortfolio.TabIndex = 0;
            this.buttonCreatePortfolio.Text = "Create portfolio";
            this.buttonCreatePortfolio.UseVisualStyleBackColor = true;
            this.buttonCreatePortfolio.Click += new System.EventHandler(this.buttonCreatePortfolio_Click);
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
            // PortfolioPlanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1227, 643);
            this.Controls.Add(this.splitContainerMain);
            this.Name = "PortfolioPlanner";
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
        private System.Windows.Forms.Button buttonCreatePortfolio;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelPortfolios;
        private System.Windows.Forms.SplitContainer splitContainerPortfoliosAndCommands;
        private UserControls.PortfolioDetailUserControl portfolioDetailUserControl1;
    }
}