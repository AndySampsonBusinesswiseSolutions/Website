USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateFlexContractDataApp')
    BEGIN
        CREATE USER [ValidateFlexContractDataApp] FOR LOGIN [ValidateFlexContractDataApp]
    END
GO

ALTER USER [ValidateFlexContractDataApp] WITH DEFAULT_SCHEMA=[System]
GO
