/*
use master
GRANT ADMINISTER BULK OPERATIONS TO analyst_usr
*/

--https://docs.microsoft.com/en-us/sql/relational-databases/import-export/bulk-import-and-export-of-data-sql-server

use analyst
BULK INSERT [Analyst].[dbo].[EdgarDatasetPresentations]
FROM 'E:\_analyst\http_sec_gov--edgar cache\files\dera\data\financial-statement-and-notes-data-sets\2017q1_notes\pre.tsv_bulkinsert_201712171914.tsv'
WITH ( 
	FIRSTROW = 1,--row 1 if there is no header, row 2 if there is a header
    FIELDTERMINATOR = ';',
    --ROWTERMINATOR = '\r\n'
	ROWTERMINATOR = '0x0A', --https://stackoverflow.com/questions/12146915/cannot-fetch-a-row-from-ole-db-provider-bulk-for-linked-server-null
	KEEPNULLS,
	DATAFILETYPE = 'char'
);


--select count(*) from EdgarDatasetPresentations