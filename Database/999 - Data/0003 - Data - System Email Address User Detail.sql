USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @UserAttributeId BIGINT = (SELECT UserAttributeId FROM [Administration.User].[UserAttribute] WHERE UserAttributeDescription = 'Email Address')

EXEC [Administration.User].[UserDetail_Insert] 
    @CreatedByUserId, 
    @SourceId, 
    @CreatedByUserId, 
    @UserAttributeId, 
    'andy.sampson@businesswisesolutions.co.uk'