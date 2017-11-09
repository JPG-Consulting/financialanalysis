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
    public interface IDimensionService:IEdgarFileService<EdgarDatasetDimension>
    {
        void ProcessDimensions(EdgarTaskState edgarTaskState);
    }
    public class DimensionService : EdgarFileService<EdgarDatasetDimension>, IDimensionService
    {

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
                if (false)//for debug purposes
                {
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
                }
                else
                {
                    using (IAnalystRepository partitionRepository = new AnalystRepository(new AnalystContext()))
                    {
                        partitionRepository.ContextConfigurationAutoDetectChangesEnabled = false;
                        try
                        {
                            ProcessRange(state, new Tuple<int, int>(1,allLines.Length), allLines, header, partitionRepository);
                        }
                        finally
                        {
                            partitionRepository.ContextConfigurationAutoDetectChangesEnabled = true;
                        }
                    }
                }
                state.ResultOk = true;
            }
            catch (Exception ex)
            {
                state.ResultOk = false;
                state.Exception = ex;
            }
        }

        private void ProcessRange(EdgarTaskState state, Tuple<int, int> range, string[] allLines, string header, IAnalystRepository partitionRepository)
        {
            for (int i = range.Item1; i < range.Item2; i++)
            {
                string line = allLines[i];
                EdgarDatasetDimension dim = ParseDim(partitionRepository, header, line);
                if(dim.Id == 0)
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