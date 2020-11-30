USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'GetFlexSpecificProfileApp')
    BEGIN
        CREATE USER [GetFlexSpecificProfileApp] FOR LOGIN [GetFlexSpecificProfileApp]
    END
GO

ALTER USER [GetFlexSpecificProfileApp] WITH DEFAULT_SCHEMA=[System]
GO
