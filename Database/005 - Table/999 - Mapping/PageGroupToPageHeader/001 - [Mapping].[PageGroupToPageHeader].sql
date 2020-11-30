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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[PageGroupToPageHeader]') AND type in (N'U'))
DROP TABLE [Mapping].[PageGroupToPageHeader]
GO
CREATE TABLE [Mapping].[PageGroupToPageHeader]
	(
	PageGroupToPageHeaderId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	PageGroupId BIGINT NOT NULL,
	PageHeaderId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[PageGroupToPageHeader] ADD CONSTRAINT
	PK_PageGroupToPageHeader PRIMARY KEY CLUSTERED 
	(
	PageGroupToPageHeaderId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[PageGroupToPageHeader] ADD CONSTRAINT
	DF_PageGroupToPageHeader_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[PageGroupToPageHeader] ADD CONSTRAINT
	DF_PageGroupToPageHeader_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[PageGroupToPageHeader] ADD CONSTRAINT
	DF_PageGroupToPageHeader_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[PageGroupToPageHeader] ADD CONSTRAINT
	FK_PageGroupToPageHeader_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PageGroupToPageHeader].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PageGroupToPageHeader', N'CONSTRAINT', N'FK_PageGroupToPageHeader_CreatedByUserId'
GO
ALTER TABLE [Mapping].[PageGroupToPageHeader] ADD CONSTRAINT
	FK_PageGroupToPageHeader_PageHeaderId FOREIGN KEY
	(
	PageHeaderId
	) REFERENCES [System].[PageHeader]
	(
	PageHeaderId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PageGroupToPageHeader].PageHeaderId to [System].[PageHeader].PageHeaderId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PageGroupToPageHeader', N'CONSTRAINT', N'FK_PageGroupToPageHeader_PageHeaderId'
GO
ALTER TABLE [Mapping].[PageGroupToPageHeader] ADD CONSTRAINT
	FK_PageGroupToPageHeader_PageGroupId FOREIGN KEY
	(
	PageGroupId
	) REFERENCES [System].[PageGroup]
	(
	PageGroupId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PageGroupToPageHeader].PageGroupId to [System].[PageGroup].PageGroupId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PageGroupToPageHeader', N'CONSTRAINT', N'FK_PageGroupToPageHeader_PageGroupId'
GO
ALTER TABLE [Mapping].[PageGroupToPageHeader] ADD CONSTRAINT
	FK_PageGroupToPageHeader_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PageGroupToPageHeader].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PageGroupToPageHeader', N'CONSTRAINT', N'FK_PageGroupToPageHeader_SourceId'
GO
ALTER TABLE [Mapping].[PageGroupToPageHeader] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
