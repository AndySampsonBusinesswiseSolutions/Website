USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'MapFutureDateIdToUsageDateIdByDate.api')
    BEGIN
        CREATE USER [MapFutureDateIdToUsageDateIdByDate.api] FOR LOGIN [MapFutureDateIdToUsageDateIdByDate.api]
    END
GO

ALTER USER [MapFutureDateIdToUsageDateIdByDate.api] WITH DEFAULT_SCHEMA=[System]
GO
