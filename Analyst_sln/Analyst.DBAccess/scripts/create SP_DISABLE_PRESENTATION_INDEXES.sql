CREATE PROCEDURE SP_DISABLE_PRESENTATION_INDEXES
	@enable bit
AS
BEGIN

	IF @enable = 1
		BEGIN
			ALTER INDEX [IX_DatasetId] ON [dbo].[EdgarDatasetPresentations] REBUILD;  
			ALTER INDEX [IX_NumberId] ON [dbo].[EdgarDatasetPresentations]  REBUILD;  
			ALTER INDEX [IX_RenderId] ON [dbo].[EdgarDatasetPresentations]  REBUILD;  
			ALTER INDEX [IX_SubmissionId] ON [dbo].[EdgarDatasetPresentations]  REBUILD;  
			ALTER INDEX [IX_TagId] ON [dbo].[EdgarDatasetPresentations]  REBUILD;  
			ALTER INDEX [IX_TextId] ON [dbo].[EdgarDatasetPresentations]  REBUILD;  

		END
	ELSE
		BEGIN
			ALTER INDEX [IX_DatasetId] ON [dbo].[EdgarDatasetPresentations]  DISABLE;  
			ALTER INDEX [IX_NumberId] ON [dbo].[EdgarDatasetPresentations]  DISABLE;  
			ALTER INDEX [IX_RenderId] ON [dbo].[EdgarDatasetPresentations]  DISABLE;  
			ALTER INDEX [IX_SubmissionId] ON [dbo].[EdgarDatasetPresentations]  DISABLE;  
			ALTER INDEX [IX_TagId] ON [dbo].[EdgarDatasetPresentations]  DISABLE;  
			ALTER INDEX [IX_TextId] ON [dbo].[EdgarDatasetPresentations]  DISABLE;  
		END;
END