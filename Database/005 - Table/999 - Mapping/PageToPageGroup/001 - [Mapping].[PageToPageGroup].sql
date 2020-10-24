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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[PageToPageGroup]') AND type in (N'U'))
DROP TABLE [Mapping].[PageToPageGroup]
GO
CREATE TABLE [Mapping].[PageToPageGroup]
	(
	PageToPageGroupId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	PageId BIGINT NOT NULL,
	PageGroupId BIGINT NOT NULL
	)  ON [Mapping]
GO
ALTER TABLE [Mapping].[PageToPageGroup] ADD CONSTRAINT
	PK_PageToPageGroup PRIMARY KEY CLUSTERED 
	(
	PageToPageGroupId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Mapping]

GO
ALTER TABLE [Mapping].[PageToPageGroup] ADD CONSTRAINT
	DF_PageToPageGroup_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[PageToPageGroup] ADD CONSTRAINT
	DF_PageToPageGroup_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[PageToPageGroup] ADD CONSTRAINT
	DF_PageToPageGroup_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[PageToPageGroup] ADD CONSTRAINT
	FK_PageToPageGroup_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PageToPageGroup].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PageToPageGroup', N'CONSTRAINT', N'FK_PageToPageGroup_CreatedByUserId'
GO
ALTER TABLE [Mapping].[PageToPageGroup] ADD CONSTRAINT
	FK_PageToPageGroup_PageGroupId FOREIGN KEY
	(
	PageGroupId
	) REFERENCES [System].[PageGroup]
	(
	PageGroupId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PageToPageGroup].PageGroupId to [System].[PageGroup].PageGroupId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PageToPageGroup', N'CONSTRAINT', N'FK_PageToPageGroup_PageGroupId'
GO
ALTER TABLE [Mapping].[PageToPageGroup] ADD CONSTRAINT
	FK_PageToPageGroup_PageId FOREIGN KEY
	(
	PageId
	) REFERENCES [System].[Page]
	(
	PageId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PageToPageGroup].PageId to [System].[Page].PageId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PageToPageGroup', N'CONSTRAINT', N'FK_PageToPageGroup_PageId'
GO
ALTER TABLE [Mapping].[PageToPageGroup] ADD CONSTRAINT
	FK_PageToPageGroup_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[PageToPageGroup].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'PageToPageGroup', N'CONSTRAINT', N'FK_PageToPageGroup_SourceId'
GO
ALTER TABLE [Mapping].[PageToPageGroup] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
