USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'SaveUsageUpload.api')
    BEGIN
        CREATE LOGIN [SaveUsageUpload.api] WITH PASSWORD=N'JM7!?q#g#uTyM^!v', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO

USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'SaveUsageUpload.api')
    BEGIN
        CREATE USER [SaveUsageUpload.api] FOR LOGIN [SaveUsageUpload.api]
    END
GO

ALTER USER [SaveUsageUpload.api] WITH DEFAULT_SCHEMA=[System]
GO

ALTER ROLE [db_denydatareader] ADD MEMBER [SaveUsageUpload.api]
GO

ALTER ROLE [db_denydatawriter] ADD MEMBER [SaveUsageUpload.api]
GO