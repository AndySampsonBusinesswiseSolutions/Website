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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[System].[PageHeader]') AND type in (N'U'))
DROP TABLE [System].[PageHeader]
GO
CREATE TABLE [System].[PageHeader]
	(
	PageHeaderId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	PageHeaderGUID UNIQUEIDENTIFIER NOT NULL,
	)  ON [System]
GO
ALTER TABLE [System].[PageHeader] ADD CONSTRAINT
	PK_PageHeader PRIMARY KEY CLUSTERED 
	(
	PageHeaderId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [System]

GO
ALTER TABLE [System].[PageHeader] ADD CONSTRAINT
	DF_PageHeader_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [System].[PageHeader] ADD CONSTRAINT
	DF_PageHeader_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [System].[PageHeader] ADD CONSTRAINT
	DF_PageHeader_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [System].[PageHeader] ADD CONSTRAINT
	FK_PageHeader_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[PageHeader].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'PageHeader', N'CONSTRAINT', N'FK_PageHeader_CreatedByUserId'
GO
ALTER TABLE [System].[PageHeader] ADD CONSTRAINT
	FK_PageHeader_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[PageHeader].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'PageHeader', N'CONSTRAINT', N'FK_PageHeader_SourceId'
GO
ALTER TABLE [System].[PageHeader] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
