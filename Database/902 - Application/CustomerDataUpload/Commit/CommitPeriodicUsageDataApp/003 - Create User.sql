USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitPeriodicUsageDataApp')
    BEGIN
        CREATE USER [CommitPeriodicUsageDataApp] FOR LOGIN [CommitPeriodicUsageDataApp]
    END
GO

ALTER USER [CommitPeriodicUsageDataApp] WITH DEFAULT_SCHEMA=[System]
GO
