USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitSubMeterUsageDataApp')
    BEGIN
        CREATE USER [CommitSubMeterUsageDataApp] FOR LOGIN [CommitSubMeterUsageDataApp]
    END
GO

ALTER USER [CommitSubMeterUsageDataApp] WITH DEFAULT_SCHEMA=[System]
GO
