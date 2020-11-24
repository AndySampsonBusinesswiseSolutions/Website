USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ArchiveProcessQueueApp')
    BEGIN
        CREATE USER [ArchiveProcessQueueApp] FOR LOGIN [ArchiveProcessQueueApp]
    END
GO

ALTER USER [ArchiveProcessQueueApp] WITH DEFAULT_SCHEMA=[System]
GO