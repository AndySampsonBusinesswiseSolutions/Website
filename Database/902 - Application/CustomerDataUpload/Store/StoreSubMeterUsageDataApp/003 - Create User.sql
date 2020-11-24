USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreSubMeterUsageDataApp')
    BEGIN
        CREATE USER [StoreSubMeterUsageDataApp] FOR LOGIN [StoreSubMeterUsageDataApp]
    END
GO

ALTER USER [StoreSubMeterUsageDataApp] WITH DEFAULT_SCHEMA=[System]
GO
