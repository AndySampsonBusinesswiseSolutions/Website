USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateEmailAddressPasswordMapping.api')
    BEGIN
        CREATE LOGIN [ValidateEmailAddressPasswordMapping.api] WITH PASSWORD=N'GQzD2!aZNvffr*zC', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO

USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateEmailAddressPasswordMapping.api')
    BEGIN
        CREATE USER [ValidateEmailAddressPasswordMapping.api] FOR LOGIN [ValidateEmailAddressPasswordMapping.api]
    END
GO

ALTER USER [ValidateEmailAddressPasswordMapping.api] WITH DEFAULT_SCHEMA=[System]
GO

ALTER ROLE [db_denydatareader] ADD MEMBER [ValidateEmailAddressPasswordMapping.api]
GO

ALTER ROLE [db_denydatawriter] ADD MEMBER [ValidateEmailAddressPasswordMapping.api]
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Insert] TO [ValidateEmailAddressPasswordMapping.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Update] TO [ValidateEmailAddressPasswordMapping.api];
GO

GRANT EXECUTE ON OBJECT::[System].[API_GetByAPIGUID] TO [ValidateEmailAddressPasswordMapping.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIAttribute_GetByAPIAttributeDescription] TO [ValidateEmailAddressPasswordMapping.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIDetail_GetByAPIIdAndAPIAttributeId] TO [ValidateEmailAddressPasswordMapping.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_GetByProcessQueueGUIDAndAPIId] TO [ValidateEmailAddressPasswordMapping.api];  
GO

GRANT EXECUTE ON OBJECT::[Administration.User].[Password_GetByPassword] TO [ValidateEmailAddressPasswordMapping.api];  
GO

GRANT EXECUTE ON OBJECT::[Administration.User].[UserDetail_GetByUserDetailDescription] TO [ValidateEmailAddressPasswordMapping.api];  
GO

GRANT EXECUTE ON OBJECT::[Mapping].[PasswordToUser_GetByPasswordIdAndUserId] TO [ValidateEmailAddressPasswordMapping.api];  
GO

GRANT EXECUTE ON OBJECT::[Administration.User].[UserDetail_GetByUserDetailId] TO [ValidateEmailAddressPasswordMapping.api];  
GO