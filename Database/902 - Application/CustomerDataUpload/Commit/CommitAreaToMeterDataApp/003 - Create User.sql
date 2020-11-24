USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitAreaToMeterDataApp')
    BEGIN
        CREATE USER [CommitAreaToMeterDataApp] FOR LOGIN [CommitAreaToMeterDataApp]
    END
GO

ALTER USER [CommitAreaToMeterDataApp] WITH DEFAULT_SCHEMA=[System]
GO
