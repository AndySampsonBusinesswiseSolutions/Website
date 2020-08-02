USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreMeterExemptionData.api')
    BEGIN
        CREATE USER [StoreMeterExemptionData.api] FOR LOGIN [StoreMeterExemptionData.api]
    END
GO

ALTER USER [StoreMeterExemptionData.api] WITH DEFAULT_SCHEMA=[System]
GO
