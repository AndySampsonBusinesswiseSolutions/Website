USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitFlexContractData.api')
    BEGIN
        CREATE USER [CommitFlexContractData.api] FOR LOGIN [CommitFlexContractData.api]
    END
GO

ALTER USER [CommitFlexContractData.api] WITH DEFAULT_SCHEMA=[System]
GO
