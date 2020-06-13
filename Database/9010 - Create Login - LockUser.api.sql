USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'LockUser.api')
    BEGIN
        CREATE LOGIN [LockUser.api] WITH PASSWORD=N'JM7!?q#g#uTyM^!v', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO

USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'LockUser.api')
    BEGIN
        CREATE USER [LockUser.api] FOR LOGIN [LockUser.api]
    END
GO

ALTER USER [LockUser.api] WITH DEFAULT_SCHEMA=[System]
GO

ALTER ROLE [db_denydatareader] ADD MEMBER [LockUser.api]
GO

ALTER ROLE [db_denydatawriter] ADD MEMBER [LockUser.api]
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Insert] TO [LockUser.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Update] TO [LockUser.api];
GO

GRANT EXECUTE ON OBJECT::[System].[API_GetByGUID] TO [LockUser.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIAttribute_GetByAPIAttributeDescription] TO [LockUser.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIDetail_GetByAPIIDAndAPIAttributeId] TO [LockUser.api];  
GO