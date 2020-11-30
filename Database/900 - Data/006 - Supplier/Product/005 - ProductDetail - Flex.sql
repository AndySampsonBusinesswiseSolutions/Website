USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @ProductId BIGINT = (SELECT ProductId FROM [Supplier].[Product] WHERE ProductGUID = 'CF1418D6-3B09-4516-AE83-E39F2B91ED6D')
DECLARE @ProductNameProductAttributeId BIGINT = (SELECT ProductAttributeId FROM [Supplier].[ProductAttribute] WHERE ProductAttributeDescription = 'Product Name')

EXEC [Supplier].[ProductDetail_Insert] @CreatedByUserId, @SourceId, @ProductId, @ProductNameProductAttributeId, 'Flex'