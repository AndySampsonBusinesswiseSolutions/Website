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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[System].[ApplicationRunDetail]') AND type in (N'U'))
DROP TABLE [System].[ApplicationRunDetail]
GO
CREATE TABLE [System].[ApplicationRunDetail]
	(
	ApplicationRunDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ApplicationRunId BIGINT NOT NULL,
	ApplicationRunAttributeId BIGINT NOT NULL,
	ApplicationRunDetailDescription VARCHAR(255) NOT NULL
	)  ON [System]
GO
ALTER TABLE [System].[ApplicationRunDetail] ADD CONSTRAINT
	PK_ApplicationRunDetail PRIMARY KEY CLUSTERED 
	(
	ApplicationRunDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [System]

GO
ALTER TABLE [System].[ApplicationRunDetail] ADD CONSTRAINT
	DF_ApplicationRunDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [System].[ApplicationRunDetail] ADD CONSTRAINT
	DF_ApplicationRunDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [System].[ApplicationRunDetail] ADD CONSTRAINT
	DF_ApplicationRunDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [System].[ApplicationRunDetail] ADD CONSTRAINT
	FK_ApplicationRunDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ApplicationRunDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ApplicationRunDetail', N'CONSTRAINT', N'FK_ApplicationRunDetail_CreatedByUserId'
GO
ALTER TABLE [System].[ApplicationRunDetail] ADD CONSTRAINT
	FK_ApplicationRunDetail_ApplicationRunId FOREIGN KEY
	(
	ApplicationRunId
	) REFERENCES [System].[ApplicationRun]
	(
	ApplicationRunId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ApplicationRunDetail].ApplicationRunId to [System].[ApplicationRun].ApplicationRunId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ApplicationRunDetail', N'CONSTRAINT', N'FK_ApplicationRunDetail_ApplicationRunId'
GO
ALTER TABLE [System].[ApplicationRunDetail] ADD CONSTRAINT
	FK_ApplicationRunDetail_ApplicationRunAttributeId FOREIGN KEY
	(
	ApplicationRunAttributeId
	) REFERENCES [System].[ApplicationRunAttribute]
	(
	ApplicationRunAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ApplicationRunDetail].ApplicationRunAttributeId to [System].[ApplicationRunAttribute].ApplicationRunAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ApplicationRunDetail', N'CONSTRAINT', N'FK_ApplicationRunDetail_ApplicationRunAttributeId'
GO
ALTER TABLE [System].[ApplicationRunDetail] ADD CONSTRAINT
	FK_ApplicationRunDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ApplicationRunDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ApplicationRunDetail', N'CONSTRAINT', N'FK_ApplicationRunDetail_SourceId'
GO
ALTER TABLE [System].[ApplicationRunDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
