USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @SupplierId BIGINT = (SELECT SupplierId FROM [Supplier].[Supplier] WHERE SupplierGUID = 'DD9D46E3-6B25-431D-BABB-7BF06BC7A42A')
DECLARE @SupplierNameSupplierAttributeId BIGINT = (SELECT SupplierAttributeId FROM [Supplier].[SupplierAttribute] WHERE SupplierAttributeDescription = 'Supplier Name')
DECLARE @SupplierAlsoKnownAsSupplierAttributeId BIGINT = (SELECT SupplierAttributeId FROM [Supplier].[SupplierAttribute] WHERE SupplierAttributeDescription = 'Supplier Also Known As')

EXEC [Supplier].[SupplierDetail_Insert] @CreatedByUserId, @SourceId, @SupplierId, @SupplierNameSupplierAttributeId, 'Haven Power Limited'
EXEC [Supplier].[SupplierDetail_Insert] @CreatedByUserId, @SourceId, @SupplierId, @SupplierAlsoKnownAsSupplierAttributeId, 'Haven'
EXEC [Supplier].[SupplierDetail_Insert] @CreatedByUserId, @SourceId, @SupplierId, @SupplierAlsoKnownAsSupplierAttributeId, 'Haven Power'
EXEC [Supplier].[SupplierDetail_Insert] @CreatedByUserId, @SourceId, @SupplierId, @SupplierAlsoKnownAsSupplierAttributeId, 'Drax'