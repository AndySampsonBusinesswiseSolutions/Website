USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'GetProfileIdApp')
    BEGIN
        CREATE USER [GetProfileIdApp] FOR LOGIN [GetProfileIdApp]
    END
GO

ALTER USER [GetProfileIdApp] WITH DEFAULT_SCHEMA=[System]
GO
