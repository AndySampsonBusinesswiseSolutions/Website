USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitAreaToMeterData.api')
    BEGIN
        CREATE USER [CommitAreaToMeterData.api] FOR LOGIN [CommitAreaToMeterData.api]
    END
GO

ALTER USER [CommitAreaToMeterData.api] WITH DEFAULT_SCHEMA=[System]
GO
