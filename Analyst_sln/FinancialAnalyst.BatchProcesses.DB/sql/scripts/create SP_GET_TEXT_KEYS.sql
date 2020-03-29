CREATE PROCEDURE SP_GET_TEXT_KEYS
	@datasetid int
AS
BEGIN

	select s.adsh + ta.tag + ta.version [key],te.Id [id]
	from EdgarDatasetTexts te
		inner join EdgarDatasetTags ta on te.TagId = ta.Id
		inner join EdgarDatasetSubmissions s on s.Id = te.SubmissionId
	where 
		te.DatasetId = @datasetid
		and ta.DatasetId = @datasetid
		and s.DatasetId = @datasetid
		;
END

