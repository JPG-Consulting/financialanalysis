using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.DataSources.FinancialDataSources.Nasdaq
{
    [JsonObject]
    [Serializable]
    internal class NasdaqResponse
    {
        /*
        {
          "data": null,
          "message": null,
          "status": {
            "rCode": 400,
            "bCodeMessage": [
              {
                "code": 1001,
                "errorMessage": "Symbol not exists."
              }
            ],
            "developerMessage": null
          }
        }
       */

        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("status")]
        public NasdaqResponseStatus Status { get; set; }
    }

    [JsonObject]
    internal class NasdaqResponseStatus
    {
        [JsonProperty("rCode")]
        public int Code { get; set; }

        [JsonProperty("bCodeMessage")]
        public List<NasdaqResponseCodeMessage> CodeMessage { get; set; }

        [JsonProperty("developerMessage")]
        public object DeveloperMessage { get; set; }
    }

    [JsonObject]
    internal class NasdaqResponseCodeMessage
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
