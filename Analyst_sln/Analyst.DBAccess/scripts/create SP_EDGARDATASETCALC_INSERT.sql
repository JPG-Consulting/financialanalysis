CREATE PROCEDURE [dbo].[SP_EDGARDATASETCALC_INSERT]
	@LineNumber int
    ,@SequentialNumberForGrouping smallint
    ,@SequentialNumberForArc smallint
    ,@Negative bit
    ,@ParentTagId int
    ,@ChildTagId int
    ,@Dataset_Id int
    ,@Submission_Id int
AS
BEGIN
	if(
		not exists(
			select 1
			from [dbo].[EdgarDatasetCalculations] 
			where 1=1
				AND [SequentialNumberForGrouping] = @SequentialNumberForGrouping
				AND [SequentialNumberForArc] = @SequentialNumberForArc
				AND [Submission_Id] = @Submission_Id
				AND [ParentTagId] = @ParentTagId
				AND [ChildTagId] = @ChildTagId
				AND [Dataset_Id] = @Dataset_Id
		)
	) 
	begin

		BEGIN TRANSACTION;
			INSERT INTO [dbo].[EdgarDatasetCalculations]
				([LineNumber]
				,[SequentialNumberForGrouping]
				,[SequentialNumberForArc]
				,[Negative]
				,[ParentTagId]
				,[ChildTagId]
				,[Dataset_Id]
				,[Submission_Id])
			VALUES
				(@LineNumber
				,@SequentialNumberForGrouping
				,@SequentialNumberForArc 
				,@Negative
				,@ParentTagId
				,@ChildTagId
				,@Dataset_Id
				,@Submission_Id);


			update DBO.EdgarDatasets SET ProcessedCalculations = ProcessedCalculations + 1 where id = @Dataset_Id;
		COMMIT TRANSACTION;
	END
END
