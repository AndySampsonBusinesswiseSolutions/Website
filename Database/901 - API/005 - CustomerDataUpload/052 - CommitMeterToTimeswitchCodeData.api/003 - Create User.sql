USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitMeterToTimeswitchCodeData.api')
    BEGIN
        CREATE USER [CommitMeterToTimeswitchCodeData.api] FOR LOGIN [CommitMeterToTimeswitchCodeData.api]
    END
GO

ALTER USER [CommitMeterToTimeswitchCodeData.api] WITH DEFAULT_SCHEMA=[System]
GO
