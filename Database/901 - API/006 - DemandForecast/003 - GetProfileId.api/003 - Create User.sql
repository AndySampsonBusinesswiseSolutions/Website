USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'GetProfileId.api')
    BEGIN
        CREATE USER [GetProfileId.api] FOR LOGIN [GetProfileId.api]
    END
GO

ALTER USER [GetProfileId.api] WITH DEFAULT_SCHEMA=[System]
GO
