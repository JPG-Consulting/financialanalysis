namespace Analyst.Windows
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnMockFiles = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnMockFiles
            // 
            this.btnMockFiles.Location = new System.Drawing.Point(80, 37);
            this.btnMockFiles.Name = "btnMockFiles";
            this.btnMockFiles.Size = new System.Drawing.Size(167, 47);
            this.btnMockFiles.TabIndex = 0;
            this.btnMockFiles.Text = "Create mock files";
            this.btnMockFiles.UseVisualStyleBackColor = true;
            this.btnMockFiles.Click += new System.EventHandler(this.btnMockFiles_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(80, 108);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(167, 54);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1039, 544);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnMockFiles);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnMockFiles;
        private System.Windows.Forms.Button button1;
    }
}

