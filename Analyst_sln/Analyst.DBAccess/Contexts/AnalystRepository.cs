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
using System.Linq.Expressions;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data;

namespace Analyst.DBAccess.Contexts
{
    public interface IAnalystRepository:IDisposable
    {
        bool ContextConfigurationAutoDetectChangesEnabled { get; set; }

        IList<T> Get<T>() where T:IEdgarEntity;
        IList<T> Get<T>(string include) where T : IEdgarEntity;
        int GetCount<T>() where T : IEdgarEntity;
        int GetDatasetsCount();
        int GetSECFormsCount();
        int GetSICCount();
        EdgarDataset GetDataset(int id);
        Registrant GetRegistrant(string cik);
        SECForm GetSECForm(string code);
        SIC GetSIC(string code);
        EdgarDatasetTag GetTag(string tag, string version);
        
        EdgarDatasetDimension GetDimension(string dimhash);
        
        void Add(SECForm sECForm);
        void Add(SIC sic);
        void Add(Registrant r);
        void Add(EdgarDataset ds);
        void Add(EdgarDataset ds, EdgarDatasetSubmission sub);
        void AddTag(EdgarDataset ds, EdgarDatasetTag tag);
        void AddTagAssociacion(EdgarDataset dataset, EdgarDatasetTag tag);
        void Add(EdgarDataset dataset, EdgarDatasetNumber number);
        void Add(EdgarDataset dataset, EdgarDatasetDimension dim);
        void Add(EdgarDataset dataset,EdgarDatasetRendering ren);
        void Add(EdgarDataset ds, EdgarDatasetPresentation pre);
        void Add(EdgarDataset dataset, EdgarDatasetCalculation file);
        void Add(EdgarDataset dataset, EdgarDatasetText file);
        void UpdateEdgarDataset(EdgarDataset dataset, string v);
        
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
        public int GetDatasetsCount()
        {
            return Context.DataSets.Count();
        }

