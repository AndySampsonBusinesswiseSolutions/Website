USE [master]
GO

DECLARE @DataPath AS NVARCHAR(255) = (SELECT CONVERT(NVARCHAR(255), SERVERPROPERTY ('InstanceDefaultDataPath')))
DECLARE @SQL NVARCHAR(500) = 'ALTER DATABASE [EMaaS] ADD FILE ( NAME = N''System'', FILENAME = N''' + @DataPath + 'System.ndf'' , SIZE = 10240KB , FILEGROWTH = 5%) TO FILEGROUP [System]'
EXEC sp_sqlexec @SQL
GO

DECLARE @LogPath AS NVARCHAR(255) = (SELECT CONVERT(NVARCHAR(255), SERVERPROPERTY ('InstanceDefaultLogPath')))
DECLARE @SQL NVARCHAR(500) = 'ALTER DATABASE [EMaaS] ADD LOG FILE ( NAME = N''System_log'', FILENAME = N''' + @LogPath + 'System_log.ldf'' , SIZE = 10240KB , FILEGROWTH = 10%)'
EXEC sp_sqlexec @SQL
GO
