using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FinancialAnalyst.DataSources
{
    public static class ConvertionsExtensions
    {
        internal static readonly CultureInfo EnUs_CultureInfo = new CultureInfo("en-us");

        public static decimal? ToNullableDecimal(this string s)
        {
            if (decimal.TryParse(s, System.Globalization.NumberStyles.Any, EnUs_CultureInfo, out decimal result))
                return result;
            else
                return null;
        }

        public static decimal ToDecimal(this string s, decimal defaultValue)
        {
            if (decimal.TryParse(s, System.Globalization.NumberStyles.Any, EnUs_CultureInfo, out decimal result))
                return result;
            else
                return defaultValue;
        }

        public static double? ToNullableDouble(this string s)
        {
            if (double.TryParse(s, System.Globalization.NumberStyles.Any, EnUs_CultureInfo, out double result))
                return result;
            else
                return null;
        }

        public static double ToDouble(this string s, double defaultValue)
        {
            if (double.TryParse(s, System.Globalization.NumberStyles.Any, EnUs_CultureInfo, out double result))
                return result;
            else
                return defaultValue;
        }

        public static int? ToNullableInt(this string s)
        {
            if (int.TryParse(s, System.Globalization.NumberStyles.Any, EnUs_CultureInfo, out int result))
                return result;
            else
                return null;
        }

        public static DateTime ToDateTime(this string s, string format = "MM/dd/yyyy", DateTime? defaultValue = null)
        {
            if (DateTime.TryParseExact(s, format, ConvertionsExtensions.EnUs_CultureInfo, DateTimeStyles.None, out DateTime result))
                return result;
            else
            {
                if (defaultValue == null)
                    return DateTime.MinValue;
                else
                    return defaultValue.Value;
            }
        }
    }
}
