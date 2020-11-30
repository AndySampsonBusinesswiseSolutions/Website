USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateMeterDataApp')
    BEGIN
        CREATE USER [ValidateMeterDataApp] FOR LOGIN [ValidateMeterDataApp]
    END
GO

ALTER USER [ValidateMeterDataApp] WITH DEFAULT_SCHEMA=[System]
GO
