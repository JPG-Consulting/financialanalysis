CREATE PROCEDURE dbo.SP_EDGARDATASETTEXT_INSERT
	@LineNumber int
    ,@DDate datetime
    ,@Qtrs int
    ,@Iprx smallint
    ,@Language nvarchar(5)
    ,@Dcml int
    ,@Durp real
    ,@Datp real
    ,@DimN smallint
    ,@Coreg int
    ,@Escaped bit
    ,@SrcLen int
    ,@TxtLen int
    ,@FootNote nvarchar(512)
    ,@FootLen int
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
			select 1
			from [dbo].[EdgarDatasetTexts] 
			where 
				[Dataset_Id] =@DataSetId 
				AND [Dimension_Id] =@Dimension_Id
				AND [Submission_Id] =@Submission_Id
				AND [Tag_Id] =@Tag_Id
		)
	) 
	BEGIN
		Begin transaction;
			INSERT INTO [dbo].[EdgarDatasetTexts]
			   ([LineNumber]
			   ,[DDate]
			   ,[Qtrs]
			   ,[Iprx]
			   ,[Language]
			   ,[Dcml]
			   ,[Durp]
			   ,[Datp]
			   ,[DimN]
			   ,[Coreg]
			   ,[Escaped]
			   ,[SrcLen]
			   ,[TxtLen]
			   ,[FootNote]
			   ,[FootLen]
			   ,[Context]
			   ,[Value]
			   ,[Dimension_Id]
			   ,[Submission_Id]
			   ,[Tag_Id]
			   ,[Dataset_Id])
		 VALUES
			   (@LineNumber 
				,@DDate
				,@Qtrs
				,@Iprx
				,@Language
				,@Dcml
				,@Durp
				,@Datp
				,@DimN
				,@Coreg
				,@Escaped
				,@SrcLen
				,@TxtLen
				,@FootNote
				,@FootLen
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