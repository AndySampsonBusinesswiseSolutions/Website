USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitContractData.api')
    BEGIN
        CREATE USER [CommitContractData.api] FOR LOGIN [CommitContractData.api]
    END
GO

ALTER USER [CommitContractData.api] WITH DEFAULT_SCHEMA=[System]
GO
