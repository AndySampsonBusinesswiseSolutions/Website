USE [master]
GO

DECLARE @DataPath AS NVARCHAR(255) = (SELECT CONVERT(NVARCHAR(255), SERVERPROPERTY ('InstanceDefaultDataPath')))
DECLARE @SQL NVARCHAR(500) = 'ALTER DATABASE [EMaaS] ADD FILE ( NAME = N''Mapping'', FILENAME = N''' + @DataPath + 'Mapping.ndf'' , SIZE = 10240KB , FILEGROWTH = 5%) TO FILEGROUP [Mapping]'
EXEC sp_sqlexec @SQL
GO

DECLARE @LogPath AS NVARCHAR(255) = (SELECT CONVERT(NVARCHAR(255), SERVERPROPERTY ('InstanceDefaultLogPath')))
DECLARE @SQL NVARCHAR(500) = 'ALTER DATABASE [EMaaS] ADD LOG FILE ( NAME = N''Mapping_log'', FILENAME = N''' + @LogPath + 'Mapping_log.ldf'' , SIZE = 10240KB , FILEGROWTH = 10%)'
EXEC sp_sqlexec @SQL
GO
