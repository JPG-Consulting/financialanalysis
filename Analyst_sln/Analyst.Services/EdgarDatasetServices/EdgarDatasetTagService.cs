﻿using System;
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

    public interface IEdgarDatasetTagService : IEdgarFileService<EdgarDatasetTag>
    {

    }

    public class EdgarDatasetTagService : EdgarFileService<EdgarDatasetTag>, IEdgarDatasetTagService
    {
        
        public override EdgarDatasetTag Parse(IAnalystRepository repository,List<string> fieldNames, List<string> fields, int linenumber)
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
            EdgarDatasetTag tag = repository.GetTag(strTag, version);
            if (tag == null)
            {
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
                tag.Iord = string.IsNullOrEmpty(value) ? (char?)null : value[0];
                value = fields[fieldNames.IndexOf("crdr")];
                tag.Crdr = string.IsNullOrEmpty(value) ? (char?)null : value[0];
                value = fields[fieldNames.IndexOf("tlabel")];
                tag.Tlabel = string.IsNullOrEmpty(value) ? null : value;
                value = fields[fieldNames.IndexOf("doc")];
                tag.Doc = string.IsNullOrEmpty(value) ? null : value;
            }
            return tag;

        }

        public override void Add(IAnalystRepository repo, EdgarDataset dataset, EdgarDatasetTag file)
        {
            if (file.Id == 0)
            {
                repo.AddTag(dataset, file);
            }
            else
            {
                repo.AddTagAssociacion(dataset, file);
            }
        }

    }
}
