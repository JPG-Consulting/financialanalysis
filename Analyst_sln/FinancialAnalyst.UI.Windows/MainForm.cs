using FinancialAnalyst.UI.Windows.ChildForms;
using FinancialAnalyst.WebAPICallers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancialAnalyst.UI.Windows
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            //TODO: Get all these data from configuration
#if DEBUG            
            //IIS
            //string baseAddress = "http://localhost/FinancialAnalyst.WebAPI";

            //IIS Express
            string baseAddress = "https://localhost:44353";
#endif
            HttpClientWebAPI.Initialize(baseAddress);
        }

        private void portfolioPlannerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PortfolioPlannerForm form = new PortfolioPlannerForm();
            form.MdiParent = this;
            form.Show();
        }

        private void toolStripMenuItemSP500_Click(object sender, EventArgs e)
        {
            StandardAndPoorsForm form = new StandardAndPoorsForm();
            form.MdiParent = this;
            form.Show();
        }


    }
}
