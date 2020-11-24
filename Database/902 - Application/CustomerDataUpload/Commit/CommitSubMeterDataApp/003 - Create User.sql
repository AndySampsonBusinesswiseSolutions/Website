USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitSubMeterDataApp')
    BEGIN
        CREATE USER [CommitSubMeterDataApp] FOR LOGIN [CommitSubMeterDataApp]
    END
GO

ALTER USER [CommitSubMeterDataApp] WITH DEFAULT_SCHEMA=[System]
GO
