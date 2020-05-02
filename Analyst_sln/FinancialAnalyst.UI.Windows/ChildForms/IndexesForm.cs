using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Entities.RequestResponse;
using FinancialAnalyst.WebAPICallers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FinancialAnalyst.UI.Windows.ChildForms
{
    public partial class IndexesForm : Form
    {
        public IndexesForm()
        {
            InitializeComponent();
            chartSP.Dock = DockStyle.Fill;
        }

        private void StandardAndPoors_Load(object sender, EventArgs e)
        {
            string ticker = "^GSPC";
            APIResponse<PriceList> pricesResponse = DataSourcesAPICaller.GetPrices(ticker, null,DateTime.Now.AddYears(-50),DateTime.Now,PriceInterval.Monthly);
            
            chartSP.Series.Clear();
            chartSP.Titles.Add("S&P 500");

            //for volume
            //https://stackoverflow.com/questions/17303378/creating-multiple-charts-and-the-relation-between-chart-series-chartarea

            Series priceSeries = this.chartSP.Series.Add("Prices");
            priceSeries.ChartType = SeriesChartType.Spline;
            foreach(HistoricalPrice p in pricesResponse.Content)
            {
                priceSeries.Points.AddXY(p.Date, p.Close);
            }
            


        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.investopedia.com/ask/answers/08/find-stocks-in-sp500.asp#sp-500-calculation");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.investopedia.com/terms/r/russell2000.asp");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.investopedia.com/articles/investing/012215/how-sp-500-and-russell-2000-indexes-differ.asp");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.investopedia.com/terms/r/russell_3000.asp");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.nasdaq.com/market-activity/quotes/nasdaq-ndx-index");
        }

        private void linkLabelBlackLitterman_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://faculty.fuqua.duke.edu/~charvey/Teaching/BA453_2006/Idzorek_onBL.pdf");
            Process.Start("http://www.diva-portal.org/smash/get/diva2:1253673/FULLTEXT01.pdf");
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.mit.edu/~dbertsim/papers/Finance/Inverse%20Optimization%20-%20A%20New%20Perspective%20on%20the%20Black-Litterman%20Model.pdf");
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://en.wikipedia.org/wiki/NASDAQ_Composite");
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://datahub.io/core/s-and-p-500-companies#data");
            Process.Start("https://pkgstore.datahub.io/core/s-and-p-500-companies/constituents_json/data/64dd3e9582b936b0352fdd826ecd3c95/constituents_json.json");
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://datahub.io/collections/stock-market-data");
            Process.Start("https://datahub.io/collections/economic-data");
            Process.Start("https://datahub.io/collections/property-prices");
            Process.Start("https://datahub.io/collections/inflation");
        }
    }
}
