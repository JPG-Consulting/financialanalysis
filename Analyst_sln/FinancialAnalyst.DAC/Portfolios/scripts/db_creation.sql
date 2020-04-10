--https://docs.microsoft.com/en-us/sql/relational-databases/databases/create-a-database?view=sql-server-ver15
USE master ;  
GO  

declare @dbname varchar(20) = 'FA_Portfolios'

IF not EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = @dbname OR name = @dbname))
BEGIN
	CREATE DATABASE FA_Portfolios  
	ON		( NAME = FA_Portfolios_DAT, FILENAME = 'C:\_analyst\DB\dev\FA_Portfolios_DAT.mdf', SIZE = 10, MAXSIZE = 50, FILEGROWTH = 50 )  
	LOG ON	( NAME = FA_Portfolios_LOG, FILENAME = 'C:\_analyst\DB\dev\FA_Portfolios_LOG.ldf', SIZE = 5MB, MAXSIZE = 25MB, FILEGROWTH = 5MB ) ;  
	
	print ('DB created')
END
ELSE
BEGIN
	print('DB "' + @dbname + '" alreadey exists')
END


--https://docs.microsoft.com/en-us/sql/t-sql/statements/create-login-transact-sql?view=sql-server-ver15
declare @loginName varchar(50) = 'fa_user'
If not Exists (select loginname from master.dbo.syslogins where name = @loginName and dbname = @dbname)
Begin
    --Select @SqlStatement = 'CREATE LOGIN ' + QUOTENAME(@loginName) + ' FROM WINDOWS WITH DEFAULT_DATABASE=[PUBS], DEFAULT_LANGUAGE=[us_english]')
    --EXEC sp_executesql @SqlStatement
	CREATE LOGIN fa_user WITH PASSWORD = 'asdf123', DEFAULT_DATABASE  = FA_Portfolios
	print('DB login created')
End
Else
Begin
	print('DB login already exists')
End


GO 

use FA_Portfolios

--https://docs.microsoft.com/en-us/sql/t-sql/statements/create-user-transact-sql?view=sql-server-ver15
CREATE USER fa_user FOR LOGIN fa_user
GO 
print ('User createed')

--usar este para que entityframework cree las tablas
EXEC sp_addrolemember 'db_owner', 'fa_user';
GO 
print('Initially, user fa_user has db_owner role so EF can create the tables. Once tables are created, change permissions')


CREATE TABLE [dbo].[Log4Net] (
    [Id] [int] IDENTITY (1, 1) NOT NULL,
    [Date] [datetime] NOT NULL,
    [Thread] [varchar] (255) NOT NULL,
    [Level] [varchar] (50) NOT NULL,
    [Logger] [varchar] (255) NOT NULL,
    [Message] [nvarchar] (max) NOT NULL,
    [Exception] [nvarchar] (max) NULL
)
print ('log4net table created')