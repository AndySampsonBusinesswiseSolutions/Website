USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'GetProfile.api')
    BEGIN
        CREATE USER [GetProfile.api] FOR LOGIN [GetProfile.api]
    END
GO

ALTER USER [GetProfile.api] WITH DEFAULT_SCHEMA=[System]
GO
