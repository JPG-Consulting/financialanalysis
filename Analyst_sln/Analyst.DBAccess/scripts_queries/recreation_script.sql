
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
print 'Creating analyst'
create database Analyst
ON   
	( NAME = Analyst_dat,  
		FILENAME = 'E:\_analyst_db\Analyst_dat.mdf',  
		SIZE = 2000MB,  
		--MAXSIZE = 50,  
		FILEGROWTH = 2000MB )  
	LOG ON  
	( NAME = Analyst_log,  
		FILENAME = 'E:\_analyst_db\Analyst_log.ldf',  
		SIZE = 100MB,  
		--MAXSIZE = 25MB,  
		FILEGROWTH = 100MB ) ; 
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
