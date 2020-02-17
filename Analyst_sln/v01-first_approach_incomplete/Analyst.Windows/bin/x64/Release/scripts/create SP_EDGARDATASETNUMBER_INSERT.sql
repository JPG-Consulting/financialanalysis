CREATE PROCEDURE dbo.SP_EDGARDATASETNUMBER_INSERT
	@DDate datetime
    ,@CountOfNumberOfQuarters int
	,@UnitOfMeasure nvarchar(20)
    ,@IPRX smallint
    ,@Value float
    ,@FootNote nvarchar(512)
    ,@FootLength smallint
    ,@NumberOfDimensions smallint
    ,@CoRegistrant nvarchar(256)
    ,@durp real
    ,@datp real
    ,@Decimals int
    ,@Dimension_Id int
    ,@Submission_Id int
    ,@Tag_Id int
	,@LineNumber int
	,@EdgarDataset_Id int
AS
BEGIN

	if(
		not exists(
			/* complete compund key:
			4.    NUM is a data set of all numeric XBRL facts presented on the primary financial statements. These fields comprise a unique compound key:
			1)    adsh - EDGAR accession number
			2)    tag - tag used by the filer
			3)    version – if a standard tag, the taxonomy of origin, otherwise equal to adsh.
			4)    ddate - period end date
			5)    qtrs - duration in number of quarters
			6)    uom - unit of measure
			7)    dimh - 16-byte dimensional qualifier
			8)    iprx - a sequential integer used to distinguish otherwise identical facts
			*/
			select 1
			from [dbo].[EdgarDatasetNumbers] 
			where [DatasetId]=@EdgarDataset_Id
				and [SubmissionId]= @Submission_Id
				and [TagId]=@Tag_Id
				and cast([DDate] as date)=cast(@DDate as date)
				and [CountOfNumberOfQuarters]=@CountOfNumberOfQuarters
				and [UnitOfMeasure]=@UnitOfMeasure
				and [DimensionId] = @Dimension_Id
				and [IPRX] =@IPRX
				and [LineNumber] = @LineNumber
			   
			   
		)
	) 
	begin
		Begin transaction;
			INSERT INTO [dbo].[EdgarDatasetNumbers]
			   ([DDate]
			   ,[CountOfNumberOfQuarters]
			   ,[UnitOfMeasure]
			   ,[IPRX]
			   ,[Value]
			   ,[FootNote]
			   ,[FootLength]
			   ,[NumberOfDimensions]
			   ,[CoRegistrant]
			   ,[durp]
			   ,[datp]
			   ,[Decimals]
			   ,[DimensionId]
			   ,[SubmissionId]
			   ,[TagId]
			   ,[LineNumber]
			   ,[DatasetId])
			 VALUES
			 (
				@DDate
				,@CountOfNumberOfQuarters
				,@UnitOfMeasure
				,@IPRX
				,@Value
				,@FootNote
				,@FootLength
				,@NumberOfDimensions
				,@CoRegistrant
				,@durp
				,@datp
				,@Decimals
				,@Dimension_Id
				,@Submission_Id
				,@Tag_Id
				,@LineNumber
				,@EdgarDataset_Id);

			update DBO.EdgarDatasets SET ProcessedNumbers = ProcessedNumbers + 1 where id = @EdgarDataset_Id;
		commit transaction;
	end
END