USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId)
DECLARE @PageId BIGINT = (SELECT PageId FROM [System].[Page] WHERE PageGUID = '6641A1BF-84C8-48F8-9D79-70D0AB2BB787')
DECLARE @PageAttributeId BIGINT = (SELECT PageAttributeId FROM [System].[PageAttribute] WHERE PageAttributeDescription = 'Page Name')

EXEC [System].[PageDetail_Insert] 
    @CreatedByUserId, 
    @SourceId,
    @PageId,
    @PageAttributeId,
    'Login'