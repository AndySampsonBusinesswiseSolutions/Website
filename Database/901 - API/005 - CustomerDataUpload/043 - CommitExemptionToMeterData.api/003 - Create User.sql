USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitExemptionToMeterData.api')
    BEGIN
        CREATE USER [CommitExemptionToMeterData.api] FOR LOGIN [CommitExemptionToMeterData.api]
    END
GO

ALTER USER [CommitExemptionToMeterData.api] WITH DEFAULT_SCHEMA=[System]
GO
