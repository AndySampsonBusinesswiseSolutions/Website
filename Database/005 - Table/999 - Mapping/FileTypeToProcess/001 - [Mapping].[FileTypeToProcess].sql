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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[FileTypeToProcess]') AND type in (N'U'))
DROP TABLE [Mapping].[FileTypeToProcess]
GO
CREATE TABLE [Mapping].[FileTypeToProcess]
	(
	FileTypeToProcessId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	FileTypeId BIGINT NOT NULL,
	ProcessId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[FileTypeToProcess] ADD CONSTRAINT
	PK_FileTypeToProcess PRIMARY KEY CLUSTERED 
	(
	FileTypeToProcessId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[FileTypeToProcess] ADD CONSTRAINT
	DF_FileTypeToProcess_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[FileTypeToProcess] ADD CONSTRAINT
	DF_FileTypeToProcess_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[FileTypeToProcess] ADD CONSTRAINT
	DF_FileTypeToProcess_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[FileTypeToProcess] ADD CONSTRAINT
	FK_FileTypeToProcess_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FileTypeToProcess].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FileTypeToProcess', N'CONSTRAINT', N'FK_FileTypeToProcess_CreatedByUserId'
GO
ALTER TABLE [Mapping].[FileTypeToProcess] ADD CONSTRAINT
	FK_FileTypeToProcess_ProcessId FOREIGN KEY
	(
	ProcessId
	) REFERENCES [System].[Process]
	(
	ProcessId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FileTypeToProcess].ProcessId to [System].[Process].ProcessId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FileTypeToProcess', N'CONSTRAINT', N'FK_FileTypeToProcess_ProcessId'
GO
ALTER TABLE [Mapping].[FileTypeToProcess] ADD CONSTRAINT
	FK_FileTypeToProcess_FileTypeId FOREIGN KEY
	(
	FileTypeId
	) REFERENCES [Information].[FileType]
	(
	FileTypeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FileTypeToProcess].FileTypeId to [Information].[FileType].FileTypeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FileTypeToProcess', N'CONSTRAINT', N'FK_FileTypeToProcess_FileTypeId'
GO
ALTER TABLE [Mapping].[FileTypeToProcess] ADD CONSTRAINT
	FK_FileTypeToProcess_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[FileTypeToProcess].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'FileTypeToProcess', N'CONSTRAINT', N'FK_FileTypeToProcess_SourceId'
GO
ALTER TABLE [Mapping].[FileTypeToProcess] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
