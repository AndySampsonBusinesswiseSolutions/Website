USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateMeterUsageData.api')
    BEGIN
        CREATE USER [ValidateMeterUsageData.api] FOR LOGIN [ValidateMeterUsageData.api]
    END
GO

ALTER USER [ValidateMeterUsageData.api] WITH DEFAULT_SCHEMA=[System]
GO
