USE [EMaaS]
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