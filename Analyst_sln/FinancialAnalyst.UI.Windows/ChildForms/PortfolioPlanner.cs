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
    public partial class PortfolioPlanner : Form, IAssetDetailShower
    {
        public PortfolioPlanner()
        {
            InitializeComponent();
            splitContainerMain.Dock = DockStyle.Fill;
            splitContainerPortfoliosAndCommands.Dock = DockStyle.Fill;
            flowLayoutPanelPortfolios.Dock = DockStyle.Fill;
            assetDetailUserControl1.Dock = DockStyle.Fill;
        }

        private void PortfolioPlanner_Load(object sender, EventArgs e)
        {
            string user = "asdf";
            IEnumerable<Portfolio> portfolios = FinancialAnalystWebAPICaller.GetPortfoliosByUser(user);
            foreach(Portfolio portfolio in portfolios)
            {
                PortfolioUserControl portfolioUserControl = new PortfolioUserControl(this);
                portfolioUserControl.Set(portfolio);
                flowLayoutPanelPortfolios.Controls.Add(portfolioUserControl);
                
            }
        }

        private void buttonCreatePortfolio_Click(object sender, EventArgs e)
        {

        }

        public void ShowAssetDetail(AssetAllocation alloc)
        {
            APIResponse<Stock> response = FinancialAnalystWebAPICaller.GetAssetData(alloc.Ticker, alloc.Exchange);
            assetDetailUserControl1.Show(response);
        }

    }
}
