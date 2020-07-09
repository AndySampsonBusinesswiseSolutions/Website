USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @FolderId BIGINT = (SELECT FolderId FROM [Information].[Folder] WHERE FolderGUID = 'FA701B83-CBDD-457F-9300-1112F503F5CF')
DECLARE @RootFolderTypeId BIGINT = (SELECT RootFolderTypeId FROM [Information].[RootFolderType] WHERE RootFolderTypeDescription = 'Customer Files')
EXEC [Mapping].[FolderToRootFolderType_Insert] @CreatedByUserId, @SourceId, @FolderId, @RootFolderTypeId