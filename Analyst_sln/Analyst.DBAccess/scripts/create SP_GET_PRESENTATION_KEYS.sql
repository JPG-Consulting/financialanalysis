CREATE PROCEDURE SP_GET_PRESENTATION_KEYS
	@datasetid int
AS
BEGIN
	select S.ADSH + CAST(p.ReportNumber as varchar) + cast(p.Line as varchar) [key], p.Id
	from EdgarDatasetPresentations p
	inner join EdgarDatasetSubmissions s on p.SubmissionId = s.Id
	where p.DatasetId = @datasetid
		and s.DatasetId = @datasetid;
END