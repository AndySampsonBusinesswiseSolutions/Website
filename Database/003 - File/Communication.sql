USE [master]
GO

DECLARE @DataPath AS NVARCHAR(255) = (SELECT CONVERT(NVARCHAR(255), SERVERPROPERTY ('InstanceDefaultDataPath')))
DECLARE @SQL NVARCHAR(500) = 'ALTER DATABASE [EMaaS] ADD FILE ( NAME = N''Communication'', FILENAME = N''' + @DataPath + 'Communication.ndf'' , SIZE = 10240KB , FILEGROWTH = 5%) TO FILEGROUP [Communication]'
EXEC sp_sqlexec @SQL
GO

DECLARE @LogPath AS NVARCHAR(255) = (SELECT CONVERT(NVARCHAR(255), SERVERPROPERTY ('InstanceDefaultLogPath')))
DECLARE @SQL NVARCHAR(500) = 'ALTER DATABASE [EMaaS] ADD LOG FILE ( NAME = N''Communication_log'', FILENAME = N''' + @LogPath + 'Communication_log.ldf'' , SIZE = 10240KB , FILEGROWTH = 10%)'
EXEC sp_sqlexec @SQL
GO
