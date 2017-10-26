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

namespace Analyst.Services
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

    public interface ITagService
    {
        void ProcessTags(EdgarTaskState ds);
    }

    public class TagService : ITagService
    {
        public static bool PROCESS_IN_PARALLEL = false;

        /*
        private IAnalystRepository repository;
        public TagService(IAnalystRepository repository)
        {
            this.repository = repository;
        }
        */

        public void ProcessTags(EdgarTaskState state)
        {
            try
            {
                ConcurrentBag<EdgarDatasetTag> tags = SaveTags(state);
                RelateTagsToDataset(tags,state);
            }
            catch (Exception ex)
            {
                state.Exception = ex;
            }
        }

        private void RelateTagsToDataset(ConcurrentBag<EdgarDatasetTag> tags,EdgarTaskState state)
        {
            IAnalystRepository repository = new AnalystRepository(new AnalystContext());
            foreach (EdgarDatasetTag tag in tags)
            {
                repository.Save(state.Dataset, tag);
            }
        }

        private ConcurrentBag<EdgarDatasetTag> SaveTags(EdgarTaskState state)
        {
            string cacheFolder = ConfigurationManager.AppSettings["cache_folder"];
            string filepath = cacheFolder + state.Dataset.RelativePath.Replace("/", "\\").Replace(".zip", "") + "\\tag.tsv";
            string[] allLines = File.ReadAllLines(filepath);
            ConcurrentBag<EdgarDatasetTag> tags = new ConcurrentBag<EdgarDatasetTag>();
            string header = allLines[0];
            if (PROCESS_IN_PARALLEL)
            {
                //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/custom-partitioners-for-plinq-and-tpl?view=netframework-4.5.2

                // Partition the entire source array.
                OrderablePartitioner<Tuple<int, int>> rangePartitioner = Partitioner.Create(1, allLines.Length);

                // Loop over the partitions in parallel.
                Parallel.ForEach(rangePartitioner, (range, loopState) =>
                {
                    /*
                    //EF no es thread safe y no permite parallel
                    //https://stackoverflow.com/questions/12827599/parallel-doesnt-work-with-entity-framework
                    //https://stackoverflow.com/questions/9099359/entity-framework-and-multi-threading
                    //https://social.msdn.microsoft.com/Forums/en-US/e5cb847c-1d77-4cd0-abb7-b61890d99fae/multithreading-and-the-entity-framework?forum=adodotnetentityframework

                    1era opcion: 1 solo contexto para toda la particion
                    */

                    IAnalystRepository partitionRepository = new AnalystRepository(new AnalystContext());

                    // Loop over each range element without a delegate invocation.
                    ProcessRange(range, allLines, header, partitionRepository, tags);
                });
            }
            else
            {
                ProcessRange(new Tuple<int, int>(1, allLines.Length), allLines, header, new AnalystRepository(new AnalystContext()), tags);
            }
            return tags;
        }

        private void ProcessRange(Tuple<int, int> range,string[] allLines,string header, IAnalystRepository repo, ConcurrentBag<EdgarDatasetTag> tags)
        {
            for (int i = range.Item1; i < range.Item2; i++)
            {
                string line = allLines[i];
                bool? isNew = false;
                EdgarDatasetTag tag = ParseTag(repo, header, line,out isNew);
                if (isNew.HasValue && isNew.Value)
                {
                    repo.Save(tag);
                }
                tags.Add(tag);
            }
        }

        private EdgarDatasetTag ParseTag(IAnalystRepository repository,string header, string line,out bool? isNew)
        {
            /*
            tag	version	custom	abstract	datatype	iord	crdr	tlabel	doc
            AccountsPayableCurrent	us-gaap/2015	0	0	monetary	I	C	Accounts Payable, Current	Carrying value as of the balance sheet date of liabilities incurred (and for which invoices have typically been received) and payable to vendors for goods and services received that are used in an entity's business. Used to reflect the current portion of the liabilities (due within one year or within the normal operating cycle if longer).
            AccountsPayableRelatedPartiesCurrent	us-gaap/2015	0	0	monetary	I	C	Accounts Payable, Related Parties, Current	Amount for accounts payable to related parties. Used to reflect the current portion of the liabilities (due within one year or within the normal operating cycle if longer).
            */
            List<string> fieldNames = header.Split('\t').ToList();
            List<string> fields = line.Split('\t').ToList();

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
                isNew = true;
            }
            else
            {
                isNew = false;
            }
            return tag;

        }



    }
}
