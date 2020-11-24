USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitMeterToMeterTimeswitchCodeDataApp')
    BEGIN
        CREATE USER [CommitMeterToMeterTimeswitchCodeDataApp] FOR LOGIN [CommitMeterToMeterTimeswitchCodeDataApp]
    END
GO

ALTER USER [CommitMeterToMeterTimeswitchCodeDataApp] WITH DEFAULT_SCHEMA=[System]
GO
