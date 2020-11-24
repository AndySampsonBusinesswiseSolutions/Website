USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitSubAreaToSubMeterDataApp')
    BEGIN
        CREATE USER [CommitSubAreaToSubMeterDataApp] FOR LOGIN [CommitSubAreaToSubMeterDataApp]
    END
GO

ALTER USER [CommitSubAreaToSubMeterDataApp] WITH DEFAULT_SCHEMA=[System]
GO