        public int GetSECFormsCount()
        {
            return Context.SECForms.Count();
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

        public IList<EdgarDatasetDimension> GetDimensions()
        {
            return Context.Dimensions.ToList();
        }

        public EdgarDatasetDimension GetDimension(string dimhash)
        {
            IQueryable<EdgarDatasetDimension> q = Context.Dimensions.Where(x => x.DimensionH == dimhash);
            string sql = q.ToString();
            return q.SingleOrDefault();
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

        public IList<TEntity> Get<TEntity>() where TEntity:IEdgarEntity
        {
            return GetQuery<TEntity>().ToList<TEntity>();
        }
        public int GetCount<TEntity>() where TEntity : IEdgarEntity
        {
            return GetQuery<TEntity>().Count();
        }

        public IList<T> Get<T>(string include) where T : IEdgarEntity
        {
            return GetQuery<T>().Include(include).ToList();
        }

        private ObjectQuery<TEntity> GetQuery<TEntity>() where TEntity : IEdgarEntity
        {
            string key = typeof(TEntity).Name;
            IObjectContextAdapter adapter = (IObjectContextAdapter)Context;
            ObjectContext objectContext = adapter.ObjectContext;
            // 1. we need the container for the conceptual model
            EntityContainer container = objectContext.MetadataWorkspace.GetEntityContainer(
                objectContext.DefaultContainerName, System.Data.Entity.Core.Metadata.Edm.DataSpace.CSpace);
            // 2. we need the name given to the element set in that conceptual model
            string name = container.BaseEntitySets.Where((s) => s.ElementType.Name.Equals(key)).FirstOrDefault().Name;
            // 3. finally, we can create a basic query for this set
            ObjectQuery<TEntity> query = objectContext.CreateQuery<TEntity>("[" + name + "]");
            return query;
        }

        

        #endregion

        #region Add methods
        public void Add(EdgarDataset ds)
        {
            Context.DataSets.Add(ds);
            Context.SaveChanges();
        }

        public void Add(SECForm sf)
        {
            Context.SECForms.Add(sf);
            Context.SaveChanges();
        }

        public void Add(SIC sic)
        {
            Context.SICs.Add(sic);
            Context.SaveChanges();
        }


        public void Add(Registrant r)
        {
            Context.Registrants.Add(r);
            Context.SaveChanges();
        }

        public void Add(EdgarDataset dataset, EdgarDatasetSubmission sub)
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
            SqlParameter lineNumber = new SqlParameter("@LineNumber", sub.LineNumber);
            SqlParameter FormId = new SqlParameter("@Form_id", sub.Form.Id);
            SqlParameter Registrant_Id = new SqlParameter("@Registrant_Id", sub.Registrant.Id);
            SqlParameter EdgarDataset_Id = new SqlParameter("@EdgarDataset_Id", dataset.Id);
            

            Context.Database.ExecuteSqlCommand("exec SP_EDGARDATASSUBMISSIONS_INSERT "+
                "@ADSH, @Period, @Detail, @XBRLInstance, @NumberOfCIKs, @AdditionalCIKs, @PubFloatUSD, @FloatDate, @FloatAxis, @FloatMems,@LineNumber, @Form_Id, @Registrant_Id, @EdgarDataset_Id",
                ADSH, Period, Detail, XBRLInstance, NumberOfCIKs, AdditionalCIKs, PubFloatUSD, FloatDate, FloatAxis, FloatMems,lineNumber, FormId, Registrant_Id, EdgarDataset_Id);
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

        public void Add(EdgarDataset dataset, EdgarDatasetNumber number)
        {
            SqlParameter ddate = new SqlParameter("@DDate", number.DDate);
            SqlParameter countOfNumberOfQuarters = new SqlParameter("@CountOfNumberOfQuarters", number.CountOfNumberOfQuarters);
            SqlParameter UnitOfMeasure = new SqlParameter("@UnitOfMeasure", number.UnitOfMeasure);
            SqlParameter iprx = new SqlParameter("@IPRX", number.IPRX);
            SqlParameter value = new SqlParameter("@Value", number.Value);
            if (!number.Value.HasValue)
                value.Value = DBNull.Value;
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
            SqlParameter lineNumber = new SqlParameter("@LineNumber", number.LineNumber);
            SqlParameter edgarDatasetId = new SqlParameter("@EdgarDataset_Id", dataset.Id);
            Context.Database.ExecuteSqlCommand("exec SP_EDGARDATASETNUMBER_INSERT "+
                "@DDate, @CountOfNumberOfQuarters,@UnitOfMeasure, @IPRX, @Value, @FootNote, @FootLength, @NumberOfDimensions, @CoRegistrant, @durp, @datp, @Decimals, @Dimension_Id, @Submission_Id, @Tag_Id,@LineNumber, @EdgarDataset_Id",
                ddate, countOfNumberOfQuarters,UnitOfMeasure, iprx, value, footNote, footLength, numberOfDimensions, coRegistrant, durp, datp, decimals, dimensionId, submissionId, tagId,lineNumber, edgarDatasetId
                );
            
        }
        
        public void Add(EdgarDataset dataset, EdgarDatasetDimension dim)
        {
            SqlParameter DimensionH = new SqlParameter("@DimensionH",SqlDbType.NVarChar ,EdgarDatasetDimension.LENGHT_FIELD_DIMENSIONH);
            DimensionH.Value = dim.DimensionH;
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

        public void Add(EdgarDataset ds,EdgarDatasetRendering ren)
        {
            SqlParameter Report = new SqlParameter("@Report", ren.Report);
            SqlParameter MenuCategory = new SqlParameter("@MenuCategory", ren.MenuCategory);
            SqlParameter ShortName = new SqlParameter("@ShortName", ren.ShortName);
            SqlParameter LongName = new SqlParameter("@LongName", ren.LongName);
            SqlParameter Roleuri = new SqlParameter("@Roleuri", ren.Roleuri);
            SqlParameter ParentRoleuri = new SqlParameter("@ParentRoleuri", ren.ParentRoleuri);
            SqlParameter ParentReport = new SqlParameter("@ParentReport", ren.ParentReport);
            if (ren.ParentReport == null)
                ParentReport.Value = DBNull.Value;
            SqlParameter UltimateParentReport = new SqlParameter("@UltimateParentReport", ren.UltimateParentReport);
            if (ren.UltimateParentReport == null)
                UltimateParentReport.Value = DBNull.Value;
            SqlParameter Submission_Id = new SqlParameter("@Submission_Id", ren.Submission.Id);
            SqlParameter DataSetId = new SqlParameter("@DataSetId", ds.Id);
            SqlParameter lineNumber = new SqlParameter("@LineNumber", ren.LineNumber);
            Context.Database.ExecuteSqlCommand("exec SP_EDGARDATASETRENDERINGS_INSERT " +
                "@Report, @MenuCategory, @ShortName, @LongName, @Roleuri, @ParentRoleuri, @ParentReport, @UltimateParentReport, @Submission_Id, @DataSetId, @LineNumber",
                Report, MenuCategory, ShortName, LongName, Roleuri, ParentRoleuri, ParentReport, UltimateParentReport, Submission_Id, DataSetId, lineNumber
                );
        }

        public void Add(EdgarDataset ds,EdgarDatasetPresentation pre)
        {
            SqlParameter ReportNumber = new SqlParameter("@ReportNumber", pre.ReportNumber);
            SqlParameter Line = new SqlParameter("@Line", pre.Line);
            SqlParameter FinancialStatement = new SqlParameter("@FinancialStatement", pre.FinancialStatement);
            SqlParameter Inpth = new SqlParameter("@Inpth", pre.Inpth);
            SqlParameter prole = new SqlParameter("@prole", pre.prole);
            SqlParameter PreferredLabel = new SqlParameter("@PreferredLabel", pre.PreferredLabel);
            SqlParameter Negating = new SqlParameter("@Negating", pre.Negating);
            SqlParameter Submission_Id = new SqlParameter("@Submission_Id", pre.Submission.Id);
            SqlParameter Tag_Id = new SqlParameter("@Tag_Id", pre.Tag.Id);
            SqlParameter DataSetId = new SqlParameter("@DataSetId", ds.Id);
            SqlParameter LineNumber = new SqlParameter("@LineNumber", pre.LineNumber);

            Context.Database.ExecuteSqlCommand("exec SP_EDGARDATASETPRESENTATIONS_INSERT " +
                "@ReportNumber, @Line, @FinancialStatement, @Inpth, @prole, @PreferredLabel, @Negating, @Submission_Id, @Tag_Id, @DataSetId, @LineNumber",
                ReportNumber, Line, FinancialStatement, Inpth, prole, PreferredLabel, Negating, Submission_Id, Tag_Id, DataSetId, LineNumber);
        }

        public void Add(EdgarDataset dataset, EdgarDatasetCalculation file)
        {
            SqlParameter LineNumber = new SqlParameter("@LineNumber", file.LineNumber);
            SqlParameter SequentialNumberForGrouping = new SqlParameter("@SequentialNumberForGrouping", file.SequentialNumberForGrouping);
            SqlParameter SequentialNumberForArc = new SqlParameter("@SequentialNumberForArc", file.SequentialNumberForArc);
            SqlParameter Negative = new SqlParameter("@Negative", file.Negative);
            SqlParameter ParentTagId = new SqlParameter("@ParentTagId", file.ParentTag.Id);
            SqlParameter ChildTagId = new SqlParameter("@ChildTagId", file.ChildTag.Id);
            SqlParameter Dataset_Id = new SqlParameter("@Dataset_Id", dataset.Id);
            SqlParameter Submission_Id = new SqlParameter("@Submission_Id", file.Submission.Id);

            Context.Database.ExecuteSqlCommand("exec SP_EDGARDATASETCALC_INSERT " +
                "@LineNumber, @SequentialNumberForGrouping, @SequentialNumberForArc, @Negative, @ParentTagId, @ChildTagId, @Dataset_Id, @Submission_Id",
                LineNumber, SequentialNumberForGrouping, SequentialNumberForArc, Negative, ParentTagId, ChildTagId, Dataset_Id, Submission_Id);
        }

        public void Add(EdgarDataset dataset, EdgarDatasetText file)
        {
            SqlParameter LineNumber = new SqlParameter("@LineNumber", file.LineNumber);
            SqlParameter DDate = new SqlParameter("@DDate", file.DDate);
            SqlParameter Qtrs = new SqlParameter("@Qtrs", file.Qtrs);
            SqlParameter Iprx = new SqlParameter("@Iprx", file.Iprx);
            SqlParameter Language = new SqlParameter("@Language", file.Language);
            SqlParameter Dcml = new SqlParameter("@Dcml", file.Dcml);
            SqlParameter Durp = new SqlParameter("@Durp", file.Durp);
            SqlParameter Datp = new SqlParameter("@Datp", file.Datp);
            SqlParameter DimN = new SqlParameter("@DimN", file.DimN);
            if (!file.DimN.HasValue)
                DimN.Value = DBNull.Value;
            SqlParameter Coreg = new SqlParameter("@Coreg", file.Coreg);
            if (!file.Coreg.HasValue)
                Coreg.Value = DBNull.Value;
            SqlParameter Escaped = new SqlParameter("@Escaped", file.Escaped);
            SqlParameter SrcLen = new SqlParameter("@SrcLen", file.SrcLen);
            SqlParameter TxtLen = new SqlParameter("@TxtLen", file.TxtLen);
            SqlParameter FootNote = new SqlParameter("@FootNote", file.FootNote);
            SqlParameter FootLen = new SqlParameter("@FootLen", file.FootLen);
            if (!file.FootLen.HasValue)
                FootLen.Value = DBNull.Value;
            SqlParameter paramContext = new SqlParameter("@Context", file.Context);
            SqlParameter Value = new SqlParameter("@Value", file.Value);
            SqlParameter Dimension_Id = new SqlParameter("@Dimension_Id", file.Dimension.Id);
            SqlParameter Submission_Id = new SqlParameter("@Submission_Id", file.Submission.Id);
            SqlParameter Tag_Id = new SqlParameter("@Tag_Id", file.Tag.Id);
            SqlParameter DatasetId = new SqlParameter("@DatasetId", dataset.Id);

            Context.Database.ExecuteSqlCommand("exec SP_EDGARDATASETTEXT_INSERT " +
                "@LineNumber, @DDate, @Qtrs, @Iprx, @Language, @Dcml, @Durp, @Datp, @DimN, @Coreg, @Escaped, @SrcLen, @TxtLen, @FootNote, @FootLen, @Context, @Value, @Dimension_Id, @Submission_Id, @Tag_Id, @DatasetId",
                LineNumber, DDate, Qtrs, Iprx, Language, Dcml, Durp, Datp, DimN, Coreg, Escaped, SrcLen, TxtLen, FootNote, FootLen, paramContext, Value, Dimension_Id, Submission_Id, Tag_Id, DatasetId);
       }

        #endregion

        public void Dispose()
        {
            if (this.Context != null)
                this.Context.Dispose();
        }

        public void UpdateEdgarDataset(EdgarDataset dataset, string property)
        {
            Context.Entry<EdgarDataset>(dataset).Property(property).IsModified = true;
            Context.SaveChanges();
        }

       
    }
}
