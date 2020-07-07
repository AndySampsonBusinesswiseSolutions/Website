USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'MapCustomerToChildCustomer.api')
    BEGIN
        DROP LOGIN [MapCustomerToChildCustomer.api]
    END
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'MapCustomerToChildCustomer.api')
    BEGIN
        CREATE LOGIN [MapCustomerToChildCustomer.api] WITH PASSWORD=N'6dFB@tk?7L$UrQ9p', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
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

GRANT EXECUTE ON OBJECT::[Customer].[CustomerAttribute_GetByCustomerAttributeDescription] TO [MapCustomerToChildCustomer.api];  
GO

GRANT EXECUTE ON OBJECT::[Customer].[Customer_GetByCustomerGUID] TO [MapCustomerToChildCustomer.api];  
GO