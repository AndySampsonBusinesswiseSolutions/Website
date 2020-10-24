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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[System].[PageGroup]') AND type in (N'U'))
DROP TABLE [System].[PageGroup]
GO
CREATE TABLE [System].[PageGroup]
	(
	PageGroupId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	PageGroupGUID UNIQUEIDENTIFIER NOT NULL,
	)  ON [System]
GO
ALTER TABLE [System].[PageGroup] ADD CONSTRAINT
	PK_PageGroup PRIMARY KEY CLUSTERED 
	(
	PageGroupId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [System]

GO
ALTER TABLE [System].[PageGroup] ADD CONSTRAINT
	DF_PageGroup_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [System].[PageGroup] ADD CONSTRAINT
	DF_PageGroup_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [System].[PageGroup] ADD CONSTRAINT
	DF_PageGroup_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [System].[PageGroup] ADD CONSTRAINT
	FK_PageGroup_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[PageGroup].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'PageGroup', N'CONSTRAINT', N'FK_PageGroup_CreatedByUserId'
GO
ALTER TABLE [System].[PageGroup] ADD CONSTRAINT
	FK_PageGroup_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[PageGroup].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'PageGroup', N'CONSTRAINT', N'FK_PageGroup_SourceId'
GO
ALTER TABLE [System].[PageGroup] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
