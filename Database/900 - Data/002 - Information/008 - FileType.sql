USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Information].[FileType_Insert] @CreatedByUserId, @SourceId, 'Usage Upload';
EXEC [Information].[FileType_Insert] @CreatedByUserId, @SourceId, 'Letter Of Authority';
EXEC [Information].[FileType_Insert] @CreatedByUserId, @SourceId, 'Supplier Contract';
EXEC [Information].[FileType_Insert] @CreatedByUserId, @SourceId, 'EMaaS Contract';
EXEC [Information].[FileType_Insert] @CreatedByUserId, @SourceId, 'Flex Contract';
EXEC [Information].[FileType_Insert] @CreatedByUserId, @SourceId, 'Invoice';
EXEC [Information].[FileType_Insert] @CreatedByUserId, @SourceId, 'Supplier Bill';