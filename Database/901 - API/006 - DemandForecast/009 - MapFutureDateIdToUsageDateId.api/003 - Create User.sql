USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'MapFutureDateIdToUsageDateId.api')
    BEGIN
        CREATE USER [MapFutureDateIdToUsageDateId.api] FOR LOGIN [MapFutureDateIdToUsageDateId.api]
    END
GO

ALTER USER [MapFutureDateIdToUsageDateId.api] WITH DEFAULT_SCHEMA=[System]
GO
