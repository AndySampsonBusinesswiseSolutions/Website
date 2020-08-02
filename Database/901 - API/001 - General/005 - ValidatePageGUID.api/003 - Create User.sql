USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidatePageGUID.api')
    BEGIN
        CREATE USER [ValidatePageGUID.api] FOR LOGIN [ValidatePageGUID.api]
    END
GO

ALTER USER [ValidatePageGUID.api] WITH DEFAULT_SCHEMA=[System]
GO