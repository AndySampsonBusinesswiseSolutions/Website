USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidatePageGUIDApp')
    BEGIN
        CREATE USER [ValidatePageGUIDApp] FOR LOGIN [ValidatePageGUIDApp]
    END
GO

ALTER USER [ValidatePageGUIDApp] WITH DEFAULT_SCHEMA=[System]
GO