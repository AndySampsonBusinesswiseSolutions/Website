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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[System].[ProcessArchiveDetail]') AND type in (N'U'))
DROP TABLE [System].[ProcessArchiveDetail]
GO
CREATE TABLE [System].[ProcessArchiveDetail]
	(
	ProcessArchiveDetailId bigint IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime datetime NOT NULL,
	EffectiveToDateTime datetime NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedByUserId bigint NOT NULL,
	SourceId bigint NOT NULL,
	ProcessArchiveId bigint NOT NULL,
	ProcessArchiveAttributeId bigint NOT NULL,
	ProcessArchiveDetailDescription varchar(200) NOT NULL
	)  ON [System]
GO
ALTER TABLE [System].[ProcessArchiveDetail] ADD CONSTRAINT
	PK_ProcessArchiveDetail PRIMARY KEY CLUSTERED 
	(
	ProcessArchiveDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [System]

GO
ALTER TABLE [System].[ProcessArchiveDetail] ADD CONSTRAINT
	DF_ProcessArchiveDetail_EffectiveFromDateTime DEFAULT GETDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [System].[ProcessArchiveDetail] ADD CONSTRAINT
	DF_ProcessArchiveDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [System].[ProcessArchiveDetail] ADD CONSTRAINT
	DF_ProcessArchiveDetail_CreatedDateTime DEFAULT GETDATE() FOR CreatedDateTime
GO
ALTER TABLE [System].[ProcessArchiveDetail] ADD CONSTRAINT
	FK_ProcessArchiveDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ProcessArchiveDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ProcessArchiveDetail', N'CONSTRAINT', N'FK_ProcessArchiveDetail_CreatedByUserId'
GO
ALTER TABLE [System].[ProcessArchiveDetail] ADD CONSTRAINT
	FK_ProcessArchiveDetail_ProcessArchiveId FOREIGN KEY
	(
	ProcessArchiveId
	) REFERENCES [System].[ProcessArchive]
	(
	ProcessArchiveId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ProcessArchiveDetail].ProcessArchiveId to [System].[ProcessArchive].ProcessArchiveId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ProcessArchiveDetail', N'CONSTRAINT', N'FK_ProcessArchiveDetail_ProcessArchiveId'
GO
ALTER TABLE [System].[ProcessArchiveDetail] ADD CONSTRAINT
	FK_ProcessArchiveDetail_ProcessArchiveAttributeId FOREIGN KEY
	(
	ProcessArchiveAttributeId
	) REFERENCES [System].[ProcessArchiveAttribute]
	(
	ProcessArchiveAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ProcessArchiveDetail].ProcessArchiveAttributeId to [System].[ProcessArchiveAttribute].ProcessArchiveAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ProcessArchiveDetail', N'CONSTRAINT', N'FK_ProcessArchiveDetail_ProcessArchiveAttributeId'
GO
ALTER TABLE [System].[ProcessArchiveDetail] ADD CONSTRAINT
	FK_ProcessArchiveDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ProcessArchiveDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ProcessArchiveDetail', N'CONSTRAINT', N'FK_ProcessArchiveDetail_SourceId'
GO
ALTER TABLE [System].[ProcessArchiveDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
