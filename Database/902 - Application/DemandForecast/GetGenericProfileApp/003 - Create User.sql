USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'GetGenericProfileApp')
    BEGIN
        CREATE USER [GetGenericProfileApp] FOR LOGIN [GetGenericProfileApp]
    END
GO

ALTER USER [GetGenericProfileApp] WITH DEFAULT_SCHEMA=[System]
GO
