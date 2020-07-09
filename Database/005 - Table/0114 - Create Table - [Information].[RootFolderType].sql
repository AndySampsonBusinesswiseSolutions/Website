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
CREATE TABLE [Information].[RootFolderType]
	(
	RootFolderTypeId BIGINT NOT NULL IDENTITY (1, 1),
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	RootFolderTypeDescription VARCHAR(200) NOT NULL
	)  ON [Information]
GO
ALTER TABLE [Information].[RootFolderType] ADD CONSTRAINT
	DF_RootFolderType_EffectiveFromDateTime DEFAULT GETDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[RootFolderType] ADD CONSTRAINT
	DF_RootFolderType_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[RootFolderType] ADD CONSTRAINT
	DF_RootFolderType_CreatedDateTime DEFAULT GETDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[RootFolderType] ADD CONSTRAINT
	PK_RootFolderType PRIMARY KEY CLUSTERED 
	(
	RootFolderTypeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]
GO
ALTER TABLE [Information].[RootFolderType] ADD CONSTRAINT
	FK_RootFolderType_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[RootFolderType].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'RootFolderType', N'CONSTRAINT', N'FK_RootFolderType_CreatedByUserId'
GO
ALTER TABLE [Information].[RootFolderType] ADD CONSTRAINT
	FK_RootFolderType_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[RootFolderType].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'RootFolderType', N'CONSTRAINT', N'FK_RootFolderType_SourceId'
GO
ALTER TABLE [Information].[RootFolderType] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
