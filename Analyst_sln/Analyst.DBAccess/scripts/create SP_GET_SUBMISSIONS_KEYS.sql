CREATE PROCEDURE SP_GET_SUBMISSIONS_KEYS
	@datasetid int
AS
BEGIN
	select ADSH [key],Id
	from EdgarDatasetSubmissions
	where DatasetId = @datasetid;
END