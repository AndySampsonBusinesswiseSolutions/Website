USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @FolderId BIGINT = (SELECT FolderId FROM [Information].[Folder] WHERE FolderGUID = 'FA701B83-CBDD-457F-9300-1112F503F5CF')

DECLARE @FolderExtensionId BIGINT = (SELECT FolderId FROM [Information].[Folder] WHERE FolderGUID = 'BB2666E4-D79A-4490-9EC2-1B57DD3D1945')
EXEC [Mapping].[FolderToFolderExtension_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderExtensionId

SET @FolderExtensionId = (SELECT FolderId FROM [Information].[Folder] WHERE FolderGUID = 'FBB997A4-0C41-426D-8A32-7A4B7168DFCE')
EXEC [Mapping].[FolderToFolderExtension_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderExtensionId

SET @FolderExtensionId = (SELECT FolderId FROM [Information].[Folder] WHERE FolderGUID = '794A95B4-1EA0-4ED1-931C-85A6EB247310')
EXEC [Mapping].[FolderToFolderExtension_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderExtensionId

SET @FolderExtensionId = (SELECT FolderId FROM [Information].[Folder] WHERE FolderGUID = 'B97F4898-C7DC-4F84-B98A-B6CCED6036C0')
EXEC [Mapping].[FolderToFolderExtension_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderExtensionId

SET @FolderExtensionId = (SELECT FolderId FROM [Information].[Folder] WHERE FolderGUID = '9EF57AD3-7FD2-4534-9D4F-D7ACFD1389DB')
EXEC [Mapping].[FolderToFolderExtension_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderExtensionId

SET @FolderExtensionId = (SELECT FolderId FROM [Information].[Folder] WHERE FolderGUID = '5D7A74E0-2E19-4529-8E72-3959D300BAC2')
EXEC [Mapping].[FolderToFolderExtension_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderExtensionId

SET @FolderExtensionId = (SELECT FolderId FROM [Information].[Folder] WHERE FolderGUID = 'F8259358-A303-4B12-9A77-596F0126C414')
EXEC [Mapping].[FolderToFolderExtension_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderExtensionId