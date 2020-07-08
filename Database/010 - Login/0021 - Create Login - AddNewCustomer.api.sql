USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'AddNewCustomer.api')
    BEGIN
        DROP LOGIN [AddNewCustomer.api]
    END
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'AddNewCustomer.api')
    BEGIN
        CREATE LOGIN [AddNewCustomer.api] WITH PASSWORD=N'$hRXtrCfb$$W3XZ+', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO

USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'AddNewCustomer.api')
    BEGIN
        CREATE USER [AddNewCustomer.api] FOR LOGIN [AddNewCustomer.api]
    END
GO

ALTER USER [AddNewCustomer.api] WITH DEFAULT_SCHEMA=[System]
GO

ALTER ROLE [db_denydatareader] ADD MEMBER [AddNewCustomer.api]
GO

ALTER ROLE [db_denydatawriter] ADD MEMBER [AddNewCustomer.api]

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Insert] TO [AddNewCustomer.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Update] TO [AddNewCustomer.api];
GO

GRANT EXECUTE ON OBJECT::[System].[API_GetByAPIGUID] TO [AddNewCustomer.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIAttribute_GetByAPIAttributeDescription] TO [AddNewCustomer.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIDetail_GetByAPIIdAndAPIAttributeId] TO [AddNewCustomer.api];  
GO

GRANT EXECUTE ON OBJECT::[Administration.User].[User_GetByUserGUID] TO [AddNewCustomer.api];
GO

GRANT EXECUTE ON OBJECT::[Information].[SourceAttribute_GetBySourceAttributeDescription] TO [AddNewCustomer.api];
GO

GRANT EXECUTE ON OBJECT::[Information].[SourceDetail_GetBySourceAttributeIdAndSourceDetailDescription] TO [AddNewCustomer.api];
GO

GRANT EXECUTE ON OBJECT::[System].[Error_Insert] TO [AddNewCustomer.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_GetByErrorGUID] TO [AddNewCustomer.api];  
GO

GRANT EXECUTE ON OBJECT::[Customer].[CustomerAttribute_GetByCustomerAttributeDescription] TO [AddNewCustomer.api];  
GO

GRANT EXECUTE ON OBJECT::[Customer].[CustomerDetail_GetByCustomerAttributeIdAndCustomerDetailDescription] TO [AddNewCustomer.api];  
GO

GRANT EXECUTE ON OBJECT::[Customer].[Customer_Insert] TO [AddNewCustomer.api];  
GO