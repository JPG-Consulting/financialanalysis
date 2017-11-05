-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
/*
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
*/
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
/*
IF OBJECT_ID ( 'SP_EDGARDATASETTAGS_RELATE', 'P' ) IS NOT NULL   
    DROP PROCEDURE SP_EDGARDATASETTAGS_RELATE;  
GO
*/
CREATE PROCEDURE dbo.SP_EDGARDATASETTAGS_RELATE
	@DataSetId int
	,@TagId int
AS
BEGIN
	Begin transaction;
		if(
			not exists(
				select 1
				from [dbo].[EdgarDatasetTagEdgarDatasets] 
				where [EdgarDatasetTag_Id] = @tagId and [EdgarDataset_Id] =@DataSetId
			)
		) 
		BEGIN
			INSERT INTO [dbo].[EdgarDatasetTagEdgarDatasets]
					([EdgarDatasetTag_Id],[EdgarDataset_Id])
				VALUES
					(@tagId,@DataSetId)
					;
				
			UPDATE DBO.EdgarDatasets 
			SET ProcessedTags = ProcessedTags + 1 
			WHERE ID= @DataSetId
			;
		END	
		
	COMMIT TRANSACTION;
END
--GO
