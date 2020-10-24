USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @PageHeaderAttributeId BIGINT = (SELECT PageHeaderAttributeId FROM [System].[PageHeaderAttribute] WHERE PageHeaderAttributeDescription = 'PageHeader Name')

DECLARE @PageHeaderId BIGINT = (SELECT PageHeaderId FROM [System].[PageHeader] WHERE PageHeaderGUID = 'EB7B2CCF-B1C7-46D5-A4E9-9068D04E1E59') --Standard
EXEC [System].[PageHeaderDetail_Insert] @CreatedByUserId, @SourceId, @PageHeaderId, @PageHeaderAttributeId, 'Standard'