using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Datasets;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace Analyst.DBAccess.Contexts
{
    public interface IAnalystRepository:IDisposable
    {
        bool ContextConfigurationAutoDetectChangesEnabled { get; set; }

        List<EdgarDataset> GetDatasets();
        
        int GetDatasetsCount();
        int GetSECFormsCount();
        List<SECForm> GetSECForms();
        List<SIC> GetSICs();
        int GetSICCount();
        EdgarDataset GetDataset(int id);
        Registrant GetRegistrant(string cik);
        SECForm GetSECForm(string code);
        SIC GetSIC(string code);
        IList<EdgarDatasetSubmissions> GetSubmissions();
        IList<EdgarDatasetDimension> GetDimensions();
        IList<EdgarDatasetTag> GetTags();
        EdgarDatasetTag GetTag(string tag, string version);
        EdgarDatasetDimension GetDimension(string dimhash);
        void AddSECForm(SECForm sECForm);
        void AddSIC(SIC sic);
        void AddRegistrant(Registrant r);
        void AddDataset(EdgarDataset ds);
        void AddSubmission(EdgarDataset ds, EdgarDatasetSubmissions sub);
        void AddTag(EdgarDataset ds, EdgarDatasetTag tag);
        
        void AddTagAssociacion(EdgarDataset dataset, EdgarDatasetTag tag);
        
        void AddNumber(EdgarDataset dataset, EdgarDatasetNumber number);
        void AddDimension(EdgarDataset dataset, EdgarDatasetDimension dim);
        
    }

    public class AnalystRepository: IAnalystRepository
    {

        private AnalystContext Context;
        
        public AnalystRepository(AnalystContext context)
        {
            this.Context = context;
        }

        

        public bool ContextConfigurationAutoDetectChangesEnabled
        {
            get { return Context.Configuration.AutoDetectChangesEnabled; }
            set { Context.Configuration.AutoDetectChangesEnabled = value; }
        }

        #region Get methods
        public List<EdgarDataset> GetDatasets()
        {
            return Context.DataSets.OrderBy(x=> x.Year).ToList();
        }

        public int GetDatasetsCount()
        {
            return Context.DataSets.Count();
        }

        public int GetSECFormsCount()
        {
            return Context.SECForms.Count();
        }

        public List<SECForm> GetSECForms()
        {
            return Context.SECForms.ToList();
        }

        public List<SIC> GetSICs()
        {
            return Context.SICs.ToList();
        }

        public int GetSICCount()
        {
            return Context.SICs.Count();
        }

        
        public EdgarDataset GetDataset(int id)
        {
            EdgarDataset ds = Context.DataSets.Single(x => x.Id == id);
            return ds;
        }

        public Registrant GetRegistrant(string cik)
        {
            int iCik = int.Parse(cik);
            return Context.Registrants.Where(x => x.CIK == iCik).SingleOrDefault();
        }

        public SIC GetSIC(string code)
        {
            int iCode = short.Parse(code);
            return Context.SICs.Where(x => x.Code == iCode).SingleOrDefault();
        }

        public SECForm GetSECForm(string code)
        {
            SECForm form = Context.SECForms.Where(x => x.Code == code).SingleOrDefault();
            if (form == null)
            {
                form = new SECForm() { Code = code };
                Context.SECForms.Add(form);
                Context.SaveChanges();
            }
            return form;
        }
        public IList<EdgarDatasetSubmissions> GetSubmissions()
        {
            return Context.Submissions.ToList();
        }

        public IList<EdgarDatasetDimension> GetDimensions()
        {
            return Context.Dimensions.ToList();
        }


        public IList<EdgarDatasetTag> GetTags()
        {
            return Context.Tags.ToList();
        }


        public EdgarDatasetDimension GetDimension(string dimhash)
        {
            return Context.Dimensions.Where(x => x.DimensionH == dimhash).SingleOrDefault();
        }

        public EdgarDatasetTag GetTag(string tag, string version)
        {
            /*
            string sql = "select * from [dbo].[EdgarDatasetTags] where " +
                "tag = '" + tag + "' COLLATE SQL_Latin1_General_CP1_CS_AS " +
                "AND version = '" + version + "' COLLATE SQL_Latin1_General_CP1_CS_AS ";
            return Context.Tags.SqlQuery(sql).SingleOrDefault();
            */
            DbQuery<EdgarDatasetTag> q = (DbQuery<EdgarDatasetTag>)Context.Tags.Where(t => t.Tag == tag && t.Version == version);
            return q.SingleOrDefault();

        }
        #endregion
        
        #region Add methods
        public void AddDataset(EdgarDataset ds)
        {
            Context.DataSets.Add(ds);
            Context.SaveChanges();
        }

        public void AddSECForm(SECForm sf)
        {
            Context.SECForms.Add(sf);
            Context.SaveChanges();
        }

        public void AddSIC(SIC sic)
        {
            Context.SICs.Add(sic);
            Context.SaveChanges();
        }


        public void AddRegistrant(Registrant r)
        {
            Context.Registrants.Add(r);
            Context.SaveChanges();
        }

        public void AddSubmission(EdgarDataset dataset, EdgarDatasetSubmissions sub)
        {

            SqlParameter ADSH = new SqlParameter("@ADSH", sub.ADSH);
            SqlParameter Period = new SqlParameter("@Period", sub.Period);
            SqlParameter Detail = new SqlParameter("@Detail", sub.Detail);
            SqlParameter XBRLInstance = new SqlParameter("@XBRLInstance", sub.XBRLInstance);
            if (sub.XBRLInstance == null) XBRLInstance.Value = DBNull.Value;
            SqlParameter NumberOfCIKs = new SqlParameter("@NumberOfCIKs", sub.NumberOfCIKs);
            SqlParameter AdditionalCIKs = new SqlParameter("@AdditionalCIKs", sub.AdditionalCIKs);
            if (sub.AdditionalCIKs == null) AdditionalCIKs.Value = DBNull.Value;
            SqlParameter PubFloatUSD = new SqlParameter("@PubFloatUSD", sub.PubFloatUSD);
            if (sub.PubFloatUSD == null) PubFloatUSD.Value = DBNull.Value;
            SqlParameter FloatDate = new SqlParameter("@FloatDate", sub.FloatDate);
            if (sub.FloatDate == null) FloatDate.Value = DBNull.Value;
            SqlParameter FloatAxis = new SqlParameter("@FloatAxis", sub.FloatAxis);
            if (sub.FloatAxis == null) FloatAxis.Value = DBNull.Value;
            SqlParameter FloatMems = new SqlParameter("@FloatMems", sub.FloatMems);
            if (sub.FloatMems == null) FloatMems.Value = DBNull.Value;
            SqlParameter Form_Code = new SqlParameter("@Form_Code", sub.Form.Code);
            SqlParameter Registrant_Id = new SqlParameter("@Registrant_Id", sub.Registrant.Id);
            SqlParameter EdgarDataset_Id = new SqlParameter("@EdgarDataset_Id", dataset.Id);

            Context.Database.ExecuteSqlCommand("exec SP_EDGARDATASSUBMISSIONS_INSERT "+
                "@ADSH, @Period, @Detail, @XBRLInstance, @NumberOfCIKs, @AdditionalCIKs, @PubFloatUSD, @FloatDate, @FloatAxis, @FloatMems, @Form_Code, @Registrant_Id, @EdgarDataset_Id",
                ADSH, Period, Detail, XBRLInstance, NumberOfCIKs, AdditionalCIKs, PubFloatUSD, FloatDate, FloatAxis, FloatMems, Form_Code, Registrant_Id, EdgarDataset_Id);
        }

        public void AddTag(EdgarDataset dataset, EdgarDatasetTag tag)
        {
            SqlParameter dsid = new SqlParameter("@DataSetId",dataset.Id);
            SqlParameter tagparam = new SqlParameter("@Tag",tag.Tag);
            SqlParameter version = new SqlParameter("@Version",tag.Version);

            SqlParameter custom = new SqlParameter("@Custom",tag.Custom); 
            SqlParameter abstracto = new SqlParameter("@Abstract",tag.Abstract);
            SqlParameter datatype = new SqlParameter("@Datatype",tag.Datatype);
            if (tag.Datatype == null) datatype.Value = DBNull.Value;
            SqlParameter tlabel = new SqlParameter("@Tlabel",tag.Tlabel);
            if (tag.Tlabel == null) tlabel.Value = DBNull.Value;
            SqlParameter doc = new SqlParameter("@Doc",tag.Doc);
            if (tag.Doc == null) doc.Value = DBNull.Value;
            Context.Database.ExecuteSqlCommand("exec SP_EDGARDATASETTAGS_INSERT @DataSetId, @tag,@version,@custom,@Abstract,@Datatype,@Tlabel,@doc",dsid, tagparam, version, custom, abstracto, datatype, tlabel, doc);
        }

        public void AddTagAssociacion(EdgarDataset dataset, EdgarDatasetTag tag)
        {
            SqlParameter dsid = new SqlParameter("@DataSetId", dataset.Id);
            SqlParameter tagid = new SqlParameter("@TagId", tag.Id);
            Context.Database.ExecuteSqlCommand("exec SP_EDGARDATASETTAGS_RELATE @DataSetId, @TagId",dsid,tagid);
        }

        public void AddNumber(EdgarDataset dataset, EdgarDatasetNumber number)
        {
            SqlParameter ddate = new SqlParameter("@DDate", number.DDate);
            SqlParameter countOfNumberOfQuarters = new SqlParameter("@CountOfNumberOfQuarters", number.CountOfNumberOfQuarters);
            SqlParameter iprx = new SqlParameter("@IPRX", number.IPRX);
            SqlParameter value = new SqlParameter("@Value", number.Value);
            SqlParameter footNote = new SqlParameter("@FootNote", number.FootNote);
            SqlParameter footLength = new SqlParameter("@FootLength", number.FootLength);
            SqlParameter numberOfDimensions = new SqlParameter("@NumberOfDimensions", number.NumberOfDimensions);
            SqlParameter coRegistrant = new SqlParameter("@CoRegistrant", number.CoRegistrant);
            SqlParameter durp = new SqlParameter("@durp", number.durp);
            SqlParameter datp = new SqlParameter("@datp", number.datp);
            SqlParameter decimals = new SqlParameter("@Decimals", number.Decimals);
            SqlParameter dimensionId = new SqlParameter("@Dimension_Id", number.Dimension.Id);
            SqlParameter submissionId = new SqlParameter("@Submission_Id", number.Submission.Id);
            SqlParameter tagId = new SqlParameter("@Tag_Id", number.Tag.Id);
            SqlParameter edgarDatasetId = new SqlParameter("@EdgarDataset_Id", dataset.Id);
            
            Context.Database.ExecuteSqlCommand("exec SP_EDGARDATASETNUMBER_INSERT "+
                "@DDate, @CountOfNumberOfQuarters, @IPRX, @Value, @FootNote, @FootLength, @NumberOfDimensions, @CoRegistrant, @durp, @datp, @Decimals, @Dimension_Id, @Submission_Id, @Tag_Id, @EdgarDataset_Id",
                ddate, countOfNumberOfQuarters, iprx, value, footNote, footLength, numberOfDimensions, coRegistrant, durp, datp, decimals, dimensionId, submissionId, tagId, edgarDatasetId
                );
            
        }
        
        public void AddDimension(EdgarDataset dataset, EdgarDatasetDimension dim)
        {
            SqlParameter DimensionH = new SqlParameter("@DimensionH", dim.DimensionH);
            SqlParameter Segments = new SqlParameter("@Segments", dim.Segments);
            if (string.IsNullOrEmpty(dim.Segments))
                Segments.Value = DBNull.Value;
            SqlParameter SegmentTruncated = new SqlParameter("@SegmentTruncated", dim.SegmentTruncated);
            SqlParameter DataSetId = new SqlParameter("@DataSetId", dataset.Id);

            Context.Database.ExecuteSqlCommand("exec SP_EDGARDATASETDIMENSIONS_INSERT " +
                "@DimensionH , @Segments, @SegmentTruncated, @DataSetId",
                DimensionH, Segments, SegmentTruncated, DataSetId)
                ;

        }
        #endregion

        public void Dispose()
        {
            if (this.Context != null)
                this.Context.Dispose();
        }
    }
}
