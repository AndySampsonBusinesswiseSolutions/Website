USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ArchiveProcessQueue.api')
    BEGIN
        CREATE LOGIN [ArchiveProcessQueue.api] WITH PASSWORD=N'nb@89qWEW5!6=2s*', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO

USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ArchiveProcessQueue.api')
    BEGIN
        CREATE USER [ArchiveProcessQueue.api] FOR LOGIN [ArchiveProcessQueue.api]
    END
GO

ALTER USER [ArchiveProcessQueue.api] WITH DEFAULT_SCHEMA=[System]
GO

ALTER ROLE [db_denydatareader] ADD MEMBER [ArchiveProcessQueue.api]
GO

ALTER ROLE [db_denydatawriter] ADD MEMBER [ArchiveProcessQueue.api]
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessArchive_Insert] TO [ArchiveProcessQueue.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessArchive_GetByGUID] TO [ArchiveProcessQueue.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessArchive_Update] TO [ArchiveProcessQueue.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessArchiveDetail_Insert] TO [ArchiveProcessQueue.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[API_GetByGUID] TO [ArchiveProcessQueue.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIAttribute_GetByAPIAttributeDescription] TO [ArchiveProcessQueue.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIDetail_GetByAPIIDAndAPIAttributeId] TO [ArchiveProcessQueue.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_GetByGUIDAndAPIId] TO [ArchiveProcessQueue.api];  
GO