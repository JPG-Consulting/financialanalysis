﻿using Analyst.DBAccess.Contexts;
using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using log4net;
using System.Globalization;
using System.Data;
using Analyst.Services.EdgarServices.EdgarDatasetServices.Interfaces;
using Analyst.DBAccess.Repositories;

namespace Analyst.Services.EdgarDatasetServices.BulkProcessStrategy
{
    public class EdgarDatasetSubmissionsService: EdgarDatasetBaseService<EdgarDatasetSubmission>, IEdgarDatasetSubmissionsService
    {
        private readonly ILog log;
        protected override ILog Log
        {
            get
            {
                return log;
            }
        }
        public EdgarDatasetSubmissionsService()
        {
            log = log4net.LogManager.GetLogger(this.GetType().Name);
        }

        public override IList<EdgarTuple> GetKeys(IAnalystEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetSubmissionKeys(datasetId);
        }

        public override void Parse(List<string> fieldNames, List<string> fields, int lineNumber, DataRow dr, int edgarDatasetId)
        {
            throw new NotImplementedException();
        }

        public override void BulkCopy(IAnalystEdgarDatasetsBulkRepository repo, DataTable dt)
        {
            throw new NotImplementedException();
        }

        public override DataTable GetEmptyDataTable(IAnalystEdgarDatasetsBulkRepository repo)
        {
            throw new NotImplementedException();
        }

        public override List<int> GetMissingLinesByTable(IAnalystEdgarDatasetsRepository repo, int datasetId, int totalLines)
        {
            return repo.GetMissingLines(datasetId,"EdgarDatasetSubmissions", totalLines);
        }
    }
}
