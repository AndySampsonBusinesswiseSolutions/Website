USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitMeterToSubMeterDataApp')
    BEGIN
        CREATE USER [CommitMeterToSubMeterDataApp] FOR LOGIN [CommitMeterToSubMeterDataApp]
    END
GO

ALTER USER [CommitMeterToSubMeterDataApp] WITH DEFAULT_SCHEMA=[System]
GO
