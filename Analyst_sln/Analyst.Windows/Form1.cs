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

        private void ProcessFile(string pathSource, string pathDestination,string filename, string[] codesToFilter)
        {
            string sourceFile = pathSource + "\\" + filename + ".tsv";
            string targetFile = pathDestination + "\\" + filename + ".tsv";
            StreamReader srS = File.OpenText(sourceFile);
            StreamWriter srT = new StreamWriter(targetFile);
            string header = srS.ReadLine();
            srT.WriteLine(header);
            while(!srS.EndOfStream)
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

        private void btnMockFiles_Click(object sender, EventArgs e)
        {
            string strCIK = "0001163302";
            //int cik = 1163302;//UNITED STATES STEEL CORP
            //string version = "0001163302-16-000148";
            string pathSource = @"D:\http_sec_gov -- edgar cache\files\dera\data\financial-statement-and-notes-data-sets\2016q4_notes--original";
            string pathDestination = @"D:\http_sec_gov -- edgar cache\files\dera\data\financial-statement-and-notes-data-sets\2016q4_notes";

            ProcessFile(pathSource, pathDestination, "dim", null);
            ProcessFile(pathSource, pathDestination, "sub", new string[] { strCIK }); //field adsh
            //ProcessFile(pathSource, pathDestination, "tag", new string[] { strCIK, "us-gaap/2016", "invest/2013" }); //field version
            ProcessFile(pathSource, pathDestination, "tag", null);
            ProcessFile(pathSource, pathDestination, "num", new string[] { strCIK }); //field adsh
            ProcessFile(pathSource, pathDestination, "ren", new string[] { strCIK }); //field adsh
            ProcessFile(pathSource, pathDestination, "pre", new string[] { strCIK }); //field adsh

            MessageBox.Show("Fin ok");
        }
    }
}
