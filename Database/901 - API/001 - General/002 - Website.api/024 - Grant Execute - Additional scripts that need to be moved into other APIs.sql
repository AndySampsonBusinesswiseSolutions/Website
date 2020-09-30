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

-- GRANT EXECUTE ON OBJECT::[Customer].[SiteDetail_GetBySiteAttributeId] TO [Website.api];
-- GO

-- GRANT EXECUTE ON OBJECT::[Customer].[SiteAttribute_GetBySiteAttributeDescription] TO [Website.api];
-- GO

-- GRANT EXECUTE ON OBJECT::[Mapping].[MeterToSite_GetList] TO [Website.api];
-- GO

-- GRANT EXECUTE ON OBJECT::[Customer].[MeterDetail_GetByMeterAttributeId] TO [Website.api];
-- GO

-- GRANT EXECUTE ON OBJECT::[Mapping].[AreaToMeter_GetList] TO [Website.api];
-- GO

-- GRANT EXECUTE ON OBJECT::[Information].[Area_GetList] TO [Website.api];
-- GO

-- GRANT EXECUTE ON OBJECT::[Mapping].[CommodityToMeter_GetList] TO [Website.api];
-- GO

-- GRANT EXECUTE ON OBJECT::[Information].[Commodity_GetList] TO [Website.api];
-- GO

-- GRANT EXECUTE ON OBJECT::[Customer].[MeterAttribute_GetByMeterAttributeDescription] TO [Website.api];
-- GO