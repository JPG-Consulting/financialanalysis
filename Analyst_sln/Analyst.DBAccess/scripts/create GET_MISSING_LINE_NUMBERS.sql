CREATE PROCEDURE GET_MISSING_LINE_NUMBERS
	@datasetid int
	,@table varchar(30)
	,@totallines int
AS
BEGIN

	SELECT  Number
	FROM Numbers n 
	where n.Number <=  @totallines 
		and n.Number > 1 --first line is the header
		and not exists (select edn.LineNumber from EdgarDatasetNumbers edn where edn.DatasetId = @datasetid and edn.LineNumber = n.Number)
		and @table = 'EdgarDatasetNumbers'
	order by Number;
		
	/*
	union
	... lo mismo para otra tabla
	*/
	;
END