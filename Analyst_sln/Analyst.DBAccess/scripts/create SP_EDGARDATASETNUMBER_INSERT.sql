CREATE PROCEDURE dbo.SP_EDGARDATASETNUMBER_INSERT
	@DDate datetime
    ,@CountOfNumberOfQuarters int
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
	,@EdgarDataset_Id int
AS
BEGIN

	Begin transaction;
		INSERT INTO [dbo].[EdgarDatasetNumbers]
           ([DDate]
           ,[CountOfNumberOfQuarters]
           ,[IPRX]
           ,[Value]
           ,[FootNote]
           ,[FootLength]
           ,[NumberOfDimensions]
           ,[CoRegistrant]
           ,[durp]
           ,[datp]
           ,[Decimals]
           ,[Dimension_Id]
           ,[Submission_Id]
           ,[Tag_Id]
		   ,[EdgarDataset_Id])
		 VALUES
		 (
			@DDate
			,@CountOfNumberOfQuarters
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
			,@EdgarDataset_Id);

		update DBO.EdgarDatasets SET ProcessedNumbers = ProcessedNumbers + 1 where id = @EdgarDataset_Id;
	commit transaction;
END