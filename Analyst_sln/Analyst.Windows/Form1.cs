using Analyst.DBAccess.Contexts;
using Analyst.Domain.Edgar.Datasets;
using Analyst.Services.EdgarDatasetServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analyst.Windows
{
    public partial class Form1 : Form
    {

        EdgarDatasetService edgarDatasetService;

        public Form1()
        {
            InitializeComponent();
            InitializeServices();
            
        }

        private void InitializeServices()
        {
            IAnalystRepository repository = new AnalystRepository(new AnalystContext());
            IEdgarDatasetSubmissionsService submissionService = new EdgarDatasetSubmissionsService();
            IEdgarDatasetTagService tagService = new EdgarDatasetTagService();
            IEdgarDatasetNumService numService = new EdgarDatasetNumService();
            IEdgarDatasetDimensionService dimensionService = new EdgarDatasetDimensionService();
            IEdgarDatasetRenderingService renderingService = new EdgarDatasetRenderingService();
            IEdgarDatasetPresentationService presentationService = new EdgarDatasetPresentationService();
            IEdgarDatasetCalculationService calcService = new EdgarDatasetCalculationService();
            IEdgarDatasetTextService textService = new EdgarDatasetTextService();

            edgarDatasetService = new EdgarDatasetService(repository,  submissionService,  tagService,  numService,  dimensionService,  renderingService,  presentationService,  calcService,  textService);
        }


        private void btnMockFiles_Click(object sender, EventArgs e)
        {
            string strCIK = "0001163302";
            //int cik = 1163302;//UNITED STATES STEEL CORP
            //string version = "0001163302-16-000148";
            string pathSource = @"D:\http_sec_gov -- edgar cache\files\dera\data\financial-statement-and-notes-data-sets\2016q4_notes--original";
            string pathDestination = @"D:\http_sec_gov -- edgar cache\files\dera\data\financial-statement-and-notes-data-sets\2016q4_notes";

            ProcessFile(pathSource, pathDestination, "cal", new string[] { strCIK }); //field adsh
            ProcessFile(pathSource, pathDestination, "dim", null);
            ProcessFile(pathSource, pathDestination, "num", new string[] { strCIK }); //field adsh
            ProcessFile(pathSource, pathDestination, "pre", new string[] { strCIK }); //field adsh
            ProcessFile(pathSource, pathDestination, "ren", new string[] { strCIK }); //field adsh
            ProcessFile(pathSource, pathDestination, "sub", new string[] { strCIK }); //field adsh
            //ProcessFile(pathSource, pathDestination, "tag", new string[] { strCIK, "us-gaap/2016", "invest/2013" }); //field version
            ProcessFile(pathSource, pathDestination, "tag", null);
            ProcessFile(pathSource, pathDestination, "txt", new string[] { strCIK });



            MessageBox.Show("Fin ok");
        }

        private void btnLoadDataset_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtDatasetId.Text);
            edgarDatasetService.ProcessDataset(id);
            
        }


        private void ProcessFile(string pathSource, string pathDestination, string filename, string[] codesToFilter)
        {
            string sourceFile = pathSource + "\\" + filename + ".tsv";
            string targetFile = pathDestination + "\\" + filename + ".tsv";
            StreamReader srS = File.OpenText(sourceFile);
            StreamWriter srT = new StreamWriter(targetFile);
            string header = srS.ReadLine();
            srT.WriteLine(header);
            while (!srS.EndOfStream)
            {
                string line = srS.ReadLine();
                if (codesToFilter != null)
                {
                    for (int i = 0; i < codesToFilter.Length; i++)
                    {
                        if (line.Contains(codesToFilter[i]))
                        {
                            srT.WriteLine(line);
                            break;
                        }
                    }
                }
                else
                    srT.WriteLine(line);

            }
            srS.Close();
            srT.Close();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Timer timer = new Timer();
            timer.Interval = (5 * 1000); // 1 secs
            timer.Tick += Timer_Tick;
            timer.Start();
            LoadDatasets(null);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            LoadDatasets(txtDatasetId.Text);
        }

        private void LoadDatasets(string dsID)
        { 
            IList<EdgarDataset> datasets = edgarDatasetService.GetDatasets();
            IList<EdgarDataset> inProcess;
            if (!string.IsNullOrEmpty(dsID))
                inProcess = datasets.Where(ds => ds.Id.ToString() == dsID).ToList();
            else
                inProcess = datasets;

            //dgvDatasets.DataSource = datasets;
            /*
            public int Id 
            public string RelativePath 
            public int Year 
            public Quarter Quarter 
            public int TotalSubmissions 
            public int ProcessedSubmissions 
            public int TotalTags 
            public int ProcessedTags 
            public int TotalNumbers 
            public int ProcessedNumbers 
            public int ProcessedDimensions 
            public int TotalDimensions 
            public int ProcessedRenders 
            public int TotalRenders 
            public int ProcessedPresentations 
            public int TotalPresentations 
            public int ProcessedCalculations 
            public int TotalCalculations 
            public int ProcessedTexts 
            public int TotalTexts 
            */
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Submissions");
            dt.Columns.Add("Tags");
            dt.Columns.Add("Numbers");
            dt.Columns.Add("Dimensions");
            dt.Columns.Add("Renders");
            dt.Columns.Add("Presentations");
            dt.Columns.Add("Calculations");
            dt.Columns.Add("Texts");
            foreach (EdgarDataset ds in inProcess)
            {
                DataRow dr = dt.NewRow();
                dr["Id"] = ds.Id;

                dr["Submissions"] = (ds.TotalSubmissions > 0 ? (float)ds.ProcessedSubmissions / (float)ds.TotalSubmissions:0.00).ToString("0.00 %");
                dr["Tags"] = (ds.TotalTags > 0 ? (float)ds.ProcessedTags / (float)ds.TotalTags : 0.00).ToString("0.00 %");
                dr["Numbers"] = (ds.TotalNumbers > 0 ? (float)ds.ProcessedNumbers / (float)ds.TotalNumbers: 0.00).ToString("0.00 %");
                dr["Dimensions"] = (ds.TotalDimensions > 0 ? (float)ds.ProcessedDimensions / (float)ds.TotalDimensions : 0.00).ToString("0.00 %");
                dr["Renders"] = (ds.TotalRenders > 0 ? (float)ds.ProcessedRenders / (float)ds.TotalRenders : 0.00).ToString("0.00 %");
                dr["Presentations"] = (ds.TotalPresentations > 0 ? (float)ds.ProcessedPresentations / (float)ds.TotalPresentations : 0.00).ToString("0.00 %");
                dr["Calculations"] = (ds.TotalCalculations > 0 ? (float)ds.ProcessedCalculations / (float)ds.TotalCalculations : 0.00).ToString("0.00 %");
                dr["Texts"] = (ds.TotalTexts > 0 ? (float)ds.ProcessedTexts / (float)ds.TotalTexts : 0.00).ToString("0.00 %");
                dt.Rows.Add(dr);
            }
            dgvDatasets.DataSource = dt;
        }
    }
}
