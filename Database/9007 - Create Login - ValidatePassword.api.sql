USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidatePassword.api')
    BEGIN
        CREATE LOGIN [ValidatePassword.api] WITH PASSWORD=N'b7.Q!!X3Hp{\mJ}j', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO

USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidatePassword.api')
    BEGIN
        CREATE USER [ValidatePassword.api] FOR LOGIN [ValidatePassword.api]
    END
GO

ALTER USER [ValidatePassword.api] WITH DEFAULT_SCHEMA=[System]
GO

ALTER ROLE [db_denydatareader] ADD MEMBER [ValidatePassword.api]
GO

ALTER ROLE [db_denydatawriter] ADD MEMBER [ValidatePassword.api]
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Insert] TO [ValidatePassword.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Update] TO [ValidatePassword.api];
GO

GRANT EXECUTE ON OBJECT::[System].[API_GetByGUID] TO [ValidatePassword.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIAttribute_GetByAPIAttributeDescription] TO [ValidatePassword.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIDetail_GetByAPIIDAndAPIAttributeId] TO [ValidatePassword.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_GetByGUIDAndAPIId] TO [ValidatePassword.api];  
GO

GRANT EXECUTE ON OBJECT::[Administration.User].[Password_GetByPassword] TO [ValidatePassword.api];  
GO