using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Portfolios;
using FinancialAnalyst.Common.Entities.RequestResponse;
using FinancialAnalyst.Common.Interfaces.UIInterfaces;
using FinancialAnalyst.UI.Windows.Managers;
using FinancialAnalyst.UI.Windows.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancialAnalyst.UI.Windows.ChildForms
{
    public partial class PortfolioPlanner : Form, ICallerForm
    {
        private readonly List<ShowAssetDetailForm> showAssetDetailForms = new List<ShowAssetDetailForm>();

        public PortfolioPlanner()
        {
            InitializeComponent();
            splitContainerMain.Dock = DockStyle.Fill;
            splitContainerPortfoliosAndCommands.Dock = DockStyle.Fill;
            flowLayoutPanelPortfolios.Dock = DockStyle.Fill;
            portfolioDetailUserControl1.Dock = DockStyle.Fill;
        }

        private void PortfolioPlanner_Load(object sender, EventArgs e)
        {
            string user = "asdf";
            IEnumerable<Portfolio> portfolios = FinancialAnalystWebAPICaller.GetPortfoliosByUser(user);
            foreach(Portfolio portfolio in portfolios)
            {
                PortfolioSummaryUserControl portfolioUserControl = new PortfolioSummaryUserControl(this);
                portfolioUserControl.Set(portfolio);
                portfolioUserControl.Click += PortfolioUserControl_Click;
                portfolioUserControl.BorderStyle = BorderStyle.FixedSingle;
                flowLayoutPanelPortfolios.Controls.Add(portfolioUserControl);
                
            }
        }

        

        private void PortfolioUserControl_Click(object sender, EventArgs e)
        {
            SetBorderToPortfolios(BorderStyle.FixedSingle);
            PortfolioSummaryUserControl control = sender as PortfolioSummaryUserControl;
            if(sender != null)
            {
                control.BorderStyle = BorderStyle.Fixed3D;
            }
        }

        private void buttonCreatePortfolio_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pending to create portfolio");
        }

        private void SetBorderToPortfolios(BorderStyle borderStyle)
        {
            foreach (PortfolioSummaryUserControl portfolioUserControl in flowLayoutPanelPortfolios.Controls)
            {
                portfolioUserControl.BorderStyle = borderStyle;
            }
        }

        public void Show(AssetAllocation alloc)
        {
            //I create a form for every request because maybe the user want to see several asset details at the same time
            APIResponse<Stock> response = FinancialAnalystWebAPICaller.GetAssetData(alloc.Ticker, alloc.Exchange);
            ShowAssetDetailForm showAssetDetailForm = new ShowAssetDetailForm(response);
            showAssetDetailForms.Add(showAssetDetailForm);
            //showAssetDetailForm.MdiParent = this.ParentForm;
            showAssetDetailForm.Show();
        }

        private void PortfolioPlanner_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach(ShowAssetDetailForm form in showAssetDetailForms)
            {
                form.Close();
            }
        }
    }
}
