using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Portfolios;
using FinancialAnalyst.Common.Entities.RequestResponse;
using FinancialAnalyst.UI.Windows.Managers;
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
    public partial class PortfolioPlanner : Form
    {
        public PortfolioPlanner()
        {
            InitializeComponent();
        }

        private void PortfolioPlanner_Load(object sender, EventArgs e)
        {
            string user = "asdf";
            IEnumerable<Portfolio> portfolios = FinancialAnalystWebAPICaller.GetPortfoliosByUser(user);
            foreach(Portfolio portfolio in portfolios)
            {
                foreach(AssetAllocation alloc in portfolio.AssetAllocations)
                {
                    Label lbl = new Label();
                    APIResponse<Stock> response = FinancialAnalystWebAPICaller.GetAssetData(alloc.Ticker, alloc.Ticker_Market);
                    if (response.Ok)
                    {
                        lbl.Text = response.Content.Description;
                    }
                    else
                    {
                        lbl.Text = response.ErrorMessage;
                    }
                    flowLayoutPanelPortfolios.Controls.Add(lbl);
                }
            }
        }

        private void buttonCreatePortfolio_Click(object sender, EventArgs e)
        {

        }
    }
}
