USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateProcessGUIDApp')
    BEGIN
        CREATE USER [ValidateProcessGUIDApp] FOR LOGIN [ValidateProcessGUIDApp]
    END
GO

ALTER USER [ValidateProcessGUIDApp] WITH DEFAULT_SCHEMA=[System]
GO