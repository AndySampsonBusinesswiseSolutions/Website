USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateFixedContractData.api')
    BEGIN
        CREATE USER [ValidateFixedContractData.api] FOR LOGIN [ValidateFixedContractData.api]
    END
GO

ALTER USER [ValidateFixedContractData.api] WITH DEFAULT_SCHEMA=[System]
GO
