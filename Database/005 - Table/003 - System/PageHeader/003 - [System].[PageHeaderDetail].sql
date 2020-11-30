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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[System].[PageHeaderDetail]') AND type in (N'U'))
DROP TABLE [System].[PageHeaderDetail]
GO
CREATE TABLE [System].[PageHeaderDetail]
	(
	PageHeaderDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	PageHeaderId BIGINT NOT NULL,
	PageHeaderAttributeId BIGINT NOT NULL,
	PageHeaderDetailDescription VARCHAR(255) NOT NULL
	)  ON [System]
GO
ALTER TABLE [System].[PageHeaderDetail] ADD CONSTRAINT
	PK_PageHeaderDetail PRIMARY KEY CLUSTERED 
	(
	PageHeaderDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [System]

GO
ALTER TABLE [System].[PageHeaderDetail] ADD CONSTRAINT
	DF_PageHeaderDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [System].[PageHeaderDetail] ADD CONSTRAINT
	DF_PageHeaderDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [System].[PageHeaderDetail] ADD CONSTRAINT
	DF_PageHeaderDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [System].[PageHeaderDetail] ADD CONSTRAINT
	FK_PageHeaderDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[PageHeaderDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'PageHeaderDetail', N'CONSTRAINT', N'FK_PageHeaderDetail_CreatedByUserId'
GO
ALTER TABLE [System].[PageHeaderDetail] ADD CONSTRAINT
	FK_PageHeaderDetail_PageHeaderId FOREIGN KEY
	(
	PageHeaderId
	) REFERENCES [System].[PageHeader]
	(
	PageHeaderId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[PageHeaderDetail].PageHeaderId to [System].[PageHeader].PageHeaderId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'PageHeaderDetail', N'CONSTRAINT', N'FK_PageHeaderDetail_PageHeaderId'
GO
ALTER TABLE [System].[PageHeaderDetail] ADD CONSTRAINT
	FK_PageHeaderDetail_PageHeaderAttributeId FOREIGN KEY
	(
	PageHeaderAttributeId
	) REFERENCES [System].[PageHeaderAttribute]
	(
	PageHeaderAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[PageHeaderDetail].PageHeaderAttributeId to [System].[PageHeaderAttribute].PageHeaderAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'PageHeaderDetail', N'CONSTRAINT', N'FK_PageHeaderDetail_PageHeaderAttributeId'
GO
ALTER TABLE [System].[PageHeaderDetail] ADD CONSTRAINT
	FK_PageHeaderDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[PageHeaderDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'PageHeaderDetail', N'CONSTRAINT', N'FK_PageHeaderDetail_SourceId'
GO
ALTER TABLE [System].[PageHeaderDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
