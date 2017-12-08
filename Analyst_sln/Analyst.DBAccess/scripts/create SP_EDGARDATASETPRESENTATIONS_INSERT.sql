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
IF OBJECT_ID ( 'SP_EDGARDATASETPRESENTATIONS_INSERT', 'P' ) IS NOT NULL   
    DROP PROCEDURE SP_EDGARDATASETPRESENTATIONS_INSERT;  
GO
*/
CREATE PROCEDURE dbo.SP_EDGARDATASETPRESENTATIONS_INSERT
	@ReportNumber int
	,@Line int
	,@FinancialStatement nvarchar(2)
	,@Inpth bit
	,@PreferredLabelXBRLLinkRole nvarchar(50)
	,@PreferredLabel nvarchar(512)
	,@Negating bit
	,@LineNumber int
	,@DataSetId int
	,@Submission_Id int
	,@Tag_Id int
	,@Number_Id int
	,@Text_Id int
	,@Render_Id int
	,@adsh_tag_version nvarchar(300)
AS
BEGIN
	
	if(
		not exists(
			select 1
			from [dbo].[EdgarDatasetPresentations] 
			where 
				[DatasetId] =@DataSetId 
				and reportnumber = @ReportNumber
				and line =@line
				and linenumber = @linenumber
		)
	) 
	BEGIN
		Begin transaction;
			INSERT INTO [dbo].[EdgarDatasetPresentations]
				   ([ReportNumber]
				   ,[Line]
				   ,[FinancialStatement]
				   ,[Inpth]
				   ,[PreferredLabelXBRLLinkRole]
				   ,[PreferredLabel]
				   ,[Negating]
				   ,[LineNumber]
				   ,[DatasetId]
				   ,[SubmissionId]
				   ,[TagId]
				   ,[NumberId]
				   ,[TextId]
				   ,[RenderId]
				   ,[ADSH_Tag_Version]
				   )
			 VALUES
				   (@ReportNumber
					,@Line
					,@FinancialStatement
					,@Inpth
					,@PreferredLabelXBRLLinkRole
					,@PreferredLabel
					,@Negating
					,@LineNumber
					,@DataSetId
					,@Submission_Id
					,@Tag_Id
					,@Number_Id
					,@Text_Id
					,@Render_Id
					,@adsh_tag_version)
			;

			UPDATE DBO.EdgarDatasets 
			SET ProcessedPresentations = ProcessedPresentations + 1 
			WHERE ID= @DataSetId
			;
		COMMIT TRANSACTION;
	END	
		
	
END
--GO
