using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.Domain.Edgar.Datasets;
using System.IO;
using System.Configuration;
using Analyst.DBAccess.Contexts;
using System.Threading;
using System.Web.Hosting;
using System.Collections.Concurrent;
using log4net;
using Analyst.Domain.Edgar;
using System.Data;
using Analyst.Services.EdgarServices.EdgarDatasetServices.Interfaces;
using Analyst.DBAccess.Repositories;

namespace Analyst.Services.EdgarDatasetServices.BulkProcessStrategy
{
    public static class ListMethods
    {
        public static string[] GetRange(this string[] arr, int from, int to)
        {
            string[] ret = new string[to - from];
            for (int i = from; i < to; i++)
            {
                ret[i - from] = arr[i];
            }
            return ret;
        }
    }

    public class EdgarDatasetTagService : EdgarDatasetBaseService<EdgarDatasetTag>, IEdgarDatasetTagService
    {

        protected override DatasetsTables RelatedTable { get { return DatasetsTables.Tags; } }

        private readonly ILog log;
        protected override ILog Log
        {
            get
            {
                return log;
            }
        }
        public EdgarDatasetTagService()
        {
            log = log4net.LogManager.GetLogger(this.GetType().Name);
        }
        
        public override IList<EdgarTuple> GetKeys(IAnalystEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetTagsKeys(datasetId);
        }
        
        public override void Parse(List<string> fieldNames, List<string> fields, int lineNumber, DataRow dr, int edgarDatasetId)
        {
            string strTag = fields[fieldNames.IndexOf("tag")];
            string version = fields[fieldNames.IndexOf("version")];

            dr["Tag"] = strTag;
            dr["Version"] = version;
            string value = fields[fieldNames.IndexOf("custom")];
            dr["Custom"] = value == "1" ? true : false;
            value = fields[fieldNames.IndexOf("abstract")];
            dr["Abstract"] = value == "1" ? true : false;
            value = fields[fieldNames.IndexOf("datatype")];
            dr["Datatype"] = string.IsNullOrEmpty(value) ? null : value;
            value = fields[fieldNames.IndexOf("iord")];
            dr["ValueTypeStr"] = string.IsNullOrEmpty(value) ? (char?)null : value[0];
            value = fields[fieldNames.IndexOf("crdr")];
            dr["NaturalAccountingBalanceStr"] = string.IsNullOrEmpty(value) ? (char?)null : value[0];
            value = fields[fieldNames.IndexOf("tlabel")];
            dr["LabelText"] = string.IsNullOrEmpty(value) ? null : value;
            value = fields[fieldNames.IndexOf("doc")];
            dr["Documentation"] = string.IsNullOrEmpty(value) ? null : value;
            dr["LineNumber"] = lineNumber;
            dr["DatasetId"] = edgarDatasetId;
        }

    }
}
