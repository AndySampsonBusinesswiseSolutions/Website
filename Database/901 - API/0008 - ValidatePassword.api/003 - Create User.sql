USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidatePassword.api')
    BEGIN
        CREATE USER [ValidatePassword.api] FOR LOGIN [ValidatePassword.api]
    END
GO

ALTER USER [ValidatePassword.api] WITH DEFAULT_SCHEMA=[System]
GO