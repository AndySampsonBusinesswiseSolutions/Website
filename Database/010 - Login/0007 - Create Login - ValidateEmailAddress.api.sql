USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateEmailAddress.api')
    BEGIN
        CREATE LOGIN [ValidateEmailAddress.api] WITH PASSWORD=N'}h8FfD2r[Rd~PPNR', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO

USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateEmailAddress.api')
    BEGIN
        CREATE USER [ValidateEmailAddress.api] FOR LOGIN [ValidateEmailAddress.api]
    END
GO

ALTER USER [ValidateEmailAddress.api] WITH DEFAULT_SCHEMA=[System]
GO

ALTER ROLE [db_denydatareader] ADD MEMBER [ValidateEmailAddress.api]
GO

ALTER ROLE [db_denydatawriter] ADD MEMBER [ValidateEmailAddress.api]
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Insert] TO [ValidateEmailAddress.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Update] TO [ValidateEmailAddress.api];
GO

GRANT EXECUTE ON OBJECT::[System].[API_GetByGUID] TO [ValidateEmailAddress.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIAttribute_GetByAPIAttributeDescription] TO [ValidateEmailAddress.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIDetail_GetByAPIIDAndAPIAttributeId] TO [ValidateEmailAddress.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_GetByGUIDAndAPIId] TO [ValidateEmailAddress.api];  
GO

GRANT EXECUTE ON OBJECT::[Administration.User].[UserDetail_GetByUserDetailDescription] TO [ValidateEmailAddress.api];  
GO