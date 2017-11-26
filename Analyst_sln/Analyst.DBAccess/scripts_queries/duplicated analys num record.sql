--me parece que el poblema esta en el if not exists que no los inserta y no avisa nada
--en el else podria hacer un insert en la tabla de log, llamando a un sp

--status in dataset
select id,totalnumbers,processednumbers,(totalnumbers -processednumbers) totalmissing  from EdgarDatasets where id=201701;

--quantity in table 
select count(1) quantity_in_table_num from EdgarDatasetNumbers where DatasetId = 201701;

--missing lines
--Datasetid 201701 -- num.tsv -- range: 5939781 to 6252401 -- BEGIN
SELECT  'missing line: ' + str(n) missing,* 
FROM 
	(select top 8000000 * from Numbers where n>1) n
	left join EdgarDatasetNumbers t on t.LineNumber = n.n 
where t.LineNumber is null
order by n
;

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
