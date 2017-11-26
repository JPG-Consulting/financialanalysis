declare @datasetid int;
declare @table varchar(10);

set @datasetid = 201701;
set @table ='num';

--status in dataset
--select id,totalnumbers,processednumbers,(totalnumbers -processednumbers) totalmissing  from EdgarDatasets where id=@datasetid;

--quantity in table 
--select count(1) quantity_in_table_num from EdgarDatasetNumbers where DatasetId = @datasetid;

/*
--missing lines
--Datasetid 201701 -- num.tsv -- range: 5939781 to 6252401 -- BEGIN
SELECT  
	--'missing line: ' + str(number) missing,* 
	number
	--,LineNumber
	--,id
FROM 
	(select top 7502895 * from Numbers where number>1) n
	left join EdgarDatasetNumbers t on t.LineNumber = n.number
where 1=1
	and t.LineNumber is null
	--and n.number <= 7502895

order by number
;
*/

exec GET_MISSING_LINE_NUMBERS @datasetid,@table;

/*
4.    NUM is a data set of all numeric XBRL facts presented on the primary financial statements. These fields comprise a unique compound key:
1)    adsh - EDGAR accession number
2)    tag - tag used by the filer
3)    version – if a standard tag, the taxonomy of origin, otherwise equal to adsh.
4)    ddate - period end date
5)    qtrs - duration in number of quarters
6)    uom - unit of measure
7)    dimh - 16-byte dimensional qualifier
8)    iprx - a sequential integer used to distinguish otherwise identical facts
*/
