USE [master]
GO

DECLARE @DataPath AS NVARCHAR(255) = (SELECT CONVERT(NVARCHAR(255), SERVERPROPERTY ('InstanceDefaultDataPath')))
DECLARE @SQL NVARCHAR(500) = 'ALTER DATABASE [EMaaS] ADD FILE ( NAME = N''DemandForecast'', FILENAME = N''' + @DataPath + 'DemandForecast.ndf'' , SIZE = 10240KB , FILEGROWTH = 5%) TO FILEGROUP [DemandForecast]'
EXEC sp_sqlexec @SQL
GO

DECLARE @LogPath AS NVARCHAR(255) = (SELECT CONVERT(NVARCHAR(255), SERVERPROPERTY ('InstanceDefaultLogPath')))
DECLARE @SQL NVARCHAR(500) = 'ALTER DATABASE [EMaaS] ADD LOG FILE ( NAME = N''DemandForecast_log'', FILENAME = N''' + @LogPath + 'DemandForecast_log.ldf'' , SIZE = 10240KB , FILEGROWTH = 10%)'
EXEC sp_sqlexec @SQL
GO
