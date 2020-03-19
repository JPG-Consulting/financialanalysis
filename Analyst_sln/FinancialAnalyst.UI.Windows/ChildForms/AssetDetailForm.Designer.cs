namespace FinancialAnalyst.UI.Windows.ChildForms
{
    partial class AssetDetailForm
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
            this.assetDetailUserControl1 = new FinancialAnalyst.UI.Windows.UserControls.AssetDetailUserControl();
            this.optionsChainUserControl1 = new FinancialAnalyst.UI.Windows.UserControls.OptionsChainUserControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Location = new System.Drawing.Point(55, 47);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.assetDetailUserControl1);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.optionsChainUserControl1);
            this.splitContainerMain.Size = new System.Drawing.Size(939, 441);
            this.splitContainerMain.SplitterDistance = 493;
            this.splitContainerMain.TabIndex = 1;
            // 
            // assetDetailUserControl1
            // 
            this.assetDetailUserControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.assetDetailUserControl1.Location = new System.Drawing.Point(3, 14);
            this.assetDetailUserControl1.Name = "assetDetailUserControl1";
            this.assetDetailUserControl1.Size = new System.Drawing.Size(279, 254);
            this.assetDetailUserControl1.TabIndex = 0;
            // 
            // optionsChainUserControl1
            // 
            this.optionsChainUserControl1.Location = new System.Drawing.Point(33, 62);
            this.optionsChainUserControl1.Name = "optionsChainUserControl1";
            this.optionsChainUserControl1.Size = new System.Drawing.Size(254, 279);
            this.optionsChainUserControl1.TabIndex = 0;
            // 
            // AssetDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 586);
            this.Controls.Add(this.splitContainerMain);
            this.Name = "AssetDetailForm";
            this.Text = "ShowAssetDetailForm";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.AssetDetailUserControl assetDetailUserControl1;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private UserControls.OptionsChainUserControl optionsChainUserControl1;
    }
}