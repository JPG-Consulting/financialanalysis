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
CREATE PROCEDURE dbo.SP_EDGARDATASETTAGS_RELATE
	@DataSetId int
	,@TagId varchar(256)
AS
BEGIN
	
	Begin transaction;
	INSERT INTO [dbo].[EdgarDatasetTagEdgarDatasets]
           ([EdgarDatasetTag_Id],[EdgarDataset_Id])
     VALUES
           (@tagId,@DataSetId)
		   ;
	COMMIT TRANSACTION;
END
GO
