USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'GetMappedUsageDateIdApp')
    BEGIN
        CREATE USER [GetMappedUsageDateIdApp] FOR LOGIN [GetMappedUsageDateIdApp]
    END
GO

ALTER USER [GetMappedUsageDateIdApp] WITH DEFAULT_SCHEMA=[System]
GO
