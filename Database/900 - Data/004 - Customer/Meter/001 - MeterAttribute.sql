USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Customer].[MeterAttribute_Insert] @CreatedByUserId, @SourceId, 'Meter Identifier'
EXEC [Customer].[MeterAttribute_Insert] @CreatedByUserId, @SourceId, 'Supply Capacity'
EXEC [Customer].[MeterAttribute_Insert] @CreatedByUserId, @SourceId, 'Standard Offtake Quantity'
EXEC [Customer].[MeterAttribute_Insert] @CreatedByUserId, @SourceId, 'Annual Usage'
EXEC [Customer].[MeterAttribute_Insert] @CreatedByUserId, @SourceId, 'Meter Serial Number'
EXEC [Customer].[MeterAttribute_Insert] @CreatedByUserId, @SourceId, 'Import/Export'