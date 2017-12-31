CREATE PROCEDURE dbo.SP_EDGARDATASETTEXT_INSERT
	@LineNumber int
    ,@DatavalueEnddate datetime
    ,@CountOfNumberOfQuarters int
    ,@Iprx smallint
    ,@Language nvarchar(5)
    ,@Dcml int
    ,@Durp real
    ,@Datp real
    ,@DimensionNumber smallint
    ,@CoRegistrant  nvarchar(256)
    ,@Escaped bit
    ,@SourceLength int
    ,@TextLength int
    ,@FootNote nvarchar(512)
    ,@FootLength int
    ,@Context nvarchar(255)
    ,@Value nvarchar(2048)
    ,@Dimension_Id int
    ,@Submission_Id int
    ,@Tag_Id int
	,@DatasetId int
AS
BEGIN

	if(
		not exists(
			/*complete compund key
			5.    TXT is a data set that contains the plain (no HTML) text of each non-numeric XBRL fact.  These fields comprise a unique compound key:
				1)    adsh - EDGAR accession number
				2)    tag – tag used by the filer
				3)    version – if a standard tag, the taxonomy of origin, otherwise equal to adsh
				4)    ddate - period end date
				5)    qtrs - duration in number of quarters
				6)    dimh - dimension hash value
				7)    iprx - a sequential integer used to distinguish otherwise identical facts
			*/
			select 1
			from [dbo].[EdgarDatasetTexts] 
			where 
				[DatasetId] =@DataSetId 
				AND [SubmissionId] =@Submission_Id
				AND [TagId] =@Tag_Id
				AND [DatavalueEnddate]=@DatavalueEnddate
				AND [CountOfNumberOfQuarters]=@CountOfNumberOfQuarters
				AND [DimensionId] =@Dimension_Id
				AND [Iprx]=@Iprx
		)
	) 
	BEGIN
		Begin transaction;
			INSERT INTO [dbo].[EdgarDatasetTexts]
			   ([LineNumber]
			   ,[DatavalueEnddate]
			   ,[CountOfNumberOfQuarters]
			   ,[Iprx]
			   ,[Language]
			   ,[Dcml]
			   ,[Durp]
			   ,[Datp]
			   ,[DimensionNumber]
			   ,[CoRegistrant]
			   ,[Escaped]
			   ,[SourceLength]
			   ,[TextLength]
			   ,[FootNote]
			   ,[FootLength]
			   ,[Context]
			   ,[Value]
			   ,[DimensionId]
			   ,[SubmissionId]
			   ,[TagId]
			   ,[DatasetId])
		 VALUES
			   (@LineNumber 
				,@DatavalueEnddate
				,@CountOfNumberOfQuarters
				,@Iprx
				,@Language
				,@Dcml
				,@Durp
				,@Datp
				,@DimensionNumber
				,@CoRegistrant
				,@Escaped
				,@SourceLength
				,@TextLength
				,@FootNote
				,@FootLength
				,@Context
				,@Value
				,@Dimension_Id
				,@Submission_Id
				,@Tag_Id
				,@DatasetId)
				;
			update DBO.EdgarDatasets SET ProcessedTexts = ProcessedTexts + 1 where id = @DatasetId;
		commit transaction;
	END
END