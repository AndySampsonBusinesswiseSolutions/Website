USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @PageGroupAttributeId BIGINT = (SELECT PageGroupAttributeId FROM [System].[PageGroupAttribute] WHERE PageGroupAttributeDescription = 'Is Internal PageGroup')

DECLARE @PageGroupId BIGINT = (SELECT PageGroupId FROM [System].[PageGroup] WHERE PageGroupGUID = '85703311-C99D-446D-9007-E8A2207A7F37') --Supplier Management
EXEC [System].[PageGroupDetail_Insert] @CreatedByUserId, @SourceId, @PageGroupId, @PageGroupAttributeId, True