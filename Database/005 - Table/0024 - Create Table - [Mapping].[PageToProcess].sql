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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[PageToProcess]') AND type in (N'U'))
DROP TABLE [Mapping].[PageToProcess]
GO
CREATE TABLE [Mapping].[PageToProcess]
	(
	PageToProcessId bigint IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime datetime NOT NULL,
	EffectiveToDateTime datetime NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedByUserId bigint NOT NULL,
	SourceId bigint NOT NULL,
	PageId bigint NOT NULL,
	ProcessId bigint NOT NULL
	)  ON Mapping
GO
ALTER TABLE [Mapping].[PageToProcess] ADD CONSTRAINT
	PK_PageToProcess PRIMARY KEY CLUSTERED 
	(
	PageToProcessId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Mapping

GO
ALTER TABLE [Mapping].[PageToProcess] ADD CONSTRAINT
	DF_PageToProcess_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[PageToProcess] ADD CONSTRAINT
	DF_PageToProcess_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[PageToProcess] ADD CONSTRAINT
	DF_PageToProcess_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[PageToProcess] ADD CONSTRAINT
	FK_PageToProcess_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PageToProcess].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PageToProcess', N'CONSTRAINT', N'FK_PageToProcess_CreatedByUserId'
GO
ALTER TABLE [Mapping].[PageToProcess] ADD CONSTRAINT
	FK_PageToProcess_ProcessId FOREIGN KEY
	(
	ProcessId
	) REFERENCES [System].[Process]
	(
	ProcessId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PageToProcess].ProcessId to [Administration.Process].[Process].ProcessId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PageToProcess', N'CONSTRAINT', N'FK_PageToProcess_ProcessId'
GO
ALTER TABLE [Mapping].[PageToProcess] ADD CONSTRAINT
	FK_PageToProcess_PageId FOREIGN KEY
	(
	PageId
	) REFERENCES [System].[Page]
	(
	PageId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PageToProcess].PageId to [System].[Page].PageId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PageToProcess', N'CONSTRAINT', N'FK_PageToProcess_PageId'
GO
ALTER TABLE [Mapping].[PageToProcess] ADD CONSTRAINT
	FK_PageToProcess_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PageToProcess].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PageToProcess', N'CONSTRAINT', N'FK_PageToProcess_SourceId'
GO
ALTER TABLE [Mapping].[PageToProcess] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
