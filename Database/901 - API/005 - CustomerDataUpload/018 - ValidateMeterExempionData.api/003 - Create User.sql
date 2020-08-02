USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateMeterExempionData.api')
    BEGIN
        CREATE USER [ValidateMeterExempionData.api] FOR LOGIN [ValidateMeterExempionData.api]
    END
GO

ALTER USER [ValidateMeterExempionData.api] WITH DEFAULT_SCHEMA=[System]
GO
