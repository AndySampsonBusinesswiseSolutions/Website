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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[System].[PageGroupDetail]') AND type in (N'U'))
DROP TABLE [System].[PageGroupDetail]
GO
CREATE TABLE [System].[PageGroupDetail]
	(
	PageGroupDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	PageGroupId BIGINT NOT NULL,
	PageGroupAttributeId BIGINT NOT NULL,
	PageGroupDetailDescription VARCHAR(255) NOT NULL
	)  ON [System]
GO
ALTER TABLE [System].[PageGroupDetail] ADD CONSTRAINT
	PK_PageGroupDetail PRIMARY KEY CLUSTERED 
	(
	PageGroupDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [System]

GO
ALTER TABLE [System].[PageGroupDetail] ADD CONSTRAINT
	DF_PageGroupDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [System].[PageGroupDetail] ADD CONSTRAINT
	DF_PageGroupDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [System].[PageGroupDetail] ADD CONSTRAINT
	DF_PageGroupDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [System].[PageGroupDetail] ADD CONSTRAINT
	FK_PageGroupDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[PageGroupDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'PageGroupDetail', N'CONSTRAINT', N'FK_PageGroupDetail_CreatedByUserId'
GO
ALTER TABLE [System].[PageGroupDetail] ADD CONSTRAINT
	FK_PageGroupDetail_PageGroupId FOREIGN KEY
	(
	PageGroupId
	) REFERENCES [System].[PageGroup]
	(
	PageGroupId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[PageGroupDetail].PageGroupId to [System].[PageGroup].PageGroupId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'PageGroupDetail', N'CONSTRAINT', N'FK_PageGroupDetail_PageGroupId'
GO
ALTER TABLE [System].[PageGroupDetail] ADD CONSTRAINT
	FK_PageGroupDetail_PageGroupAttributeId FOREIGN KEY
	(
	PageGroupAttributeId
	) REFERENCES [System].[PageGroupAttribute]
	(
	PageGroupAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[PageGroupDetail].PageGroupAttributeId to [System].[PageGroupAttribute].PageGroupAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'PageGroupDetail', N'CONSTRAINT', N'FK_PageGroupDetail_PageGroupAttributeId'
GO
ALTER TABLE [System].[PageGroupDetail] ADD CONSTRAINT
	FK_PageGroupDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [System].[PageGroupDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'System', N'TABLE', N'PageGroupDetail', N'CONSTRAINT', N'FK_PageGroupDetail_SourceId'
GO
ALTER TABLE [System].[PageGroupDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
