USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreMeterDataApp')
    BEGIN
        CREATE USER [StoreMeterDataApp] FOR LOGIN [StoreMeterDataApp]
    END
GO

ALTER USER [StoreMeterDataApp] WITH DEFAULT_SCHEMA=[System]
GO
