USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitReferenceVolumeToContractData.api')
    BEGIN
        CREATE USER [CommitReferenceVolumeToContractData.api] FOR LOGIN [CommitReferenceVolumeToContractData.api]
    END
GO

ALTER USER [CommitReferenceVolumeToContractData.api] WITH DEFAULT_SCHEMA=[System]
GO
