USE [EMaaS]
GO

DECLARE @ApplicationGUID UNIQUEIDENTIFIER = '9EDA4294-4FF7-45C3-8FD1-1CD50BD4B7C3'
DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [System].[Application_Insert] @CreatedByUserId, @SourceId, @ApplicationGUID
