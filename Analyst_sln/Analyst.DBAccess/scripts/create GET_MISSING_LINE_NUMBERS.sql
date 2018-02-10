CREATE PROCEDURE GET_MISSING_LINE_NUMBERS
	@datasetid int
	,@table varchar(30)
	,@totallines int
AS
BEGIN
	if @table = 'EdgarDatasetCalculations'
		begin
			SELECT  Number
			FROM Numbers n 
			where n.Number <=  @totallines 
				and n.Number > 1 --first line is the header
				and not exists (select ds.LineNumber from EdgarDatasetCalculations ds where ds.DatasetId = @datasetid and ds.LineNumber = n.Number)
			order by Number
		end
	else if @table = 'EdgarDatasetDimensions'
		BEGIN
			SELECT  Number
			FROM Numbers n 
			where n.Number <=  @totallines 
				and n.Number > 1 --first line is the header
				and not exists (select ds.LineNumber from EdgarDatasetDimensions ds where ds.DatasetId = @datasetid and ds.LineNumber = n.Number)
			order by Number
		END
	else if @table = 'EdgarDatasetNumbers'
		BEGIN
			SELECT  Number
			FROM Numbers n 
			where n.Number <=  @totallines 
				and n.Number > 1 --first line is the header
				and not exists (select ds.LineNumber from EdgarDatasetNumbers ds where ds.DatasetId = @datasetid and ds.LineNumber = n.Number)
			order by Number
		end
	else if @table = 'EdgarDatasetPresentations'
		BEGIN
			SELECT  Number
			FROM Numbers n 
			where n.Number <=  @totallines 
				and n.Number > 1 --first line is the header
				and not exists (select ds.LineNumber from EdgarDatasetPresentations ds where ds.DatasetId = @datasetid and ds.LineNumber = n.Number)
			order by Number
		end
	else if @table = 'EdgarDatasetRenders'
		BEGIN
			SELECT  Number
			FROM Numbers n 
			where n.Number <=  @totallines 
				and n.Number > 1 --first line is the header
				and not exists (select ds.LineNumber from EdgarDatasetRenders ds where ds.DatasetId = @datasetid and ds.LineNumber = n.Number)
			order by Number
		end
	else if @table = 'EdgarDatasetSubmissions'
		BEGIN
			SELECT  Number
			FROM Numbers n 
			where n.Number <=  @totallines 
				and n.Number > 1 --first line is the header
				and not exists (select ds.LineNumber from EdgarDatasetSubmissions ds where ds.DatasetId = @datasetid and ds.LineNumber = n.Number)
			order by Number
		end
	else if @table = 'EdgarDatasetTags'
		BEGIN
			SELECT  Number
			FROM Numbers n 
			where n.Number <=  @totallines 
				and n.Number > 1 --first line is the header
				and not exists (select ds.LineNumber from EdgarDatasetTags ds where ds.DatasetId = @datasetid and ds.LineNumber = n.Number)
			order by Number
		end
	else if @table = 'EdgarDatasetTexts'
		BEGIN
			SELECT  Number
			FROM Numbers n 
			where n.Number <=  @totallines 
				and n.Number > 1 --first line is the header
				and not exists (select ds.LineNumber from EdgarDatasetTexts ds where ds.DatasetId = @datasetid and ds.LineNumber = n.Number)
			order by Number
		end
	;
END