
DECLARE @dbname nvarchar(128)
SET @dbname = N'Analyst'

use [master];
IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = @dbname OR name = @dbname)))
	begin
		PRINT 'Droping Analyst';
		drop database Analyst;
	end
else
	begin
		print 'analyst does not exist';
	end

use [master];
print 'Creating Analyst'
create database Analyst
ON   
	( NAME = Analyst_dat,  
		FILENAME = 'E:\_analyst\DB\Analyst\Analyst_dat.mdf',  
		SIZE = 20000MB,  
		MAXSIZE = UNLIMITED,  
		FILEGROWTH = 2000MB )  
	LOG ON  
	( NAME = Analyst_log,  
		FILENAME = 'E:\_analyst\DB\Analyst\Analyst_log.ldf',  
		SIZE = 1000MB,  
		MAXSIZE = UNLIMITED,  
		FILEGROWTH = 5000MB ) ; 
go

--[]

/*
use [master];
go
drop user analyst_usr
go
create login analyst_usr with password = 'asdf0001';
go
*/

use [Analyst];
go
print 'creating user in DB Analyst'
create user analyst_usr for login analyst_usr;
go

use [Analyst];
print 'assigning db_owner to analyst_usr'
EXEC sp_addrolemember N'db_owner', N'analyst_usr'

print 'end'
-------------------------------------------------------------------------------------------------------------------------------------------------

--delete all tables instead of recraete the entire DB
/*
[dbo].[EdgarDatasetPresentations]
[dbo].[EdgarDatasetRenders]
[dbo].[EdgarDatasetCalculations]
[dbo].[EdgarDatasetTexts]
[dbo].[EdgarDatasetNumbers]
[dbo].[EdgarDatasetTags]
[dbo].[EdgarDatasetDimensions]
[dbo].[EdgarDatasetSubmissions]
[dbo].[EdgarDatasets]
[dbo].[Registrants]
[dbo].[SECForms]
[dbo].[SICs]
[dbo].[Log]
[dbo].[__MigrationHistory]

--faltan todos los SP
*/





