USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId)
DECLARE @UserAttributeId BIGINT = (SELECT UserAttributeId FROM [Administration.User].[UserAttribute] WHERE UserAttributeDescription = 'User Name')

EXEC [Administration.User].[UserDetail_Insert] 
    @CreatedByUserId, 
    @SourceId, 
    @CreatedByUserId, 
    @UserAttributeId, 
    'System'