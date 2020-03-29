CREATE PROCEDURE SP_GET_TAGS_KEYS
	@datasetid int
AS
BEGIN
	select Tag+[version] [key],id
	from EdgarDatasetTags
	where DatasetId = @datasetid;
END