USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateSiteData.api')
    BEGIN
        CREATE USER [ValidateSiteData.api] FOR LOGIN [ValidateSiteData.api]
    END
GO

ALTER USER [ValidateSiteData.api] WITH DEFAULT_SCHEMA=[System]
GO
