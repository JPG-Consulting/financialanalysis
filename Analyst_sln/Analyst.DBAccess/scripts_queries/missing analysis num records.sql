use Analyst

declare @totallines int = 7502895;
--select top(@totallines) * from Numbers--it works

select TotalNumbers,ProcessedNumbers,TotalNumbers-ProcessedNumbers as Missing from EdgarDatasets where Id =201701

/*
SELECT  'missing line: ' + str(Number),* 
FROM Numbers n 
where n.Number <=  @totallines 
	and n.Number > 1 --first line is the header
	and not exists (select edn.LineNumber from EdgarDatasetNumbers edn where edn.DatasetId = 201701 and edn.LineNumber = n.Number)
order by Number;
*/


------------------------------------------------------------------------------------------------------------
/*
declare @datasetid int = 201701
declare @table varchar(20) = 'EdgarDatasetNumbers'
declare @totallines int = 7502895


SELECT  Number
FROM Numbers n 
where n.Number <=  @totallines 
	and n.Number > 1 --first line is the header
	and not exists (select edn.LineNumber from EdgarDatasetNumbers edn where edn.DatasetId = @datasetid and edn.LineNumber = n.Number)
	and @table = 'EdgarDatasetNumbers'
order by Number
*/
------------------------------------------------------------------------------------------------------------

exec GET_MISSING_LINE_NUMBERS 201701,'EdgarDatasetNumbers',7502895
