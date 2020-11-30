USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitFixedContractDataApp')
    BEGIN
        CREATE USER [CommitFixedContractDataApp] FOR LOGIN [CommitFixedContractDataApp]
    END
GO

ALTER USER [CommitFixedContractDataApp] WITH DEFAULT_SCHEMA=[System]
GO
