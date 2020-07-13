USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateProcessGUID.api')
    BEGIN
        CREATE USER [ValidateProcessGUID.api] FOR LOGIN [ValidateProcessGUID.api]
    END
GO

ALTER USER [ValidateProcessGUID.api] WITH DEFAULT_SCHEMA=[System]
GO