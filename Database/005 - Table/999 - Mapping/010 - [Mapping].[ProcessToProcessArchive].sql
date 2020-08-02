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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[ProcessToProcessArchive]') AND type in (N'U'))
DROP TABLE [Mapping].[ProcessToProcessArchive]
GO
CREATE TABLE [Mapping].[ProcessToProcessArchive]
	(
	ProcessToProcessArchiveId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ProcessId BIGINT NOT NULL,
	ProcessArchiveId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[ProcessToProcessArchive] ADD CONSTRAINT
	PK_ProcessToProcessArchive PRIMARY KEY CLUSTERED 
	(
	ProcessToProcessArchiveId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[ProcessToProcessArchive] ADD CONSTRAINT
	DF_ProcessToProcessArchive_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[ProcessToProcessArchive] ADD CONSTRAINT
	DF_ProcessToProcessArchive_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[ProcessToProcessArchive] ADD CONSTRAINT
	DF_ProcessToProcessArchive_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[ProcessToProcessArchive] ADD CONSTRAINT
	FK_ProcessToProcessArchive_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ProcessToProcessArchive].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ProcessToProcessArchive', N'CONSTRAINT', N'FK_ProcessToProcessArchive_CreatedByUserId'
GO
ALTER TABLE [Mapping].[ProcessToProcessArchive] ADD CONSTRAINT
	FK_ProcessToProcessArchive_ProcessArchiveId FOREIGN KEY
	(
	ProcessArchiveId
	) REFERENCES [System].[ProcessArchive]
	(
	ProcessArchiveId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ProcessToProcessArchive].ProcessArchiveId to [System].[ProcessArchive].ProcessArchiveId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ProcessToProcessArchive', N'CONSTRAINT', N'FK_ProcessToProcessArchive_ProcessArchiveId'
GO
ALTER TABLE [Mapping].[ProcessToProcessArchive] ADD CONSTRAINT
	FK_ProcessToProcessArchive_ProcessId FOREIGN KEY
	(
	ProcessId
	) REFERENCES [System].[Process]
	(
	ProcessId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ProcessToProcessArchive].ProcessId to [System].[Process].ProcessId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ProcessToProcessArchive', N'CONSTRAINT', N'FK_ProcessToProcessArchive_ProcessId'
GO
ALTER TABLE [Mapping].[ProcessToProcessArchive] ADD CONSTRAINT
	FK_ProcessToProcessArchive_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[ProcessToProcessArchive].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'ProcessToProcessArchive', N'CONSTRAINT', N'FK_ProcessToProcessArchive_SourceId'
GO
ALTER TABLE [Mapping].[ProcessToProcessArchive] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
