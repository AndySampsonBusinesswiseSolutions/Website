USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateMeterExemptionDataApp')
    BEGIN
        CREATE USER [ValidateMeterExemptionDataApp] FOR LOGIN [ValidateMeterExemptionDataApp]
    END
GO

ALTER USER [ValidateMeterExemptionDataApp] WITH DEFAULT_SCHEMA=[System]
GO
