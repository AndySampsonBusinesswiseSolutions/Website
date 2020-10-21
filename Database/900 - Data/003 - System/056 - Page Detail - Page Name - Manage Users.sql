USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @PageId BIGINT = (SELECT PageId FROM [System].[Page] WHERE PageGUID = 'B974CFB4-AE27-4A72-A1D7-927B84C5CB62')
DECLARE @PageAttributeId BIGINT = (SELECT PageAttributeId FROM [System].[PageAttribute] WHERE PageAttributeDescription = 'Page Name')

EXEC [System].[PageDetail_Insert] 
    @CreatedByUserId, 
    @SourceId,
    @PageId,
    @PageAttributeId,
    'Manage Users'