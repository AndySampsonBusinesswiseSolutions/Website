USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreMeterData.api')
    BEGIN
        CREATE USER [StoreMeterData.api] FOR LOGIN [StoreMeterData.api]
    END
GO

ALTER USER [StoreMeterData.api] WITH DEFAULT_SCHEMA=[System]
GO
