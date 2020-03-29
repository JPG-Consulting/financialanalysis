using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using log4net;
using FinancialAnalyst.Common.Entities.EdgarSEC;
using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using FinancialAnalyst.Common.Entities.EdgarSEC.Repositories;
using FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.Interfaces.InterfacesForConcreteParsers;

namespace FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.LineByLineProcessStrategy.ConcreteParsers
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



    public class TagDatasetParser : LineByLineEdgarDatasetParser<EdgarDatasetTag>, ITagDatasetParser
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
        public TagDatasetParser()
        {
            log = log4net.LogManager.GetLogger(this.GetType());
        }
        public override EdgarDatasetTag Parse(IEdgarDatasetsRepository repository,List<string> fieldNames, List<string> fields, int linenumber)
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

        public override void Add(IEdgarDatasetsRepository repo, EdgarDataset dataset, EdgarDatasetTag file)
        {
            repo.Add(dataset, file);
        }

        public override IList<EdgarTuple> GetKeys(IEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetTagsKeys(datasetId);
        }
    }
}
