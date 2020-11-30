USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateSiteDataApp')
    BEGIN
        CREATE USER [ValidateSiteDataApp] FOR LOGIN [ValidateSiteDataApp]
    END
GO

ALTER USER [ValidateSiteDataApp] WITH DEFAULT_SCHEMA=[System]
GO
