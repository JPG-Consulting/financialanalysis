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

namespace Analyst.Services.EdgarDatasetServices
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

    public interface IEdgarDatasetTagService : IEdgarDatasetBaseService<EdgarDatasetTag>
    {

    }

    public class EdgarDatasetTagService : EdgarDatasetBaseService<EdgarDatasetTag>, IEdgarDatasetTagService
    {
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
        public override EdgarDatasetTag Parse(IAnalystEdgarDatasetsRepository repository,List<string> fieldNames, List<string> fields, int linenumber)
        {
            /*
            File content:
            tag	version	custom	abstract	datatype	iord	crdr	tlabel	doc
            AccountsPayableCurrent	us-gaap/2015	0	0	monetary	I	C	Accounts Payable, Current	Carrying value as of the balance sheet date of liabilities incurred (and for which invoices have typically been received) and payable to vendors for goods and services received that are used in an entity's business. Used to reflect the current portion of the liabilities (due within one year or within the normal operating cycle if longer).
            AccountsPayableRelatedPartiesCurrent	us-gaap/2015	0	0	monetary	I	C	Accounts Payable, Related Parties, Current	Amount for accounts payable to related parties. Used to reflect the current portion of the liabilities (due within one year or within the normal operating cycle if longer).
            ...
            */
            

            string strTag = fields[fieldNames.IndexOf("tag")];
            string version = fields[fieldNames.IndexOf("version")];

            EdgarDatasetTag tag;
            tag = new EdgarDatasetTag();
            tag.Tag = strTag;
            tag.Version = version;
            string value = fields[fieldNames.IndexOf("custom")];
            tag.Custom = value == "1" ? true : false;
            value = fields[fieldNames.IndexOf("abstract")];
            tag.Abstract = value == "1" ? true : false;
            value = fields[fieldNames.IndexOf("datatype")];
            tag.Datatype = string.IsNullOrEmpty(value) ? null : value;
            value = fields[fieldNames.IndexOf("iord")];
            tag.ValueType = string.IsNullOrEmpty(value) ? (char?)null : value[0];
            value = fields[fieldNames.IndexOf("crdr")];
            tag.NaturalAccountingBalance = string.IsNullOrEmpty(value) ? (char?)null : value[0];
            value = fields[fieldNames.IndexOf("tlabel")];
            tag.LabelText = string.IsNullOrEmpty(value) ? null : value;
            value = fields[fieldNames.IndexOf("doc")];
            tag.Documentation = string.IsNullOrEmpty(value) ? null : value;
            tag.LineNumber = linenumber;
            
            return tag;

        }

        public override void Add(IAnalystEdgarDatasetsRepository repo, EdgarDataset dataset, EdgarDatasetTag file)
        {
            repo.AddTag(dataset, file);
        }

        public override IList<EdgarTuple> GetKeys(IAnalystEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetTagsKeys(datasetId);
        }

        public override string GetKey(List<string> fieldNames, List<string> fields)
        {
            throw new NotImplementedException();
        }

        public override void Parse(List<string> fieldNames, List<string> fields, int lineNumber, DataRow dr, int edgarDatasetId)
        {
            throw new NotImplementedException();
        }

        public override void BulkCopy(SQLAnalystEdgarDatasetsRepository repo, DataTable dt)
        {
            throw new NotImplementedException();
        }

        public override DataTable GetEmptyDataTable(SQLAnalystEdgarDatasetsRepository repo)
        {
            throw new NotImplementedException();
        }

        public override List<int> GetMissingLinesByTable(IAnalystEdgarDatasetsRepository repo, int datasetId, int totalLines)
        {
            return repo.GetMissingLines(datasetId,"EdgarDatasetTags", totalLines);
        }
    }
}
