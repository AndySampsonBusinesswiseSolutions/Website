USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateMeterExemptionData.api')
    BEGIN
        CREATE USER [ValidateMeterExemptionData.api] FOR LOGIN [ValidateMeterExemptionData.api]
    END
GO

ALTER USER [ValidateMeterExemptionData.api] WITH DEFAULT_SCHEMA=[System]
GO
