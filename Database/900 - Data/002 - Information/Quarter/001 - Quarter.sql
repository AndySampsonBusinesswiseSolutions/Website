USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Information].[Quarter_Insert] @CreatedByUserId, @SourceId, 'Quarter 1'
EXEC [Information].[Quarter_Insert] @CreatedByUserId, @SourceId, 'Quarter 2'
EXEC [Information].[Quarter_Insert] @CreatedByUserId, @SourceId, 'Quarter 3'
EXEC [Information].[Quarter_Insert] @CreatedByUserId, @SourceId, 'Quarter 4'