using System;
using System.Collections.Concurrent;
using Analyst.Domain.Edgar.Datasets;
using Analyst.DBAccess.Contexts;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Analyst.Services.EdgarServices
{
    public interface IDimensionService
    {
        ConcurrentDictionary<string, EdgarDatasetDimension> GetDimensions();
        void ProcessDimensions(EdgarTaskState edgarTaskState);
    }
    public class DimensionService : IDimensionService
    {
        public ConcurrentDictionary<string, EdgarDatasetDimension> GetDimensions()
        {
            IAnalystRepository repository = new AnalystRepository(new AnalystContext());
            ConcurrentDictionary<string, EdgarDatasetDimension> ret = new ConcurrentDictionary<string, EdgarDatasetDimension>();
            IList<EdgarDatasetDimension> dims = repository.GetDimensions();
            foreach (EdgarDatasetDimension dim in dims)
            {
                ret.TryAdd(dim.DimensionH, dim);
            }
            return ret;
        }

        public void ProcessDimensions(EdgarTaskState state)
        {
            try
            {
                string cacheFolder = ConfigurationManager.AppSettings["cache_folder"];
                string filepath = cacheFolder + state.Dataset.RelativePath.Replace("/", "\\").Replace(".zip", "") + "\\dim.tsv";
                StreamReader sr = File.OpenText(filepath);
                string header = sr.ReadLine();
                using (IAnalystRepository repository = new AnalystRepository(new AnalystContext()))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        EdgarDatasetDimension dim = ParseDim(repository, header, line);
                        repository.AddDimension(state.Dataset, dim);
                    }
                }
                sr.Close();
                state.Result = true;
            }
            catch (Exception ex)
            {
                state.Result = false;
                state.Exception = ex;
            }
        }

        private EdgarDatasetDimension ParseDim(IAnalystRepository repository, string header, string line)
        {
            List<string> fieldNames = header.Split('\t').ToList();
            List<string> fields = line.Split('\t').ToList();
            string dimhash = fields[fieldNames.IndexOf("dimhash")];
            EdgarDatasetDimension dim = repository.GetDimension(dimhash);
            if(dim == null)
            {
                dim = new EdgarDatasetDimension();
                dim.DimensionH = dimhash;
                dim.Segments = fields[fieldNames.IndexOf("segments")];
                dim.SegmentTruncated = !(fields[fieldNames.IndexOf("segt")] == "0");
            }
            return dim;
        }
    }
}