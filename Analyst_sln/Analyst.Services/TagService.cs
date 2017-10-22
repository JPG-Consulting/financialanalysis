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

namespace Analyst.Services
{
    public static class ListMethods
    {
        public static string[] GetRange(this string[] arr,int from, int to)
        {
            string[] ret = new string[to - from];
            for(int i=from; i<to;i++)
            {
                ret[i - from] = arr[i];
            }
            return ret;
        }
    }

    public class TagService
    {

        private int MAX_NUMBER_OF_PARTITIONS = 5;

        

        public void ProcessTags(EdgarDataset ds)
        {
            string cacheFolder = ConfigurationManager.AppSettings["cache_folder"];
            string filepath = cacheFolder + ds.RelativePath.Replace("/", "\\").Replace(".zip", "") + "\\tag.tsv";
            /*
            StreamReader sr = File.OpenText(filepath);
            string header = sr.ReadLine();//header
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                //EdgarDatasetTag tag = ParseTag(header, line);
                //Repository.Save(ds, tag);
            }
            sr.Close();
            */

            //Deberia empezar por aca --> ya hay clases para particionar
            //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/custom-partitioners-for-plinq-and-tpl?view=netframework-4.5.2

            string[] allLines = File.ReadAllLines(filepath);
            string header = allLines[0];
            int partitions = (allLines.Count() - 1) / MAX_NUMBER_OF_PARTITIONS;
            for (int i=0;i< MAX_NUMBER_OF_PARTITIONS; i++)
            {
                string[] lines = allLines.GetRange(i * partitions, (i + 1) * partitions);//inclusive,exclusive

                AsyncTagService serv = new AsyncTagService(ds, header, lines);

                //Opcion 1
                //https://blogs.msdn.microsoft.com/webdev/2014/06/04/queuebackgroundworkitem-to-reliably-schedule-and-run-background-processes-in-asp-net/
                //HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => serv.ParseTags(cancellationToken));

                //opcion 2
                //https://docs.microsoft.com/en-us/dotnet/standard/threading/the-managed-thread-pool
                //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/index
                //https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent?view=netframework-4.5.2
                //y desde la vista uso la opcion 1


                //opcion 3
                //WebBackgrounder
                //http://www.hanselman.com/blog/HowToRunBackgroundTasksInASPNET.aspx


                //Opcion 4
                //framework hangfire, version gratuita
                //https://www.hangfire.io/downloads.html


            }
            if ((allLines.Count() - 1) % MAX_NUMBER_OF_PARTITIONS > 0)
            {
                string[] lines = allLines.GetRange(partitions * MAX_NUMBER_OF_PARTITIONS, allLines.Count()-1);
            }

        }

        
        /// <summary>
        /// Ejemplo: https://github.com/NuGet/WebBackgrounder/blob/master/src/WebBackgrounder.EntityFramework/WorkItemCleanupJob.cs
        /// Usa Entity Framework!!!
        /// </summary>
        internal class AsyncTagService //: Job
        {
            //http://www.hanselman.com/blog/HowToRunBackgroundTasksInASPNET.aspx
            //https://github.com/NuGet/WebBackgrounder

            public IAnalystRepository Repository { get; set; }

            private EdgarDataset ds;
            private string header;
            private string[] lines;
            public AsyncTagService(EdgarDataset ds,string header,string[] lines)
            {
                this.ds = ds;
                this.header = header;
                this.lines = lines;
            }

            //private async Task ParseTags(string header, List<string> lines)
            internal void ParseTags()
            {
                for(int i =0;i<lines.Length;i++)
                {
                    EdgarDatasetTag tag = ParseTag(header, lines[i]);
                    Repository.Save(ds, tag);
                }
            }

            internal void ParseTags(CancellationToken cancellationToken)
            {
                ParseTags();
            }

            private EdgarDatasetTag ParseTag(string header, string line)
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
                EdgarDatasetTag tag = Repository.GetTag(strTag, version);
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

            
        }
    }
}
