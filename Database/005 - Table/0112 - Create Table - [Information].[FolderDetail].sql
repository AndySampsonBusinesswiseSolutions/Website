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
USE [EMaaS]
GO
CREATE TABLE [Information].[FolderDetail]
	(
	FolderDetailId BIGINT NOT NULL IDENTITY (1, 1),
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	FolderId BIGINT NOT NULL,
	FolderAttributeId BIGINT NOT NULL,
	FolderDetailDescription VARCHAR(255) NOT NULL
	)  ON [Information]
GO
ALTER TABLE [Information].[FolderDetail] ADD CONSTRAINT
	DF_FolderDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[FolderDetail] ADD CONSTRAINT
	DF_FolderDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[FolderDetail] ADD CONSTRAINT
	DF_FolderDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[FolderDetail] ADD CONSTRAINT
	PK_FolderDetail PRIMARY KEY CLUSTERED 
	(
	FolderDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]
GO
ALTER TABLE [Information].[FolderDetail] ADD CONSTRAINT
	FK_FolderDetail_FolderId FOREIGN KEY
	(
	FolderId
	) REFERENCES [Information].[Folder]
	(
	FolderId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[FolderDetail].SourceId to [Information].[Folder].FolderId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'FolderDetail', N'CONSTRAINT', N'FK_FolderDetail_FolderId'
GO
ALTER TABLE [Information].[FolderDetail] ADD CONSTRAINT
	FK_FolderDetail_FolderAttributeId FOREIGN KEY
	(
	FolderAttributeId
	) REFERENCES [Information].[FolderAttribute]
	(
	FolderAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[FolderDetail].FolderAttributeId to [Information].[FolderAttribute].FolderAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'FolderDetail', N'CONSTRAINT', N'FK_FolderDetail_FolderAttributeId'
GO
ALTER TABLE [Information].[FolderDetail] ADD CONSTRAINT
	FK_FolderDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[FolderDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'FolderDetail', N'CONSTRAINT', N'FK_FolderDetail_CreatedByUserId'
GO
ALTER TABLE [Information].[FolderDetail] ADD CONSTRAINT
	FK_FolderDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[FolderDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'FolderDetail', N'CONSTRAINT', N'FK_FolderDetail_SourceId'
GO
ALTER TABLE [Information].[FolderDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
