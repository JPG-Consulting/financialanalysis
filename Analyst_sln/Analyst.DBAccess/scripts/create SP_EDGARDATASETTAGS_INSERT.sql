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
IF OBJECT_ID ( 'SP_EDGARDATASETTAGS_INSERT', 'P' ) IS NOT NULL   
    DROP PROCEDURE SP_EDGARDATASETTAGS_INSERT;  
GO
*/  
CREATE PROCEDURE dbo.SP_EDGARDATASETTAGS_INSERT
	@DataSetId int
	,@Tag varchar(256)
	,@Version varchar(20)
    ,@Custom bit
    ,@Abstract bit
    ,@Datatype nvarchar(max)
    ,@Tlabel nvarchar(max)
    ,@Doc nvarchar(max)
	
AS

BEGIN
	declare @tagId int;
	
    BEGIN TRANSACTION;
	INSERT INTO [dbo].[EdgarDatasetTags]
           ([Tag],[Version],[Custom],[Abstract],[Datatype],[Tlabel],[Doc])
     VALUES
           (@tag,@version,@custom,@Abstract,@Datatype,@Tlabel,@doc);
	
	set @tagid = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]);

	exec [dbo].[SP_EDGARDATASETTAGS_RELATE] @DataSetId,@tagId;
	COMMIT TRANSACTION;
END
--GO
