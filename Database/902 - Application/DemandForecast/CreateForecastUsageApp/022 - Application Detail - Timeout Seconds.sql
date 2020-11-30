USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @ApplicationId BIGINT = (SELECT ApplicationId FROM [System].[Application] WHERE ApplicationGUID = 'F0664F98-0EE6-413B-9FD1-0D05226EC3DB')
DECLARE @ApplicationAttributeId BIGINT = (SELECT ApplicationAttributeId FROM [System].[ApplicationAttribute] WHERE ApplicationAttributeDescription = 'Timeout Seconds')

EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '600'
