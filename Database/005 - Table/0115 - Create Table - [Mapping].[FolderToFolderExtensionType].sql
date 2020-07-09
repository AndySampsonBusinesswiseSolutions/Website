USE [EMaaS]
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[FolderToFolderExtensionType]') AND type in (N'U'))
DROP TABLE [Mapping].[FolderToFolderExtensionType]
GO
CREATE TABLE [Mapping].[FolderToFolderExtensionType]
	(
	FolderToFolderExtensionTypeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	FolderId BIGINT NOT NULL,
	FolderExtensionTypeId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[FolderToFolderExtensionType] ADD CONSTRAINT
	PK_FolderToFolderExtensionType PRIMARY KEY CLUSTERED 
	(
	FolderToFolderExtensionTypeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[FolderToFolderExtensionType] ADD CONSTRAINT
	DF_FolderToFolderExtensionType_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[FolderToFolderExtensionType] ADD CONSTRAINT
	DF_FolderToFolderExtensionType_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[FolderToFolderExtensionType] ADD CONSTRAINT
	DF_FolderToFolderExtensionType_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[FolderToFolderExtensionType] ADD CONSTRAINT
	FK_FolderToFolderExtensionType_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FolderToFolderExtensionType].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FolderToFolderExtensionType', N'CONSTRAINT', N'FK_FolderToFolderExtensionType_CreatedByUserId'
GO
ALTER TABLE [Mapping].[FolderToFolderExtensionType] ADD CONSTRAINT
	FK_FolderToFolderExtensionType_FolderExtensionTypeId FOREIGN KEY
	(
	FolderExtensionTypeId
	) REFERENCES [Information].[FolderExtensionType]
	(
	FolderExtensionTypeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FolderToFolderExtensionType].FolderExtensionTypeId to [Information].[FolderExtensionType].FolderExtensionTypeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FolderToFolderExtensionType', N'CONSTRAINT', N'FK_FolderToFolderExtensionType_FolderExtensionTypeId'
GO
ALTER TABLE [Mapping].[FolderToFolderExtensionType] ADD CONSTRAINT
	FK_FolderToFolderExtensionType_FolderId FOREIGN KEY
	(
	FolderId
	) REFERENCES [Information].[Folder]
	(
	FolderId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FolderToFolderExtensionType].FolderId to [Information].[Folder].FolderId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FolderToFolderExtensionType', N'CONSTRAINT', N'FK_FolderToFolderExtensionType_FolderId'
GO
ALTER TABLE [Mapping].[FolderToFolderExtensionType] ADD CONSTRAINT
	FK_FolderToFolderExtensionType_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FolderToFolderExtensionType].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FolderToFolderExtensionType', N'CONSTRAINT', N'FK_FolderToFolderExtensionType_SourceId'
GO
ALTER TABLE [Mapping].[FolderToFolderExtensionType] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
