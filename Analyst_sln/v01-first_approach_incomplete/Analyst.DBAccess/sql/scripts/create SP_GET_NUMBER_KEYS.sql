CREATE PROCEDURE SP_GET_NUMBER_KEYS
	@datasetid int
AS
BEGIN

	select s.adsh + ta.tag + ta.version [key],n.Id [id]
	from EdgarDatasetNumbers n
		inner join EdgarDatasetTags ta on n.TagId = ta.Id
		inner join EdgarDatasetSubmissions s on s.Id = n.SubmissionId
	where 
		n.DatasetId = @datasetid
		and ta.DatasetId = @datasetid
		and s.DatasetId = @datasetid
		;
END