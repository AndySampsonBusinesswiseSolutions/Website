USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateFlexContractData.api')
    BEGIN
        CREATE USER [ValidateFlexContractData.api] FOR LOGIN [ValidateFlexContractData.api]
    END
GO

ALTER USER [ValidateFlexContractData.api] WITH DEFAULT_SCHEMA=[System]
GO
