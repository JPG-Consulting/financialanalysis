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
IF OBJECT_ID ( 'SP_EDGARDATASETRENDERS_INSERT', 'P' ) IS NOT NULL   
    DROP PROCEDURE SP_EDGARDATASETRENDERS_INSERT;  
GO
*/
CREATE PROCEDURE dbo.SP_EDGARDATASETRENDERS_INSERT
	@Report int
	,@RenderFile nvarchar(1)
	,@MenuCategory nvarchar(20)
	,@ShortName nvarchar(255)
	,@LongName nvarchar(max)
	,@Roleuri nvarchar(255)
	,@ParentRoleuri nvarchar(255)
	,@ParentReport int
	,@UltimateParentReport int
	,@Submission_Id int
	,@DataSetId int
	,@LineNumber int
AS
BEGIN
	
	if(
		not exists(
			select 1
			from [dbo].[EdgarDatasetRenders] 
			where 
				[DatasetId] =@DataSetId 
				and report = @report
				and linenumber = @linenumber
		)
	) 
	BEGIN
		Begin transaction;
			INSERT INTO [dbo].[EdgarDatasetRenders]
				   ([Report]
				   ,[RenderFileStr]
				   ,[MenuCategory]
				   ,[ShortName]
				   ,[LongName]
				   ,[Roleuri]
				   ,[ParentRoleuri]
				   ,[ParentReport]
				   ,[UltimateParentReport]
				   ,[SubmissionId]
				   ,[DatasetId]
				   ,[LineNumber])
			 VALUES
				   (
					@Report
					,@RenderFile
					,@MenuCategory
					,@ShortName
					,@LongName
					,@Roleuri
					,@ParentRoleuri
					,@ParentReport
					,@UltimateParentReport
					,@Submission_Id
					,@DataSetId
					,@LineNumber
				   )
			;

			UPDATE DBO.EdgarDatasets 
			SET ProcessedRenders = ProcessedRenders + 1 
			WHERE ID= @DataSetId
			;
		COMMIT TRANSACTION;
	END	
END
--GO
