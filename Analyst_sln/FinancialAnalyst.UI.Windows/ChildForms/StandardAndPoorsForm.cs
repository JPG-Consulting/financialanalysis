using FinancialAnalyst.Common.Entities;
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
            //daily
            //https://query1.finance.yahoo.com/v7/finance/download/%5EGSPC?period1=-1325635200&period2=1584144000&interval=1d&events=history

            //monthly
            //https://query1.finance.yahoo.com/v7/finance/download/%5EGSPC?period1=-1325635200&period2=1584144000&interval=1mo&events=history

            string[] lines = File.ReadAllLines(@"HistoricalData\^GSPC.csv");

            chartSP.Series.Clear();
            chartSP.Titles.Add("S&P 500");

            //for volume
            //https://stackoverflow.com/questions/17303378/creating-multiple-charts-and-the-relation-between-chart-series-chartarea

            Series priceSeries = this.chartSP.Series.Add("Prices");
            priceSeries.ChartType = SeriesChartType.Spline;
            for (int i=1;i<lines.Length;i++)
            {
                Price p = Price.From(lines[i]);
                priceSeries.Points.AddXY(p.Date, p.Close);
            }
            


        }
    }
}
