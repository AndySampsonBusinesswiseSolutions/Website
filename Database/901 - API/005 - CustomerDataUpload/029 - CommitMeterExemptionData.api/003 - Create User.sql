USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitMeterExemptionData.api')
    BEGIN
        CREATE USER [CommitMeterExemptionData.api] FOR LOGIN [CommitMeterExemptionData.api]
    END
GO

ALTER USER [CommitMeterExemptionData.api] WITH DEFAULT_SCHEMA=[System]
GO
