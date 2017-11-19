--select * from EdgarDatasets where id=201604;

SELECT  'missing line: ' + str(n),* 
FROM 
	(select top 986 * from Numbers where n>1) n
	left join EdgarDatasetNumbers t on t.LineNumber = n.n 
where t.LineNumber is null;

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
