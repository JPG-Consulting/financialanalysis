
use [master];
drop database Analyst
go

use [master];
create database Analyst
ON   
	( NAME = Analyst_dat,  
		FILENAME = 'D:\Importante\Desarrollo\_Bases de datos\Analyst\Analyst_dat.mdf',  
		SIZE = 10,  
		MAXSIZE = 50,  
		FILEGROWTH = 5 )  
	LOG ON  
	( NAME = Analyst_log,  
		FILENAME = 'D:\Importante\Desarrollo\_Bases de datos\Analyst\Analyst_log.ldf',  
		SIZE = 5MB,  
		MAXSIZE = 25MB,  
		FILEGROWTH = 5MB ) ; 
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
select count(1) from secforms; --son 144, mas los 7 duplicados son los 151 que deberian ser
select count(1) from sics;

*/