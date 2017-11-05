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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strCIK = "0001163302";
            //int cik = 1163302;//UNITED STATES STEEL CORP
            //string version = "0001163302-16-000148";
            string pathSource = @"D:\http_sec_gov -- edgar cache\files\dera\data\financial-statement-and-notes-data-sets\2016q4_notes--original";
            string pathDestination = @"D:\http_sec_gov -- edgar cache\files\dera\data\financial-statement-and-notes-data-sets\2016q4_notes";

            ProcessFile(pathSource, pathDestination,"sub", new string[] { strCIK },"adsh");
            ProcessFile(pathSource, pathDestination, "tag", new string[] { strCIK, "us-gaap/2016" }, "version");
            ProcessFile(pathSource, pathDestination, "num", new string[] { strCIK }, "adsh");
            MessageBox.Show("Fin ok");
        }

        private void ProcessFile(string pathSource, string pathDestination,string filename, string[] codesToFilter,string fieldReturn)
        {
            string sourceFile = pathSource + "\\" + filename + ".tsv";
            string targetFile = pathDestination + "\\" + filename + ".tsv";
            StreamReader srS = File.OpenText(sourceFile);
            StreamWriter srT = new StreamWriter(targetFile);
            string header = srS.ReadLine();
            srT.WriteLine(header);
            List<String> filters = new List<string>(codesToFilter);
            while(!srS.EndOfStream)
            {
                string line = srS.ReadLine();
                for(int i =0;i<codesToFilter.Length;i++)
                {
                    if (line.Contains(codesToFilter[i]))
                    {
                        srT.WriteLine(line);
                        break;
                    }
                }
                    
            }
            srS.Close();
            srT.Close();

        }

        
    }
}
