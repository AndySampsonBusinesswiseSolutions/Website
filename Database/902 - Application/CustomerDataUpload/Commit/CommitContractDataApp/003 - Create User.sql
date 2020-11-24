USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitContractDataApp')
    BEGIN
        CREATE USER [CommitContractDataApp] FOR LOGIN [CommitContractDataApp]
    END
GO

ALTER USER [CommitContractDataApp] WITH DEFAULT_SCHEMA=[System]
GO
