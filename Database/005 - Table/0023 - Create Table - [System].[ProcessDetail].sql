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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[System].[ProcessDetail]') AND type in (N'U'))
DROP TABLE [System].[ProcessDetail]
GO
CREATE TABLE [System].[ProcessDetail]
	(
	ProcessDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ProcessId BIGINT NOT NULL,
	ProcessAttributeId BIGINT NOT NULL,
	ProcessDetailDescription VARCHAR(200) NOT NULL
	)  ON [System]
GO
ALTER TABLE [System].[ProcessDetail] ADD CONSTRAINT
	PK_ProcessDetail PRIMARY KEY CLUSTERED 
	(
	ProcessDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [System]

GO
ALTER TABLE [System].[ProcessDetail] ADD CONSTRAINT
	DF_ProcessDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [System].[ProcessDetail] ADD CONSTRAINT
	DF_ProcessDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [System].[ProcessDetail] ADD CONSTRAINT
	DF_ProcessDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [System].[ProcessDetail] ADD CONSTRAINT
	FK_ProcessDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ProcessDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ProcessDetail', N'CONSTRAINT', N'FK_ProcessDetail_CreatedByUserId'
GO
ALTER TABLE [System].[ProcessDetail] ADD CONSTRAINT
	FK_ProcessDetail_ProcessId FOREIGN KEY
	(
	ProcessId
	) REFERENCES [System].[Process]
	(
	ProcessId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ProcessDetail].ProcessId to [System].[Process].ProcessId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ProcessDetail', N'CONSTRAINT', N'FK_ProcessDetail_ProcessId'
GO
ALTER TABLE [System].[ProcessDetail] ADD CONSTRAINT
	FK_ProcessDetail_ProcessAttributeId FOREIGN KEY
	(
	ProcessAttributeId
	) REFERENCES [System].[ProcessAttribute]
	(
	ProcessAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ProcessDetail].ProcessAttributeId to [System].[ProcessAttribute].ProcessAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ProcessDetail', N'CONSTRAINT', N'FK_ProcessDetail_ProcessAttributeId'
GO
ALTER TABLE [System].[ProcessDetail] ADD CONSTRAINT
	FK_ProcessDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ProcessDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ProcessDetail', N'CONSTRAINT', N'FK_ProcessDetail_SourceId'
GO
ALTER TABLE [System].[ProcessDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
