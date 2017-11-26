/*
USE [Analyst]
GO
*/
/****** Object:  StoredProcedure [dbo].[SP_EDGARDATASETTAGS_INSERT]    Script Date: 4 nov. 2017 23:51:59 ******/
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
IF OBJECT_ID ( 'SP_EDGARDATASETDIMENSIONS_INSERT', 'P' ) IS NOT NULL   
    DROP PROCEDURE SP_EDGARDATASETDIMENSIONS_INSERT;  
GO
*/
CREATE PROCEDURE [dbo].[SP_EDGARDATASETDIMENSIONS_INSERT]
	@DimensionH nvarchar(34)
	,@Segments nvarchar(1024)
	,@SegmentTruncated bit
	,@DataSetId int
	,@LineNumber int
	
AS

BEGIN
	IF(
		NOT EXISTS(
			SELECT 1
			FROM [EdgarDatasetDimensions]
			WHERE 1=1
				and [Datasetid] = @DataSetId
				and [LineNumber] = @LineNumber
				and [DimensionH] = @DimensionH
		)
	)
	BEGIN
		BEGIN TRANSACTION;

			INSERT INTO [dbo].[EdgarDatasetDimensions]
				   ([DimensionH]
				   ,[Segments]
				   ,[SegmentTruncated]
				   ,[Datasetid]
				   ,[LineNumber])
			 VALUES
				   (@DimensionH
					,@Segments
					,@SegmentTruncated
					,@DataSetId
					,@LineNumber)
				;

			UPDATE DBO.EdgarDatasets 
			SET ProcessedDimensions = ProcessedDimensions + 1 
			WHERE ID= @DataSetId;

		COMMIT TRANSACTION;
	END
END
--GO


