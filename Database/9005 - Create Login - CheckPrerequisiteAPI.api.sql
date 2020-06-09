USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CheckPrerequisiteAPI.api')
    BEGIN
        CREATE LOGIN [CheckPrerequisiteAPI.api] WITH PASSWORD=N'w8chCkRAW]\N[7Hh', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO

USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CheckPrerequisiteAPI.api')
    BEGIN
        CREATE USER [CheckPrerequisiteAPI.api] FOR LOGIN [CheckPrerequisiteAPI.api]
    END
GO

ALTER USER [CheckPrerequisiteAPI.api] WITH DEFAULT_SCHEMA=[System]
GO

ALTER ROLE [db_denydatareader] ADD MEMBER [CheckPrerequisiteAPI.api]
GO

ALTER ROLE [db_denydatawriter] ADD MEMBER [CheckPrerequisiteAPI.api]
GO

GRANT EXECUTE ON OBJECT::[System].[API_GetByGUID] TO [CheckPrerequisiteAPI.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIAttribute_GetByAPIAttributeDescription] TO [CheckPrerequisiteAPI.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIDetail_GetByAPIIDAndAPIAttributeId] TO [CheckPrerequisiteAPI.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_GetByGUIDAndAPIId] TO [CheckPrerequisiteAPI.api];  
GO