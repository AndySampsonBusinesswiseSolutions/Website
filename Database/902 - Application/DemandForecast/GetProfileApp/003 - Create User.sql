USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'GetProfileApp')
    BEGIN
        CREATE USER [GetProfileApp] FOR LOGIN [GetProfileApp]
    END
GO

ALTER USER [GetProfileApp] WITH DEFAULT_SCHEMA=[System]
GO
