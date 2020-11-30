USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreMeterExemptionDataApp')
    BEGIN
        CREATE USER [StoreMeterExemptionDataApp] FOR LOGIN [StoreMeterExemptionDataApp]
    END
GO

ALTER USER [StoreMeterExemptionDataApp] WITH DEFAULT_SCHEMA=[System]
GO
