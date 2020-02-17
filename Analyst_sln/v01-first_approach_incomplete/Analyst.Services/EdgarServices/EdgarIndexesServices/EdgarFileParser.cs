using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.DBAccess.Repositories;
using Analyst.Domain.Edgar.Indexes;

namespace Analyst.Services.EdgarServices.EdgarIndexesServices
{
    public enum TipoSubArchivo
    {
        XBRL,
        XML,
        UUEncode,
        Default
    }

    public interface IEdgarFileParser
    {
        bool CreateSubDocuments(string content, string localPath, List<TipoSubArchivo> tipos, out List<string> files, out List<string> errors);
        List<IndexEntry> ParseMasterIndex(string content);
    }

    public class EdgarFileParser: IEdgarFileParser
    {
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(EdgarMasterIndexService).Name);

        private IAnalystEdgarFilesRepository edgarFileRepository;
        public EdgarFileParser(IAnalystEdgarFilesRepository repo)
        {
            this.edgarFileRepository = repo;
        }

        public bool CreateSubDocuments(string content, string localPath, List<TipoSubArchivo> tipos,out List<string> files,out List<string> errors)
        {
            errors = new List<string>();
            files = new List<string>();
            if (!File.Exists(localPath))
            {
                errors.Add("Archivo [" + localPath + "] no existe");
                return false;
            }
            //StreamReader sr = File.OpenText(localPath);
            byte[] byteArray = Encoding.GetEncoding("windows-1250").GetBytes(content);
            MemoryStream ms = new MemoryStream(byteArray);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (line == "<DOCUMENT>")
                {
                    string type = sr.ReadLine().Replace("<TYPE>", ""); //Ej: <TYPE>EX-32.1
                    string strSequence = sr.ReadLine().Replace("<SEQUENCE>", ""); //Ej: <SEQUENCE>4
                    int iSequence = Convert.ToInt32(strSequence);
                    string fileName = sr.ReadLine().Replace("<FILENAME>", "");//Ej: <FILENAME>thld-ex321_8.htm

                    string description = sr.ReadLine();//<DESCRIPTION>IDEA: XBRL DOCUMENT
                    string subDescription = description.Split(' ')[0];

                    TipoSubArchivo tipo = TipoSubArchivo.Default;
                    if (subDescription == "<DESCRIPTION>XBRL")
                        tipo = TipoSubArchivo.XBRL;
                    else if (fileName.Contains(".xml"))
                        tipo = TipoSubArchivo.XML;
                    else if (type == "ZIP" || type == "EXCEL")
                        tipo = TipoSubArchivo.UUEncode;
                    else
                        tipo = TipoSubArchivo.Default;

                    if (tipos.Contains(tipo))
                    {
                        String newFileName;

                        if (tipo == TipoSubArchivo.XBRL)
                            newFileName = localPath + "__" + iSequence.ToString("000") + "__" + type + "__" + fileName.Replace(".xml", ".xbrl.xml");
                        else
                            newFileName = localPath + "__" + iSequence.ToString("000") + "__" + "__" + type + fileName;

                        switch (tipo)
                        {
                            case TipoSubArchivo.XBRL:
                                {
                                    StreamWriter sw = File.CreateText(newFileName);
                                    line = sr.ReadLine();//<TEXT>
                                    line = sr.ReadLine();//<XBRL>
                                    line = sr.ReadLine();//1era linea del contenido
                                    while (!sr.EndOfStream && line != "</XBRL>")
                                    {
                                        sw.WriteLine(line);
                                        line = sr.ReadLine();
                                    }
                                    line = sr.ReadLine();//</TEXT>
                                    sw.Close();
                                    break;
                                }
                            case TipoSubArchivo.XML:
                                {
                                    StreamWriter sw = File.CreateText(newFileName);
                                    line = sr.ReadLine();//<TEXT>
                                    line = sr.ReadLine();//<XML>
                                    line = sr.ReadLine();//1era linea del contenido
                                    while (!sr.EndOfStream && line != "</XML>")
                                    {
                                        sw.WriteLine(line);
                                        line = sr.ReadLine();
                                    }
                                    line = sr.ReadLine();//</TEXT>
                                    sw.Close();
                                    break;
                                }
                            case TipoSubArchivo.UUEncode:
                                {
                                    //https://en.wikipedia.org/wiki/Uuencoding
                                    /*
                                    File:
                                     File Name = wikipedia-url.txt
                                     File Contents = http://www.wikipedia.org%0D%0A
                            
                                    UUencoding:
                                     begin 644 wikipedia-url.txt
                                     ::'1T<#HO+W=W=RYW:6MI<&5D:6$N;W)G#0H`
                                     `
                                     end
                                    */

                                    line = sr.ReadLine();//<TEXT>
                                    line = sr.ReadLine();//begin ...
                                    line = sr.ReadLine();
                                    /*
                                     * 1er intento, no guarda
                                    StringBuilder content = new StringBuilder();
                                    while (line != "end")
                                    {
                                        if(line != "")
                                            content.Append(line + "\n");
                                        line = sr.ReadLine();
                                    }
                                    //content.Append(line + "\n");//end
                                    line = sr.ReadLine();//</TEXT>

                                    TextWriter tw = File.CreateText(newFileName + "_antesdedecode.txt");
                                    tw.Write(content.ToString());
                                    Stream input = Codecs.GenerateStreamFromString(content.ToString());
                                    Stream output = File.Create(newFileName);
                                    Codecs.UUDecode(input, output);
                                    input.Close();
                                    output.Close();
                                    */

                                    /*
                                     * 2do intento, lo trunca antes
                                    MemoryStream content = new MemoryStream();
                                    while (line != "end")
                                    {
                                        if (line != "")
                                        {
                                            byte[] bytes = Encoding.GetEncoding(1252).GetBytes(line);
                                            content.Write(bytes,0,bytes.Length);
                                            content.Write(new byte[]{(byte)'\n'}, 0, 1);
                                        }
                                        line = sr.ReadLine();
                                    }
                                    //content.Append(line + "\n");//end
                                    line = sr.ReadLine();//</TEXT>

                                    TextWriter tw = File.CreateText(newFileName + "_antesdedecode.txt");
                                    content.Position = 0;
                                    StreamReader srOutput = new StreamReader(content);
                                    tw.Write(srOutput.ReadToEnd());
                                    Stream output = File.Create(newFileName);
                                    content.Position = 0;
                                    Codecs.UUDecode(content, output);
                                    content.Close();
                                    output.Close();
                                    */

                                    //ahora si copia todo el contenido completo, pero no puedo abrir el archivo
                                    StreamWriter sw = new StreamWriter(File.Open(newFileName + "_antesdeldecode.txt", FileMode.Create), Encoding.GetEncoding("windows-1250")); ;
                                    while (line != "end")
                                    {
                                        if (line != "")
                                        {
                                            sw.Write(line + "\n");
                                        }
                                        line = sr.ReadLine();
                                    }
                                    line = sr.ReadLine();//</TEXT>
                                    sw.Close();

                                    FileStream inputStream = File.Open(newFileName + "_antesdeldecode.txt", FileMode.Open);//el archivo tiene que ser ANSI
                                    //FileStream outputStream = File.Create(newFileName);
                                    FileStream outputStream = File.Create(newFileName);
                                    Codecs.UUDecode(inputStream, outputStream);
                                    inputStream.Close();
                                    outputStream.Close();


                                    break;
                                }
                            case TipoSubArchivo.Default:
                                {
                                    StreamWriter sw = File.CreateText(newFileName);
                                    line = sr.ReadLine();//<TEXT>
                                    line = sr.ReadLine();//1era linea del contenido
                                    while (!sr.EndOfStream && line != "</TEXT>")
                                    {
                                        sw.WriteLine(line);
                                        line = sr.ReadLine();
                                    }
                                    sw.Close();
                                    break;
                                }
                        }
                    }
                    files.Add(strSequence + "_" + fileName);
                    line = sr.ReadLine();//</DOCUMENT>
                }
            }
            sr.Close();
            return true;
        }

        public List<IndexEntry> ParseMasterIndex(string content)
        {
            logger.Info("ParseMasterIndex - Init");
            bool isData = false;
            List<string> lines = content.Split('\n').ToList();
            int i = 0;
            logger.Info("ParseMasterIndex - Finding when start the data");
            while (!isData)
            {
                if (lines[i] == "--------------------------------------------------------------------------------")
                    isData = true;
                i++;
            }
            logger.Info($"ParseMasterIndex - Data start at line {i+1}, getting lines to process");
            var linesToProces = lines.GetRange(i, lines.Count - (i + 1));
            logger.Info($"ParseMasterIndex - Start partitioning");
            OrderablePartitioner<Tuple<int, int>> rangePartitioner = Partitioner.Create(0, linesToProces.Count);
            ConcurrentBag<IndexEntry> indexEntries = new ConcurrentBag<IndexEntry>();
            // Loop over the partitions in parallel.
            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                using (IAnalystEdgarFilesRepository repo = new AnalystEdgarRepository())
                {
                    logger.Info($"$ParseMasterIndex - Processing range: from { range.Item1} to {range.Item2}");
                    for (int j = range.Item1; j < range.Item2; j++)
                    {
                        string line = linesToProces[j];
                        IndexEntry entry = ParseMasterIndexLine(line, j,repo);
                        indexEntries.Add(entry);
                    }
                }
            });
            logger.Info($"ParseMasterIndex - Partitioning end");
            logger.Info("ParseMasterIndex - End");
            return indexEntries.ToList();
        }

        private IndexEntry ParseMasterIndexLine(string line,int lineNumber, IAnalystEdgarFilesRepository repo)
        {
            //Example
            //1163302|UNITED STATES STEEL CORP|10-Q|2016-07-27|edgar/data/1163302/0001163302-16-000134.txt
            IndexEntry entry = new IndexEntry();
            //entry.OriginalLine = line;
            string[] fields = line.Split('|');
            entry.CIK = int.Parse(fields[0]);
            entry.Company = repo.GetRegistrant(entry.CIK,fields[1]);
            entry.CompanyName = fields[1];
            entry.FormType = repo.GetSECForm(fields[2]);
            entry.FormTypeId = entry.FormType.Id;
            string format;
            if (fields[3].Length == 10)
            {
                format = "yyyy-MM-dd";
            }
            else if(fields[3].Length == 8)
            {
                format = "yyyyMMdd";
            }
            else
            {
                throw new InvalidFormatException($"Date of line {lineNumber} is not valid: {line}");
            }
            entry.DateFiled = DateTime.ParseExact(fields[3], format, null);
            entry.RelativeURL = fields[4];
            return entry;
        }
    }
}
