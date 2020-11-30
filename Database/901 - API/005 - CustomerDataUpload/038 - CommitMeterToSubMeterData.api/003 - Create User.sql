USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitMeterToSubMeterData.api')
    BEGIN
        CREATE USER [CommitMeterToSubMeterData.api] FOR LOGIN [CommitMeterToSubMeterData.api]
    END
GO

ALTER USER [CommitMeterToSubMeterData.api] WITH DEFAULT_SCHEMA=[System]
GO
