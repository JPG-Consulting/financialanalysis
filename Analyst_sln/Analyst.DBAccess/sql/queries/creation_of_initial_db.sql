
/*
use [master];
go
drop user analyst_usr
go
create login analyst_usr with password = 'asdf0001';
go
*/

-----------------------------------------------------------------------------------------------------------------------------
--DB: Analyst_EdgarDatasets

DECLARE @dbname nvarchar(128)
SET @dbname = N'Analyst_EdgarDatasets'

use [master];



IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = @dbname OR name = @dbname)))
	begin
		PRINT 'Droping Analyst_EdgarDatasets';
		drop database Analyst_EdgarDatasets;
	end
else
	begin
		print 'Analyst_EdgarDatasets does not exist';
	end

use [master];
print 'Creating Analyst_EdgarDatasets'
create database Analyst_EdgarDatasets
ON   
	( NAME = Analyst_EdgarDatasets_dat,  
		FILENAME = 'C:\_analyst\DB\Analyst\dev\Analyst_EdgarDatasets_dat.mdf',  
		SIZE = 20000MB,  
		MAXSIZE = UNLIMITED,  
		FILEGROWTH = 2000MB )  
	LOG ON  
	( NAME = Analyst_EdgarDatasets_log,  
		FILENAME = 'C:\_analyst\DB\Analyst\dev\Analyst_EdgarDatasets_log.ldf',  
		SIZE = 1000MB,  
		MAXSIZE = UNLIMITED,  
		FILEGROWTH = 5000MB ) ; 
go


use [Analyst_EdgarDatasets];
go
print 'creating user in DB Analyst_EdgarDatasets'
create user analyst_usr for login analyst_usr;
go

use [Analyst_EdgarDatasets];
print 'assigning db_owner to analyst_usr'
EXEC sp_addrolemember N'db_owner', N'analyst_usr'

print 'end'

-----------------------------------------------------
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
-----------------------------------------------------

-----------------------------------------------------------------------------------------------------------------------------
--DB: Analyst_EdgarFiles

DECLARE @dbname nvarchar(128)
SET @dbname = N'Analyst_EdgarFiles'

use [master];

--create login analyst_usr with password = 'asdf0001';

IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = @dbname OR name = @dbname)))
	begin
		PRINT 'Droping Analyst_EdgarFiles';
		drop database Analyst_EdgarFiles;
	end
else
	begin
		print 'Analyst_EdgarFiles does not exist';
	end

use [master];
print 'Creating Analyst_EdgarFiles'
create database Analyst_EdgarFiles
ON   
	( NAME = Analyst_EdgarFiles_dat,  
		FILENAME = 'C:\_analyst\DB\Analyst\dev\Analyst_EdgarFiles_dat.mdf',  
		SIZE = 1000MB,  
		MAXSIZE = UNLIMITED,  
		FILEGROWTH = 2000MB )  
	LOG ON  
	( NAME = Analyst_EdgarDatasets_log,  
		FILENAME = 'C:\_analyst\DB\Analyst\dev\Analyst_EdgarFiles_log.ldf',  
		SIZE = 100MB,  
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

use [Analyst_EdgarFiles];
go
print 'creating user in DB Analyst_EdgarFiles'
create user analyst_usr for login analyst_usr;
go

use [Analyst_EdgarFiles];
print 'assigning db_owner to analyst_usr'
EXEC sp_addrolemember N'db_owner', N'analyst_usr'

print 'end'
