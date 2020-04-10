using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Portfolios;
using FinancialAnalyst.Common.Entities.RequestResponse;
using FinancialAnalyst.Common.Interfaces.UIInterfaces;
using FinancialAnalyst.UI.Windows.UserControls;
using FinancialAnalyst.WebAPICallers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancialAnalyst.UI.Windows.ChildForms
{
    public partial class PortfolioPlannerForm : Form, ICallerForm
    {
        private readonly List<AssetDetailForm> showAssetDetailForms = new List<AssetDetailForm>();
        const string userName = "sgastia";


        public PortfolioPlannerForm()
        {
            InitializeComponent();
            splitContainerMain.Dock = DockStyle.Fill;
            splitContainerPortfoliosAndCommands.Dock = DockStyle.Fill;
            flowLayoutPanelPortfolios.Dock = DockStyle.Fill;
            portfolioDetailUserControl1.Dock = DockStyle.Fill;
        }

        private void PortfolioPlanner_Load(object sender, EventArgs e)
        {
            IEnumerable<Portfolio> portfolios = PortfoliosAPICaller.GetPortfoliosByUser(userName);
            foreach(Portfolio portfolio in portfolios)
            {
                Show(portfolio);
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

        private void buttonCreatePortfolioFromTransactions_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                //openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.Filter = "CSV files (*.csv)|*.csv";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Stream fileStream = openFileDialog.OpenFile();
                        string name = "New portfolio";
                        
                        APIResponse<Portfolio> response = PortfoliosAPICaller.CreateNewPortfolioFromTransacions(userName, name, fileStream);
                        if (response.Ok)
                            Show(response.Content);
                        else
                        {
                            MessageBox.Show(response.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            
        }

        private void PortfolioPlanner_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (AssetDetailForm form in showAssetDetailForms)
            {
                form.Close();
            }
        }

        private void Show(Portfolio portfolio)
        {
            PortfolioSummaryUserControl portfolioUserControl = new PortfolioSummaryUserControl(this);
            portfolioUserControl.Set(portfolio);
            portfolioUserControl.Click += PortfolioUserControl_Click;
            portfolioUserControl.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanelPortfolios.Controls.Add(portfolioUserControl);
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
            APIResponse<Stock> response = DataSourcesAPICaller.GetCompleteStockData(alloc.Ticker, alloc.Exchange, true ,true );
            AssetDetailForm showAssetDetailForm = new AssetDetailForm(response);
            showAssetDetailForms.Add(showAssetDetailForm);
            //showAssetDetailForm.MdiParent = this.ParentForm;
            showAssetDetailForm.Show();
        }

        
    }
}
