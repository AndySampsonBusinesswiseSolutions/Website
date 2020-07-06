USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'UpdateCustomerDetail.api')
    BEGIN
        CREATE LOGIN [UpdateCustomerDetail.api] WITH PASSWORD=N'7QJyVNc4+K74F67V', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO

USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'UpdateCustomerDetail.api')
    BEGIN
        CREATE USER [UpdateCustomerDetail.api] FOR LOGIN [UpdateCustomerDetail.api]
    END
GO

ALTER USER [UpdateCustomerDetail.api] WITH DEFAULT_SCHEMA=[System]
GO

ALTER ROLE [db_denydatareader] ADD MEMBER [UpdateCustomerDetail.api]
GO

ALTER ROLE [db_denydatawriter] ADD MEMBER [UpdateCustomerDetail.api]

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Insert] TO [UpdateCustomerDetail.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Update] TO [UpdateCustomerDetail.api];
GO

GRANT EXECUTE ON OBJECT::[System].[API_GetByAPIGUID] TO [UpdateCustomerDetail.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIAttribute_GetByAPIAttributeDescription] TO [UpdateCustomerDetail.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIDetail_GetByAPIIdAndAPIAttributeId] TO [UpdateCustomerDetail.api];  
GO

GRANT EXECUTE ON OBJECT::[Administration.User].[User_GetByUserGUID] TO [UpdateCustomerDetail.api];
GO

GRANT EXECUTE ON OBJECT::[Information].[SourceAttribute_GetBySourceAttributeDescription] TO [UpdateCustomerDetail.api];
GO

GRANT EXECUTE ON OBJECT::[Information].[SourceDetail_GetBySourceAttributeIdAndSourceDetailDescription] TO [UpdateCustomerDetail.api];
GO

GRANT EXECUTE ON OBJECT::[System].[Error_Insert] TO [UpdateCustomerDetail.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_GetByErrorGUID] TO [UpdateCustomerDetail.api];  
GO

GRANT EXECUTE ON OBJECT::[Customer].[CustomerAttribute_GetByCustomerAttributeDescription] TO [UpdateCustomerDetail.api];  
GO

GRANT EXECUTE ON OBJECT::[Customer].[CustomerDetail_Insert] TO [UpdateCustomerDetail.api];  
GO