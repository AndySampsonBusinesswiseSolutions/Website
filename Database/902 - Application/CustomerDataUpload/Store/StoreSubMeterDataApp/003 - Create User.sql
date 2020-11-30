USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreSubMeterDataApp')
    BEGIN
        CREATE USER [StoreSubMeterDataApp] FOR LOGIN [StoreSubMeterDataApp]
    END
GO

ALTER USER [StoreSubMeterDataApp] WITH DEFAULT_SCHEMA=[System]
GO
