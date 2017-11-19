/*
--opcion A
CREATE TABLE dbo.Numbers 
( 
    Number INT IDENTITY(1,1) PRIMARY KEY CLUSTERED 
) 
 
WHILE COALESCE(SCOPE_IDENTITY(), 0) <= 1024 
BEGIN 
    INSERT dbo.Numbers DEFAULT VALUES 
END
*/

--Opcion B
SELECT TOP (1000000) n = CONVERT(INT, ROW_NUMBER() OVER (ORDER BY s1.[object_id]))
INTO dbo.Numbers
FROM sys.all_objects AS s1 CROSS JOIN sys.all_objects AS s2
OPTION (MAXDOP 1);

 
CREATE UNIQUE CLUSTERED INDEX n ON dbo.Numbers(n)
-- WITH (DATA_COMPRESSION = PAGE)
;

