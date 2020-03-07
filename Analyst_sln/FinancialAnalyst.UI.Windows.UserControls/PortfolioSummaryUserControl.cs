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

namespace FinancialAnalyst.UI.Windows.UserControls
{
    public partial class PortfolioSummaryUserControl: UserControl
    {
        private Portfolio portfolio;
        private ICallerForm callerForm;

        public PortfolioSummaryUserControl(ICallerForm callerForm)
        {
            InitializeComponent();
            this.callerForm = callerForm;
        }

        public void Set(Portfolio portfolio)
        {
            this.portfolio = portfolio;
            dataGridViewAssets.DataSource = portfolio.AssetAllocations;
            labelName.Text = portfolio.Name;
            CalculateTotal();
            CalculateBeta();
        }

        private void dataGridViewAssets_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string msg = String.Format("Cell at row {0}, column {1} value changed",e.RowIndex, e.ColumnIndex);

            if (e.RowIndex < 0)
                return;

            if(e.ColumnIndex == dataGridViewAssets_ProportionColumn.Index)
            {
                CalculateTotal();
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

        private void CalculateTotal()
        {
            int index = dataGridViewAssets.Columns.Cast<DataGridViewColumn>().Where(c => c.Name == dataGridViewAssets_ProportionColumn.Name).Single().Index;
            Decimal total = 0;
            for (int i=1;i< dataGridViewAssets.Rows.Count;i++)
            {
                DataGridViewRow row = dataGridViewAssets.Rows[i];
                if(row.Cells[index].Value != null)
                    total += (decimal)row.Cells[index].Value;
            }
            labelTotalPercentage.Text = $"{total.ToString("0.00")}%";
        }

        private void CalculateBeta()
        {

        }


    }
}
