using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Xml;
using System.Linq;
using System.Globalization;

namespace FinancialAnalyst.DataSources.USTreasury
{
    /// <summary>
    /// Web:
    /// https://www.treasury.gov/resource-center/data-chart-center/interest-rates/pages/TextView.aspx?data=yield
    /// </summary>
    public class USTreasuryDataSource : IRiskFreeRatesDataSource
    {
        
        public bool TryGetRiskFreeRates(out RiskFreeRates riskFreeRate, out string message)
        {
            try
            {
                USTreasuryApiCaller.GetDailyTreasuryYieldCurveRateData(DateTime.Now.Year, DateTime.Now.Month, out string xml, out message);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlElement root = doc.DocumentElement;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("xx", "http://www.w3.org/2005/Atom");
                nsmgr.AddNamespace("d", "http://schemas.microsoft.com/ado/2007/08/dataservices");
                nsmgr.AddNamespace("m", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
                
                //https://stackoverflow.com/questions/3786443/xpath-to-get-the-element-with-the-highest-id
                var properties = root.SelectSingleNode("/xx:feed/xx:entry/xx:content/m:properties[not(/xx:feed/xx:entry/xx:content/m:properties/d:Id > d:Id)]", nsmgr);
                var enus = new CultureInfo("en-US");
                riskFreeRate = new RiskFreeRates();
                riskFreeRate.Date = DateTime.ParseExact(properties.SelectSingleNode("d:NEW_DATE", nsmgr).InnerText, "s", enus);
                riskFreeRate.OneMonth = double.Parse(properties.SelectSingleNode("d:BC_1MONTH", nsmgr).InnerText, enus) / 100;
                riskFreeRate.TwoMonths = double.Parse(properties.SelectSingleNode("d:BC_3MONTH", nsmgr).InnerText, enus) / 100;
                riskFreeRate.ThreeMonths = double.Parse(properties.SelectSingleNode("d:BC_3MONTH", nsmgr).InnerText, enus) / 100;
                riskFreeRate.SixMonths = double.Parse(properties.SelectSingleNode("d:BC_6MONTH", nsmgr).InnerText, enus) / 100;
                riskFreeRate.OneYear = double.Parse(properties.SelectSingleNode("d:BC_1YEAR", nsmgr).InnerText, enus) / 100;
                riskFreeRate.TwoYears = double.Parse(properties.SelectSingleNode("d:BC_2YEAR", nsmgr).InnerText, enus) / 100;
                riskFreeRate.ThreeYears = double.Parse(properties.SelectSingleNode("d:BC_3YEAR", nsmgr).InnerText, enus) / 100;
                riskFreeRate.FiveYears = double.Parse(properties.SelectSingleNode("d:BC_5YEAR", nsmgr).InnerText, enus) / 100;
                riskFreeRate.SevenYears = double.Parse(properties.SelectSingleNode("d:BC_7YEAR", nsmgr).InnerText, enus) / 100;
                riskFreeRate.TenYears = double.Parse(properties.SelectSingleNode("d:BC_10YEAR", nsmgr).InnerText, enus) / 100;
                riskFreeRate.TwentyYears = double.Parse(properties.SelectSingleNode("d:BC_20YEAR", nsmgr).InnerText, enus) / 100;
                riskFreeRate.ThirtyYears = double.Parse(properties.SelectSingleNode("d:BC_30YEAR", nsmgr).InnerText, enus) / 100;
                return true;
            }
            catch(Exception ex)
            {
                message = ex.Message;
                riskFreeRate = null;
                return false;
            }
        }
    }
}
