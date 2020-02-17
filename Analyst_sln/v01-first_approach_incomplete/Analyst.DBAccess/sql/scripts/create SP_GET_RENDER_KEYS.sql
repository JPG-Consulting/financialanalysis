CREATE PROCEDURE SP_GET_RENDER_KEYS
	@datasetid int
AS
BEGIN
	SELECT s.ADSH + cast(r.Report as nvarchar) [Key],r.Id
	FROM EdgarDatasetRenders r
		INNER JOIN EdgarDatasetSubmissions s
			on r.SubmissionId = s.Id
	where 
		r.DatasetId = @datasetid
		and s.DatasetId = @datasetid;
END