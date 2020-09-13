USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'GetGenericProfile.api')
    BEGIN
        CREATE USER [GetGenericProfile.api] FOR LOGIN [GetGenericProfile.api]
    END
GO

ALTER USER [GetGenericProfile.api] WITH DEFAULT_SCHEMA=[System]
GO
