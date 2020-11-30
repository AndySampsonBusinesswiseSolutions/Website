USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @ApplicationId BIGINT = (SELECT ApplicationId FROM [System].[Application] WHERE ApplicationGUID = '0AF991E7-EE90-4CB0-AD03-CE57F45450EF')
DECLARE @ApplicationAttributeId BIGINT = (SELECT ApplicationAttributeId FROM [System].[ApplicationAttribute] WHERE ApplicationAttributeDescription = 'Required Data Key')

EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, 'ProcessQueueGUID'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, 'FileGUID'

