USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Information].[OrdinalNumber_Insert] @CreatedByUserId, @SourceId, '1st'
EXEC [Information].[OrdinalNumber_Insert] @CreatedByUserId, @SourceId, '2nd'
EXEC [Information].[OrdinalNumber_Insert] @CreatedByUserId, @SourceId, '3rd'
EXEC [Information].[OrdinalNumber_Insert] @CreatedByUserId, @SourceId, '4th'
EXEC [Information].[OrdinalNumber_Insert] @CreatedByUserId, @SourceId, '5th'