USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateFixedContractDataApp')
    BEGIN
        CREATE USER [ValidateFixedContractDataApp] FOR LOGIN [ValidateFixedContractDataApp]
    END
GO

ALTER USER [ValidateFixedContractDataApp] WITH DEFAULT_SCHEMA=[System]
GO
