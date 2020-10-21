USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @SupplierId BIGINT = (SELECT SupplierId FROM [Supplier].[Supplier] WHERE SupplierGUID = 'DD9D46E3-6B25-431D-BABB-7BF06BC7A42A') --Haven

DECLARE @ProductId BIGINT = (SELECT ProductId FROM [Supplier].[Product] WHERE ProductGUID = '4E16DE14-299C-4F29-8638-2317B1CA7B79') --Fully Fixed
EXEC [Mapping].[ProductToSupplier_Insert] @CreatedByUserId, @SourceId, @ProductId, @SupplierId

SET @ProductId = (SELECT ProductId FROM [Supplier].[Product] WHERE ProductGUID = 'CF1418D6-3B09-4516-AE83-E39F2B91ED6D') --Flex
EXEC [Mapping].[ProductToSupplier_Insert] @CreatedByUserId, @SourceId, @ProductId, @SupplierId