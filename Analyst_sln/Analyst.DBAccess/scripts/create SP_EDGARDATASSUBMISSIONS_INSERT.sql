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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE dbo.SP_EDGARDATASSUBMISSIONS_INSERT
	@ADSH nvarchar(max)
	,@Period datetime
	,@Detail bit
	,@XBRLInstance nvarchar(max)
	,@NumberOfCIKs int
	,@AdditionalCIKs nvarchar(max)
	,@PubFloatUSD real
	,@FloatDate datetime
	,@FloatAxis nvarchar(max)
	,@FloatMems int
	,@Form_Code nvarchar(128)
	,@Registrant_Id int
	,@EdgarDataset_Id int
AS
BEGIN
	Begin transaction;
	INSERT INTO [dbo].[EdgarDatasetSubmissions]
			   ([ADSH]
			   ,[Period]
			   ,[Detail]
			   ,[XBRLInstance]
			   ,[NumberOfCIKs]
			   ,[AdditionalCIKs]
			   ,[PubFloatUSD]
			   ,[FloatDate]
			   ,[FloatAxis]
			   ,[FloatMems]
			   ,[Form_Code]
			   ,[Registrant_Id]
			   ,[EdgarDataset_Id])
		 VALUES
			   (@ADSH
				,@Period
				,@Detail
				,@XBRLInstance
				,@NumberOfCIKs
				,@AdditionalCIKs
				,@PubFloatUSD
				,@FloatDate
				,@FloatAxis
				,@FloatMems
				,@Form_Code
				,@Registrant_Id
				,@EdgarDataset_Id);
		Commit transaction;
END
GO


