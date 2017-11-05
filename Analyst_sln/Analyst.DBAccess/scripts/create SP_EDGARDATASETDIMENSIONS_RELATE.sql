/*
USE [Analyst]
GO
*/
/****** Object:  StoredProcedure [dbo].[SP_EDGARDATASETDIMENSIONS_RELATE]*/
/*
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
*/
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
IF OBJECT_ID ( 'SP_EDGARDATASETDIMENSIONS_RELATE', 'P' ) IS NOT NULL   
    DROP PROCEDURE SP_EDGARDATASETDIMENSIONS_RELATE;  
GO
*/  
CREATE PROCEDURE [dbo].[SP_EDGARDATASETDIMENSIONS_RELATE]
	@DataSetId int
	,@DimId int
	
AS

BEGIN
	
    BEGIN TRANSACTION;

		if(
			not exists(
				select 1
				from [dbo].[EdgarDatasetDimensionEdgarDatasets] 
				where [EdgarDatasetDimension_Id] = @DimId and [EdgarDataset_Id] =@DataSetId
			)
		) 
		begin
			INSERT INTO [dbo].[EdgarDatasetDimensionEdgarDatasets]
				   ([EdgarDatasetDimension_Id]
				   ,[EdgarDataset_Id])
			 VALUES
				   (@DimId
				   ,@DataSetId)
			;

			UPDATE DBO.EdgarDatasets 
				SET ProcessedDimensions = ProcessedDimensions + 1 
				WHERE ID= @DataSetId;
		end

	COMMIT TRANSACTION;

END
--GO
