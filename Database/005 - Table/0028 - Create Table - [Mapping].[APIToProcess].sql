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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[APIToProcess]') AND type in (N'U'))
DROP TABLE [Mapping].[APIToProcess]
GO
CREATE TABLE [Mapping].[APIToProcess]
	(
	APIToProcessId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	APIId BIGINT NOT NULL,
	ProcessId BIGINT NOT NULL
	)  ON Mapping
GO
ALTER TABLE [Mapping].[APIToProcess] ADD CONSTRAINT
	PK_APIToProcess PRIMARY KEY CLUSTERED 
	(
	APIToProcessId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Mapping

GO
ALTER TABLE [Mapping].[APIToProcess] ADD CONSTRAINT
	DF_APIToProcess_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[APIToProcess] ADD CONSTRAINT
	DF_APIToProcess_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[APIToProcess] ADD CONSTRAINT
	DF_APIToProcess_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[APIToProcess] ADD CONSTRAINT
	FK_APIToProcess_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[APIToProcess].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'APIToProcess', N'CONSTRAINT', N'FK_APIToProcess_CreatedByUserId'
GO
ALTER TABLE [Mapping].[APIToProcess] ADD CONSTRAINT
	FK_APIToProcess_ProcessId FOREIGN KEY
	(
	ProcessId
	) REFERENCES [System].[Process]
	(
	ProcessId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[APIToProcess].ProcessId to [System].[Process].ProcessId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'APIToProcess', N'CONSTRAINT', N'FK_APIToProcess_ProcessId'
GO
ALTER TABLE [Mapping].[APIToProcess] ADD CONSTRAINT
	FK_APIToProcess_APIId FOREIGN KEY
	(
	APIId
	) REFERENCES [System].[API]
	(
	APIId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[APIToProcess].APIId to [System].[API].APIId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'APIToProcess', N'CONSTRAINT', N'FK_APIToProcess_APIId'
GO
ALTER TABLE [Mapping].[APIToProcess] ADD CONSTRAINT
	FK_APIToProcess_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[APIToProcess].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'APIToProcess', N'CONSTRAINT', N'FK_APIToProcess_SourceId'
GO
ALTER TABLE [Mapping].[APIToProcess] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
