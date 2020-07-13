USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateEmailAddress.api')
    BEGIN
        CREATE USER [ValidateEmailAddress.api] FOR LOGIN [ValidateEmailAddress.api]
    END
GO

ALTER USER [ValidateEmailAddress.api] WITH DEFAULT_SCHEMA=[System]
GO