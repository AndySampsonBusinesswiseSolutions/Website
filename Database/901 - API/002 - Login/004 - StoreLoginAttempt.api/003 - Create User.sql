USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreLoginAttempt.api')
    BEGIN
        CREATE USER [StoreLoginAttempt.api] FOR LOGIN [StoreLoginAttempt.api]
    END
GO

ALTER USER [StoreLoginAttempt.api] WITH DEFAULT_SCHEMA=[System]
GO