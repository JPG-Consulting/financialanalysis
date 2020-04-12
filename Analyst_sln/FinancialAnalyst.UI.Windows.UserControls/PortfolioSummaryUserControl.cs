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

namespace FinancialAnalyst.UI.Windows.UserControls
{
    public partial class PortfolioSummaryUserControl: UserControl
    {
        private Portfolio portfolio;
        private ICallerForm callerForm;

        public PortfolioSummaryUserControl()
        {
            InitializeComponent();
            dataGridViewAssets.AutoGenerateColumns = false;
        }

        public void Set(ICallerForm callerForm,Portfolio portfolio)
        {
            this.callerForm = callerForm;
            dataGridViewAssets.AutoGenerateColumns = false;

            int indexPercentage = dataGridViewAssets.Columns.Cast<DataGridViewColumn>().Where(c => c.Name == dataGridViewAssets_ProportionColumn.Name).Single().Index;
            dataGridViewAssets.Columns[indexPercentage].DefaultCellStyle.Format = "0.00\\%";
            dataGridViewAssets.Columns[indexPercentage].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            int indexMktValue = dataGridViewAssets.Columns.Cast<DataGridViewColumn>().Where(c => c.Name == dataGridViewAssets_MarketValueColumn.Name).Single().Index;
            dataGridViewAssets.Columns[indexMktValue].DefaultCellStyle.Format = "N2";
            dataGridViewAssets.Columns[indexMktValue].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.portfolio = portfolio;
            dataGridViewAssets.DataSource = portfolio.AssetAllocations;
            labelName.Text = portfolio.Name;

            int indexCosts = dataGridViewAssets.Columns.Cast<DataGridViewColumn>().Where(c => c.Name == dataGridViewAssets_CostsColumn.Name).Single().Index;
            dataGridViewAssets.Columns[indexCosts].Visible = portfolio.IsSimulated == false;

            CalculateTotals(portfolio);
        }

        private void dataGridViewAssets_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string msg = String.Format("Cell at row {0}, column {1} value changed",e.RowIndex, e.ColumnIndex);

            if (e.RowIndex < 0)
                return;

            if(e.ColumnIndex == dataGridViewAssets_ProportionColumn.Index)
            {
                CalculateTotals(portfolio);
            }
        }

        private void dataGridViewAssets_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == dataGridViewAssets_ShowDetailColumn.Index)
            {
                AssetAllocation assetAllocation = (AssetAllocation)dataGridViewAssets.Rows[e.RowIndex].DataBoundItem;
                callerForm.Show(assetAllocation);
            }
        }

        private void CalculateTotals(Portfolio portfolio)
        {
            int indexPercentage = dataGridViewAssets.Columns.Cast<DataGridViewColumn>().Where(c => c.Name == dataGridViewAssets_ProportionColumn.Name).Single().Index;
            int indexCosts = dataGridViewAssets.Columns.Cast<DataGridViewColumn>().Where(c => c.Name == dataGridViewAssets_CostsColumn.Name).Single().Index;
            int indexMarketValue = dataGridViewAssets.Columns.Cast<DataGridViewColumn>().Where(c => c.Name == dataGridViewAssets_MarketValueColumn.Name).Single().Index;
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
                labelTotalCosts.Text = "0.00";

            if (portfolio.MarketValue.HasValue)
            {
                if (totalValue == portfolio.MarketValue.Value)
                {
                    labelMarketValue.Text = portfolio.MarketValue.Value.ToString("N2");
                    labelMarketValue.BackColor = SystemColors.Control; ;
                }
                else
                {
                    labelMarketValue.Text = $"{totalCosts.ToString("N2")} (Diff:{(totalValue - portfolio.MarketValue.Value).ToString("N2")})";
                    labelMarketValue.BackColor = Color.Red;
                }
            }
            else
                labelMarketValue.Text = "0.00";



            if (portfolio.Cash.HasValue)
                labelCash.Text = portfolio.Cash.Value.ToString("N2");
            else
                labelCash.Text = "0.00";
        }

        private void CalculateBeta()
        {

        }
    }
}
