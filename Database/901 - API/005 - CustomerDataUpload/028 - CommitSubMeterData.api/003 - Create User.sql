USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitSubMeterData.api')
    BEGIN
        CREATE USER [CommitSubMeterData.api] FOR LOGIN [CommitSubMeterData.api]
    END
GO

ALTER USER [CommitSubMeterData.api] WITH DEFAULT_SCHEMA=[System]
GO
