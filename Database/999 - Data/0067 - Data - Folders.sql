USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @FolderPathAttributeId BIGINT = (SELECT FolderAttributeId FROM [Information].[FolderAttribute] WHERE FolderAttributeDescription = 'Folder Path')
DECLARE @FolderTypeAttributeId BIGINT = (SELECT FolderAttributeId FROM [Information].[FolderAttribute] WHERE FolderAttributeDescription = 'Folder Type')

DECLARE @FolderGUID UNIQUEIDENTIFIER = 'FA701B83-CBDD-457F-9300-1112F503F5CF'
EXEC [Information].[Folder_Insert] @CreatedByUserId, @SourceId, @FolderGUID

DECLARE @FolderId BIGINT = (SELECT FolderId FROM [Information].[Folder] WHERE FolderGUID = @FolderGUID)
EXEC [Information].[FolderDetail_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderPathAttributeId, 'C:\Users\andy.sampson\Downloads\BWS Files\Energy Portal\Customer Files'
EXEC [Information].[FolderDetail_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderTypeAttributeId, 'Root Folder'

SET @FolderGUID = 'BB2666E4-D79A-4490-9EC2-1B57DD3D1945'
EXEC [Information].[Folder_Insert] @CreatedByUserId, @SourceId, @FolderGUID

SET @FolderId = (SELECT FolderId FROM [Information].[Folder] WHERE FolderGUID = @FolderGUID)
EXEC [Information].[FolderDetail_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderPathAttributeId, '\Usage Upload'
EXEC [Information].[FolderDetail_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderTypeAttributeId, 'Folder Extension'

SET @FolderGUID = 'FBB997A4-0C41-426D-8A32-7A4B7168DFCE'
EXEC [Information].[Folder_Insert] @CreatedByUserId, @SourceId, @FolderGUID

SET @FolderId = (SELECT FolderId FROM [Information].[Folder] WHERE FolderGUID = @FolderGUID)
EXEC [Information].[FolderDetail_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderPathAttributeId, '\Letter Of Authority'
EXEC [Information].[FolderDetail_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderTypeAttributeId, 'Folder Extension'

SET @FolderGUID = '794A95B4-1EA0-4ED1-931C-85A6EB247310'
EXEC [Information].[Folder_Insert] @CreatedByUserId, @SourceId, @FolderGUID

SET @FolderId = (SELECT FolderId FROM [Information].[Folder] WHERE FolderGUID = @FolderGUID)
EXEC [Information].[FolderDetail_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderPathAttributeId, '\Supplier Contract'
EXEC [Information].[FolderDetail_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderTypeAttributeId, 'Folder Extension'

SET @FolderGUID = 'B97F4898-C7DC-4F84-B98A-B6CCED6036C0'
EXEC [Information].[Folder_Insert] @CreatedByUserId, @SourceId, @FolderGUID

SET @FolderId = (SELECT FolderId FROM [Information].[Folder] WHERE FolderGUID = @FolderGUID)
EXEC [Information].[FolderDetail_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderPathAttributeId, '\EMaaS Contract'
EXEC [Information].[FolderDetail_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderTypeAttributeId, 'Folder Extension'

SET @FolderGUID = '9EF57AD3-7FD2-4534-9D4F-D7ACFD1389DB'
EXEC [Information].[Folder_Insert] @CreatedByUserId, @SourceId, @FolderGUID

SET @FolderId = (SELECT FolderId FROM [Information].[Folder] WHERE FolderGUID = @FolderGUID)
EXEC [Information].[FolderDetail_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderPathAttributeId, '\Flex Contract'
EXEC [Information].[FolderDetail_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderTypeAttributeId, 'Folder Extension'

SET @FolderGUID = '5D7A74E0-2E19-4529-8E72-3959D300BAC2'
EXEC [Information].[Folder_Insert] @CreatedByUserId, @SourceId, @FolderGUID

SET @FolderId = (SELECT FolderId FROM [Information].[Folder] WHERE FolderGUID = @FolderGUID)
EXEC [Information].[FolderDetail_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderPathAttributeId, '\Invoice'
EXEC [Information].[FolderDetail_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderTypeAttributeId, 'Folder Extension'

SET @FolderGUID = 'F8259358-A303-4B12-9A77-596F0126C414'
EXEC [Information].[Folder_Insert] @CreatedByUserId, @SourceId, @FolderGUID

SET @FolderId = (SELECT FolderId FROM [Information].[Folder] WHERE FolderGUID = @FolderGUID)
EXEC [Information].[FolderDetail_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderPathAttributeId, '\Supplier Bill'
EXEC [Information].[FolderDetail_Insert] @CreatedByUserId, @SourceId, @FolderId, @FolderTypeAttributeId, 'Folder Extension'