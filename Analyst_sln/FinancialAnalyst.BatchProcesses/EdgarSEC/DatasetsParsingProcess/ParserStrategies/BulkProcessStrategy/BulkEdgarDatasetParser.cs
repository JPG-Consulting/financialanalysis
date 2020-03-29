using FinancialAnalyst.BatchProcesses.DB.EdgarSEC.Repositories.BulkRepositories;
using FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.Interfaces;
using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using FinancialAnalyst.Common.Exceptions.EdgarSEC;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.BulkProcessStrategy
{
    public abstract class BulkEdgarDatasetParser<T>: TemplateMethodOfEdgarDatasetParser<T>, IEdgarDatasetParser<T> where T : class, IEdgarDatasetFile
    {
        public override void ProcessFile(ConcurrentBag<int> missing,string fileToProcess,string fieldToUpdate, EdgarTaskState state, string[] allLines, string header, string cacheFolder, string tsvFileName, bool processInParallel)
        {
            //https://msdn.microsoft.com/en-us/library/ex21zs8x(v=vs.110).aspx
            //https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/transaction-and-bulk-copy-operations

            Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- BEGIN BULK PROCESS");
            using (IEdgarDatasetsBulkRepository repo = new EdgarDatasetsBulkRepository())
            {
                Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- Initializing variables");
                DataTable dt = repo.GetEmptyDataTable(RelatedTable);
                List<string> fieldNames = header.Split('\t').ToList();
                ConcurrentDictionary<int, string> failedLines = new ConcurrentDictionary<int, string>();
                List<Exception> exceptions = new List<Exception>();
                int lineNumber = 0;
                string prefixMsg = "Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess;
                Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- Creating DataTable");
                //first line is the header
                for (int i=1;i<allLines.Length;i++)
                {
                    lineNumber = i + 1;//i+1: indexes starts with 0 but header is line 1 and the first row is line 2
                    //It will be processed if:
                    //it's the first time (missing == null) 
                    //or it's processed again and line wasn't processed the firs time (missing.Contains(i+1))
                    if (missing == null || missing.Contains(i + 1))
                    {
                        string line = allLines[i];
                        if (!string.IsNullOrEmpty(line))
                        {
                            try
                            {
                                List<string> fields = line.Split('\t').ToList();
                                DataRow dr = dt.NewRow();
                                Parse(fieldNames, fields, i + 1, dr, state.Dataset.Id);
                                dt.Rows.Add(dr);
                            }
                            catch (Exception ex)
                            {
                                EdgarLineException elex = new EdgarLineException(fileToProcess, lineNumber, ex);
                                exceptions.Add(elex);
                                failedLines.TryAdd(lineNumber, line);
                                Log.Error(prefixMsg + " -- line[" + lineNumber.ToString() + "]: " + line);
                                Log.Error(prefixMsg + " -- line[" + lineNumber.ToString() + "]: " + ex.Message, elex);
                                if (exceptions.Count > MaxErrorsAllowed)
                                {
                                    Log.Fatal(prefixMsg + " -- line[" + i.ToString() + "]: max errors allowed reached", ex);
                                    throw new EdgarDatasetException(fileToProcess, exceptions);
                                }
                            }
                        }
                    }
                }
                Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- Starting bulk copy");
                repo.BulkCopyTable(RelatedTable, dt);
                Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " --End bulk copy, now saving failed lines.");
                state.FileNameToReprocess = WriteFailedLines(cacheFolder, tsvFileName, header, failedLines, allLines.Length);
                Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- END BULK PROCESS");
            }
        }

        public abstract void Parse(List<string> fieldNames, List<string> fields, int lineNumber, DataRow dr, int edgarDatasetId);

    }
}