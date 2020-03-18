using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Entities.RequestResponse;
using FinancialAnalyst.UI.Windows.Managers;
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
using System.Windows.Forms.DataVisualization.Charting;

namespace FinancialAnalyst.UI.Windows.ChildForms
{
    public partial class StandardAndPoorsForm : Form
    {
        public StandardAndPoorsForm()
        {
            InitializeComponent();
            chartSP.Dock = DockStyle.Fill;
        }

        private void StandardAndPoors_Load(object sender, EventArgs e)
        {
            string ticker = "^GSPC";
            APIResponse<PriceList> pricesResponse = FinancialAnalystWebAPICaller.GetPrices(ticker, null,DateTime.Now.AddYears(-50),DateTime.Now,PriceInterval.Monthly);
            
            chartSP.Series.Clear();
            chartSP.Titles.Add("S&P 500");

            //for volume
            //https://stackoverflow.com/questions/17303378/creating-multiple-charts-and-the-relation-between-chart-series-chartarea

            Series priceSeries = this.chartSP.Series.Add("Prices");
            priceSeries.ChartType = SeriesChartType.Spline;
            foreach(Price p in pricesResponse.Content)
            {
                priceSeries.Points.AddXY(p.Date, p.Close);
            }
            


        }
    }
}
