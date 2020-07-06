USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'MapCustomerToChildCustomer.api')
    BEGIN
        CREATE LOGIN [MapCustomerToChildCustomer.api] WITH PASSWORD=N'=t@wGL*kf4$DjdJ6', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO

USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'MapCustomerToChildCustomer.api')
    BEGIN
        CREATE USER [MapCustomerToChildCustomer.api] FOR LOGIN [MapCustomerToChildCustomer.api]
    END
GO

ALTER USER [MapCustomerToChildCustomer.api] WITH DEFAULT_SCHEMA=[System]
GO

ALTER ROLE [db_denydatareader] ADD MEMBER [MapCustomerToChildCustomer.api]
GO

ALTER ROLE [db_denydatawriter] ADD MEMBER [MapCustomerToChildCustomer.api]

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Insert] TO [MapCustomerToChildCustomer.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Update] TO [MapCustomerToChildCustomer.api];
GO

GRANT EXECUTE ON OBJECT::[System].[API_GetByAPIGUID] TO [MapCustomerToChildCustomer.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIAttribute_GetByAPIAttributeDescription] TO [MapCustomerToChildCustomer.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIDetail_GetByAPIIdAndAPIAttributeId] TO [MapCustomerToChildCustomer.api];  
GO

GRANT EXECUTE ON OBJECT::[Administration.User].[User_GetByUserGUID] TO [MapCustomerToChildCustomer.api];
GO

GRANT EXECUTE ON OBJECT::[Information].[SourceAttribute_GetBySourceAttributeDescription] TO [MapCustomerToChildCustomer.api];
GO

GRANT EXECUTE ON OBJECT::[Information].[SourceDetail_GetBySourceAttributeIdAndSourceDetailDescription] TO [MapCustomerToChildCustomer.api];
GO

GRANT EXECUTE ON OBJECT::[System].[Error_Insert] TO [MapCustomerToChildCustomer.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_GetByErrorGUID] TO [MapCustomerToChildCustomer.api];  
GO

GRANT EXECUTE ON OBJECT::[Mapping].[CustomerToChildCustomer_Insert] TO [MapCustomerToChildCustomer.api];  
GO

GRANT EXECUTE ON OBJECT::[Mapping].[CustomerToChildCustomer_GetByCustomerId] TO [MapCustomerToChildCustomer.api];  
GO

GRANT EXECUTE ON OBJECT::[Customer].[CustomerAttribute_GetByCustomerAttributeDescription] TO [UpdateCustomerDetail.api];  
GO