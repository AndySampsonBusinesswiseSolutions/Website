USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ArchiveProcessQueue.api')
    BEGIN
        CREATE USER [ArchiveProcessQueue.api] FOR LOGIN [ArchiveProcessQueue.api]
    END
GO

ALTER USER [ArchiveProcessQueue.api] WITH DEFAULT_SCHEMA=[System]
GO