USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'LockUser.api')
    BEGIN
        CREATE USER [LockUser.api] FOR LOGIN [LockUser.api]
    END
GO

ALTER USER [LockUser.api] WITH DEFAULT_SCHEMA=[System]
GO