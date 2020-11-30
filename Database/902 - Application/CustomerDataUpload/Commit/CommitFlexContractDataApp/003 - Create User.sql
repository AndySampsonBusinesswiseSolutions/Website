USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitFlexContractDataApp')
    BEGIN
        CREATE USER [CommitFlexContractDataApp] FOR LOGIN [CommitFlexContractDataApp]
    END
GO

ALTER USER [CommitFlexContractDataApp] WITH DEFAULT_SCHEMA=[System]
GO
