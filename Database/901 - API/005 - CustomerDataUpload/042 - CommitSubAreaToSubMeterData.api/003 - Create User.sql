USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitSubAreaToSubMeterData.api')
    BEGIN
        CREATE USER [CommitSubAreaToSubMeterData.api] FOR LOGIN [CommitSubAreaToSubMeterData.api]
    END
GO

ALTER USER [CommitSubAreaToSubMeterData.api] WITH DEFAULT_SCHEMA=[System]
GO
