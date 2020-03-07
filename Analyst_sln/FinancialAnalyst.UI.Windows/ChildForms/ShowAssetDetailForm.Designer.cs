namespace FinancialAnalyst.UI.Windows.ChildForms
{
    partial class ShowAssetDetailForm
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
            this.assetDetailUserControl1 = new FinancialAnalyst.UI.Windows.UserControls.AssetDetailUserControl();
            this.SuspendLayout();
            // 
            // assetDetailUserControl1
            // 
            this.assetDetailUserControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.assetDetailUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assetDetailUserControl1.Location = new System.Drawing.Point(0, 0);
            this.assetDetailUserControl1.Name = "assetDetailUserControl1";
            this.assetDetailUserControl1.Size = new System.Drawing.Size(800, 450);
            this.assetDetailUserControl1.TabIndex = 0;
            // 
            // ShowAssetDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.assetDetailUserControl1);
            this.Name = "ShowAssetDetailForm";
            this.Text = "ShowAssetDetailForm";
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.AssetDetailUserControl assetDetailUserControl1;
    }
}