USE [EMaaS]
GO

DECLARE @ApplicationGUID UNIQUEIDENTIFIER = 'EB061C46-A893-4C44-9EB7-E9D5CF6B0895'
DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [System].[Application_Insert] @CreatedByUserId, @SourceId, @ApplicationGUID
