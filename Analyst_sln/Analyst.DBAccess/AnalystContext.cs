using Analyst.Domain;
using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.DBAccess
{
    public class AnalystContext
    {
        public List<Analyst.Domain.Edgar.Datasets.EdgarDataset> GetDatasets()
        {
            List<EdgarDataset> datasets = new List<EdgarDataset>();
            string genericPath = "/files/dera/data/financial-statement-and-notes-data-sets/{0}q{1}_notes.zip";
            for (int i=2009;i<=2016;i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                    Quarter q = (Quarter)j;

                    EdgarDataset ds = new EdgarDataset
                    {
                        RelativePath = String.Format(genericPath, i.ToString(), j),
                        Year = i,
                        Quarter = q
                    };
                    datasets.Add(ds);
                }
            }
            datasets.Add(new EdgarDataset { RelativePath = "/files/0dera/data/financial-statement-and-notes-data-sets/2017q1_notes.zip", Year = 2017, Quarter = Quarter.QTR1 });
            datasets.Add(new EdgarDataset { RelativePath = "/files/0dera/data/financial-statement-and-notes-data-sets/2017q2_notes.zip", Year = 2017, Quarter = Quarter.QTR2 });
            datasets.Add(new EdgarDataset { RelativePath = "/files/0dera/data/financial-statement-and-notes-data-sets/2017q3_notes.zip", Year = 2017, Quarter = Quarter.QTR3 });
            return datasets;
        }
    }
}
