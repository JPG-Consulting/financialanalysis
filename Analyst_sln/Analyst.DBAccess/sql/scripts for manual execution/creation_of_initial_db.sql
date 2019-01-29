
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
use [master];

DECLARE @dbname nvarchar(128)
SET @dbname = N'Analyst_Edgar_dev'

IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = @dbname OR name = @dbname)))
	begin
		PRINT 'Droping ' +@dbname;
		drop database Analyst_Edgar_dev;
	end
else
	begin
		print @dbname +' does not exist';
	end


print 'Creating '+@dbname;
create database Analyst_Edgar_dev
ON   
	( NAME = Analyst_EdgarDatasets_dat,  
		FILENAME = 'C:\_analyst\DB\dev\Analyst_Edgar_dat.mdf',  
		SIZE = 5000MB,  
		MAXSIZE = UNLIMITED,  
		FILEGROWTH = 2000MB )  
	LOG ON  
	( NAME = Analyst_EdgarDatasets_log,  
		FILENAME = 'C:\_analyst\DB\dev\Analyst_Edgar_log.ldf',  
		SIZE = 1000MB,  
		MAXSIZE = UNLIMITED,  
		FILEGROWTH = 1000MB ) ; 
go


use [Analyst_Edgar_dev];
DECLARE @dbname nvarchar(128)
SET @dbname = N'Analyst_Edgar_dev'

print N'creating user in DB ' + @dbname;
create user analyst_usr for login analyst_usr;

print 'assigning db_owner to analyst_usr'
EXEC sp_addrolemember N'db_owner', N'analyst_usr'

print 'end '+@dbname
go
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
SET @dbname = N'Analyst_dev'

use [master];

--create login analyst_usr with password = 'asdf0001';

IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = @dbname OR name = @dbname)))
	begin
		PRINT 'Droping '+@dbname;
		drop database Analyst_dev;
	end
else
	begin
		print @dbname+' does not exist';
	end


print 'Creating '+@dbname
create database Analyst_dev
ON   
	( NAME = Analyst_EdgarFiles_dat,  
		FILENAME = 'C:\_analyst\DB\dev\Analyst_dat.mdf',  
		SIZE = 1000MB,  
		MAXSIZE = UNLIMITED,  
		FILEGROWTH = 2000MB )  
	LOG ON  
	( NAME = Analyst_EdgarDatasets_log,  
		FILENAME = 'C:\_analyst\DB\dev\Analyst_log.ldf',  
		SIZE = 100MB,  
		MAXSIZE = UNLIMITED,  
		FILEGROWTH = 5000MB ) ; 
go


use [Analyst_dev];

DECLARE @dbname nvarchar(128)
SET @dbname = N'Analyst_dev'

print 'creating user in DB '+@dbname
create user analyst_usr for login analyst_usr;

print 'assigning db_owner to analyst_usr'
EXEC sp_addrolemember N'db_owner', N'analyst_usr'

print 'end '+@dbname
go