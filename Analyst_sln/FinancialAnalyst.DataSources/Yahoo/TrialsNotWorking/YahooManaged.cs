using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.DataSources.Yahoo
{
    /// <summary>
    /// https://www.codeproject.com/Articles/42575/Yahoo-Managed
    /// https://code.google.com/archive/p/yahoo-finance-managed/downloads
    /// https://github.com/benmcevoy/FatDividends
    /// </summary>
    internal class YahooManaged
    {
        /*
         * https://www.codeproject.com/Articles/42575/Yahoo-Managed
         * no funciona
        QuotesDownload dl = new QuotesDownload();
        DownloadClient<QuotesResult> baseDl = dl;

        QuotesDownloadSettings settings = dl.Settings;
        settings.IDs = new string[] { "MSFT", "GOOG", "YHOO" };
        settings.Properties = new QuoteProperty[] { QuoteProperty.Symbol,
                                        QuoteProperty.Name,
                                        QuoteProperty.LastTradePriceOnly
                                        };
        SettingsBase baseSettings = baseDl.Settings;
        Response<QuotesResult> resp = baseDl.Download();
        */

        /*
         * https://code.google.com/archive/p/yahoo-finance-managed/
         * tampoco funciona
         */

        /*
         * https://github.com/benmcevoy/FatDividends
         * tampoco
         */
    }
}
