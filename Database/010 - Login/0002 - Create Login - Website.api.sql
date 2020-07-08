USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'Website.api')
    BEGIN
        DROP LOGIN [Website.api]
    END
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'Website.api')
    BEGIN
        CREATE LOGIN [Website.api] WITH PASSWORD=N'\wU.D[ArWjPG!F4$', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO

USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'Website.api')
    BEGIN
        CREATE USER [Website.api] FOR LOGIN [Website.api]
    END
GO

ALTER USER [Website.api] WITH DEFAULT_SCHEMA=[System]
GO

ALTER ROLE [db_denydatareader] ADD MEMBER [Website.api]
GO

ALTER ROLE [db_denydatawriter] ADD MEMBER [Website.api]
GO

GRANT EXECUTE ON OBJECT::[System].[API_GetByAPIGUID] TO [Website.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIAttribute_GetByAPIAttributeDescription] TO [Website.api];
GO

GRANT EXECUTE ON OBJECT::[System].[APIDetail_GetByAPIIdAndAPIAttributeId] TO [Website.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Insert] TO [Website.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Update] TO [Website.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessArchive_GetByProcessArchiveGUID] TO [Website.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessArchiveAttribute_GetByProcessArchiveAttributeDescription] TO [Website.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessArchiveDetail_GetByProcessArchiveIDAndProcessArchiveAttributeId] TO [Website.api];
GO

GRANT EXECUTE ON OBJECT::[Administration.User].[User_GetByUserGUID] TO [Website.api];
GO

GRANT EXECUTE ON OBJECT::[Information].[SourceAttribute_GetBySourceAttributeDescription] TO [Website.api];
GO

GRANT EXECUTE ON OBJECT::[Information].[SourceDetail_GetBySourceAttributeIdAndSourceDetailDescription] TO [Website.api];
GO

--TODO: Move into new APIs
-- GRANT EXECUTE ON OBJECT::Customer.Customer_GetByCustomerGUID TO [Website.api];
-- GO

-- GRANT EXECUTE ON OBJECT::Customer.Customer_GetList TO [Website.api];
-- GO

-- GRANT EXECUTE ON OBJECT::Customer.CustomerAttribute_GetByCustomerAttributeDescription TO [Website.api];
-- GO

-- GRANT EXECUTE ON OBJECT::Mapping.CustomerToChildCustomer_GetList TO [Website.api];
-- GO

-- GRANT EXECUTE ON OBJECT::[Mapping].[APIToProcessArchiveDetail_GetByAPIId] TO [Website.api];
-- GO

-- GRANT EXECUTE ON OBJECT::[System].[ProcessArchiveDetail_GetByProcessArchiveDetailId] TO [Website.api];
-- GO

-- GRANT EXECUTE ON OBJECT::[Customer].[CustomerDetail_GetByCustomerIdAndCustomerAttributeId] TO [Website.api];
-- GO