
DECLARE @dbname nvarchar(128)
SET @dbname = N'Analyr'

use [master];
IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = @dbname OR name = @dbname)))
	PRINT 'Droping Analyst';
	drop database Analyst;
go

use [master];
create database Analyst
ON   
	( NAME = Analyst_dat,  
		FILENAME = 'E:\_analyst_db\Analyst_dat.mdf',  
		SIZE = 20,  
		--MAXSIZE = 50,  
		FILEGROWTH = 10 )  
	LOG ON  
	( NAME = Analyst_log,  
		FILENAME = 'E:\_analyst_db\Analyst_log.ldf',  
		SIZE = 20MB,  
		--MAXSIZE = 25MB,  
		FILEGROWTH = 10MB ) ; 
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
create user analyst_usr for login analyst_usr;
go

use [Analyst];
EXEC sp_addrolemember N'db_owner', N'analyst_usr'

----------------------------------------------------------------------
/*
--no funciona porque tiene el indice
use [Analyst];
ALTER TABLE [dbo].[EdgarDatasetTags] ALTER COLUMN [Tag] VARCHAR(256) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL
*/
----------------------------------------------------------------------

/*
select count(1) from secforms; --son 144, mas los 7 duplicados son los 151 que deberian ser
select count(1) from sics;

*/