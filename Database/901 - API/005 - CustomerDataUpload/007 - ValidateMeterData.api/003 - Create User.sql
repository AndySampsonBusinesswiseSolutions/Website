USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateMeterData.api')
    BEGIN
        CREATE USER [ValidateMeterData.api] FOR LOGIN [ValidateMeterData.api]
    END
GO

ALTER USER [ValidateMeterData.api] WITH DEFAULT_SCHEMA=[System]
GO
