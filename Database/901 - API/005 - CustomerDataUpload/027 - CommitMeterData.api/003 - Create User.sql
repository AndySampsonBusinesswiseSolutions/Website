USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitMeterData.api')
    BEGIN
        CREATE USER [CommitMeterData.api] FOR LOGIN [CommitMeterData.api]
    END
GO

ALTER USER [CommitMeterData.api] WITH DEFAULT_SCHEMA=[System]
GO
