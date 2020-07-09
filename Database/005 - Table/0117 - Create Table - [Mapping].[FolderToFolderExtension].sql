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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[FolderToFolderExtension]') AND type in (N'U'))
DROP TABLE [Mapping].[FolderToFolderExtension]
GO
CREATE TABLE [Mapping].[FolderToFolderExtension]
	(
	FolderToFolderExtensionId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	FolderId BIGINT NOT NULL,
	FolderExtensionId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[FolderToFolderExtension] ADD CONSTRAINT
	PK_FolderToFolderExtension PRIMARY KEY CLUSTERED 
	(
	FolderToFolderExtensionId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[FolderToFolderExtension] ADD CONSTRAINT
	DF_FolderToFolderExtension_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[FolderToFolderExtension] ADD CONSTRAINT
	DF_FolderToFolderExtension_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[FolderToFolderExtension] ADD CONSTRAINT
	DF_FolderToFolderExtension_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[FolderToFolderExtension] ADD CONSTRAINT
	FK_FolderToFolderExtension_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FolderToFolderExtension].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FolderToFolderExtension', N'CONSTRAINT', N'FK_FolderToFolderExtension_CreatedByUserId'
GO
ALTER TABLE [Mapping].[FolderToFolderExtension] ADD CONSTRAINT
	FK_FolderToFolderExtension_FolderExtensionId FOREIGN KEY
	(
	FolderExtensionId
	) REFERENCES [Information].[Folder]
	(
	FolderId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FolderToFolderExtension].FolderExtensionId to [Information].[Folder].FolderId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FolderToFolderExtension', N'CONSTRAINT', N'FK_FolderToFolderExtension_FolderExtensionId'
GO
ALTER TABLE [Mapping].[FolderToFolderExtension] ADD CONSTRAINT
	FK_FolderToFolderExtension_FolderId FOREIGN KEY
	(
	FolderId
	) REFERENCES [Information].[Folder]
	(
	FolderId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FolderToFolderExtension].FolderId to [Information].[Folder].FolderId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FolderToFolderExtension', N'CONSTRAINT', N'FK_FolderToFolderExtension_FolderId'
GO
ALTER TABLE [Mapping].[FolderToFolderExtension] ADD CONSTRAINT
	FK_FolderToFolderExtension_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FolderToFolderExtension].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FolderToFolderExtension', N'CONSTRAINT', N'FK_FolderToFolderExtension_SourceId'
GO
ALTER TABLE [Mapping].[FolderToFolderExtension] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
