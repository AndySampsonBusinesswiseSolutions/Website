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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[System].[ApplicationRun]') AND type in (N'U'))
DROP TABLE [System].[ApplicationRun]
GO
CREATE TABLE [System].[ApplicationRun]
	(
	ApplicationRunId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	ApplicationRunGUID UNIQUEIDENTIFIER NOT NULL,
	)  ON [System]
GO
ALTER TABLE [System].[ApplicationRun] ADD CONSTRAINT
	PK_ApplicationRun PRIMARY KEY CLUSTERED 
	(
	ApplicationRunId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [System]

GO
ALTER TABLE [System].[ApplicationRun] ADD CONSTRAINT
	DF_ApplicationRun_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [System].[ApplicationRun] ADD CONSTRAINT
	DF_ApplicationRun_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [System].[ApplicationRun] ADD CONSTRAINT
	DF_ApplicationRun_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [System].[ApplicationRun] ADD CONSTRAINT
	FK_ApplicationRun_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ApplicationRun].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ApplicationRun', N'CONSTRAINT', N'FK_ApplicationRun_CreatedByUserId'
GO
ALTER TABLE [System].[ApplicationRun] ADD CONSTRAINT
	FK_ApplicationRun_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[ApplicationRun].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'ApplicationRun', N'CONSTRAINT', N'FK_ApplicationRun_SourceId'
GO
ALTER TABLE [System].[ApplicationRun] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
