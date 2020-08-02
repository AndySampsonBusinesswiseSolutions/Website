USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreMeterUsageData.api')
    BEGIN
        CREATE USER [StoreMeterUsageData.api] FOR LOGIN [StoreMeterUsageData.api]
    END
GO

ALTER USER [StoreMeterUsageData.api] WITH DEFAULT_SCHEMA=[System]
GO
