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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analyst.Windows
{
    public partial class Form1 : Form
    {
        private const int SECONDS_REFRESH = 1;
        private int? datasetIdInProcess;
        private BindingSource bindingSourceDatasets;
        private BindingSource bindingSourceDatasetInProcess;
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private DateTime startTime;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer.Interval = (SECONDS_REFRESH * 1000);
            timer.Tick += Timer_Tick;
            bindingSourceDatasets = new BindingSource();
            bindingSourceDatasetInProcess = new BindingSource();
            dgvDatasets.DataSource = bindingSourceDatasets;
            dgvDatasetInProcess.DataSource = bindingSourceDatasetInProcess;
            LoadDatasets();
            LoadTables();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            using (IEdgarDatasetService edsserv = EdgarDatasetService.CreateOnlyForRetrieval())
            {
                int id = Convert.ToInt32(lblDatasetInProcess.Text);
                EdgarDataset ds = edsserv.GetDataset(id);
                LoadDatasets(ds, bindingSourceDatasetInProcess);
                if (ds.IsComplete || !edsserv.IsRunning(id))
                {
                    timer.Stop();
                    btnLoadDataset.Enabled = true;
                }
            }
            lblTimer.Text = "Timer running - " + (DateTime.Now - startTime).ToString();
        }


        private void btnLoadDataset_Click(object sender, EventArgs e)
        {
            if (dgvDatasets.SelectedRows.Count > 0)
            {
                int id = GetSelectedDatasetId();
                CreateEdgarDatasetService().ProcessDataset(id);
                timer.Start();
                this.startTime = DateTime.Now;
                btnLoadDataset.Enabled = false;
            }
            LoadDatasets();
        }

        private void btnGenerateMissingLines_Click(object sender, EventArgs e)
        {
            int datasetID = GetSelectedDatasetId();
            string table = cboTables.SelectedItem.ToString();
            using (IEdgarDatasetService serv = CreateEdgarDatasetService())
            {
                serv.WriteMissingFiles(datasetID, table);
            }
            MessageBox.Show("Finished");
        }

        private void btnFilterSubmissions_Click(object sender, EventArgs e)
        {
            //string key = "0001163302";//cik = 1163302; --> UNITED STATES STEEL CORP
            //string dataset = "2017q1";
            //string key = "0001564590-17-001812"; //0001564590-17-001812	1379661	TARGA RESOURCES PARTNERS 

            string key = txtKey.Text;
            int datasetId = GetSelectedDatasetId();
            string dataset = (datasetId / 100).ToString() + "q" + (datasetId % 100).ToString();

            string cacheFolder = @"E:\_analyst\http_sec_gov--edgar cache\files\dera\data\financial-statement-and-notes-data-sets";

            string pathSource = string.Format(cacheFolder + @"\{0}_notes", dataset);
            string pathDestination = string.Format(cacheFolder + @"\{0}_notes--copy", dataset);

            ProcessFile(pathSource, pathDestination, "cal", new string[] { key }); //field adsh
            ProcessFile(pathSource, pathDestination, "dim", null);
            ProcessFile(pathSource, pathDestination, "num", new string[] { key }); //field adsh
            ProcessFile(pathSource, pathDestination, "pre", new string[] { key }); //field adsh
            ProcessFile(pathSource, pathDestination, "ren", new string[] { key }); //field adsh
            ProcessFile(pathSource, pathDestination, "sub", new string[] { key }); //field adsh
            ProcessFile(pathSource, pathDestination, "tag", null);
            ProcessFile(pathSource, pathDestination, "txt", new string[] { key });
            MessageBox.Show("Fin ok");
        }

        private IEdgarDatasetService CreateEdgarDatasetService()
        {
            IAnalystRepository repository = new AnalystRepository(new AnalystContext());
            IEdgarDatasetSubmissionsService submissionService = new EdgarDatasetSubmissionsService();
            IEdgarDatasetTagService tagService = new EdgarDatasetTagService();
            IEdgarDatasetNumService numService = new EdgarDatasetNumService();
            IEdgarDatasetDimensionService dimensionService = new EdgarDatasetDimensionService();
            IEdgarDatasetRenderService renderingService = new EdgarDatasetRenderService();
            IEdgarDatasetPresentationService presentationService = new EdgarDatasetPresentationService();
            IEdgarDatasetCalculationService calcService = new EdgarDatasetCalculationService();
            IEdgarDatasetTextService textService = new EdgarDatasetTextService();

            IEdgarDatasetService edgarDatasetService = new EdgarDatasetService(repository, submissionService, tagService, numService, dimensionService, renderingService, presentationService, calcService, textService);
            return edgarDatasetService;
        }

        private void LoadDatasets()
        {
            IList<EdgarDataset> datasets;
            using (IEdgarDatasetService dsServ = EdgarDatasetService.CreateOnlyForRetrieval())
            {
                datasets = dsServ.GetDatasets();
            }
            LoadDatasets(datasets, bindingSourceDatasets);
        }
        private void LoadDatasets(EdgarDataset ds,BindingSource bs)
        {
            IList<EdgarDataset> datasets = new List<EdgarDataset>();
            datasets.Add(ds);
            LoadDatasets(datasets, bindingSourceDatasetInProcess);
        }

        private void LoadDatasets(IList<EdgarDataset> datasets, BindingSource bs)
        {
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
            dt.Columns.Add("Year");
            dt.Columns.Add("Submissions");
            dt.Columns.Add("Tags");
            dt.Columns.Add("Numbers");
            dt.Columns.Add("Dimensions");
            dt.Columns.Add("Renders");
            dt.Columns.Add("Presentations");
            dt.Columns.Add("Calculations");
            dt.Columns.Add("Texts");
            foreach (EdgarDataset ds in datasets)
            {
                DataRow dr = dt.NewRow();
                dr["Id"] = ds.Id;
                dr["Year"] = ds.Year;
                dr["Submissions"] = (ds.TotalSubmissions > 0 ? (float)ds.ProcessedSubmissions / (float)ds.TotalSubmissions : 0.00).ToString("0.0000 %");
                dr["Tags"] = (ds.TotalTags > 0 ? (float)ds.ProcessedTags / (float)ds.TotalTags : 0.00).ToString("0.0000 %");
                dr["Dimensions"] = (ds.TotalDimensions > 0 ? (float)ds.ProcessedDimensions / (float)ds.TotalDimensions : 0.00).ToString("0.0000 %");
                dr["Calculations"] = (ds.TotalCalculations > 0 ? (float)ds.ProcessedCalculations / (float)ds.TotalCalculations : 0.00).ToString("0.0000 %");
                dr["Texts"] = (ds.TotalTexts > 0 ? (float)ds.ProcessedTexts / (float)ds.TotalTexts : 0.00).ToString("0.0000 %");
                dr["Numbers"] = (ds.TotalNumbers > 0 ? (float)ds.ProcessedNumbers / (float)ds.TotalNumbers : 0.00).ToString("0.0000 %");
                dr["Renders"] = (ds.TotalRenders > 0 ? (float)ds.ProcessedRenders / (float)ds.TotalRenders : 0.00).ToString("0.0000 %");
                dr["Presentations"] = (ds.TotalPresentations > 0 ? (float)ds.ProcessedPresentations / (float)ds.TotalPresentations : 0.00).ToString("0.0000 %");
                
                dt.Rows.Add(dr);
            }
            bs.DataSource = null;
            bs.DataSource = dt;
          
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

        private void LoadTables()
        {
            string[] tables = new string[] { "cal", "dim", "num", "pre", "ren", "sub", "tag", "txt" };
            cboTables.DataSource = tables;
        }
        private int GetSelectedDatasetId()
        {
            DataRowView dr = dgvDatasets.SelectedRows[0].DataBoundItem as DataRowView;
            lblDatasetInProcess.Text = dr["Id"].ToString();
            int id = Convert.ToInt32(dr["Id"]);
            return id;
        }

     
    }
}
