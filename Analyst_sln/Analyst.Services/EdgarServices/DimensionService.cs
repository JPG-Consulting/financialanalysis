using System;
using System.Collections.Concurrent;
using Analyst.Domain.Edgar.Datasets;
using Analyst.DBAccess.Contexts;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
                string[] allLines = File.ReadAllLines(filepath);
                string header = allLines[0];
                state.Dataset.TotalDimensions = allLines.Length-1;
                state.DatasetSharedRepo.UpdateEdgarDataset(state.Dataset, "TotalDimensions");

                OrderablePartitioner<Tuple<int, int>> rangePartitioner = Partitioner.Create(1, allLines.Length);
                Parallel.ForEach(rangePartitioner, (range, loopState) =>
                {
                    using (IAnalystRepository partitionRepository = new AnalystRepository(new AnalystContext()))
                    {
                        partitionRepository.ContextConfigurationAutoDetectChangesEnabled = false;
                        try
                        {
                            ProcessRange(state, range, allLines, header, partitionRepository);
                        }
                        finally
                        {
                            partitionRepository.ContextConfigurationAutoDetectChangesEnabled = true;
                        }
                    }
                });
                state.Result = true;
            }
            catch (Exception ex)
            {
                state.Result = false;
                state.Exception = ex;
            }
        }

        private void ProcessRange(EdgarTaskState state, Tuple<int, int> range, string[] allLines, string header, IAnalystRepository partitionRepository)
        {
            for (int i = range.Item1; i < range.Item2; i++)
            {
                string line = allLines[i];
                EdgarDatasetDimension dim = ParseDim(partitionRepository, header, line);
                partitionRepository.AddDimension(state.Dataset, dim);
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