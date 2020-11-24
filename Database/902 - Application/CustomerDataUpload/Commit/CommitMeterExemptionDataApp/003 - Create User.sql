USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitMeterExemptionDataApp')
    BEGIN
        CREATE USER [CommitMeterExemptionDataApp] FOR LOGIN [CommitMeterExemptionDataApp]
    END
GO

ALTER USER [CommitMeterExemptionDataApp] WITH DEFAULT_SCHEMA=[System]
GO
