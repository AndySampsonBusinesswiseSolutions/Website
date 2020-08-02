USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreSiteData.api')
    BEGIN
        CREATE USER [StoreSiteData.api] FOR LOGIN [StoreSiteData.api]
    END
GO

ALTER USER [StoreSiteData.api] WITH DEFAULT_SCHEMA=[System]
GO
