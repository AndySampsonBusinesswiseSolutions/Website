USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreMeterUsageDataApp')
    BEGIN
        CREATE USER [StoreMeterUsageDataApp] FOR LOGIN [StoreMeterUsageDataApp]
    END
GO

ALTER USER [StoreMeterUsageDataApp] WITH DEFAULT_SCHEMA=[System]
GO
