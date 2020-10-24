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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[System].[ProcessQueue]') AND type in (N'U'))
DROP TABLE [System].[ProcessQueue]
GO
CREATE TABLE [System].[ProcessQueue]
	(
	ProcessQueueGUID UNIQUEIDENTIFIER NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	APIId BIGINT NOT NULL,
	HasError BIT NOT NULL
	)  ON [System]
GO
ALTER TABLE [System].[ProcessQueue] ADD CONSTRAINT
	DF_ProcessQueue_EffectiveFromDateTime DEFAULT '9999-12-31' FOR EffectiveFromDateTime
GO
ALTER TABLE [System].[ProcessQueue] ADD CONSTRAINT
	DF_ProcessQueue_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [System].[ProcessQueue] ADD CONSTRAINT
	DF_ProcessQueue_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [System].[ProcessQueue] ADD CONSTRAINT
	DF_ProcessQueue_HasError DEFAULT 0 FOR HasError
GO
ALTER TABLE [System].[ProcessQueue] ADD CONSTRAINT
	FK_ProcessQueue_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ProcessQueue].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ProcessQueue', N'CONSTRAINT', N'FK_ProcessQueue_CreatedByUserId'
GO
ALTER TABLE [System].[ProcessQueue] ADD CONSTRAINT
	FK_ProcessQueue_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ProcessQueue].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ProcessQueue', N'CONSTRAINT', N'FK_ProcessQueue_SourceId'
GO
ALTER TABLE [System].[ProcessQueue] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
