namespace FinancialAnalyst.UI.Windows
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemPortfolios = new System.Windows.Forms.ToolStripMenuItem();
            this.portfolioPlannerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSP500 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemPortfolios,
            this.toolStripMenuItemSP500});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItemPortfolios
            // 
            this.toolStripMenuItemPortfolios.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.portfolioPlannerToolStripMenuItem});
            this.toolStripMenuItemPortfolios.Name = "toolStripMenuItemPortfolios";
            this.toolStripMenuItemPortfolios.Size = new System.Drawing.Size(70, 20);
            this.toolStripMenuItemPortfolios.Text = "Portfolios";
            // 
            // portfolioPlannerToolStripMenuItem
            // 
            this.portfolioPlannerToolStripMenuItem.Name = "portfolioPlannerToolStripMenuItem";
            this.portfolioPlannerToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.portfolioPlannerToolStripMenuItem.Text = "Portfolio planner";
            this.portfolioPlannerToolStripMenuItem.Click += new System.EventHandler(this.portfolioPlannerToolStripMenuItem_Click);
            // 
            // toolStripMenuItemSP500
            // 
            this.toolStripMenuItemSP500.Name = "toolStripMenuItemSP500";
            this.toolStripMenuItemSP500.Size = new System.Drawing.Size(53, 20);
            this.toolStripMenuItemSP500.Text = "SP 500";
            this.toolStripMenuItemSP500.Click += new System.EventHandler(this.toolStripMenuItemSP500_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPortfolios;
        private System.Windows.Forms.ToolStripMenuItem portfolioPlannerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSP500;
    }
}

