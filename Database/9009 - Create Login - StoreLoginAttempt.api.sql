USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreLoginAttempt.api')
    BEGIN
        CREATE LOGIN [StoreLoginAttempt.api] WITH PASSWORD=N'mLdas-Y*x2rbnJ2e', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO

USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreLoginAttempt.api')
    BEGIN
        CREATE USER [StoreLoginAttempt.api] FOR LOGIN [StoreLoginAttempt.api]
    END
GO

ALTER USER [StoreLoginAttempt.api] WITH DEFAULT_SCHEMA=[System]
GO

ALTER ROLE [db_denydatareader] ADD MEMBER [StoreLoginAttempt.api]
GO

ALTER ROLE [db_denydatawriter] ADD MEMBER [StoreLoginAttempt.api]
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Insert] TO [StoreLoginAttempt.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Update] TO [StoreLoginAttempt.api];
GO

GRANT EXECUTE ON OBJECT::[System].[API_GetByGUID] TO [StoreLoginAttempt.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIAttribute_GetByAPIAttributeDescription] TO [StoreLoginAttempt.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIDetail_GetByAPIIDAndAPIAttributeId] TO [StoreLoginAttempt.api];  
GO

GRANT EXECUTE ON OBJECT::[Administration.User].[UserDetail_GetByUserDetailDescription] TO [StoreLoginAttempt.api];  
GO

GRANT EXECUTE ON OBJECT::[Administration.User].[UserDetail_GetByUserDetailId] TO [StoreLoginAttempt.api];  
GO