using FinancialAnalyst.Common.Entities.Assets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Utils
{
    /// <summary>
    /// Thanks to:
    /// https://blog.codeinside.eu/2015/03/30/json-dotnet-deserialize-to-abstract-class-or-interface/
    /// https://github.com/Code-Inside/Samples/blob/master/2015/JsonConvertIssuesWithBaseClasses/JsonConvertIssuesWithBaseClasses/Program.cs
    /// </summary>
    public class AssetsJsonConverter:JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(AssetBase));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            AssetClass assetClass = (AssetClass)jo["assetClass"].Value<int>();
                
            switch(assetClass)
            {
                case AssetClass.Stock:
                    return jo.ToObject<Stock>(serializer);
                case AssetClass.ETF:
                    return jo.ToObject<ETF>(serializer);
                case AssetClass.Option:
                    return jo.ToObject<Option>(serializer);
                case AssetClass.Cash:
                    return jo.ToObject<Cash>(serializer);
                case AssetClass.Bond:
                    return jo.ToObject<Bond>(serializer);
                default:
                    throw new NotImplementedException($"There is no convertion for asset class='{assetClass.ToString()}'");
            }

            
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
