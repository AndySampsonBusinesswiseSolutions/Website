USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitMeterToMeterTimeswitchCodeData.api')
    BEGIN
        CREATE USER [CommitMeterToMeterTimeswitchCodeData.api] FOR LOGIN [CommitMeterToMeterTimeswitchCodeData.api]
    END
GO

ALTER USER [CommitMeterToMeterTimeswitchCodeData.api] WITH DEFAULT_SCHEMA=[System]
GO
