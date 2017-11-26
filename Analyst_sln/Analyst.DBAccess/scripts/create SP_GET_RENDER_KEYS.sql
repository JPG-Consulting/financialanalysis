CREATE PROCEDURE SP_GET_RENDER_KEYS
	@datasetid int
AS
BEGIN
	SELECT s.ADSH + cast(r.Report as varchar) [Key],r.Id
	FROM EdgarDatasetRenders r
		INNER JOIN EdgarDatasetSubmissions s
			on r.SubmissionId = r.SubmissionId
	where 
		r.DatasetId = @datasetid
		and s.DatasetId = @datasetid;
END