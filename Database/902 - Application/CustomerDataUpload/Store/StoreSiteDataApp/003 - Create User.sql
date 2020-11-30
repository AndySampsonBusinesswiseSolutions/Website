USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreSiteDataApp')
    BEGIN
        CREATE USER [StoreSiteDataApp] FOR LOGIN [StoreSiteDataApp]
    END
GO

ALTER USER [StoreSiteDataApp] WITH DEFAULT_SCHEMA=[System]
GO
