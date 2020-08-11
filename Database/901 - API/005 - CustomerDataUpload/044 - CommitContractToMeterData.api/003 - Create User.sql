USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitContractToMeterData.api')
    BEGIN
        CREATE USER [CommitContractToMeterData.api] FOR LOGIN [CommitContractToMeterData.api]
    END
GO

ALTER USER [CommitContractToMeterData.api] WITH DEFAULT_SCHEMA=[System]
GO
