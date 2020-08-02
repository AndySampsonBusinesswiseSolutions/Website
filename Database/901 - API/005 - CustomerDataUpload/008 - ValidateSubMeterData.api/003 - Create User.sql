USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateSubMeterData.api')
    BEGIN
        CREATE USER [ValidateSubMeterData.api] FOR LOGIN [ValidateSubMeterData.api]
    END
GO

ALTER USER [ValidateSubMeterData.api] WITH DEFAULT_SCHEMA=[System]
GO
