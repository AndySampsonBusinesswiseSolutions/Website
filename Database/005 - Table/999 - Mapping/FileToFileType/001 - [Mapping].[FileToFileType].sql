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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[FileToFileType]') AND type in (N'U'))
DROP TABLE [Mapping].[FileToFileType]
GO
CREATE TABLE [Mapping].[FileToFileType]
	(
	FileToFileTypeId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	FileId BIGINT NOT NULL,
	FileTypeId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[FileToFileType] ADD CONSTRAINT
	PK_FileToFileType PRIMARY KEY CLUSTERED 
	(
	FileToFileTypeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[FileToFileType] ADD CONSTRAINT
	DF_FileToFileType_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[FileToFileType] ADD CONSTRAINT
	DF_FileToFileType_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[FileToFileType] ADD CONSTRAINT
	DF_FileToFileType_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[FileToFileType] ADD CONSTRAINT
	FK_FileToFileType_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FileToFileType].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FileToFileType', N'CONSTRAINT', N'FK_FileToFileType_CreatedByUserId'
GO
ALTER TABLE [Mapping].[FileToFileType] ADD CONSTRAINT
	FK_FileToFileType_FileTypeId FOREIGN KEY
	(
	FileTypeId
	) REFERENCES [Information].[FileType]
	(
	FileTypeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FileToFileType].FileTypeId to [Information].[FileType].FileTypeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FileToFileType', N'CONSTRAINT', N'FK_FileToFileType_FileTypeId'
GO
ALTER TABLE [Mapping].[FileToFileType] ADD CONSTRAINT
	FK_FileToFileType_FileId FOREIGN KEY
	(
	FileId
	) REFERENCES [Information].[File]
	(
	FileId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FileToFileType].FileId to [Information].[File].FileId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FileToFileType', N'CONSTRAINT', N'FK_FileToFileType_FileId'
GO
ALTER TABLE [Mapping].[FileToFileType] ADD CONSTRAINT
	FK_FileToFileType_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FileToFileType].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FileToFileType', N'CONSTRAINT', N'FK_FileToFileType_SourceId'
GO
ALTER TABLE [Mapping].[FileToFileType] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
