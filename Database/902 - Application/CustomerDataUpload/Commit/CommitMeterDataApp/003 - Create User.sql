USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitMeterDataApp')
    BEGIN
        CREATE USER [CommitMeterDataApp] FOR LOGIN [CommitMeterDataApp]
    END
GO

ALTER USER [CommitMeterDataApp] WITH DEFAULT_SCHEMA=[System]
GO
