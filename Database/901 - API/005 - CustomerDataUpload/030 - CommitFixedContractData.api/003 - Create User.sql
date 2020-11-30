USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitFixedContractData.api')
    BEGIN
        CREATE USER [CommitFixedContractData.api] FOR LOGIN [CommitFixedContractData.api]
    END
GO

ALTER USER [CommitFixedContractData.api] WITH DEFAULT_SCHEMA=[System]
GO
