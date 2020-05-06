using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FinancialAnalyst.Common.Entities.Portfolios;
using FinancialAnalyst.Common.Interfaces.UIInterfaces;
using System.Runtime.InteropServices;
using FinancialAnalyst.WebAPICallers;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Entities.RequestResponse;
using FinancialAnalyst.Common.Entities.Assets;

namespace FinancialAnalyst.UI.Windows.UserControls
{
    public partial class PortfolioSummaryUserControl: UserControl
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(PortfolioSummaryUserControl));

        private Portfolio _selectedPortfolio;
        private ICallerForm _callerForm;
        private Task _updateTask = null;

        public PortfolioSummaryUserControl()
        {
            InitializeComponent();
            dataGridViewAssets.AutoGenerateColumns = false;
            dataGridViewAssets.DataBindingComplete += DataGridViewAssets_DataBindingComplete;
        }

        public void Set(ICallerForm callerForm, Portfolio portfolio)
        {
            if (_selectedPortfolio != null)
            { 
                if (_updateTask != null)
                    Task.WaitAll(new Task[] { _updateTask });
            }

            _callerForm = callerForm;
            _selectedPortfolio = portfolio;

            dataGridViewAssets.AutoGenerateColumns = false;

            int indexPercentage = dataGridViewAssets.Columns.Cast<DataGridViewColumn>().Where(c => c.Name == dataGridViewAssets_ProportionColumn.Name).Single().Index;
            dataGridViewAssets.Columns[indexPercentage].DefaultCellStyle.Format = "0.00\\%";
            dataGridViewAssets.Columns[indexPercentage].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            int indexMktValue = dataGridViewAssets.Columns.Cast<DataGridViewColumn>().Where(c => c.Name == dataGridViewAssets_MarketValueColumn.Name).Single().Index;
            dataGridViewAssets.Columns[indexMktValue].DefaultCellStyle.Format = "N2";
            dataGridViewAssets.Columns[indexMktValue].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            
            dataGridViewAssets.DataSource = portfolio.AssetAllocations;
            labelName.Text = portfolio.Name;

            int indexCosts = dataGridViewAssets.Columns.Cast<DataGridViewColumn>().Where(c => c.Name == dataGridViewAssets_CostsColumn.Name).Single().Index;
            dataGridViewAssets.Columns[indexCosts].Visible = portfolio.IsSimulated == false;

            CalculateTotals(portfolio, false);
        }

        public void Clear()
        {
            _selectedPortfolio = null;
            dataGridViewAssets.DataSource = null;
        }

        private void dataGridViewAssets_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string msg = String.Format("Cell at row {0}, column {1} value changed",e.RowIndex, e.ColumnIndex);

            if (e.RowIndex < 0)
                return;

            if(e.ColumnIndex == dataGridViewAssets_ProportionColumn.Index)
            {
                CalculateTotals(_selectedPortfolio, false);
            }
        }

        private void dataGridViewAssets_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == dataGridViewAssets_ShowDetailColumn.Index)
            {
                AssetAllocation assetAllocation = (AssetAllocation)dataGridViewAssets.Rows[e.RowIndex].DataBoundItem;
                _callerForm.Show(assetAllocation);
            }
        }

        private void DataGridViewAssets_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewAssets.Rows)
            {
                string ticker = ((AssetAllocation)row.DataBoundItem).Asset.Ticker;
                row.Cells[dataGridViewAssets_TickerColumn.Index].Value = ticker;
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            LaunchUpdateTask(_selectedPortfolio);
        }

        private void CalculateTotals(Portfolio portfolio, bool savePortfolio)
        {
            int indexPercentage = dataGridViewAssets_ProportionColumn.Index;
            int indexCosts = dataGridViewAssets_CostsColumn.Index;
            int indexMarketValue = dataGridViewAssets_MarketValueColumn.Index;
            decimal totalPercentage = 0;
            decimal totalCosts = 0;
            decimal totalValue = 0;
            for (int i=0;i< dataGridViewAssets.Rows.Count;i++)
            {
                DataGridViewRow row = dataGridViewAssets.Rows[i];
                if(row.Cells[indexPercentage].Value != null)
                    totalPercentage += (decimal)row.Cells[indexPercentage].Value;

                if (row.Cells[indexCosts].Value != null)
                    totalCosts += (decimal)row.Cells[indexCosts].Value;

                if (row.Cells[indexMarketValue].Value != null)
                    totalValue += (decimal)row.Cells[indexMarketValue].Value;
            }

            if (portfolio.TotalCosts.HasValue)
            {
                if (totalCosts == portfolio.TotalCosts.Value)
                {
                    labelTotalCosts.Text = portfolio.TotalCosts.Value.ToString("N2");
                    labelTotalCosts.BackColor = SystemColors.Control; ;
                }
                else
                {
                    labelTotalCosts.Text = $"{totalCosts.ToString("N2")} (Diff:{(totalCosts - portfolio.TotalCosts.Value).ToString("N2")})";
                    labelTotalCosts.BackColor = Color.Red;
                }
            }
            else
            {
                labelTotalCosts.Text = "0.00";
                labelTotalCosts.BackColor = SystemColors.Control; ;
            }

            if (portfolio.MarketValue.HasValue)
            {
                if (totalValue == portfolio.MarketValue.Value)
                {
                    
                    labelMarketValue.Text = portfolio.MarketValue.Value.ToString("N2");
                    labelMarketValue.BackColor = SystemColors.Control;
                }
                else
                {
                    labelMarketValue.Text = $"{totalValue.ToString("N2")} (Diff:{(totalValue - portfolio.MarketValue.Value).ToString("N2")})";
                    labelMarketValue.BackColor = Color.Red;
                }
            }
            else
            {
                labelMarketValue.Text = "0.00";
                labelMarketValue.BackColor = SystemColors.Control;
            }


            if (portfolio.Cash.HasValue)
            {
                labelCash.Text = portfolio.Cash.Value.ToString("N2");
                if(portfolio.CashPercentage.HasValue)
                    labelCash.Text += $" ({portfolio.CashPercentage.Value.ToString("N2")}%)"; 
            }
            else
                labelCash.Text = "0.00 (0.00%)";

            if(savePortfolio)
                UpdatePortfolio(portfolio, totalValue);

        }

        private void UpdatePortfolio(Portfolio portfolio, decimal marketValue)
        {
            bool ok = PortfoliosAPICaller.Save(portfolio.Id, marketValue, out APIResponse<bool> response, out string message);
            if (ok)
            {
                if (response.Ok)
                {
                    MessageBox.Show(response.ErrorMessage, "Update process", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(response.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LaunchUpdateTask(Portfolio portfolio)
        {
            if(_updateTask != null)
            {
                MessageBox.Show("Task is already running", "Update proces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if(_selectedPortfolio == null)
            {
                MessageBox.Show("Must select a portfolio", "Update proces", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _updateTask = Task.Factory.StartNew(() =>
            {
                try
                {
                    //update market values
                    int i = 0;
                    decimal total = 0;
                    while (i < portfolio.AssetAllocations.Count)
                    {
                        AssetAllocation assetAllocation = portfolio.AssetAllocations[i];
                        try
                        {
                            bool ok = PortfoliosAPICaller.UpdateAssetAllocation(assetAllocation, out APIResponse<AssetAllocation> response ,out string message);
                            decimal marketValue;
                            if (ok)
                            {
                                marketValue = response.Content.MarketValue.Value;
                                portfolio.AssetAllocations[i] = response.Content;
                            }
                            else
                                marketValue = 0;
                            
                            total += marketValue;

                            Invoke(new Action(() => dataGridViewAssets.Refresh()));
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"LaunchUpdateTask failed for portfolio='{portfolio.Name}' (UserId='{portfolio.UserId}'), Ticker={assetAllocation.Asset.Ticker}: {ex.Message}", ex);
                        }
                        i++;
                    }

                    //pending to update percentages
                    foreach(AssetAllocation assetAllocation in portfolio.AssetAllocations)
                    {
                        if (assetAllocation.MarketValue.HasValue)
                            assetAllocation.Percentage = assetAllocation.MarketValue.Value / total * 100;
                        else
                        {
                            if (assetAllocation.Costs.HasValue)
                                assetAllocation.Percentage = assetAllocation.Costs.Value / total * 100;
                        }

                        Invoke(new Action(() => dataGridViewAssets.Refresh()));
                    }

                    Invoke(new Action(() =>
                    {
                        CalculateTotals(portfolio, true);
                    }));


                }
                catch (Exception ex)
                {
                    _logger.Error($"LaunchUpdateTask failed for portfolio='{portfolio.Name}' (UserId='{portfolio.UserId}'): {ex.Message}", ex);
                    MessageBox.Show(ex.Message, "Prices update failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    _updateTask = null;
                }
            });
        }
    }
}
