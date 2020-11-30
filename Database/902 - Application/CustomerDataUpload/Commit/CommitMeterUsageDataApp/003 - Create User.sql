USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitMeterUsageDataApp')
    BEGIN
        CREATE USER [CommitMeterUsageDataApp] FOR LOGIN [CommitMeterUsageDataApp]
    END
GO

ALTER USER [CommitMeterUsageDataApp] WITH DEFAULT_SCHEMA=[System]
GO
