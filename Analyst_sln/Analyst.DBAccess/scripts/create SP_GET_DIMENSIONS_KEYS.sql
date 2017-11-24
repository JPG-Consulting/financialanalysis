CREATE PROCEDURE SP_GET_DIMENSIONS_KEYS
	@datsetid int
AS
BEGIN
	select DimensionH [key],Id
	from EdgarDatasetDimensions where DatasetId = @datsetid;
END