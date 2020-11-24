USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitAssetToSubMeterDataApp')
    BEGIN
        CREATE USER [CommitAssetToSubMeterDataApp] FOR LOGIN [CommitAssetToSubMeterDataApp]
    END
GO

ALTER USER [CommitAssetToSubMeterDataApp] WITH DEFAULT_SCHEMA=[System]
GO
