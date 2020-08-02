USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreSubMeterData.api')
    BEGIN
        CREATE USER [StoreSubMeterData.api] FOR LOGIN [StoreSubMeterData.api]
    END
GO

ALTER USER [StoreSubMeterData.api] WITH DEFAULT_SCHEMA=[System]
GO
