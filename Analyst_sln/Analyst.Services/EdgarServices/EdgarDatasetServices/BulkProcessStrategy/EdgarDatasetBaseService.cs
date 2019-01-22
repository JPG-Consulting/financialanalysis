using Analyst.DBAccess.Contexts;
using Analyst.DBAccess.Repositories;
using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Datasets;
using Analyst.Domain.Edgar.Exceptions;
using Analyst.Services.EdgarServices.EdgarDatasetServices;
using Analyst.Services.EdgarServices.EdgarDatasetServices.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Analyst.Services.EdgarDatasetServices.BulkProcessStrategy
{
    public abstract class EdgarDatasetBaseService<T>: EdgarDatasetTemplateProcessBaseService<T>, IEdgarDatasetBaseService<T> where T : class, IEdgarDatasetFile
    {
        public override void ProcessFile(ConcurrentBag<int> missing,string fileToProcess,string fieldToUpdate, EdgarTaskState state, string[] allLines, string header, string cacheFolder, string tsvFileName, bool processInParallel)
        {
            //https://msdn.microsoft.com/en-us/library/ex21zs8x(v=vs.110).aspx
            //https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/transaction-and-bulk-copy-operations

            Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- BEGIN BULK PROCESS");
            using (IAnalystEdgarDatasetsBulkRepository repo = new AnalystEdgarDatasetsBulkRepository())
            {
                Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- Retrieving structure");
                DataTable dt = GetEmptyDataTable(repo);
                List<string> fieldNames = header.Split('\t').ToList();

                Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- Creating DataTable");
                //first line is the header
                for (int i=1;i<allLines.Length;i++)
                {
                    if (missing == null || missing.Contains(i + 1))
                    {
                        string line = allLines[i];
                        if (!string.IsNullOrEmpty(line))
                        {
                            List<string> fields = line.Split('\t').ToList();
                            DataRow dr = dt.NewRow();
                            Parse(fieldNames, fields, i + 1, dr, state.Dataset.Id);
                            dt.Rows.Add(dr);
                        }
                    }
                }
                Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- Starting bulk copy");
                BulkCopy(repo, dt);
                Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- END BULK PROCESS");
            }
        }

        public abstract void BulkCopy(IAnalystEdgarDatasetsBulkRepository repo, DataTable dt);

        public abstract DataTable GetEmptyDataTable(IAnalystEdgarDatasetsBulkRepository repo);

        public abstract void Parse(List<string> fieldNames, List<string> fields, int lineNumber, DataRow dr, int edgarDatasetId);

    }
}